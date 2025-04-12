using System;
using System.Collections;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B0A RID: 2826
	public class PoseableMannequin : MonoBehaviour
	{
		// Token: 0x06004694 RID: 18068 RVA: 0x0005D25C File Offset: 0x0005B45C
		public void Start()
		{
			this.skinnedMeshRenderer.gameObject.SetActive(false);
			this.staticGorillaMesh.gameObject.SetActive(true);
		}

		// Token: 0x06004695 RID: 18069 RVA: 0x00053D14 File Offset: 0x00051F14
		private string GetPrefabPathFromCurrentPrefabStage()
		{
			return "";
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x00053D14 File Offset: 0x00051F14
		private string GetMeshPathFromPrefabPath(string prefabPath)
		{
			return "";
		}

		// Token: 0x06004697 RID: 18071 RVA: 0x0005D280 File Offset: 0x0005B480
		public void BakeSkinnedMesh()
		{
			this.BakeAndSaveMeshInPath(this.GetMeshPathFromPrefabPath(this.GetPrefabPathFromCurrentPrefabStage()));
		}

		// Token: 0x06004698 RID: 18072 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void BakeAndSaveMeshInPath(string meshPath)
		{
		}

		// Token: 0x06004699 RID: 18073 RVA: 0x0005D294 File Offset: 0x0005B494
		private void UpdateStaticMeshMannequin()
		{
			this.staticGorillaMesh.sharedMesh = this.BakedColliderMesh;
			this.staticGorillaMeshRenderer.sharedMaterials = this.skinnedMeshRenderer.sharedMaterials;
			this.staticGorillaMeshCollider.sharedMesh = this.BakedColliderMesh;
		}

		// Token: 0x0600469A RID: 18074 RVA: 0x0005D2CE File Offset: 0x0005B4CE
		private void UpdateSkinnedMeshCollider()
		{
			this.skinnedMeshCollider.sharedMesh = this.BakedColliderMesh;
		}

		// Token: 0x0600469B RID: 18075 RVA: 0x00184E54 File Offset: 0x00183054
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

		// Token: 0x0600469C RID: 18076 RVA: 0x00184EA4 File Offset: 0x001830A4
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

		// Token: 0x0600469D RID: 18077 RVA: 0x00184F18 File Offset: 0x00183118
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

		// Token: 0x0600469E RID: 18078 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void CreasteTestClip()
		{
		}

		// Token: 0x0600469F RID: 18079 RVA: 0x0005D2E1 File Offset: 0x0005B4E1
		public void SerializeVRRig()
		{
			base.StartCoroutine(this.SaveLocalPlayerPose());
		}

		// Token: 0x060046A0 RID: 18080 RVA: 0x0005D2F0 File Offset: 0x0005B4F0
		public IEnumerator SaveLocalPlayerPose()
		{
			yield return null;
			yield break;
		}

		// Token: 0x060046A1 RID: 18081 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void SerializeOutBonesFromSkinnedMesh(SkinnedMeshRenderer paramSkinnedMeshRenderer)
		{
		}

		// Token: 0x060046A2 RID: 18082 RVA: 0x00184F54 File Offset: 0x00183154
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

		// Token: 0x060046A3 RID: 18083 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void UpdatePrefabWithAnimationClip(string AnimationFileName)
		{
		}

		// Token: 0x060046A4 RID: 18084 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void LoadPoseOntoMannequin(AnimationClip clip, float frameTime = 0f)
		{
		}

		// Token: 0x060046A5 RID: 18085 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnValidate()
		{
		}

		// Token: 0x0400482B RID: 18475
		public SkinnedMeshRenderer skinnedMeshRenderer;

		// Token: 0x0400482C RID: 18476
		[FormerlySerializedAs("meshCollider")]
		public MeshCollider skinnedMeshCollider;

		// Token: 0x0400482D RID: 18477
		public GTPosRotConstraints[] cosmeticConstraints;

		// Token: 0x0400482E RID: 18478
		public Mesh BakedColliderMesh;

		// Token: 0x0400482F RID: 18479
		[SerializeField]
		[FormerlySerializedAs("liveAssetPath")]
		protected string prefabAssetPath;

		// Token: 0x04004830 RID: 18480
		[SerializeField]
		protected string prefabFolderPath;

		// Token: 0x04004831 RID: 18481
		[SerializeField]
		protected string prefabAssetName;

		// Token: 0x04004832 RID: 18482
		public MeshFilter staticGorillaMesh;

		// Token: 0x04004833 RID: 18483
		public MeshCollider staticGorillaMeshCollider;

		// Token: 0x04004834 RID: 18484
		public MeshRenderer staticGorillaMeshRenderer;
	}
}
