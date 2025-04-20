using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x0200022E RID: 558
[GTStripGameObjectFromBuild("!GT_AUTOMATED_PERF_TEST")]
public class PerfTestGorillaHarness : MonoBehaviour
{
	// Token: 0x06000CE3 RID: 3299 RVA: 0x000A0B20 File Offset: 0x0009ED20
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

	// Token: 0x06000CE4 RID: 3300 RVA: 0x000A0B64 File Offset: 0x0009ED64
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

	// Token: 0x06000CE5 RID: 3301 RVA: 0x000390A6 File Offset: 0x000372A6
	public void StartRecording()
	{
		this._isRecording = true;
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x000A0C08 File Offset: 0x0009EE08
	public void StopRecording()
	{
		foreach (PerfTestGorillaSlot perfTestGorillaSlot in this.dummySlots)
		{
			perfTestGorillaSlot.transform.localPosition = perfTestGorillaSlot.localStartPosition;
		}
		this._isRecording = false;
	}

	// Token: 0x04001043 RID: 4163
	public PerfTestGorillaSlot _vrSlot;

	// Token: 0x04001044 RID: 4164
	public List<PerfTestGorillaSlot> dummySlots = new List<PerfTestGorillaSlot>(9);

	// Token: 0x04001045 RID: 4165
	[OnEnterPlay_Set(false)]
	private bool _isRecording;

	// Token: 0x04001046 RID: 4166
	private float _nextRandomMoveTime;

	// Token: 0x04001047 RID: 4167
	private float bounceSpeed = 5f;

	// Token: 0x04001048 RID: 4168
	private float bounceAmplitude = 0.5f;
}
