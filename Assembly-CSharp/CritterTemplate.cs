using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000063 RID: 99
public class CritterTemplate : ScriptableObject
{
	// Token: 0x17000023 RID: 35
	// (get) Token: 0x06000276 RID: 630 RVA: 0x0000F97C File Offset: 0x0000DB7C
	private string HapticsBlurb
	{
		get
		{
			float num = this.grabbedStruggleHaptics.GetPeakMagnitude() * this.grabbedStruggleHapticsStrength;
			float num2 = this.grabbedStruggleHaptics.GetRMSMagnitude() * this.grabbedStruggleHapticsStrength;
			return string.Format("Peak Strength: {0:0.##} Mean Strength: {1:0.##}", num, num2);
		}
	}

	// Token: 0x06000277 RID: 631 RVA: 0x0000F9C8 File Offset: 0x0000DBC8
	private void SetMaxStrength(float maxStrength = 1f)
	{
		float peakMagnitude = this.grabbedStruggleHaptics.GetPeakMagnitude();
		Debug.Log(string.Format("Clip {0} max strength: {1}", this.grabbedStruggleHaptics, peakMagnitude));
		if (peakMagnitude > 0f)
		{
			this.grabbedStruggleHapticsStrength = maxStrength / peakMagnitude;
		}
	}

	// Token: 0x06000278 RID: 632 RVA: 0x0000FA10 File Offset: 0x0000DC10
	private void SetMeanStrength(float meanStrength = 1f)
	{
		float rmsmagnitude = this.grabbedStruggleHaptics.GetRMSMagnitude();
		Debug.Log(string.Format("Clip {0} mean strength: {1}", this.grabbedStruggleHaptics, rmsmagnitude));
		if (meanStrength > 0f)
		{
			this.grabbedStruggleHapticsStrength = meanStrength / rmsmagnitude;
		}
	}

	// Token: 0x06000279 RID: 633 RVA: 0x0000FA55 File Offset: 0x0000DC55
	private void OnValidate()
	{
		this.modifiedValues.Clear();
		this.RegisterModifiedBehaviour();
		this.RegisterModifiedVisual();
	}

	// Token: 0x0600027A RID: 634 RVA: 0x0000FA6E File Offset: 0x0000DC6E
	private void OnEnable()
	{
		this.OnValidate();
	}

	// Token: 0x0600027B RID: 635 RVA: 0x0000FA78 File Offset: 0x0000DC78
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

	// Token: 0x0600027C RID: 636 RVA: 0x0000FF74 File Offset: 0x0000E174
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

	// Token: 0x0600027D RID: 637 RVA: 0x000104BE File Offset: 0x0000E6BE
	public bool IsValueModified(string valueName)
	{
		return this.modifiedValues.ContainsKey(valueName);
	}

	// Token: 0x0600027E RID: 638 RVA: 0x000104CC File Offset: 0x0000E6CC
	public T GetParentValue<T>(string valueName)
	{
		if (this.parent != null)
		{
			return this.parent.GetTemplateValue<T>(valueName);
		}
		return default(T);
	}

	// Token: 0x0600027F RID: 639 RVA: 0x00010500 File Offset: 0x0000E700
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

	// Token: 0x06000280 RID: 640 RVA: 0x00010548 File Offset: 0x0000E748
	public void ApplyToCritter(CrittersPawn critter)
	{
		this.ApplyBehaviour(critter);
		this.ApplyBehaviourFX(critter);
	}

	// Token: 0x06000281 RID: 641 RVA: 0x00010558 File Offset: 0x0000E758
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

	// Token: 0x06000282 RID: 642 RVA: 0x00010788 File Offset: 0x0000E988
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

	// Token: 0x040002ED RID: 749
	public CritterTemplate parent;

	// Token: 0x040002EE RID: 750
	[Space]
	[Header("Description")]
	public string temperament = "UNKNOWN";

	// Token: 0x040002EF RID: 751
	[Space]
	[Header("Behaviour")]
	[CritterTemplateParameter]
	public float maxJumpVel;

	// Token: 0x040002F0 RID: 752
	[CritterTemplateParameter]
	public float jumpCooldown;

	// Token: 0x040002F1 RID: 753
	[CritterTemplateParameter]
	public float scaredJumpCooldown;

	// Token: 0x040002F2 RID: 754
	[CritterTemplateParameter]
	public float jumpVariabilityTime;

	// Token: 0x040002F3 RID: 755
	[Space]
	[CritterTemplateParameter]
	public float visionConeAngle;

	// Token: 0x040002F4 RID: 756
	[FormerlySerializedAs("visionConeHeight")]
	[CritterTemplateParameter]
	public float sensoryRange;

	// Token: 0x040002F5 RID: 757
	[Space]
	[CritterTemplateParameter]
	public float maxHunger;

	// Token: 0x040002F6 RID: 758
	[CritterTemplateParameter]
	public float hungryThreshold;

	// Token: 0x040002F7 RID: 759
	[CritterTemplateParameter]
	public float satiatedThreshold;

	// Token: 0x040002F8 RID: 760
	[CritterTemplateParameter]
	public float hungerLostPerSecond;

	// Token: 0x040002F9 RID: 761
	[CritterTemplateParameter]
	public float hungerGainedPerSecond;

	// Token: 0x040002FA RID: 762
	[Space]
	[CritterTemplateParameter]
	public float maxFear;

	// Token: 0x040002FB RID: 763
	[CritterTemplateParameter]
	public float scaredThreshold;

	// Token: 0x040002FC RID: 764
	[CritterTemplateParameter]
	public float calmThreshold;

