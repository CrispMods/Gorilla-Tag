using System;
using UnityEngine;

// Token: 0x020002E1 RID: 737
public class TeleportPoint : MonoBehaviour
{
	// Token: 0x060011BF RID: 4543 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void Start()
	{
	}

	// Token: 0x060011C0 RID: 4544 RVA: 0x0003B1B6 File Offset: 0x000393B6
	public Transform GetDestTransform()
	{
		return this.destTransform;
	}

	// Token: 0x060011C1 RID: 4545 RVA: 0x000AC9FC File Offset: 0x000AABFC
	private void Update()
	{
		float value = Mathf.SmoothStep(this.fullIntensity, this.lowIntensity, (Time.time - this.lastLookAtTime) * this.dimmingSpeed);
		base.GetComponent<MeshRenderer>().material.SetFloat("_Intensity", value);
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x0003B1BE File Offset: 0x000393BE
	public void OnLookAt()
	{
		this.lastLookAtTime = Time.time;
	}

	// Token: 0x040013A7 RID: 5031
	public float dimmingSpeed = 1f;

	// Token: 0x040013A8 RID: 5032
	public float fullIntensity = 1f;

	// Token: 0x040013A9 RID: 5033
	public float lowIntensity = 0.5f;

	// Token: 0x040013AA RID: 5034
	public Transform destTransform;

	// Token: 0x040013AB RID: 5035
	private float lastLookAtTime;
}
