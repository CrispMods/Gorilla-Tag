using System;
using GorillaTag.CosmeticSystem;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C79 RID: 3193
	public class NearbyCosmeticsEffect : MonoBehaviour, ISpawnable, ITickSystemTick
	{
		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06004FC2 RID: 20418 RVA: 0x000640ED File Offset: 0x000622ED
		// (set) Token: 0x06004FC3 RID: 20419 RVA: 0x000640F5 File Offset: 0x000622F5
		public bool IsMatched { get; set; }

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06004FC4 RID: 20420 RVA: 0x000640FE File Offset: 0x000622FE
		// (set) Token: 0x06004FC5 RID: 20421 RVA: 0x00064106 File Offset: 0x00062306
		public VRRig MyRig { get; private set; }

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06004FC6 RID: 20422 RVA: 0x0006410F File Offset: 0x0006230F
		// (set) Token: 0x06004FC7 RID: 20423 RVA: 0x00064117 File Offset: 0x00062317
		public bool IsSpawned { get; set; }

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06004FC8 RID: 20424 RVA: 0x00064120 File Offset: 0x00062320
		// (set) Token: 0x06004FC9 RID: 20425 RVA: 0x00064128 File Offset: 0x00062328
		public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06004FCA RID: 20426 RVA: 0x00064131 File Offset: 0x00062331
		// (set) Token: 0x06004FCB RID: 20427 RVA: 0x00064139 File Offset: 0x00062339
		public bool TickRunning { get; set; }

		// Token: 0x06004FCC RID: 20428 RVA: 0x00064142 File Offset: 0x00062342
		public void OnSpawn(VRRig rig)
		{
			this.MyRig = rig;
		}

		// Token: 0x06004FCD RID: 20429 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnDespawn()
		{
		}

		// Token: 0x06004FCE RID: 20430 RVA: 0x0006414B File Offset: 0x0006234B
		private void OnEnable()
		{
			TickSystem<object>.AddCallbackTarget(this);
			this.canPlayEffects = true;
			this.IsMatched = false;
			NearbyCosmeticsManager.Instance.Register(this);
		}

		// Token: 0x06004FCF RID: 20431 RVA: 0x0006416C File Offset: 0x0006236C
		public void Tick()
		{
			if (!this.canPlayEffects && Time.time - this.timer >= this.cooldownTime)
			{
				this.canPlayEffects = true;
			}
		}

		// Token: 0x06004FD0 RID: 20432 RVA: 0x00064191 File Offset: 0x00062391
		private void OnDisable()
		{
			TickSystem<object>.RemoveCallbackTarget(this);
			if (NearbyCosmeticsManager.Instance)
			{
				NearbyCosmeticsManager.Instance.Unregister(this);
			}
		}

		// Token: 0x06004FD1 RID: 20433 RVA: 0x001B9D6C File Offset: 0x001B7F6C
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

		// Token: 0x06004FD2 RID: 20434 RVA: 0x000641B0 File Offset: 0x000623B0
		private void PlayEffectLocal()
		{
			GorillaTagger.Instance.StartVibration(this.leftHand, this.hapticStrength, this.hapticDuration);
		}

		// Token: 0x040052FF RID: 21247
		[SerializeField]
		private bool leftHand;

		// Token: 0x04005300 RID: 21248
		public string cosmeticType;

		// Token: 0x04005301 RID: 21249
		[SerializeField]
		private ParticleSystem particlesFX;

		// Token: 0x04005302 RID: 21250
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04005303 RID: 21251
		[SerializeField]
		private float hapticStrength = 0.5f;

		// Token: 0x04005304 RID: 21252
		[SerializeField]
		private float hapticDuration = 0.1f;

		// Token: 0x04005305 RID: 21253
		[SerializeField]
		private float cooldownTime = 0.5f;

		// Token: 0x04005306 RID: 21254
		public Transform cosmeticCenter;

		// Token: 0x04005307 RID: 21255
		private float timer;

		// Token: 0x04005308 RID: 21256
		private bool canPlayEffects;

		// Token: 0x0400530B RID: 21259
		private RubberDuckEvents _events;
	}
}
