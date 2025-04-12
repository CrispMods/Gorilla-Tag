using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E7 RID: 231
public class BeeSwarmManager : MonoBehaviour
{
	// Token: 0x1700006F RID: 111
	// (get) Token: 0x060005DF RID: 1503 RVA: 0x000336E8 File Offset: 0x000318E8
	// (set) Token: 0x060005E0 RID: 1504 RVA: 0x000336F0 File Offset: 0x000318F0
	public BeePerchPoint BeeHive { get; private set; }

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x060005E1 RID: 1505 RVA: 0x000336F9 File Offset: 0x000318F9
	// (set) Token: 0x060005E2 RID: 1506 RVA: 0x00033701 File Offset: 0x00031901
	public float BeeSpeed { get; private set; }

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0003370A File Offset: 0x0003190A
	// (set) Token: 0x060005E4 RID: 1508 RVA: 0x00033712 File Offset: 0x00031912
	public float BeeMaxTravelTime { get; private set; }

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x060005E5 RID: 1509 RVA: 0x0003371B File Offset: 0x0003191B
	// (set) Token: 0x060005E6 RID: 1510 RVA: 0x00033723 File Offset: 0x00031923
	public float BeeAcceleration { get; private set; }

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x060005E7 RID: 1511 RVA: 0x0003372C File Offset: 0x0003192C
	// (set) Token: 0x060005E8 RID: 1512 RVA: 0x00033734 File Offset: 0x00031934
	public float BeeJitterStrength { get; private set; }

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x060005E9 RID: 1513 RVA: 0x0003373D File Offset: 0x0003193D
	// (set) Token: 0x060005EA RID: 1514 RVA: 0x00033745 File Offset: 0x00031945
	public float BeeJitterDamping { get; private set; }

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x060005EB RID: 1515 RVA: 0x0003374E File Offset: 0x0003194E
	// (set) Token: 0x060005EC RID: 1516 RVA: 0x00033756 File Offset: 0x00031956
	public float BeeMaxJitterRadius { get; private set; }

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x060005ED RID: 1517 RVA: 0x0003375F File Offset: 0x0003195F
	// (set) Token: 0x060005EE RID: 1518 RVA: 0x00033767 File Offset: 0x00031967
	public float BeeNearDestinationRadius { get; private set; }

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x060005EF RID: 1519 RVA: 0x00033770 File Offset: 0x00031970
	// (set) Token: 0x060005F0 RID: 1520 RVA: 0x00033778 File Offset: 0x00031978
	public float AvoidPointRadius { get; private set; }

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x060005F1 RID: 1521 RVA: 0x00033781 File Offset: 0x00031981
	// (set) Token: 0x060005F2 RID: 1522 RVA: 0x00033789 File Offset: 0x00031989
	public float BeeMinFlowerDuration { get; private set; }

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x060005F3 RID: 1523 RVA: 0x00033792 File Offset: 0x00031992
	// (set) Token: 0x060005F4 RID: 1524 RVA: 0x0003379A File Offset: 0x0003199A
	public float BeeMaxFlowerDuration { get; private set; }

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x060005F5 RID: 1525 RVA: 0x000337A3 File Offset: 0x000319A3
	// (set) Token: 0x060005F6 RID: 1526 RVA: 0x000337AB File Offset: 0x000319AB
	public float GeneralBuzzRange { get; private set; }

	// Token: 0x060005F7 RID: 1527 RVA: 0x00082FC4 File Offset: 0x000811C4
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

	// Token: 0x060005F8 RID: 1528 RVA: 0x00083028 File Offset: 0x00081228
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

	// Token: 0x060005F9 RID: 1529 RVA: 0x000337B4 File Offset: 0x000319B4
	private void OnDestroy()
	{
		RandomTimedSeedManager.instance.RemoveCallbackOnSeedChanged(new Action(this.OnSeedChange));
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x000830A8 File Offset: 0x000812A8
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

	// Token: 0x060005FB RID: 1531 RVA: 0x000831D8 File Offset: 0x000813D8
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

	// Token: 0x060005FC RID: 1532 RVA: 0x000832CC File Offset: 0x000814CC
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

	// Token: 0x060005FD RID: 1533 RVA: 0x000337CC File Offset: 0x000319CC
	public static void RegisterAvoidPoint(GameObject obj)
	{
		BeeSwarmManager.avoidPoints.Add(obj);
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x000337D9 File Offset: 0x000319D9
	public static void UnregisterAvoidPoint(GameObject obj)
	{
		BeeSwarmManager.avoidPoints.Remove(obj);
	}

	// Token: 0x04000716 RID: 1814
	[SerializeField]
	private XSceneRef[] flowerSections;

	// Token: 0x04000717 RID: 1815
	[SerializeField]
	private int loopSizePerBee;

	// Token: 0x04000718 RID: 1816
	[SerializeField]
	private int numBees;

	// Token: 0x04000719 RID: 1817
	[SerializeField]
	private MeshRenderer beePrefab;

	// Token: 0x0400071A RID: 1818
	[SerializeField]
	private AudioSource nearbyBeeBuzz;

	// Token: 0x0400071B RID: 1819
	[SerializeField]
	private AudioSource generalBeeBuzz;

	// Token: 0x0400071C RID: 1820
	private GameObject[] flowerSectionsResolved;

	// Token: 0x04000729 RID: 1833
	private List<AnimatedBee> bees;

	// Token: 0x0400072A RID: 1834
	private Transform playerCamera;

	// Token: 0x0400072B RID: 1835
	private List<BeePerchPoint> allPerchPoints = new List<BeePerchPoint>();

	// Token: 0x0400072C RID: 1836
	public static readonly List<GameObject> avoidPoints = new List<GameObject>();
}
