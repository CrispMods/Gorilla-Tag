using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000061 RID: 97
public class CrittersRegion : MonoBehaviour
{
	// Token: 0x17000023 RID: 35
	// (get) Token: 0x0600026C RID: 620 RVA: 0x00031E01 File Offset: 0x00030001
	public static List<CrittersRegion> Regions
	{
		get
		{
			return CrittersRegion._regions;
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x0600026D RID: 621 RVA: 0x00031E08 File Offset: 0x00030008
	public int CritterCount
	{
		get
		{
			return this._critters.Count;
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x0600026E RID: 622 RVA: 0x00031E15 File Offset: 0x00030015
	// (set) Token: 0x0600026F RID: 623 RVA: 0x00031E1D File Offset: 0x0003001D
	public int ID { get; private set; }

	// Token: 0x06000270 RID: 624 RVA: 0x00031E26 File Offset: 0x00030026
	private void OnEnable()
	{
		CrittersRegion.RegisterRegion(this);
	}

	// Token: 0x06000271 RID: 625 RVA: 0x00031E2E File Offset: 0x0003002E
	private void OnDisable()
	{
		CrittersRegion.UnregisterRegion(this);
	}

	// Token: 0x06000272 RID: 626 RVA: 0x00031E36 File Offset: 0x00030036
	private static void RegisterRegion(CrittersRegion region)
	{
		CrittersRegion._regionLookup[region.ID] = region;
		CrittersRegion._regions.Add(region);
	}

	// Token: 0x06000273 RID: 627 RVA: 0x00031E54 File Offset: 0x00030054
	private static void UnregisterRegion(CrittersRegion region)
	{
		CrittersRegion._regionLookup.Remove(region.ID);
		CrittersRegion._regions.Remove(region);
	}

	// Token: 0x06000274 RID: 628 RVA: 0x00073E8C File Offset: 0x0007208C
	public static void AddCritterToRegion(CrittersPawn critter, int regionId)
	{
		CrittersRegion crittersRegion;
		if (CrittersRegion._regionLookup.TryGetValue(regionId, out crittersRegion))
		{
			crittersRegion.AddCritter(critter);
			return;
		}
		GTDev.LogError<string>(string.Format("Attempted to add critter to non-existing region {0}.", regionId), null);
	}

	// Token: 0x06000275 RID: 629 RVA: 0x00073EC8 File Offset: 0x000720C8
	public static void RemoveCritterFromRegion(CrittersPawn critter)
	{
		CrittersRegion crittersRegion;
		if (CrittersRegion._regionLookup.TryGetValue(critter.regionId, out crittersRegion))
		{
			crittersRegion.RemoveCritter(critter);
			return;
		}
		GTDev.LogError<string>(string.Format("Couldn't find region with id {0}", critter.regionId), null);
	}

	// Token: 0x06000276 RID: 630 RVA: 0x00031E73 File Offset: 0x00030073
	public void AddCritter(CrittersPawn pawn)
	{
		this._critters.Add(pawn);
	}

	// Token: 0x06000277 RID: 631 RVA: 0x00031E81 File Offset: 0x00030081
	public void RemoveCritter(CrittersPawn pawn)
	{
		this._critters.Remove(pawn);
	}

	// Token: 0x06000278 RID: 632 RVA: 0x00073F0C File Offset: 0x0007210C
	public Vector3 GetSpawnPoint()
	{
		float num = this.scale / 2f;
		float num2 = base.transform.lossyScale.y * this.scale;
		Vector3 vector = base.transform.TransformPoint(new Vector3(UnityEngine.Random.Range(-num, num), num, UnityEngine.Random.Range(-num, num)));
		RaycastHit raycastHit;
		if (Physics.Raycast(vector, -base.transform.up, out raycastHit, num2, -1, QueryTriggerInteraction.Ignore))
		{
			Debug.DrawLine(vector, raycastHit.point, Color.green, 5f);
			return raycastHit.point;
		}
		Debug.DrawLine(vector, vector - base.transform.up * num2, Color.red, 5f);
		return vector;
	}

	// Token: 0x040002F7 RID: 759
	private static List<CrittersRegion> _regions = new List<CrittersRegion>();

	// Token: 0x040002F8 RID: 760
	private static Dictionary<int, CrittersRegion> _regionLookup = new Dictionary<int, CrittersRegion>();

	// Token: 0x040002F9 RID: 761
	public CrittersBiome Biome = CrittersBiome.Any;

	// Token: 0x040002FA RID: 762
	public int maxCritters = 10;

	// Token: 0x040002FB RID: 763
	public float scale = 10f;

	// Token: 0x040002FC RID: 764
	public List<CrittersPawn> _critters = new List<CrittersPawn>();
}
