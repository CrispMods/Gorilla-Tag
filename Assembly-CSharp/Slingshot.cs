using System;
using GorillaLocomotion;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200038E RID: 910
public class Slingshot : ProjectileWeapon
{
	// Token: 0x06001559 RID: 5465 RVA: 0x000BF9C0 File Offset: 0x000BDBC0
	private void DestroyDummyProjectile()
	{
		if (this.hasDummyProjectile)
		{
			this.dummyProjectile.transform.localScale = Vector3.one * this.dummyProjectileInitialScale;
			this.dummyProjectile.GetComponent<SphereCollider>().enabled = true;
			ObjectPools.instance.Destroy(this.dummyProjectile);
			this.dummyProjectile = null;
			this.hasDummyProjectile = false;
		}
	}

	// Token: 0x0600155A RID: 5466 RVA: 0x0003E684 File Offset: 0x0003C884
	protected override void Awake()
	{
		base.Awake();
		this._elasticIntialWidthMultiplier = this.elasticLeft.widthMultiplier;
	}

	// Token: 0x0600155B RID: 5467 RVA: 0x0003E69D File Offset: 0x0003C89D
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.myRig = rig;
	}

	// Token: 0x0600155C RID: 5468 RVA: 0x000BFA24 File Offset: 0x000BDC24
	internal override void OnEnable()
	{
		this.leftHandSnap = this.myRig.cosmeticReferences.Get(CosmeticRefID.SlingshotSnapLeft).transform;
		this.rightHandSnap = this.myRig.cosmeticReferences.Get(CosmeticRefID.SlingshotSnapRight).transform;
		this.currentState = TransferrableObject.PositionState.OnChest;
		this.itemState = TransferrableObject.ItemStates.State0;
		this.elasticLeft.positionCount = 2;
		this.elasticRight.positionCount = 2;
		this.dummyProjectile = null;
		base.OnEnable();
	}

	// Token: 0x0600155D RID: 5469 RVA: 0x0003E6AD File Offset: 0x0003C8AD
	internal override void OnDisable()
	{
		this.DestroyDummyProjectile();
		base.OnDisable();
	}

	// Token: 0x0600155E RID: 5470 RVA: 0x000BFAA0 File Offset: 0x000BDCA0
	protected override void LateUpdateShared()
	{
		base.LateUpdateShared();
		float num = Mathf.Abs(base.transform.lossyScale.x);
		Vector3 vector;
		if (this.InDrawingState())
		{
			if (!this.hasDummyProjectile)
			{
				this.dummyProjectile = ObjectPools.instance.Instantiate(this.projectilePrefab);
				this.hasDummyProjectile = true;
				SphereCollider component = this.dummyProjectile.GetComponent<SphereCollider>();
				component.enabled = false;
				this.dummyProjectileColliderRadius = component.radius;
				this.dummyProjectileInitialScale = this.dummyProjectile.transform.localScale.x;
				bool blueTeam;
				bool orangeTeam;
				base.GetIsOnTeams(out blueTeam, out orangeTeam);
				this.dummyProjectile.GetComponent<SlingshotProjectile>().ApplyTeamModelAndColor(blueTeam, orangeTeam, false, default(Color));
			}
			float d = this.dummyProjectileInitialScale * num;
			this.dummyProjectile.transform.localScale = Vector3.one * d;
			Vector3 position = this.drawingHand.transform.position;
			Vector3 position2 = this.centerOrigin.position;
			Vector3 normalized = (position2 - position).normalized;
			float d2 = (EquipmentInteractor.instance.grabRadius - this.dummyProjectileColliderRadius) * num;
			vector = position + normalized * d2;
			this.dummyProjectile.transform.position = vector;
			this.dummyProjectile.transform.rotation = Quaternion.LookRotation(position2 - vector, Vector3.up);
		}
		else
		{
			this.DestroyDummyProjectile();
			vector = this.centerOrigin.position;
		}
		this.center.position = vector;
		this.elasticLeftPoints[0] = this.leftArm.position;
		this.elasticLeftPoints[1] = (this.elasticRightPoints[1] = vector);
		this.elasticRightPoints[0] = this.rightArm.position;
		this.elasticLeft.SetPositions(this.elasticLeftPoints);
		this.elasticRight.SetPositions(this.elasticRightPoints);
		this.elasticLeft.widthMultiplier = this._elasticIntialWidthMultiplier * num;
		this.elasticRight.widthMultiplier = this._elasticIntialWidthMultiplier * num;
		if (!NetworkSystem.Instance.InRoom && this.disableWhenNotInRoom)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600155F RID: 5471 RVA: 0x0003E6BB File Offset: 0x0003C8BB
	protected override void LateUpdateLocal()
	{
		base.LateUpdateLocal();
		if (this.InDrawingState())
		{
			if (this.ForLeftHandSlingshot())
			{
				this.drawingHand = EquipmentInteractor.instance.rightHand;
				return;
			}
			this.drawingHand = EquipmentInteractor.instance.leftHand;
		}
	}

	// Token: 0x06001560 RID: 5472 RVA: 0x0003E6F8 File Offset: 0x0003C8F8
	protected override void LateUpdateReplicated()
	{
		base.LateUpdateReplicated();
		if (this.InDrawingState())
		{
			if (this.ForLeftHandSlingshot())
			{
				this.drawingHand = this.rightHandSnap.gameObject;
				return;
			}
			this.drawingHand = this.leftHandSnap.gameObject;
		}
	}

	// Token: 0x06001561 RID: 5473 RVA: 0x0003E733 File Offset: 0x0003C933
	public static bool IsSlingShotEnabled()
	{
		return !(GorillaTagger.Instance == null) && !(GorillaTagger.Instance.offlineVRRig == null) && GorillaTagger.Instance.offlineVRRig.cosmeticSet.HasItemOfCategory(CosmeticsController.CosmeticCategory.Chest);
	}

	// Token: 0x06001562 RID: 5474 RVA: 0x000BFCE8 File Offset: 0x000BDEE8
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		if (!this.IsMyItem())
		{
			return;
		}
		bool flag = pointGrabbed == this.nock;
		if (flag && !base.InHand())
		{
			return;
		}
		base.OnGrab(pointGrabbed, grabbingHand);
		if (this.InDrawingState() || base.OnChest())
		{
			return;
		}
		if (flag)
		{
			if (grabbingHand == EquipmentInteractor.instance.leftHand)
			{
				EquipmentInteractor.instance.disableLeftGrab = true;
			}
			else
			{
				EquipmentInteractor.instance.disableRightGrab = true;
			}
			if (this.ForLeftHandSlingshot())
			{
				this.itemState = TransferrableObject.ItemStates.State2;
			}
			else
			{
				this.itemState = TransferrableObject.ItemStates.State3;
			}
			this.minTimeToLaunch = Time.time + this.delayLaunchTime;
			GorillaTagger.Instance.StartVibration(!this.ForLeftHandSlingshot(), GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration * 1.5f);
		}
	}

	// Token: 0x06001563 RID: 5475 RVA: 0x000BFDC4 File Offset: 0x000BDFC4
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		base.OnRelease(zoneReleased, releasingHand);
		if (this.InDrawingState() && releasingHand == this.drawingHand)
		{
			if (releasingHand == EquipmentInteractor.instance.leftHand)
			{
				EquipmentInteractor.instance.disableLeftGrab = false;
			}
			else
			{
				EquipmentInteractor.instance.disableRightGrab = false;
			}
			if (this.ForLeftHandSlingshot())
			{
				this.currentState = TransferrableObject.PositionState.InLeftHand;
			}
			else
			{
				this.currentState = TransferrableObject.PositionState.InRightHand;
			}
			this.itemState = TransferrableObject.ItemStates.State0;
			GorillaTagger.Instance.StartVibration(this.ForLeftHandSlingshot(), GorillaTagger.Instance.tapHapticStrength * 2f, GorillaTagger.Instance.tapHapticDuration * 1.5f);
			if (Time.time > this.minTimeToLaunch)
			{
				base.LaunchProjectile();
			}
		}
		else
		{
			EquipmentInteractor.instance.disableLeftGrab = false;
			EquipmentInteractor.instance.disableRightGrab = false;
		}
		return true;
	}

	// Token: 0x06001564 RID: 5476 RVA: 0x0003E76B File Offset: 0x0003C96B
	public override void DropItemCleanup()
	{
		base.DropItemCleanup();
		this.currentState = TransferrableObject.PositionState.OnChest;
		this.itemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x06001565 RID: 5477 RVA: 0x00039846 File Offset: 0x00037A46
	public override bool AutoGrabTrue(bool leftGrabbingHand)
	{
		return true;
	}

	// Token: 0x06001566 RID: 5478 RVA: 0x0003E782 File Offset: 0x0003C982
	private bool ForLeftHandSlingshot()
	{
		return this.itemState == TransferrableObject.ItemStates.State2 || this.currentState == TransferrableObject.PositionState.InLeftHand;
	}

	// Token: 0x06001567 RID: 5479 RVA: 0x0003E798 File Offset: 0x0003C998
	private bool InDrawingState()
	{
		return this.itemState == TransferrableObject.ItemStates.State2 || this.itemState == TransferrableObject.ItemStates.State3;
	}

	// Token: 0x06001568 RID: 5480 RVA: 0x0003E7AE File Offset: 0x0003C9AE
	protected override Vector3 GetLaunchPosition()
	{
		return this.dummyProjectile.transform.position;
	}

	// Token: 0x06001569 RID: 5481 RVA: 0x000BFEA8 File Offset: 0x000BE0A8
	protected override Vector3 GetLaunchVelocity()
	{
		float d = Mathf.Abs(base.transform.lossyScale.x);
		Vector3 a = this.centerOrigin.position - this.center.position;
		a /= d;
		Vector3 a2 = Mathf.Min(this.springConstant * this.maxDraw, a.magnitude * this.springConstant) * a.normalized * d;
		Vector3 averagedVelocity = GTPlayer.Instance.AveragedVelocity;
		return a2 + averagedVelocity;
	}

	// Token: 0x0400179C RID: 6044
	[FormerlySerializedAs("elastic")]
	public LineRenderer elasticLeft;

	// Token: 0x0400179D RID: 6045
	public LineRenderer elasticRight;

	// Token: 0x0400179E RID: 6046
	public Transform leftArm;

	// Token: 0x0400179F RID: 6047
	public Transform rightArm;

	// Token: 0x040017A0 RID: 6048
	public Transform center;

	// Token: 0x040017A1 RID: 6049
	public Transform centerOrigin;

	// Token: 0x040017A2 RID: 6050
	private GameObject dummyProjectile;

	// Token: 0x040017A3 RID: 6051
	public GameObject drawingHand;

	// Token: 0x040017A4 RID: 6052
	public InteractionPoint nock;

	// Token: 0x040017A5 RID: 6053
	public InteractionPoint grip;

	// Token: 0x040017A6 RID: 6054
	public float springConstant;

	// Token: 0x040017A7 RID: 6055
	public float maxDraw;

	// Token: 0x040017A8 RID: 6056
	private Transform leftHandSnap;

	// Token: 0x040017A9 RID: 6057
	private Transform rightHandSnap;

	// Token: 0x040017AA RID: 6058
	public bool disableWhenNotInRoom;

	// Token: 0x040017AB RID: 6059
	private bool hasDummyProjectile;

	// Token: 0x040017AC RID: 6060
	private float delayLaunchTime = 0.07f;

	// Token: 0x040017AD RID: 6061
	private float minTimeToLaunch = -1f;

	// Token: 0x040017AE RID: 6062
	private float dummyProjectileColliderRadius;

	// Token: 0x040017AF RID: 6063
	private float dummyProjectileInitialScale;

	// Token: 0x040017B0 RID: 6064
	private int projectileCount;

	// Token: 0x040017B1 RID: 6065
	private Vector3[] elasticLeftPoints = new Vector3[2];

	// Token: 0x040017B2 RID: 6066
	private Vector3[] elasticRightPoints = new Vector3[2];

	// Token: 0x040017B3 RID: 6067
	private float _elasticIntialWidthMultiplier;

	// Token: 0x040017B4 RID: 6068
	private new VRRig myRig;

	// Token: 0x0200038F RID: 911
	public enum SlingshotState
	{
		// Token: 0x040017B6 RID: 6070
		NoState = 1,
		// Token: 0x040017B7 RID: 6071
		OnChest,
		// Token: 0x040017B8 RID: 6072
		LeftHandDrawing = 4,
		// Token: 0x040017B9 RID: 6073
		RightHandDrawing = 8
	}

	// Token: 0x02000390 RID: 912
	public enum SlingshotActions
	{
		// Token: 0x040017BB RID: 6075
		Grab,
		// Token: 0x040017BC RID: 6076
		Release
	}
}
