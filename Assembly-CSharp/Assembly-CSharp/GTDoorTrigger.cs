using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

// Token: 0x020001B7 RID: 439
public class GTDoorTrigger : MonoBehaviour
{
	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06000A5E RID: 2654 RVA: 0x00038A40 File Offset: 0x00036C40
	public int overlapCount
	{
		get
		{
			return this.overlappingColliders.Count;
		}
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06000A5F RID: 2655 RVA: 0x00038A4D File Offset: 0x00036C4D
	public bool TriggeredThisFrame
	{
		get
		{
			return this.lastTriggeredFrame == Time.frameCount;
		}
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x00038A5C File Offset: 0x00036C5C
	public void ValidateOverlappingColliders()
	{
		for (int i = this.overlappingColliders.Count - 1; i >= 0; i--)
		{
			if (this.overlappingColliders[i] == null || !this.overlappingColliders[i].gameObject.activeInHierarchy || !this.overlappingColliders[i].enabled)
			{
				this.overlappingColliders.RemoveAt(i);
			}
		}
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x00038ACC File Offset: 0x00036CCC
	private void OnTriggerEnter(Collider other)
	{
		if (!this.overlappingColliders.Contains(other))
		{
			this.overlappingColliders.Add(other);
		}
		this.lastTriggeredFrame = Time.frameCount;
		this.TriggeredEvent.Invoke();
		if (this.timeline != null && (this.timeline.time == 0.0 || this.timeline.time >= this.timeline.duration))
		{
			this.timeline.Play();
		}
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x00038B50 File Offset: 0x00036D50
	private void OnTriggerExit(Collider other)
	{
		this.overlappingColliders.Remove(other);
	}

	// Token: 0x04000CAD RID: 3245
	[Tooltip("Optional timeline to play to animate the thing getting activated, play sound, particles, etc...")]
	public PlayableDirector timeline;

	// Token: 0x04000CAE RID: 3246
	private int lastTriggeredFrame = -1;

	// Token: 0x04000CAF RID: 3247
	private List<Collider> overlappingColliders = new List<Collider>(20);

	// Token: 0x04000CB0 RID: 3248
	internal UnityEvent TriggeredEvent = new UnityEvent();
}
