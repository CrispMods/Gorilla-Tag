using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x02000223 RID: 547
[GTStripGameObjectFromBuild("!GT_AUTOMATED_PERF_TEST")]
public class PerfTestGorillaHarness : MonoBehaviour
{
	// Token: 0x06000C9A RID: 3226 RVA: 0x0009E294 File Offset: 0x0009C494
	private void Awake()
	{
		foreach (PerfTestGorillaSlot perfTestGorillaSlot in base.GetComponentsInChildren<PerfTestGorillaSlot>())
		{
			if (perfTestGorillaSlot.slotType == PerfTestGorillaSlot.SlotType.VR_PLAYER)
			{
				this._vrSlot = perfTestGorillaSlot;
			}
			else
			{
				this.dummySlots.Add(perfTestGorillaSlot);
			}
		}
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x0009E2D8 File Offset: 0x0009C4D8
	private void Update()
	{
		if (!this._isRecording)
		{
			return;
		}
		foreach (PerfTestGorillaSlot perfTestGorillaSlot in this.dummySlots)
		{
			float y = perfTestGorillaSlot.localStartPosition.y + Mathf.Sin(Time.time * this.bounceSpeed) * this.bounceAmplitude;
			perfTestGorillaSlot.transform.localPosition = new Vector3(perfTestGorillaSlot.localStartPosition.x, y, perfTestGorillaSlot.localStartPosition.z);
		}
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x00037DE6 File Offset: 0x00035FE6
	public void StartRecording()
	{
		this._isRecording = true;
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x0009E37C File Offset: 0x0009C57C
	public void StopRecording()
	{
		foreach (PerfTestGorillaSlot perfTestGorillaSlot in this.dummySlots)
		{
			perfTestGorillaSlot.transform.localPosition = perfTestGorillaSlot.localStartPosition;
		}
		this._isRecording = false;
	}

	// Token: 0x04000FFE RID: 4094
	public PerfTestGorillaSlot _vrSlot;

	// Token: 0x04000FFF RID: 4095
	public List<PerfTestGorillaSlot> dummySlots = new List<PerfTestGorillaSlot>(9);

	// Token: 0x04001000 RID: 4096
	[OnEnterPlay_Set(false)]
	private bool _isRecording;

	// Token: 0x04001001 RID: 4097
	private float _nextRandomMoveTime;

	// Token: 0x04001002 RID: 4098
	private float bounceSpeed = 5f;

	// Token: 0x04001003 RID: 4099
	private float bounceAmplitude = 0.5f;
}
