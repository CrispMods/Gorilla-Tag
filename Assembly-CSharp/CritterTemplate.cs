using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000069 RID: 105
public class CritterTemplate : ScriptableObject
{
	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060002A4 RID: 676 RVA: 0x000746D8 File Offset: 0x000728D8
	private string HapticsBlurb
	{
		get
		{
			float num = this.grabbedStruggleHaptics.GetPeakMagnitude() * this.grabbedStruggleHapticsStrength;
			float num2 = this.grabbedStruggleHaptics.GetRMSMagnitude() * this.grabbedStruggleHapticsStrength;
			return string.Format("Peak Strength: {0:0.##} Mean Strength: {1:0.##}", num, num2);
		}
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x00074724 File Offset: 0x00072924
	private void SetMaxStrength(float maxStrength = 1f)
	{
		float peakMagnitude = this.grabbedStruggleHaptics.GetPeakMagnitude();
		Debug.Log(string.Format("Clip {0} max strength: {1}", this.grabbedStruggleHaptics, peakMagnitude));
		if (peakMagnitude > 0f)
		{
			this.grabbedStruggleHapticsStrength = maxStrength / peakMagnitude;
		}
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x0007476C File Offset: 0x0007296C
	private void SetMeanStrength(float meanStrength = 1f)
	{
		float rmsmagnitude = this.grabbedStruggleHaptics.GetRMSMagnitude();
		Debug.Log(string.Format("Clip {0} mean strength: {1}", this.grabbedStruggleHaptics, rmsmagnitude));
		if (meanStrength > 0f)
		{
			this.grabbedStruggleHapticsStrength = meanStrength / rmsmagnitude;
		}
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x0003210D File Offset: 0x0003030D
	private void OnValidate()
	{
		this.modifiedValues.Clear();
		this.RegisterModifiedBehaviour();
		this.RegisterModifiedVisual();
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x00032126 File Offset: 0x00030326
	private void OnEnable()
	{
		this.OnValidate();
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x000747B4 File Offset: 0x000729B4
	private void RegisterModifiedBehaviour()
	{
		if (this.maxJumpVel != 0f)
		{
			this.modifiedValues.Add("maxJumpVel", this.maxJumpVel);
		}
		if (this.jumpCooldown != 0f)
		{
			this.modifiedValues.Add("jumpCooldown", this.jumpCooldown);
		}
		if (this.scaredJumpCooldown != 0f)
		{
			this.modifiedValues.Add("scaredJumpCooldown", this.scaredJumpCooldown);
		}
		if (this.jumpVariabilityTime != 0f)
		{
			this.modifiedValues.Add("jumpVariabilityTime", this.jumpVariabilityTime);
		}
		if (this.visionConeAngle != 0f)
		{
			this.modifiedValues.Add("visionConeAngle", this.visionConeAngle);
		}
		if (this.sensoryRange != 0f)
		{
			this.modifiedValues.Add("sensoryRange", this.sensoryRange);
		}
		if (this.maxHunger != 0f)
		{
			this.modifiedValues.Add("maxHunger", this.maxHunger);
		}
		if (this.hungryThreshold != 0f)
		{
			this.modifiedValues.Add("hungryThreshold", this.hungryThreshold);
		}
		if (this.satiatedThreshold != 0f)
		{
			this.modifiedValues.Add("satiatedThreshold", this.satiatedThreshold);
		}
		if (this.hungerLostPerSecond != 0f)
		{
			this.modifiedValues.Add("hungerLostPerSecond", this.hungerLostPerSecond);
		}
		if (this.hungerGainedPerSecond != 0f)
		{
			this.modifiedValues.Add("hungerGainedPerSecond", this.hungerGainedPerSecond);
		}
		if (this.maxFear != 0f)
		{
			this.modifiedValues.Add("maxFear", this.maxFear);
		}
		if (this.scaredThreshold != 0f)
		{
			this.modifiedValues.Add("scaredThreshold", this.scaredThreshold);
		}
		if (this.calmThreshold != 0f)
		{
			this.modifiedValues.Add("calmThreshold", this.calmThreshold);
		}
		if (this.fearLostPerSecond != 0f)
		{
			this.modifiedValues.Add("fearLostPerSecond", this.fearLostPerSecond);
		}
		if (this.maxAttraction != 0f)
		{
			this.modifiedValues.Add("maxAttraction", this.maxAttraction);
		}
		if (this.attractedThreshold != 0f)
		{
			this.modifiedValues.Add("attractedThreshold", this.attractedThreshold);
		}
		if (this.unattractedThreshold != 0f)
		{
			this.modifiedValues.Add("unattractedThreshold", this.unattractedThreshold);
		}
		if (this.attractionLostPerSecond != 0f)
		{
			this.modifiedValues.Add("attractionLostPerSecond", this.attractionLostPerSecond);
		}
		if (this.maxSleepiness != 0f)
		{
			this.modifiedValues.Add("maxSleepiness", this.maxSleepiness);
		}
		if (this.tiredThreshold != 0f)
		{
			this.modifiedValues.Add("tiredThreshold", this.tiredThreshold);
		}
		if (this.awakeThreshold != 0f)
		{
			this.modifiedValues.Add("awakeThreshold", this.awakeThreshold);
		}
		if (this.sleepinessGainedPerSecond != 0f)
		{
			this.modifiedValues.Add("sleepinessGainedPerSecond", this.sleepinessGainedPerSecond);
		}
		if (this.sleepinessLostPerSecond != 0f)
		{
			this.modifiedValues.Add("sleepinessLostPerSecond", this.sleepinessLostPerSecond);
		}
		if (this.maxStruggle != 0f)
		{
			this.modifiedValues.Add("maxStruggle", this.maxStruggle);
		}
		if (this.escapeThreshold != 0f)
		{
			this.modifiedValues.Add("escapeThreshold", this.escapeThreshold);
		}
		if (this.catchableThreshold != 0f)
		{
			this.modifiedValues.Add("catchableThreshold", this.catchableThreshold);
		}
		if (this.struggleGainedPerSecond != 0f)
		{
			this.modifiedValues.Add("struggleGainedPerSecond", this.struggleGainedPerSecond);
		}
		if (this.struggleLostPerSecond != 0f)
		{
			this.modifiedValues.Add("struggleLostPerSecond", this.struggleLostPerSecond);
		}
		if (this.afraidOfList != null)
		{
			this.modifiedValues.Add("afraidOfList", this.afraidOfList);
		}
		if (this.attractedToList != null)
		{
			this.modifiedValues.Add("attractedToList", this.attractedToList);
		}
		if (this.lifeTime != 0f)
		{
			this.modifiedValues.Add("lifeTime", this.lifeTime);
		}
	}

	// Token: 0x060002AA RID: 682 RVA: 0x00074CB0 File Offset: 0x00072EB0
	private void RegisterModifiedVisual()
	{
		if (this.hatChance != 0f)
		{
			this.modifiedValues.Add("hatChance", this.hatChance);
		}
		if (this.hats != null && this.hats.Length != 0)
		{
			this.modifiedValues.Add("hats", this.hats);
		}
		if (this.minSize != 0f)
		{
			this.modifiedValues.Add("minSize", this.minSize);
		}
		if (this.maxSize != 0f)
		{
			this.modifiedValues.Add("maxSize", this.maxSize);
		}
		if (this.eatingStartFX != null)
		{
			this.modifiedValues.Add("eatingStartFX", this.eatingStartFX);
		}
		if (this.eatingOngoingFX != null)
		{
			this.modifiedValues.Add("eatingOngoingFX", this.eatingOngoingFX);
		}
		if (CrittersAnim.IsModified(this.eatingAnim))
		{
			this.modifiedValues.Add("eatingAnim", this.eatingAnim);
		}
		if (this.fearStartFX != null)
		{
			this.modifiedValues.Add("fearStartFX", this.fearStartFX);
		}
		if (this.fearOngoingFX != null)
		{
			this.modifiedValues.Add("fearOngoingFX", this.fearOngoingFX);
		}
		if (CrittersAnim.IsModified(this.fearAnim))
		{
			this.modifiedValues.Add("fearAnim", this.fearAnim);
		}
		if (this.attractionStartFX != null)
		{
			this.modifiedValues.Add("attractionStartFX", this.attractionStartFX);
		}
		if (this.attractionOngoingFX != null)
		{
			this.modifiedValues.Add("attractionOngoingFX", this.attractionOngoingFX);
		}
		if (CrittersAnim.IsModified(this.attractionAnim))
		{
			this.modifiedValues.Add("attractionAnim", this.attractionAnim);
		}
		if (this.sleepStartFX != null)
		{
			this.modifiedValues.Add("sleepStartFX", this.sleepStartFX);
		}
		if (this.sleepOngoingFX != null)
		{
			this.modifiedValues.Add("sleepOngoingFX", this.sleepOngoingFX);
		}
		if (CrittersAnim.IsModified(this.sleepAnim))
		{
			this.modifiedValues.Add("sleepAnim", this.sleepAnim);
		}
		if (this.grabbedStartFX != null)
		{
			this.modifiedValues.Add("grabbedStartFX", this.grabbedStartFX);
		}
		if (this.grabbedOngoingFX != null)
		{
			this.modifiedValues.Add("grabbedOngoingFX", this.grabbedOngoingFX);
		}
		if (this.grabbedStopFX != null)
		{
			this.modifiedValues.Add("grabbedStopFX", this.grabbedStopFX);
		}
		if (CrittersAnim.IsModified(this.grabbedAnim))
		{
			this.modifiedValues.Add("grabbedAnim", this.grabbedAnim);
		}
		if (this.hungryStartFX != null)
		{
			this.modifiedValues.Add("hungryStartFX", this.hungryStartFX);
		}
		if (this.hungryOngoingFX != null)
		{
			this.modifiedValues.Add("hungryOngoingFX", this.hungryOngoingFX);
		}
		if (CrittersAnim.IsModified(this.hungryAnim))
		{
			this.modifiedValues.Add("hungryAnim", this.hungryAnim);
		}
		if (this.despawningStartFX != null)
		{
			this.modifiedValues.Add("despawningStartFX", this.despawningStartFX);
		}
		if (this.despawningOngoingFX != null)
		{
			this.modifiedValues.Add("despawningOngoingFX", this.despawningOngoingFX);
		}
		if (CrittersAnim.IsModified(this.despawningAnim))
		{
			this.modifiedValues.Add("despawningAnim", this.despawningAnim);
		}
		if (this.spawningStartFX != null)
		{
			this.modifiedValues.Add("spawningStartFX", this.spawningStartFX);
		}
		if (this.spawningOngoingFX != null)
		{
			this.modifiedValues.Add("spawningOngoingFX", this.spawningOngoingFX);
		}
		if (CrittersAnim.IsModified(this.spawningAnim))
		{
			this.modifiedValues.Add("spawningAnim", this.spawningAnim);
		}
		if (this.capturedStartFX != null)
		{
			this.modifiedValues.Add("capturedStartFX", this.capturedStartFX);
		}
		if (this.capturedOngoingFX != null)
		{
			this.modifiedValues.Add("capturedOngoingFX", this.capturedOngoingFX);
		}
		if (CrittersAnim.IsModified(this.capturedAnim))
		{
			this.modifiedValues.Add("capturedAnim", this.capturedAnim);
		}
		if (this.stunnedStartFX != null)
		{
			this.modifiedValues.Add("stunnedStartFX", this.stunnedStartFX);
		}
		if (this.stunnedOngoingFX != null)
		{
			this.modifiedValues.Add("stunnedOngoingFX", this.stunnedOngoingFX);
		}
		if (CrittersAnim.IsModified(this.stunnedAnim))
		{
			this.modifiedValues.Add("stunnedAnim", this.stunnedAnim);
		}
		if (this.grabbedStruggleHaptics != null)
		{
			this.modifiedValues.Add("grabbedStruggleHaptics", this.grabbedStruggleHaptics);
		}
		if (this.grabbedStruggleHapticsStrength != 0f)
		{
			this.modifiedValues.Add("grabbedStruggleHapticsStrength", this.grabbedStruggleHapticsStrength);
		}
	}

	// Token: 0x060002AB RID: 683 RVA: 0x0003212E File Offset: 0x0003032E
	public bool IsValueModified(string valueName)
	{
		return this.modifiedValues.ContainsKey(valueName);
	}

	// Token: 0x060002AC RID: 684 RVA: 0x000751FC File Offset: 0x000733FC
	public T GetParentValue<T>(string valueName)
	{
		if (this.parent != null)
		{
			return this.parent.GetTemplateValue<T>(valueName);
		}
		return default(T);
	}

	// Token: 0x060002AD RID: 685 RVA: 0x00075230 File Offset: 0x00073430
	public T GetTemplateValue<T>(string valueName)
	{
		object obj;
		if (this.modifiedValues.TryGetValue(valueName, out obj))
		{
			return (T)((object)obj);
		}
		if (this.parent != null)
		{
			return this.parent.GetTemplateValue<T>(valueName);
		}
		return default(T);
	}

	// Token: 0x060002AE RID: 686 RVA: 0x0003213C File Offset: 0x0003033C
	public void ApplyToCritter(CrittersPawn critter)
	{
		this.ApplyBehaviour(critter);
		this.ApplyBehaviourFX(critter);
	}

	// Token: 0x060002AF RID: 687 RVA: 0x00075278 File Offset: 0x00073478
	private void ApplyBehaviour(CrittersPawn critter)
	{
		critter.maxJumpVel = this.GetTemplateValue<float>("maxJumpVel");
		critter.jumpCooldown = this.GetTemplateValue<float>("jumpCooldown");
		critter.scaredJumpCooldown = this.GetTemplateValue<float>("scaredJumpCooldown");
		critter.jumpVariabilityTime = this.GetTemplateValue<float>("jumpVariabilityTime");
		critter.visionConeAngle = this.GetTemplateValue<float>("visionConeAngle");
		critter.sensoryRange = this.GetTemplateValue<float>("sensoryRange");
		critter.maxHunger = this.GetTemplateValue<float>("maxHunger");
		critter.hungryThreshold = this.GetTemplateValue<float>("hungryThreshold");
		critter.satiatedThreshold = this.GetTemplateValue<float>("satiatedThreshold");
		critter.hungerLostPerSecond = this.GetTemplateValue<float>("hungerLostPerSecond");
		critter.hungerGainedPerSecond = this.GetTemplateValue<float>("hungerGainedPerSecond");
		critter.maxFear = this.GetTemplateValue<float>("maxFear");
		critter.scaredThreshold = this.GetTemplateValue<float>("scaredThreshold");
		critter.calmThreshold = this.GetTemplateValue<float>("calmThreshold");
		critter.fearLostPerSecond = this.GetTemplateValue<float>("fearLostPerSecond");
		critter.maxAttraction = this.GetTemplateValue<float>("maxAttraction");
		critter.attractedThreshold = this.GetTemplateValue<float>("attractedThreshold");
		critter.unattractedThreshold = this.GetTemplateValue<float>("unattractedThreshold");
		critter.attractionLostPerSecond = this.GetTemplateValue<float>("attractionLostPerSecond");
		critter.maxSleepiness = this.GetTemplateValue<float>("maxSleepiness");
		critter.tiredThreshold = this.GetTemplateValue<float>("tiredThreshold");
		critter.awakeThreshold = this.GetTemplateValue<float>("awakeThreshold");
		critter.sleepinessGainedPerSecond = this.GetTemplateValue<float>("sleepinessGainedPerSecond");
		critter.sleepinessLostPerSecond = this.GetTemplateValue<float>("sleepinessLostPerSecond");
		critter.maxStruggle = this.GetTemplateValue<float>("maxStruggle");
		critter.escapeThreshold = this.GetTemplateValue<float>("escapeThreshold");
		critter.catchableThreshold = this.GetTemplateValue<float>("catchableThreshold");
		critter.struggleGainedPerSecond = this.GetTemplateValue<float>("struggleGainedPerSecond");
		critter.struggleLostPerSecond = this.GetTemplateValue<float>("struggleLostPerSecond");
		critter.lifeTime = (double)this.GetTemplateValue<float>("lifeTime");
		critter.attractedToList = this.GetTemplateValue<List<crittersAttractorStruct>>("attractedToList");
		critter.afraidOfList = this.GetTemplateValue<List<crittersAttractorStruct>>("afraidOfList");
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x000754A8 File Offset: 0x000736A8
	private void ApplyBehaviourFX(CrittersPawn critter)
	{
		critter.StartStateFX.Clear();
		critter.OngoingStateFX.Clear();
		critter.stateAnim.Clear();
		critter.StartStateFX.Add(CrittersPawn.CreatureState.Eating, this.GetTemplateValue<GameObject>("eatingStartFX"));
		critter.OngoingStateFX.Add(CrittersPawn.CreatureState.Eating, this.GetTemplateValue<GameObject>("eatingOngoingFX"));
		critter.stateAnim.Add(CrittersPawn.CreatureState.Eating, this.GetTemplateValue<CrittersAnim>("eatingAnim"));
		critter.StartStateFX.Add(CrittersPawn.CreatureState.Running, this.GetTemplateValue<GameObject>("fearStartFX"));
		critter.OngoingStateFX.Add(CrittersPawn.CreatureState.Running, this.GetTemplateValue<GameObject>("fearOngoingFX"));
		critter.stateAnim.Add(CrittersPawn.CreatureState.Running, this.GetTemplateValue<CrittersAnim>("fearAnim"));
		critter.StartStateFX.Add(CrittersPawn.CreatureState.AttractedTo, this.GetTemplateValue<GameObject>("attractionStartFX"));
		critter.OngoingStateFX.Add(CrittersPawn.CreatureState.AttractedTo, this.GetTemplateValue<GameObject>("attractionOngoingFX"));
		critter.stateAnim.Add(CrittersPawn.CreatureState.AttractedTo, this.GetTemplateValue<CrittersAnim>("attractionAnim"));
		critter.StartStateFX.Add(CrittersPawn.CreatureState.Sleeping, this.GetTemplateValue<GameObject>("sleepStartFX"));
		critter.OngoingStateFX.Add(CrittersPawn.CreatureState.Sleeping, this.GetTemplateValue<GameObject>("sleepOngoingFX"));
		critter.stateAnim.Add(CrittersPawn.CreatureState.Sleeping, this.GetTemplateValue<CrittersAnim>("sleepAnim"));
		critter.StartStateFX.Add(CrittersPawn.CreatureState.Grabbed, this.GetTemplateValue<GameObject>("grabbedStartFX"));
		critter.OngoingStateFX.Add(CrittersPawn.CreatureState.Grabbed, this.GetTemplateValue<GameObject>("grabbedOngoingFX"));
		critter.OnReleasedFX = this.GetTemplateValue<GameObject>("grabbedStopFX");
		critter.stateAnim.Add(CrittersPawn.CreatureState.Grabbed, this.GetTemplateValue<CrittersAnim>("grabbedAnim"));
		critter.StartStateFX.Add(CrittersPawn.CreatureState.SeekingFood, this.GetTemplateValue<GameObject>("hungryStartFX"));
		critter.OngoingStateFX.Add(CrittersPawn.CreatureState.SeekingFood, this.GetTemplateValue<GameObject>("hungryOngoingFX"));
		critter.stateAnim.Add(CrittersPawn.CreatureState.SeekingFood, this.GetTemplateValue<CrittersAnim>("hungryAnim"));
		critter.StartStateFX.Add(CrittersPawn.CreatureState.Despawning, this.GetTemplateValue<GameObject>("despawningStartFX"));
		critter.OngoingStateFX.Add(CrittersPawn.CreatureState.Despawning, this.GetTemplateValue<GameObject>("despawningOngoingFX"));
		critter.stateAnim.Add(CrittersPawn.CreatureState.Despawning, this.GetTemplateValue<CrittersAnim>("despawningAnim"));
		critter.StartStateFX.Add(CrittersPawn.CreatureState.Spawning, this.GetTemplateValue<GameObject>("spawningStartFX"));
		critter.OngoingStateFX.Add(CrittersPawn.CreatureState.Spawning, this.GetTemplateValue<GameObject>("spawningOngoingFX"));
		critter.stateAnim.Add(CrittersPawn.CreatureState.Spawning, this.GetTemplateValue<CrittersAnim>("spawningAnim"));
		critter.StartStateFX.Add(CrittersPawn.CreatureState.Captured, this.GetTemplateValue<GameObject>("capturedStartFX"));
		critter.OngoingStateFX.Add(CrittersPawn.CreatureState.Captured, this.GetTemplateValue<GameObject>("capturedOngoingFX"));
		critter.stateAnim.Add(CrittersPawn.CreatureState.Captured, this.GetTemplateValue<CrittersAnim>("capturedAnim"));
		critter.StartStateFX.Add(CrittersPawn.CreatureState.Stunned, this.GetTemplateValue<GameObject>("stunnedStartFX"));
		critter.OngoingStateFX.Add(CrittersPawn.CreatureState.Stunned, this.GetTemplateValue<GameObject>("stunnedOngoingFX"));
		critter.stateAnim.Add(CrittersPawn.CreatureState.Stunned, this.GetTemplateValue<CrittersAnim>("stunnedAnim"));
		critter.grabbedHaptics = this.GetTemplateValue<AudioClip>("grabbedStruggleHaptics");
		critter.grabbedHapticsStrength = this.GetTemplateValue<float>("grabbedStruggleHapticsStrength");
	}

	// Token: 0x0400031F RID: 799
	public CritterTemplate parent;

	// Token: 0x04000320 RID: 800
	[Space]
	[Header("Description")]
	public string temperament = "UNKNOWN";

	// Token: 0x04000321 RID: 801
	[Space]
	[Header("Behaviour")]
	[CritterTemplateParameter]
	public float maxJumpVel;

	// Token: 0x04000322 RID: 802
	[CritterTemplateParameter]
	public float jumpCooldown;

	// Token: 0x04000323 RID: 803
	[CritterTemplateParameter]
	public float scaredJumpCooldown;

	// Token: 0x04000324 RID: 804
	[CritterTemplateParameter]
	public float jumpVariabilityTime;

	// Token: 0x04000325 RID: 805
	[Space]
	[CritterTemplateParameter]
	public float visionConeAngle;

	// Token: 0x04000326 RID: 806
	[FormerlySerializedAs("visionConeHeight")]
	[CritterTemplateParameter]
	public float sensoryRange;

	// Token: 0x04000327 RID: 807
	[Space]
	[CritterTemplateParameter]
	public float maxHunger;

	// Token: 0x04000328 RID: 808
	[CritterTemplateParameter]
	public float hungryThreshold;

	// Token: 0x04000329 RID: 809
	[CritterTemplateParameter]
	public float satiatedThreshold;

	// Token: 0x0400032A RID: 810
	[CritterTemplateParameter]
	public float hungerLostPerSecond;

	// Token: 0x0400032B RID: 811
	[CritterTemplateParameter]
	public float hungerGainedPerSecond;

	// Token: 0x0400032C RID: 812
	[Space]
	[CritterTemplateParameter]
	public float maxFear;

	// Token: 0x0400032D RID: 813
	[CritterTemplateParameter]
	public float scaredThreshold;

	// Token: 0x0400032E RID: 814
	[CritterTemplateParameter]
	public float calmThreshold;

	// Token: 0x0400032F RID: 815
	[CritterTemplateParameter]
	public float fearLostPerSecond;

	// Token: 0x04000330 RID: 816
	[Space]
	[CritterTemplateParameter]
	public float maxAttraction;

	// Token: 0x04000331 RID: 817
	[CritterTemplateParameter]
	public float attractedThreshold;

	// Token: 0x04000332 RID: 818
	[CritterTemplateParameter]
	public float unattractedThreshold;

	// Token: 0x04000333 RID: 819
	[CritterTemplateParameter]
	public float attractionLostPerSecond;

	// Token: 0x04000334 RID: 820
	[Space]
	[CritterTemplateParameter]
	public float maxSleepiness;

	// Token: 0x04000335 RID: 821
	[CritterTemplateParameter]
	public float tiredThreshold;

	// Token: 0x04000336 RID: 822
	[CritterTemplateParameter]
	public float awakeThreshold;

	// Token: 0x04000337 RID: 823
	[CritterTemplateParameter]
	public float sleepinessGainedPerSecond;

	// Token: 0x04000338 RID: 824
	[CritterTemplateParameter]
	public float sleepinessLostPerSecond;

	// Token: 0x04000339 RID: 825
	[Space]
	[CritterTemplateParameter]
	public float struggleGainedPerSecond;

	// Token: 0x0400033A RID: 826
	[CritterTemplateParameter]
	public float maxStruggle;

	// Token: 0x0400033B RID: 827
	[CritterTemplateParameter]
	public float escapeThreshold;

	// Token: 0x0400033C RID: 828
	[CritterTemplateParameter]
	public float catchableThreshold;

	// Token: 0x0400033D RID: 829
	[CritterTemplateParameter]
	public float struggleLostPerSecond;

	// Token: 0x0400033E RID: 830
	[Space]
	[CritterTemplateParameter]
	public float lifeTime;

	// Token: 0x0400033F RID: 831
	[Space]
	public List<crittersAttractorStruct> attractedToList;

	// Token: 0x04000340 RID: 832
	public List<crittersAttractorStruct> afraidOfList;

	// Token: 0x04000341 RID: 833
	[Space]
	[Header("Visual")]
	[CritterTemplateParameter]
	public float minSize;

	// Token: 0x04000342 RID: 834
	[CritterTemplateParameter]
	public float maxSize;

	// Token: 0x04000343 RID: 835
	[CritterTemplateParameter]
	public float hatChance;

	// Token: 0x04000344 RID: 836
	public GameObject[] hats;

	// Token: 0x04000345 RID: 837
	[Space]
	[Header("Behaviour FX")]
	public GameObject eatingStartFX;

	// Token: 0x04000346 RID: 838
	public GameObject eatingOngoingFX;

	// Token: 0x04000347 RID: 839
	public CrittersAnim eatingAnim;

	// Token: 0x04000348 RID: 840
	public GameObject fearStartFX;

	// Token: 0x04000349 RID: 841
	public GameObject fearOngoingFX;

	// Token: 0x0400034A RID: 842
	public CrittersAnim fearAnim;

	// Token: 0x0400034B RID: 843
	public GameObject attractionStartFX;

	// Token: 0x0400034C RID: 844
	public GameObject attractionOngoingFX;

	// Token: 0x0400034D RID: 845
	public CrittersAnim attractionAnim;

	// Token: 0x0400034E RID: 846
	public GameObject sleepStartFX;

	// Token: 0x0400034F RID: 847
	public GameObject sleepOngoingFX;

	// Token: 0x04000350 RID: 848
	public CrittersAnim sleepAnim;

	// Token: 0x04000351 RID: 849
	public GameObject grabbedStartFX;

	// Token: 0x04000352 RID: 850
	public GameObject grabbedOngoingFX;

	// Token: 0x04000353 RID: 851
	public GameObject grabbedStopFX;

	// Token: 0x04000354 RID: 852
	public CrittersAnim grabbedAnim;

	// Token: 0x04000355 RID: 853
	public GameObject hungryStartFX;

	// Token: 0x04000356 RID: 854
	public GameObject hungryOngoingFX;

	// Token: 0x04000357 RID: 855
	public CrittersAnim hungryAnim;

	// Token: 0x04000358 RID: 856
	public GameObject spawningStartFX;

	// Token: 0x04000359 RID: 857
	public GameObject spawningOngoingFX;

	// Token: 0x0400035A RID: 858
	public CrittersAnim spawningAnim;

	// Token: 0x0400035B RID: 859
	public GameObject despawningStartFX;

	// Token: 0x0400035C RID: 860
	public GameObject despawningOngoingFX;

	// Token: 0x0400035D RID: 861
	public CrittersAnim despawningAnim;

	// Token: 0x0400035E RID: 862
	public GameObject capturedStartFX;

	// Token: 0x0400035F RID: 863
	public GameObject capturedOngoingFX;

	// Token: 0x04000360 RID: 864
	public CrittersAnim capturedAnim;

	// Token: 0x04000361 RID: 865
	public GameObject stunnedStartFX;

	// Token: 0x04000362 RID: 866
	public GameObject stunnedOngoingFX;

	// Token: 0x04000363 RID: 867
	public CrittersAnim stunnedAnim;

	// Token: 0x04000364 RID: 868
	public AudioClip grabbedStruggleHaptics;

	// Token: 0x04000365 RID: 869
	public float grabbedStruggleHapticsStrength;

	// Token: 0x04000366 RID: 870
	private Dictionary<string, object> modifiedValues = new Dictionary<string, object>();
}
