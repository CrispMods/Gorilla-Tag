﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E8 RID: 232
public class ButterflySwarmManager : MonoBehaviour
{
	// Token: 0x1700007B RID: 123
	// (get) Token: 0x060005FF RID: 1535 RVA: 0x0002352F File Offset: 0x0002172F
	// (set) Token: 0x06000600 RID: 1536 RVA: 0x00023537 File Offset: 0x00021737
	public float PerchedFlapSpeed { get; private set; }

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x06000601 RID: 1537 RVA: 0x00023540 File Offset: 0x00021740
	// (set) Token: 0x06000602 RID: 1538 RVA: 0x00023548 File Offset: 0x00021748
	public float PerchedFlapPhase { get; private set; }

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x06000603 RID: 1539 RVA: 0x00023551 File Offset: 0x00021751
	// (set) Token: 0x06000604 RID: 1540 RVA: 0x00023559 File Offset: 0x00021759
	public float BeeSpeed { get; private set; }

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x06000605 RID: 1541 RVA: 0x00023562 File Offset: 0x00021762
	// (set) Token: 0x06000606 RID: 1542 RVA: 0x0002356A File Offset: 0x0002176A
	public float BeeMaxTravelTime { get; private set; }

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06000607 RID: 1543 RVA: 0x00023573 File Offset: 0x00021773
	// (set) Token: 0x06000608 RID: 1544 RVA: 0x0002357B File Offset: 0x0002177B
	public float BeeAcceleration { get; private set; }

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000609 RID: 1545 RVA: 0x00023584 File Offset: 0x00021784
	// (set) Token: 0x0600060A RID: 1546 RVA: 0x0002358C File Offset: 0x0002178C
	public float BeeJitterStrength { get; private set; }

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x0600060B RID: 1547 RVA: 0x00023595 File Offset: 0x00021795
	// (set) Token: 0x0600060C RID: 1548 RVA: 0x0002359D File Offset: 0x0002179D
	public float BeeJitterDamping { get; private set; }

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x0600060D RID: 1549 RVA: 0x000235A6 File Offset: 0x000217A6
	// (set) Token: 0x0600060E RID: 1550 RVA: 0x000235AE File Offset: 0x000217AE
	public float BeeMaxJitterRadius { get; private set; }

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x0600060F RID: 1551 RVA: 0x000235B7 File Offset: 0x000217B7
	// (set) Token: 0x06000610 RID: 1552 RVA: 0x000235BF File Offset: 0x000217BF
	public float BeeNearDestinationRadius { get; private set; }

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000611 RID: 1553 RVA: 0x000235C8 File Offset: 0x000217C8
	// (set) Token: 0x06000612 RID: 1554 RVA: 0x000235D0 File Offset: 0x000217D0
	public float DestRotationAlignmentSpeed { get; private set; }

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x06000613 RID: 1555 RVA: 0x000235D9 File Offset: 0x000217D9
	// (set) Token: 0x06000614 RID: 1556 RVA: 0x000235E1 File Offset: 0x000217E1
	public Vector3 TravellingLocalRotationEuler { get; private set; }

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x06000615 RID: 1557 RVA: 0x000235EA File Offset: 0x000217EA
	// (set) Token: 0x06000616 RID: 1558 RVA: 0x000235F2 File Offset: 0x000217F2
	public Quaternion TravellingLocalRotation { get; private set; }

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x06000617 RID: 1559 RVA: 0x000235FB File Offset: 0x000217FB
	// (set) Token: 0x06000618 RID: 1560 RVA: 0x00023603 File Offset: 0x00021803
	public float AvoidPointRadius { get; private set; }

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x06000619 RID: 1561 RVA: 0x0002360C File Offset: 0x0002180C
	// (set) Token: 0x0600061A RID: 1562 RVA: 0x00023614 File Offset: 0x00021814
	public float BeeMinFlowerDuration { get; private set; }

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x0600061B RID: 1563 RVA: 0x0002361D File Offset: 0x0002181D
	// (set) Token: 0x0600061C RID: 1564 RVA: 0x00023625 File Offset: 0x00021825
	public float BeeMaxFlowerDuration { get; private set; }

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x0600061D RID: 1565 RVA: 0x0002362E File Offset: 0x0002182E
	// (set) Token: 0x0600061E RID: 1566 RVA: 0x00023636 File Offset: 0x00021836
	public Color[] BeeColors { get; private set; }

	// Token: 0x0600061F RID: 1567 RVA: 0x00023640 File Offset: 0x00021840
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

	// Token: 0x06000620 RID: 1568 RVA: 0x000236C8 File Offset: 0x000218C8
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

	// Token: 0x06000621 RID: 1569 RVA: 0x00023788 File Offset: 0x00021988
	private void OnDestroy()
	{
		RandomTimedSeedManager.instance.RemoveCallbackOnSeedChanged(new Action(this.OnSeedChange));
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x000237A0 File Offset: 0x000219A0
	private void Update()
	{
		for (int i = 0; i < this.butterflies.Count; i++)
		{
			AnimatedButterfly value = this.butterflies[i];
			value.UpdateVisual(RandomTimedSeedManager.instance.currentSyncTime, this);
			this.butterflies[i] = value;
		}
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x000237F0 File Offset: 0x000219F0
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

	// Token: 0x06000624 RID: 1572 RVA: 0x00023934 File Offset: 0x00021B34
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

	// Token: 0x0400072C RID: 1836
	[SerializeField]
	private XSceneRef[] perchSections;

	// Token: 0x0400072D RID: 1837
	[SerializeField]
	private int loopSizePerBee;

	// Token: 0x0400072E RID: 1838
	[SerializeField]
	private int numBees;

	// Token: 0x0400072F RID: 1839
	[SerializeField]
	private MeshRenderer beePrefab;

	// Token: 0x04000730 RID: 1840
	[SerializeField]
	private float maxFlapSpeed;

	// Token: 0x04000731 RID: 1841
	[SerializeField]
	private float minFlapSpeed;

	// Token: 0x04000742 RID: 1858
	private List<AnimatedButterfly> butterflies;

	// Token: 0x04000743 RID: 1859
	private List<List<GameObject>> allPerchZones = new List<List<GameObject>>();
}
