using System;
using UnityEngine;

// Token: 0x0200057D RID: 1405
public class GorillaMouthFlap : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06002287 RID: 8839 RVA: 0x000465D5 File Offset: 0x000447D5
	private void Start()
	{
		this.speaker = base.GetComponent<GorillaSpeakerLoudness>();
		this.targetFaceRenderer = this.targetFace.GetComponent<Renderer>();
		this.facePropBlock = new MaterialPropertyBlock();
	}

	// Token: 0x06002288 RID: 8840 RVA: 0x000465FF File Offset: 0x000447FF
	public void EnableLeafBlower()
	{
		this.leafBlowerActiveUntilTimestamp = Time.time + 0.1f;
	}

	// Token: 0x06002289 RID: 8841 RVA: 0x00046612 File Offset: 0x00044812
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		this.lastTimeUpdated = Time.time;
		this.deltaTime = Time.deltaTime;
	}

	// Token: 0x0600228A RID: 8842 RVA: 0x00030F5E File Offset: 0x0002F15E
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x0600228B RID: 8843 RVA: 0x000F859C File Offset: 0x000F679C
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

	// Token: 0x0600228C RID: 8844 RVA: 0x000F8650 File Offset: 0x000F6850
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

	// Token: 0x0600228D RID: 8845 RVA: 0x000F86DC File Offset: 0x000F68DC
	private void UpdateMouthFlapFlipbook(MouthFlapLevel mouthFlap)
	{
		Material material = this.targetFaceRenderer.material;
		this.activeFlipbookPlayTime += this.deltaTime;
		this.activeFlipbookPlayTime %= mouthFlap.cycleDuration;
		int num = Mathf.FloorToInt(this.activeFlipbookPlayTime * (float)mouthFlap.faces.Length / mouthFlap.cycleDuration);
		material.SetTextureOffset(this._MouthMap, mouthFlap.faces[num]);
	}

	// Token: 0x0600228E RID: 8846 RVA: 0x000F8754 File Offset: 0x000F6954
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

	// Token: 0x0600228F RID: 8847 RVA: 0x00046631 File Offset: 0x00044831
	public void ClearMouthTextureReplacement()
	{
		this.targetFaceRenderer.material.SetTexture(this._MouthMap, this.defaultMouthAtlas);
	}

	// Token: 0x06002291 RID: 8849 RVA: 0x00030F9B File Offset: 0x0002F19B
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04002621 RID: 9761
	public GameObject targetFace;

	// Token: 0x04002622 RID: 9762
	public MouthFlapLevel[] mouthFlapLevels;

	// Token: 0x04002623 RID: 9763
	public MouthFlapLevel noMicFace;

	// Token: 0x04002624 RID: 9764
	public MouthFlapLevel leafBlowerFace;

	// Token: 0x04002625 RID: 9765
	private bool useMicEnabled;

	// Token: 0x04002626 RID: 9766
	private float leafBlowerActiveUntilTimestamp;

	// Token: 0x04002627 RID: 9767
	private int activeFlipbookIndex;

	// Token: 0x04002628 RID: 9768
	private float activeFlipbookPlayTime;

	// Token: 0x04002629 RID: 9769
	private GorillaSpeakerLoudness speaker;

	// Token: 0x0400262A RID: 9770
	private float lastTimeUpdated;

	// Token: 0x0400262B RID: 9771
	private float deltaTime;

	// Token: 0x0400262C RID: 9772
	private Renderer targetFaceRenderer;

	// Token: 0x0400262D RID: 9773
	private MaterialPropertyBlock facePropBlock;

	// Token: 0x0400262E RID: 9774
	private Texture defaultMouthAtlas;

	// Token: 0x0400262F RID: 9775
	private bool hasDefaultMouthAtlas;

	// Token: 0x04002630 RID: 9776
	private ShaderHashId _MouthMap = "_MouthMap";
}
