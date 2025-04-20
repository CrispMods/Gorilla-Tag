using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

// Token: 0x020001C2 RID: 450
public class GTDoorTrigger : MonoBehaviour
{
	// Token: 0x17000110 RID: 272
	// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x00037722 File Offset: 0x00035922
	public int overlapCount
	{
		get
		{
			return this.overlappingColliders.Count;
		}
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x06000AA9 RID: 2729 RVA: 0x0003772F File Offset: 0x0003592F
	public bool TriggeredThisFrame
	{
		get
		{
			return this.lastTriggeredFrame == Time.frameCount;
		}
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x000981C8 File Offset: 0x000963C8
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

	// Token: 0x06000AAB RID: 2731 RVA: 0x00098238 File Offset: 0x00096438
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

	// Token: 0x06000AAC RID: 2732 RVA: 0x0003773E File Offset: 0x0003593E
	private void OnTriggerExit(Collider other)
	{
		this.overlappingColliders.Remove(other);
	}

	// Token: 0x04000CF2 RID: 3314
	[Tooltip("Optional timeline to play to animate the thing getting activated, play sound, particles, etc...")]
	public PlayableDirector timeline;

	// Token: 0x04000CF3 RID: 3315
	private int lastTriggeredFrame = -1;

	// Token: 0x04000CF4 RID: 3316
	private List<Collider> overlappingColliders = new List<Collider>(20);

	// Token: 0x04000CF5 RID: 3317
	internal UnityEvent TriggeredEvent = new UnityEvent();
}
