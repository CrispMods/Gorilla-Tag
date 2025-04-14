using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E7 RID: 231
public class BeeSwarmManager : MonoBehaviour
{
	// Token: 0x1700006F RID: 111
	// (get) Token: 0x060005DD RID: 1501 RVA: 0x00023086 File Offset: 0x00021286
	// (set) Token: 0x060005DE RID: 1502 RVA: 0x0002308E File Offset: 0x0002128E
	public BeePerchPoint BeeHive { get; private set; }

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x060005DF RID: 1503 RVA: 0x00023097 File Offset: 0x00021297
	// (set) Token: 0x060005E0 RID: 1504 RVA: 0x0002309F File Offset: 0x0002129F
	public float BeeSpeed { get; private set; }

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x060005E1 RID: 1505 RVA: 0x000230A8 File Offset: 0x000212A8
	// (set) Token: 0x060005E2 RID: 1506 RVA: 0x000230B0 File Offset: 0x000212B0
	public float BeeMaxTravelTime { get; private set; }

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x060005E3 RID: 1507 RVA: 0x000230B9 File Offset: 0x000212B9
	// (set) Token: 0x060005E4 RID: 1508 RVA: 0x000230C1 File Offset: 0x000212C1
	public float BeeAcceleration { get; private set; }

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x060005E5 RID: 1509 RVA: 0x000230CA File Offset: 0x000212CA
	// (set) Token: 0x060005E6 RID: 1510 RVA: 0x000230D2 File Offset: 0x000212D2
	public float BeeJitterStrength { get; private set; }

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x060005E7 RID: 1511 RVA: 0x000230DB File Offset: 0x000212DB
	// (set) Token: 0x060005E8 RID: 1512 RVA: 0x000230E3 File Offset: 0x000212E3
	public float BeeJitterDamping { get; private set; }

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x060005E9 RID: 1513 RVA: 0x000230EC File Offset: 0x000212EC
	// (set) Token: 0x060005EA RID: 1514 RVA: 0x000230F4 File Offset: 0x000212F4
	public float BeeMaxJitterRadius { get; private set; }

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x060005EB RID: 1515 RVA: 0x000230FD File Offset: 0x000212FD
	// (set) Token: 0x060005EC RID: 1516 RVA: 0x00023105 File Offset: 0x00021305
	public float BeeNearDestinationRadius { get; private set; }

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x060005ED RID: 1517 RVA: 0x0002310E File Offset: 0x0002130E
	// (set) Token: 0x060005EE RID: 1518 RVA: 0x00023116 File Offset: 0x00021316
	public float AvoidPointRadius { get; private set; }

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x060005EF RID: 1519 RVA: 0x0002311F File Offset: 0x0002131F
	// (set) Token: 0x060005F0 RID: 1520 RVA: 0x00023127 File Offset: 0x00021327
	public float BeeMinFlowerDuration { get; private set; }

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x060005F1 RID: 1521 RVA: 0x00023130 File Offset: 0x00021330
	// (set) Token: 0x060005F2 RID: 1522 RVA: 0x00023138 File Offset: 0x00021338
	public float BeeMaxFlowerDuration { get; private set; }

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x060005F3 RID: 1523 RVA: 0x00023141 File Offset: 0x00021341
	// (set) Token: 0x060005F4 RID: 1524 RVA: 0x00023149 File Offset: 0x00021349
	public float GeneralBuzzRange { get; private set; }

