using System;
using System.Collections.Generic;
using CjLib;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C7A RID: 3194
	public class NearbyCosmeticsManager : MonoBehaviour, ITickSystemTick
	{
		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06004FD4 RID: 20436 RVA: 0x000641F7 File Offset: 0x000623F7
		public static NearbyCosmeticsManager Instance
		{
			get
			{
				return NearbyCosmeticsManager._instance;
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06004FD5 RID: 20437 RVA: 0x000641FE File Offset: 0x000623FE
		// (set) Token: 0x06004FD6 RID: 20438 RVA: 0x00064206 File Offset: 0x00062406
		public bool TickRunning { get; set; }

		// Token: 0x06004FD7 RID: 20439 RVA: 0x0006420F File Offset: 0x0006240F
		private void Awake()
		{
			if (NearbyCosmeticsManager._instance != null && NearbyCosmeticsManager._instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			NearbyCosmeticsManager._instance = this;
		}

		// Token: 0x06004FD8 RID: 20440 RVA: 0x00033683 File Offset: 0x00031883
		private void OnEnable()
		{
			TickSystem<object>.AddCallbackTarget(this);
		}

		// Token: 0x06004FD9 RID: 20441 RVA: 0x0003368B File Offset: 0x0003188B
		private void OnDisable()
		{
			TickSystem<object>.RemoveCallbackTarget(this);
		}

		// Token: 0x06004FDA RID: 20442 RVA: 0x0006423D File Offset: 0x0006243D
		private void OnDestroy()
		{
			if (NearbyCosmeticsManager._instance == this)
			{
				NearbyCosmeticsManager._instance = null;
			}
		}

		// Token: 0x06004FDB RID: 20443 RVA: 0x00064252 File Offset: 0x00062452
		public void Register(NearbyCosmeticsEffect cosmetic)
		{
			this.cosmetics.Add(cosmetic);
		}

		// Token: 0x06004FDC RID: 20444 RVA: 0x00064260 File Offset: 0x00062460
		public void Unregister(NearbyCosmeticsEffect cosmetic)
		{
			this.cosmetics.Remove(cosmetic);
		}

		// Token: 0x06004FDD RID: 20445 RVA: 0x001B9DD0 File Offset: 0x001B7FD0
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

		// Token: 0x06004FDE RID: 20446 RVA: 0x001B9E5C File Offset: 0x001B805C
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

		// Token: 0x06004FDF RID: 20447 RVA: 0x001B9F54 File Offset: 0x001B8154
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

		// Token: 0x0400530F RID: 21263
		[SerializeField]
		private float proximityThreshold = 0.1f;

		// Token: 0x04005310 RID: 21264
		[SerializeField]
		private bool debug;

		// Token: 0x04005311 RID: 21265
		private List<NearbyCosmeticsEffect> cosmetics = new List<NearbyCosmeticsEffect>();

		// Token: 0x04005312 RID: 21266
		private static NearbyCosmeticsManager _instance;
	}
}
