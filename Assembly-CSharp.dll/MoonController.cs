using System;
using System.Collections.Generic;
using CjLib;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public class MoonController : MonoBehaviour
{
	// Token: 0x17000090 RID: 144
	// (get) Token: 0x0600064C RID: 1612 RVA: 0x00033A7A File Offset: 0x00031C7A
	public float Distance
	{
		get
		{
			return this.distance;
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x0600064D RID: 1613 RVA: 0x00033A82 File Offset: 0x00031C82
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

	// Token: 0x0600064E RID: 1614 RVA: 0x00033ABE File Offset: 0x00031CBE
	public void SetEyeOpenAnimation()
	{
		this.openMoonAnimator.SetBool(this.eyeOpenHash, true);
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x00033AD2 File Offset: 0x00031CD2
	public void StartEyeCloseAnimation()
	{
		this.openMoonAnimator.SetBool(this.eyeOpenHash, false);
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x00084384 File Offset: 0x00082584
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

	// Token: 0x06000651 RID: 1617 RVA: 0x00033AE6 File Offset: 0x00031CE6
	private void OnDestroy()
	{
		if (GreyZoneManager.Instance != null)
		{
			GreyZoneManager.Instance.UnregisterMoon(this);
		}
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x00084580 File Offset: 0x00082780
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

	// Token: 0x06000653 RID: 1619 RVA: 0x00033B04 File Offset: 0x00031D04
	private void UpdateActiveScene(MoonController.Scenes nextScene)
	{
		this.activeScene = nextScene;
		this.UpdateCrack();
		this.UpdatePlacement();
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x000845D4 File Offset: 0x000827D4
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

	// Token: 0x06000655 RID: 1621 RVA: 0x00033B19 File Offset: 0x00031D19
	public void UpdateDistance(float nextDistance)
	{
		this.distance = nextDistance;
		this.UpdateVisualState();
		this.UpdatePlacement();
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x000846C8 File Offset: 0x000828C8
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

	// Token: 0x06000657 RID: 1623 RVA: 0x00033B2E File Offset: 0x00031D2E
	public void UpdatePlacement()
	{
		if (this.alwaysInTheSky)
		{
			this.UpdatePlacementSimple();
			return;
		}
		this.UpdatePlacementOrbit();
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x00084768 File Offset: 0x00082968
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

	// Token: 0x06000659 RID: 1625 RVA: 0x000848AC File Offset: 0x00082AAC
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

	// Token: 0x0600065A RID: 1626 RVA: 0x00084C3C File Offset: 0x00082E3C
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

	// Token: 0x04000778 RID: 1912
	[SerializeField]
	private List<MoonController.SceneData> scenes = new List<MoonController.SceneData>();

	// Token: 0x04000779 RID: 1913
	[SerializeField]
	private MoonController.Scenes activeScene;

	// Token: 0x0400077A RID: 1914
	[SerializeField]
	private MoonController.Placement defaultPlacement;

	// Token: 0x0400077B RID: 1915
	[SerializeField]
	[Range(0f, 1f)]
	private float distance;

	// Token: 0x0400077C RID: 1916
	[SerializeField]
	private bool alwaysInTheSky;

	// Token: 0x0400077D RID: 1917
	[Header("Model Swap")]
	[SerializeField]
	private Transform defaultMoon;

	// Token: 0x0400077E RID: 1918
	[SerializeField]
	private Transform openMoon;

	// Token: 0x0400077F RID: 1919
	[Header("Animation")]
	[SerializeField]
	private Animator openMoonAnimator;

	// Token: 0x04000780 RID: 1920
	[SerializeField]
	private float eyeOpenDistThreshold = 0.9f;

	// Token: 0x04000781 RID: 1921
	[SerializeField]
	private float eyeCloseDistThreshold = 0.05f;

	// Token: 0x04000782 RID: 1922
	[Header("Debug")]
	[SerializeField]
	private bool debugOverrideTimeOfDay;

	// Token: 0x04000783 RID: 1923
	[SerializeField]
	[Range(0f, 4f)]
	private float timeOfDayOverride;

	// Token: 0x04000784 RID: 1924
	[SerializeField]
	private bool debugOverrideCrackProgress;

	// Token: 0x04000785 RID: 1925
	[SerializeField]
	[Range(0f, 1f)]
	private float crackProgress;

	// Token: 0x04000786 RID: 1926
	[SerializeField]
	private bool debugOverrideCrackDayInOctober;

	// Token: 0x04000787 RID: 1927
	[SerializeField]
	[Range(1f, 31f)]
	private int crackDayInOctoberOverride = 4;

	// Token: 0x04000788 RID: 1928
	[SerializeField]
	private MeshRenderer crackRenderer;

	// Token: 0x04000789 RID: 1929
	private int crackStartDayOfYear;

	// Token: 0x0400078A RID: 1930
	private int crackEndDayOfYear;

	// Token: 0x0400078B RID: 1931
	private float orbitAngle;

	// Token: 0x0400078C RID: 1932
	private int crackProgressHash = Shader.PropertyToID("_Progress");

	// Token: 0x0400078D RID: 1933
	private int eyeOpenHash;

	// Token: 0x0400078E RID: 1934
	private bool openEyeModelEnabled;

	// Token: 0x0400078F RID: 1935
	private float currentlySetCrackProgress;

	// Token: 0x04000790 RID: 1936
	private MaterialPropertyBlock crackMaterialPropertyBlock;

	// Token: 0x04000791 RID: 1937
	private bool debugDrawOrbit;

	// Token: 0x04000792 RID: 1938
	private Dictionary<GTZone, MoonController.Scenes> zoneToSceneMapping = new Dictionary<GTZone, MoonController.Scenes>();

	// Token: 0x04000793 RID: 1939
	private const float moonFallStart = 0.086666666f;

	// Token: 0x04000794 RID: 1940
	private const float moonFallEnd = 0.22f;

	// Token: 0x04000795 RID: 1941
	private const float moonRiseStart = 0.53999996f;

	// Token: 0x04000796 RID: 1942
	private const float moonRiseEnd = 0.6733333f;

	// Token: 0x020000F1 RID: 241
	public enum Scenes
	{
		// Token: 0x04000798 RID: 1944
		Forest,
		// Token: 0x04000799 RID: 1945
		Bayou,
		// Token: 0x0400079A RID: 1946
		Beach,
		// Token: 0x0400079B RID: 1947
		Canyon,
		// Token: 0x0400079C RID: 1948
		Clouds,
		// Token: 0x0400079D RID: 1949
		City,
		// Token: 0x0400079E RID: 1950
		Metropolis,
		// Token: 0x0400079F RID: 1951
		Mountain
	}

	// Token: 0x020000F2 RID: 242
	[Serializable]
	public struct SceneData
	{
		// Token: 0x040007A0 RID: 1952
		public MoonController.Scenes scene;

		// Token: 0x040007A1 RID: 1953
		public Transform referencePoint;

		// Token: 0x040007A2 RID: 1954
		public bool overridePlacement;

		// Token: 0x040007A3 RID: 1955
		public MoonController.Placement PlacementOverride;
	}

	// Token: 0x020000F3 RID: 243
	[Serializable]
	public struct Placement
	{
		// Token: 0x040007A4 RID: 1956
		public Vector2 radiusRange;

		// Token: 0x040007A5 RID: 1957
		public Vector2 heightRange;

		// Token: 0x040007A6 RID: 1958
		public Vector2 scaleRange;

		// Token: 0x040007A7 RID: 1959
		public float restAngle;
	}
}
