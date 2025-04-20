using System;
using GorillaExtensions;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000419 RID: 1049
public class VRRigAnchorOverrides : MonoBehaviour
{
	// Token: 0x170002CF RID: 719
	// (get) Token: 0x060019F9 RID: 6649 RVA: 0x00041814 File Offset: 0x0003FA14
	// (set) Token: 0x060019FA RID: 6650 RVA: 0x000D4028 File Offset: 0x000D2228
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

	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x060019FB RID: 6651 RVA: 0x0004181C File Offset: 0x0003FA1C
	public Transform HuntDefaultAnchor
	{
		get
		{
			return this.huntComputerDefaultAnchor;
		}
	}

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x060019FC RID: 6652 RVA: 0x00041824 File Offset: 0x0003FA24
	public Transform HuntComputer
	{
		get
		{
			return this.huntComputer;
		}
	}

	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x060019FD RID: 6653 RVA: 0x0004182C File Offset: 0x0003FA2C
	public Transform BuilderWatchAnchor
	{
		get
		{
			return this.builderResizeButtonDefaultAnchor;
		}
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x060019FE RID: 6654 RVA: 0x00041834 File Offset: 0x0003FA34
	public Transform BuilderWatch
	{
		get
		{
			return this.builderResizeButton;
		}
	}

	// Token: 0x060019FF RID: 6655 RVA: 0x000D4078 File Offset: 0x000D2278
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

	// Token: 0x06001A00 RID: 6656 RVA: 0x000D40CC File Offset: 0x000D22CC
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

	// Token: 0x06001A01 RID: 6657 RVA: 0x000D41E8 File Offset: 0x000D23E8
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

	// Token: 0x06001A02 RID: 6658 RVA: 0x000D4208 File Offset: 0x000D2408
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

	// Token: 0x06001A03 RID: 6659 RVA: 0x000D4284 File Offset: 0x000D2484
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

	// Token: 0x06001A04 RID: 6660 RVA: 0x000D42A8 File Offset: 0x000D24A8
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

	// Token: 0x06001A05 RID: 6661 RVA: 0x000D4300 File Offset: 0x000D2500
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

	// Token: 0x06001A06 RID: 6662 RVA: 0x0004183C File Offset: 0x0003FA3C
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

	// Token: 0x06001A07 RID: 6663 RVA: 0x000D43BC File Offset: 0x000D25BC
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

	// Token: 0x06001A08 RID: 6664 RVA: 0x0004187A File Offset: 0x0003FA7A
	private void ResetBadge()
	{
		if (!this.currentBadgeTransform)
		{
			return;
		}
		this.currentBadgeTransform.localRotation = this.badgeDefaultRot;
		this.currentBadgeTransform.localPosition = this.badgeDefaultPos;
	}

	// Token: 0x04001CDC RID: 7388
	[SerializeField]
	internal Transform nameDefaultAnchor;

	// Token: 0x04001CDD RID: 7389
	[SerializeField]
	internal Transform nameTransform;

	// Token: 0x04001CDE RID: 7390
	[SerializeField]
	internal Transform chestDefaultTransform;

	// Token: 0x04001CDF RID: 7391
	[SerializeField]
	internal Transform huntComputer;

	// Token: 0x04001CE0 RID: 7392
	[SerializeField]
	internal Transform huntComputerDefaultAnchor;

	// Token: 0x04001CE1 RID: 7393
	private Transform huntDefaultTransform;

	// Token: 0x04001CE2 RID: 7394
	[SerializeField]
	protected Transform builderResizeButton;

	// Token: 0x04001CE3 RID: 7395
	[SerializeField]
	protected Transform builderResizeButtonDefaultAnchor;

	// Token: 0x04001CE4 RID: 7396
	private Transform builderResizeButtonDefaultTransform;

	// Token: 0x04001CE5 RID: 7397
	private readonly Transform[] overrideAnchors = new Transform[8];

	// Token: 0x04001CE6 RID: 7398
	private GameObject nameLastObjectToAttach;

	// Token: 0x04001CE7 RID: 7399
	private Transform currentBadgeTransform;

	// Token: 0x04001CE8 RID: 7400
	private Vector3 badgeDefaultPos;

	// Token: 0x04001CE9 RID: 7401
	private Quaternion badgeDefaultRot;

	// Token: 0x04001CEA RID: 7402
	private GameObject[] badgeAnchors = new GameObject[3];

	// Token: 0x04001CEB RID: 7403
	private GameObject[] nameAnchors = new GameObject[4];

	// Token: 0x04001CEC RID: 7404
	[SerializeField]
	public Transform friendshipBraceletLeftDefaultAnchor;

	// Token: 0x04001CED RID: 7405
	public Transform friendshipBraceletLeftAnchor;

	// Token: 0x04001CEE RID: 7406
	[SerializeField]
	public Transform friendshipBraceletRightDefaultAnchor;

	// Token: 0x04001CEF RID: 7407
	public Transform friendshipBraceletRightAnchor;
}
