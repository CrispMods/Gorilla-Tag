using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F2 RID: 242
public class ButterflySwarmManager : MonoBehaviour
{
	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000640 RID: 1600 RVA: 0x00034A6A File Offset: 0x00032C6A
	// (set) Token: 0x06000641 RID: 1601 RVA: 0x00034A72 File Offset: 0x00032C72
	public float PerchedFlapSpeed { get; private set; }

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x06000642 RID: 1602 RVA: 0x00034A7B File Offset: 0x00032C7B
	// (set) Token: 0x06000643 RID: 1603 RVA: 0x00034A83 File Offset: 0x00032C83
	public float PerchedFlapPhase { get; private set; }

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06000644 RID: 1604 RVA: 0x00034A8C File Offset: 0x00032C8C
	// (set) Token: 0x06000645 RID: 1605 RVA: 0x00034A94 File Offset: 0x00032C94
	public float BeeSpeed { get; private set; }

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06000646 RID: 1606 RVA: 0x00034A9D File Offset: 0x00032C9D
	// (set) Token: 0x06000647 RID: 1607 RVA: 0x00034AA5 File Offset: 0x00032CA5
	public float BeeMaxTravelTime { get; private set; }

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000648 RID: 1608 RVA: 0x00034AAE File Offset: 0x00032CAE
	// (set) Token: 0x06000649 RID: 1609 RVA: 0x00034AB6 File Offset: 0x00032CB6
	public float BeeAcceleration { get; private set; }

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x0600064A RID: 1610 RVA: 0x00034ABF File Offset: 0x00032CBF
	// (set) Token: 0x0600064B RID: 1611 RVA: 0x00034AC7 File Offset: 0x00032CC7
	public float BeeJitterStrength { get; private set; }

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x0600064C RID: 1612 RVA: 0x00034AD0 File Offset: 0x00032CD0
	// (set) Token: 0x0600064D RID: 1613 RVA: 0x00034AD8 File Offset: 0x00032CD8
	public float BeeJitterDamping { get; private set; }

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x0600064E RID: 1614 RVA: 0x00034AE1 File Offset: 0x00032CE1
	// (set) Token: 0x0600064F RID: 1615 RVA: 0x00034AE9 File Offset: 0x00032CE9
	public float BeeMaxJitterRadius { get; private set; }

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x06000650 RID: 1616 RVA: 0x00034AF2 File Offset: 0x00032CF2
	// (set) Token: 0x06000651 RID: 1617 RVA: 0x00034AFA File Offset: 0x00032CFA
	public float BeeNearDestinationRadius { get; private set; }

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x06000652 RID: 1618 RVA: 0x00034B03 File Offset: 0x00032D03
	// (set) Token: 0x06000653 RID: 1619 RVA: 0x00034B0B File Offset: 0x00032D0B
	public float DestRotationAlignmentSpeed { get; private set; }

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x06000654 RID: 1620 RVA: 0x00034B14 File Offset: 0x00032D14
	// (set) Token: 0x06000655 RID: 1621 RVA: 0x00034B1C File Offset: 0x00032D1C
	public Vector3 TravellingLocalRotationEuler { get; private set; }

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x06000656 RID: 1622 RVA: 0x00034B25 File Offset: 0x00032D25
	// (set) Token: 0x06000657 RID: 1623 RVA: 0x00034B2D File Offset: 0x00032D2D
	public Quaternion TravellingLocalRotation { get; private set; }

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x06000658 RID: 1624 RVA: 0x00034B36 File Offset: 0x00032D36
	// (set) Token: 0x06000659 RID: 1625 RVA: 0x00034B3E File Offset: 0x00032D3E
	public float AvoidPointRadius { get; private set; }

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x0600065A RID: 1626 RVA: 0x00034B47 File Offset: 0x00032D47
	// (set) Token: 0x0600065B RID: 1627 RVA: 0x00034B4F File Offset: 0x00032D4F
	public float BeeMinFlowerDuration { get; private set; }

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x0600065C RID: 1628 RVA: 0x00034B58 File Offset: 0x00032D58
	// (set) Token: 0x0600065D RID: 1629 RVA: 0x00034B60 File Offset: 0x00032D60
	public float BeeMaxFlowerDuration { get; private set; }

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x0600065E RID: 1630 RVA: 0x00034B69 File Offset: 0x00032D69
	// (set) Token: 0x0600065F RID: 1631 RVA: 0x00034B71 File Offset: 0x00032D71
	public Color[] BeeColors { get; private set; }

