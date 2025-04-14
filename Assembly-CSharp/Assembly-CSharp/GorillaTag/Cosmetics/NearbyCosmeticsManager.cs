using System;
using System.Collections.Generic;
using CjLib;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C4C RID: 3148
	public class NearbyCosmeticsManager : MonoBehaviour, ITickSystemTick
	{
		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06004E80 RID: 20096 RVA: 0x00181E5D File Offset: 0x0018005D
		public static NearbyCosmeticsManager Instance
		{
			get
			{
				return NearbyCosmeticsManager._instance;
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06004E81 RID: 20097 RVA: 0x00181E64 File Offset: 0x00180064
		// (set) Token: 0x06004E82 RID: 20098 RVA: 0x00181E6C File Offset: 0x0018006C
		public bool TickRunning { get; set; }

		// Token: 0x06004E83 RID: 20099 RVA: 0x00181E75 File Offset: 0x00180075
		private void Awake()
		{
			if (NearbyCosmeticsManager._instance != null && NearbyCosmeticsManager._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			NearbyCosmeticsManager._instance = this;
		}

		// Token: 0x06004E84 RID: 20100 RVA: 0x00019E2F File Offset: 0x0001802F
		private void OnEnable()
		{
			TickSystem<object>.AddCallbackTarget(this);
		}

		// Token: 0x06004E85 RID: 20101 RVA: 0x00019E37 File Offset: 0x00018037
		private void OnDisable()
		{
			TickSystem<object>.RemoveCallbackTarget(this);
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x00181EA3 File Offset: 0x001800A3
		private void OnDestroy()
		{
			if (NearbyCosmeticsManager._instance == this)
			{
				NearbyCosmeticsManager._instance = null;
			}
		}

		// Token: 0x06004E87 RID: 20103 RVA: 0x00181EB8 File Offset: 0x001800B8
		public void Register(NearbyCosmeticsEffect cosmetic)
		{
			this.cosmetics.Add(cosmetic);
		}

		// Token: 0x06004E88 RID: 20104 RVA: 0x00181EC6 File Offset: 0x001800C6
		public void Unregister(NearbyCosmeticsEffect cosmetic)
		{
			this.cosmetics.Remove(cosmetic);
		}

		// Token: 0x06004E89 RID: 20105 RVA: 0x00181ED8 File Offset: 0x001800D8
		public void Tick()
		{
			if (this.cosmetics.Count == 0)
			{
				return;
			}
			this.CheckProximity();
			this.BreakTheBound();
			if (this.debug)
			{
				foreach (NearbyCosmeticsEffect nearbyCosmeticsEffect in this.cosmetics)
				{
					DebugUtil.DrawSphere(nearbyCosmeticsEffect.cosmeticCenter.position, this.proximityThreshold, 6, 6, Color.green, true, DebugUtil.Style.Wireframe);
				}
			}
		}

		// Token: 0x06004E8A RID: 20106 RVA: 0x00181F64 File Offset: 0x00180164
		private void CheckProximity()
		{
			for (int i = 0; i < this.cosmetics.Count; i++)
			{
				NearbyCosmeticsEffect nearbyCosmeticsEffect = this.cosmetics[i];
				for (int j = i + 1; j < this.cosmetics.Count; j++)
				{
					NearbyCosmeticsEffect nearbyCosmeticsEffect2 = this.cosmetics[j];
					if ((!(nearbyCosmeticsEffect.MyRig != null) || !(nearbyCosmeticsEffect.MyRig == nearbyCosmeticsEffect2.MyRig)) && !nearbyCosmeticsEffect.IsMatched && !nearbyCosmeticsEffect2.IsMatched && (nearbyCosmeticsEffect.cosmeticCenter.position - nearbyCosmeticsEffect2.cosmeticCenter.position).IsShorterThan(this.proximityThreshold) && !string.IsNullOrEmpty(nearbyCosmeticsEffect.cosmeticType) && string.Equals(nearbyCosmeticsEffect.cosmeticType, nearbyCosmeticsEffect2.cosmeticType))
					{
						nearbyCosmeticsEffect.PlayEffects(true);
						nearbyCosmeticsEffect2.PlayEffects(false);
						nearbyCosmeticsEffect.IsMatched = true;
						nearbyCosmeticsEffect2.IsMatched = true;
					}
				}
			}
		}

		// Token: 0x06004E8B RID: 20107 RVA: 0x0018205C File Offset: 0x0018025C
		private void BreakTheBound()
		{
			for (int i = 0; i < this.cosmetics.Count; i++)
			{
				NearbyCosmeticsEffect nearbyCosmeticsEffect = this.cosmetics[i];
				bool isMatched = false;
				if (nearbyCosmeticsEffect.IsMatched)
				{
					for (int j = 0; j < this.cosmetics.Count; j++)
					{
						if (i != j)
						{
							NearbyCosmeticsEffect nearbyCosmeticsEffect2 = this.cosmetics[j];
							if ((nearbyCosmeticsEffect.cosmeticCenter.position - nearbyCosmeticsEffect2.cosmeticCenter.position).IsShorterThan(this.proximityThreshold) && !string.IsNullOrEmpty(nearbyCosmeticsEffect.cosmeticType) && string.Equals(nearbyCosmeticsEffect.cosmeticType, nearbyCosmeticsEffect2.cosmeticType))
							{
								isMatched = true;
								break;
							}
						}
					}
					nearbyCosmeticsEffect.IsMatched = isMatched;
				}
			}
		}

		// Token: 0x04005215 RID: 21013
		[SerializeField]
		private float proximityThreshold = 0.1f;

		// Token: 0x04005216 RID: 21014
		[SerializeField]
		private bool debug;

		// Token: 0x04005217 RID: 21015
		private List<NearbyCosmeticsEffect> cosmetics = new List<NearbyCosmeticsEffect>();

		// Token: 0x04005218 RID: 21016
		private static NearbyCosmeticsManager _instance;
	}
}
