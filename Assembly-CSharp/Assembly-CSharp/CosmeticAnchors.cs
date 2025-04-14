using System;
using GorillaExtensions;
using GorillaNetworking;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x020003DA RID: 986
public class CosmeticAnchors : MonoBehaviour, ISpawnable
{
	// Token: 0x170002AB RID: 683
	// (get) Token: 0x060017DE RID: 6110 RVA: 0x00074394 File Offset: 0x00072594
	// (set) Token: 0x060017DF RID: 6111 RVA: 0x0007439C File Offset: 0x0007259C
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170002AC RID: 684
	// (get) Token: 0x060017E0 RID: 6112 RVA: 0x000743A5 File Offset: 0x000725A5
	// (set) Token: 0x060017E1 RID: 6113 RVA: 0x000743AD File Offset: 0x000725AD
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x060017E2 RID: 6114 RVA: 0x000743B8 File Offset: 0x000725B8
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.anchorEnabled = false;
		this.vrRig = rig;
		if (!this.vrRig)
		{
			Debug.LogError("CosmeticAnchors.OnSpawn: Disabling! Could not find VRRig in parent! Path: " + base.transform.GetPathQ(), this);
			base.enabled = false;
			return;
		}
		this.anchorOverrides = this.vrRig.gameObject.GetComponent<VRRigAnchorOverrides>();
		this.AssignAnchorToPath(ref this.nameAnchor, this.nameAnchor_path);
		this.AssignAnchorToPath(ref this.leftArmAnchor, this.leftArmAnchor_path);
		this.AssignAnchorToPath(ref this.rightArmAnchor, this.rightArmAnchor_path);
		this.AssignAnchorToPath(ref this.chestAnchor, this.chestAnchor_path);
		this.AssignAnchorToPath(ref this.huntComputerAnchor, this.huntComputerAnchor_path);
		this.AssignAnchorToPath(ref this.badgeAnchor, this.badgeAnchor_path);
		this.AssignAnchorToPath(ref this.builderWatchAnchor, this.builderWatchAnchor_path);
		this.AssignAnchorToPath(ref this.friendshipBraceletLeftOverride, this.friendshipBraceletLeftOverride_path);
		this.AssignAnchorToPath(ref this.friendshipBraceletRightOverride, this.friendshipBraceletRightOverride_path);
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x060017E4 RID: 6116 RVA: 0x000744BC File Offset: 0x000726BC
	private void AssignAnchorToPath(ref GameObject anchorGObjRef, string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return;
		}
		Transform transform;
		if (!base.transform.TryFindByPath(path, out transform, false))
		{
			this.vrRig = base.GetComponentInParent<VRRig>(true);
			if (this.vrRig && this.vrRig.isOfflineVRRig)
			{
				Debug.LogError("CosmeticAnchors: Could not find path: \"" + path + "\".\nPath to this component: " + base.transform.GetPathQ(), this);
			}
			return;
		}
		anchorGObjRef = transform.gameObject;
	}

	// Token: 0x060017E5 RID: 6117 RVA: 0x00074534 File Offset: 0x00072734
	private void OnEnable()
	{
		if (this.huntComputerAnchor || this.builderWatchAnchor)
		{
			CosmeticAnchorManager.RegisterCosmeticAnchor(this);
		}
	}

	// Token: 0x060017E6 RID: 6118 RVA: 0x00074556 File Offset: 0x00072756
	private void OnDisable()
	{
		if (this.huntComputerAnchor || this.builderWatchAnchor)
		{
			CosmeticAnchorManager.UnregisterCosmeticAnchor(this);
		}
	}

	// Token: 0x060017E7 RID: 6119 RVA: 0x00074578 File Offset: 0x00072778
	public void TryUpdate()
	{
		if (this.anchorEnabled && this.huntComputerAnchor && !GorillaTagger.Instance.offlineVRRig.huntComputer.activeSelf && this.anchorOverrides.HuntComputer.parent != this.anchorOverrides.HuntDefaultAnchor)
		{
			this.anchorOverrides.HuntComputer.parent = this.anchorOverrides.HuntDefaultAnchor;
			return;
		}
		if (this.anchorEnabled && this.huntComputerAnchor && GorillaTagger.Instance.offlineVRRig.huntComputer.activeSelf && this.anchorOverrides.HuntComputer.parent == this.anchorOverrides.HuntDefaultAnchor)
		{
			this.SetHuntComputerAnchor(this.anchorEnabled);
		}
		if (this.anchorEnabled && this.builderWatchAnchor && !GorillaTagger.Instance.offlineVRRig.builderResizeWatch.activeSelf && this.anchorOverrides.BuilderWatch.parent != this.anchorOverrides.BuilderWatchAnchor)
		{
			this.anchorOverrides.BuilderWatch.parent = this.anchorOverrides.BuilderWatchAnchor;
			return;
		}
		if (this.anchorEnabled && this.builderWatchAnchor && GorillaTagger.Instance.offlineVRRig.builderResizeWatch.activeSelf && this.anchorOverrides.BuilderWatch.parent == this.anchorOverrides.BuilderWatchAnchor)
		{
			this.SetBuilderWatchAnchor(this.anchorEnabled);
		}
	}

	// Token: 0x060017E8 RID: 6120 RVA: 0x0007470C File Offset: 0x0007290C
	public void EnableAnchor(bool enable)
	{
		this.anchorEnabled = enable;
		if (this.anchorOverrides == null)
		{
			return;
		}
		if (this.leftArmAnchor)
		{
			this.anchorOverrides.OverrideAnchor(TransferrableObject.PositionState.OnLeftArm, enable ? this.leftArmAnchor.transform : null);
		}
		if (this.rightArmAnchor)
		{
			this.anchorOverrides.OverrideAnchor(TransferrableObject.PositionState.OnRightArm, enable ? this.rightArmAnchor.transform : null);
		}
		if (this.chestAnchor)
		{
			this.anchorOverrides.OverrideAnchor(TransferrableObject.PositionState.OnChest, enable ? this.chestAnchor.transform : this.anchorOverrides.chestDefaultTransform);
		}
		this.anchorOverrides.UpdateNameAnchor(enable ? this.nameAnchor : null, this.slot);
		this.anchorOverrides.UpdateBadgeAnchor(enable ? this.badgeAnchor : null, this.slot);
		if (this.huntComputerAnchor)
		{
			this.SetHuntComputerAnchor(enable);
		}
		if (this.builderWatchAnchor)
		{
			this.SetBuilderWatchAnchor(enable);
		}
		this.SetCustomAnchor(this.anchorOverrides.friendshipBraceletLeftAnchor, enable, this.friendshipBraceletLeftOverride, this.anchorOverrides.friendshipBraceletLeftDefaultAnchor);
		this.SetCustomAnchor(this.anchorOverrides.friendshipBraceletRightAnchor, enable, this.friendshipBraceletRightOverride, this.anchorOverrides.friendshipBraceletRightDefaultAnchor);
	}

	// Token: 0x060017E9 RID: 6121 RVA: 0x00074860 File Offset: 0x00072A60
	private void SetHuntComputerAnchor(bool enable)
	{
		Transform huntComputer = this.anchorOverrides.HuntComputer;
		if (!GorillaTagger.Instance.offlineVRRig.huntComputer.activeSelf || !enable)
		{
			huntComputer.parent = this.anchorOverrides.HuntDefaultAnchor;
		}
		else
		{
			huntComputer.parent = this.huntComputerAnchor.transform;
		}
		huntComputer.transform.localPosition = Vector3.zero;
		huntComputer.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x060017EA RID: 6122 RVA: 0x000748D8 File Offset: 0x00072AD8
	private void SetBuilderWatchAnchor(bool enable)
	{
		Transform builderWatch = this.anchorOverrides.BuilderWatch;
		if (!GorillaTagger.Instance.offlineVRRig.builderResizeWatch.activeSelf || !enable)
		{
			builderWatch.parent = this.anchorOverrides.BuilderWatchAnchor;
		}
		else
		{
			builderWatch.parent = this.builderWatchAnchor.transform;
		}
		builderWatch.transform.localPosition = Vector3.zero;
		builderWatch.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x060017EB RID: 6123 RVA: 0x00074950 File Offset: 0x00072B50
	private void SetCustomAnchor(Transform target, bool enable, GameObject overrideAnchor, Transform defaultAnchor)
	{
		Transform transform = (enable && overrideAnchor != null) ? overrideAnchor.transform : defaultAnchor;
		if (target != null && target.parent != transform)
		{
			target.parent = transform;
			target.transform.localPosition = Vector3.zero;
			target.transform.localRotation = Quaternion.identity;
			target.transform.localScale = Vector3.one;
		}
	}

	// Token: 0x060017EC RID: 6124 RVA: 0x000749C4 File Offset: 0x00072BC4
	public Transform GetPositionAnchor(TransferrableObject.PositionState pos)
	{
		if (pos != TransferrableObject.PositionState.OnLeftArm)
		{
			if (pos != TransferrableObject.PositionState.OnRightArm)
			{
				if (pos != TransferrableObject.PositionState.OnChest)
				{
					return null;
				}
				if (!this.chestAnchor)
				{
					return null;
				}
				return this.chestAnchor.transform;
			}
			else
			{
				if (!this.rightArmAnchor)
				{
					return null;
				}
				return this.rightArmAnchor.transform;
			}
		}
		else
		{
			if (!this.leftArmAnchor)
			{
				return null;
			}
			return this.leftArmAnchor.transform;
		}
	}

	// Token: 0x060017ED RID: 6125 RVA: 0x00074A32 File Offset: 0x00072C32
	public Transform GetNameAnchor()
	{
		if (!this.nameAnchor)
		{
			return null;
		}
		return this.nameAnchor.transform;
	}

	// Token: 0x060017EE RID: 6126 RVA: 0x00074A4E File Offset: 0x00072C4E
	public bool AffectedByHunt()
	{
		return this.huntComputerAnchor != null;
	}

	// Token: 0x060017EF RID: 6127 RVA: 0x00074A5C File Offset: 0x00072C5C
	public bool AffectedByBuilder()
	{
		return this.builderWatchAnchor != null;
	}

	// Token: 0x04001A6D RID: 6765
	[SerializeField]
	protected GameObject nameAnchor;

	// Token: 0x04001A6E RID: 6766
	[Delayed]
	[SerializeField]
	protected string nameAnchor_path;

	// Token: 0x04001A6F RID: 6767
	[SerializeField]
	protected GameObject leftArmAnchor;

	// Token: 0x04001A70 RID: 6768
	[Delayed]
	[SerializeField]
	protected string leftArmAnchor_path;

	// Token: 0x04001A71 RID: 6769
	[SerializeField]
	protected GameObject rightArmAnchor;

	// Token: 0x04001A72 RID: 6770
	[Delayed]
	[SerializeField]
	protected string rightArmAnchor_path;

	// Token: 0x04001A73 RID: 6771
	[SerializeField]
	protected GameObject chestAnchor;

	// Token: 0x04001A74 RID: 6772
	[Delayed]
	[SerializeField]
	protected string chestAnchor_path;

	// Token: 0x04001A75 RID: 6773
	[SerializeField]
	protected GameObject huntComputerAnchor;

	// Token: 0x04001A76 RID: 6774
	[Delayed]
	[SerializeField]
	protected string huntComputerAnchor_path;

	// Token: 0x04001A77 RID: 6775
	[SerializeField]
	protected GameObject builderWatchAnchor;

	// Token: 0x04001A78 RID: 6776
	[Delayed]
	[SerializeField]
	protected string builderWatchAnchor_path;

	// Token: 0x04001A79 RID: 6777
	[SerializeField]
	protected GameObject friendshipBraceletLeftOverride;

	// Token: 0x04001A7A RID: 6778
	[Delayed]
	[SerializeField]
	protected string friendshipBraceletLeftOverride_path;

	// Token: 0x04001A7B RID: 6779
	[SerializeField]
	protected GameObject friendshipBraceletRightOverride;

	// Token: 0x04001A7C RID: 6780
	[Delayed]
	[SerializeField]
	protected string friendshipBraceletRightOverride_path;

	// Token: 0x04001A7D RID: 6781
	[SerializeField]
	protected GameObject badgeAnchor;

	// Token: 0x04001A7E RID: 6782
	[Delayed]
	[SerializeField]
	protected string badgeAnchor_path;

	// Token: 0x04001A7F RID: 6783
	[SerializeField]
	public CosmeticsController.CosmeticSlots slot;

	// Token: 0x04001A80 RID: 6784
	private VRRig vrRig;

	// Token: 0x04001A81 RID: 6785
	private VRRigAnchorOverrides anchorOverrides;

	// Token: 0x04001A82 RID: 6786
	private bool anchorEnabled;

	// Token: 0x04001A83 RID: 6787
	private static GTLogErrorLimiter k_debugLogError_anchorOverridesNull = new GTLogErrorLimiter("The array `anchorOverrides` was null. Is the cosmetic getting initialized properly? ", 10, "\n- ");
}
