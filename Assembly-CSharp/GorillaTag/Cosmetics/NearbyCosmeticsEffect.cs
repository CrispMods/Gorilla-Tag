using System;
using GorillaTag.CosmeticSystem;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C48 RID: 3144
	public class NearbyCosmeticsEffect : MonoBehaviour, ISpawnable, ITickSystemTick
	{
		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06004E62 RID: 20066 RVA: 0x00181729 File Offset: 0x0017F929
		// (set) Token: 0x06004E63 RID: 20067 RVA: 0x00181731 File Offset: 0x0017F931
		public bool IsMatched { get; set; }

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06004E64 RID: 20068 RVA: 0x0018173A File Offset: 0x0017F93A
		// (set) Token: 0x06004E65 RID: 20069 RVA: 0x00181742 File Offset: 0x0017F942
		public VRRig MyRig { get; private set; }

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06004E66 RID: 20070 RVA: 0x0018174B File Offset: 0x0017F94B
		// (set) Token: 0x06004E67 RID: 20071 RVA: 0x00181753 File Offset: 0x0017F953
		public bool IsSpawned { get; set; }

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06004E68 RID: 20072 RVA: 0x0018175C File Offset: 0x0017F95C
		// (set) Token: 0x06004E69 RID: 20073 RVA: 0x00181764 File Offset: 0x0017F964
		public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06004E6A RID: 20074 RVA: 0x0018176D File Offset: 0x0017F96D
		// (set) Token: 0x06004E6B RID: 20075 RVA: 0x00181775 File Offset: 0x0017F975
		public bool TickRunning { get; set; }

		// Token: 0x06004E6C RID: 20076 RVA: 0x0018177E File Offset: 0x0017F97E
		public void OnSpawn(VRRig rig)
		{
			this.MyRig = rig;
		}

		// Token: 0x06004E6D RID: 20077 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnDespawn()
		{
		}

		// Token: 0x06004E6E RID: 20078 RVA: 0x00181787 File Offset: 0x0017F987
		private void OnEnable()
		{
			TickSystem<object>.AddCallbackTarget(this);
			this.canPlayEffects = true;
			this.IsMatched = false;
			NearbyCosmeticsManager.Instance.Register(this);
		}

		// Token: 0x06004E6F RID: 20079 RVA: 0x001817A8 File Offset: 0x0017F9A8
		public void Tick()
		{
			if (!this.canPlayEffects && Time.time - this.timer >= this.cooldownTime)
			{
				this.canPlayEffects = true;
			}
		}

		// Token: 0x06004E70 RID: 20080 RVA: 0x001817CD File Offset: 0x0017F9CD
		private void OnDisable()
		{
			TickSystem<object>.RemoveCallbackTarget(this);
			if (NearbyCosmeticsManager.Instance)
			{
				NearbyCosmeticsManager.Instance.Unregister(this);
			}
		}

		// Token: 0x06004E71 RID: 20081 RVA: 0x001817EC File Offset: 0x0017F9EC
		public void PlayEffects(bool playAudio = false)
		{
			if (!this.canPlayEffects)
			{
				return;
			}
			this.timer = Time.time;
			if (this.particlesFX != null)
			{
				this.particlesFX.Play();
			}
			if (playAudio)
			{
				this.audioSource.GTPlay();
			}
			if (this.MyRig.isLocal)
			{
				this.PlayEffectLocal();
			}
			this.canPlayEffects = false;
		}

		// Token: 0x06004E72 RID: 20082 RVA: 0x0018184E File Offset: 0x0017FA4E
		private void PlayEffectLocal()
		{
			GorillaTagger.Instance.StartVibration(this.leftHand, this.hapticStrength, this.hapticDuration);
		}

		// Token: 0x040051F3 RID: 20979
		[SerializeField]
		private bool leftHand;

		// Token: 0x040051F4 RID: 20980
		public string cosmeticType;

		// Token: 0x040051F5 RID: 20981
		[SerializeField]
		private ParticleSystem particlesFX;

		// Token: 0x040051F6 RID: 20982
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x040051F7 RID: 20983
		[SerializeField]
		private float hapticStrength = 0.5f;

		// Token: 0x040051F8 RID: 20984
		[SerializeField]
		private float hapticDuration = 0.1f;

		// Token: 0x040051F9 RID: 20985
		[SerializeField]
		private float cooldownTime = 0.5f;

		// Token: 0x040051FA RID: 20986
		public Transform cosmeticCenter;

		// Token: 0x040051FB RID: 20987
		private float timer;

		// Token: 0x040051FC RID: 20988
		private bool canPlayEffects;

		// Token: 0x040051FF RID: 20991
		private RubberDuckEvents _events;
	}
}
