﻿using System;
using System.Collections.Generic;
using Drawing;
using GorillaTag;
using UnityEngine;

// Token: 0x020006EE RID: 1774
public class VolumeCast : MonoBehaviourGizmos
{
	// Token: 0x06002C4E RID: 11342 RVA: 0x00121D84 File Offset: 0x0011FF84
	public bool CheckOverlaps()
	{
		Transform transform = base.transform;
		Vector3 lossyScale = transform.lossyScale;
		Quaternion rotation = transform.rotation;
		int num = (int)this.physicsMask;
		QueryTriggerInteraction queryTriggerInteraction = this.includeTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore;
		Vector3 vector;
		Vector3 vector2;
		float num2;
		VolumeCast.GetEndsAndRadius(transform, this.center, this.height, this.radius, out vector, out vector2, out num2);
		VolumeCast.VolumeShape volumeShape = this.shape;
		Vector3 vector3;
		Vector3 halfExtents;
		if (volumeShape != VolumeCast.VolumeShape.Box)
		{
			if (volumeShape != VolumeCast.VolumeShape.Cylinder)
			{
				return false;
			}
			vector3 = (vector + vector2) * 0.5f;
			halfExtents = new Vector3(num2, Vector3.Distance(vector, vector2) * 0.5f, num2);
		}
		else
		{
			vector3 = transform.TransformPoint(this.center);
			halfExtents = Vector3.Scale(lossyScale, this.size * 0.5f).Abs();
		}
		Array.Clear(this._boxOverlaps, 0, 8);
		this._boxHits = Physics.OverlapBoxNonAlloc(vector3, halfExtents, this._boxOverlaps, rotation, num, queryTriggerInteraction);
		if (this.shape != VolumeCast.VolumeShape.Cylinder)
		{
			return this._colliding = (this._boxHits > 0);
		}
		this._hits = 0;
		Array.Clear(this._capOverlaps, 0, 8);
		Array.Clear(this._overlaps, 0, 8);
		this._capHits = Physics.OverlapCapsuleNonAlloc(vector, vector2, num2, this._capOverlaps, num, queryTriggerInteraction);
		this._set.Clear();
		int num3 = Math.Max(this._capHits, this._boxHits);
		Collider[] array = (this._capHits < this._boxHits) ? this._capOverlaps : this._boxOverlaps;
		Collider[] array2 = (this._capHits < this._boxHits) ? this._boxOverlaps : this._capOverlaps;
		for (int i = 0; i < num3; i++)
		{
			Collider collider = array[i];
			if (collider && !this._set.Add(collider))
			{
				Collider[] overlaps = this._overlaps;
				int hits = this._hits;
				this._hits = hits + 1;
				overlaps[hits] = collider;
			}
			Collider collider2 = array2[i];
			if (collider2 && !this._set.Add(collider2))
			{
				Collider[] overlaps2 = this._overlaps;
				int hits = this._hits;
				this._hits = hits + 1;
				overlaps2[hits] = collider2;
			}
		}
		return this._colliding = (this._hits > 0);
	}

	// Token: 0x06002C4F RID: 11343 RVA: 0x00121FC8 File Offset: 0x001201C8
	private static void GetEndsAndRadius(Transform t, Vector3 center, float height, float radius, out Vector3 a, out Vector3 b, out float r)
	{
		float d = height * 0.5f;
		Vector3 lossyScale = t.lossyScale;
		a = t.TransformPoint(center + Vector3.down * d);
		b = t.TransformPoint(center + Vector3.up * d);
		r = Math.Max(Math.Abs(lossyScale.x), Math.Abs(lossyScale.z)) * radius;
	}

	// Token: 0x0400316B RID: 12651
	public VolumeCast.VolumeShape shape;

	// Token: 0x0400316C RID: 12652
	[Space]
	public Vector3 center;

	// Token: 0x0400316D RID: 12653
	public Vector3 size = Vector3.one;

	// Token: 0x0400316E RID: 12654
	public float height = 1f;

	// Token: 0x0400316F RID: 12655
	public float radius = 1f;

	// Token: 0x04003170 RID: 12656
	private const int MAX_HITS = 8;

	// Token: 0x04003171 RID: 12657
	[Space]
	public UnityLayerMask physicsMask = UnityLayerMask.Everything;

	// Token: 0x04003172 RID: 12658
	public bool includeTriggers;

	// Token: 0x04003173 RID: 12659
	[Space]
	[SerializeField]
	private bool _simulateInEditMode;

	// Token: 0x04003174 RID: 12660
	[DebugReadout]
	[NonSerialized]
	private int _capHits;

	// Token: 0x04003175 RID: 12661
	[DebugReadout]
	[NonSerialized]
	private Collider[] _capOverlaps = new Collider[8];

	// Token: 0x04003176 RID: 12662
	[DebugReadout]
	[NonSerialized]
	private int _boxHits;

	// Token: 0x04003177 RID: 12663
	[DebugReadout]
	[NonSerialized]
	private Collider[] _boxOverlaps = new Collider[8];

	// Token: 0x04003178 RID: 12664
	[DebugReadout]
	[NonSerialized]
	private int _hits;

	// Token: 0x04003179 RID: 12665
	[DebugReadout]
	[NonSerialized]
	private Collider[] _overlaps = new Collider[8];

	// Token: 0x0400317A RID: 12666
	[DebugReadout]
	[NonSerialized]
	private bool _colliding;

	// Token: 0x0400317B RID: 12667
	[NonSerialized]
	private HashSet<Collider> _set = new HashSet<Collider>(8);

	// Token: 0x020006EF RID: 1775
	public enum VolumeShape
	{
		// Token: 0x0400317D RID: 12669
		Box,
		// Token: 0x0400317E RID: 12670
		Cylinder
	}
}
