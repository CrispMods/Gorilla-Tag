using System;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class Monkeye_LazerFX : MonoBehaviour
{
	// Token: 0x06000421 RID: 1057 RVA: 0x00018E9E File Offset: 0x0001709E
	private void Awake()
	{
		base.enabled = false;
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x00018EA8 File Offset: 0x000170A8
	public void EnableLazer(Transform[] eyes_, VRRig rig_)
	{
		if (rig_ == this.rig)
		{
			return;
		}
		this.eyeBones = eyes_;
		this.rig = rig_;
		base.enabled = true;
		LineRenderer[] array = this.lines;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].positionCount = 2;
		}
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x00018EF8 File Offset: 0x000170F8
	public void DisableLazer()
	{
		if (base.enabled)
		{
			base.enabled = false;
			LineRenderer[] array = this.lines;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].positionCount = 0;
			}
		}
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x00018F34 File Offset: 0x00017134
	private void Update()
	{
		for (int i = 0; i < this.lines.Length; i++)
		{
			this.lines[i].SetPosition(0, this.eyeBones[i].transform.position);
			this.lines[i].SetPosition(1, this.rig.transform.position);
		}
	}

	// Token: 0x040004C2 RID: 1218
	private Transform[] eyeBones;

	// Token: 0x040004C3 RID: 1219
	private VRRig rig;

	// Token: 0x040004C4 RID: 1220
	public LineRenderer[] lines;
}
