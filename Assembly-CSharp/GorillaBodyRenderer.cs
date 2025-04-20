using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000559 RID: 1369
public class GorillaBodyRenderer : MonoBehaviour
{
	// Token: 0x17000371 RID: 881
	// (get) Token: 0x0600218E RID: 8590 RVA: 0x00046DE6 File Offset: 0x00044FE6
	// (set) Token: 0x0600218F RID: 8591 RVA: 0x00046DEE File Offset: 0x00044FEE
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

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x06002190 RID: 8592 RVA: 0x00046DF7 File Offset: 0x00044FF7
	public bool renderFace
	{
		get
		{
			return this._renderFace;
		}
	}

	// Token: 0x06002191 RID: 8593 RVA: 0x00046DFF File Offset: 0x00044FFF
	public SkinnedMeshRenderer GetBody(GorillaBodyType type)
	{
		return this._renderersCache[(int)type];
	}

	// Token: 0x17000373 RID: 883
	// (get) Token: 0x06002192 RID: 8594 RVA: 0x00046E09 File Offset: 0x00045009
	public SkinnedMeshRenderer ActiveBody
	{
		get
		{
			return this.GetBody(this._bodyType);
		}
	}

	// Token: 0x06002193 RID: 8595 RVA: 0x000F601C File Offset: 0x000F421C
	public static void SetAllSkeletons(bool allSkeletons)
	{
		GorillaBodyRenderer.oopsAllSkeletons = allSkeletons;
		GorillaTagger.Instance.offlineVRRig.bodyRenderer.Refresh();
		foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
		{
			vrrig.bodyRenderer.Refresh();
		}
	}

	// Token: 0x06002194 RID: 8596 RVA: 0x00046E17 File Offset: 0x00045017
	public void SetGameModeBodyType(GorillaBodyType bodyType)
	{
		this.gameModeBodyType = bodyType;
		this.Refresh();
	}

	// Token: 0x06002195 RID: 8597 RVA: 0x00046E26 File Offset: 0x00045026
	public void SetCosmeticBodyType(GorillaBodyType bodyType)
	{
		this.cosmeticBodyType = bodyType;
		this.Refresh();
	}

	// Token: 0x06002196 RID: 8598 RVA: 0x00046E35 File Offset: 0x00045035
	public void SetDefaults()
	{
		this.gameModeBodyType = GorillaBodyType.Default;
		this.cosmeticBodyType = GorillaBodyType.Default;
		this.Refresh();
	}

	// Token: 0x06002197 RID: 8599 RVA: 0x000F6094 File Offset: 0x000F4294
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

	// Token: 0x06002198 RID: 8600 RVA: 0x000F60CC File Offset: 0x000F42CC
	public void SetMaterialIndex(int materialIndex)
	{
		this.bodyDefault.sharedMaterial = this.rig.materialsToChangeTo[materialIndex];
		this.bodyNoHead.sharedMaterial = this.bodyDefault.sharedMaterial;
		this.rig.skeleton.SetMaterialIndex(materialIndex);
	}

	// Token: 0x06002199 RID: 8601 RVA: 0x000F6118 File Offset: 0x000F4318
	public void SetSkinMaterials(Material bodyMat, Material chestMat)
	{
		Material[] sharedMaterials = this.bodyDefault.sharedMaterials;
		sharedMaterials[0] = bodyMat;
		sharedMaterials[1] = chestMat;
		this.bodyDefault.sharedMaterials = sharedMaterials;
		this.bodyNoHead.sharedMaterials = sharedMaterials;
	}

	// Token: 0x0600219A RID: 8602 RVA: 0x00046E4B File Offset: 0x0004504B
	public void SetupAsLocalPlayerBody()
	{
		this.faceRenderer.gameObject.layer = 22;
	}

	// Token: 0x0600219B RID: 8603 RVA: 0x000F6154 File Offset: 0x000F4354
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

	// Token: 0x0600219C RID: 8604 RVA: 0x000F61C4 File Offset: 0x000F43C4
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

	// Token: 0x0600219D RID: 8605 RVA: 0x00046E5F File Offset: 0x0004505F
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x0600219E RID: 8606 RVA: 0x000F6210 File Offset: 0x000F4410
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

	// Token: 0x04002522 RID: 9506
	[SerializeField]
	private GorillaBodyType _bodyType;

	// Token: 0x04002523 RID: 9507
	[SerializeField]
	private bool _renderFace = true;

	// Token: 0x04002524 RID: 9508
	public MeshRenderer faceRenderer;

	// Token: 0x04002525 RID: 9509
	public SkinnedMeshRenderer bodyDefault;

	// Token: 0x04002526 RID: 9510
	public SkinnedMeshRenderer bodyNoHead;

	// Token: 0x04002527 RID: 9511
	public SkinnedMeshRenderer bodySkeleton;

	// Token: 0x04002528 RID: 9512
	public SkinnedMeshRenderer bodyCosmetic;

	// Token: 0x04002529 RID: 9513
	private static bool oopsAllSkeletons;

	// Token: 0x0400252A RID: 9514
	private GorillaBodyType gameModeBodyType;

	// Token: 0x0400252B RID: 9515
	private GorillaBodyType cosmeticBodyType;

	// Token: 0x0400252C RID: 9516
	[Space]
	[NonSerialized]
	private SkinnedMeshRenderer[] _renderersCache = new SkinnedMeshRenderer[0];

	// Token: 0x0400252D RID: 9517
	[NonSerialized]
	private List<Material> _cachedDefaultMats = new List<Material>(2);

	// Token: 0x0400252E RID: 9518
	private static readonly List<Material> gEmptyDefaultMats = new List<Material>();

	// Token: 0x0400252F RID: 9519
	[Space]
	public VRRig rig;
}
