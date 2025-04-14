using System;
using Cinemachine.Utility;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x0200016E RID: 366
public class LookDirectionStabilizer : MonoBehaviour, ISpawnable
{
	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000924 RID: 2340 RVA: 0x000319F2 File Offset: 0x0002FBF2
	// (set) Token: 0x06000925 RID: 2341 RVA: 0x000319FA File Offset: 0x0002FBFA
	public bool IsSpawned { get; set; }

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000926 RID: 2342 RVA: 0x00031A03 File Offset: 0x0002FC03
	// (set) Token: 0x06000927 RID: 2343 RVA: 0x00031A0B File Offset: 0x0002FC0B
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x06000928 RID: 2344 RVA: 0x00031A14 File Offset: 0x0002FC14
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x00031A20 File Offset: 0x0002FC20
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
