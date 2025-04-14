using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200054C RID: 1356
public class GorillaBodyRenderer : MonoBehaviour
{
	// Token: 0x1700036A RID: 874
	// (get) Token: 0x06002138 RID: 8504 RVA: 0x000A5C0A File Offset: 0x000A3E0A
	// (set) Token: 0x06002139 RID: 8505 RVA: 0x000A5C12 File Offset: 0x000A3E12
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

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x0600213A RID: 8506 RVA: 0x000A5C1B File Offset: 0x000A3E1B
	public bool renderFace
	{
		get
		{
			return this._renderFace;
		}
	}

	// Token: 0x0600213B RID: 8507 RVA: 0x000A5C23 File Offset: 0x000A3E23
	public SkinnedMeshRenderer GetBody(GorillaBodyType type)
	{
		return this._renderersCache[(int)type];
	}

	// Token: 0x1700036C RID: 876
	// (get) Token: 0x0600213C RID: 8508 RVA: 0x000A5C2D File Offset: 0x000A3E2D
	public SkinnedMeshRenderer ActiveBody
	{
		get
		{
			return this.GetBody(this._bodyType);
		}
	}

	// Token: 0x0600213D RID: 8509 RVA: 0x000A5C3C File Offset: 0x000A3E3C
	public static void SetAllSkeletons(bool allSkeletons)
	{
		GorillaBodyRenderer.oopsAllSkeletons = allSkeletons;
		GorillaTagger.Instance.offlineVRRig.bodyRenderer.Refresh();
		foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
		{
			vrrig.bodyRenderer.Refresh();
		}
	}

	// Token: 0x0600213E RID: 8510 RVA: 0x000A5CB4 File Offset: 0x000A3EB4
	public void SetGameModeBodyType(GorillaBodyType bodyType)
	{
		this.gameModeBodyType = bodyType;
		this.Refresh();
	}

	// Token: 0x0600213F RID: 8511 RVA: 0x000A5CC3 File Offset: 0x000A3EC3
	public void SetCosmeticBodyType(GorillaBodyType bodyType)
	{
		this.cosmeticBodyType = bodyType;
		this.Refresh();
	}

	// Token: 0x06002140 RID: 8512 RVA: 0x000A5CD2 File Offset: 0x000A3ED2
	public void SetDefaults()
	{
		this.gameModeBodyType = GorillaBodyType.Default;
		this.cosmeticBodyType = GorillaBodyType.Default;
		this.Refresh();
	}

	// Token: 0x06002141 RID: 8513 RVA: 0x000A5CE8 File Offset: 0x000A3EE8
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

	// Token: 0x06002142 RID: 8514 RVA: 0x000A5D20 File Offset: 0x000A3F20
	public void SetMaterialIndex(int materialIndex)
	{
		this.bodyDefault.sharedMaterial = this.rig.materialsToChangeTo[materialIndex];
		this.bodyNoHead.sharedMaterial = this.bodyDefault.sharedMaterial;
		this.rig.skeleton.SetMaterialIndex(materialIndex);
	}

	// Token: 0x06002143 RID: 8515 RVA: 0x000A5D6C File Offset: 0x000A3F6C
	public void SetSkinMaterials(Material bodyMat, Material chestMat)
	{
		Material[] sharedMaterials = this.bodyDefault.sharedMaterials;
		sharedMaterials[0] = bodyMat;
		sharedMaterials[1] = chestMat;
		this.bodyDefault.sharedMaterials = sharedMaterials;
		this.bodyNoHead.sharedMaterials = sharedMaterials;
	}

	// Token: 0x06002144 RID: 8516 RVA: 0x000A5DA5 File Offset: 0x000A3FA5
	public void SetupAsLocalPlayerBody()
	{
		this.faceRenderer.gameObject.layer = 22;
	}

	// Token: 0x06002145 RID: 8517 RVA: 0x000A5DBC File Offset: 0x000A3FBC
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

	// Token: 0x06002146 RID: 8518 RVA: 0x000A5E2C File Offset: 0x000A402C
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

	// Token: 0x06002147 RID: 8519 RVA: 0x000A5E75 File Offset: 0x000A4075
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x06002148 RID: 8520 RVA: 0x000A5E80 File Offset: 0x000A4080
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

	// Token: 0x040024D0 RID: 9424
	[SerializeField]
	private GorillaBodyType _bodyType;

	// Token: 0x040024D1 RID: 9425
	[SerializeField]
	private bool _renderFace = true;

	// Token: 0x040024D2 RID: 9426
	public MeshRenderer faceRenderer;

	// Token: 0x040024D3 RID: 9427
	public SkinnedMeshRenderer bodyDefault;

	// Token: 0x040024D4 RID: 9428
	public SkinnedMeshRenderer bodyNoHead;

	// Token: 0x040024D5 RID: 9429
	public SkinnedMeshRenderer bodySkeleton;

	// Token: 0x040024D6 RID: 9430
	public SkinnedMeshRenderer bodyCosmetic;

	// Token: 0x040024D7 RID: 9431
	private static bool oopsAllSkeletons;

	// Token: 0x040024D8 RID: 9432
	private GorillaBodyType gameModeBodyType;

	// Token: 0x040024D9 RID: 9433
	private GorillaBodyType cosmeticBodyType;

	// Token: 0x040024DA RID: 9434
	[Space]
	[NonSerialized]
	private SkinnedMeshRenderer[] _renderersCache = new SkinnedMeshRenderer[0];

	// Token: 0x040024DB RID: 9435
	[NonSerialized]
	private List<Material> _cachedDefaultMats = new List<Material>(2);

	// Token: 0x040024DC RID: 9436
	private static readonly List<Material> gEmptyDefaultMats = new List<Material>();

	// Token: 0x040024DD RID: 9437
	[Space]
	public VRRig rig;
}
