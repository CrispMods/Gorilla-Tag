using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200037C RID: 892
public class InteractionPoint : MonoBehaviour, ISpawnable, IBuildValidation
{
	// Token: 0x17000250 RID: 592
	// (get) Token: 0x060014E0 RID: 5344 RVA: 0x00066976 File Offset: 0x00064B76
	// (set) Token: 0x060014E1 RID: 5345 RVA: 0x0006697E File Offset: 0x00064B7E
	public bool ignoreLeftHand { get; private set; }

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x060014E2 RID: 5346 RVA: 0x00066987 File Offset: 0x00064B87
	// (set) Token: 0x060014E3 RID: 5347 RVA: 0x0006698F File Offset: 0x00064B8F
	public bool ignoreRightHand { get; private set; }

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x060014E4 RID: 5348 RVA: 0x00066998 File Offset: 0x00064B98
	public IHoldableObject Holdable
	{
		get
		{
			return this.parentHoldable;
		}
	}

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x060014E5 RID: 5349 RVA: 0x000669A0 File Offset: 0x00064BA0
	// (set) Token: 0x060014E6 RID: 5350 RVA: 0x000669A8 File Offset: 0x00064BA8
	public bool IsSpawned { get; set; }

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x060014E7 RID: 5351 RVA: 0x000669B1 File Offset: 0x00064BB1
	// (set) Token: 0x060014E8 RID: 5352 RVA: 0x000669B9 File Offset: 0x00064BB9
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x060014E9 RID: 5353 RVA: 0x000669C4 File Offset: 0x00064BC4
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

	// Token: 0x060014EA RID: 5354 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x060014EB RID: 5355 RVA: 0x00066AA2 File Offset: 0x00064CA2
	private void Awake()
	{
		if (this.isNonSpawnedObject)
		{
			this.OnSpawn(null);
		}
	}

	// Token: 0x060014EC RID: 5356 RVA: 0x00066AB3 File Offset: 0x00064CB3
	private void OnEnable()
	{
		this.wasInLeft = false;
		this.wasInRight = false;
	}

	// Token: 0x060014ED RID: 5357 RVA: 0x00066AC3 File Offset: 0x00064CC3
	public void OnDisable()
	{
		if (!this.forLocalPlayer || this.interactor == null)
		{
			return;
		}
		this.interactor.InteractionPointDisabled(this);
	}

	// Token: 0x060014EE RID: 5358 RVA: 0x00066AE8 File Offset: 0x00064CE8
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

	// Token: 0x060014EF RID: 5359 RVA: 0x00066C78 File Offset: 0x00064E78
	private bool OverlapCheck(Vector3 point)
	{
		if (this.interactionRadius > 0f)
		{
			return (base.transform.position - point).IsShorterThan(this.interactionRadius * base.transform.lossyScale);
		}
		return this.myCollider != null && this.myCollider.bounds.Contains(point);
	}

	// Token: 0x060014F0 RID: 5360 RVA: 0x00044826 File Offset: 0x00042A26
	public bool BuildValidationCheck()
	{
		return true;
	}

	// Token: 0x04001727 RID: 5927
	[SerializeField]
	[FormerlySerializedAs("parentTransferrableObject")]
	private GameObject parentHoldableObject;

	// Token: 0x04001728 RID: 5928
	private IHoldableObject parentHoldable;

	// Token: 0x0400172B RID: 5931
	[SerializeField]
	private bool isNonSpawnedObject;

	// Token: 0x0400172C RID: 5932
	[SerializeField]
	private float interactionRadius;

	// Token: 0x0400172D RID: 5933
	public Collider myCollider;

	// Token: 0x0400172E RID: 5934
	public EquipmentInteractor interactor;

	// Token: 0x0400172F RID: 5935
	public bool wasInLeft;

	// Token: 0x04001730 RID: 5936
	public bool wasInRight;

	// Token: 0x04001731 RID: 5937
	public bool forLocalPlayer;
}
