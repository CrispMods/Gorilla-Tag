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
	// (get) Token: 0x060014DD RID: 5341 RVA: 0x000665F2 File Offset: 0x000647F2
	// (set) Token: 0x060014DE RID: 5342 RVA: 0x000665FA File Offset: 0x000647FA
	public bool ignoreLeftHand { get; private set; }

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x060014DF RID: 5343 RVA: 0x00066603 File Offset: 0x00064803
	// (set) Token: 0x060014E0 RID: 5344 RVA: 0x0006660B File Offset: 0x0006480B
	public bool ignoreRightHand { get; private set; }

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x060014E1 RID: 5345 RVA: 0x00066614 File Offset: 0x00064814
	public IHoldableObject Holdable
	{
		get
		{
			return this.parentHoldable;
		}
	}

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x060014E2 RID: 5346 RVA: 0x0006661C File Offset: 0x0006481C
	// (set) Token: 0x060014E3 RID: 5347 RVA: 0x00066624 File Offset: 0x00064824
	public bool IsSpawned { get; set; }

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x060014E4 RID: 5348 RVA: 0x0006662D File Offset: 0x0006482D
	// (set) Token: 0x060014E5 RID: 5349 RVA: 0x00066635 File Offset: 0x00064835
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x060014E6 RID: 5350 RVA: 0x00066640 File Offset: 0x00064840
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

	// Token: 0x060014E7 RID: 5351 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x060014E8 RID: 5352 RVA: 0x0006671E File Offset: 0x0006491E
	private void Awake()
	{
		if (this.isNonSpawnedObject)
		{
			this.OnSpawn(null);
		}
	}

	// Token: 0x060014E9 RID: 5353 RVA: 0x0006672F File Offset: 0x0006492F
	private void OnEnable()
	{
		this.wasInLeft = false;
		this.wasInRight = false;
	}

	// Token: 0x060014EA RID: 5354 RVA: 0x0006673F File Offset: 0x0006493F
	public void OnDisable()
	{
		if (!this.forLocalPlayer || this.interactor == null)
		{
			return;
		}
		this.interactor.InteractionPointDisabled(this);
	}

	// Token: 0x060014EB RID: 5355 RVA: 0x00066764 File Offset: 0x00064964
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

	// Token: 0x060014EC RID: 5356 RVA: 0x000668F4 File Offset: 0x00064AF4
	private bool OverlapCheck(Vector3 point)
	{
		if (this.interactionRadius > 0f)
		{
			return (base.transform.position - point).IsShorterThan(this.interactionRadius * base.transform.lossyScale);
		}
		return this.myCollider != null && this.myCollider.bounds.Contains(point);
	}

	// Token: 0x060014ED RID: 5357 RVA: 0x000444E2 File Offset: 0x000426E2
	public bool BuildValidationCheck()
	{
		return true;
	}

	// Token: 0x04001726 RID: 5926
	[SerializeField]
	[FormerlySerializedAs("parentTransferrableObject")]
	private GameObject parentHoldableObject;

	// Token: 0x04001727 RID: 5927
	private IHoldableObject parentHoldable;

	// Token: 0x0400172A RID: 5930
	[SerializeField]
	private bool isNonSpawnedObject;

	// Token: 0x0400172B RID: 5931
	[SerializeField]
	private float interactionRadius;

	// Token: 0x0400172C RID: 5932
	public Collider myCollider;

	// Token: 0x0400172D RID: 5933
	public EquipmentInteractor interactor;

	// Token: 0x0400172E RID: 5934
	public bool wasInLeft;

	// Token: 0x0400172F RID: 5935
	public bool wasInRight;

	// Token: 0x04001730 RID: 5936
	public bool forLocalPlayer;
}
