using System;
using GorillaTag;
using UnityEngine;

// Token: 0x020000DD RID: 221
public class WizardStaffHoldable : TransferrableObject
{
	// Token: 0x060005B1 RID: 1457 RVA: 0x000342EF File Offset: 0x000324EF
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.tipTargetLocalPosition = this.tipTransform.localPosition;
		this.hasEffectsGameObject = (this.effectsGameObject != null);
		this.effectsHaveBeenPlayed = false;
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x00034322 File Offset: 0x00032522
	internal override void OnEnable()
	{
		base.OnEnable();
		this.InitToDefault();
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x00034330 File Offset: 0x00032530
	public override void ResetToDefaultState()
	{
		base.ResetToDefaultState();
		this.InitToDefault();
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x0003433E File Offset: 0x0003253E
	private void InitToDefault()
	{
		this.cooldownRemaining = 0f;
		if (this.hasEffectsGameObject && this.effectsHaveBeenPlayed)
		{
			this.effectsGameObject.SetActive(false);
		}
		this.effectsHaveBeenPlayed = false;
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x00083110 File Offset: 0x00081310
	protected override void LateUpdateLocal()
	{
		base.LateUpdateLocal();
		if (!base.InHand() || this.itemState == TransferrableObject.ItemStates.State1 || !GorillaParent.hasInstance || !this.hitLastFrame)
		{
			return;
		}
		if (this.velocityEstimator.linearVelocity.magnitude < this.minSlamVelocity)
		{
			return;
		}
		Vector3 up = this.tipTransform.up;
		Vector3 up2 = Vector3.up;
		if (Vector3.Angle(up, up2) > this.minSlamAngle)
		{
			return;
		}
		this.itemState = TransferrableObject.ItemStates.State1;
		this.cooldownRemaining = this.cooldown;
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x00083194 File Offset: 0x00081394
	protected override void LateUpdateShared()
	{
		base.LateUpdateShared();
		this.cooldownRemaining -= Time.deltaTime;
		if (this.cooldownRemaining <= 0f)
		{
			this.itemState = TransferrableObject.ItemStates.State0;
			if (this.hasEffectsGameObject)
			{
				this.effectsGameObject.SetActive(false);
			}
			this.effectsHaveBeenPlayed = false;
		}
		if (base.InHand())
		{
			Vector3 position = base.transform.position;
			Vector3 end = base.transform.TransformPoint(this.tipTargetLocalPosition);
			RaycastHit raycastHit;
			if (Physics.Linecast(position, end, out raycastHit, this.tipCollisionLayerMask))
			{
				this.tipTransform.position = raycastHit.point;
				this.hitLastFrame = true;
			}
			else
			{
				this.tipTransform.localPosition = this.tipTargetLocalPosition;
				this.hitLastFrame = false;
			}
			if (this.itemState == TransferrableObject.ItemStates.State1 && this.hasEffectsGameObject && !this.effectsHaveBeenPlayed)
			{
				this.effectsGameObject.SetActive(true);
				this.effectsHaveBeenPlayed = true;
			}
		}
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x0003436E File Offset: 0x0003256E
	protected override void LateUpdateReplicated()
	{
		base.LateUpdateReplicated();
		if (this.itemState == TransferrableObject.ItemStates.State1 && !this.effectsHaveBeenPlayed)
		{
			this.cooldownRemaining = this.cooldown;
		}
	}

	// Token: 0x0400069A RID: 1690
	[Tooltip("This GameObject will activate when the staff hits the ground with enough force.")]
	public GameObject effectsGameObject;

	// Token: 0x0400069B RID: 1691
	[Tooltip("The Transform of the staff's tip which will be used to determine if the staff is being slammed. Up axis (Y) should point along the length of the staff.")]
	public Transform tipTransform;

	// Token: 0x0400069C RID: 1692
	public float tipCollisionRadius = 0.05f;

	// Token: 0x0400069D RID: 1693
	public LayerMask tipCollisionLayerMask;

	// Token: 0x0400069E RID: 1694
	[Tooltip("Used to calculate velocity of the staff.")]
	public GorillaVelocityEstimator velocityEstimator;

	// Token: 0x0400069F RID: 1695
	public float cooldown = 5f;

	// Token: 0x040006A0 RID: 1696
	[Tooltip("The velocity of the staff's tip must be greater than this value to activate the effect.")]
	public float minSlamVelocity = 0.5f;

	// Token: 0x040006A1 RID: 1697
	[Tooltip("The angle (in degrees) between the staff's tip and the ground must be less than this value to activate the effect.")]
	public float minSlamAngle = 5f;

	// Token: 0x040006A2 RID: 1698
	[DebugReadout]
	private float cooldownRemaining;

	// Token: 0x040006A3 RID: 1699
	[DebugReadout]
	private bool hitLastFrame;

	// Token: 0x040006A4 RID: 1700
	private Vector3 tipTargetLocalPosition;

	// Token: 0x040006A5 RID: 1701
	private bool hasEffectsGameObject;

	// Token: 0x040006A6 RID: 1702
	private bool effectsHaveBeenPlayed;
}
