using System;
using UnityEngine;

// Token: 0x020002EC RID: 748
public class TeleportPoint : MonoBehaviour
{
	// Token: 0x06001208 RID: 4616 RVA: 0x00030607 File Offset: 0x0002E807
	private void Start()
	{
	}

	// Token: 0x06001209 RID: 4617 RVA: 0x0003C476 File Offset: 0x0003A676
	public Transform GetDestTransform()
	{
		return this.destTransform;
	}

	// Token: 0x0600120A RID: 4618 RVA: 0x000AF294 File Offset: 0x000AD494
	private void Update()
	{
		float value = Mathf.SmoothStep(this.fullIntensity, this.lowIntensity, (Time.time - this.lastLookAtTime) * this.dimmingSpeed);
		base.GetComponent<MeshRenderer>().material.SetFloat("_Intensity", value);
	}

	// Token: 0x0600120B RID: 4619 RVA: 0x0003C47E File Offset: 0x0003A67E
	public void OnLookAt()
	{
		this.lastLookAtTime = Time.time;
	}

	// Token: 0x040013EE RID: 5102
	public float dimmingSpeed = 1f;

	// Token: 0x040013EF RID: 5103
	public float fullIntensity = 1f;

	// Token: 0x040013F0 RID: 5104
	public float lowIntensity = 0.5f;

	// Token: 0x040013F1 RID: 5105
	public Transform destTransform;

	// Token: 0x040013F2 RID: 5106
	private float lastLookAtTime;
}
