using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

// Token: 0x020001B7 RID: 439
public class GTDoorTrigger : MonoBehaviour
{
	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06000A5C RID: 2652 RVA: 0x0003871C File Offset: 0x0003691C
	public int overlapCount
	{
		get
		{
			return this.overlappingColliders.Count;
		}
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06000A5D RID: 2653 RVA: 0x00038729 File Offset: 0x00036929
	public bool TriggeredThisFrame
	{
		get
		{
			return this.lastTriggeredFrame == Time.frameCount;
		}
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x00038738 File Offset: 0x00036938
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

	// Token: 0x06000A5F RID: 2655 RVA: 0x000387A8 File Offset: 0x000369A8
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

	// Token: 0x06000A60 RID: 2656 RVA: 0x0003882C File Offset: 0x00036A2C
	private void OnTriggerExit(Collider other)
	{
		this.overlappingColliders.Remove(other);
	}

	// Token: 0x04000CAC RID: 3244
	[Tooltip("Optional timeline to play to animate the thing getting activated, play sound, particles, etc...")]
	public PlayableDirector timeline;

	// Token: 0x04000CAD RID: 3245
	private int lastTriggeredFrame = -1;

	// Token: 0x04000CAE RID: 3246
	private List<Collider> overlappingColliders = new List<Collider>(20);

	// Token: 0x04000CAF RID: 3247
	internal UnityEvent TriggeredEvent = new UnityEvent();
}
