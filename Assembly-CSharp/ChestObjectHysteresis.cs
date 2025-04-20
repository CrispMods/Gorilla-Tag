using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x020003E1 RID: 993
public class ChestObjectHysteresis : MonoBehaviour, ISpawnable
{
	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x06001802 RID: 6146 RVA: 0x0004043B File Offset: 0x0003E63B
	// (set) Token: 0x06001803 RID: 6147 RVA: 0x00040443 File Offset: 0x0003E643
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06001804 RID: 6148 RVA: 0x0004044C File Offset: 0x0003E64C
	// (set) Token: 0x06001805 RID: 6149 RVA: 0x00040454 File Offset: 0x0003E654
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06001806 RID: 6150 RVA: 0x000CA748 File Offset: 0x000C8948
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

	// Token: 0x06001807 RID: 6151 RVA: 0x00030607 File Offset: 0x0002E807
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06001808 RID: 6152 RVA: 0x0004045D File Offset: 0x0003E65D
	private void Start()
	{
		this.lastAngleQuat = base.transform.rotation;
		this.currentAngleQuat = base.transform.rotation;
	}

	// Token: 0x06001809 RID: 6153 RVA: 0x00040481 File Offset: 0x0003E681
	private void OnEnable()
	{
		ChestObjectHysteresisManager.RegisterCH(this);
	}

	// Token: 0x0600180A RID: 6154 RVA: 0x00040489 File Offset: 0x0003E689
	private void OnDisable()
	{
		ChestObjectHysteresisManager.UnregisterCH(this);
	}

	// Token: 0x0600180B RID: 6155 RVA: 0x000CA7D8 File Offset: 0x000C89D8
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

	// Token: 0x04001A9F RID: 6815
	public float angleHysteresis;

	// Token: 0x04001AA0 RID: 6816
	public float angleBetween;

	// Token: 0x04001AA1 RID: 6817
	public Transform angleFollower;

	// Token: 0x04001AA2 RID: 6818
	[Delayed]
	public string angleFollower_path;

	// Token: 0x04001AA3 RID: 6819
	private Quaternion lastAngleQuat;

	// Token: 0x04001AA4 RID: 6820
	private Quaternion currentAngleQuat;
}