	// Token: 0x040002FD RID: 765
	[CritterTemplateParameter]
	public float fearLostPerSecond;

	// Token: 0x040002FE RID: 766
	[Space]
	[CritterTemplateParameter]
	public float maxAttraction;

	// Token: 0x040002FF RID: 767
	[CritterTemplateParameter]
	public float attractedThreshold;

	// Token: 0x04000300 RID: 768
	[CritterTemplateParameter]
	public float unattractedThreshold;

	// Token: 0x04000301 RID: 769
	[CritterTemplateParameter]
	public float attractionLostPerSecond;

	// Token: 0x04000302 RID: 770
	[Space]
	[CritterTemplateParameter]
	public float maxSleepiness;

	// Token: 0x04000303 RID: 771
	[CritterTemplateParameter]
	public float tiredThreshold;

	// Token: 0x04000304 RID: 772
	[CritterTemplateParameter]
	public float awakeThreshold;

	// Token: 0x04000305 RID: 773
	[CritterTemplateParameter]
	public float sleepinessGainedPerSecond;

	// Token: 0x04000306 RID: 774
	[CritterTemplateParameter]
	public float sleepinessLostPerSecond;

	// Token: 0x04000307 RID: 775
	[Space]
	[CritterTemplateParameter]
	public float struggleGainedPerSecond;

	// Token: 0x04000308 RID: 776
	[CritterTemplateParameter]
	public float maxStruggle;

	// Token: 0x04000309 RID: 777
	[CritterTemplateParameter]
	public float escapeThreshold;

	// Token: 0x0400030A RID: 778
	[CritterTemplateParameter]
	public float catchableThreshold;

	// Token: 0x0400030B RID: 779
	[CritterTemplateParameter]
	public float struggleLostPerSecond;

	// Token: 0x0400030C RID: 780
	[Space]
	[CritterTemplateParameter]
	public float lifeTime;

	// Token: 0x0400030D RID: 781
	[Space]
	public List<crittersAttractorStruct> attractedToList;

	// Token: 0x0400030E RID: 782
	public List<crittersAttractorStruct> afraidOfList;

	// Token: 0x0400030F RID: 783
	[Space]
	[Header("Visual")]
	[CritterTemplateParameter]
	public float minSize;

	// Token: 0x04000310 RID: 784
	[CritterTemplateParameter]
	public float maxSize;

	// Token: 0x04000311 RID: 785
	[CritterTemplateParameter]
	public float hatChance;

	// Token: 0x04000312 RID: 786
	public GameObject[] hats;

	// Token: 0x04000313 RID: 787
	[Space]
	[Header("Behaviour FX")]
	public GameObject eatingStartFX;

	// Token: 0x04000314 RID: 788
	public GameObject eatingOngoingFX;

	// Token: 0x04000315 RID: 789
	public CrittersAnim eatingAnim;

	// Token: 0x04000316 RID: 790
	public GameObject fearStartFX;

	// Token: 0x04000317 RID: 791
	public GameObject fearOngoingFX;

	// Token: 0x04000318 RID: 792
	public CrittersAnim fearAnim;

	// Token: 0x04000319 RID: 793
	public GameObject attractionStartFX;

	// Token: 0x0400031A RID: 794
	public GameObject attractionOngoingFX;

	// Token: 0x0400031B RID: 795
	public CrittersAnim attractionAnim;

	// Token: 0x0400031C RID: 796
	public GameObject sleepStartFX;

	// Token: 0x0400031D RID: 797
	public GameObject sleepOngoingFX;

	// Token: 0x0400031E RID: 798
	public CrittersAnim sleepAnim;

	// Token: 0x0400031F RID: 799
	public GameObject grabbedStartFX;

	// Token: 0x04000320 RID: 800
	public GameObject grabbedOngoingFX;

	// Token: 0x04000321 RID: 801
	public GameObject grabbedStopFX;

	// Token: 0x04000322 RID: 802
	public CrittersAnim grabbedAnim;

	// Token: 0x04000323 RID: 803
	public GameObject hungryStartFX;

	// Token: 0x04000324 RID: 804
	public GameObject hungryOngoingFX;

	// Token: 0x04000325 RID: 805
	public CrittersAnim hungryAnim;

	// Token: 0x04000326 RID: 806
	public GameObject spawningStartFX;

	// Token: 0x04000327 RID: 807
	public GameObject spawningOngoingFX;

	// Token: 0x04000328 RID: 808
	public CrittersAnim spawningAnim;

	// Token: 0x04000329 RID: 809
	public GameObject despawningStartFX;

	// Token: 0x0400032A RID: 810
	public GameObject despawningOngoingFX;

	// Token: 0x0400032B RID: 811
	public CrittersAnim despawningAnim;

	// Token: 0x0400032C RID: 812
	public GameObject capturedStartFX;

	// Token: 0x0400032D RID: 813
	public GameObject capturedOngoingFX;

	// Token: 0x0400032E RID: 814
	public CrittersAnim capturedAnim;

	// Token: 0x0400032F RID: 815
	public GameObject stunnedStartFX;

	// Token: 0x04000330 RID: 816
	public GameObject stunnedOngoingFX;

	// Token: 0x04000331 RID: 817
	public CrittersAnim stunnedAnim;

	// Token: 0x04000332 RID: 818
	public AudioClip grabbedStruggleHaptics;

	// Token: 0x04000333 RID: 819
	public float grabbedStruggleHapticsStrength;

	// Token: 0x04000334 RID: 820
	private Dictionary<string, object> modifiedValues = new Dictionary<string, object>();
}
