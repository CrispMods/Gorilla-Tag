using System;
using System.Collections.Generic;
using System.Linq;
using GorillaTag.Rendering;
using MTAssets.EasyMeshCombiner;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

// Token: 0x020008EC RID: 2284
[RequireComponent(typeof(RuntimeMeshCombiner))]
public class UberCombiner : MonoBehaviour
{
	// Token: 0x0600374B RID: 14155 RVA: 0x00146FFC File Offset: 0x001451FC
	private void CollectRenderers()
	{
		MeshRenderer[] array = UberCombiner.FilterRenderers(this.meshSources.SelectMany((GameObject g) => g.GetComponentsInChildren<MeshRenderer>(this.includeInactive)).ToArray<MeshRenderer>()).DistinctBy((MeshRenderer mr) => mr.GetInstanceID()).ToArray<MeshRenderer>();
		this.renderersToCombine = array;
		string.Format("Found {0} renderers to combine.", array.Length).Echo<string>();
	}

	// Token: 0x0600374C RID: 14156 RVA: 0x00147074 File Offset: 0x00145274
	private void ValidateRenderers()
	{
		List<GameObject> list = new List<GameObject>(16);
		for (int i = 0; i < this.renderersToCombine.Length; i++)
		{
			MeshRenderer meshRenderer = this.renderersToCombine[i];
			GameObject gameObject = meshRenderer.gameObject;
			string name = gameObject.name;
			MeshFilter component = gameObject.GetComponent<MeshFilter>();
			if (meshRenderer == null || component == null)
			{
				Debug.LogError("Ojbect '" + name + "' is missing a MeshRenderer, MeshFilter, or both.", gameObject);
				list.Add(gameObject);
			}
			else
			{
				Mesh sharedMesh = component.sharedMesh;
				if (sharedMesh == null)
				{
					Debug.LogError("MeshFilter for '" + name + "' has no shared mesh.", gameObject);
					list.Add(gameObject);
				}
				else
				{
					int subMeshCount = sharedMesh.subMeshCount;
					if (subMeshCount == 0)
					{
						Debug.LogError("Shared mesh for '" + name + "' has 0 submeshes.", gameObject);
						list.Add(gameObject);
					}
					else if (sharedMesh.vertexCount < 3)
					{
						Debug.LogError("Shared mesh for '" + name + "' has less than 3 vertices.", gameObject);
						list.Add(gameObject);
					}
					else
					{
						Material[] sharedMaterials = meshRenderer.sharedMaterials;
						if (sharedMaterials.IsNullOrEmpty<Material>())
						{
							Debug.LogError("Object '" + name + "' has null or empty shared materials array.", gameObject);
							list.Add(gameObject);
						}
						else
						{
							foreach (Material material in sharedMaterials)
							{
								string name2 = material.name;
								Texture mainTexture = material.mainTexture;
								if (!(mainTexture == null) && mainTexture is RenderTexture)
								{
									Debug.LogError(string.Concat(new string[]
									{
										"Object '",
										name,
										"' has material (",
										name2,
										") that uses a RenderTexture"
									}), gameObject);
									list.Add(gameObject);
									break;
								}
								if (material.HasProperty(UberCombiner._BaseMap))
								{
									Texture texture = material.GetTexture(UberCombiner._BaseMap);
									if (!(texture == null) && texture is RenderTexture)
									{
										Debug.LogError(string.Concat(new string[]
										{
											"Object '",
											name,
											"' has material (",
											name2,
											") that uses a RenderTexture"
										}), gameObject);
										list.Add(gameObject);
										break;
									}
								}
								if (UberShader.IsAnimated(material))
								{
									Debug.LogError(string.Concat(new string[]
									{
										"Object '",
										name,
										"' has a material (",
										name2,
										") that's animated"
									}), gameObject);
									list.Add(gameObject);
									break;
								}
							}
							if (subMeshCount != sharedMaterials.Length)
							{
								Debug.LogError("Object '" + name + "' has mismatched number of materials/submeshes" + string.Format(" Submeshes: {0} Materials: {1}", subMeshCount, sharedMaterials.Length), gameObject);
								list.Add(gameObject);
							}
						}
					}
				}
			}
		}
		this.invalidObjects = list.DistinctBy((GameObject g) => g.GetHashCode()).ToList<GameObject>();
	}

	// Token: 0x0600374D RID: 14157 RVA: 0x00147370 File Offset: 0x00145570
	private void SendToCombiner()
	{
		List<GameObject> targetMeshes = (from r in this.renderersToCombine
		select r.gameObject into g
		where !(g == null)
		where !this.objectsToIgnore.Contains(g)
		where !this.invalidObjects.Contains(g)
		select g).DistinctBy((GameObject g) => g.GetInstanceID()).ToList<GameObject>();
		this._combiner.targetMeshes = targetMeshes;
	}

	// Token: 0x0600374E RID: 14158 RVA: 0x00054852 File Offset: 0x00052A52
	private void MergeMeshes()
	{
		this._combiner.CombineMeshes();
	}

	// Token: 0x0600374F RID: 14159 RVA: 0x00054860 File Offset: 0x00052A60
	private void UndoMerge()
	{
		this._combiner.UndoMerge();
	}

	// Token: 0x06003750 RID: 14160 RVA: 0x0005486E File Offset: 0x00052A6E
	private void MergeAndExtractPerMaterialMeshes()
	{
		this._combiner.onDoneMerge.AddListener(new UnityAction(this.OnPostMerge));
		this._combiner.CombineMeshes();
	}

	// Token: 0x06003751 RID: 14161 RVA: 0x00054898 File Offset: 0x00052A98
	private void QuickMerge()
	{
		this.CollectRenderers();
		this.ValidateRenderers();
		this.SendToCombiner();
		this.MergeAndExtractPerMaterialMeshes();
	}