	// Token: 0x06000660 RID: 1632 RVA: 0x00085C58 File Offset: 0x00083E58
	private void Awake()
	{
		this.TravellingLocalRotation = Quaternion.Euler(this.TravellingLocalRotationEuler);
		this.butterflies = new List<AnimatedButterfly>(this.numBees);
		for (int i = 0; i < this.numBees; i++)
		{
			AnimatedButterfly item = default(AnimatedButterfly);
			item.InitVisual(this.beePrefab, this);
			if (this.BeeColors.Length != 0)
			{
				item.SetColor(this.BeeColors[i % this.BeeColors.Length]);
			}
			this.butterflies.Add(item);
		}
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x00085CE0 File Offset: 0x00083EE0
	private void Start()
	{
		foreach (XSceneRef xsceneRef in this.perchSections)
		{
			GameObject gameObject;
			if (xsceneRef.TryResolve(out gameObject))
			{
				List<GameObject> list = new List<GameObject>();
				this.allPerchZones.Add(list);
				foreach (object obj in gameObject.transform)
				{
					Transform transform = (Transform)obj;
					list.Add(transform.gameObject);
				}
			}
		}
		this.OnSeedChange();
		RandomTimedSeedManager.instance.AddCallbackOnSeedChanged(new Action(this.OnSeedChange));
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x00034B7A File Offset: 0x00032D7A
	private void OnDestroy()
	{
		RandomTimedSeedManager.instance.RemoveCallbackOnSeedChanged(new Action(this.OnSeedChange));
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x00085DA0 File Offset: 0x00083FA0
	private void Update()
	{
		for (int i = 0; i < this.butterflies.Count; i++)
		{
			AnimatedButterfly value = this.butterflies[i];
			value.UpdateVisual(RandomTimedSeedManager.instance.currentSyncTime, this);
			this.butterflies[i] = value;
		}
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x00085DF0 File Offset: 0x00083FF0
	private void OnSeedChange()
	{
		SRand srand = new SRand(RandomTimedSeedManager.instance.seed);
		List<List<GameObject>> list = new List<List<GameObject>>(this.allPerchZones.Count);
		for (int i = 0; i < this.allPerchZones.Count; i++)
		{
			List<GameObject> list2 = new List<GameObject>();
			list2.AddRange(this.allPerchZones[i]);
			list.Add(list2);
		}
		List<GameObject> list3 = new List<GameObject>(this.loopSizePerBee);
		List<float> list4 = new List<float>(this.loopSizePerBee);
		for (int j = 0; j < this.butterflies.Count; j++)
		{
			AnimatedButterfly value = this.butterflies[j];
			value.SetFlapSpeed(srand.NextFloat(this.minFlapSpeed, this.maxFlapSpeed));
			list3.Clear();
			list4.Clear();
			this.PickPoints(this.loopSizePerBee, list, ref srand, list3);
			for (int k = 0; k < list3.Count; k++)
			{
				list4.Add(srand.NextFloat(this.BeeMinFlowerDuration, this.BeeMaxFlowerDuration));
			}
			if (list3.Count == 0)
			{
				this.butterflies.Clear();
				return;
			}
			value.InitRoute(list3, list4, this);
			this.butterflies[j] = value;
		}
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x00085F34 File Offset: 0x00084134
	private void PickPoints(int n, List<List<GameObject>> pickBuffer, ref SRand rand, List<GameObject> resultBuffer)
	{
		int exclude = rand.NextInt(0, pickBuffer.Count);
		int num = -1;
		int num2 = n - 2;
		while (resultBuffer.Count < n)
		{
			int num3;
			if (resultBuffer.Count < num2)
			{
				num3 = rand.NextIntWithExclusion(0, pickBuffer.Count, num);
			}
			else
			{
				num3 = rand.NextIntWithExclusion2(0, pickBuffer.Count, num, exclude);
			}
			int num4 = 10;
			while (num3 == num || pickBuffer[num3].Count == 0)
			{
				num3 = (num3 + 1) % pickBuffer.Count;
				num4--;
				if (num4 <= 0)
				{
					return;
				}
			}
			num = num3;
			List<GameObject> list = pickBuffer[num];
			while (list.Count == 0)
			{
				num = (num + 1) % pickBuffer.Count;
				list = pickBuffer[num];
			}
			resultBuffer.Add(list[list.Count - 1]);
			list.RemoveAt(list.Count - 1);
		}
	}

	// Token: 0x0400076D RID: 1901
	[SerializeField]
	private XSceneRef[] perchSections;

	// Token: 0x0400076E RID: 1902
	[SerializeField]
	private int loopSizePerBee;

	// Token: 0x0400076F RID: 1903
	[SerializeField]
	private int numBees;

	// Token: 0x04000770 RID: 1904
	[SerializeField]
	private MeshRenderer beePrefab;

	// Token: 0x04000771 RID: 1905
	[SerializeField]
	private float maxFlapSpeed;

	// Token: 0x04000772 RID: 1906
	[SerializeField]
	private float minFlapSpeed;

	// Token: 0x04000783 RID: 1923
	private List<AnimatedButterfly> butterflies;

	// Token: 0x04000784 RID: 1924
	private List<List<GameObject>> allPerchZones = new List<List<GameObject>>();
}
