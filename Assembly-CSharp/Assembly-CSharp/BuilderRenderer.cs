using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Rendering;

// Token: 0x020004DE RID: 1246
public class BuilderRenderer : MonoBehaviour
{
	// Token: 0x06001E47 RID: 7751 RVA: 0x000970C7 File Offset: 0x000952C7
	private void Awake()
	{
		this.InitIfNeeded();
	}

	// Token: 0x06001E48 RID: 7752 RVA: 0x000970D0 File Offset: 0x000952D0
	public void InitIfNeeded()
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		this.renderData = new BuilderTableDataRenderData();
		this.renderData.materialToIndex = new Dictionary<Material, int>(256);
		this.renderData.materials = new List<Material>(256);
		this.renderData.meshToIndex = new Dictionary<Mesh, int>(1024);
		this.renderData.meshInstanceCount = new List<int>(1024);
		this.renderData.meshes = new List<Mesh>(4096);
		this.renderData.textureToIndex = new Dictionary<Texture2D, int>(256);
		this.renderData.textures = new List<Texture2D>(256);
		this.renderData.perTextureMaterial = new List<Material>(256);
		this.renderData.perTexturePropertyBlock = new List<MaterialPropertyBlock>(256);
		this.renderData.sharedMaterial = new Material(this.sharedMaterialBase);
		this.renderData.sharedMaterialIndirect = new Material(this.sharedMaterialIndirectBase);
		this.built = false;
		this.showing = false;
	}

	// Token: 0x06001E49 RID: 7753 RVA: 0x000971EF File Offset: 0x000953EF
	public void Show(bool show)
	{
		this.showing = show;
	}

	// Token: 0x06001E4A RID: 7754 RVA: 0x000971F8 File Offset: 0x000953F8
	public void BuildRenderer(List<BuilderPiece> piecePrefabs)
	{
		this.InitIfNeeded();
		for (int i = 0; i < piecePrefabs.Count; i++)
		{
			if (piecePrefabs[i] != null)
			{
				this.AddPrefab(piecePrefabs[i]);
			}
			else
			{
				Debug.LogErrorFormat("Prefab at {0} is null", new object[]
				{
					i
				});
			}
		}
		this.BuildSharedMaterial();
		this.BuildSharedMesh();
		this.BuildBuffer();
		this.built = true;
	}

	// Token: 0x06001E4B RID: 7755 RVA: 0x0009726C File Offset: 0x0009546C
	public void LogDraws()
	{
		Debug.LogFormat("Builder Renderer Counts {0} {1} {2} {3}", new object[]
		{
			this.renderData.meshes.Count,
			this.renderData.textures.Count,
			this.renderData.dynamicBatch.totalInstances,
			this.renderData.staticBatch.totalInstances
		});
	}

	// Token: 0x06001E4C RID: 7756 RVA: 0x000972E9 File Offset: 0x000954E9
	public void LateUpdate()
	{
		if (!this.built || !this.showing)
		{
			return;
		}
		this.RenderIndirect();
	}

	// Token: 0x06001E4D RID: 7757 RVA: 0x00097304 File Offset: 0x00095504
	public void AddPrefab(BuilderPiece prefab)
	{
		BuilderRenderer.meshRenderers.Clear();
		prefab.GetComponentsInChildren<MeshRenderer>(true, BuilderRenderer.meshRenderers);
		for (int i = 0; i < BuilderRenderer.meshRenderers.Count; i++)
		{
			MeshRenderer meshRenderer = BuilderRenderer.meshRenderers[i];
			Material sharedMaterial = meshRenderer.sharedMaterial;
			if (sharedMaterial == null)
			{
				if (!prefab.suppressMaterialWarnings)
				{
					Debug.LogErrorFormat("{0} {1} is missing a buidler material", new object[]
					{
						prefab.name,
						meshRenderer.name
					});
				}
			}
			else if (!this.AddMaterial(sharedMaterial, prefab.suppressMaterialWarnings))
			{
				if (!prefab.suppressMaterialWarnings)
				{
					Debug.LogWarningFormat("{0} {1} failed to add builder material", new object[]
					{
						prefab.name,
						meshRenderer.name
					});
				}
			}
			else
			{
				MeshFilter component = meshRenderer.GetComponent<MeshFilter>();
				if (component != null)
				{
					Mesh sharedMesh = component.sharedMesh;
					int num;
					if (sharedMesh != null && !this.renderData.meshToIndex.TryGetValue(sharedMesh, out num))
					{
						this.renderData.meshToIndex.Add(sharedMesh, this.renderData.meshToIndex.Count);
						this.renderData.meshInstanceCount.Add(0);
						for (int j = 0; j < 1; j++)
						{
							this.renderData.meshes.Add(sharedMesh);
						}
					}
				}
			}
		}
		if (prefab.materialOptions != null)
		{
			for (int k = 0; k < prefab.materialOptions.options.Count; k++)
			{
				Material material = prefab.materialOptions.options[k].material;
				if (!this.AddMaterial(material, prefab.suppressMaterialWarnings) && !prefab.suppressMaterialWarnings)
				{
					Debug.LogWarningFormat("builder material options {0} bad material index {1}", new object[]
					{
						prefab.materialOptions.name,
						k
					});
				}
			}
		}
	}

	// Token: 0x06001E4E RID: 7758 RVA: 0x000974E4 File Offset: 0x000956E4
	private bool AddMaterial(Material material, bool suppressWarnings = false)
	{
		if (material == null)
		{
			return false;
		}
		if (!material.HasTexture("_BaseMap"))
		{
			if (!suppressWarnings)
			{
				Debug.LogWarningFormat("builder material {0} does not have texture property {1}", new object[]
				{
					material.name,
					"_BaseMap"
				});
			}
			return false;
		}
		Texture texture = material.GetTexture("_BaseMap");
		if (texture == null)
		{
			if (!suppressWarnings)
			{
				Debug.LogWarningFormat("builder material {0} null texture", new object[]
				{
					material.name
				});
			}
			return false;
		}
		Texture2D texture2D = texture as Texture2D;
		if (texture2D == null)
		{
			if (!suppressWarnings)
			{
				Debug.LogWarningFormat("builder material {0} no texture2d type is {1}", new object[]
				{
					material.name,
					texture.GetType()
				});
			}
			return false;
		}
		int num;
		if (!this.renderData.materialToIndex.TryGetValue(material, out num))
		{
			this.renderData.materialToIndex.Add(material, this.renderData.materials.Count);
			this.renderData.materials.Add(material);
		}
		int num2;
		if (!this.renderData.textureToIndex.TryGetValue(texture2D, out num2))
		{
			this.renderData.textureToIndex.Add(texture2D, this.renderData.textures.Count);
			this.renderData.textures.Add(texture2D);
			if (this.renderData.textures.Count == 1)
			{
				this.renderData.textureFormat = texture2D.format;
				this.renderData.texWidth = texture2D.width;
				this.renderData.texHeight = texture2D.height;
			}
		}
		return true;
	}

	// Token: 0x06001E4F RID: 7759 RVA: 0x00097674 File Offset: 0x00095874
	public void BuildSharedMaterial()
	{
		TextureFormat textureFormat = TextureFormat.RGBA32;
		this.renderData.sharedTexArray = new Texture2DArray(this.renderData.texWidth, this.renderData.texHeight, this.renderData.textures.Count, textureFormat, true);
		this.renderData.sharedTexArray.filterMode = FilterMode.Point;
		for (int i = 0; i < this.renderData.textures.Count; i++)
		{
			this.renderData.sharedTexArray.SetPixels(this.renderData.textures[i].GetPixels(), i);
		}
		this.renderData.sharedTexArray.Apply(true, true);
		this.renderData.sharedMaterial.SetTexture("_BaseMapArray", this.renderData.sharedTexArray);
		this.renderData.sharedMaterialIndirect.SetTexture("_BaseMapArray", this.renderData.sharedTexArray);
		this.renderData.sharedMaterialIndirect.enableInstancing = true;
		for (int j = 0; j < this.renderData.textures.Count; j++)
		{
			Material material = new Material(this.renderData.sharedMaterial);
			material.SetInt("_BaseMapArrayIndex", j);
			this.renderData.perTextureMaterial.Add(material);
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			materialPropertyBlock.SetInt("_BaseMapArrayIndex", j);
			this.renderData.perTexturePropertyBlock.Add(materialPropertyBlock);
		}
	}

	// Token: 0x06001E50 RID: 7760 RVA: 0x000977E0 File Offset: 0x000959E0
	public void BuildSharedMesh()
	{
		this.renderData.sharedMesh = new Mesh();
		this.renderData.sharedMesh.indexFormat = IndexFormat.UInt32;
		BuilderRenderer.verticesAll.Clear();
		BuilderRenderer.normalsAll.Clear();
		BuilderRenderer.uv1All.Clear();
		BuilderRenderer.trianglesAll.Clear();
		this.renderData.subMeshes = new NativeList<BuilderTableSubMesh>(512, Allocator.Persistent);
		for (int i = 0; i < this.renderData.meshes.Count; i++)
		{
			Mesh mesh = this.renderData.meshes[i];
			int count = BuilderRenderer.trianglesAll.Count;
			int count2 = BuilderRenderer.verticesAll.Count;
			BuilderRenderer.vertices.Clear();
			BuilderRenderer.normals.Clear();
			BuilderRenderer.uv1.Clear();
			BuilderRenderer.triangles.Clear();
			mesh.GetVertices(BuilderRenderer.vertices);
			mesh.GetNormals(BuilderRenderer.normals);
			mesh.GetUVs(0, BuilderRenderer.uv1);
			mesh.GetTriangles(BuilderRenderer.triangles, 0);
			BuilderRenderer.verticesAll.AddRange(BuilderRenderer.vertices);
			BuilderRenderer.normalsAll.AddRange(BuilderRenderer.normals);
			BuilderRenderer.uv1All.AddRange(BuilderRenderer.uv1);
			BuilderRenderer.trianglesAll.AddRange(BuilderRenderer.triangles);
			int indexCount = BuilderRenderer.trianglesAll.Count - count;
			BuilderTableSubMesh builderTableSubMesh = new BuilderTableSubMesh
			{
				startIndex = count,
				indexCount = indexCount,
				startVertex = count2
			};
			this.renderData.subMeshes.Add(builderTableSubMesh);
		}
		this.renderData.sharedMesh.SetVertices(BuilderRenderer.verticesAll);
		this.renderData.sharedMesh.SetNormals(BuilderRenderer.normalsAll);
		this.renderData.sharedMesh.SetUVs(0, BuilderRenderer.uv1All);
		this.renderData.sharedMesh.SetTriangles(BuilderRenderer.trianglesAll, 0);
	}

	// Token: 0x06001E51 RID: 7761 RVA: 0x000979C4 File Offset: 0x00095BC4
	public void BuildBuffer()
	{
		this.renderData.dynamicBatch = new BuilderTableDataRenderIndirectBatch();
		BuilderRenderer.BuildBatch(this.renderData.dynamicBatch, this.renderData.meshes.Count, 8192, this.renderData.sharedMaterialIndirect);
		this.renderData.staticBatch = new BuilderTableDataRenderIndirectBatch();
		BuilderRenderer.BuildBatch(this.renderData.staticBatch, this.renderData.meshes.Count, 8192, this.renderData.sharedMaterialIndirect);
	}

	// Token: 0x06001E52 RID: 7762 RVA: 0x00097A54 File Offset: 0x00095C54
	public static void BuildBatch(BuilderTableDataRenderIndirectBatch indirectBatch, int meshCount, int maxInstances, Material sharedMaterialIndirect)
	{
		indirectBatch.totalInstances = 0;
		indirectBatch.commandCount = meshCount;
		indirectBatch.commandBuf = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, indirectBatch.commandCount, 20);
		indirectBatch.commandData = new NativeArray<GraphicsBuffer.IndirectDrawIndexedArgs>(indirectBatch.commandCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		indirectBatch.matrixBuf = new GraphicsBuffer(GraphicsBuffer.Target.Structured, maxInstances, 64);
		indirectBatch.texIndexBuf = new GraphicsBuffer(GraphicsBuffer.Target.Structured, maxInstances, 4);
		indirectBatch.tintBuf = new GraphicsBuffer(GraphicsBuffer.Target.Structured, maxInstances, 4);
		indirectBatch.instanceTransform = new TransformAccessArray(maxInstances, 3);
		for (int i = 0; i < maxInstances; i++)
		{
			indirectBatch.instanceTransform.Add(null);
		}
		indirectBatch.instanceTransformIndexToDataIndex = new NativeArray<int>(maxInstances, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		for (int j = 0; j < maxInstances; j++)
		{
			indirectBatch.instanceTransformIndexToDataIndex[j] = -1;
		}
		indirectBatch.instanceObjectToWorld = new NativeArray<Matrix4x4>(maxInstances, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		indirectBatch.instanceTexIndex = new NativeArray<int>(maxInstances, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		indirectBatch.instanceTint = new NativeArray<float>(maxInstances, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		indirectBatch.renderMeshes = new NativeList<BuilderTableMeshInstances>(512, Allocator.Persistent);
		for (int k = 0; k < meshCount; k++)
		{
			BuilderTableMeshInstances builderTableMeshInstances = new BuilderTableMeshInstances
			{
				transforms = new TransformAccessArray(maxInstances, 3),
				texIndex = new NativeList<int>(Allocator.Persistent),
				tint = new NativeList<float>(Allocator.Persistent)
			};
			indirectBatch.renderMeshes.Add(builderTableMeshInstances);
		}
		indirectBatch.rp = new RenderParams(sharedMaterialIndirect);
		indirectBatch.rp.worldBounds = new Bounds(Vector3.zero, 10000f * Vector3.one);
		indirectBatch.rp.matProps = new MaterialPropertyBlock();
		indirectBatch.rp.matProps.SetMatrix("_ObjectToWorld", Matrix4x4.identity);
		indirectBatch.matrixBuf.SetData<Matrix4x4>(indirectBatch.instanceObjectToWorld);
		indirectBatch.texIndexBuf.SetData<int>(indirectBatch.instanceTexIndex);
		indirectBatch.tintBuf.SetData<float>(indirectBatch.instanceTint);
		indirectBatch.rp.matProps.SetBuffer("_TransformMatrix", indirectBatch.matrixBuf);
		indirectBatch.rp.matProps.SetBuffer("_TexIndex", indirectBatch.texIndexBuf);
		indirectBatch.rp.matProps.SetBuffer("_Tint", indirectBatch.tintBuf);
	}

	// Token: 0x06001E53 RID: 7763 RVA: 0x00097C88 File Offset: 0x00095E88
	private void OnDestroy()
	{
		this.DestroyBuffer();
		this.renderData.subMeshes.Dispose();
	}

	// Token: 0x06001E54 RID: 7764 RVA: 0x00097CA0 File Offset: 0x00095EA0
	public void DestroyBuffer()
	{
		BuilderRenderer.DestroyBatch(this.renderData.staticBatch);
		BuilderRenderer.DestroyBatch(this.renderData.dynamicBatch);
	}

	// Token: 0x06001E55 RID: 7765 RVA: 0x00097CC4 File Offset: 0x00095EC4
	public static void DestroyBatch(BuilderTableDataRenderIndirectBatch indirectBatch)
	{
		indirectBatch.commandBuf.Dispose();
		indirectBatch.commandData.Dispose();
		indirectBatch.matrixBuf.Dispose();
		indirectBatch.texIndexBuf.Dispose();
		indirectBatch.tintBuf.Dispose();
		indirectBatch.instanceTransform.Dispose();
		indirectBatch.instanceTransformIndexToDataIndex.Dispose();
		indirectBatch.instanceObjectToWorld.Dispose();
		indirectBatch.instanceTexIndex.Dispose();
		indirectBatch.instanceTint.Dispose();
		foreach (BuilderTableMeshInstances builderTableMeshInstances in indirectBatch.renderMeshes)
		{
			TransformAccessArray transforms = builderTableMeshInstances.transforms;
			transforms.Dispose();
			NativeList<int> texIndex = builderTableMeshInstances.texIndex;
			texIndex.Dispose();
			NativeList<float> tint = builderTableMeshInstances.tint;
			tint.Dispose();
		}
		indirectBatch.renderMeshes.Dispose();
	}

	// Token: 0x06001E56 RID: 7766 RVA: 0x00097DB4 File Offset: 0x00095FB4
	public void PreRenderIndirect()
	{
		if (!this.built || !this.showing)
		{
			return;
		}
		this.renderData.setupInstancesJobs = default(JobHandle);
		BuilderRenderer.SetupIndirectBatchArgs(this.renderData.staticBatch, this.renderData.subMeshes);
		BuilderRenderer.SetupInstanceDataForMeshStatic jobData = new BuilderRenderer.SetupInstanceDataForMeshStatic
		{
			transformIndexToDataIndex = this.renderData.staticBatch.instanceTransformIndexToDataIndex,
			objectToWorld = this.renderData.staticBatch.instanceObjectToWorld
		};
		this.renderData.setupInstancesJobs = jobData.ScheduleReadOnly(this.renderData.staticBatch.instanceTransform, 32, default(JobHandle));
		JobHandle.ScheduleBatchedJobs();
	}

	// Token: 0x06001E57 RID: 7767 RVA: 0x00097E67 File Offset: 0x00096067
	public void RenderIndirect()
	{
		this.renderData.setupInstancesJobs.Complete();
		this.RenderIndirectBatch(this.renderData.staticBatch);
	}

	// Token: 0x06001E58 RID: 7768 RVA: 0x00097E8C File Offset: 0x0009608C
	private static void SetupIndirectBatchArgs(BuilderTableDataRenderIndirectBatch indirectBatch, NativeList<BuilderTableSubMesh> subMeshes)
	{
		uint num = 0U;
		for (int i = 0; i < indirectBatch.commandCount; i++)
		{
			BuilderTableMeshInstances builderTableMeshInstances = indirectBatch.renderMeshes[i];
			BuilderTableSubMesh builderTableSubMesh = subMeshes[i];
			GraphicsBuffer.IndirectDrawIndexedArgs value = default(GraphicsBuffer.IndirectDrawIndexedArgs);
			value.indexCountPerInstance = (uint)builderTableSubMesh.indexCount;
			value.startIndex = (uint)builderTableSubMesh.startIndex;
			value.baseVertexIndex = (uint)builderTableSubMesh.startVertex;
			value.startInstance = num;
			value.instanceCount = (uint)builderTableMeshInstances.transforms.length;
			num += value.instanceCount;
			indirectBatch.commandData[i] = value;
		}
	}

	// Token: 0x06001E59 RID: 7769 RVA: 0x00097F28 File Offset: 0x00096128
	private void RenderIndirectBatch(BuilderTableDataRenderIndirectBatch indirectBatch)
	{
		indirectBatch.matrixBuf.SetData<Matrix4x4>(indirectBatch.instanceObjectToWorld);
		indirectBatch.texIndexBuf.SetData<int>(indirectBatch.instanceTexIndex);
		indirectBatch.tintBuf.SetData<float>(indirectBatch.instanceTint);
		indirectBatch.commandBuf.SetData<GraphicsBuffer.IndirectDrawIndexedArgs>(indirectBatch.commandData);
		Graphics.RenderMeshIndirect(indirectBatch.rp, this.renderData.sharedMesh, indirectBatch.commandBuf, indirectBatch.commandCount, 0);
	}

	// Token: 0x06001E5A RID: 7770 RVA: 0x00097F9C File Offset: 0x0009619C
	public void AddPiece(BuilderPiece piece)
	{
		bool isStatic = piece.isStatic;
		BuilderRenderer.meshRenderers.Clear();
		piece.GetComponentsInChildren<MeshRenderer>(false, BuilderRenderer.meshRenderers);
		for (int i = 0; i < BuilderRenderer.meshRenderers.Count; i++)
		{
			MeshRenderer meshRenderer = BuilderRenderer.meshRenderers[i];
			if (meshRenderer.enabled)
			{
				Material material = meshRenderer.material;
				if (material.HasTexture("_BaseMap"))
				{
					Texture2D texture2D = material.GetTexture("_BaseMap") as Texture2D;
					if (!(texture2D == null))
					{
						int value;
						if (!this.renderData.textureToIndex.TryGetValue(texture2D, out value))
						{
							if (!piece.suppressMaterialWarnings)
							{
								Debug.LogWarningFormat("builder piece {0} material {1} texture not found in render data", new object[]
								{
									piece.displayName,
									material.name
								});
							}
						}
						else
						{
							MeshFilter component = meshRenderer.GetComponent<MeshFilter>();
							if (!(component == null))
							{
								Mesh sharedMesh = component.sharedMesh;
								if (!(sharedMesh == null))
								{
									int num;
									if (!this.renderData.meshToIndex.TryGetValue(sharedMesh, out num))
									{
										Debug.LogWarningFormat("builder piece {0} mesh {1} not found in render data", new object[]
										{
											piece.displayName,
											meshRenderer.name
										});
									}
									else
									{
										int num2 = this.renderData.meshInstanceCount[num] % 1;
										this.renderData.meshInstanceCount[num] = this.renderData.meshInstanceCount[num] + 1;
										num += num2;
										int num3 = -1;
										if (isStatic)
										{
											NativeArray<int> instanceTransformIndexToDataIndex = this.renderData.staticBatch.instanceTransformIndexToDataIndex;
											int length = instanceTransformIndexToDataIndex.Length;
											for (int j = 0; j < length; j++)
											{
												if (instanceTransformIndexToDataIndex[j] == -1)
												{
													num3 = j;
													break;
												}
											}
											BuilderTableMeshInstances builderTableMeshInstances = this.renderData.staticBatch.renderMeshes[num];
											int num4 = 0;
											for (int k = 0; k <= num; k++)
											{
												num4 += this.renderData.staticBatch.renderMeshes[k].transforms.length;
											}
											for (int l = 0; l < length; l++)
											{
												if (this.renderData.staticBatch.instanceTransformIndexToDataIndex[l] >= num4)
												{
													this.renderData.staticBatch.instanceTransformIndexToDataIndex[l] = this.renderData.staticBatch.instanceTransformIndexToDataIndex[l] + 1;
												}
											}
											this.renderData.staticBatch.instanceTransform[num3] = meshRenderer.transform;
											this.renderData.staticBatch.instanceTransformIndexToDataIndex[num3] = num4;
											builderTableMeshInstances.transforms.Add(meshRenderer.transform);
											builderTableMeshInstances.texIndex.Add(value);
											builderTableMeshInstances.tint.Add(piece.tint);
											int num5 = this.renderData.staticBatch.totalInstances - 1;
											for (int m = num5; m >= num4; m--)
											{
												this.renderData.staticBatch.instanceTexIndex[m + 1] = this.renderData.staticBatch.instanceTexIndex[m];
											}
											for (int n = num5; n >= num4; n--)
											{
												this.renderData.staticBatch.instanceObjectToWorld[n + 1] = this.renderData.staticBatch.instanceObjectToWorld[n];
											}
											for (int num6 = num5; num6 >= num4; num6--)
											{
												this.renderData.staticBatch.instanceTint[num6 + 1] = this.renderData.staticBatch.instanceTint[num6];
											}
											this.renderData.staticBatch.instanceObjectToWorld[num4] = meshRenderer.transform.localToWorldMatrix;
											this.renderData.staticBatch.instanceTexIndex[num4] = value;
											this.renderData.staticBatch.instanceTint[num4] = 1f;
											this.renderData.staticBatch.totalInstances++;
										}
										else
										{
											BuilderTableMeshInstances builderTableMeshInstances2 = this.renderData.dynamicBatch.renderMeshes[num];
											builderTableMeshInstances2.transforms.Add(meshRenderer.transform);
											builderTableMeshInstances2.texIndex.Add(value);
											builderTableMeshInstances2.tint.Add(piece.tint);
											this.renderData.dynamicBatch.totalInstances++;
										}
										piece.renderingIndirect.Add(meshRenderer);
										piece.renderingIndirectTransformIndex.Add(num3);
										meshRenderer.enabled = false;
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06001E5B RID: 7771 RVA: 0x00098454 File Offset: 0x00096654
	public void RemovePiece(BuilderPiece piece)
	{
		bool isStatic = piece.isStatic;
		for (int i = 0; i < piece.renderingIndirect.Count; i++)
		{
			MeshRenderer meshRenderer = piece.renderingIndirect[i];
			if (!(meshRenderer == null))
			{
				Material sharedMaterial = meshRenderer.sharedMaterial;
				if (sharedMaterial.HasTexture("_BaseMap"))
				{
					Texture2D texture2D = sharedMaterial.GetTexture("_BaseMap") as Texture2D;
					int num;
					if (!(texture2D == null) && this.renderData.textureToIndex.TryGetValue(texture2D, out num))
					{
						MeshFilter component = meshRenderer.GetComponent<MeshFilter>();
						if (!(component == null))
						{
							Mesh sharedMesh = component.sharedMesh;
							int num2;
							if (this.renderData.meshToIndex.TryGetValue(sharedMesh, out num2))
							{
								Transform transform = meshRenderer.transform;
								bool flag = false;
								int num3 = 0;
								int num4 = -1;
								if (isStatic)
								{
									for (int j = 0; j < num2; j++)
									{
										num3 += this.renderData.staticBatch.renderMeshes[j].transforms.length;
									}
									TransformAccessArray instanceTransform = this.renderData.staticBatch.instanceTransform;
									int length = instanceTransform.length;
									int index = piece.renderingIndirectTransformIndex[i];
									num4 = this.renderData.staticBatch.instanceTransformIndexToDataIndex[index];
									this.renderData.staticBatch.instanceTransform[index] = null;
									this.renderData.staticBatch.instanceTransformIndexToDataIndex[index] = -1;
									for (int k = 0; k < length; k++)
									{
										if (this.renderData.staticBatch.instanceTransformIndexToDataIndex[k] > num4)
										{
											this.renderData.staticBatch.instanceTransformIndexToDataIndex[k] = this.renderData.staticBatch.instanceTransformIndexToDataIndex[k] - 1;
										}
									}
								}
								for (int l = 0; l < 1; l++)
								{
									int index2 = num2 + l;
									if (isStatic)
									{
										BuilderTableMeshInstances builderTableMeshInstances = this.renderData.staticBatch.renderMeshes[index2];
										for (int m = 0; m < builderTableMeshInstances.transforms.length; m++)
										{
											if (builderTableMeshInstances.transforms[m] == transform)
											{
												num3 += m;
												BuilderRenderer.RemoveAt(builderTableMeshInstances.transforms, m);
												builderTableMeshInstances.texIndex.RemoveAt(m);
												builderTableMeshInstances.tint.RemoveAt(m);
												flag = true;
												this.renderData.staticBatch.totalInstances--;
												break;
											}
										}
									}
									else
									{
										BuilderTableMeshInstances builderTableMeshInstances2 = this.renderData.dynamicBatch.renderMeshes[index2];
										for (int n = 0; n < builderTableMeshInstances2.transforms.length; n++)
										{
											if (builderTableMeshInstances2.transforms[n] == transform)
											{
												BuilderRenderer.RemoveAt(builderTableMeshInstances2.transforms, n);
												builderTableMeshInstances2.texIndex.RemoveAt(n);
												builderTableMeshInstances2.tint.RemoveAt(n);
												flag = true;
												this.renderData.dynamicBatch.totalInstances--;
												break;
											}
										}
									}
									if (flag)
									{
										break;
									}
								}
								if (flag && isStatic)
								{
									int num5 = this.renderData.staticBatch.totalInstances + 1;
									for (int num6 = num4; num6 < num5; num6++)
									{
										this.renderData.staticBatch.instanceTexIndex[num6] = this.renderData.staticBatch.instanceTexIndex[num6 + 1];
									}
									for (int num7 = num4; num7 < num5; num7++)
									{
										this.renderData.staticBatch.instanceObjectToWorld[num7] = this.renderData.staticBatch.instanceObjectToWorld[num7 + 1];
									}
									for (int num8 = num4; num8 < num5; num8++)
									{
										this.renderData.staticBatch.instanceTint[num8] = this.renderData.staticBatch.instanceTint[num8 + 1];
									}
								}
								meshRenderer.enabled = true;
							}
						}
					}
				}
			}
		}
		piece.renderingIndirect.Clear();
		piece.renderingIndirectTransformIndex.Clear();
	}

	// Token: 0x06001E5C RID: 7772 RVA: 0x000988A0 File Offset: 0x00096AA0
	public void ChangePieceIndirectMaterial(BuilderPiece piece, List<MeshRenderer> targetRenderers, Material targetMaterial)
	{
		if (targetMaterial == null)
		{
			return;
		}
		if (!targetMaterial.HasTexture("_BaseMap"))
		{
			Debug.LogError("New Material is missing a texture");
			return;
		}
		Texture2D texture2D = targetMaterial.GetTexture("_BaseMap") as Texture2D;
		if (texture2D == null)
		{
			Debug.LogError("New Material does not have a \"_BaseMap\" property");
			return;
		}
		int value;
		if (!this.renderData.textureToIndex.TryGetValue(texture2D, out value))
		{
			Debug.LogError("New Material is not in the texture array");
			return;
		}
		bool isStatic = piece.isStatic;
		for (int i = 0; i < piece.renderingIndirect.Count; i++)
		{
			MeshRenderer meshRenderer = piece.renderingIndirect[i];
			if (!targetRenderers.Contains(meshRenderer))
			{
				Debug.Log("renderer not in target list");
			}
			else
			{
				meshRenderer.material = targetMaterial;
				MeshFilter component = meshRenderer.GetComponent<MeshFilter>();
				if (!(component == null))
				{
					Mesh sharedMesh = component.sharedMesh;
					int num;
					if (this.renderData.meshToIndex.TryGetValue(sharedMesh, out num))
					{
						Transform transform = meshRenderer.transform;
						bool flag = false;
						if (isStatic)
						{
							int index = piece.renderingIndirectTransformIndex[i];
							int num2 = this.renderData.staticBatch.instanceTransformIndexToDataIndex[index];
							if (num2 >= 0)
							{
								this.renderData.staticBatch.instanceTexIndex[num2] = value;
							}
						}
						else
						{
							for (int j = 0; j < 1; j++)
							{
								int index2 = num + j;
								BuilderTableMeshInstances builderTableMeshInstances = this.renderData.dynamicBatch.renderMeshes[index2];
								for (int k = 0; k < builderTableMeshInstances.transforms.length; k++)
								{
									if (builderTableMeshInstances.transforms[k] == transform)
									{
										this.renderData.dynamicBatch.renderMeshes.ElementAt(index2).texIndex[k] = value;
										flag = true;
										break;
									}
								}
								if (flag)
								{
									break;
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06001E5D RID: 7773 RVA: 0x00098A90 File Offset: 0x00096C90
	public static void RemoveAt(TransformAccessArray a, int i)
	{
		int length = a.length;
		for (int j = i; j < length - 1; j++)
		{
			a[j] = a[j + 1];
		}
		a.RemoveAtSwapBack(length - 1);
	}

	// Token: 0x06001E5E RID: 7774 RVA: 0x00098AD0 File Offset: 0x00096CD0
	public void SetPieceTint(BuilderPiece piece, float tint)
	{
		for (int i = 0; i < piece.renderingIndirect.Count; i++)
		{
			MeshRenderer meshRenderer = piece.renderingIndirect[i];
			Material sharedMaterial = meshRenderer.sharedMaterial;
			if (sharedMaterial.HasTexture("_BaseMap"))
			{
				Texture2D texture2D = sharedMaterial.GetTexture("_BaseMap") as Texture2D;
				int num;
				if (!(texture2D == null) && this.renderData.textureToIndex.TryGetValue(texture2D, out num))
				{
					MeshFilter component = meshRenderer.GetComponent<MeshFilter>();
					if (!(component == null))
					{
						Mesh sharedMesh = component.sharedMesh;
						int num2;
						if (this.renderData.meshToIndex.TryGetValue(sharedMesh, out num2))
						{
							Transform transform = meshRenderer.transform;
							if (piece.isStatic)
							{
								int index = piece.renderingIndirectTransformIndex[i];
								int num3 = this.renderData.staticBatch.instanceTransformIndexToDataIndex[index];
								if (num3 >= 0)
								{
									this.renderData.staticBatch.instanceTint[num3] = tint;
								}
							}
							else
							{
								for (int j = 0; j < 1; j++)
								{
									int index2 = num2 + j;
									BuilderTableMeshInstances builderTableMeshInstances = this.renderData.dynamicBatch.renderMeshes[index2];
									for (int k = 0; k < builderTableMeshInstances.transforms.length; k++)
									{
										if (builderTableMeshInstances.transforms[k] == transform)
										{
											builderTableMeshInstances.tint[k] = tint;
											break;
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x040021C9 RID: 8649
	public Material sharedMaterialBase;

	// Token: 0x040021CA RID: 8650
	public Material sharedMaterialIndirectBase;

	// Token: 0x040021CB RID: 8651
	public BuilderTableDataRenderData renderData;

	// Token: 0x040021CC RID: 8652
	private const string texturePropName = "_BaseMap";

	// Token: 0x040021CD RID: 8653
	private const string textureArrayPropName = "_BaseMapArray";

	// Token: 0x040021CE RID: 8654
	private const string textureArrayIndexPropName = "_BaseMapArrayIndex";

	// Token: 0x040021CF RID: 8655
	private const string transformMatrixPropName = "_TransformMatrix";

	// Token: 0x040021D0 RID: 8656
	private const string texIndexPropName = "_TexIndex";

	// Token: 0x040021D1 RID: 8657
	private const string tintPropName = "_Tint";

	// Token: 0x040021D2 RID: 8658
	public const int MAX_STATIC_INSTANCES = 8192;

	// Token: 0x040021D3 RID: 8659
	public const int MAX_DYNAMIC_INSTANCES = 8192;

	// Token: 0x040021D4 RID: 8660
	private bool initialized;

	// Token: 0x040021D5 RID: 8661
	private bool built;

	// Token: 0x040021D6 RID: 8662
	private bool showing;

	// Token: 0x040021D7 RID: 8663
	private static List<MeshRenderer> meshRenderers = new List<MeshRenderer>(128);

	// Token: 0x040021D8 RID: 8664
	private const int MAX_TOTAL_VERTS = 65536;

	// Token: 0x040021D9 RID: 8665
	private const int MAX_TOTAL_TRIS = 65536;

	// Token: 0x040021DA RID: 8666
	private static List<Vector3> verticesAll = new List<Vector3>(65536);

	// Token: 0x040021DB RID: 8667
	private static List<Vector3> normalsAll = new List<Vector3>(65536);

	// Token: 0x040021DC RID: 8668
	private static List<Vector2> uv1All = new List<Vector2>(65536);

	// Token: 0x040021DD RID: 8669
	private static List<int> trianglesAll = new List<int>(65536);

	// Token: 0x040021DE RID: 8670
	private static List<Vector3> vertices = new List<Vector3>(65536);

	// Token: 0x040021DF RID: 8671
	private static List<Vector3> normals = new List<Vector3>(65536);

	// Token: 0x040021E0 RID: 8672
	private static List<Vector2> uv1 = new List<Vector2>(65536);

	// Token: 0x040021E1 RID: 8673
	private static List<int> triangles = new List<int>(65536);

	// Token: 0x020004DF RID: 1247
	[BurstCompile]
	public struct SetupInstanceDataForMesh : IJobParallelForTransform
	{
		// Token: 0x06001E61 RID: 7777 RVA: 0x00098CEC File Offset: 0x00096EEC
		public void Execute(int index, TransformAccess transform)
		{
			int index2 = index + (int)this.commandData.startInstance;
			this.objectToWorld[index2] = transform.localToWorldMatrix;
			this.instanceTexIndex[index2] = this.texIndex[index];
			this.instanceTint[index2] = this.tint[index];
		}

		// Token: 0x040021E2 RID: 8674
		[ReadOnly]
		public NativeList<int> texIndex;

		// Token: 0x040021E3 RID: 8675
		[ReadOnly]
		public NativeList<float> tint;

		// Token: 0x040021E4 RID: 8676
		[ReadOnly]
		public GraphicsBuffer.IndirectDrawIndexedArgs commandData;

		// Token: 0x040021E5 RID: 8677
		[ReadOnly]
		public Vector3 cameraPos;

		// Token: 0x040021E6 RID: 8678
		[NativeDisableContainerSafetyRestriction]
		public NativeArray<int> instanceTexIndex;

		// Token: 0x040021E7 RID: 8679
		[NativeDisableContainerSafetyRestriction]
		public NativeArray<Matrix4x4> objectToWorld;

		// Token: 0x040021E8 RID: 8680
		[NativeDisableContainerSafetyRestriction]
		public NativeArray<float> instanceTint;

		// Token: 0x040021E9 RID: 8681
		[NativeDisableContainerSafetyRestriction]
		public NativeArray<int> lodLevel;

		// Token: 0x040021EA RID: 8682
		[NativeDisableContainerSafetyRestriction]
		public NativeArray<int> lodDirty;
	}

	// Token: 0x020004E0 RID: 1248
	[BurstCompile]
	public struct SetupInstanceDataForMeshStatic : IJobParallelForTransform
	{
		// Token: 0x06001E62 RID: 7778 RVA: 0x00098D4C File Offset: 0x00096F4C
		public void Execute(int index, TransformAccess transform)
		{
			if (transform.isValid)
			{
				int index2 = this.transformIndexToDataIndex[index];
				this.objectToWorld[index2] = transform.localToWorldMatrix;
			}
		}

		// Token: 0x040021EB RID: 8683
		[ReadOnly]
		public NativeArray<int> transformIndexToDataIndex;

		// Token: 0x040021EC RID: 8684
		[NativeDisableContainerSafetyRestriction]
		public NativeArray<Matrix4x4> objectToWorld;
	}
}