	// Token: 0x060005F5 RID: 1525 RVA: 0x00023154 File Offset: 0x00021354
	private void Awake()
	{
		this.bees = new List<AnimatedBee>(this.numBees);
		for (int i = 0; i < this.numBees; i++)
		{
			AnimatedBee item = default(AnimatedBee);
			item.InitVisual(this.beePrefab, this);
			this.bees.Add(item);
		}
		this.playerCamera = Camera.main.transform;
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x000231B8 File Offset: 0x000213B8
	private void Start()
	{
		foreach (XSceneRef xsceneRef in this.flowerSections)
		{
			GameObject gameObject;
			if (xsceneRef.TryResolve(out gameObject))
			{
				foreach (BeePerchPoint item in gameObject.GetComponentsInChildren<BeePerchPoint>())
				{
					this.allPerchPoints.Add(item);
				}
			}
		}
		this.OnSeedChange();
		RandomTimedSeedManager.instance.AddCallbackOnSeedChanged(new Action(this.OnSeedChange));
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x00023238 File Offset: 0x00021438
	private void OnDestroy()
	{
		RandomTimedSeedManager.instance.RemoveCallbackOnSeedChanged(new Action(this.OnSeedChange));
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x00023250 File Offset: 0x00021450
	private void Update()
	{
		Vector3 position = this.playerCamera.transform.position;
		Vector3 position2 = Vector3.zero;
		Vector3 a = Vector3.zero;
		float num = 1f / (float)this.bees.Count;
		float num2 = float.PositiveInfinity;
		float num3 = this.GeneralBuzzRange * this.GeneralBuzzRange;
		int num4 = 0;
		for (int i = 0; i < this.bees.Count; i++)
		{
			AnimatedBee animatedBee = this.bees[i];
			animatedBee.UpdateVisual(RandomTimedSeedManager.instance.currentSyncTime, this);
			Vector3 position3 = animatedBee.visual.transform.position;
			float sqrMagnitude = (position3 - position).sqrMagnitude;
			if (sqrMagnitude < num2)
			{
				position2 = position3;
				num2 = sqrMagnitude;
			}
			if (sqrMagnitude < num3)
			{
				a += position3;
				num4++;
			}
			this.bees[i] = animatedBee;
		}
		this.nearbyBeeBuzz.transform.position = position2;
		if (num4 > 0)
		{
			this.generalBeeBuzz.transform.position = a / (float)num4;
			this.generalBeeBuzz.enabled = true;
			return;
		}
		this.generalBeeBuzz.enabled = false;
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x00023380 File Offset: 0x00021580
	private void OnSeedChange()
	{
		SRand srand = new SRand(RandomTimedSeedManager.instance.seed);
		List<BeePerchPoint> pickBuffer = new List<BeePerchPoint>(this.allPerchPoints.Count);
		List<BeePerchPoint> list = new List<BeePerchPoint>(this.loopSizePerBee);
		List<float> list2 = new List<float>(this.loopSizePerBee);
		for (int i = 0; i < this.bees.Count; i++)
		{
			AnimatedBee value = this.bees[i];
			list = new List<BeePerchPoint>(this.loopSizePerBee);
			list2 = new List<float>(this.loopSizePerBee);
			this.PickPoints(this.loopSizePerBee, pickBuffer, this.allPerchPoints, ref srand, list);
			for (int j = 0; j < list.Count; j++)
			{
				list2.Add(srand.NextFloat(this.BeeMinFlowerDuration, this.BeeMaxFlowerDuration));
			}
			value.InitRoute(list, list2, this);
			value.InitRouteTimestamps();
			this.bees[i] = value;
		}
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x00023474 File Offset: 0x00021674
	private void PickPoints(int n, List<BeePerchPoint> pickBuffer, List<BeePerchPoint> allPerchPoints, ref SRand rand, List<BeePerchPoint> resultBuffer)
	{
		resultBuffer.Add(this.BeeHive);
		n--;
		int num = 100;
		while (pickBuffer.Count < n && num-- > 0)
		{
			n -= pickBuffer.Count;
			resultBuffer.AddRange(pickBuffer);
			pickBuffer.Clear();
			pickBuffer.AddRange(allPerchPoints);
			rand.Shuffle<BeePerchPoint>(pickBuffer);
		}
		resultBuffer.AddRange(pickBuffer.GetRange(pickBuffer.Count - n, n));
		pickBuffer.RemoveRange(pickBuffer.Count - n, n);
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x000234F5 File Offset: 0x000216F5
	public static void RegisterAvoidPoint(GameObject obj)
	{
		BeeSwarmManager.avoidPoints.Add(obj);
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x00023502 File Offset: 0x00021702
	public static void UnregisterAvoidPoint(GameObject obj)
	{
		BeeSwarmManager.avoidPoints.Remove(obj);
	}

	// Token: 0x04000715 RID: 1813
	[SerializeField]
	private XSceneRef[] flowerSections;

	// Token: 0x04000716 RID: 1814
	[SerializeField]
	private int loopSizePerBee;

	// Token: 0x04000717 RID: 1815
	[SerializeField]
	private int numBees;

	// Token: 0x04000718 RID: 1816
	[SerializeField]
	private MeshRenderer beePrefab;

	// Token: 0x04000719 RID: 1817
	[SerializeField]
	private AudioSource nearbyBeeBuzz;

	// Token: 0x0400071A RID: 1818
	[SerializeField]
	private AudioSource generalBeeBuzz;

	// Token: 0x0400071B RID: 1819
	private GameObject[] flowerSectionsResolved;

	// Token: 0x04000728 RID: 1832
	private List<AnimatedBee> bees;

	// Token: 0x04000729 RID: 1833
	private Transform playerCamera;

	// Token: 0x0400072A RID: 1834
	private List<BeePerchPoint> allPerchPoints = new List<BeePerchPoint>();

	// Token: 0x0400072B RID: 1835
	public static readonly List<GameObject> avoidPoints = new List<GameObject>();
}
