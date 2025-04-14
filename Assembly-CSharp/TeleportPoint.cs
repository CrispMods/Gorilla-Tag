using System;
using UnityEngine;

// Token: 0x020002E1 RID: 737
public class TeleportPoint : MonoBehaviour
{
	// Token: 0x060011BC RID: 4540 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Start()
	{
	}

	// Token: 0x060011BD RID: 4541 RVA: 0x00054577 File Offset: 0x00052777
	public Transform GetDestTransform()
	{
		return this.destTransform;
	}

	// Token: 0x060011BE RID: 4542 RVA: 0x00054580 File Offset: 0x00052780
	private void Update()
	{
		float value = Mathf.SmoothStep(this.fullIntensity, this.lowIntensity, (Time.time - this.lastLookAtTime) * this.dimmingSpeed);
		base.GetComponent<MeshRenderer>().material.SetFloat("_Intensity", value);
	}

	// Token: 0x060011BF RID: 4543 RVA: 0x000545C8 File Offset: 0x000527C8
	public void OnLookAt()
	{
		this.lastLookAtTime = Time.time;
	}

	// Token: 0x040013A6 RID: 5030
	public float dimmingSpeed = 1f;

	// Token: 0x040013A7 RID: 5031
	public float fullIntensity = 1f;

	// Token: 0x040013A8 RID: 5032
	public float lowIntensity = 0.5f;

	// Token: 0x040013A9 RID: 5033
	public Transform destTransform;

	// Token: 0x040013AA RID: 5034
	private float lastLookAtTime;
}
