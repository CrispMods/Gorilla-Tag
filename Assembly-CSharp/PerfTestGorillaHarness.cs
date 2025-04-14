using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x02000223 RID: 547
[GTStripGameObjectFromBuild("!GT_AUTOMATED_PERF_TEST")]
public class PerfTestGorillaHarness : MonoBehaviour
{
	// Token: 0x06000C98 RID: 3224 RVA: 0x00042A50 File Offset: 0x00040C50
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

	// Token: 0x06000C99 RID: 3225 RVA: 0x00042A94 File Offset: 0x00040C94
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

	// Token: 0x06000C9A RID: 3226 RVA: 0x00042B38 File Offset: 0x00040D38
	public void StartRecording()
	{
		this._isRecording = true;
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x00042B44 File Offset: 0x00040D44
	public void StopRecording()
	{
		foreach (PerfTestGorillaSlot perfTestGorillaSlot in this.dummySlots)
		{
			perfTestGorillaSlot.transform.localPosition = perfTestGorillaSlot.localStartPosition;
		}
		this._isRecording = false;
	}

	// Token: 0x04000FFD RID: 4093
	public PerfTestGorillaSlot _vrSlot;

	// Token: 0x04000FFE RID: 4094
	public List<PerfTestGorillaSlot> dummySlots = new List<PerfTestGorillaSlot>(9);

	// Token: 0x04000FFF RID: 4095
	[OnEnterPlay_Set(false)]
	private bool _isRecording;

	// Token: 0x04001000 RID: 4096
	private float _nextRandomMoveTime;

	// Token: 0x04001001 RID: 4097
	private float bounceSpeed = 5f;

	// Token: 0x04001002 RID: 4098
	private float bounceAmplitude = 0.5f;
}
