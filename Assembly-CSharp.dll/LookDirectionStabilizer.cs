using System;
using Cinemachine.Utility;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x0200016E RID: 366
public class LookDirectionStabilizer : MonoBehaviour, ISpawnable
{
	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000924 RID: 2340 RVA: 0x0003577F File Offset: 0x0003397F
	// (set) Token: 0x06000925 RID: 2341 RVA: 0x00035787 File Offset: 0x00033987
	public bool IsSpawned { get; set; }

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000926 RID: 2342 RVA: 0x00035790 File Offset: 0x00033990
	// (set) Token: 0x06000927 RID: 2343 RVA: 0x00035798 File Offset: 0x00033998
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x06000928 RID: 2344 RVA: 0x000357A1 File Offset: 0x000339A1
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x0002F75F File Offset: 0x0002D95F
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x0008F56C File Offset: 0x0008D76C
	private void Update()
	{
		Transform rigTarget = this.myRig.head.rigTarget;
		if (rigTarget.forward.y < 0f)
		{
			Quaternion b = Quaternion.LookRotation(rigTarget.up.ProjectOntoPlane(Vector3.up));
			Quaternion rotation = base.transform.parent.rotation;
			float value = Vector3.Dot(rigTarget.up, Vector3.up);
			base.transform.rotation = Quaternion.Lerp(rotation, b, Mathf.InverseLerp(1f, 0.7f, value));
			return;
		}
		base.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x04000B2A RID: 2858
	private VRRig myRig;
}
