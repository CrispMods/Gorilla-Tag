using System;
using GorillaExtensions;
using GorillaNetworking;
using UnityEngine;

// Token: 0x0200040E RID: 1038
public class VRRigAnchorOverrides : MonoBehaviour
{
	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x060019AF RID: 6575 RVA: 0x0007EBC1 File Offset: 0x0007CDC1
	// (set) Token: 0x060019B0 RID: 6576 RVA: 0x0007EBCC File Offset: 0x0007CDCC
	public Transform CurrentBadgeTransform
	{
		get
		{
			return this.currentBadgeTransform;
		}
		set
		{
			if (value != this.currentBadgeTransform)
			{
				this.ResetBadge();
				this.currentBadgeTransform = value;
				this.badgeDefaultRot = this.currentBadgeTransform.localRotation;
				this.badgeDefaultPos = this.currentBadgeTransform.localPosition;
				this.UpdateBadge();
			}
		}
	}

	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x060019B1 RID: 6577 RVA: 0x0007EC1C File Offset: 0x0007CE1C
	public Transform HuntDefaultAnchor
	{
		get
		{
			return this.huntComputerDefaultAnchor;
		}
	}

	// Token: 0x170002CA RID: 714
	// (get) Token: 0x060019B2 RID: 6578 RVA: 0x0007EC24 File Offset: 0x0007CE24
	public Transform HuntComputer
	{
		get
		{
			return this.huntComputer;
		}
	}

	// Token: 0x170002CB RID: 715
	// (get) Token: 0x060019B3 RID: 6579 RVA: 0x0007EC2C File Offset: 0x0007CE2C
	public Transform BuilderWatchAnchor
	{
		get
		{
			return this.builderResizeButtonDefaultAnchor;
		}
	}

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x060019B4 RID: 6580 RVA: 0x0007EC34 File Offset: 0x0007CE34
	public Transform BuilderWatch
	{
		get
		{
			return this.builderResizeButton;
		}
	}

	// Token: 0x060019B5 RID: 6581 RVA: 0x0007EC3C File Offset: 0x0007CE3C
	private void Awake()
	{
		for (int i = 0; i < 8; i++)
		{
			this.overrideAnchors[i] = null;
		}
		int num = this.MapPositionToIndex(TransferrableObject.PositionState.OnChest);
		this.overrideAnchors[num] = this.chestDefaultTransform;
		this.huntDefaultTransform = this.huntComputer;
		this.builderResizeButtonDefaultTransform = this.builderResizeButton;
	}

	// Token: 0x060019B6 RID: 6582 RVA: 0x0007EC90 File Offset: 0x0007CE90
	private void OnEnable()
	{
		if (this.nameDefaultAnchor && this.nameDefaultAnchor.parent)
		{
			this.nameTransform.parent = this.nameDefaultAnchor.parent;
		}
		else
		{
			Debug.LogError("VRRigAnchorOverrides: could not set parent `nameTransform` because `nameDefaultAnchor` or its parent was null!" + base.transform.GetPathQ(), this);
		}
		this.huntComputer = this.huntDefaultTransform;
		if (this.huntComputerDefaultAnchor && this.huntComputerDefaultAnchor.parent)
		{
			this.huntComputer.parent = this.huntComputerDefaultAnchor.parent;
		}
		else
		{
			Debug.LogError("VRRigAnchorOverrides: could not set parent `huntComputer` because `huntComputerDefaultAnchor` or its parent was null!" + base.transform.GetPathQ(), this);
		}
		this.builderResizeButton = this.builderResizeButtonDefaultTransform;
		if (this.builderResizeButtonDefaultAnchor && this.builderResizeButtonDefaultAnchor.parent)
		{
			this.builderResizeButton.parent = this.builderResizeButtonDefaultAnchor.parent;
			return;
		}
		Debug.LogError("VRRigAnchorOverrides: could not set parent `builderResizeButton` because `builderResizeButtonDefaultAnchor` or its parent was null! Path: " + base.transform.GetPathQ(), this);
	}

	// Token: 0x060019B7 RID: 6583 RVA: 0x0007EDAC File Offset: 0x0007CFAC
	private int MapPositionToIndex(TransferrableObject.PositionState pos)
	{
		int num = (int)pos;
		int num2 = 0;
		while ((num >>= 1) != 0)
		{
			num2++;
		}
		return num2;
	}

	// Token: 0x060019B8 RID: 6584 RVA: 0x0007EDCC File Offset: 0x0007CFCC
	public void OverrideAnchor(TransferrableObject.PositionState pos, Transform anchor)
	{
		int num = this.MapPositionToIndex(pos);
		if (this.overrideAnchors[num])
		{
			foreach (object obj in this.overrideAnchors[num])
			{
				((Transform)obj).parent = null;
			}
		}
		this.overrideAnchors[num] = anchor;
	}

	// Token: 0x060019B9 RID: 6585 RVA: 0x0007EE48 File Offset: 0x0007D048
	public Transform AnchorOverride(TransferrableObject.PositionState pos, Transform fallback)
	{
		int num = this.MapPositionToIndex(pos);
		Transform transform = this.overrideAnchors[num];
		if (transform != null)
		{
			return transform;
		}
		return fallback;
	}

