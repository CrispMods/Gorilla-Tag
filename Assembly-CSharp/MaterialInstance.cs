using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006C7 RID: 1735
[HelpURL("https://docs.microsoft.com/windows/mixed-reality/mrtk-unity/features/rendering/material-instance")]
[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
[AddComponentMenu("Scripts/MRTK/Core/MaterialInstance")]
public class MaterialInstance : MonoBehaviour
{
	// Token: 0x06002AE8 RID: 10984 RVA: 0x000D4E2D File Offset: 0x000D302D
	public Material AcquireMaterial(Object owner = null, bool instance = true)
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

	// Token: 0x06002AE9 RID: 10985 RVA: 0x000D4E6B File Offset: 0x000D306B
	public Material[] AcquireMaterials(Object owner = null, bool instance = true)
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

	// Token: 0x06002AEA RID: 10986 RVA: 0x000D4E9E File Offset: 0x000D309E
	public void ReleaseMaterial(Object owner, bool autoDestroy = true)
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

	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x06002AEB RID: 10987 RVA: 0x000D4ED6 File Offset: 0x000D30D6
	public Material Material
	{
		get
		{
			return this.AcquireMaterial(null, true);
		}
	}

	// Token: 0x17000487 RID: 1159
	// (get) Token: 0x06002AEC RID: 10988 RVA: 0x000D4EE0 File Offset: 0x000D30E0
	public Material[] Materials
	{
		get
		{
			return this.AcquireMaterials(null, true);
		}
	}

	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x06002AED RID: 10989 RVA: 0x000D4EEA File Offset: 0x000D30EA
	// (set) Token: 0x06002AEE RID: 10990 RVA: 0x000D4EF2 File Offset: 0x000D30F2
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

	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x06002AEF RID: 10991 RVA: 0x000D4F21 File Offset: 0x000D3121
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

	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x06002AF0 RID: 10992 RVA: 0x000D4F5C File Offset: 0x000D315C
	// (set) Token: 0x06002AF1 RID: 10993 RVA: 0x000D4F91 File Offset: 0x000D3191
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

	// Token: 0x06002AF2 RID: 10994 RVA: 0x000D4FAE File Offset: 0x000D31AE
	private void Awake()
	{
		this.Initialize();
	}

	// Token: 0x06002AF3 RID: 10995 RVA: 0x000D4FB6 File Offset: 0x000D31B6
	private void OnDestroy()
	{
		this.RestoreRenderer();
	}

	// Token: 0x06002AF4 RID: 10996 RVA: 0x000D4FBE File Offset: 0x000D31BE
	private void RestoreRenderer()
	{
		if (this.CachedRenderer != null && this.defaultMaterials != null)
		{
			this.CachedRendererSharedMaterials = this.defaultMaterials;
		}
		MaterialInstance.DestroyMaterials(this.instanceMaterials);
		this.instanceMaterials = null;
	}

	// Token: 0x06002AF5 RID: 10997 RVA: 0x000D4FF4 File Offset: 0x000D31F4
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

	// Token: 0x06002AF6 RID: 10998 RVA: 0x000D504D File Offset: 0x000D324D
	private void AcquireInstances()
	{
		if (this.CachedRenderer != null && !MaterialInstance.MaterialsMatch(this.CachedRendererSharedMaterials, this.instanceMaterials))
		{
			this.CreateInstances();
		}
	}

	// Token: 0x06002AF7 RID: 10999 RVA: 0x000D5078 File Offset: 0x000D3278
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

	// Token: 0x06002AF8 RID: 11000 RVA: 0x000D50D0 File Offset: 0x000D32D0
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

	// Token: 0x06002AF9 RID: 11001 RVA: 0x000D5174 File Offset: 0x000D3374
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

	// Token: 0x06002AFA RID: 11002 RVA: 0x000D51F4 File Offset: 0x000D33F4
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

	// Token: 0x06002AFB RID: 11003 RVA: 0x000D521A File Offset: 0x000D341A
	private static bool IsInstanceMaterial(Material material)
	{
		return material != null && material.name.Contains(" (Instance)");
	}

	// Token: 0x06002AFC RID: 11004 RVA: 0x000D5238 File Offset: 0x000D3438
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

	// Token: 0x06002AFD RID: 11005 RVA: 0x000D5266 File Offset: 0x000D3466
	private static void DestroySafe(Object toDestroy)
	{
		if (toDestroy != null && Application.isPlaying)
		{
			Object.Destroy(toDestroy);
		}
	}

	// Token: 0x04003081 RID: 12417
	private Renderer cachedRenderer;

	// Token: 0x04003082 RID: 12418
	[SerializeField]
	[HideInInspector]
	private Material[] defaultMaterials;

	// Token: 0x04003083 RID: 12419
	private Material[] instanceMaterials;

	// Token: 0x04003084 RID: 12420
	private Material[] cachedSharedMaterials;

	// Token: 0x04003085 RID: 12421
	private bool initialized;

	// Token: 0x04003086 RID: 12422
	private bool materialsInstanced;

	// Token: 0x04003087 RID: 12423
	[SerializeField]
	[Tooltip("Whether to use a cached copy of cachedRenderer.sharedMaterials or call sharedMaterials on the Renderer directly. Enabling the option will lead to better performance but you must turn it off before modifying sharedMaterials of the Renderer.")]
	private bool cacheSharedMaterialsFromRenderer;

	// Token: 0x04003088 RID: 12424
	private readonly HashSet<Object> materialOwners = new HashSet<Object>();

	// Token: 0x04003089 RID: 12425
	private const string instancePostfix = " (Instance)";
}
