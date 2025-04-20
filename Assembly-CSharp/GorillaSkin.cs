using System;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200016D RID: 365
public class GorillaSkin : ScriptableObject
{
	// Token: 0x170000DF RID: 223
	// (get) Token: 0x06000932 RID: 2354 RVA: 0x000367EA File Offset: 0x000349EA
	public Mesh bodyMesh
	{
		get
		{
			return this._bodyMesh;
		}
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x00090FB4 File Offset: 0x0008F1B4
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

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06000934 RID: 2356 RVA: 0x000367F2 File Offset: 0x000349F2
	public Material faceMaterial
	{
		get
		{
			return this._faceMaterial;
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06000935 RID: 2357 RVA: 0x000367FA File Offset: 0x000349FA
	public Material bodyMaterial
	{
		get
		{
			return this._bodyMaterial;
		}
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x06000936 RID: 2358 RVA: 0x00036802 File Offset: 0x00034A02
	public Material chestMaterial
	{
		get
		{
			return this._chestMaterial;
		}
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000937 RID: 2359 RVA: 0x0003680A File Offset: 0x00034A0A
	public Material scoreboardMaterial
	{
		get
		{
			return this._scoreboardMaterial;
		}
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x00091018 File Offset: 0x0008F218
	public static void ShowActiveSkin(VRRig rig)
	{
		bool useDefaultBodySkin;
		GorillaSkin activeSkin = GorillaSkin.GetActiveSkin(rig, out useDefaultBodySkin);
		GorillaSkin.ShowSkin(rig, activeSkin, useDefaultBodySkin);
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x00091038 File Offset: 0x0008F238
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

	// Token: 0x0600093A RID: 2362 RVA: 0x000910AC File Offset: 0x0008F2AC
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

	// Token: 0x0600093B RID: 2363 RVA: 0x00091108 File Offset: 0x0008F308
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

	// Token: 0x0600093C RID: 2364 RVA: 0x00091180 File Offset: 0x0008F380
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

	// Token: 0x04000B10 RID: 2832
	[FormerlySerializedAs("faceMaterial")]
	[Space]
	[SerializeField]
	private Material _faceMaterial;

	// Token: 0x04000B11 RID: 2833
	[FormerlySerializedAs("chestMaterial")]
	[FormerlySerializedAs("chestEarsMaterial")]
	[SerializeField]
	private Material _chestMaterial;

	// Token: 0x04000B12 RID: 2834
	[FormerlySerializedAs("bodyMaterial")]
	[SerializeField]
	private Material _bodyMaterial;

	// Token: 0x04000B13 RID: 2835
	[SerializeField]
	private Material _scoreboardMaterial;

	// Token: 0x04000B14 RID: 2836
	[Space]
	[SerializeField]
	private Mesh _bodyMesh;

	// Token: 0x04000B15 RID: 2837
	[Space]
	[NonSerialized]
	private Material _bodyRuntime;

	// Token: 0x04000B16 RID: 2838
	[NonSerialized]
	private Material _chestRuntime;

	// Token: 0x04000B17 RID: 2839
	[NonSerialized]
	private Material _faceRuntime;

	// Token: 0x04000B18 RID: 2840
	[NonSerialized]
	private Material _scoreRuntime;

	// Token: 0x0200016E RID: 366
	public enum SkinType
	{
		// Token: 0x04000B1A RID: 2842
		cosmetic,
		// Token: 0x04000B1B RID: 2843
		gameMode,
		// Token: 0x04000B1C RID: 2844
		temporaryEffect
	}
}
