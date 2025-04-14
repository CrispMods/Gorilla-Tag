using System;
using UnityEngine;

// Token: 0x0200057C RID: 1404
public class GorillaMouthFlap : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x0600227F RID: 8831 RVA: 0x000AB623 File Offset: 0x000A9823
	private void Start()
	{
		this.speaker = base.GetComponent<GorillaSpeakerLoudness>();
		this.targetFaceRenderer = this.targetFace.GetComponent<Renderer>();
		this.facePropBlock = new MaterialPropertyBlock();
	}

	// Token: 0x06002280 RID: 8832 RVA: 0x000AB64D File Offset: 0x000A984D
	public void EnableLeafBlower()
	{
		this.leafBlowerActiveUntilTimestamp = Time.time + 0.1f;
	}

	// Token: 0x06002281 RID: 8833 RVA: 0x000AB660 File Offset: 0x000A9860
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		this.lastTimeUpdated = Time.time;
		this.deltaTime = Time.deltaTime;
	}

	// Token: 0x06002282 RID: 8834 RVA: 0x0000F86B File Offset: 0x0000DA6B
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06002283 RID: 8835 RVA: 0x000AB680 File Offset: 0x000A9880
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

	// Token: 0x06002284 RID: 8836 RVA: 0x000AB734 File Offset: 0x000A9934
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

	// Token: 0x06002285 RID: 8837 RVA: 0x000AB7C0 File Offset: 0x000A99C0
	private void UpdateMouthFlapFlipbook(MouthFlapLevel mouthFlap)
	{
		Material material = this.targetFaceRenderer.material;
		this.activeFlipbookPlayTime += this.deltaTime;
		this.activeFlipbookPlayTime %= mouthFlap.cycleDuration;
		int num = Mathf.FloorToInt(this.activeFlipbookPlayTime * (float)mouthFlap.faces.Length / mouthFlap.cycleDuration);
		material.SetTextureOffset(this._MouthMap, mouthFlap.faces[num]);
	}

	// Token: 0x06002286 RID: 8838 RVA: 0x000AB838 File Offset: 0x000A9A38
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

	// Token: 0x06002287 RID: 8839 RVA: 0x000AB889 File Offset: 0x000A9A89
	public void ClearMouthTextureReplacement()
	{
		this.targetFaceRenderer.material.SetTexture(this._MouthMap, this.defaultMouthAtlas);
	}

	// Token: 0x06002289 RID: 8841 RVA: 0x0000F974 File Offset: 0x0000DB74
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x0400261B RID: 9755
	public GameObject targetFace;

	// Token: 0x0400261C RID: 9756
	public MouthFlapLevel[] mouthFlapLevels;

	// Token: 0x0400261D RID: 9757
	public MouthFlapLevel noMicFace;

	// Token: 0x0400261E RID: 9758
	public MouthFlapLevel leafBlowerFace;

	// Token: 0x0400261F RID: 9759
	private bool useMicEnabled;

	// Token: 0x04002620 RID: 9760
	private float leafBlowerActiveUntilTimestamp;

	// Token: 0x04002621 RID: 9761
	private int activeFlipbookIndex;

	// Token: 0x04002622 RID: 9762
	private float activeFlipbookPlayTime;

	// Token: 0x04002623 RID: 9763
	private GorillaSpeakerLoudness speaker;

	// Token: 0x04002624 RID: 9764
	private float lastTimeUpdated;

	// Token: 0x04002625 RID: 9765
	private float deltaTime;

	// Token: 0x04002626 RID: 9766
	private Renderer targetFaceRenderer;

	// Token: 0x04002627 RID: 9767
	private MaterialPropertyBlock facePropBlock;

	// Token: 0x04002628 RID: 9768
	private Texture defaultMouthAtlas;

	// Token: 0x04002629 RID: 9769
	private bool hasDefaultMouthAtlas;

	// Token: 0x0400262A RID: 9770
	private ShaderHashId _MouthMap = "_MouthMap";
}