	// Token: 0x06003752 RID: 14162 RVA: 0x00147424 File Offset: 0x00145624
	private void OnPostMerge()
	{
		MeshFilter component = base.GetComponent<MeshFilter>();
		MeshRenderer component2 = base.GetComponent<MeshRenderer>();
		Mesh sharedMesh = component.sharedMesh;
		int subMeshCount = sharedMesh.subMeshCount;
		string name = component2.name;
		Material[] sharedMaterials = component2.sharedMaterials;
		GameObject gameObject = new GameObject(name + "_PerMaterialMeshes");
		UberCombinerPerMaterialMeshes uberCombinerPerMaterialMeshes;
		this.GetOrAddComponent(out uberCombinerPerMaterialMeshes);
		uberCombinerPerMaterialMeshes.rootObject = gameObject;
		uberCombinerPerMaterialMeshes.objects = new GameObject[subMeshCount];
		uberCombinerPerMaterialMeshes.filters = new MeshFilter[subMeshCount];
		uberCombinerPerMaterialMeshes.renderers = new MeshRenderer[subMeshCount];
		uberCombinerPerMaterialMeshes.materials = new Material[subMeshCount];
		GTMeshData gtmeshData = GTMeshData.Parse(sharedMesh);
		for (int i = 0; i < subMeshCount; i++)
		{
			GameObject gameObject2 = new GameObject(string.Format("{0}_{1}", i, sharedMaterials[i].name));
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.isStatic = true;
			MeshFilter meshFilter = gameObject2.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject2.AddComponent<MeshRenderer>();
			Mesh sharedMesh2 = gtmeshData.ExtractSubmesh(i, false);
			meshFilter.sharedMesh = sharedMesh2;
			meshRenderer.sharedMaterial = sharedMaterials[i];
			meshRenderer.lightProbeUsage = LightProbeUsage.Off;
			meshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
			uberCombinerPerMaterialMeshes.objects[i] = gameObject2;
			uberCombinerPerMaterialMeshes.filters[i] = meshFilter;
			uberCombinerPerMaterialMeshes.renderers[i] = meshRenderer;
			uberCombinerPerMaterialMeshes.materials[i] = sharedMaterials[i];
		}
	}

	// Token: 0x06003753 RID: 14163 RVA: 0x0014757C File Offset: 0x0014577C
	private void OnValidate()
	{
		if (!base.transform.position.Approx0(1E-05f))
		{
			base.transform.position = Vector3.zero;
		}
		if (this._combiner == null)
		{
			this._combiner = base.GetComponent<RuntimeMeshCombiner>();
			this._combiner.recalculateNormals = false;
			this._combiner.recalculateTangents = false;
			this._combiner.combineInactives = false;
			this._combiner.garbageCollectorAfterUndo = true;
			this._combiner.afterMerge = RuntimeMeshCombiner.AfterMerge.DoNothing;
		}
	}

	// Token: 0x06003754 RID: 14164 RVA: 0x000548B2 File Offset: 0x00052AB2
	private static IEnumerable<MeshRenderer> FilterRenderers(IList<MeshRenderer> renderers)
	{
		Shader uberShader = UberShader.ReferenceShader;
		Shader uberShaderNonSRP = UberShader.ReferenceShaderNonSRP;
		RenderQueueRange transQueue = RenderQueueRange.transparent;
		int num;
		for (int i = 0; i < renderers.Count; i = num)
		{
			MeshRenderer mr = renderers[i];
			if (!(mr == null) && mr.enabled && mr.gameObject.isStatic && !mr.GetComponent<EdDoNotMeshCombine>())
			{
				MeshFilter component = mr.GetComponent<MeshFilter>();
				if (!(component == null))
				{
					Mesh sharedMesh = component.sharedMesh;
					if (!(sharedMesh == null) && sharedMesh.vertexCount >= 3)
					{
						Material[] sharedMats = mr.sharedMaterials;
						if (!sharedMats.IsNullOrEmpty<Material>())
						{
							for (int j = 0; j < sharedMats.Length; j = num)
							{
								Material material = sharedMats[j];
								if (!(material == null))
								{
									int renderQueue = material.renderQueue;
									if ((renderQueue < transQueue.lowerBound || renderQueue > transQueue.upperBound) && (renderQueue < 2450 || renderQueue > 2500))
									{
										Shader shader = material.shader;
										if (shader == uberShader)
										{
											yield return mr;
										}
										else if (shader == uberShaderNonSRP)
										{
											yield return mr;
										}
									}
								}
								num = j + 1;
							}
							mr = null;
							sharedMats = null;
						}
					}
				}
			}
			num = i + 1;
		}
		yield break;
	}

	// Token: 0x04003990 RID: 14736
	[SerializeField]
	private RuntimeMeshCombiner _combiner;

	// Token: 0x04003991 RID: 14737
	[Space]
	public GameObject[] meshSources = new GameObject[0];

	// Token: 0x04003992 RID: 14738
	[Space]
	public GameObject[] objectsToIgnore = new GameObject[0];

	// Token: 0x04003993 RID: 14739
	[Space]
	[NonSerialized]
	private MeshRenderer[] renderersToCombine = new MeshRenderer[0];

	// Token: 0x04003994 RID: 14740
	[Space]
	[NonSerialized]
	private List<GameObject> invalidObjects = new List<GameObject>();

	// Token: 0x04003995 RID: 14741
	public bool includeInactive;

	// Token: 0x04003996 RID: 14742
	private static ShaderHashId _BaseMap = "_BaseMap";
}
