﻿using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Rendering;

// Token: 0x020004EB RID: 1259
public class BuilderRenderer : MonoBehaviour
{
	// Token: 0x06001E9D RID: 7837 RVA: 0x00044CE8 File Offset: 0x00042EE8
	private void Awake()
	{
		this.InitIfNeeded();
	}

	// Token: 0x06001E9E RID: 7838 RVA: 0x000E95C8 File Offset: 0x000E77C8
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

	// Token: 0x06001E9F RID: 7839 RVA: 0x00044CF0 File Offset: 0x00042EF0
	public void Show(bool show)
	{
		this.showing = show;
	}

	// Token: 0x06001EA0 RID: 7840 RVA: 0x000E96E8 File Offset: 0x000E78E8
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

	// Token: 0x06001EA1 RID: 7841 RVA: 0x000E975C File Offset: 0x000E795C
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

	// Token: 0x06001EA2 RID: 7842 RVA: 0x00044CF9 File Offset: 0x00042EF9
	public void LateUpdate()
	{
		if (!this.built || !this.showing)
		{
			return;
		}
		this.RenderIndirect();
	}

	// Token: 0x06001EA3 RID: 7843 RVA: 0x000E97DC File Offset: 0x000E79DC
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

	// Token: 0x06001EA4 RID: 7844 RVA: 0x000E99BC File Offset: 0x000E7BBC
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

	// Token: 0x06001EA5 RID: 7845 RVA: 0x000E9B4C File Offset: 0x000E7D4C
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

	// Token: 0x06001EA6 RID: 7846 RVA: 0x000E9CB8 File Offset: 0x000E7EB8
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

	// Token: 0x06001EA7 RID: 7847 RVA: 0x000E9E9C File Offset: 0x000E809C
	public void BuildBuffer()
	{
		this.renderData.dynamicBatch = new BuilderTableDataRenderIndirectBatch();
		BuilderRenderer.BuildBatch(this.renderData.dynamicBatch, this.renderData.meshes.Count, 8192, this.renderData.sharedMaterialIndirect);
		this.renderData.staticBatch = new BuilderTableDataRenderIndirectBatch();
		BuilderRenderer.BuildBatch(this.renderData.staticBatch, this.renderData.meshes.Count, 8192, this.renderData.sharedMaterialIndirect);
	}

	// Token: 0x06001EA8 RID: 7848 RVA: 0x000E9F2C File Offset: 0x000E812C
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

	// Token: 0x06001EA9 RID: 7849 RVA: 0x00044D12 File Offset: 0x00042F12
	private void OnDestroy()
	{
		this.DestroyBuffer();
		this.renderData.subMeshes.Dispose();
	}

	// Token: 0x06001EAA RID: 7850 RVA: 0x00044D2A File Offset: 0x00042F2A
	public void DestroyBuffer()
	{
		BuilderRenderer.DestroyBatch(this.renderData.staticBatch);
		BuilderRenderer.DestroyBatch(this.renderData.dynamicBatch);
	}

	// Token: 0x06001EAB RID: 7851 RVA: 0x000EA160 File Offset: 0x000E8360
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

	// Token: 0x06001EAC RID: 7852 RVA: 0x000EA250 File Offset: 0x000E8450
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

	// Token: 0x06001EAD RID: 7853 RVA: 0x00044D4C File Offset: 0x00042F4C
	public void RenderIndirect()
	{
		this.renderData.setupInstancesJobs.Complete();
		this.RenderIndirectBatch(this.renderData.staticBatch);
	}

	// Token: 0x06001EAE RID: 7854 RVA: 0x000EA304 File Offset: 0x000E8504
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

	// Token: 0x06001EAF RID: 7855 RVA: 0x000EA3A0 File Offset: 0x000E85A0
	private void RenderIndirectBatch(BuilderTableDataRenderIndirectBatch indirectBatch)
	{
		indirectBatch.matrixBuf.SetData<Matrix4x4>(indirectBatch.instanceObjectToWorld);
		indirectBatch.texIndexBuf.SetData<int>(indirectBatch.instanceTexIndex);
		indirectBatch.tintBuf.SetData<float>(indirectBatch.instanceTint);
		indirectBatch.commandBuf.SetData<GraphicsBuffer.IndirectDrawIndexedArgs>(indirectBatch.commandData);
		Graphics.RenderMeshIndirect(indirectBatch.rp, this.renderData.sharedMesh, indirectBatch.commandBuf, indirectBatch.commandCount, 0);
	}

	// Token: 0x06001EB0 RID: 7856 RVA: 0x000EA414 File Offset: 0x000E8614
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

	// Token: 0x06001EB1 RID: 7857 RVA: 0x000EA8CC File Offset: 0x000E8ACC
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

	// Token: 0x06001EB2 RID: 7858 RVA: 0x000EAD18 File Offset: 0x000E8F18
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

	// Token: 0x06001EB3 RID: 7859 RVA: 0x000EAF08 File Offset: 0x000E9108
	public static void RemoveAt(TransformAccessArray a, int i)
	{
		int length = a.length;
		for (int j = i; j < length - 1; j++)
		{
			a[j] = a[j + 1];
		}
		a.RemoveAtSwapBack(length - 1);
	}

	// Token: 0x06001EB4 RID: 7860 RVA: 0x000EAF48 File Offset: 0x000E9148
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

