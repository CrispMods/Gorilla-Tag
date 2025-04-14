using System;
using System.Collections.Generic;
using Drawing;
using GorillaTag;
using UnityEngine;

// Token: 0x020006D9 RID: 1753
public class VolumeCast : MonoBehaviourGizmos
{
	// Token: 0x06002BB8 RID: 11192 RVA: 0x000D6B50 File Offset: 0x000D4D50
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

	// Token: 0x06002BB9 RID: 11193 RVA: 0x000D6D94 File Offset: 0x000D4F94
	private static void GetEndsAndRadius(Transform t, Vector3 center, float height, float radius, out Vector3 a, out Vector3 b, out float r)
	{
		float d = height * 0.5f;
		Vector3 lossyScale = t.lossyScale;
		a = t.TransformPoint(center + Vector3.down * d);
		b = t.TransformPoint(center + Vector3.up * d);
		r = Math.Max(Math.Abs(lossyScale.x), Math.Abs(lossyScale.z)) * radius;
	}

	// Token: 0x040030CE RID: 12494
	public VolumeCast.VolumeShape shape;

	// Token: 0x040030CF RID: 12495
	[Space]
	public Vector3 center;

	// Token: 0x040030D0 RID: 12496
	public Vector3 size = Vector3.one;

	// Token: 0x040030D1 RID: 12497
	public float height = 1f;

	// Token: 0x040030D2 RID: 12498
	public float radius = 1f;

	// Token: 0x040030D3 RID: 12499
	private const int MAX_HITS = 8;

	// Token: 0x040030D4 RID: 12500
	[Space]
	public UnityLayerMask physicsMask = UnityLayerMask.Everything;

	// Token: 0x040030D5 RID: 12501
	public bool includeTriggers;

	// Token: 0x040030D6 RID: 12502
	[Space]
	[SerializeField]
	private bool _simulateInEditMode;

	// Token: 0x040030D7 RID: 12503
	[DebugReadout]
	[NonSerialized]
	private int _capHits;

	// Token: 0x040030D8 RID: 12504
	[DebugReadout]
	[NonSerialized]
	private Collider[] _capOverlaps = new Collider[8];

	// Token: 0x040030D9 RID: 12505
	[DebugReadout]
	[NonSerialized]
	private int _boxHits;

	// Token: 0x040030DA RID: 12506
	[DebugReadout]
	[NonSerialized]
	private Collider[] _boxOverlaps = new Collider[8];

	// Token: 0x040030DB RID: 12507
	[DebugReadout]
	[NonSerialized]
	private int _hits;

	// Token: 0x040030DC RID: 12508
	[DebugReadout]
	[NonSerialized]
	private Collider[] _overlaps = new Collider[8];

	// Token: 0x040030DD RID: 12509
	[DebugReadout]
	[NonSerialized]
	private bool _colliding;

	// Token: 0x040030DE RID: 12510
	[NonSerialized]
	private HashSet<Collider> _set = new HashSet<Collider>(8);

	// Token: 0x020006DA RID: 1754
	public enum VolumeShape
	{
		// Token: 0x040030E0 RID: 12512
		Box,
		// Token: 0x040030E1 RID: 12513
		Cylinder
	}
}
