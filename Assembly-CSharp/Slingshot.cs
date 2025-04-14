using System;
using GorillaLocomotion;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000383 RID: 899
public class Slingshot : ProjectileWeapon
{
	// Token: 0x0600150D RID: 5389 RVA: 0x00066FA0 File Offset: 0x000651A0
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

	// Token: 0x0600150E RID: 5390 RVA: 0x00067004 File Offset: 0x00065204
	protected override void Awake()
	{
		base.Awake();
		this._elasticIntialWidthMultiplier = this.elasticLeft.widthMultiplier;
	}

	// Token: 0x0600150F RID: 5391 RVA: 0x0006701D File Offset: 0x0006521D
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.myRig = rig;
	}

	// Token: 0x06001510 RID: 5392 RVA: 0x00067030 File Offset: 0x00065230
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

	// Token: 0x06001511 RID: 5393 RVA: 0x000670A9 File Offset: 0x000652A9
	internal override void OnDisable()
	{
		this.DestroyDummyProjectile();
		base.OnDisable();
	}

	// Token: 0x06001512 RID: 5394 RVA: 0x000670B8 File Offset: 0x000652B8
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

	// Token: 0x06001513 RID: 5395 RVA: 0x000672FD File Offset: 0x000654FD
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

	// Token: 0x06001514 RID: 5396 RVA: 0x0006733A File Offset: 0x0006553A
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

	// Token: 0x06001515 RID: 5397 RVA: 0x00067375 File Offset: 0x00065575
	public static bool IsSlingShotEnabled()
	{
		return !(GorillaTagger.Instance == null) && !(GorillaTagger.Instance.offlineVRRig == null) && GorillaTagger.Instance.offlineVRRig.cosmeticSet.HasItemOfCategory(CosmeticsController.CosmeticCategory.Chest);
	}

	// Token: 0x06001516 RID: 5398 RVA: 0x000673B0 File Offset: 0x000655B0
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

	// Token: 0x06001517 RID: 5399 RVA: 0x0006748C File Offset: 0x0006568C
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

	// Token: 0x06001518 RID: 5400 RVA: 0x0006756D File Offset: 0x0006576D
	public override void DropItemCleanup()
	{
		base.DropItemCleanup();
		this.currentState = TransferrableObject.PositionState.OnChest;
		this.itemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x06001519 RID: 5401 RVA: 0x000444E2 File Offset: 0x000426E2
	public override bool AutoGrabTrue(bool leftGrabbingHand)
	{
		return true;
	}

	// Token: 0x0600151A RID: 5402 RVA: 0x00067584 File Offset: 0x00065784
	private bool ForLeftHandSlingshot()
	{
		return this.itemState == TransferrableObject.ItemStates.State2 || this.currentState == TransferrableObject.PositionState.InLeftHand;
	}

	// Token: 0x0600151B RID: 5403 RVA: 0x0006759A File Offset: 0x0006579A
	private bool InDrawingState()
	{
		return this.itemState == TransferrableObject.ItemStates.State2 || this.itemState == TransferrableObject.ItemStates.State3;
	}

	// Token: 0x0600151C RID: 5404 RVA: 0x000675B0 File Offset: 0x000657B0
	protected override Vector3 GetLaunchPosition()
	{
		return this.dummyProjectile.transform.position;
	}

	// Token: 0x0600151D RID: 5405 RVA: 0x000675C4 File Offset: 0x000657C4
	protected override Vector3 GetLaunchVelocity()
	{
		float d = Mathf.Abs(base.transform.lossyScale.x);
		Vector3 a = this.centerOrigin.position - this.center.position;
		a /= d;
		Vector3 a2 = Mathf.Min(this.springConstant * this.maxDraw, a.magnitude * this.springConstant) * a.normalized * d;
		Vector3 averagedVelocity = GTPlayer.Instance.AveragedVelocity;
		return a2 + averagedVelocity;
	}

	// Token: 0x04001754 RID: 5972
	[FormerlySerializedAs("elastic")]
	public LineRenderer elasticLeft;

	// Token: 0x04001755 RID: 5973
	public LineRenderer elasticRight;

	// Token: 0x04001756 RID: 5974
	public Transform leftArm;

	// Token: 0x04001757 RID: 5975
	public Transform rightArm;

	// Token: 0x04001758 RID: 5976
	public Transform center;

	// Token: 0x04001759 RID: 5977
	public Transform centerOrigin;

	// Token: 0x0400175A RID: 5978
	private GameObject dummyProjectile;

	// Token: 0x0400175B RID: 5979
	public GameObject drawingHand;

	// Token: 0x0400175C RID: 5980
	public InteractionPoint nock;

	// Token: 0x0400175D RID: 5981
	public InteractionPoint grip;

	// Token: 0x0400175E RID: 5982
	public float springConstant;

	// Token: 0x0400175F RID: 5983
	public float maxDraw;

	// Token: 0x04001760 RID: 5984
	private Transform leftHandSnap;

	// Token: 0x04001761 RID: 5985
	private Transform rightHandSnap;

	// Token: 0x04001762 RID: 5986
	public bool disableWhenNotInRoom;

	// Token: 0x04001763 RID: 5987
	private bool hasDummyProjectile;

	// Token: 0x04001764 RID: 5988
	private float delayLaunchTime = 0.07f;

	// Token: 0x04001765 RID: 5989
	private float minTimeToLaunch = -1f;

	// Token: 0x04001766 RID: 5990
	private float dummyProjectileColliderRadius;

	// Token: 0x04001767 RID: 5991
	private float dummyProjectileInitialScale;

	// Token: 0x04001768 RID: 5992
	private int projectileCount;

	// Token: 0x04001769 RID: 5993
	private Vector3[] elasticLeftPoints = new Vector3[2];

	// Token: 0x0400176A RID: 5994
	private Vector3[] elasticRightPoints = new Vector3[2];

	// Token: 0x0400176B RID: 5995
	private float _elasticIntialWidthMultiplier;

	// Token: 0x0400176C RID: 5996
	private new VRRig myRig;

	// Token: 0x02000384 RID: 900
	public enum SlingshotState
	{
		// Token: 0x0400176E RID: 5998
		NoState = 1,
		// Token: 0x0400176F RID: 5999
		OnChest,
		// Token: 0x04001770 RID: 6000
		LeftHandDrawing = 4,
		// Token: 0x04001771 RID: 6001
		RightHandDrawing = 8
	}

	// Token: 0x02000385 RID: 901
	public enum SlingshotActions
	{
		// Token: 0x04001773 RID: 6003
		Grab,
		// Token: 0x04001774 RID: 6004
		Release
	}
}
