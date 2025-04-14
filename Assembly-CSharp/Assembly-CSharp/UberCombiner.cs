using System;
using System.Collections.Generic;
using System.Linq;
using GorillaTag.Rendering;
using MTAssets.EasyMeshCombiner;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

// Token: 0x020008D3 RID: 2259
[RequireComponent(typeof(RuntimeMeshCombiner))]
public class UberCombiner : MonoBehaviour
{
	// Token: 0x0600368F RID: 13967 RVA: 0x00102100 File Offset: 0x00100300
	private void CollectRenderers()
	{
		MeshRenderer[] array = UberCombiner.FilterRenderers(this.meshSources.SelectMany((GameObject g) => g.GetComponentsInChildren<MeshRenderer>(this.includeInactive)).ToArray<MeshRenderer>()).DistinctBy((MeshRenderer mr) => mr.GetInstanceID()).ToArray<MeshRenderer>();
		this.renderersToCombine = array;
		string.Format("Found {0} renderers to combine.", array.Length).Echo<string>();
	}

	// Token: 0x06003690 RID: 13968 RVA: 0x00102178 File Offset: 0x00100378
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

	// Token: 0x06003691 RID: 13969 RVA: 0x00102474 File Offset: 0x00100674
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

	// Token: 0x06003692 RID: 13970 RVA: 0x00102527 File Offset: 0x00100727
	private void MergeMeshes()
	{
		this._combiner.CombineMeshes();
	}

	// Token: 0x06003693 RID: 13971 RVA: 0x00102535 File Offset: 0x00100735
	private void UndoMerge()
	{
		this._combiner.UndoMerge();
	}

	// Token: 0x06003694 RID: 13972 RVA: 0x00102543 File Offset: 0x00100743
	private void MergeAndExtractPerMaterialMeshes()
	{
		this._combiner.onDoneMerge.AddListener(new UnityAction(this.OnPostMerge));
		this._combiner.CombineMeshes();
	}

	// Token: 0x06003695 RID: 13973 RVA: 0x0010256D File Offset: 0x0010076D
	private void QuickMerge()
	{
		this.CollectRenderers();
		this.ValidateRenderers();
		this.SendToCombiner();
		this.MergeAndExtractPerMaterialMeshes();
	}

	// Token: 0x06003696 RID: 13974 RVA: 0x00102588 File Offset: 0x00100788
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

	// Token: 0x06003697 RID: 13975 RVA: 0x001026E0 File Offset: 0x001008E0
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

	// Token: 0x06003698 RID: 13976 RVA: 0x0010276A File Offset: 0x0010096A
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

	// Token: 0x040038E1 RID: 14561
	[SerializeField]
	private RuntimeMeshCombiner _combiner;

	// Token: 0x040038E2 RID: 14562
	[Space]
	public GameObject[] meshSources = new GameObject[0];

	// Token: 0x040038E3 RID: 14563
	[Space]
	public GameObject[] objectsToIgnore = new GameObject[0];

	// Token: 0x040038E4 RID: 14564
	[Space]
	[NonSerialized]
	private MeshRenderer[] renderersToCombine = new MeshRenderer[0];

	// Token: 0x040038E5 RID: 14565
	[Space]
	[NonSerialized]
	private List<GameObject> invalidObjects = new List<GameObject>();

	// Token: 0x040038E6 RID: 14566
	public bool includeInactive;

	// Token: 0x040038E7 RID: 14567
	private static ShaderHashId _BaseMap = "_BaseMap";
}
