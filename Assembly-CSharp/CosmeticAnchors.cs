using System;
using GorillaExtensions;
using GorillaNetworking;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x020003E5 RID: 997
public class CosmeticAnchors : MonoBehaviour, ISpawnable
{
	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x06001828 RID: 6184 RVA: 0x00040666 File Offset: 0x0003E866
	// (set) Token: 0x06001829 RID: 6185 RVA: 0x0004066E File Offset: 0x0003E86E
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x0600182A RID: 6186 RVA: 0x00040677 File Offset: 0x0003E877
	// (set) Token: 0x0600182B RID: 6187 RVA: 0x0004067F File Offset: 0x0003E87F
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x0600182C RID: 6188 RVA: 0x000CAA44 File Offset: 0x000C8C44
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

	// Token: 0x0600182D RID: 6189 RVA: 0x00030607 File Offset: 0x0002E807
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x0600182E RID: 6190 RVA: 0x000CAB48 File Offset: 0x000C8D48
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

	// Token: 0x0600182F RID: 6191 RVA: 0x00040688 File Offset: 0x0003E888
	private void OnEnable()
	{
		if (this.huntComputerAnchor || this.builderWatchAnchor)
		{
			CosmeticAnchorManager.RegisterCosmeticAnchor(this);
		}
	}

	// Token: 0x06001830 RID: 6192 RVA: 0x000406AA File Offset: 0x0003E8AA
	private void OnDisable()
	{
		if (this.huntComputerAnchor || this.builderWatchAnchor)
		{
			CosmeticAnchorManager.UnregisterCosmeticAnchor(this);
		}
	}

	// Token: 0x06001831 RID: 6193 RVA: 0x000CABC0 File Offset: 0x000C8DC0
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

	// Token: 0x06001832 RID: 6194 RVA: 0x000CAD54 File Offset: 0x000C8F54
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

	// Token: 0x06001833 RID: 6195 RVA: 0x000CAEA8 File Offset: 0x000C90A8
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

	// Token: 0x06001834 RID: 6196 RVA: 0x000CAF20 File Offset: 0x000C9120
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

	// Token: 0x06001835 RID: 6197 RVA: 0x000CAF98 File Offset: 0x000C9198
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

	// Token: 0x06001836 RID: 6198 RVA: 0x000CB00C File Offset: 0x000C920C
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

	// Token: 0x06001837 RID: 6199 RVA: 0x000406CC File Offset: 0x0003E8CC
	public Transform GetNameAnchor()
	{
		if (!this.nameAnchor)
		{
			return null;
		}
		return this.nameAnchor.transform;
	}

	// Token: 0x06001838 RID: 6200 RVA: 0x000406E8 File Offset: 0x0003E8E8
	public bool AffectedByHunt()
	{
		return this.huntComputerAnchor != null;
	}

	// Token: 0x06001839 RID: 6201 RVA: 0x000406F6 File Offset: 0x0003E8F6
	public bool AffectedByBuilder()
	{
		return this.builderWatchAnchor != null;
	}

	// Token: 0x04001AB5 RID: 6837
	[SerializeField]
	protected GameObject nameAnchor;

	// Token: 0x04001AB6 RID: 6838
	[Delayed]
	[SerializeField]
	protected string nameAnchor_path;

	// Token: 0x04001AB7 RID: 6839
	[SerializeField]
	protected GameObject leftArmAnchor;

	// Token: 0x04001AB8 RID: 6840
	[Delayed]
	[SerializeField]
	protected string leftArmAnchor_path;

	// Token: 0x04001AB9 RID: 6841
	[SerializeField]
	protected GameObject rightArmAnchor;

	// Token: 0x04001ABA RID: 6842
	[Delayed]
	[SerializeField]
	protected string rightArmAnchor_path;

	// Token: 0x04001ABB RID: 6843
	[SerializeField]
	protected GameObject chestAnchor;

	// Token: 0x04001ABC RID: 6844
	[Delayed]
	[SerializeField]
	protected string chestAnchor_path;

	// Token: 0x04001ABD RID: 6845
	[SerializeField]
	protected GameObject huntComputerAnchor;

	// Token: 0x04001ABE RID: 6846
	[Delayed]
	[SerializeField]
	protected string huntComputerAnchor_path;

	// Token: 0x04001ABF RID: 6847
	[SerializeField]
	protected GameObject builderWatchAnchor;

	// Token: 0x04001AC0 RID: 6848
	[Delayed]
	[SerializeField]
	protected string builderWatchAnchor_path;

	// Token: 0x04001AC1 RID: 6849
	[SerializeField]
	protected GameObject friendshipBraceletLeftOverride;

	// Token: 0x04001AC2 RID: 6850
	[Delayed]
	[SerializeField]
	protected string friendshipBraceletLeftOverride_path;

	// Token: 0x04001AC3 RID: 6851
	[SerializeField]
	protected GameObject friendshipBraceletRightOverride;

	// Token: 0x04001AC4 RID: 6852
	[Delayed]
	[SerializeField]
	protected string friendshipBraceletRightOverride_path;

	// Token: 0x04001AC5 RID: 6853
	[SerializeField]
	protected GameObject badgeAnchor;

	// Token: 0x04001AC6 RID: 6854
	[Delayed]
	[SerializeField]
	protected string badgeAnchor_path;

	// Token: 0x04001AC7 RID: 6855
	[SerializeField]
	public CosmeticsController.CosmeticSlots slot;

	// Token: 0x04001AC8 RID: 6856
	private VRRig vrRig;

	// Token: 0x04001AC9 RID: 6857
	private VRRigAnchorOverrides anchorOverrides;

	// Token: 0x04001ACA RID: 6858
	private bool anchorEnabled;

	// Token: 0x04001ACB RID: 6859
	private static GTLogErrorLimiter k_debugLogError_anchorOverridesNull = new GTLogErrorLimiter("The array `anchorOverrides` was null. Is the cosmetic getting initialized properly? ", 10, "\n- ");
}
