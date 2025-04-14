using System;
using GorillaTag.CosmeticSystem;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C4B RID: 3147
	public class NearbyCosmeticsEffect : MonoBehaviour, ISpawnable, ITickSystemTick
	{
		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06004E6E RID: 20078 RVA: 0x00181CF1 File Offset: 0x0017FEF1
		// (set) Token: 0x06004E6F RID: 20079 RVA: 0x00181CF9 File Offset: 0x0017FEF9
		public bool IsMatched { get; set; }

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06004E70 RID: 20080 RVA: 0x00181D02 File Offset: 0x0017FF02
		// (set) Token: 0x06004E71 RID: 20081 RVA: 0x00181D0A File Offset: 0x0017FF0A
		public VRRig MyRig { get; private set; }

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06004E72 RID: 20082 RVA: 0x00181D13 File Offset: 0x0017FF13
		// (set) Token: 0x06004E73 RID: 20083 RVA: 0x00181D1B File Offset: 0x0017FF1B
		public bool IsSpawned { get; set; }

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06004E74 RID: 20084 RVA: 0x00181D24 File Offset: 0x0017FF24
		// (set) Token: 0x06004E75 RID: 20085 RVA: 0x00181D2C File Offset: 0x0017FF2C
		public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06004E76 RID: 20086 RVA: 0x00181D35 File Offset: 0x0017FF35
		// (set) Token: 0x06004E77 RID: 20087 RVA: 0x00181D3D File Offset: 0x0017FF3D
		public bool TickRunning { get; set; }

		// Token: 0x06004E78 RID: 20088 RVA: 0x00181D46 File Offset: 0x0017FF46
		public void OnSpawn(VRRig rig)
		{
			this.MyRig = rig;
		}

		// Token: 0x06004E79 RID: 20089 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnDespawn()
		{
		}

		// Token: 0x06004E7A RID: 20090 RVA: 0x00181D4F File Offset: 0x0017FF4F
		private void OnEnable()
		{
			TickSystem<object>.AddCallbackTarget(this);
			this.canPlayEffects = true;
			this.IsMatched = false;
			NearbyCosmeticsManager.Instance.Register(this);
		}

		// Token: 0x06004E7B RID: 20091 RVA: 0x00181D70 File Offset: 0x0017FF70
		public void Tick()
		{
			if (!this.canPlayEffects && Time.time - this.timer >= this.cooldownTime)
			{
				this.canPlayEffects = true;
			}
		}

		// Token: 0x06004E7C RID: 20092 RVA: 0x00181D95 File Offset: 0x0017FF95
		private void OnDisable()
		{
			TickSystem<object>.RemoveCallbackTarget(this);
			if (NearbyCosmeticsManager.Instance)
			{
				NearbyCosmeticsManager.Instance.Unregister(this);
			}
		}

		// Token: 0x06004E7D RID: 20093 RVA: 0x00181DB4 File Offset: 0x0017FFB4
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

		// Token: 0x06004E7E RID: 20094 RVA: 0x00181E16 File Offset: 0x00180016
		private void PlayEffectLocal()
		{
			GorillaTagger.Instance.StartVibration(this.leftHand, this.hapticStrength, this.hapticDuration);
		}

		// Token: 0x04005205 RID: 20997
		[SerializeField]
		private bool leftHand;

		// Token: 0x04005206 RID: 20998
		public string cosmeticType;

		// Token: 0x04005207 RID: 20999
		[SerializeField]
		private ParticleSystem particlesFX;

		// Token: 0x04005208 RID: 21000
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04005209 RID: 21001
		[SerializeField]
		private float hapticStrength = 0.5f;

		// Token: 0x0400520A RID: 21002
		[SerializeField]
		private float hapticDuration = 0.1f;

		// Token: 0x0400520B RID: 21003
		[SerializeField]
		private float cooldownTime = 0.5f;

		// Token: 0x0400520C RID: 21004
		public Transform cosmeticCenter;

		// Token: 0x0400520D RID: 21005
		private float timer;

		// Token: 0x0400520E RID: 21006
		private bool canPlayEffects;

		// Token: 0x04005211 RID: 21009
		private RubberDuckEvents _events;
	}
}
