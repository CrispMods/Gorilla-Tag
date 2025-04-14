using System;
using UnityEngine;

// Token: 0x020008E3 RID: 2275
public class VoiceLoudnessReactor : MonoBehaviour
{
	// Token: 0x060036BB RID: 14011 RVA: 0x00103238 File Offset: 0x00101438
	private void Start()
	{
		for (int i = 0; i < this.transformPositionTargets.Length; i++)
		{
			this.transformPositionTargets[i].Initial = this.transformPositionTargets[i].transform.localPosition;
		}
		for (int j = 0; j < this.transformScaleTargets.Length; j++)
		{
			this.transformScaleTargets[j].Initial = this.transformScaleTargets[j].transform.localScale;
		}
		for (int k = 0; k < this.transformRotationTargets.Length; k++)
		{
			this.transformRotationTargets[k].Initial = this.transformRotationTargets[k].transform.localRotation;
		}
		for (int l = 0; l < this.particleTargets.Length; l++)
		{
			this.particleTargets[l].Main = this.particleTargets[l].particleSystem.main;
			this.particleTargets[l].InitialSpeed = this.particleTargets[l].Main.startSpeedMultiplier;
			this.particleTargets[l].InitialSize = this.particleTargets[l].Main.startSizeMultiplier;
			this.particleTargets[l].Emission = this.particleTargets[l].particleSystem.emission;
			this.particleTargets[l].InitialRate = this.particleTargets[l].Emission.rateOverTimeMultiplier;
			this.particleTargets[l].Main.startSpeedMultiplier = 0f;
			this.particleTargets[l].Main.startSizeMultiplier = 0f;
			this.particleTargets[l].Emission.rateOverTimeMultiplier = 0f;
		}
		for (int m = 0; m < this.gameObjectEnableTargets.Length; m++)
		{
			this.gameObjectEnableTargets[m].GameObject.SetActive(!this.gameObjectEnableTargets[m].TurnOnAtThreshhold);
		}
		for (int n = 0; n < this.rendererColorTargets.Length; n++)
		{
			this.rendererColorTargets[n].Inititialize();
		}
	}

	// Token: 0x060036BC RID: 14012 RVA: 0x00103434 File Offset: 0x00101634
	private void OnEnable()
	{
		if (this.loudness != null)
		{
			return;
		}
		this.loudness = base.GetComponentInParent<GorillaSpeakerLoudness>(true);
		if (this.loudness == null)
		{
			GorillaTagger componentInParent = base.GetComponentInParent<GorillaTagger>();
			if (componentInParent != null)
			{
				this.loudness = componentInParent.offlineVRRig.GetComponent<GorillaSpeakerLoudness>();
			}
		}
		if (this.loudness != null)
		{
			this.frameLoudness = this.loudness.Loudness;
			this.frameSmoothedLoudness = this.loudness.SmoothedLoudness;
		}
	}

