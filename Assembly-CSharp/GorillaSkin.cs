using System;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000162 RID: 354
public class GorillaSkin : ScriptableObject
{
	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x060008E5 RID: 2277 RVA: 0x0003052E File Offset: 0x0002E72E
	public Mesh bodyMesh
	{
		get
		{
			return this._bodyMesh;
		}
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x00030538 File Offset: 0x0002E738
	public static GorillaSkin CopyWithInstancedMaterials(GorillaSkin basis)
	{
		GorillaSkin gorillaSkin = ScriptableObject.CreateInstance<GorillaSkin>();
		gorillaSkin._faceMaterial = new Material(basis._faceMaterial);
		gorillaSkin._chestMaterial = new Material(basis._chestMaterial);
		gorillaSkin._bodyMaterial = new Material(basis._bodyMaterial);
		gorillaSkin._scoreboardMaterial = new Material(basis._scoreboardMaterial);
		gorillaSkin._bodyMesh = basis.bodyMesh;
		return gorillaSkin;
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x060008E7 RID: 2279 RVA: 0x0003059A File Offset: 0x0002E79A
	public Material faceMaterial
	{
		get
		{
			return this._faceMaterial;
		}
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x060008E8 RID: 2280 RVA: 0x000305A2 File Offset: 0x0002E7A2
	public Material bodyMaterial
	{
		get
		{
			return this._bodyMaterial;
		}
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x060008E9 RID: 2281 RVA: 0x000305AA File Offset: 0x0002E7AA
	public Material chestMaterial
	{
		get
		{
			return this._chestMaterial;
		}
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x060008EA RID: 2282 RVA: 0x000305B2 File Offset: 0x0002E7B2
	public Material scoreboardMaterial
	{
		get
		{
			return this._scoreboardMaterial;
		}
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x000305BC File Offset: 0x0002E7BC
	public static void ShowActiveSkin(VRRig rig)
	{
		bool useDefaultBodySkin;
		GorillaSkin activeSkin = GorillaSkin.GetActiveSkin(rig, out useDefaultBodySkin);
		GorillaSkin.ShowSkin(rig, activeSkin, useDefaultBodySkin);
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x000305DC File Offset: 0x0002E7DC
	public void ApplySkinToMannequin(GameObject mannequin)
	{
		SkinnedMeshRenderer skinnedMeshRenderer;
		if (mannequin.TryGetComponent<SkinnedMeshRenderer>(out skinnedMeshRenderer))
		{
			Material[] sharedMaterials = new Material[]
			{
				this.bodyMaterial,
				this.chestMaterial,
				this.faceMaterial
			};
			skinnedMeshRenderer.sharedMaterials = sharedMaterials;
			return;
		}
		MeshRenderer meshRenderer;
		if (mannequin.TryGetComponent<MeshRenderer>(out meshRenderer))
		{
			Material[] sharedMaterials2 = new Material[]
			{
				this.bodyMaterial,
				this.chestMaterial,
				this.faceMaterial
			};
			meshRenderer.sharedMaterials = sharedMaterials2;
		}
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x00030650 File Offset: 0x0002E850
	public static GorillaSkin GetActiveSkin(VRRig rig, out bool useDefaultBodySkin)
	{
		if (rig.CurrentModeSkin.IsNotNull())
		{
			useDefaultBodySkin = false;
			return rig.CurrentModeSkin;
		}
		if (rig.TemporaryEffectSkin.IsNotNull())
		{
			useDefaultBodySkin = false;
			return rig.TemporaryEffectSkin;
		}
		if (rig.CurrentCosmeticSkin.IsNotNull())
		{
			useDefaultBodySkin = false;
			return rig.CurrentCosmeticSkin;
		}
		useDefaultBodySkin = true;
		return rig.defaultSkin;
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x000306AC File Offset: 0x0002E8AC
	public static void ShowSkin(VRRig rig, GorillaSkin skin, bool useDefaultBodySkin = false)
	{
		if (skin.bodyMesh != null)
		{
			rig.mainSkin.sharedMesh = skin.bodyMesh;
		}
		if (useDefaultBodySkin)
		{
			rig.materialsToChangeTo[0] = rig.myDefaultSkinMaterialInstance;
		}
		else
		{
			rig.materialsToChangeTo[0] = skin.bodyMaterial;
		}
		rig.bodyRenderer.SetSkinMaterials(rig.materialsToChangeTo[rig.setMatIndex], skin.chestMaterial);
		rig.scoreboardMaterial = skin.scoreboardMaterial;
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x00030724 File Offset: 0x0002E924
	public static void ApplyToRig(VRRig rig, GorillaSkin skin, GorillaSkin.SkinType type)
	{
		bool flag;
		GorillaSkin activeSkin = GorillaSkin.GetActiveSkin(rig, out flag);
		switch (type)
		{
		case GorillaSkin.SkinType.cosmetic:
			rig.CurrentCosmeticSkin = skin;
			break;
		case GorillaSkin.SkinType.gameMode:
			rig.CurrentModeSkin = skin;
			break;
		case GorillaSkin.SkinType.temporaryEffect:
			rig.TemporaryEffectSkin = skin;
			break;
		default:
			Debug.LogError("Unknown skin slot");
			break;
		}
		bool useDefaultBodySkin;
		GorillaSkin activeSkin2 = GorillaSkin.GetActiveSkin(rig, out useDefaultBodySkin);
		if (activeSkin != activeSkin2)
		{
			GorillaSkin.ShowSkin(rig, activeSkin2, useDefaultBodySkin);
		}
	}

	// Token: 0x04000AC9 RID: 2761
	[FormerlySerializedAs("faceMaterial")]
	[Space]
	[SerializeField]
	private Material _faceMaterial;

	// Token: 0x04000ACA RID: 2762
	[FormerlySerializedAs("chestMaterial")]
	[FormerlySerializedAs("chestEarsMaterial")]
	[SerializeField]
	private Material _chestMaterial;

	// Token: 0x04000ACB RID: 2763
	[FormerlySerializedAs("bodyMaterial")]
	[SerializeField]
	private Material _bodyMaterial;

	// Token: 0x04000ACC RID: 2764
	[SerializeField]
	private Material _scoreboardMaterial;

	// Token: 0x04000ACD RID: 2765
	[Space]
	[SerializeField]
	private Mesh _bodyMesh;

	// Token: 0x04000ACE RID: 2766
	[Space]
	[NonSerialized]
	private Material _bodyRuntime;

	// Token: 0x04000ACF RID: 2767
	[NonSerialized]
	private Material _chestRuntime;

	// Token: 0x04000AD0 RID: 2768
	[NonSerialized]
	private Material _faceRuntime;

	// Token: 0x04000AD1 RID: 2769
	[NonSerialized]
	private Material _scoreRuntime;

	// Token: 0x02000163 RID: 355
	public enum SkinType
	{
		// Token: 0x04000AD3 RID: 2771
		cosmetic,
		// Token: 0x04000AD4 RID: 2772
		gameMode,
		// Token: 0x04000AD5 RID: 2773
		temporaryEffect
	}
}
