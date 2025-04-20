using System;
using System.Collections.Generic;
using CjLib;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020000FA RID: 250
public class MoonController : MonoBehaviour
{
	// Token: 0x17000095 RID: 149
	// (get) Token: 0x0600068B RID: 1675 RVA: 0x00034CDE File Offset: 0x00032EDE
	public float Distance
	{
		get
		{
			return this.distance;
		}
	}

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x0600068C RID: 1676 RVA: 0x00034CE6 File Offset: 0x00032EE6
	private float TimeOfDay
	{
		get
		{
			if (this.debugOverrideTimeOfDay)
			{
				return Mathf.Repeat(this.timeOfDayOverride, 1f);
			}
			if (!(BetterDayNightManager.instance != null))
			{
				return 1f;
			}
			return BetterDayNightManager.instance.NormalizedTimeOfDay;
		}
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x00034D22 File Offset: 0x00032F22
	public void SetEyeOpenAnimation()
	{
		this.openMoonAnimator.SetBool(this.eyeOpenHash, true);
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x00034D36 File Offset: 0x00032F36
	public void StartEyeCloseAnimation()
	{
		this.openMoonAnimator.SetBool(this.eyeOpenHash, false);
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x00086C8C File Offset: 0x00084E8C
	private void Start()
	{
		this.eyeOpenHash = Animator.StringToHash("EyeOpen");
		this.zoneToSceneMapping.Add(GTZone.forest, MoonController.Scenes.Forest);
		this.zoneToSceneMapping.Add(GTZone.city, MoonController.Scenes.City);
		this.zoneToSceneMapping.Add(GTZone.basement, MoonController.Scenes.City);
		this.zoneToSceneMapping.Add(GTZone.canyon, MoonController.Scenes.Canyon);
		this.zoneToSceneMapping.Add(GTZone.beach, MoonController.Scenes.Beach);
		this.zoneToSceneMapping.Add(GTZone.mountain, MoonController.Scenes.Mountain);
		this.zoneToSceneMapping.Add(GTZone.skyJungle, MoonController.Scenes.Clouds);
		this.zoneToSceneMapping.Add(GTZone.cave, MoonController.Scenes.Forest);
		this.zoneToSceneMapping.Add(GTZone.cityWithSkyJungle, MoonController.Scenes.City);
		this.zoneToSceneMapping.Add(GTZone.tutorial, MoonController.Scenes.Forest);
		this.zoneToSceneMapping.Add(GTZone.rotating, MoonController.Scenes.Forest);
		this.zoneToSceneMapping.Add(GTZone.none, MoonController.Scenes.Forest);
		this.zoneToSceneMapping.Add(GTZone.Metropolis, MoonController.Scenes.Metropolis);
		this.zoneToSceneMapping.Add(GTZone.cityNoBuildings, MoonController.Scenes.City);
		this.zoneToSceneMapping.Add(GTZone.attic, MoonController.Scenes.Forest);
		this.zoneToSceneMapping.Add(GTZone.arcade, MoonController.Scenes.City);
		this.zoneToSceneMapping.Add(GTZone.bayou, MoonController.Scenes.Bayou);
		if (ZoneManagement.instance != null)
		{
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
		}
		if (GreyZoneManager.Instance != null)
		{
			GreyZoneManager.Instance.RegisterMoon(this);
		}
		this.crackStartDayOfYear = new DateTime(2024, 10, 4).DayOfYear;
		this.crackEndDayOfYear = new DateTime(2024, 10, 25).DayOfYear;
		if (this.crackRenderer != null)
		{
			this.currentlySetCrackProgress = 1f;
			this.crackMaterialPropertyBlock = new MaterialPropertyBlock();
			this.crackRenderer.GetPropertyBlock(this.crackMaterialPropertyBlock);
			this.crackMaterialPropertyBlock.SetFloat(this.crackProgressHash, this.currentlySetCrackProgress);
			this.crackRenderer.SetPropertyBlock(this.crackMaterialPropertyBlock);
		}
		this.orbitAngle = 0f;
		this.UpdateCrack();
		this.UpdatePlacement();
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x00034D4A File Offset: 0x00032F4A
	private void OnDestroy()
	{
		if (GreyZoneManager.Instance != null)
		{
			GreyZoneManager.Instance.UnregisterMoon(this);
		}
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x00086E88 File Offset: 0x00085088
	private void OnZoneChanged()
	{
		ZoneManagement instance = ZoneManagement.instance;
		MoonController.Scenes scenes = MoonController.Scenes.Forest;
		for (int i = 0; i < instance.activeZones.Count; i++)
		{
			MoonController.Scenes scenes2;
			if (this.zoneToSceneMapping.TryGetValue(instance.activeZones[i], out scenes2) && scenes2 > scenes)
			{
				scenes = scenes2;
			}
		}
		this.UpdateActiveScene(scenes);
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x00034D68 File Offset: 0x00032F68
	private void UpdateActiveScene(MoonController.Scenes nextScene)
	{
		this.activeScene = nextScene;
		this.UpdateCrack();
		this.UpdatePlacement();
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x00086EDC File Offset: 0x000850DC
	private void Update()
	{
		this.UpdateCrack();
		if (!this.alwaysInTheSky)
		{
			float timeOfDay = this.TimeOfDay;
			bool flag = timeOfDay > 0.53999996f && timeOfDay < 0.6733333f;
			bool flag2 = timeOfDay > 0.086666666f && timeOfDay < 0.22f;
			bool flag3 = timeOfDay <= 0.086666666f || timeOfDay >= 0.6733333f;
			if (timeOfDay >= 0.22f)
			{
				bool flag4 = timeOfDay <= 0.53999996f;
			}
			float num = this.orbitAngle;
			if (flag)
			{
				num = Mathf.Lerp(3.1415927f, 0f, (timeOfDay - 0.53999996f) / 0.13333333f);
			}
			else if (flag2)
			{
				num = Mathf.Lerp(0f, -3.1415927f, (timeOfDay - 0.086666666f) / 0.13333333f);
			}
			else if (flag3)
			{
				num = 0f;
			}
			else
			{
				num = 3.1415927f;
			}
			if (this.orbitAngle != num)
			{
				this.orbitAngle = num;
				this.UpdateCrack();
				this.UpdatePlacement();
			}
		}
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x00034D7D File Offset: 0x00032F7D
	public void UpdateDistance(float nextDistance)
	{
		this.distance = nextDistance;
		this.UpdateVisualState();
		this.UpdatePlacement();
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x00086FD0 File Offset: 0x000851D0
	public void UpdateVisualState()
	{
		bool flag = false;
		if (GreyZoneManager.Instance != null)
		{
			flag = GreyZoneManager.Instance.GreyZoneActive;
		}
		if (flag && this.openEyeModelEnabled && this.distance < this.eyeOpenDistThreshold && !this.openMoonAnimator.GetBool(this.eyeOpenHash))
		{
			this.openMoonAnimator.SetBool(this.eyeOpenHash, true);
			return;
		}
		if (!flag && this.distance > this.eyeCloseDistThreshold && this.openMoonAnimator.GetBool(this.eyeOpenHash))
		{
			this.openMoonAnimator.SetBool(this.eyeOpenHash, false);
		}
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x00034D92 File Offset: 0x00032F92
	public void UpdatePlacement()
	{
		if (this.alwaysInTheSky)
		{
			this.UpdatePlacementSimple();
			return;
		}
		this.UpdatePlacementOrbit();
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x00087070 File Offset: 0x00085270
	private void UpdatePlacementSimple()
	{
		MoonController.SceneData sceneData = this.scenes[(int)this.activeScene];
		Transform referencePoint = sceneData.referencePoint;
		MoonController.Placement placement = sceneData.overridePlacement ? sceneData.PlacementOverride : this.defaultPlacement;
		float num = Mathf.Lerp(placement.heightRange.x, placement.heightRange.y, this.distance);
		float num2 = Mathf.Lerp(placement.radiusRange.x, placement.radiusRange.y, this.distance);
		float d = Mathf.Lerp(placement.scaleRange.x, placement.scaleRange.y, this.distance);
		float restAngle = placement.restAngle;
		Vector3 position = referencePoint.position;
		position.y += num;
		position.x += num2 * Mathf.Cos(restAngle * 0.017453292f);
		position.z += num2 * Mathf.Sin(restAngle * 0.017453292f);
		base.transform.position = position;
		base.transform.rotation = Quaternion.LookRotation(referencePoint.position - base.transform.position);
		base.transform.localScale = Vector3.one * d;
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x000871B4 File Offset: 0x000853B4
	public void UpdatePlacementOrbit()
	{
		MoonController.SceneData sceneData = this.scenes[(int)this.activeScene];
		Transform referencePoint = sceneData.referencePoint;
		MoonController.Placement placement = sceneData.overridePlacement ? sceneData.PlacementOverride : this.defaultPlacement;
		float y = placement.heightRange.y;
		float y2 = placement.radiusRange.y;
		Vector3 position = referencePoint.position;
		position.y += y;
		position.x += y2 * Mathf.Cos(placement.restAngle * 0.017453292f);
		position.z += y2 * Mathf.Sin(placement.restAngle * 0.017453292f);
		float d = Mathf.Sqrt(y * y + y2 * y2);
		float num = Mathf.Atan2(y, y2);
		Quaternion rotation = Quaternion.AngleAxis(57.29578f * num, Vector3.Cross(position - referencePoint.position, Vector3.up));
		float f = placement.restAngle * 0.017453292f + this.orbitAngle;
		Vector3 vector = referencePoint.position + rotation * new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f)) * d;
		if (this.distance < 1f)
		{
			Vector3 position2 = referencePoint.position;
			position2.y += placement.heightRange.x;
			position2.x += placement.radiusRange.x * Mathf.Cos(placement.restAngle * 0.017453292f);
			position2.z += placement.radiusRange.x * Mathf.Sin(placement.restAngle * 0.017453292f);
			if (Mathf.Abs(this.orbitAngle) < 0.9424779f)
			{
				vector = Vector3.Lerp(position2, vector, this.distance);
			}
			else
			{
				vector = Vector3.Lerp(position2, position, this.distance);
			}
		}
		base.transform.position = vector;
		base.transform.rotation = Quaternion.LookRotation(referencePoint.position - base.transform.position);
		base.transform.localScale = Vector3.one * Mathf.Lerp(placement.scaleRange.x, placement.scaleRange.y, this.distance);
		if (this.debugDrawOrbit)
		{
			int num2 = 32;
			float timeOfDay = this.TimeOfDay;
			float num3 = 0.086666666f;
			float num4 = 0.24666667f;
			float num5 = 0.6333333f;
			float num6 = 0.76f;
			bool flag = timeOfDay > num3 && timeOfDay < num4;
			bool flag2 = timeOfDay > num5 && timeOfDay < num6;
			bool flag3 = timeOfDay <= num3 || timeOfDay >= num6;
			if (timeOfDay >= num4)
			{
				bool flag4 = timeOfDay <= num5;
			}
			Color color = flag2 ? Color.red : (flag3 ? Color.green : (flag ? Color.yellow : Color.blue));
			Vector3 v = referencePoint.position + rotation * new Vector3(Mathf.Cos(0f), 0f, Mathf.Sin(0f)) * d;
			for (int i = 1; i <= num2; i++)
			{
				float num7 = (float)i / (float)num2;
				Vector3 vector2 = referencePoint.position + rotation * new Vector3(Mathf.Cos(6.2831855f * num7), 0f, Mathf.Sin(6.2831855f * num7)) * d;
				DebugUtil.DrawLine(v, vector2, color, false);
				v = vector2;
			}
		}
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x00087544 File Offset: 0x00085744
	private void UpdateCrack()
	{
		bool flag = GreyZoneManager.Instance != null && GreyZoneManager.Instance.GreyZoneAvailable;
		if (flag && !this.openEyeModelEnabled)
		{
			this.openEyeModelEnabled = true;
			this.defaultMoon.gameObject.SetActive(false);
			this.openMoon.gameObject.SetActive(true);
		}
		else if (!flag && this.openEyeModelEnabled)
		{
			this.openEyeModelEnabled = false;
			this.defaultMoon.gameObject.SetActive(true);
			this.openMoon.gameObject.SetActive(false);
		}
		if (!flag && GorillaComputer.instance != null)
		{
			DateTime serverTime = GorillaComputer.instance.GetServerTime();
			if (this.debugOverrideCrackDayInOctober)
			{
				serverTime = new DateTime(2024, 10, Mathf.Clamp(this.crackDayInOctoberOverride, 1, 31));
			}
			float value = Mathf.InverseLerp((float)this.crackStartDayOfYear, (float)this.crackEndDayOfYear, (float)serverTime.DayOfYear);
			if (this.debugOverrideCrackProgress)
			{
				value = this.crackProgress;
			}
			float num = 1f - Mathf.Clamp01(value);
			if (this.crackRenderer != null && Mathf.Abs(num - this.currentlySetCrackProgress) > Mathf.Epsilon)
			{
				this.currentlySetCrackProgress = num;
				this.crackMaterialPropertyBlock.SetFloat(this.crackProgressHash, this.currentlySetCrackProgress);
				this.crackRenderer.SetPropertyBlock(this.crackMaterialPropertyBlock);
			}
		}
	}

	// Token: 0x040007B8 RID: 1976
	[SerializeField]
	private List<MoonController.SceneData> scenes = new List<MoonController.SceneData>();

	// Token: 0x040007B9 RID: 1977
	[SerializeField]
	private MoonController.Scenes activeScene;

	// Token: 0x040007BA RID: 1978
	[SerializeField]
	private MoonController.Placement defaultPlacement;

	// Token: 0x040007BB RID: 1979
	[SerializeField]
	[Range(0f, 1f)]
	private float distance;

	// Token: 0x040007BC RID: 1980
	[SerializeField]
	private bool alwaysInTheSky;

	// Token: 0x040007BD RID: 1981
	[Header("Model Swap")]
	[SerializeField]
	private Transform defaultMoon;

	// Token: 0x040007BE RID: 1982
	[SerializeField]
	private Transform openMoon;

	// Token: 0x040007BF RID: 1983
	[Header("Animation")]
	[SerializeField]
	private Animator openMoonAnimator;

	// Token: 0x040007C0 RID: 1984
	[SerializeField]
	private float eyeOpenDistThreshold = 0.9f;

	// Token: 0x040007C1 RID: 1985
	[SerializeField]
	private float eyeCloseDistThreshold = 0.05f;

	// Token: 0x040007C2 RID: 1986
	[Header("Debug")]
	[SerializeField]
	private bool debugOverrideTimeOfDay;

	// Token: 0x040007C3 RID: 1987
	[SerializeField]
	[Range(0f, 4f)]
	private float timeOfDayOverride;

	// Token: 0x040007C4 RID: 1988
	[SerializeField]
	private bool debugOverrideCrackProgress;

	// Token: 0x040007C5 RID: 1989
	[SerializeField]
	[Range(0f, 1f)]
	private float crackProgress;

	// Token: 0x040007C6 RID: 1990
	[SerializeField]
	private bool debugOverrideCrackDayInOctober;

	// Token: 0x040007C7 RID: 1991
	[SerializeField]
	[Range(1f, 31f)]
	private int crackDayInOctoberOverride = 4;

	// Token: 0x040007C8 RID: 1992
	[SerializeField]
	private MeshRenderer crackRenderer;

	// Token: 0x040007C9 RID: 1993
	private int crackStartDayOfYear;

	// Token: 0x040007CA RID: 1994
	private int crackEndDayOfYear;

	// Token: 0x040007CB RID: 1995
	private float orbitAngle;

	// Token: 0x040007CC RID: 1996
	private int crackProgressHash = Shader.PropertyToID("_Progress");

	// Token: 0x040007CD RID: 1997
	private int eyeOpenHash;

	// Token: 0x040007CE RID: 1998
	private bool openEyeModelEnabled;

	// Token: 0x040007CF RID: 1999
	private float currentlySetCrackProgress;

	// Token: 0x040007D0 RID: 2000
	private MaterialPropertyBlock crackMaterialPropertyBlock;

	// Token: 0x040007D1 RID: 2001
	private bool debugDrawOrbit;

	// Token: 0x040007D2 RID: 2002
	private Dictionary<GTZone, MoonController.Scenes> zoneToSceneMapping = new Dictionary<GTZone, MoonController.Scenes>();

	// Token: 0x040007D3 RID: 2003
	private const float moonFallStart = 0.086666666f;

	// Token: 0x040007D4 RID: 2004
	private const float moonFallEnd = 0.22f;

	// Token: 0x040007D5 RID: 2005
	private const float moonRiseStart = 0.53999996f;

	// Token: 0x040007D6 RID: 2006
	private const float moonRiseEnd = 0.6733333f;

	// Token: 0x020000FB RID: 251
	public enum Scenes
	{
		// Token: 0x040007D8 RID: 2008
		Forest,
		// Token: 0x040007D9 RID: 2009
		Bayou,
		// Token: 0x040007DA RID: 2010
		Beach,
		// Token: 0x040007DB RID: 2011
		Canyon,
		// Token: 0x040007DC RID: 2012
		Clouds,
		// Token: 0x040007DD RID: 2013
		City,
		// Token: 0x040007DE RID: 2014
		Metropolis,
		// Token: 0x040007DF RID: 2015
		Mountain
	}

	// Token: 0x020000FC RID: 252
	[Serializable]
	public struct SceneData
	{
		// Token: 0x040007E0 RID: 2016
		public MoonController.Scenes scene;

		// Token: 0x040007E1 RID: 2017
		public Transform referencePoint;

		// Token: 0x040007E2 RID: 2018
		public bool overridePlacement;

		// Token: 0x040007E3 RID: 2019
		public MoonController.Placement PlacementOverride;
	}

	// Token: 0x020000FD RID: 253
	[Serializable]
	public struct Placement
	{
		// Token: 0x040007E4 RID: 2020
		public Vector2 radiusRange;

		// Token: 0x040007E5 RID: 2021
		public Vector2 heightRange;

		// Token: 0x040007E6 RID: 2022
		public Vector2 scaleRange;

		// Token: 0x040007E7 RID: 2023
		public float restAngle;
	}
}