	// Token: 0x0400221B RID: 8731
	public Material sharedMaterialBase;

	// Token: 0x0400221C RID: 8732
	public Material sharedMaterialIndirectBase;

	// Token: 0x0400221D RID: 8733
	public BuilderTableDataRenderData renderData;

	// Token: 0x0400221E RID: 8734
	private const string texturePropName = "_BaseMap";

	// Token: 0x0400221F RID: 8735
	private const string textureArrayPropName = "_BaseMapArray";

	// Token: 0x04002220 RID: 8736
	private const string textureArrayIndexPropName = "_BaseMapArrayIndex";

	// Token: 0x04002221 RID: 8737
	private const string transformMatrixPropName = "_TransformMatrix";

	// Token: 0x04002222 RID: 8738
	private const string texIndexPropName = "_TexIndex";

	// Token: 0x04002223 RID: 8739
	private const string tintPropName = "_Tint";

	// Token: 0x04002224 RID: 8740
	public const int MAX_STATIC_INSTANCES = 8192;

	// Token: 0x04002225 RID: 8741
	public const int MAX_DYNAMIC_INSTANCES = 8192;

	// Token: 0x04002226 RID: 8742
	private bool initialized;

	// Token: 0x04002227 RID: 8743
	private bool built;

	// Token: 0x04002228 RID: 8744
	private bool showing;

	// Token: 0x04002229 RID: 8745
	private static List<MeshRenderer> meshRenderers = new List<MeshRenderer>(128);

	// Token: 0x0400222A RID: 8746
	private const int MAX_TOTAL_VERTS = 65536;

	// Token: 0x0400222B RID: 8747
	private const int MAX_TOTAL_TRIS = 65536;

	// Token: 0x0400222C RID: 8748
	private static List<Vector3> verticesAll = new List<Vector3>(65536);

	// Token: 0x0400222D RID: 8749
	private static List<Vector3> normalsAll = new List<Vector3>(65536);

	// Token: 0x0400222E RID: 8750
	private static List<Vector2> uv1All = new List<Vector2>(65536);

	// Token: 0x0400222F RID: 8751
	private static List<int> trianglesAll = new List<int>(65536);

	// Token: 0x04002230 RID: 8752
	private static List<Vector3> vertices = new List<Vector3>(65536);

	// Token: 0x04002231 RID: 8753
	private static List<Vector3> normals = new List<Vector3>(65536);

	// Token: 0x04002232 RID: 8754
	private static List<Vector2> uv1 = new List<Vector2>(65536);

	// Token: 0x04002233 RID: 8755
	private static List<int> triangles = new List<int>(65536);

	// Token: 0x020004EC RID: 1260
	[BurstCompile]
	public struct SetupInstanceDataForMesh : IJobParallelForTransform
	{
		// Token: 0x06001EB7 RID: 7863 RVA: 0x000EB164 File Offset: 0x000E9364
		public void Execute(int index, TransformAccess transform)
		{
			int index2 = index + (int)this.commandData.startInstance;
			this.objectToWorld[index2] = transform.localToWorldMatrix;
			this.instanceTexIndex[index2] = this.texIndex[index];
			this.instanceTint[index2] = this.tint[index];
		}

		// Token: 0x04002234 RID: 8756
		[ReadOnly]
		public NativeList<int> texIndex;

		// Token: 0x04002235 RID: 8757
		[ReadOnly]
		public NativeList<float> tint;

		// Token: 0x04002236 RID: 8758
		[ReadOnly]
		public GraphicsBuffer.IndirectDrawIndexedArgs commandData;

		// Token: 0x04002237 RID: 8759
		[ReadOnly]
		public Vector3 cameraPos;

		// Token: 0x04002238 RID: 8760
		[NativeDisableContainerSafetyRestriction]
		public NativeArray<int> instanceTexIndex;

		// Token: 0x04002239 RID: 8761
		[NativeDisableContainerSafetyRestriction]
		public NativeArray<Matrix4x4> objectToWorld;

		// Token: 0x0400223A RID: 8762
		[NativeDisableContainerSafetyRestriction]
		public NativeArray<float> instanceTint;

		// Token: 0x0400223B RID: 8763
		[NativeDisableContainerSafetyRestriction]
		public NativeArray<int> lodLevel;

		// Token: 0x0400223C RID: 8764
		[NativeDisableContainerSafetyRestriction]
		public NativeArray<int> lodDirty;
	}

	// Token: 0x020004ED RID: 1261
	[BurstCompile]
	public struct SetupInstanceDataForMeshStatic : IJobParallelForTransform
	{
		// Token: 0x06001EB8 RID: 7864 RVA: 0x000EB1C4 File Offset: 0x000E93C4
		public void Execute(int index, TransformAccess transform)
		{
			if (transform.isValid)
			{
				int index2 = this.transformIndexToDataIndex[index];
				this.objectToWorld[index2] = transform.localToWorldMatrix;
			}
		}

		// Token: 0x0400223D RID: 8765
		[ReadOnly]
		public NativeArray<int> transformIndexToDataIndex;

		// Token: 0x0400223E RID: 8766
		[NativeDisableContainerSafetyRestriction]
		public NativeArray<Matrix4x4> objectToWorld;
	}
}
