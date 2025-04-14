using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x020003D6 RID: 982
public class ChestObjectHysteresis : MonoBehaviour, ISpawnable
{
	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x060017B5 RID: 6069 RVA: 0x00073AE9 File Offset: 0x00071CE9
	// (set) Token: 0x060017B6 RID: 6070 RVA: 0x00073AF1 File Offset: 0x00071CF1
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x060017B7 RID: 6071 RVA: 0x00073AFA File Offset: 0x00071CFA
	// (set) Token: 0x060017B8 RID: 6072 RVA: 0x00073B02 File Offset: 0x00071D02
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x060017B9 RID: 6073 RVA: 0x00073B0C File Offset: 0x00071D0C
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

	// Token: 0x060017BA RID: 6074 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x060017BB RID: 6075 RVA: 0x00073B9A File Offset: 0x00071D9A
	private void Start()
	{
		this.lastAngleQuat = base.transform.rotation;
		this.currentAngleQuat = base.transform.rotation;
	}

	// Token: 0x060017BC RID: 6076 RVA: 0x00073BBE File Offset: 0x00071DBE
	private void OnEnable()
	{
		ChestObjectHysteresisManager.RegisterCH(this);
	}

	// Token: 0x060017BD RID: 6077 RVA: 0x00073BC6 File Offset: 0x00071DC6
	private void OnDisable()
	{
		ChestObjectHysteresisManager.UnregisterCH(this);
	}

	// Token: 0x060017BE RID: 6078 RVA: 0x00073BD0 File Offset: 0x00071DD0
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

	// Token: 0x04001A56 RID: 6742
	public float angleHysteresis;

	// Token: 0x04001A57 RID: 6743
	public float angleBetween;

	// Token: 0x04001A58 RID: 6744
	public Transform angleFollower;

	// Token: 0x04001A59 RID: 6745
	[Delayed]
	public string angleFollower_path;

	// Token: 0x04001A5A RID: 6746
	private Quaternion lastAngleQuat;

	// Token: 0x04001A5B RID: 6747
	private Quaternion currentAngleQuat;
}
