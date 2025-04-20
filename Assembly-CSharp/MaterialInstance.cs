using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006DC RID: 1756
[HelpURL("https://docs.microsoft.com/windows/mixed-reality/mrtk-unity/features/rendering/material-instance")]
[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
[AddComponentMenu("Scripts/MRTK/Core/MaterialInstance")]
public class MaterialInstance : MonoBehaviour
{
	// Token: 0x06002B7E RID: 11134 RVA: 0x0004D609 File Offset: 0x0004B809
	public Material AcquireMaterial(UnityEngine.Object owner = null, bool instance = true)
	{
		if (owner != null)
		{
			this.materialOwners.Add(owner);
		}
		if (instance)
		{
			this.AcquireInstances();
		}
		Material[] array = this.instanceMaterials;
		if (array != null && array.Length != 0)
		{
			return this.instanceMaterials[0];
		}
		return null;
	}

	// Token: 0x06002B7F RID: 11135 RVA: 0x0004D647 File Offset: 0x0004B847
	public Material[] AcquireMaterials(UnityEngine.Object owner = null, bool instance = true)
	{
		if (owner != null)
		{
			this.materialOwners.Add(owner);
		}
		if (instance)
		{
			this.AcquireInstances();
		}
		base.gameObject.GetComponent<Material>();
		return this.instanceMaterials;
	}

	// Token: 0x06002B80 RID: 11136 RVA: 0x0004D67A File Offset: 0x0004B87A
	public void ReleaseMaterial(UnityEngine.Object owner, bool autoDestroy = true)
	{
		this.materialOwners.Remove(owner);
		if (autoDestroy && this.materialOwners.Count == 0)
		{
			MaterialInstance.DestroySafe(this);
			if (!base.gameObject.activeInHierarchy)
			{
				this.RestoreRenderer();
			}
		}
	}

	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x06002B81 RID: 11137 RVA: 0x0004D6B2 File Offset: 0x0004B8B2
	public Material Material
	{
		get
		{
			return this.AcquireMaterial(null, true);
		}
	}

	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x06002B82 RID: 11138 RVA: 0x0004D6BC File Offset: 0x0004B8BC
	public Material[] Materials
	{
		get
		{
			return this.AcquireMaterials(null, true);
		}
	}

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x06002B83 RID: 11139 RVA: 0x0004D6C6 File Offset: 0x0004B8C6
	// (set) Token: 0x06002B84 RID: 11140 RVA: 0x0004D6CE File Offset: 0x0004B8CE
	public bool CacheSharedMaterialsFromRenderer
	{
		get
		{
			return this.cacheSharedMaterialsFromRenderer;
		}
		set
		{
			if (this.cacheSharedMaterialsFromRenderer != value)
			{
				if (value)
				{
					this.cachedSharedMaterials = this.CachedRenderer.sharedMaterials;
				}
				else
				{
					this.cachedSharedMaterials = null;
				}
				this.cacheSharedMaterialsFromRenderer = value;
			}
		}
	}

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x06002B85 RID: 11141 RVA: 0x0004D6FD File Offset: 0x0004B8FD
	private Renderer CachedRenderer
	{
		get
		{
			if (this.cachedRenderer == null)
			{
				this.cachedRenderer = base.GetComponent<Renderer>();
				if (this.CacheSharedMaterialsFromRenderer)
				{
					this.cachedSharedMaterials = this.cachedRenderer.sharedMaterials;
				}
			}
			return this.cachedRenderer;
		}
	}

	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x06002B86 RID: 11142 RVA: 0x0004D738 File Offset: 0x0004B938
	// (set) Token: 0x06002B87 RID: 11143 RVA: 0x0004D76D File Offset: 0x0004B96D
	private Material[] CachedRendererSharedMaterials
	{
		get
		{
			if (this.CacheSharedMaterialsFromRenderer)
			{
				if (this.cachedSharedMaterials == null)
				{
					this.cachedSharedMaterials = this.cachedRenderer.sharedMaterials;
				}
				return this.cachedSharedMaterials;
			}
			return this.cachedRenderer.sharedMaterials;
		}
		set
		{
			if (this.CacheSharedMaterialsFromRenderer)
			{
				this.cachedSharedMaterials = value;
			}
			this.cachedRenderer.sharedMaterials = value;
		}
	}

	// Token: 0x06002B88 RID: 11144 RVA: 0x0004D78A File Offset: 0x0004B98A
	private void Awake()
	{
		this.Initialize();
	}

	// Token: 0x06002B89 RID: 11145 RVA: 0x0004D792 File Offset: 0x0004B992
	private void OnDestroy()
	{
		this.RestoreRenderer();
	}

	// Token: 0x06002B8A RID: 11146 RVA: 0x0004D79A File Offset: 0x0004B99A
	private void RestoreRenderer()
	{
		if (this.CachedRenderer != null && this.defaultMaterials != null)
		{
			this.CachedRendererSharedMaterials = this.defaultMaterials;
		}
		MaterialInstance.DestroyMaterials(this.instanceMaterials);
		this.instanceMaterials = null;
	}

	// Token: 0x06002B8B RID: 11147 RVA: 0x00120C48 File Offset: 0x0011EE48
	private void Initialize()
	{
		if (!this.initialized && this.CachedRenderer != null)
		{
			if (!MaterialInstance.HasValidMaterial(this.defaultMaterials))
			{
				this.defaultMaterials = this.CachedRendererSharedMaterials;
			}
			else if (!this.materialsInstanced)
			{
				this.CachedRendererSharedMaterials = this.defaultMaterials;
			}
			this.initialized = true;
		}
	}

	// Token: 0x06002B8C RID: 11148 RVA: 0x0004D7D0 File Offset: 0x0004B9D0
	private void AcquireInstances()
	{
		if (this.CachedRenderer != null && !MaterialInstance.MaterialsMatch(this.CachedRendererSharedMaterials, this.instanceMaterials))
		{
			this.CreateInstances();
		}
	}

	// Token: 0x06002B8D RID: 11149 RVA: 0x00120CA4 File Offset: 0x0011EEA4
	private void CreateInstances()
	{
		this.Initialize();
		MaterialInstance.DestroyMaterials(this.instanceMaterials);
		this.instanceMaterials = MaterialInstance.InstanceMaterials(this.defaultMaterials);
		if (this.CachedRenderer != null && this.instanceMaterials != null)
		{
			this.CachedRendererSharedMaterials = this.instanceMaterials;
		}
		this.materialsInstanced = true;
	}

	// Token: 0x06002B8E RID: 11150 RVA: 0x00120CFC File Offset: 0x0011EEFC
	private static bool MaterialsMatch(Material[] a, Material[] b)
	{
		int? num = (a != null) ? new int?(a.Length) : null;
		int? num2 = (b != null) ? new int?(b.Length) : null;
		if (!(num.GetValueOrDefault() == num2.GetValueOrDefault() & num != null == (num2 != null)))
		{
			return false;
		}
		int num3 = 0;
		for (;;)
		{
			int num4 = num3;
			num2 = ((a != null) ? new int?(a.Length) : null);
			if (!(num4 < num2.GetValueOrDefault() & num2 != null))
			{
				return true;
			}
			if (a[num3] != b[num3])
			{
				break;
			}
			num3++;
		}
		return false;
	}

	// Token: 0x06002B8F RID: 11151 RVA: 0x00120DA0 File Offset: 0x0011EFA0
	private static Material[] InstanceMaterials(Material[] source)
	{
		if (source == null)
		{
			return null;
		}
		Material[] array = new Material[source.Length];
		for (int i = 0; i < source.Length; i++)
		{
			if (source[i] != null)
			{
				if (MaterialInstance.IsInstanceMaterial(source[i]))
				{
					Debug.LogWarning("A material (" + source[i].name + ") which is already instanced was instanced multiple times.");
				}
				array[i] = new Material(source[i]);
				Material material = array[i];
				material.name += " (Instance)";
			}
		}
		return array;
	}

	// Token: 0x06002B90 RID: 11152 RVA: 0x00120E20 File Offset: 0x0011F020
	private static void DestroyMaterials(Material[] materials)
	{
		if (materials != null)
		{
			for (int i = 0; i < materials.Length; i++)
			{
				MaterialInstance.DestroySafe(materials[i]);
			}
		}
	}

	// Token: 0x06002B91 RID: 11153 RVA: 0x0004D7F9 File Offset: 0x0004B9F9
	private static bool IsInstanceMaterial(Material material)
	{
		return material != null && material.name.Contains(" (Instance)");
	}

	// Token: 0x06002B92 RID: 11154 RVA: 0x00120E48 File Offset: 0x0011F048
	private static bool HasValidMaterial(Material[] materials)
	{
		if (materials != null)
		{
			for (int i = 0; i < materials.Length; i++)
			{
				if (materials[i] != null)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002B93 RID: 11155 RVA: 0x0004D816 File Offset: 0x0004BA16
	private static void DestroySafe(UnityEngine.Object toDestroy)
	{
		if (toDestroy != null && Application.isPlaying)
		{
			UnityEngine.Object.Destroy(toDestroy);
		}
	}

	// Token: 0x0400311E RID: 12574
	private Renderer cachedRenderer;

	// Token: 0x0400311F RID: 12575
	[SerializeField]
	[HideInInspector]
	private Material[] defaultMaterials;

	// Token: 0x04003120 RID: 12576
	private Material[] instanceMaterials;

	// Token: 0x04003121 RID: 12577
	private Material[] cachedSharedMaterials;

	// Token: 0x04003122 RID: 12578
	private bool initialized;

	// Token: 0x04003123 RID: 12579
	private bool materialsInstanced;

	// Token: 0x04003124 RID: 12580
	[SerializeField]
	[Tooltip("Whether to use a cached copy of cachedRenderer.sharedMaterials or call sharedMaterials on the Renderer directly. Enabling the option will lead to better performance but you must turn it off before modifying sharedMaterials of the Renderer.")]
	private bool cacheSharedMaterialsFromRenderer;

	// Token: 0x04003125 RID: 12581
	private readonly HashSet<UnityEngine.Object> materialOwners = new HashSet<UnityEngine.Object>();

	// Token: 0x04003126 RID: 12582
	private const string instancePostfix = " (Instance)";
}
