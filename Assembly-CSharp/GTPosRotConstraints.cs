using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x020001BB RID: 443
public class GTPosRotConstraints : MonoBehaviour, ISpawnable
{
	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06000A6F RID: 2671 RVA: 0x00038EA0 File Offset: 0x000370A0
	// (set) Token: 0x06000A70 RID: 2672 RVA: 0x00038EA8 File Offset: 0x000370A8
	public bool IsSpawned { get; set; }

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x06000A71 RID: 2673 RVA: 0x00038EB1 File Offset: 0x000370B1
	// (set) Token: 0x06000A72 RID: 2674 RVA: 0x00038EB9 File Offset: 0x000370B9
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06000A73 RID: 2675 RVA: 0x00038EC4 File Offset: 0x000370C4
	void ISpawnable.OnSpawn(VRRig rig)
	{
		Transform[] array = Array.Empty<Transform>();
		string str;
		if (rig != null && !GTHardCodedBones.TryGetBoneXforms(rig, out array, out str))
		{
			Debug.LogError("GTPosRotConstraints: Error getting bone Transforms: " + str, this);
			return;
		}
		for (int i = 0; i < this.constraints.Length; i++)
		{
			GorillaPosRotConstraint gorillaPosRotConstraint = this.constraints[i];
			if (Mathf.Approximately(gorillaPosRotConstraint.rotationOffset.x, 0f) && Mathf.Approximately(gorillaPosRotConstraint.rotationOffset.y, 0f) && Mathf.Approximately(gorillaPosRotConstraint.rotationOffset.z, 0f) && Mathf.Approximately(gorillaPosRotConstraint.rotationOffset.w, 0f))
			{
				gorillaPosRotConstraint.rotationOffset = Quaternion.identity;
			}
			if (!gorillaPosRotConstraint.follower)
			{
				Debug.LogError(string.Concat(new string[]
				{
					string.Format("{0}: Disabling component! At index {1}, Transform `follower` is ", "GTPosRotConstraints", i),
					"null. Affected component path: ",
					base.transform.GetPathQ(),
					"\n- Affected component path: ",
					base.transform.GetPathQ()
				}), this);
				base.enabled = false;
				return;
			}
			if (gorillaPosRotConstraint.sourceGorillaBone == GTHardCodedBones.EBone.None)
			{
				if (!gorillaPosRotConstraint.source)
				{
					if (string.IsNullOrEmpty(gorillaPosRotConstraint.sourceRelativePath))
					{
						Debug.LogError(string.Format("{0}: Disabling component! At index {1} Transform `source` is ", "GTPosRotConstraints", i) + "null, not EBone, and `sourceRelativePath` is null or empty.\n- Affected component path: " + base.transform.GetPathQ(), this);
						base.enabled = false;
						return;
					}
					if (!base.transform.TryFindByPath(gorillaPosRotConstraint.sourceRelativePath, out gorillaPosRotConstraint.source, false))
					{
						Debug.LogError(string.Concat(new string[]
						{
							string.Format("{0}: Disabling component! At index {1} Transform `source` is ", "GTPosRotConstraints", i),
							"null, not EBone, and could not find by path: \"",
							gorillaPosRotConstraint.sourceRelativePath,
							"\"\n- Affected component path: ",
							base.transform.GetPathQ()
						}), this);
						base.enabled = false;
						return;
					}
				}
				this.constraints[i] = gorillaPosRotConstraint;
			}
			else
			{
				if (rig == null)
				{
					Debug.LogError("GTPosRotConstraints: Disabling component! `VRRig` could not be found in parents, but " + string.Format("bone at index {0} is set to use EBone `{1}` but without `VRRig` it cannot ", i, gorillaPosRotConstraint.sourceGorillaBone) + "be resolved.\n- Affected component path: " + base.transform.GetPathQ(), this);
					base.enabled = false;
					return;
				}
				int boneIndex = GTHardCodedBones.GetBoneIndex(gorillaPosRotConstraint.sourceGorillaBone);
				if (boneIndex <= 0)
				{
					Debug.LogError(string.Format("{0}: (should never happen) Disabling component! At index {1}, could ", "GTPosRotConstraints", i) + string.Format("not find EBone `{0}`.\n", gorillaPosRotConstraint.sourceGorillaBone) + "- Affected component path: " + base.transform.GetPathQ(), this);
					base.enabled = false;
					return;
				}
				gorillaPosRotConstraint.source = array[boneIndex];
				if (!gorillaPosRotConstraint.source)
				{
					Debug.LogError(string.Concat(new string[]
					{
						string.Format("{0}: Disabling component! At index {1}, bone {2} was ", "GTPosRotConstraints", i, gorillaPosRotConstraint.sourceGorillaBone),
						"not present in `VRRig` path: ",
						rig.transform.GetPathQ(),
						"\n- Affected component path: ",
						base.transform.GetPathQ()
					}), this);
					base.enabled = false;
					return;
				}
				this.constraints[i] = gorillaPosRotConstraint;
			}
		}
		if (base.isActiveAndEnabled)
		{
			GTPosRotConstraintManager.Register(this);
		}
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x00039233 File Offset: 0x00037433
	protected void OnEnable()
	{
		if (this.IsSpawned)
		{
			GTPosRotConstraintManager.Register(this);
		}
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x00039243 File Offset: 0x00037443
	protected void OnDisable()
	{
		GTPosRotConstraintManager.Unregister(this);
	}

	// Token: 0x04000CC0 RID: 3264
	public GorillaPosRotConstraint[] constraints;
}
