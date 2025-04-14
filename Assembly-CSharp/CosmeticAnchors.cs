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
	// (get) Token: 0x060017DB RID: 6107 RVA: 0x00074010 File Offset: 0x00072210
	// (set) Token: 0x060017DC RID: 6108 RVA: 0x00074018 File Offset: 0x00072218
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170002AC RID: 684
	// (get) Token: 0x060017DD RID: 6109 RVA: 0x00074021 File Offset: 0x00072221
	// (set) Token: 0x060017DE RID: 6110 RVA: 0x00074029 File Offset: 0x00072229
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x060017DF RID: 6111 RVA: 0x00074034 File Offset: 0x00072234
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

	// Token: 0x060017E0 RID: 6112 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x060017E1 RID: 6113 RVA: 0x00074138 File Offset: 0x00072338
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

	// Token: 0x060017E2 RID: 6114 RVA: 0x000741B0 File Offset: 0x000723B0
	private void OnEnable()
	{
		if (this.huntComputerAnchor || this.builderWatchAnchor)
		{
			CosmeticAnchorManager.RegisterCosmeticAnchor(this);
		}
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x000741D2 File Offset: 0x000723D2
	private void OnDisable()
	{
		if (this.huntComputerAnchor || this.builderWatchAnchor)
		{
			CosmeticAnchorManager.UnregisterCosmeticAnchor(this);
		}
	}

	// Token: 0x060017E4 RID: 6116 RVA: 0x000741F4 File Offset: 0x000723F4
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

	// Token: 0x060017E5 RID: 6117 RVA: 0x00074388 File Offset: 0x00072588
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

	// Token: 0x060017E6 RID: 6118 RVA: 0x000744DC File Offset: 0x000726DC
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

	// Token: 0x060017E7 RID: 6119 RVA: 0x00074554 File Offset: 0x00072754
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

	// Token: 0x060017E8 RID: 6120 RVA: 0x000745CC File Offset: 0x000727CC
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

	// Token: 0x060017E9 RID: 6121 RVA: 0x00074640 File Offset: 0x00072840
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

	// Token: 0x060017EA RID: 6122 RVA: 0x000746AE File Offset: 0x000728AE
	public Transform GetNameAnchor()
	{
		if (!this.nameAnchor)
		{
			return null;
		}
		return this.nameAnchor.transform;
	}

	// Token: 0x060017EB RID: 6123 RVA: 0x000746CA File Offset: 0x000728CA
	public bool AffectedByHunt()
	{
		return this.huntComputerAnchor != null;
	}

	// Token: 0x060017EC RID: 6124 RVA: 0x000746D8 File Offset: 0x000728D8
	public bool AffectedByBuilder()
	{
		return this.builderWatchAnchor != null;
	}

	// Token: 0x04001A6C RID: 6764
	[SerializeField]
	protected GameObject nameAnchor;

	// Token: 0x04001A6D RID: 6765
	[Delayed]
	[SerializeField]
	protected string nameAnchor_path;

	// Token: 0x04001A6E RID: 6766
	[SerializeField]
	protected GameObject leftArmAnchor;

	// Token: 0x04001A6F RID: 6767
	[Delayed]
	[SerializeField]
	protected string leftArmAnchor_path;

	// Token: 0x04001A70 RID: 6768
	[SerializeField]
	protected GameObject rightArmAnchor;

	// Token: 0x04001A71 RID: 6769
	[Delayed]
	[SerializeField]
	protected string rightArmAnchor_path;

	// Token: 0x04001A72 RID: 6770
	[SerializeField]
	protected GameObject chestAnchor;

	// Token: 0x04001A73 RID: 6771
	[Delayed]
	[SerializeField]
	protected string chestAnchor_path;

	// Token: 0x04001A74 RID: 6772
	[SerializeField]
	protected GameObject huntComputerAnchor;

	// Token: 0x04001A75 RID: 6773
	[Delayed]
	[SerializeField]
	protected string huntComputerAnchor_path;

	// Token: 0x04001A76 RID: 6774
	[SerializeField]
	protected GameObject builderWatchAnchor;

	// Token: 0x04001A77 RID: 6775
	[Delayed]
	[SerializeField]
	protected string builderWatchAnchor_path;

	// Token: 0x04001A78 RID: 6776
	[SerializeField]
	protected GameObject friendshipBraceletLeftOverride;

	// Token: 0x04001A79 RID: 6777
	[Delayed]
	[SerializeField]
	protected string friendshipBraceletLeftOverride_path;

	// Token: 0x04001A7A RID: 6778
	[SerializeField]
	protected GameObject friendshipBraceletRightOverride;

	// Token: 0x04001A7B RID: 6779
	[Delayed]
	[SerializeField]
	protected string friendshipBraceletRightOverride_path;

	// Token: 0x04001A7C RID: 6780
	[SerializeField]
	protected GameObject badgeAnchor;

	// Token: 0x04001A7D RID: 6781
	[Delayed]
	[SerializeField]
	protected string badgeAnchor_path;

	// Token: 0x04001A7E RID: 6782
	[SerializeField]
	public CosmeticsController.CosmeticSlots slot;

	// Token: 0x04001A7F RID: 6783
	private VRRig vrRig;

	// Token: 0x04001A80 RID: 6784
	private VRRigAnchorOverrides anchorOverrides;

	// Token: 0x04001A81 RID: 6785
	private bool anchorEnabled;

	// Token: 0x04001A82 RID: 6786
	private static GTLogErrorLimiter k_debugLogError_anchorOverridesNull = new GTLogErrorLimiter("The array `anchorOverrides` was null. Is the cosmetic getting initialized properly? ", 10, "\n- ");
}
