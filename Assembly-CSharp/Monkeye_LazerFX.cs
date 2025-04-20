using System;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class Monkeye_LazerFX : MonoBehaviour
{
	// Token: 0x0600045D RID: 1117 RVA: 0x000334BC File Offset: 0x000316BC
	private void Awake()
	{
		base.enabled = false;
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x0007CA78 File Offset: 0x0007AC78
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

	// Token: 0x0600045F RID: 1119 RVA: 0x0007CAC8 File Offset: 0x0007ACC8
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

	// Token: 0x06000460 RID: 1120 RVA: 0x0007CB04 File Offset: 0x0007AD04
	private void Update()
	{
		for (int i = 0; i < this.lines.Length; i++)
		{
			this.lines[i].SetPosition(0, this.eyeBones[i].transform.position);
			this.lines[i].SetPosition(1, this.rig.transform.position);
		}
	}

	// Token: 0x04000502 RID: 1282
	private Transform[] eyeBones;

	// Token: 0x04000503 RID: 1283
	private VRRig rig;

	// Token: 0x04000504 RID: 1284
	public LineRenderer[] lines;
}
