using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020008AA RID: 2218
public class FireworksController : MonoBehaviour
{
	// Token: 0x060035BD RID: 13757 RVA: 0x000FEA5E File Offset: 0x000FCC5E
	private void Awake()
	{
		this._launchOrder = this.fireworks.ToArray<Firework>();
		this._rnd = new SRand(this.seed);
	}

	// Token: 0x060035BE RID: 13758 RVA: 0x000FEA84 File Offset: 0x000FCC84
	public void LaunchVolley()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		this._rnd.Shuffle<Firework>(this._launchOrder);
		for (int i = 0; i < this._launchOrder.Length; i++)
		{
			MonoBehaviour monoBehaviour = this._launchOrder[i];
			float time = this._rnd.NextFloat() * this.roundLength;
			monoBehaviour.Invoke("Launch", time);
		}
	}

	// Token: 0x060035BF RID: 13759 RVA: 0x000FEAE8 File Offset: 0x000FCCE8
	public void LaunchVolleyRound()
	{
		int num = 0;
		while ((long)num < (long)((ulong)this.roundNumVolleys))
		{
			float time = this._rnd.NextFloat() * this.roundLength;
			base.Invoke("LaunchVolley", time);
			num++;
		}
	}

	// Token: 0x060035C0 RID: 13760 RVA: 0x000FEB2C File Offset: 0x000FCD2C
	public void Launch(Firework fw)
	{
		if (!fw)
		{
			return;
		}
		Vector3 position = fw.origin.position;
		Vector3 position2 = fw.target.position;
		AudioSource sourceOrigin = fw.sourceOrigin;
		int num = this._rnd.NextInt(this.bursts.Length);
		AudioClip audioClip = this.whistles[this._rnd.NextInt(this.whistles.Length)];
		AudioClip audioClip2 = this.bursts[num];
		while (this._lastWhistle == audioClip)
		{
			audioClip = this.whistles[this._rnd.NextInt(this.whistles.Length)];
		}
		while (this._lastBurst == audioClip2)
		{
			num = this._rnd.NextInt(this.bursts.Length);
			audioClip2 = this.bursts[num];
		}
		this._lastWhistle = audioClip;
		this._lastBurst = audioClip2;
		int num2 = this._rnd.NextInt(fw.explosions.Length);
		ParticleSystem particleSystem = fw.explosions[num2];
		if (fw.doTrail)
		{
			ParticleSystem trail = fw.trail;
			trail.startColor = fw.colorOrigin;
			trail.subEmitters.GetSubEmitterSystem(0).colorOverLifetime.color = new ParticleSystem.MinMaxGradient(fw.colorOrigin, fw.colorTarget);
			trail.Stop();
			trail.Play();
		}
		sourceOrigin.pitch = this._rnd.NextFloat(0.92f, 1f);
		fw.doTrailAudio = this._rnd.NextBool();
		FireworksController.ExplosionEvent ev = new FireworksController.ExplosionEvent
		{
			firework = fw,
			timeSince = TimeSince.Now(),
			burstIndex = num,
			explosionIndex = num2,
			delay = (double)(fw.doTrail ? audioClip.length : 0f),
			active = true
		};
		if (fw.doExplosion)
		{
			this.PostExplosionEvent(ev);
		}
		if (fw.doTrailAudio && this._timeSinceLastWhistle > this.minWhistleDelay)
		{
			this._timeSinceLastWhistle = TimeSince.Now();
			sourceOrigin.PlayOneShot(audioClip, this._rnd.NextFloat(this.whistleVolumeMin, this.whistleVolumeMax));
		}
		particleSystem.Stop();
		particleSystem.transform.position = position2;
	}

	// Token: 0x060035C1 RID: 13761 RVA: 0x000FED5C File Offset: 0x000FCF5C
	private void PostExplosionEvent(FireworksController.ExplosionEvent ev)
	{
		for (int i = 0; i < this._explosionQueue.Length; i++)
		{
			if (!this._explosionQueue[i].active)
			{
				this._explosionQueue[i] = ev;
				return;
			}
		}
	}

	// Token: 0x060035C2 RID: 13762 RVA: 0x000FED9D File Offset: 0x000FCF9D
	private void Update()
	{
		this.ProcessEvents();
	}

	// Token: 0x060035C3 RID: 13763 RVA: 0x000FEDA8 File Offset: 0x000FCFA8
	private void ProcessEvents()
	{
		if (this._explosionQueue == null || this._explosionQueue.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this._explosionQueue.Length; i++)
		{
			FireworksController.ExplosionEvent explosionEvent = this._explosionQueue[i];
			if (explosionEvent.active && explosionEvent.timeSince >= explosionEvent.delay)
			{
				this.DoExplosion(explosionEvent);
				this._explosionQueue[i] = default(FireworksController.ExplosionEvent);
			}
		}
	}

	// Token: 0x060035C4 RID: 13764 RVA: 0x000FEE1C File Offset: 0x000FD01C
	private void DoExplosion(FireworksController.ExplosionEvent ev)
	{
		Firework firework = ev.firework;
		ParticleSystem particleSystem = firework.explosions[ev.explosionIndex];
		ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient(firework.colorOrigin, firework.colorTarget);
		ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particleSystem.colorOverLifetime;
		ParticleSystem.ColorOverLifetimeModule colorOverLifetime2 = particleSystem.subEmitters.GetSubEmitterSystem(0).colorOverLifetime;
		colorOverLifetime.color = color;
		colorOverLifetime2.color = color;
		ParticleSystem particleSystem2 = firework.explosions[ev.explosionIndex];
		particleSystem2.Stop();
		particleSystem2.Play();
		firework.sourceTarget.PlayOneShot(this.bursts[ev.burstIndex]);
	}

	// Token: 0x060035C5 RID: 13765 RVA: 0x000FEEAC File Offset: 0x000FD0AC
	public void RenderGizmo(Firework fw, Color c)
	{
		if (!fw)
		{
			return;
		}
		if (!fw.origin || !fw.target)
		{
			return;
		}
		Gizmos.color = c;
		Vector3 position = fw.origin.position;
		Vector3 position2 = fw.target.position;
		Gizmos.DrawLine(position, position2);
		Gizmos.DrawWireCube(position, Vector3.one * 0.5f);
		Gizmos.DrawWireCube(position2, Vector3.one * 0.5f);
	}

	// Token: 0x040037E3 RID: 14307
	public Firework[] fireworks;

	// Token: 0x040037E4 RID: 14308
	public AudioClip[] whistles;

	// Token: 0x040037E5 RID: 14309
	public AudioClip[] bursts;

	// Token: 0x040037E6 RID: 14310
	[Space]
	[Range(0f, 1f)]
	public float whistleVolumeMin = 0.1f;

	// Token: 0x040037E7 RID: 14311
	[Range(0f, 1f)]
	public float whistleVolumeMax = 0.15f;

	// Token: 0x040037E8 RID: 14312
	public float minWhistleDelay = 1f;

	// Token: 0x040037E9 RID: 14313
	[Space]
	[NonSerialized]
	private AudioClip _lastWhistle;

	// Token: 0x040037EA RID: 14314
	[NonSerialized]
	private AudioClip _lastBurst;

	// Token: 0x040037EB RID: 14315
	[NonSerialized]
	private Firework[] _launchOrder;

	// Token: 0x040037EC RID: 14316
	[NonSerialized]
	private SRand _rnd;

	// Token: 0x040037ED RID: 14317
	[NonSerialized]
	private FireworksController.ExplosionEvent[] _explosionQueue = new FireworksController.ExplosionEvent[8];

	// Token: 0x040037EE RID: 14318
	[NonSerialized]
	private TimeSince _timeSinceLastWhistle = 10f;

	// Token: 0x040037EF RID: 14319
	[Space]
	public string seed = "Fireworks.Summer23";

	// Token: 0x040037F0 RID: 14320
	[Space]
	public uint roundNumVolleys = 6U;

	// Token: 0x040037F1 RID: 14321
	public uint roundLength = 6U;

	// Token: 0x040037F2 RID: 14322
	[FormerlySerializedAs("_timeOfDayEvent")]
	[FormerlySerializedAs("_timeOfDay")]
	[Space]
	[SerializeField]
	private TimeEvent _fireworksEvent;

	// Token: 0x020008AB RID: 2219
	[Serializable]
	public struct ExplosionEvent
	{
		// Token: 0x040037F3 RID: 14323
		public TimeSince timeSince;

		// Token: 0x040037F4 RID: 14324
		public double delay;

		// Token: 0x040037F5 RID: 14325
		public int explosionIndex;

		// Token: 0x040037F6 RID: 14326
		public int burstIndex;

		// Token: 0x040037F7 RID: 14327
		public bool active;

		// Token: 0x040037F8 RID: 14328
		public Firework firework;
	}
}