	// Token: 0x060036BD RID: 14013 RVA: 0x001034BC File Offset: 0x001016BC
	private void Update()
	{
		if (this.loudness == null)
		{
			return;
		}
		float num = this.loudness.Loudness;
		float num2 = this.frameLoudness;
		float num3 = this.loudness.Loudness;
		float num4 = this.frameLoudness;
		if (this.attack > 0f && this.loudness.Loudness > this.frameLoudness)
		{
			this.frameLoudness = Mathf.MoveTowards(this.frameLoudness, this.loudness.Loudness, Time.deltaTime / this.attack);
		}
		else if (this.decay > 0f && this.loudness.Loudness < this.frameLoudness)
		{
			this.frameLoudness = Mathf.MoveTowards(this.frameLoudness, this.loudness.Loudness, Time.deltaTime / this.decay);
		}
		else
		{
			this.frameLoudness = this.loudness.Loudness;
		}
		if (this.attack > 0f && this.loudness.SmoothedLoudness > this.frameSmoothedLoudness)
		{
			this.frameSmoothedLoudness = Mathf.MoveTowards(this.frameLoudness, this.loudness.SmoothedLoudness, Time.deltaTime * this.attack);
		}
		else if (this.decay > 0f && this.loudness.SmoothedLoudness < this.frameSmoothedLoudness)
		{
			this.frameSmoothedLoudness = Mathf.MoveTowards(this.frameLoudness, this.loudness.SmoothedLoudness, Time.deltaTime * this.decay);
		}
		else
		{
			this.frameSmoothedLoudness = this.loudness.SmoothedLoudness;
		}
		for (int i = 0; i < this.blendShapeTargets.Length; i++)
		{
			float t = this.blendShapeTargets[i].UseSmoothedLoudness ? this.frameSmoothedLoudness : this.frameLoudness;
			this.blendShapeTargets[i].SkinnedMeshRenderer.SetBlendShapeWeight(this.blendShapeTargets[i].BlendShapeIndex, Mathf.Lerp(this.blendShapeTargets[i].minValue, this.blendShapeTargets[i].maxValue, t));
		}
		for (int j = 0; j < this.transformPositionTargets.Length; j++)
		{
			float t2 = (this.transformPositionTargets[j].UseSmoothedLoudness ? this.frameSmoothedLoudness : this.frameLoudness) * this.transformPositionTargets[j].Scale;
			this.transformPositionTargets[j].transform.localPosition = Vector3.Lerp(this.transformPositionTargets[j].Initial, this.transformPositionTargets[j].Max, t2);
		}
		for (int k = 0; k < this.transformScaleTargets.Length; k++)
		{
			float t3 = (this.transformScaleTargets[k].UseSmoothedLoudness ? this.frameSmoothedLoudness : this.frameLoudness) * this.transformScaleTargets[k].Scale;
			this.transformScaleTargets[k].transform.localScale = Vector3.Lerp(this.transformScaleTargets[k].Initial, this.transformScaleTargets[k].Max, t3);
		}
		for (int l = 0; l < this.transformRotationTargets.Length; l++)
		{
			float t4 = (this.transformRotationTargets[l].UseSmoothedLoudness ? this.frameSmoothedLoudness : this.frameLoudness) * this.transformRotationTargets[l].Scale;
			this.transformRotationTargets[l].transform.localRotation = Quaternion.Slerp(this.transformRotationTargets[l].Initial, this.transformRotationTargets[l].Max, t4);
		}
		for (int m = 0; m < this.particleTargets.Length; m++)
		{
			float time = (this.particleTargets[m].UseSmoothedLoudness ? this.frameSmoothedLoudness : this.frameLoudness) * this.particleTargets[m].Scale;
			this.particleTargets[m].Main.startSpeedMultiplier = this.particleTargets[m].InitialSpeed * this.particleTargets[m].speed.Evaluate(time);
			this.particleTargets[m].Main.startSizeMultiplier = this.particleTargets[m].InitialSize * this.particleTargets[m].size.Evaluate(time);
			this.particleTargets[m].Emission.rateOverTimeMultiplier = this.particleTargets[m].InitialRate * this.particleTargets[m].rate.Evaluate(time);
		}
		for (int n = 0; n < this.gameObjectEnableTargets.Length; n++)
		{
			bool flag = (this.gameObjectEnableTargets[n].UseSmoothedLoudness ? this.frameSmoothedLoudness : (this.frameLoudness * this.gameObjectEnableTargets[n].Scale)) >= this.gameObjectEnableTargets[n].Threshold;
			if (!this.gameObjectEnableTargets[n].TurnOnAtThreshhold)
			{
				flag = !flag;
			}
			if (this.gameObjectEnableTargets[n].GameObject.activeInHierarchy != flag)
			{
				this.gameObjectEnableTargets[n].GameObject.SetActive(flag);
			}
		}
		for (int num5 = 0; num5 < this.rendererColorTargets.Length; num5++)
		{
			VoiceLoudnessReactorRendererColorTarget voiceLoudnessReactorRendererColorTarget = this.rendererColorTargets[num5];
			float level = voiceLoudnessReactorRendererColorTarget.useSmoothedLoudness ? this.frameSmoothedLoudness : (this.frameLoudness * voiceLoudnessReactorRendererColorTarget.scale);
			voiceLoudnessReactorRendererColorTarget.UpdateMaterialColor(level);
		}
		for (int num6 = 0; num6 < this.animatorTargets.Length; num6++)
		{
			VoiceLoudnessReactorAnimatorTarget voiceLoudnessReactorAnimatorTarget = this.animatorTargets[num6];
			float num7 = voiceLoudnessReactorAnimatorTarget.useSmoothedLoudness ? this.frameSmoothedLoudness : this.frameLoudness;
			if (voiceLoudnessReactorAnimatorTarget.animatorSpeedToLoudness < 0f)
			{
				voiceLoudnessReactorAnimatorTarget.animator.speed = Mathf.Max(0f, (1f - num7) * -voiceLoudnessReactorAnimatorTarget.animatorSpeedToLoudness);
			}
			else
			{
				voiceLoudnessReactorAnimatorTarget.animator.speed = Mathf.Max(0f, num7 * voiceLoudnessReactorAnimatorTarget.animatorSpeedToLoudness);
			}
		}
	}

	// Token: 0x0400399C RID: 14748
	private GorillaSpeakerLoudness loudness;

	// Token: 0x0400399D RID: 14749
	[SerializeField]
	private VoiceLoudnessReactorBlendShapeTarget[] blendShapeTargets = new VoiceLoudnessReactorBlendShapeTarget[0];

	// Token: 0x0400399E RID: 14750
	[SerializeField]
	private VoiceLoudnessReactorTransformTarget[] transformPositionTargets = new VoiceLoudnessReactorTransformTarget[0];

	// Token: 0x0400399F RID: 14751
	[SerializeField]
	private VoiceLoudnessReactorTransformRotationTarget[] transformRotationTargets = new VoiceLoudnessReactorTransformRotationTarget[0];

	// Token: 0x040039A0 RID: 14752
	[SerializeField]
	private VoiceLoudnessReactorTransformTarget[] transformScaleTargets = new VoiceLoudnessReactorTransformTarget[0];

	// Token: 0x040039A1 RID: 14753
	[SerializeField]
	private VoiceLoudnessReactorParticleSystemTarget[] particleTargets = new VoiceLoudnessReactorParticleSystemTarget[0];

	// Token: 0x040039A2 RID: 14754
	[SerializeField]
	private VoiceLoudnessReactorGameObjectEnableTarget[] gameObjectEnableTargets = new VoiceLoudnessReactorGameObjectEnableTarget[0];

	// Token: 0x040039A3 RID: 14755
	[SerializeField]
	private VoiceLoudnessReactorRendererColorTarget[] rendererColorTargets = new VoiceLoudnessReactorRendererColorTarget[0];

	// Token: 0x040039A4 RID: 14756
	[SerializeField]
	private VoiceLoudnessReactorAnimatorTarget[] animatorTargets = new VoiceLoudnessReactorAnimatorTarget[0];

	// Token: 0x040039A5 RID: 14757
	private float frameLoudness;

	// Token: 0x040039A6 RID: 14758
	private float frameSmoothedLoudness;

	// Token: 0x040039A7 RID: 14759
	[SerializeField]
	private float attack;

	// Token: 0x040039A8 RID: 14760
	[SerializeField]
	private float decay;
}
