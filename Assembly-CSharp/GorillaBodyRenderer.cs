using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200054B RID: 1355
public class GorillaBodyRenderer : MonoBehaviour
{
	// Token: 0x17000369 RID: 873
	// (get) Token: 0x06002130 RID: 8496 RVA: 0x000A578A File Offset: 0x000A398A
	// (set) Token: 0x06002131 RID: 8497 RVA: 0x000A5792 File Offset: 0x000A3992
	public GorillaBodyType bodyType
	{
		get
		{
			return this._bodyType;
		}
		set
		{
			this.SetBodyType(value);
		}
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x06002132 RID: 8498 RVA: 0x000A579B File Offset: 0x000A399B
	public bool renderFace
	{
		get
		{
			return this._renderFace;
		}
	}

	// Token: 0x06002133 RID: 8499 RVA: 0x000A57A3 File Offset: 0x000A39A3
	public SkinnedMeshRenderer GetBody(GorillaBodyType type)
	{
		return this._renderersCache[(int)type];
	}

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x06002134 RID: 8500 RVA: 0x000A57AD File Offset: 0x000A39AD
	public SkinnedMeshRenderer ActiveBody
	{
		get
		{
			return this.GetBody(this._bodyType);
		}
	}

	// Token: 0x06002135 RID: 8501 RVA: 0x000A57BC File Offset: 0x000A39BC
	public static void SetAllSkeletons(bool allSkeletons)
	{
		GorillaBodyRenderer.oopsAllSkeletons = allSkeletons;
		GorillaTagger.Instance.offlineVRRig.bodyRenderer.Refresh();
		foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
		{
			vrrig.bodyRenderer.Refresh();
		}
	}

	// Token: 0x06002136 RID: 8502 RVA: 0x000A5834 File Offset: 0x000A3A34
	public void SetGameModeBodyType(GorillaBodyType bodyType)
	{
		this.gameModeBodyType = bodyType;
		this.Refresh();
	}

	// Token: 0x06002137 RID: 8503 RVA: 0x000A5843 File Offset: 0x000A3A43
	public void SetCosmeticBodyType(GorillaBodyType bodyType)
	{
		this.cosmeticBodyType = bodyType;
		this.Refresh();
	}

	// Token: 0x06002138 RID: 8504 RVA: 0x000A5852 File Offset: 0x000A3A52
	public void SetDefaults()
	{
		this.gameModeBodyType = GorillaBodyType.Default;
		this.cosmeticBodyType = GorillaBodyType.Default;
		this.Refresh();
	}

	// Token: 0x06002139 RID: 8505 RVA: 0x000A5868 File Offset: 0x000A3A68
	private void Refresh()
	{
		GorillaBodyType bodyType;
		if (GorillaBodyRenderer.oopsAllSkeletons)
		{
			bodyType = GorillaBodyType.Skeleton;
		}
		else if (this.gameModeBodyType != GorillaBodyType.Default)
		{
			bodyType = this.gameModeBodyType;
		}
		else
		{
			bodyType = this.cosmeticBodyType;
		}
		this.SetBodyType(bodyType);
	}

	// Token: 0x0600213A RID: 8506 RVA: 0x000A58A0 File Offset: 0x000A3AA0
	public void SetMaterialIndex(int materialIndex)
	{
		this.bodyDefault.sharedMaterial = this.rig.materialsToChangeTo[materialIndex];
		this.bodyNoHead.sharedMaterial = this.bodyDefault.sharedMaterial;
		this.rig.skeleton.SetMaterialIndex(materialIndex);
	}

	// Token: 0x0600213B RID: 8507 RVA: 0x000A58EC File Offset: 0x000A3AEC
	public void SetSkinMaterials(Material bodyMat, Material chestMat)
	{
		Material[] sharedMaterials = this.bodyDefault.sharedMaterials;
		sharedMaterials[0] = bodyMat;
		sharedMaterials[1] = chestMat;
		this.bodyDefault.sharedMaterials = sharedMaterials;
		this.bodyNoHead.sharedMaterials = sharedMaterials;
	}

	// Token: 0x0600213C RID: 8508 RVA: 0x000A5925 File Offset: 0x000A3B25
	public void SetupAsLocalPlayerBody()
	{
		this.faceRenderer.gameObject.layer = 22;
	}

	// Token: 0x0600213D RID: 8509 RVA: 0x000A593C File Offset: 0x000A3B3C
	private void SetBodyType(GorillaBodyType type)
	{
		if (this._bodyType == type)
		{
			return;
		}
		this.SetBodyEnabled(this._bodyType, false);
		this._bodyType = type;
		this.SetBodyEnabled(type, true);
		this._renderFace = (this._bodyType != GorillaBodyType.NoHead && this._bodyType != GorillaBodyType.Skeleton);
		if (this.faceRenderer != null)
		{
			this.faceRenderer.enabled = this._renderFace;
		}
	}

	// Token: 0x0600213E RID: 8510 RVA: 0x000A59AC File Offset: 0x000A3BAC
	private void SetBodyEnabled(GorillaBodyType bodyType, bool enabled)
	{
		SkinnedMeshRenderer body = this.GetBody(bodyType);
		if (body == null)
		{
			return;
		}
		body.enabled = enabled;
		Transform[] bones = body.bones;
		for (int i = 0; i < bones.Length; i++)
		{
			bones[i].gameObject.SetActive(enabled);
		}
	}

	// Token: 0x0600213F RID: 8511 RVA: 0x000A59F5 File Offset: 0x000A3BF5
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x06002140 RID: 8512 RVA: 0x000A5A00 File Offset: 0x000A3C00
	private void Setup()
	{
		this.rig = base.GetComponentInParent<VRRig>();
		this._renderersCache = new SkinnedMeshRenderer[EnumData<GorillaBodyType>.Shared.Values.Length];
		this._renderersCache[0] = this.bodyDefault;
		this._renderersCache[1] = this.bodyNoHead;
		this._renderersCache[2] = this.bodySkeleton;
		this.SetBodyEnabled(GorillaBodyType.Default, true);
		this.SetBodyEnabled(GorillaBodyType.NoHead, false);
		this.SetBodyEnabled(GorillaBodyType.Skeleton, false);
		this.bodyDefault.GetSharedMaterials(this._cachedDefaultMats);
	}

	// Token: 0x040024CA RID: 9418
	[SerializeField]
	private GorillaBodyType _bodyType;

	// Token: 0x040024CB RID: 9419
	[SerializeField]
	private bool _renderFace = true;

	// Token: 0x040024CC RID: 9420
	public MeshRenderer faceRenderer;

	// Token: 0x040024CD RID: 9421
	public SkinnedMeshRenderer bodyDefault;

	// Token: 0x040024CE RID: 9422
	public SkinnedMeshRenderer bodyNoHead;

	// Token: 0x040024CF RID: 9423
	public SkinnedMeshRenderer bodySkeleton;

	// Token: 0x040024D0 RID: 9424
	public SkinnedMeshRenderer bodyCosmetic;

	// Token: 0x040024D1 RID: 9425
	private static bool oopsAllSkeletons;

	// Token: 0x040024D2 RID: 9426
	private GorillaBodyType gameModeBodyType;

	// Token: 0x040024D3 RID: 9427
	private GorillaBodyType cosmeticBodyType;

	// Token: 0x040024D4 RID: 9428
	[Space]
	[NonSerialized]
	private SkinnedMeshRenderer[] _renderersCache = new SkinnedMeshRenderer[0];

	// Token: 0x040024D5 RID: 9429
	[NonSerialized]
	private List<Material> _cachedDefaultMats = new List<Material>(2);

	// Token: 0x040024D6 RID: 9430
	private static readonly List<Material> gEmptyDefaultMats = new List<Material>();

	// Token: 0x040024D7 RID: 9431
	[Space]
	public VRRig rig;
}