	// Token: 0x060019BA RID: 6586 RVA: 0x0007EE6C File Offset: 0x0007D06C
	public void UpdateNameAnchor(GameObject nameAnchor, CosmeticsController.CosmeticSlots slot)
	{
		if (slot != CosmeticsController.CosmeticSlots.Badge)
		{
			switch (slot)
			{
			case CosmeticsController.CosmeticSlots.Shirt:
				this.nameAnchors[0] = nameAnchor;
				break;
			case CosmeticsController.CosmeticSlots.Pants:
				this.nameAnchors[1] = nameAnchor;
				break;
			case CosmeticsController.CosmeticSlots.Back:
				this.nameAnchors[2] = nameAnchor;
				break;
			}
		}
		else
		{
			this.nameAnchors[3] = nameAnchor;
		}
		this.UpdateName();
	}

	// Token: 0x060019BB RID: 6587 RVA: 0x0007EEC4 File Offset: 0x0007D0C4
	private void UpdateName()
	{
		foreach (GameObject gameObject in this.nameAnchors)
		{
			if (gameObject)
			{
				this.nameTransform.parent = gameObject.transform;
				this.nameTransform.localRotation = Quaternion.identity;
				this.nameTransform.localPosition = Vector3.zero;
				return;
			}
		}
		if (this.nameDefaultAnchor)
		{
			this.nameTransform.parent = this.nameDefaultAnchor;
			this.nameTransform.localRotation = Quaternion.identity;
			this.nameTransform.localPosition = Vector3.zero;
			return;
		}
		Debug.LogError("VRRigAnchorOverrides: could not set parent for `nameTransform` because `nameDefaultAnchor` or its parent was null! Path: " + base.transform.GetPathQ(), this);
	}

	// Token: 0x060019BC RID: 6588 RVA: 0x0007EF7E File Offset: 0x0007D17E
	public void UpdateBadgeAnchor(GameObject badgeAnchor, CosmeticsController.CosmeticSlots slot)
	{
		switch (slot)
		{
		case CosmeticsController.CosmeticSlots.Shirt:
			this.badgeAnchors[0] = badgeAnchor;
			break;
		case CosmeticsController.CosmeticSlots.Pants:
			this.badgeAnchors[1] = badgeAnchor;
			break;
		case CosmeticsController.CosmeticSlots.Back:
			this.badgeAnchors[2] = badgeAnchor;
			break;
		}
		this.UpdateBadge();
	}

	// Token: 0x060019BD RID: 6589 RVA: 0x0007EFBC File Offset: 0x0007D1BC
	private void UpdateBadge()
	{
		if (!this.currentBadgeTransform)
		{
			return;
		}
		foreach (GameObject gameObject in this.badgeAnchors)
		{
			if (gameObject)
			{
				this.currentBadgeTransform.localRotation = gameObject.transform.localRotation;
				this.currentBadgeTransform.localPosition = gameObject.transform.localPosition;
				return;
			}
		}
		this.ResetBadge();
	}

	// Token: 0x060019BE RID: 6590 RVA: 0x0007F02B File Offset: 0x0007D22B
	private void ResetBadge()
	{
		if (!this.currentBadgeTransform)
		{
			return;
		}
		this.currentBadgeTransform.localRotation = this.badgeDefaultRot;
		this.currentBadgeTransform.localPosition = this.badgeDefaultPos;
	}

	// Token: 0x04001C94 RID: 7316
	[SerializeField]
	internal Transform nameDefaultAnchor;

	// Token: 0x04001C95 RID: 7317
	[SerializeField]
	internal Transform nameTransform;

	// Token: 0x04001C96 RID: 7318
	[SerializeField]
	internal Transform chestDefaultTransform;

	// Token: 0x04001C97 RID: 7319
	[SerializeField]
	internal Transform huntComputer;

	// Token: 0x04001C98 RID: 7320
	[SerializeField]
	internal Transform huntComputerDefaultAnchor;

	// Token: 0x04001C99 RID: 7321
	private Transform huntDefaultTransform;

	// Token: 0x04001C9A RID: 7322
	[SerializeField]
	protected Transform builderResizeButton;

	// Token: 0x04001C9B RID: 7323
	[SerializeField]
	protected Transform builderResizeButtonDefaultAnchor;

	// Token: 0x04001C9C RID: 7324
	private Transform builderResizeButtonDefaultTransform;

	// Token: 0x04001C9D RID: 7325
	private readonly Transform[] overrideAnchors = new Transform[8];

	// Token: 0x04001C9E RID: 7326
	private GameObject nameLastObjectToAttach;

	// Token: 0x04001C9F RID: 7327
	private Transform currentBadgeTransform;

	// Token: 0x04001CA0 RID: 7328
	private Vector3 badgeDefaultPos;

	// Token: 0x04001CA1 RID: 7329
	private Quaternion badgeDefaultRot;

	// Token: 0x04001CA2 RID: 7330
	private GameObject[] badgeAnchors = new GameObject[3];

	// Token: 0x04001CA3 RID: 7331
	private GameObject[] nameAnchors = new GameObject[4];

	// Token: 0x04001CA4 RID: 7332
	[SerializeField]
	public Transform friendshipBraceletLeftDefaultAnchor;

	// Token: 0x04001CA5 RID: 7333
	public Transform friendshipBraceletLeftAnchor;

	// Token: 0x04001CA6 RID: 7334
	[SerializeField]
	public Transform friendshipBraceletRightDefaultAnchor;

	// Token: 0x04001CA7 RID: 7335
	public Transform friendshipBraceletRightAnchor;
}
