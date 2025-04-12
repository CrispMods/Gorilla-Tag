using System;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class Monkeye_LazerFX : MonoBehaviour
{
	// Token: 0x06000423 RID: 1059 RVA: 0x000322B5 File Offset: 0x000304B5
	private void Awake()
	{
		base.enabled = false;
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x0007A21C File Offset: 0x0007841C
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

	// Token: 0x06000425 RID: 1061 RVA: 0x0007A26C File Offset: 0x0007846C
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

	// Token: 0x06000426 RID: 1062 RVA: 0x0007A2A8 File Offset: 0x000784A8
	private void Update()
	{
		for (int i = 0; i < this.lines.Length; i++)
		{
			this.lines[i].SetPosition(0, this.eyeBones[i].transform.position);
			this.lines[i].SetPosition(1, this.rig.transform.position);
		}
	}

	// Token: 0x040004C3 RID: 1219
	private Transform[] eyeBones;

	// Token: 0x040004C4 RID: 1220
	private VRRig rig;

	// Token: 0x040004C5 RID: 1221
	public LineRenderer[] lines;
}
