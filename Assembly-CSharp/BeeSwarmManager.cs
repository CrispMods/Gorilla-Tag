using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class BeeSwarmManager : MonoBehaviour
{
	// Token: 0x17000074 RID: 116
	// (get) Token: 0x0600061E RID: 1566 RVA: 0x0003494C File Offset: 0x00032B4C
	// (set) Token: 0x0600061F RID: 1567 RVA: 0x00034954 File Offset: 0x00032B54
	public BeePerchPoint BeeHive { get; private set; }

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x06000620 RID: 1568 RVA: 0x0003495D File Offset: 0x00032B5D
	// (set) Token: 0x06000621 RID: 1569 RVA: 0x00034965 File Offset: 0x00032B65
	public float BeeSpeed { get; private set; }

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x06000622 RID: 1570 RVA: 0x0003496E File Offset: 0x00032B6E
	// (set) Token: 0x06000623 RID: 1571 RVA: 0x00034976 File Offset: 0x00032B76
	public float BeeMaxTravelTime { get; private set; }

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x06000624 RID: 1572 RVA: 0x0003497F File Offset: 0x00032B7F
	// (set) Token: 0x06000625 RID: 1573 RVA: 0x00034987 File Offset: 0x00032B87
	public float BeeAcceleration { get; private set; }

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x06000626 RID: 1574 RVA: 0x00034990 File Offset: 0x00032B90
	// (set) Token: 0x06000627 RID: 1575 RVA: 0x00034998 File Offset: 0x00032B98
	public float BeeJitterStrength { get; private set; }

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x06000628 RID: 1576 RVA: 0x000349A1 File Offset: 0x00032BA1
	// (set) Token: 0x06000629 RID: 1577 RVA: 0x000349A9 File Offset: 0x00032BA9
	public float BeeJitterDamping { get; private set; }

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x0600062A RID: 1578 RVA: 0x000349B2 File Offset: 0x00032BB2
	// (set) Token: 0x0600062B RID: 1579 RVA: 0x000349BA File Offset: 0x00032BBA
	public float BeeMaxJitterRadius { get; private set; }

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x0600062C RID: 1580 RVA: 0x000349C3 File Offset: 0x00032BC3
	// (set) Token: 0x0600062D RID: 1581 RVA: 0x000349CB File Offset: 0x00032BCB
	public float BeeNearDestinationRadius { get; private set; }

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x0600062E RID: 1582 RVA: 0x000349D4 File Offset: 0x00032BD4
	// (set) Token: 0x0600062F RID: 1583 RVA: 0x000349DC File Offset: 0x00032BDC
	public float AvoidPointRadius { get; private set; }

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x06000630 RID: 1584 RVA: 0x000349E5 File Offset: 0x00032BE5
	// (set) Token: 0x06000631 RID: 1585 RVA: 0x000349ED File Offset: 0x00032BED
	public float BeeMinFlowerDuration { get; private set; }

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x06000632 RID: 1586 RVA: 0x000349F6 File Offset: 0x00032BF6
	// (set) Token: 0x06000633 RID: 1587 RVA: 0x000349FE File Offset: 0x00032BFE
	public float BeeMaxFlowerDuration { get; private set; }

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06000634 RID: 1588 RVA: 0x00034A07 File Offset: 0x00032C07
	// (set) Token: 0x06000635 RID: 1589 RVA: 0x00034A0F File Offset: 0x00032C0F
	public float GeneralBuzzRange { get; private set; }

	// Token: 0x06000636 RID: 1590 RVA: 0x000858CC File Offset: 0x00083ACC
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

	// Token: 0x06000637 RID: 1591 RVA: 0x00085930 File Offset: 0x00083B30
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

	// Token: 0x06000638 RID: 1592 RVA: 0x00034A18 File Offset: 0x00032C18
	private void OnDestroy()
	{
		RandomTimedSeedManager.instance.RemoveCallbackOnSeedChanged(new Action(this.OnSeedChange));
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x000859B0 File Offset: 0x00083BB0
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

	// Token: 0x0600063A RID: 1594 RVA: 0x00085AE0 File Offset: 0x00083CE0
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

	// Token: 0x0600063B RID: 1595 RVA: 0x00085BD4 File Offset: 0x00083DD4
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

	// Token: 0x0600063C RID: 1596 RVA: 0x00034A30 File Offset: 0x00032C30
	public static void RegisterAvoidPoint(GameObject obj)
	{
		BeeSwarmManager.avoidPoints.Add(obj);
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x00034A3D File Offset: 0x00032C3D
	public static void UnregisterAvoidPoint(GameObject obj)
	{
		BeeSwarmManager.avoidPoints.Remove(obj);
	}

	// Token: 0x04000756 RID: 1878
	[SerializeField]
	private XSceneRef[] flowerSections;

	// Token: 0x04000757 RID: 1879
	[SerializeField]
	private int loopSizePerBee;

	// Token: 0x04000758 RID: 1880
	[SerializeField]
	private int numBees;

	// Token: 0x04000759 RID: 1881
	[SerializeField]
	private MeshRenderer beePrefab;

	// Token: 0x0400075A RID: 1882
	[SerializeField]
	private AudioSource nearbyBeeBuzz;

	// Token: 0x0400075B RID: 1883
	[SerializeField]
	private AudioSource generalBeeBuzz;

	// Token: 0x0400075C RID: 1884
	private GameObject[] flowerSectionsResolved;

	// Token: 0x04000769 RID: 1897
	private List<AnimatedBee> bees;

	// Token: 0x0400076A RID: 1898
	private Transform playerCamera;

	// Token: 0x0400076B RID: 1899
	private List<BeePerchPoint> allPerchPoints = new List<BeePerchPoint>();

	// Token: 0x0400076C RID: 1900
	public static readonly List<GameObject> avoidPoints = new List<GameObject>();
}
