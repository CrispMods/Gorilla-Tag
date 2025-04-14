using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x020003D6 RID: 982
public class ChestObjectHysteresis : MonoBehaviour, ISpawnable
{
	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x060017B8 RID: 6072 RVA: 0x00073E6D File Offset: 0x0007206D
	// (set) Token: 0x060017B9 RID: 6073 RVA: 0x00073E75 File Offset: 0x00072075
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x060017BA RID: 6074 RVA: 0x00073E7E File Offset: 0x0007207E
	// (set) Token: 0x060017BB RID: 6075 RVA: 0x00073E86 File Offset: 0x00072086
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x060017BC RID: 6076 RVA: 0x00073E90 File Offset: 0x00072090
	void ISpawnable.OnSpawn(VRRig rig)
	{
		if (!this.angleFollower && (string.IsNullOrEmpty(this.angleFollower_path) || base.transform.TryFindByPath(this.angleFollower_path, out this.angleFollower, false)))
		{
			Debug.LogError(string.Concat(new string[]
			{
				"ChestObjectHysteresis: DEACTIVATING! Could not find `angleFollower` using path: \"",
				this.angleFollower_path,
				"\". For component at: \"",
				this.GetComponentPath(int.MaxValue),
				"\""
			}), this);
			base.gameObject.SetActive(false);
			return;
		}
	}

	// Token: 0x060017BD RID: 6077 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x060017BE RID: 6078 RVA: 0x00073F1E File Offset: 0x0007211E
	private void Start()
	{
		this.lastAngleQuat = base.transform.rotation;
		this.currentAngleQuat = base.transform.rotation;
	}

	// Token: 0x060017BF RID: 6079 RVA: 0x00073F42 File Offset: 0x00072142
	private void OnEnable()
	{
		ChestObjectHysteresisManager.RegisterCH(this);
	}

	// Token: 0x060017C0 RID: 6080 RVA: 0x00073F4A File Offset: 0x0007214A
	private void OnDisable()
	{
		ChestObjectHysteresisManager.UnregisterCH(this);
	}

	// Token: 0x060017C1 RID: 6081 RVA: 0x00073F54 File Offset: 0x00072154
	public void InvokeUpdate()
	{
		this.currentAngleQuat = this.angleFollower.rotation;
		this.angleBetween = Quaternion.Angle(this.currentAngleQuat, this.lastAngleQuat);
		if (this.angleBetween > this.angleHysteresis)
		{
			base.transform.rotation = Quaternion.Slerp(this.currentAngleQuat, this.lastAngleQuat, this.angleHysteresis / this.angleBetween);
			this.lastAngleQuat = base.transform.rotation;
		}
		base.transform.rotation = this.lastAngleQuat;
	}

	// Token: 0x04001A57 RID: 6743
	public float angleHysteresis;

	// Token: 0x04001A58 RID: 6744
	public float angleBetween;

	// Token: 0x04001A59 RID: 6745
	public Transform angleFollower;

	// Token: 0x04001A5A RID: 6746
	[Delayed]
	public string angleFollower_path;

	// Token: 0x04001A5B RID: 6747
	private Quaternion lastAngleQuat;

	// Token: 0x04001A5C RID: 6748
	private Quaternion currentAngleQuat;
}
