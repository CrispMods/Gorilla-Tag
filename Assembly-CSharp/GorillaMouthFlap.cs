using System;
using UnityEngine;

// Token: 0x0200058A RID: 1418
public class GorillaMouthFlap : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x060022DD RID: 8925 RVA: 0x0004797A File Offset: 0x00045B7A
	private void Start()
	{
		this.speaker = base.GetComponent<GorillaSpeakerLoudness>();
		this.targetFaceRenderer = this.targetFace.GetComponent<Renderer>();
		this.facePropBlock = new MaterialPropertyBlock();
	}

	// Token: 0x060022DE RID: 8926 RVA: 0x000479A4 File Offset: 0x00045BA4
	public void EnableLeafBlower()
	{
		this.leafBlowerActiveUntilTimestamp = Time.time + 0.1f;
	}

	// Token: 0x060022DF RID: 8927 RVA: 0x000479B7 File Offset: 0x00045BB7
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		this.lastTimeUpdated = Time.time;
		this.deltaTime = Time.deltaTime;
	}

	// Token: 0x060022E0 RID: 8928 RVA: 0x000320C8 File Offset: 0x000302C8
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x060022E1 RID: 8929 RVA: 0x000FB318 File Offset: 0x000F9518
	public void SliceUpdate()
	{
		this.deltaTime = Time.time - this.lastTimeUpdated;
		this.lastTimeUpdated = Time.time;
		if (this.speaker == null)
		{
			this.speaker = base.GetComponent<GorillaSpeakerLoudness>();
			return;
		}
		float currentLoudness = 0f;
		if (this.speaker.IsSpeaking)
		{
			currentLoudness = this.speaker.Loudness;
		}
		this.CheckMouthflapChange(this.speaker.IsMicEnabled, currentLoudness);
		MouthFlapLevel mouthFlap = this.noMicFace;
		if (this.leafBlowerActiveUntilTimestamp > Time.time)
		{
			mouthFlap = this.leafBlowerFace;
		}
		else if (this.useMicEnabled)
		{
			mouthFlap = this.mouthFlapLevels[this.activeFlipbookIndex];
		}
		this.UpdateMouthFlapFlipbook(mouthFlap);
	}

	// Token: 0x060022E2 RID: 8930 RVA: 0x000FB3CC File Offset: 0x000F95CC
	private void CheckMouthflapChange(bool isMicEnabled, float currentLoudness)
	{
		if (isMicEnabled)
		{
			this.useMicEnabled = true;
			int i = this.mouthFlapLevels.Length - 1;
			while (i >= 0)
			{
				if (currentLoudness >= this.mouthFlapLevels[i].maxRequiredVolume)
				{
					return;
				}
				if (currentLoudness > this.mouthFlapLevels[i].minRequiredVolume)
				{
					if (this.activeFlipbookIndex != i)
					{
						this.activeFlipbookIndex = i;
						this.activeFlipbookPlayTime = 0f;
						return;
					}
					return;
				}
				else
				{
					i--;
				}
			}
			return;
		}
		if (this.useMicEnabled)
		{
			this.useMicEnabled = false;
			this.activeFlipbookPlayTime = 0f;
		}
	}

	// Token: 0x060022E3 RID: 8931 RVA: 0x000FB458 File Offset: 0x000F9658
	private void UpdateMouthFlapFlipbook(MouthFlapLevel mouthFlap)
	{
		Material material = this.targetFaceRenderer.material;
		this.activeFlipbookPlayTime += this.deltaTime;
		this.activeFlipbookPlayTime %= mouthFlap.cycleDuration;
		int num = Mathf.FloorToInt(this.activeFlipbookPlayTime * (float)mouthFlap.faces.Length / mouthFlap.cycleDuration);
		material.SetTextureOffset(this._MouthMap, mouthFlap.faces[num]);
	}

	// Token: 0x060022E4 RID: 8932 RVA: 0x000FB4D0 File Offset: 0x000F96D0
	public void SetMouthTextureReplacement(Texture2D replacementMouthAtlas)
	{
		Material material = this.targetFaceRenderer.material;
		if (!this.hasDefaultMouthAtlas)
		{
			this.defaultMouthAtlas = material.GetTexture(this._MouthMap);
			this.hasDefaultMouthAtlas = true;
		}
		material.SetTexture(this._MouthMap, replacementMouthAtlas);
	}

	// Token: 0x060022E5 RID: 8933 RVA: 0x000479D6 File Offset: 0x00045BD6
	public void ClearMouthTextureReplacement()
	{
		this.targetFaceRenderer.material.SetTexture(this._MouthMap, this.defaultMouthAtlas);
	}

	// Token: 0x060022E6 RID: 8934 RVA: 0x000479F9 File Offset: 0x00045BF9
	public void SetFaceMaterialReplacement(Material replacementFaceMaterial)
	{
		if (!this.hasDefaultFaceMaterial)
		{
			this.defaultFaceMaterial = this.targetFaceRenderer.material;
			this.hasDefaultFaceMaterial = true;
		}
		this.targetFaceRenderer.material = replacementFaceMaterial;
	}

	// Token: 0x060022E7 RID: 8935 RVA: 0x00047A27 File Offset: 0x00045C27
	public void ClearFaceMaterialReplacement()
	{
		if (this.hasDefaultFaceMaterial)
		{
			this.targetFaceRenderer.material = this.defaultFaceMaterial;
		}
	}

	// Token: 0x060022E9 RID: 8937 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04002673 RID: 9843
	public GameObject targetFace;

	// Token: 0x04002674 RID: 9844
	public MouthFlapLevel[] mouthFlapLevels;

	// Token: 0x04002675 RID: 9845
	public MouthFlapLevel noMicFace;

	// Token: 0x04002676 RID: 9846
	public MouthFlapLevel leafBlowerFace;

	// Token: 0x04002677 RID: 9847
	private bool useMicEnabled;

	// Token: 0x04002678 RID: 9848
	private float leafBlowerActiveUntilTimestamp;

	// Token: 0x04002679 RID: 9849
	private int activeFlipbookIndex;

	// Token: 0x0400267A RID: 9850
	private float activeFlipbookPlayTime;

	// Token: 0x0400267B RID: 9851
	private GorillaSpeakerLoudness speaker;

	// Token: 0x0400267C RID: 9852
	private float lastTimeUpdated;

	// Token: 0x0400267D RID: 9853
	private float deltaTime;

	// Token: 0x0400267E RID: 9854
	private Renderer targetFaceRenderer;

	// Token: 0x0400267F RID: 9855
	private MaterialPropertyBlock facePropBlock;

	// Token: 0x04002680 RID: 9856
	private Texture defaultMouthAtlas;

	// Token: 0x04002681 RID: 9857
	private Material defaultFaceMaterial;

	// Token: 0x04002682 RID: 9858
	private bool hasDefaultMouthAtlas;

	// Token: 0x04002683 RID: 9859
	private bool hasDefaultFaceMaterial;

	// Token: 0x04002684 RID: 9860
	private ShaderHashId _MouthMap = "_MouthMap";

	// Token: 0x04002685 RID: 9861
	private ShaderHashId _BaseMap = "_BaseMap";
}
