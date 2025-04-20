using System;
using Cinemachine.Utility;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000179 RID: 377
public class LookDirectionStabilizer : MonoBehaviour, ISpawnable
{
	// Token: 0x170000EB RID: 235
	// (get) Token: 0x0600096F RID: 2415 RVA: 0x00036A4A File Offset: 0x00034C4A
	// (set) Token: 0x06000970 RID: 2416 RVA: 0x00036A52 File Offset: 0x00034C52
	public bool IsSpawned { get; set; }

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x06000971 RID: 2417 RVA: 0x00036A5B File Offset: 0x00034C5B
	// (set) Token: 0x06000972 RID: 2418 RVA: 0x00036A63 File Offset: 0x00034C63
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x06000973 RID: 2419 RVA: 0x00036A6C File Offset: 0x00034C6C
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x00030607 File Offset: 0x0002E807
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x00091EF4 File Offset: 0x000900F4
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

	// Token: 0x04000B70 RID: 2928
	private VRRig myRig;
}
