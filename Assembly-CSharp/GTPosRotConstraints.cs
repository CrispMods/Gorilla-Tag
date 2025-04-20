using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x020001C6 RID: 454
public class GTPosRotConstraints : MonoBehaviour, ISpawnable
{
	// Token: 0x17000113 RID: 275
	// (get) Token: 0x06000ABB RID: 2747 RVA: 0x0003783E File Offset: 0x00035A3E
	// (set) Token: 0x06000ABC RID: 2748 RVA: 0x00037846 File Offset: 0x00035A46
	public bool IsSpawned { get; set; }

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x06000ABD RID: 2749 RVA: 0x0003784F File Offset: 0x00035A4F
	// (set) Token: 0x06000ABE RID: 2750 RVA: 0x00037857 File Offset: 0x00035A57
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06000ABF RID: 2751 RVA: 0x00098830 File Offset: 0x00096A30
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

	// Token: 0x06000AC0 RID: 2752 RVA: 0x00030607 File Offset: 0x0002E807
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x00037860 File Offset: 0x00035A60
	protected void OnEnable()
	{
		if (this.IsSpawned)
		{
			GTPosRotConstraintManager.Register(this);
		}
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x00037870 File Offset: 0x00035A70
	protected void OnDisable()
	{
		GTPosRotConstraintManager.Unregister(this);
	}

	// Token: 0x04000D06 RID: 3334
	public GorillaPosRotConstraint[] constraints;
}
