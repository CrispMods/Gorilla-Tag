using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000387 RID: 903
public class InteractionPoint : MonoBehaviour, ISpawnable, IBuildValidation
{
	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06001529 RID: 5417 RVA: 0x0003E548 File Offset: 0x0003C748
	// (set) Token: 0x0600152A RID: 5418 RVA: 0x0003E550 File Offset: 0x0003C750
	public bool ignoreLeftHand { get; private set; }

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x0600152B RID: 5419 RVA: 0x0003E559 File Offset: 0x0003C759
	// (set) Token: 0x0600152C RID: 5420 RVA: 0x0003E561 File Offset: 0x0003C761
	public bool ignoreRightHand { get; private set; }

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x0600152D RID: 5421 RVA: 0x0003E56A File Offset: 0x0003C76A
	public IHoldableObject Holdable
	{
		get
		{
			return this.parentHoldable;
		}
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x0600152E RID: 5422 RVA: 0x0003E572 File Offset: 0x0003C772
	// (set) Token: 0x0600152F RID: 5423 RVA: 0x0003E57A File Offset: 0x0003C77A
	public bool IsSpawned { get; set; }

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06001530 RID: 5424 RVA: 0x0003E583 File Offset: 0x0003C783
	// (set) Token: 0x06001531 RID: 5425 RVA: 0x0003E58B File Offset: 0x0003C78B
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x06001532 RID: 5426 RVA: 0x000BF14C File Offset: 0x000BD34C
	public void OnSpawn(VRRig rig)
	{
		this.interactor = EquipmentInteractor.instance;
		this.myCollider = base.GetComponent<Collider>();
		if (this.parentHoldableObject != null)
		{
			this.parentHoldable = this.parentHoldableObject.GetComponent<IHoldableObject>();
		}
		else
		{
			this.parentHoldable = base.GetComponentInParent<IHoldableObject>();
			this.parentHoldableObject = this.parentHoldable.gameObject;
		}
		if (this.parentHoldable == null)
		{
			if (this.parentHoldableObject == null)
			{
				Debug.LogError("InteractionPoint: Disabling because expected field `parentHoldableObject` is null. Path=" + base.transform.GetPathQ());
				base.enabled = false;
				return;
			}
			Debug.LogError("InteractionPoint: Disabling because `parentHoldableObject` does not have a IHoldableObject component. Path=" + base.transform.GetPathQ());
		}
		TransferrableObject transferrableObject = this.parentHoldable as TransferrableObject;
		this.forLocalPlayer = (transferrableObject == null || transferrableObject.IsLocalObject() || transferrableObject.isSceneObject || transferrableObject.canDrop);
	}

	// Token: 0x06001533 RID: 5427 RVA: 0x00030607 File Offset: 0x0002E807
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06001534 RID: 5428 RVA: 0x0003E594 File Offset: 0x0003C794
	private void Awake()
	{
		if (this.isNonSpawnedObject)
		{
			this.OnSpawn(null);
		}
	}

	// Token: 0x06001535 RID: 5429 RVA: 0x0003E5A5 File Offset: 0x0003C7A5
	private void OnEnable()
	{
		this.wasInLeft = false;
		this.wasInRight = false;
	}

	// Token: 0x06001536 RID: 5430 RVA: 0x0003E5B5 File Offset: 0x0003C7B5
	public void OnDisable()
	{
		if (!this.forLocalPlayer || this.interactor == null)
		{
			return;
		}
		this.interactor.InteractionPointDisabled(this);
	}

	// Token: 0x06001537 RID: 5431 RVA: 0x000BF22C File Offset: 0x000BD42C
	protected void LateUpdate()
	{
		if (!this.forLocalPlayer)
		{
			base.enabled = false;
			this.myCollider.enabled = false;
			return;
		}
		if (this.interactor == null)
		{
			this.interactor = EquipmentInteractor.instance;
			return;
		}
		if (this.interactionRadius > 0f || this.myCollider != null)
		{
			if (!this.ignoreLeftHand && this.OverlapCheck(this.interactor.leftHand.transform.position) != this.wasInLeft)
			{
				if (!this.wasInLeft && !this.interactor.overlapInteractionPointsLeft.Contains(this))
				{
					this.interactor.overlapInteractionPointsLeft.Add(this);
					this.wasInLeft = true;
				}
				else if (this.wasInLeft && this.interactor.overlapInteractionPointsLeft.Contains(this))
				{
					this.interactor.overlapInteractionPointsLeft.Remove(this);
					this.wasInLeft = false;
				}
			}
			if (!this.ignoreRightHand && this.OverlapCheck(this.interactor.rightHand.transform.position) != this.wasInRight)
			{
				if (!this.wasInRight && !this.interactor.overlapInteractionPointsRight.Contains(this))
				{
					this.interactor.overlapInteractionPointsRight.Add(this);
					this.wasInRight = true;
					return;
				}
				if (this.wasInRight && this.interactor.overlapInteractionPointsRight.Contains(this))
				{
					this.interactor.overlapInteractionPointsRight.Remove(this);
					this.wasInRight = false;
				}
			}
		}
	}

	// Token: 0x06001538 RID: 5432 RVA: 0x000BF3BC File Offset: 0x000BD5BC
	private bool OverlapCheck(Vector3 point)
	{
		if (this.interactionRadius > 0f)
		{
			return (base.transform.position - point).IsShorterThan(this.interactionRadius * base.transform.lossyScale);
		}
		return this.myCollider != null && this.myCollider.bounds.Contains(point);
	}

	// Token: 0x06001539 RID: 5433 RVA: 0x00039846 File Offset: 0x00037A46
	public bool BuildValidationCheck()
	{
		return true;
	}

	// Token: 0x0400176E RID: 5998
	[SerializeField]
	[FormerlySerializedAs("parentTransferrableObject")]
	private GameObject parentHoldableObject;

	// Token: 0x0400176F RID: 5999
	private IHoldableObject parentHoldable;

	// Token: 0x04001772 RID: 6002
	[SerializeField]
	private bool isNonSpawnedObject;

	// Token: 0x04001773 RID: 6003
	[SerializeField]
	private float interactionRadius;

	// Token: 0x04001774 RID: 6004
	public Collider myCollider;

	// Token: 0x04001775 RID: 6005
	public EquipmentInteractor interactor;

	// Token: 0x04001776 RID: 6006
	public bool wasInLeft;

	// Token: 0x04001777 RID: 6007
	public bool wasInRight;

	// Token: 0x04001778 RID: 6008
	public bool forLocalPlayer;
}
