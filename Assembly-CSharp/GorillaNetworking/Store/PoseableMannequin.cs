using System;
using System.Collections;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B07 RID: 2823
	public class PoseableMannequin : MonoBehaviour
	{
		// Token: 0x06004688 RID: 18056 RVA: 0x0014F235 File Offset: 0x0014D435
		public void Start()
		{
			this.skinnedMeshRenderer.gameObject.SetActive(false);
			this.staticGorillaMesh.gameObject.SetActive(true);
		}

		// Token: 0x06004689 RID: 18057 RVA: 0x00105D54 File Offset: 0x00103F54
		private string GetPrefabPathFromCurrentPrefabStage()
		{
			return "";
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x00105D54 File Offset: 0x00103F54
		private string GetMeshPathFromPrefabPath(string prefabPath)
		{
			return "";
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x0014F259 File Offset: 0x0014D459
		public void BakeSkinnedMesh()
		{
			this.BakeAndSaveMeshInPath(this.GetMeshPathFromPrefabPath(this.GetPrefabPathFromCurrentPrefabStage()));
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x000023F4 File Offset: 0x000005F4
		public void BakeAndSaveMeshInPath(string meshPath)
		{
		}

		// Token: 0x0600468D RID: 18061 RVA: 0x0014F26D File Offset: 0x0014D46D
		private void UpdateStaticMeshMannequin()
		{
			this.staticGorillaMesh.sharedMesh = this.BakedColliderMesh;
			this.staticGorillaMeshRenderer.sharedMaterials = this.skinnedMeshRenderer.sharedMaterials;
			this.staticGorillaMeshCollider.sharedMesh = this.BakedColliderMesh;
		}

		// Token: 0x0600468E RID: 18062 RVA: 0x0014F2A7 File Offset: 0x0014D4A7
		private void UpdateSkinnedMeshCollider()
		{
			this.skinnedMeshCollider.sharedMesh = this.BakedColliderMesh;
		}

		// Token: 0x0600468F RID: 18063 RVA: 0x0014F2BC File Offset: 0x0014D4BC
		public void UpdateGTPosRotConstraints()
		{
			GTPosRotConstraints[] array = this.cosmeticConstraints;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].constraints.ForEach(delegate(GorillaPosRotConstraint c)
				{
					c.follower.rotation = c.source.rotation;
					c.follower.position = c.source.position;
				});
			}
		}

		// Token: 0x06004690 RID: 18064 RVA: 0x0014F30C File Offset: 0x0014D50C
		private void HookupCosmeticConstraints()
		{
			this.cosmeticConstraints = base.GetComponentsInChildren<GTPosRotConstraints>();
			foreach (GTPosRotConstraints gtposRotConstraints in this.cosmeticConstraints)
			{
				for (int j = 0; j < gtposRotConstraints.constraints.Length; j++)
				{
					gtposRotConstraints.constraints[j].source = this.FindBone(gtposRotConstraints.constraints[j].follower.name);
				}
			}
		}

		// Token: 0x06004691 RID: 18065 RVA: 0x0014F380 File Offset: 0x0014D580
		private Transform FindBone(string boneName)
		{
			foreach (Transform transform in this.skinnedMeshRenderer.bones)
			{
				if (transform.name == boneName)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x06004692 RID: 18066 RVA: 0x000023F4 File Offset: 0x000005F4
		public void CreasteTestClip()
		{
		}

		// Token: 0x06004693 RID: 18067 RVA: 0x0014F3BC File Offset: 0x0014D5BC
		public void SerializeVRRig()
		{
			base.StartCoroutine(this.SaveLocalPlayerPose());
		}

		// Token: 0x06004694 RID: 18068 RVA: 0x0014F3CB File Offset: 0x0014D5CB
		public IEnumerator SaveLocalPlayerPose()
		{
			yield return null;
			yield break;
		}

		// Token: 0x06004695 RID: 18069 RVA: 0x000023F4 File Offset: 0x000005F4
		public void SerializeOutBonesFromSkinnedMesh(SkinnedMeshRenderer paramSkinnedMeshRenderer)
		{
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x0014F3D4 File Offset: 0x0014D5D4
		public void SetCurvesForBone(SkinnedMeshRenderer paramSkinnedMeshRenderer, AnimationClip clip, Transform bone)
		{
			Keyframe[] keys = new Keyframe[]
			{
				new Keyframe(0f, bone.parent.localRotation.x)
			};
			Keyframe[] keys2 = new Keyframe[]
			{
				new Keyframe(0f, bone.parent.localRotation.y)
			};
			Keyframe[] keys3 = new Keyframe[]
			{
				new Keyframe(0f, bone.parent.localRotation.z)
			};
			Keyframe[] keys4 = new Keyframe[]
			{
				new Keyframe(0f, bone.parent.localRotation.w)
			};
			AnimationCurve curve = new AnimationCurve(keys);
			AnimationCurve curve2 = new AnimationCurve(keys2);
			AnimationCurve curve3 = new AnimationCurve(keys3);
			AnimationCurve curve4 = new AnimationCurve(keys4);
			string relativePath = "";
			string b = bone.name.Replace("_new", "");
			foreach (Transform transform in this.skinnedMeshRenderer.bones)
			{
				if (transform.name == b)
				{
					relativePath = transform.GetPath(this.skinnedMeshRenderer.transform.parent).TrimStart('/');
					break;
				}
			}
			clip.SetCurve(relativePath, typeof(Transform), "m_LocalRotation.x", curve);
			clip.SetCurve(relativePath, typeof(Transform), "m_LocalRotation.y", curve2);
			clip.SetCurve(relativePath, typeof(Transform), "m_LocalRotation.z", curve3);
			clip.SetCurve(relativePath, typeof(Transform), "m_LocalRotation.w", curve4);
		}

		// Token: 0x06004697 RID: 18071 RVA: 0x000023F4 File Offset: 0x000005F4
		public void UpdatePrefabWithAnimationClip(string AnimationFileName)
		{
		}

		// Token: 0x06004698 RID: 18072 RVA: 0x000023F4 File Offset: 0x000005F4
		public void LoadPoseOntoMannequin(AnimationClip clip, float frameTime = 0f)
		{
		}

		// Token: 0x06004699 RID: 18073 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnValidate()
		{
		}

		// Token: 0x04004819 RID: 18457
		public SkinnedMeshRenderer skinnedMeshRenderer;

		// Token: 0x0400481A RID: 18458
		[FormerlySerializedAs("meshCollider")]
		public MeshCollider skinnedMeshCollider;

		// Token: 0x0400481B RID: 18459
		public GTPosRotConstraints[] cosmeticConstraints;

		// Token: 0x0400481C RID: 18460
		public Mesh BakedColliderMesh;

		// Token: 0x0400481D RID: 18461
		[SerializeField]
		[FormerlySerializedAs("liveAssetPath")]
		protected string prefabAssetPath;

		// Token: 0x0400481E RID: 18462
		[SerializeField]
		protected string prefabFolderPath;

		// Token: 0x0400481F RID: 18463
		[SerializeField]
		protected string prefabAssetName;

		// Token: 0x04004820 RID: 18464
		public MeshFilter staticGorillaMesh;

		// Token: 0x04004821 RID: 18465
		public MeshCollider staticGorillaMeshCollider;

		// Token: 0x04004822 RID: 18466
		public MeshRenderer staticGorillaMeshRenderer;
	}
}
