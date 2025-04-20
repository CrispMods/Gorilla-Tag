using System;
using System.Collections.Generic;
using System.Linq;
using GorillaExtensions;
using UnityEngine;

// Token: 0x020001C4 RID: 452
[DefaultExecutionOrder(1300)]
public class GTPosRotConstraintManager : MonoBehaviour
{
	// Token: 0x06000AB1 RID: 2737 RVA: 0x000377B0 File Offset: 0x000359B0
	protected void Awake()
	{
		if (GTPosRotConstraintManager.hasInstance && GTPosRotConstraintManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		GTPosRotConstraintManager.SetInstance(this);
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x000377D3 File Offset: 0x000359D3
	protected void OnDestroy()
	{
		if (GTPosRotConstraintManager.instance == this)
		{
			GTPosRotConstraintManager.hasInstance = false;
			GTPosRotConstraintManager.instance = null;
		}
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x000982BC File Offset: 0x000964BC
	public void InvokeConstraint(GorillaPosRotConstraint constraint, int index)
	{
		Transform source = constraint.source;
		Transform follower = constraint.follower;
		Vector3 position = source.position + source.TransformVector(constraint.positionOffset);
		Quaternion rotation = source.rotation * constraint.rotationOffset;
		follower.SetPositionAndRotation(position, rotation);
	}

	// Token: 0x06000AB4 RID: 2740 RVA: 0x00098308 File Offset: 0x00096508
	protected void LateUpdate()
	{
		if (this.constraintsToDisable.Count <= 0)
		{
			return;
		}
		for (int i = this.constraintsToDisable.Count - 1; i >= 0; i--)
		{
			for (int j = 0; j < this.constraintsToDisable[i].constraints.Length; j++)
			{
				Transform follower = this.constraintsToDisable[i].constraints[j].follower;
				if (this.originalParent.ContainsKey(follower))
				{
					follower.SetParent(this.originalParent[follower], true);
					follower.localRotation = this.originalRot[follower];
					follower.localPosition = this.originalOffset[follower];
					follower.localScale = this.originalScale[follower];
					this.InvokeConstraint(this.constraintsToDisable[i].constraints[j], i);
				}
			}
			this.constraintsToDisable.RemoveAt(i);
		}
	}

	// Token: 0x06000AB5 RID: 2741 RVA: 0x000377EE File Offset: 0x000359EE
	public static void CreateManager()
	{
		GTPosRotConstraintManager gtposRotConstraintManager = new GameObject("GTPosRotConstraintManager").AddComponent<GTPosRotConstraintManager>();
		GTPosRotConstraintManager.constraints.Clear();
		GTPosRotConstraintManager.componentRanges.Clear();
		GTPosRotConstraintManager.SetInstance(gtposRotConstraintManager);
	}

	// Token: 0x06000AB6 RID: 2742 RVA: 0x00098404 File Offset: 0x00096604
	private static void SetInstance(GTPosRotConstraintManager manager)
	{
		GTPosRotConstraintManager.instance = manager;
		GTPosRotConstraintManager.hasInstance = true;
		GTPosRotConstraintManager.instance.originalParent = new Dictionary<Transform, Transform>();
		GTPosRotConstraintManager.instance.originalOffset = new Dictionary<Transform, Vector3>();
		GTPosRotConstraintManager.instance.originalScale = new Dictionary<Transform, Vector3>();
		GTPosRotConstraintManager.instance.originalRot = new Dictionary<Transform, Quaternion>();
		GTPosRotConstraintManager.instance.constraintsToDisable = new List<GTPosRotConstraints>();
		if (Application.isPlaying)
		{
			manager.transform.SetParent(null, false);
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x06000AB7 RID: 2743 RVA: 0x00098484 File Offset: 0x00096684
	public static void Register(GTPosRotConstraints component)
	{
		if (!GTPosRotConstraintManager.hasInstance)
		{
			GTPosRotConstraintManager.CreateManager();
		}
		int instanceID = component.GetInstanceID();
		if (GTPosRotConstraintManager.componentRanges.ContainsKey(instanceID))
		{
			return;
		}
		for (int i = 0; i < component.constraints.Length; i++)
		{
			if (!component.constraints[i].follower)
			{
				Debug.LogError("Cannot add constraints for GTPosRotConstraints component because the `follower` Transform is null " + string.Format("at index {0}. Path in scene: {1}", i, component.transform.GetPathQ()), component);
				return;
			}
			if (!component.constraints[i].source)
			{
				Debug.LogError("Cannot add constraints for GTPosRotConstraints component because the `source` Transform is null " + string.Format("at index {0}. Path in scene: {1}", i, component.transform.GetPathQ()), component);
				return;
			}
		}
		GTPosRotConstraintManager.Range value = new GTPosRotConstraintManager.Range
		{
			start = GTPosRotConstraintManager.constraints.Count,
			end = GTPosRotConstraintManager.constraints.Count + component.constraints.Length - 1
		};
		GTPosRotConstraintManager.componentRanges.Add(instanceID, value);
		GTPosRotConstraintManager.constraints.AddRange(component.constraints);
		if (GTPosRotConstraintManager.instance.constraintsToDisable.Contains(component))
		{
			GTPosRotConstraintManager.instance.constraintsToDisable.Remove(component);
		}
		for (int j = 0; j < component.constraints.Length; j++)
		{
			Transform follower = component.constraints[j].follower;
			if (GTPosRotConstraintManager.instance.originalParent.ContainsKey(follower))
			{
				component.constraints[j].follower.SetParent(GTPosRotConstraintManager.instance.originalParent[follower], true);
				follower.localRotation = GTPosRotConstraintManager.instance.originalRot[follower];
				follower.localPosition = GTPosRotConstraintManager.instance.originalOffset[follower];
				follower.localScale = GTPosRotConstraintManager.instance.originalScale[follower];
			}
			else
			{
				GTPosRotConstraintManager.instance.originalParent[follower] = follower.parent;
				GTPosRotConstraintManager.instance.originalRot[follower] = follower.localRotation;
				GTPosRotConstraintManager.instance.originalOffset[follower] = follower.localPosition;
				GTPosRotConstraintManager.instance.originalScale[follower] = follower.localScale;
			}
			GTPosRotConstraintManager.instance.InvokeConstraint(component.constraints[j], j);
			component.constraints[j].follower.SetParent(component.constraints[j].source);
		}
	}

	// Token: 0x06000AB8 RID: 2744 RVA: 0x00098724 File Offset: 0x00096924
	public static void Unregister(GTPosRotConstraints component)
	{
		int instanceID = component.GetInstanceID();
		GTPosRotConstraintManager.Range range;
		if (!GTPosRotConstraintManager.hasInstance || !GTPosRotConstraintManager.componentRanges.TryGetValue(instanceID, out range))
		{
			return;
		}
		GTPosRotConstraintManager.constraints.RemoveRange(range.start, 1 + range.end - range.start);
		GTPosRotConstraintManager.componentRanges.Remove(instanceID);
		foreach (int key in GTPosRotConstraintManager.componentRanges.Keys.ToArray<int>())
		{
			GTPosRotConstraintManager.Range range2 = GTPosRotConstraintManager.componentRanges[key];
			if (range2.start > range.end)
			{
				GTPosRotConstraintManager.componentRanges[key] = new GTPosRotConstraintManager.Range
				{
					start = range2.start - range.end + range.start - 1,
					end = range2.end - range.end + range.start - 1
				};
			}
		}
		if (!GTPosRotConstraintManager.instance.constraintsToDisable.Contains(component))
		{
			GTPosRotConstraintManager.instance.constraintsToDisable.Add(component);
		}
	}

	// Token: 0x04000CF9 RID: 3321
	public static GTPosRotConstraintManager instance;

	// Token: 0x04000CFA RID: 3322
	public static bool hasInstance = false;

	// Token: 0x04000CFB RID: 3323
	private const int _kComponentsCapacity = 256;

	// Token: 0x04000CFC RID: 3324
	private const int _kConstraintsCapacity = 1024;

	// Token: 0x04000CFD RID: 3325
	[NonSerialized]
	public Dictionary<Transform, Transform> originalParent;

	// Token: 0x04000CFE RID: 3326
	[NonSerialized]
	public Dictionary<Transform, Vector3> originalOffset;

	// Token: 0x04000CFF RID: 3327
	[NonSerialized]
	public Dictionary<Transform, Vector3> originalScale;

	// Token: 0x04000D00 RID: 3328
	[NonSerialized]
	public Dictionary<Transform, Quaternion> originalRot;

	// Token: 0x04000D01 RID: 3329
	[NonSerialized]
	public List<GTPosRotConstraints> constraintsToDisable;

	// Token: 0x04000D02 RID: 3330
	[OnEnterPlay_Clear]
	private static readonly List<GorillaPosRotConstraint> constraints = new List<GorillaPosRotConstraint>(1024);

	// Token: 0x04000D03 RID: 3331
	[OnEnterPlay_Clear]
	public static readonly Dictionary<int, GTPosRotConstraintManager.Range> componentRanges = new Dictionary<int, GTPosRotConstraintManager.Range>(256);

	// Token: 0x020001C5 RID: 453
	public struct Range
	{
		// Token: 0x04000D04 RID: 3332
		public int start;

		// Token: 0x04000D05 RID: 3333
		public int end;
	}
}
