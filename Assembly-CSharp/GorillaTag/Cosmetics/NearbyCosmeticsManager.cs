using System;
using System.Collections.Generic;
using CjLib;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C49 RID: 3145
	public class NearbyCosmeticsManager : MonoBehaviour, ITickSystemTick
	{
		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06004E74 RID: 20084 RVA: 0x00181895 File Offset: 0x0017FA95
		public static NearbyCosmeticsManager Instance
		{
			get
			{
				return NearbyCosmeticsManager._instance;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06004E75 RID: 20085 RVA: 0x0018189C File Offset: 0x0017FA9C
		// (set) Token: 0x06004E76 RID: 20086 RVA: 0x001818A4 File Offset: 0x0017FAA4
		public bool TickRunning { get; set; }

		// Token: 0x06004E77 RID: 20087 RVA: 0x001818AD File Offset: 0x0017FAAD
		private void Awake()
		{
			if (NearbyCosmeticsManager._instance != null && NearbyCosmeticsManager._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			NearbyCosmeticsManager._instance = this;
		}

		// Token: 0x06004E78 RID: 20088 RVA: 0x00019B0B File Offset: 0x00017D0B
		private void OnEnable()
		{
			TickSystem<object>.AddCallbackTarget(this);
		}

		// Token: 0x06004E79 RID: 20089 RVA: 0x00019B13 File Offset: 0x00017D13
		private void OnDisable()
		{
			TickSystem<object>.RemoveCallbackTarget(this);
		}

		// Token: 0x06004E7A RID: 20090 RVA: 0x001818DB File Offset: 0x0017FADB
		private void OnDestroy()
		{
			if (NearbyCosmeticsManager._instance == this)
			{
				NearbyCosmeticsManager._instance = null;
			}
		}

		// Token: 0x06004E7B RID: 20091 RVA: 0x001818F0 File Offset: 0x0017FAF0
		public void Register(NearbyCosmeticsEffect cosmetic)
		{
			this.cosmetics.Add(cosmetic);
		}

		// Token: 0x06004E7C RID: 20092 RVA: 0x001818FE File Offset: 0x0017FAFE
		public void Unregister(NearbyCosmeticsEffect cosmetic)
		{
			this.cosmetics.Remove(cosmetic);
		}

		// Token: 0x06004E7D RID: 20093 RVA: 0x00181910 File Offset: 0x0017FB10
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

		// Token: 0x06004E7E RID: 20094 RVA: 0x0018199C File Offset: 0x0017FB9C
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

		// Token: 0x06004E7F RID: 20095 RVA: 0x00181A94 File Offset: 0x0017FC94
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

		// Token: 0x04005203 RID: 20995
		[SerializeField]
		private float proximityThreshold = 0.1f;

		// Token: 0x04005204 RID: 20996
		[SerializeField]
		private bool debug;

		// Token: 0x04005205 RID: 20997
		private List<NearbyCosmeticsEffect> cosmetics = new List<NearbyCosmeticsEffect>();

		// Token: 0x04005206 RID: 20998
		private static NearbyCosmeticsManager _instance;
	}
}
