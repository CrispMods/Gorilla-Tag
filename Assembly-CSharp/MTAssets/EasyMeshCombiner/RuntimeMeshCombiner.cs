using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace MTAssets.EasyMeshCombiner
{
	// Token: 0x02000B1B RID: 2843
	[AddComponentMenu("MT Assets/Easy Mesh Combiner/Runtime Mesh Combiner")]
	public class RuntimeMeshCombiner : MonoBehaviour
	{
		// Token: 0x060046FD RID: 18173 RVA: 0x00150D39 File Offset: 0x0014EF39
		private void Awake()
		{
			if (this.combineMeshesAtStartUp == RuntimeMeshCombiner.CombineOnStart.OnAwake)
			{
				if (this.showDebugLogs)
				{
					Debug.Log("The merge started in Runtime Combiner \"" + base.gameObject.name + "\".");
				}
				this.CombineMeshes();
			}
		}

		// Token: 0x060046FE RID: 18174 RVA: 0x00150D72 File Offset: 0x0014EF72
		private void Start()
		{
			if (this.combineMeshesAtStartUp == RuntimeMeshCombiner.CombineOnStart.OnStart)
			{
				if (this.showDebugLogs)
				{
					Debug.Log("The merge started in Runtime Combiner \"" + base.gameObject.name + "\".");
				}
				this.CombineMeshes();
			}
		}

		// Token: 0x060046FF RID: 18175 RVA: 0x00150DAC File Offset: 0x0014EFAC
		private RuntimeMeshCombiner.GameObjectWithMesh[] GetValidatedTargetGameObjects()
		{
			List<Transform> list = new List<Transform>();
			for (int i = 0; i < this.targetMeshes.Count; i++)
			{
				if (!(this.targetMeshes[i] == null))
				{
					if (this.combineInChildren)
					{
						foreach (Transform item in this.targetMeshes[i].GetComponentsInChildren<Transform>(true))
						{
							if (!list.Contains(item))
							{
								list.Add(item);
							}
						}
					}
					if (!this.combineInChildren)
					{
						Transform component = this.targetMeshes[i].GetComponent<Transform>();
						if (!list.Contains(component))
						{
							list.Add(component);
						}
					}
				}
			}
			List<RuntimeMeshCombiner.GameObjectWithMesh> list2 = new List<RuntimeMeshCombiner.GameObjectWithMesh>();
			for (int k = 0; k < list.Count; k++)
			{
				MeshFilter component2 = list[k].GetComponent<MeshFilter>();
				MeshRenderer component3 = list[k].GetComponent<MeshRenderer>();
				if ((component2 != null || component3 != null) && (this.combineInactives || component3.enabled) && (this.combineInactives || list[k].gameObject.activeSelf) && (this.combineInactives || list[k].gameObject.activeInHierarchy))
				{
					list2.Add(new RuntimeMeshCombiner.GameObjectWithMesh(list[k].gameObject, component2, component3));
				}
			}
			List<RuntimeMeshCombiner.GameObjectWithMesh> list3 = new List<RuntimeMeshCombiner.GameObjectWithMesh>();
			for (int l = 0; l < list2.Count; l++)
			{
				bool flag = true;
				if (list2[l].meshFilter == null)
				{
					if (this.showDebugLogs)
					{
						Debug.LogError("GameObject \"" + list2[l].gameObject.name + "\" does not have the Mesh Filter component, so it is not a valid mesh and will be ignored in the merge process.");
					}
					flag = false;
				}
				if (list2[l].meshRenderer == null)
				{
					if (this.showDebugLogs)
					{
						Debug.LogError("GameObject \"" + list2[l].gameObject.name + "\" does not have the Mesh Renderer component, so it is not a valid mesh and will be ignored in the merge process.");
					}
					flag = false;
				}
				if (list2[l].meshFilter != null && list2[l].meshFilter.sharedMesh == null)
				{
					if (this.showDebugLogs)
					{
						Debug.LogError("GameObject \"" + list2[l].gameObject.name + "\" does not have a Mesh in Mesh Filter component, so it is not a valid mesh and will be ignored in the merge process.");
					}
					flag = false;
				}
				if (list2[l].meshFilter != null && list2[l].meshRenderer != null && list2[l].meshFilter.sharedMesh != null && list2[l].meshFilter.sharedMesh.subMeshCount != list2[l].meshRenderer.sharedMaterials.Length)
				{
					if (this.showDebugLogs)
					{
						Debug.LogError(string.Concat(new string[]
						{
							"The Mesh Renderer component found in GameObject \"",
							list2[l].gameObject.name,
							"\" has more or less material needed. The mesh that is in this GameObject has ",
							list2[l].meshFilter.sharedMesh.subMeshCount.ToString(),
							" submeshes, but has a number of ",
							list2[l].meshRenderer.sharedMaterials.Length.ToString(),
							" materials. This mesh will be ignored during the merge process."
						}));
					}
					flag = false;
				}
				if (list2[l].meshRenderer != null)
				{
					for (int m = 0; m < list2[l].meshRenderer.sharedMaterials.Length; m++)
					{
						if (list2[l].meshRenderer.sharedMaterials[m] == null)
						{
							if (this.showDebugLogs)
							{
								Debug.LogError(string.Concat(new string[]
								{
									"Material ",
									m.ToString(),
									" in Mesh Renderer present in component \"",
									list2[l].gameObject.name,
									"\" is null. For the merge process to work well, all materials must be completed. This GameObject will be ignored in the merge process."
								}));
							}
							flag = false;
						}
					}
				}
				if (list2[l].gameObject.GetComponent<CombinedMeshesManager>() != null)
				{
					if (this.showDebugLogs)
					{
						Debug.LogError("GameObject \"" + list2[l].gameObject.name + "\" is the result of a previous merge, so it will be ignored by this merge.");
					}
					flag = false;
				}
				if (flag)
				{
					list3.Add(list2[l]);
				}
			}
			return list3.ToArray();
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x00151250 File Offset: 0x0014F450
		public bool CombineMeshes()
		{
			if (this.isTargetMeshesMerged())
			{
				if (this.showDebugLogs)
				{
					Debug.Log("The Runtime Combiner \"" + base.gameObject.name + "\" meshes are already combined!");
				}
				return true;
			}
			if (this.isTargetMeshesMerged())
			{
				return false;
			}
			if (base.gameObject.GetComponent<MeshFilter>() != null || base.gameObject.GetComponent<MeshRenderer>() != null)
			{
				if (this.showDebugLogs)
				{
					Debug.LogError("Unable to merge. Apparently the GameObject \"" + base.gameObject.name + "\" already contains the Mesh Filter and/or Mesh Renderer component. The Runtime Mesh Combiner needs a GameObject that does not contain these two components. Please remove them or place the Runtime Mesh Combiner in a new GameObject and try again.");
				}
				return false;
			}
			this.originalPosition = base.gameObject.transform.position;
			this.originalEulerAngles = base.gameObject.transform.eulerAngles;
			this.originalScale = base.gameObject.transform.lossyScale;
			base.gameObject.transform.position = Vector3.zero;
			base.gameObject.transform.eulerAngles = Vector3.zero;
			base.gameObject.transform.localScale = Vector3.one;
			RuntimeMeshCombiner.GameObjectWithMesh[] validatedTargetGameObjects = this.GetValidatedTargetGameObjects();
			if (validatedTargetGameObjects.Length == 0)
			{
				if (this.showDebugLogs)
				{
					Debug.LogError("No valid, meshed GameObjects were found in the target GameObjects list. Therefore the merge was interrupted.");
				}
				return false;
			}
			Dictionary<Material, List<RuntimeMeshCombiner.SubMeshToCombine>> dictionary = new Dictionary<Material, List<RuntimeMeshCombiner.SubMeshToCombine>>();
			foreach (RuntimeMeshCombiner.GameObjectWithMesh gameObjectWithMesh in validatedTargetGameObjects)
			{
				for (int j = 0; j < gameObjectWithMesh.meshFilter.sharedMesh.subMeshCount; j++)
				{
					Material key = gameObjectWithMesh.meshRenderer.sharedMaterials[j];
					if (dictionary.ContainsKey(key))
					{
						dictionary[key].Add(new RuntimeMeshCombiner.SubMeshToCombine(gameObjectWithMesh.gameObject.transform, gameObjectWithMesh.meshFilter, gameObjectWithMesh.meshRenderer, j));
					}
					if (!dictionary.ContainsKey(key))
					{
						dictionary.Add(key, new List<RuntimeMeshCombiner.SubMeshToCombine>
						{
							new RuntimeMeshCombiner.SubMeshToCombine(gameObjectWithMesh.gameObject.transform, gameObjectWithMesh.meshFilter, gameObjectWithMesh.meshRenderer, j)
						});
					}
				}
			}
			MeshFilter meshFilter = base.gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
			int num = 0;
			foreach (RuntimeMeshCombiner.GameObjectWithMesh gameObjectWithMesh2 in validatedTargetGameObjects)
			{
				num += gameObjectWithMesh2.meshFilter.sharedMesh.vertexCount;
			}
			List<Mesh> list = new List<Mesh>();
			foreach (KeyValuePair<Material, List<RuntimeMeshCombiner.SubMeshToCombine>> keyValuePair in dictionary)
			{
				List<RuntimeMeshCombiner.SubMeshToCombine> value = keyValuePair.Value;
				List<CombineInstance> list2 = new List<CombineInstance>();
				for (int l = 0; l < value.Count; l++)
				{
					list2.Add(new CombineInstance
					{
						mesh = value[l].meshFilter.sharedMesh,
						subMeshIndex = value[l].subMeshIndex,
						transform = value[l].transform.localToWorldMatrix
					});
				}
				Mesh mesh = new Mesh();
				if (num <= this.MAX_VERTICES_FOR_16BITS_MESH)
				{
					mesh.indexFormat = IndexFormat.UInt16;
				}
				if (num > this.MAX_VERTICES_FOR_16BITS_MESH)
				{
					mesh.indexFormat = IndexFormat.UInt32;
				}
				mesh.CombineMeshes(list2.ToArray(), true, true);
				list.Add(mesh);
			}
			List<CombineInstance> list3 = new List<CombineInstance>();
			foreach (Mesh mesh2 in list)
			{
				list3.Add(new CombineInstance
				{
					mesh = mesh2,
					subMeshIndex = 0,
					transform = Matrix4x4.identity
				});
			}
			Mesh mesh3 = new Mesh();
			if (num <= this.MAX_VERTICES_FOR_16BITS_MESH)
			{
				mesh3.indexFormat = IndexFormat.UInt16;
			}
			if (num > this.MAX_VERTICES_FOR_16BITS_MESH)
			{
				mesh3.indexFormat = IndexFormat.UInt32;
			}
			mesh3.name = base.gameObject.name + " (Temp Merge)";
			mesh3.CombineMeshes(list3.ToArray(), false);
			mesh3.RecalculateBounds();
			if (this.recalculateNormals)
			{
				mesh3.RecalculateNormals();
			}
			if (this.recalculateTangents)
			{
				mesh3.RecalculateTangents();
			}
			if (this.optimizeResultingMesh)
			{
				mesh3.Optimize();
			}
			meshFilter.sharedMesh = mesh3;
			List<Material> list4 = new List<Material>();
			foreach (KeyValuePair<Material, List<RuntimeMeshCombiner.SubMeshToCombine>> keyValuePair2 in dictionary)
			{
				list4.Add(keyValuePair2.Key);
			}
			meshRenderer.sharedMaterials = list4.ToArray();
			if (this.afterMerge == RuntimeMeshCombiner.AfterMerge.DeactiveOriginalGameObjects)
			{
				foreach (RuntimeMeshCombiner.GameObjectWithMesh gameObjectWithMesh3 in validatedTargetGameObjects)
				{
					this.originalGameObjectsWithMeshToRestore.Add(new RuntimeMeshCombiner.OriginalGameObjectWithMesh(gameObjectWithMesh3.gameObject, gameObjectWithMesh3.gameObject.activeSelf, gameObjectWithMesh3.meshRenderer, gameObjectWithMesh3.meshRenderer.enabled));
					gameObjectWithMesh3.gameObject.SetActive(false);
				}
				if (this.addMeshColliderAfter)
				{
					base.gameObject.AddComponent<MeshCollider>();
				}
			}
			if (this.afterMerge == RuntimeMeshCombiner.AfterMerge.DisableOriginalMeshes)
			{
				foreach (RuntimeMeshCombiner.GameObjectWithMesh gameObjectWithMesh4 in validatedTargetGameObjects)
				{
					this.originalGameObjectsWithMeshToRestore.Add(new RuntimeMeshCombiner.OriginalGameObjectWithMesh(gameObjectWithMesh4.gameObject, gameObjectWithMesh4.gameObject.activeSelf, gameObjectWithMesh4.meshRenderer, gameObjectWithMesh4.meshRenderer.enabled));
					gameObjectWithMesh4.meshRenderer.enabled = false;
				}
			}
			RuntimeMeshCombiner.AfterMerge afterMerge = this.afterMerge;
			base.gameObject.transform.position = this.originalPosition;
			base.gameObject.transform.eulerAngles = this.originalEulerAngles;
			base.gameObject.transform.localScale = this.originalScale;
			if (this.showDebugLogs)
			{
				Debug.Log("The merge has been successfully completed in Runtime Combiner \"" + base.gameObject.name + "\"!");
			}
			if (this.onDoneMerge != null)
			{
				this.onDoneMerge.Invoke();
			}
			this.targetMeshesMerged = true;
			return true;
		}

		// Token: 0x06004701 RID: 18177 RVA: 0x00151884 File Offset: 0x0014FA84
		public bool UndoMerge()
		{
			if (!this.isTargetMeshesMerged())
			{
				if (this.showDebugLogs)
				{
					Debug.Log("The Runtime Combiner \"" + base.gameObject.name + "\" meshes are already uncombined!");
				}
				return true;
			}
			if (this.isTargetMeshesMerged())
			{
				if (this.afterMerge == RuntimeMeshCombiner.AfterMerge.DisableOriginalMeshes)
				{
					foreach (RuntimeMeshCombiner.OriginalGameObjectWithMesh originalGameObjectWithMesh in this.originalGameObjectsWithMeshToRestore)
					{
						if (!(originalGameObjectWithMesh.meshRenderer == null))
						{
							originalGameObjectWithMesh.meshRenderer.enabled = originalGameObjectWithMesh.originalMrState;
						}
					}
				}
				if (this.afterMerge == RuntimeMeshCombiner.AfterMerge.DeactiveOriginalGameObjects)
				{
					foreach (RuntimeMeshCombiner.OriginalGameObjectWithMesh originalGameObjectWithMesh2 in this.originalGameObjectsWithMeshToRestore)
					{
						if (!(originalGameObjectWithMesh2.gameObject == null))
						{
							originalGameObjectWithMesh2.gameObject.SetActive(originalGameObjectWithMesh2.originalGoState);
						}
					}
					if (this.addMeshColliderAfter)
					{
						MeshCollider component = base.GetComponent<MeshCollider>();
						if (component != null)
						{
							Object.Destroy(component);
						}
					}
				}
				RuntimeMeshCombiner.AfterMerge afterMerge = this.afterMerge;
				this.originalGameObjectsWithMeshToRestore.Clear();
				Object.Destroy(base.GetComponent<MeshRenderer>());
				Object.Destroy(base.GetComponent<MeshFilter>());
				if (this.garbageCollectorAfterUndo)
				{
					Resources.UnloadUnusedAssets();
					GC.Collect();
				}
				if (this.showDebugLogs)
				{
					Debug.Log("The Runtime Combiner \"" + base.gameObject.name + "\" merge was successfully undone!");
				}
				if (this.onDoneUnmerge != null)
				{
					this.onDoneUnmerge.Invoke();
				}
				this.targetMeshesMerged = false;
				return true;
			}
			return false;
		}

		// Token: 0x06004702 RID: 18178 RVA: 0x00151A38 File Offset: 0x0014FC38
		public bool isTargetMeshesMerged()
		{
			return this.targetMeshesMerged;
		}

		// Token: 0x04004865 RID: 18533
		private int MAX_VERTICES_FOR_16BITS_MESH = 50000;

		// Token: 0x04004866 RID: 18534
		private Vector3 originalPosition = Vector3.zero;

		// Token: 0x04004867 RID: 18535
		private Vector3 originalEulerAngles = Vector3.zero;

		// Token: 0x04004868 RID: 18536
		private Vector3 originalScale = Vector3.zero;

		// Token: 0x04004869 RID: 18537
		private List<RuntimeMeshCombiner.OriginalGameObjectWithMesh> originalGameObjectsWithMeshToRestore = new List<RuntimeMeshCombiner.OriginalGameObjectWithMesh>();

		// Token: 0x0400486A RID: 18538
		private bool targetMeshesMerged;

		// Token: 0x0400486B RID: 18539
		[HideInInspector]
		public RuntimeMeshCombiner.AfterMerge afterMerge;

		// Token: 0x0400486C RID: 18540
		[HideInInspector]
		public bool addMeshColliderAfter = true;

		// Token: 0x0400486D RID: 18541
		[HideInInspector]
		public RuntimeMeshCombiner.CombineOnStart combineMeshesAtStartUp;

		// Token: 0x0400486E RID: 18542
		[HideInInspector]
		public bool combineInChildren;

		// Token: 0x0400486F RID: 18543
		[HideInInspector]
		public bool combineInactives;

		// Token: 0x04004870 RID: 18544
		[HideInInspector]
		public bool recalculateNormals = true;

		// Token: 0x04004871 RID: 18545
		[HideInInspector]
		public bool recalculateTangents = true;

		// Token: 0x04004872 RID: 18546
		[HideInInspector]
		public bool optimizeResultingMesh;

		// Token: 0x04004873 RID: 18547
		[HideInInspector]
		public List<GameObject> targetMeshes = new List<GameObject>();

		// Token: 0x04004874 RID: 18548
		[HideInInspector]
		public bool showDebugLogs = true;

		// Token: 0x04004875 RID: 18549
		[HideInInspector]
		public bool garbageCollectorAfterUndo = true;

		// Token: 0x04004876 RID: 18550
		public UnityEvent onDoneMerge;

		// Token: 0x04004877 RID: 18551
		public UnityEvent onDoneUnmerge;

		// Token: 0x02000B1C RID: 2844
		private class GameObjectWithMesh
		{
			// Token: 0x06004704 RID: 18180 RVA: 0x00151AB8 File Offset: 0x0014FCB8
			public GameObjectWithMesh(GameObject gameObject, MeshFilter meshFilter, MeshRenderer meshRenderer)
			{
				this.gameObject = gameObject;
				this.meshFilter = meshFilter;
				this.meshRenderer = meshRenderer;
			}

			// Token: 0x04004878 RID: 18552
			public GameObject gameObject;

			// Token: 0x04004879 RID: 18553
			public MeshFilter meshFilter;

			// Token: 0x0400487A RID: 18554
			public MeshRenderer meshRenderer;
		}

		// Token: 0x02000B1D RID: 2845
		private class OriginalGameObjectWithMesh
		{
			// Token: 0x06004705 RID: 18181 RVA: 0x00151AD5 File Offset: 0x0014FCD5
			public OriginalGameObjectWithMesh(GameObject gameObject, bool originalGoState, MeshRenderer meshRenderer, bool originalMrState)
			{
				this.gameObject = gameObject;
				this.originalGoState = originalGoState;
				this.meshRenderer = meshRenderer;
				this.originalMrState = originalMrState;
			}

			// Token: 0x0400487B RID: 18555
			public GameObject gameObject;

			// Token: 0x0400487C RID: 18556
			public bool originalGoState;

			// Token: 0x0400487D RID: 18557
			public MeshRenderer meshRenderer;

			// Token: 0x0400487E RID: 18558
			public bool originalMrState;
		}

		// Token: 0x02000B1E RID: 2846
		private class SubMeshToCombine
		{
			// Token: 0x06004706 RID: 18182 RVA: 0x00151AFA File Offset: 0x0014FCFA
			public SubMeshToCombine(Transform transform, MeshFilter meshFilter, MeshRenderer meshRenderer, int subMeshIndex)
			{
				this.transform = transform;
				this.meshFilter = meshFilter;
				this.meshRenderer = meshRenderer;
				this.subMeshIndex = subMeshIndex;
			}

			// Token: 0x0400487F RID: 18559
			public Transform transform;

			// Token: 0x04004880 RID: 18560
			public MeshFilter meshFilter;

			// Token: 0x04004881 RID: 18561
			public MeshRenderer meshRenderer;

			// Token: 0x04004882 RID: 18562
			public int subMeshIndex;
		}

		// Token: 0x02000B1F RID: 2847
		public enum CombineOnStart
		{
			// Token: 0x04004884 RID: 18564
			Disabled,
			// Token: 0x04004885 RID: 18565
			OnStart,
			// Token: 0x04004886 RID: 18566
			OnAwake
		}

		// Token: 0x02000B20 RID: 2848
		public enum AfterMerge
		{
			// Token: 0x04004888 RID: 18568
			DisableOriginalMeshes,
			// Token: 0x04004889 RID: 18569
			DeactiveOriginalGameObjects,
			// Token: 0x0400488A RID: 18570
			DoNothing
		}
	}
}
