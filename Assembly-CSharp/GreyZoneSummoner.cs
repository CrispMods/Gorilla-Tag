using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020000F7 RID: 247
public class GreyZoneSummoner : MonoBehaviour
{
	// Token: 0x1700009C RID: 156
	// (get) Token: 0x0600068B RID: 1675 RVA: 0x00026230 File Offset: 0x00024430
	public Vector3 SummoningFocusPoint
	{
		get
		{
			return this.summoningFocusPoint.position;
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x0600068C RID: 1676 RVA: 0x0002623D File Offset: 0x0002443D
	public float SummonerMaxDistance
	{
		get
		{
			return this.areaTriggerCollider.radius + 1f;
		}
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x00026250 File Offset: 0x00024450
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

	// Token: 0x0600068E RID: 1678 RVA: 0x000262B4 File Offset: 0x000244B4
	private void OnDisable()
	{
		if (GreyZoneManager.Instance != null)
		{
			GreyZoneManager.Instance.DeregisterSummoner(this);
		}
		this.areaTriggerNotifier.TriggerEnterEvent -= this.ColliderEnteredArea;
		this.areaTriggerNotifier.TriggerExitEvent -= this.ColliderExitedArea;
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x0002630C File Offset: 0x0002450C
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

	// Token: 0x06000690 RID: 1680 RVA: 0x00026455 File Offset: 0x00024655
	public void OnGreyZoneActivated()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.FadeOutSummoningTones());
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x0002646A File Offset: 0x0002466A
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

	// Token: 0x06000692 RID: 1682 RVA: 0x0002647C File Offset: 0x0002467C
	public void ColliderEnteredArea(TriggerEventNotifier notifier, Collider other)
	{
		ZoneEntity component = other.GetComponent<ZoneEntity>();
		VRRig vrrig = (component != null) ? component.entityRig : null;
		if (vrrig != null && this.greyZoneManager != null)
		{
			this.greyZoneManager.VRRigEnteredSummonerProximity(vrrig, this);
		}
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x000264C8 File Offset: 0x000246C8
	public void ColliderExitedArea(TriggerEventNotifier notifier, Collider other)
	{
		ZoneEntity component = other.GetComponent<ZoneEntity>();
		VRRig vrrig = (component != null) ? component.entityRig : null;
		if (vrrig != null && this.greyZoneManager != null)
		{
			this.greyZoneManager.VRRigExitedSummonerProximity(vrrig, this);
		}
	}

	// Token: 0x040007D6 RID: 2006
	[SerializeField]
	private Transform summoningFocusPoint;

	// Token: 0x040007D7 RID: 2007
	[SerializeField]
	private Transform candlesParent;

	// Token: 0x040007D8 RID: 2008
	[SerializeField]
	private PlayableDirector candlesTimeline;

	// Token: 0x040007D9 RID: 2009
	[SerializeField]
	private TriggerEventNotifier areaTriggerNotifier;

	// Token: 0x040007DA RID: 2010
	[SerializeField]
	private SphereCollider areaTriggerCollider;

	// Token: 0x040007DB RID: 2011
	[SerializeField]
	private GorillaPressableButton greyZoneActivationButton;

	// Token: 0x040007DC RID: 2012
	[SerializeField]
	private List<AudioSource> summoningTones = new List<AudioSource>();

	// Token: 0x040007DD RID: 2013
	[SerializeField]
	private float summoningTonesMaxVolume = 1f;

	// Token: 0x040007DE RID: 2014
	[SerializeField]
	private float summoningTonesFadeOverlap = 0.5f;

	// Token: 0x040007DF RID: 2015
	[SerializeField]
	private float summoningTonesFadeTime = 4f;

	// Token: 0x040007E0 RID: 2016
	[SerializeField]
	private List<GorillaPressableButton> greyZoneGravityFactorButtons = new List<GorillaPressableButton>();

	// Token: 0x040007E1 RID: 2017
	private GreyZoneManager greyZoneManager;
}
