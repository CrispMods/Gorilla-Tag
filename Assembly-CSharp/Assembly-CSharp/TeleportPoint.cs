using System;
using UnityEngine;

// Token: 0x020002E1 RID: 737
public class TeleportPoint : MonoBehaviour
{
	// Token: 0x060011BF RID: 4543 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Start()
	{
	}

	// Token: 0x060011C0 RID: 4544 RVA: 0x000548FB File Offset: 0x00052AFB
	public Transform GetDestTransform()
	{
		return this.destTransform;
	}

	// Token: 0x060011C1 RID: 4545 RVA: 0x00054904 File Offset: 0x00052B04
	private void Update()
	{
		float value = Mathf.SmoothStep(this.fullIntensity, this.lowIntensity, (Time.time - this.lastLookAtTime) * this.dimmingSpeed);
		base.GetComponent<MeshRenderer>().material.SetFloat("_Intensity", value);
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x0005494C File Offset: 0x00052B4C
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
