using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000101 RID: 257
public class GreyZoneSummoner : MonoBehaviour
{
	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x060006CC RID: 1740 RVA: 0x00034F28 File Offset: 0x00033128
	public Vector3 SummoningFocusPoint
	{
		get
		{
			return this.summoningFocusPoint.position;
		}
	}

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x060006CD RID: 1741 RVA: 0x00034F35 File Offset: 0x00033135
	public float SummonerMaxDistance
	{
		get
		{
			return this.areaTriggerCollider.radius + 1f;
		}
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x000884A0 File Offset: 0x000866A0
	private void OnEnable()
	{
		this.greyZoneManager = GreyZoneManager.Instance;
		if (this.greyZoneManager == null)
		{
			return;
		}
		this.greyZoneManager.RegisterSummoner(this);
		this.areaTriggerNotifier.TriggerEnterEvent += this.ColliderEnteredArea;
		this.areaTriggerNotifier.TriggerExitEvent += this.ColliderExitedArea;
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x00088504 File Offset: 0x00086704
	private void OnDisable()
	{
		if (GreyZoneManager.Instance != null)
		{
			GreyZoneManager.Instance.DeregisterSummoner(this);
		}
		this.areaTriggerNotifier.TriggerEnterEvent -= this.ColliderEnteredArea;
		this.areaTriggerNotifier.TriggerExitEvent -= this.ColliderExitedArea;
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x0008855C File Offset: 0x0008675C
	public void UpdateProgressFeedback(bool greyZoneAvailable)
	{
		if (this.greyZoneManager == null)
		{
			return;
		}
		if (greyZoneAvailable && !this.candlesParent.gameObject.activeSelf)
		{
			this.candlesParent.gameObject.SetActive(true);
		}
		this.candlesTimeline.time = (double)Mathf.Clamp01(this.greyZoneManager.SummoningProgress) * this.candlesTimeline.duration;
		this.candlesTimeline.Evaluate();
		if (!this.greyZoneManager.GreyZoneActive)
		{
			float value = (float)this.summoningTones.Count * this.greyZoneManager.SummoningProgress;
			for (int i = 0; i < this.summoningTones.Count; i++)
			{
				float num = Mathf.InverseLerp((float)i, (float)i + 1f + this.summoningTonesFadeOverlap, value);
				this.summoningTones[i].volume = num * this.summoningTonesMaxVolume;
			}
		}
		this.greyZoneActivationButton.isOn = this.greyZoneManager.GreyZoneActive;
		this.greyZoneActivationButton.UpdateColor();
		for (int j = 0; j < this.greyZoneGravityFactorButtons.Count; j++)
		{
			this.greyZoneGravityFactorButtons[j].isOn = (this.greyZoneManager.GravityFactorSelection == j);
			this.greyZoneGravityFactorButtons[j].UpdateColor();
		}
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x00034F48 File Offset: 0x00033148
	public void OnGreyZoneActivated()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.FadeOutSummoningTones());
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x00034F5D File Offset: 0x0003315D
	private IEnumerator FadeOutSummoningTones()
	{
		float fadeStartTime = Time.time;
		float fadeRate = 1f / this.summoningTonesFadeTime;
		while (Time.time < fadeStartTime + this.summoningTonesFadeTime)
		{
			for (int i = 0; i < this.summoningTones.Count; i++)
			{
				this.summoningTones[i].volume = Mathf.MoveTowards(this.summoningTones[i].volume, 0f, this.summoningTonesMaxVolume * fadeRate * Time.deltaTime);
			}
			yield return null;
		}
		for (int j = 0; j < this.summoningTones.Count; j++)
		{
			this.summoningTones[j].volume = 0f;
		}
		yield break;
	}

	// Token: 0x060006D3 RID: 1747 RVA: 0x000886A8 File Offset: 0x000868A8
	public void ColliderEnteredArea(TriggerEventNotifier notifier, Collider other)
	{
		ZoneEntity component = other.GetComponent<ZoneEntity>();
		VRRig vrrig = (component != null) ? component.entityRig : null;
		if (vrrig != null && this.greyZoneManager != null)
		{
			this.greyZoneManager.VRRigEnteredSummonerProximity(vrrig, this);
		}
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x000886F4 File Offset: 0x000868F4
	public void ColliderExitedArea(TriggerEventNotifier notifier, Collider other)
	{
		ZoneEntity component = other.GetComponent<ZoneEntity>();
		VRRig vrrig = (component != null) ? component.entityRig : null;
		if (vrrig != null && this.greyZoneManager != null)
		{
			this.greyZoneManager.VRRigExitedSummonerProximity(vrrig, this);
		}
	}

	// Token: 0x04000817 RID: 2071
	[SerializeField]
	private Transform summoningFocusPoint;

	// Token: 0x04000818 RID: 2072
	[SerializeField]
	private Transform candlesParent;

	// Token: 0x04000819 RID: 2073
	[SerializeField]
	private PlayableDirector candlesTimeline;

	// Token: 0x0400081A RID: 2074
	[SerializeField]
	private TriggerEventNotifier areaTriggerNotifier;

	// Token: 0x0400081B RID: 2075
	[SerializeField]
	private SphereCollider areaTriggerCollider;

	// Token: 0x0400081C RID: 2076
	[SerializeField]
	private GorillaPressableButton greyZoneActivationButton;

	// Token: 0x0400081D RID: 2077
	[SerializeField]
	private List<AudioSource> summoningTones = new List<AudioSource>();

	// Token: 0x0400081E RID: 2078
	[SerializeField]
	private float summoningTonesMaxVolume = 1f;

	// Token: 0x0400081F RID: 2079
	[SerializeField]
	private float summoningTonesFadeOverlap = 0.5f;

	// Token: 0x04000820 RID: 2080
	[SerializeField]
	private float summoningTonesFadeTime = 4f;

	// Token: 0x04000821 RID: 2081
	[SerializeField]
	private List<GorillaPressableButton> greyZoneGravityFactorButtons = new List<GorillaPressableButton>();

	// Token: 0x04000822 RID: 2082
	private GreyZoneManager greyZoneManager;
}
