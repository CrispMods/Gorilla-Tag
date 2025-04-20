using System;
using System.Collections;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B34 RID: 2868
	public class PoseableMannequin : MonoBehaviour
	{
		// Token: 0x060047D1 RID: 18385 RVA: 0x0005EC73 File Offset: 0x0005CE73
		public void Start()
		{
			this.skinnedMeshRenderer.gameObject.SetActive(false);
			this.staticGorillaMesh.gameObject.SetActive(true);
		}

		// Token: 0x060047D2 RID: 18386 RVA: 0x000552B6 File Offset: 0x000534B6
		private string GetPrefabPathFromCurrentPrefabStage()
		{
			return "";
		}

		// Token: 0x060047D3 RID: 18387 RVA: 0x000552B6 File Offset: 0x000534B6
		private string GetMeshPathFromPrefabPath(string prefabPath)
		{
			return "";
		}

		// Token: 0x060047D4 RID: 18388 RVA: 0x0005EC97 File Offset: 0x0005CE97
		public void BakeSkinnedMesh()
		{
			this.BakeAndSaveMeshInPath(this.GetMeshPathFromPrefabPath(this.GetPrefabPathFromCurrentPrefabStage()));
		}

		// Token: 0x060047D5 RID: 18389 RVA: 0x00030607 File Offset: 0x0002E807
		public void BakeAndSaveMeshInPath(string meshPath)
		{
		}

		// Token: 0x060047D6 RID: 18390 RVA: 0x0005ECAB File Offset: 0x0005CEAB
		private void UpdateStaticMeshMannequin()
		{
			this.staticGorillaMesh.sharedMesh = this.BakedColliderMesh;
			this.staticGorillaMeshRenderer.sharedMaterials = this.skinnedMeshRenderer.sharedMaterials;
			this.staticGorillaMeshCollider.sharedMesh = this.BakedColliderMesh;
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x0005ECE5 File Offset: 0x0005CEE5
		private void UpdateSkinnedMeshCollider()
		{
			this.skinnedMeshCollider.sharedMesh = this.BakedColliderMesh;
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x0018BDC8 File Offset: 0x00189FC8
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

		// Token: 0x060047D9 RID: 18393 RVA: 0x0018BE18 File Offset: 0x0018A018
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

		// Token: 0x060047DA RID: 18394 RVA: 0x0018BE8C File Offset: 0x0018A08C
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

		// Token: 0x060047DB RID: 18395 RVA: 0x00030607 File Offset: 0x0002E807
		public void CreasteTestClip()
		{
		}

		// Token: 0x060047DC RID: 18396 RVA: 0x0005ECF8 File Offset: 0x0005CEF8
		public void SerializeVRRig()
		{
			base.StartCoroutine(this.SaveLocalPlayerPose());
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x0005ED07 File Offset: 0x0005CF07
		public IEnumerator SaveLocalPlayerPose()
		{
			yield return null;
			yield break;
		}

		// Token: 0x060047DE RID: 18398 RVA: 0x00030607 File Offset: 0x0002E807
		public void SerializeOutBonesFromSkinnedMesh(SkinnedMeshRenderer paramSkinnedMeshRenderer)
		{
		}

		// Token: 0x060047DF RID: 18399 RVA: 0x0018BEC8 File Offset: 0x0018A0C8
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

		// Token: 0x060047E0 RID: 18400 RVA: 0x00030607 File Offset: 0x0002E807
		public void UpdatePrefabWithAnimationClip(string AnimationFileName)
		{
		}

		// Token: 0x060047E1 RID: 18401 RVA: 0x00030607 File Offset: 0x0002E807
		public void LoadPoseOntoMannequin(AnimationClip clip, float frameTime = 0f)
		{
		}

		// Token: 0x060047E2 RID: 18402 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnValidate()
		{
		}

		// Token: 0x0400490E RID: 18702
		public SkinnedMeshRenderer skinnedMeshRenderer;

		// Token: 0x0400490F RID: 18703
		[FormerlySerializedAs("meshCollider")]
		public MeshCollider skinnedMeshCollider;

		// Token: 0x04004910 RID: 18704
		public GTPosRotConstraints[] cosmeticConstraints;

		// Token: 0x04004911 RID: 18705
		public Mesh BakedColliderMesh;

		// Token: 0x04004912 RID: 18706
		[SerializeField]
		[FormerlySerializedAs("liveAssetPath")]
		protected string prefabAssetPath;

		// Token: 0x04004913 RID: 18707
		[SerializeField]
		protected string prefabFolderPath;

		// Token: 0x04004914 RID: 18708
		[SerializeField]
		protected string prefabAssetName;

		// Token: 0x04004915 RID: 18709
		public MeshFilter staticGorillaMesh;

		// Token: 0x04004916 RID: 18710
		public MeshCollider staticGorillaMeshCollider;

		// Token: 0x04004917 RID: 18711
		public MeshRenderer staticGorillaMeshRenderer;
	}
}
