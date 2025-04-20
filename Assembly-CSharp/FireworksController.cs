using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020008C6 RID: 2246
public class FireworksController : MonoBehaviour
{
	// Token: 0x06003685 RID: 13957 RVA: 0x00053F6A File Offset: 0x0005216A
	private void Awake()
	{
		this._launchOrder = this.fireworks.ToArray<Firework>();
		this._rnd = new SRand(this.seed);
	}

	// Token: 0x06003686 RID: 13958 RVA: 0x00144840 File Offset: 0x00142A40
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

	// Token: 0x06003687 RID: 13959 RVA: 0x001448A4 File Offset: 0x00142AA4
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

	// Token: 0x06003688 RID: 13960 RVA: 0x001448E8 File Offset: 0x00142AE8
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

	// Token: 0x06003689 RID: 13961 RVA: 0x00144B18 File Offset: 0x00142D18
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

	// Token: 0x0600368A RID: 13962 RVA: 0x00053F8E File Offset: 0x0005218E
	private void Update()
	{
		this.ProcessEvents();
	}

	// Token: 0x0600368B RID: 13963 RVA: 0x00144B5C File Offset: 0x00142D5C
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

	// Token: 0x0600368C RID: 13964 RVA: 0x00144BD0 File Offset: 0x00142DD0
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

	// Token: 0x0600368D RID: 13965 RVA: 0x00144C60 File Offset: 0x00142E60
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

	// Token: 0x040038A4 RID: 14500
	public Firework[] fireworks;

	// Token: 0x040038A5 RID: 14501
	public AudioClip[] whistles;

	// Token: 0x040038A6 RID: 14502
	public AudioClip[] bursts;

	// Token: 0x040038A7 RID: 14503
	[Space]
	[Range(0f, 1f)]
	public float whistleVolumeMin = 0.1f;

	// Token: 0x040038A8 RID: 14504
	[Range(0f, 1f)]
	public float whistleVolumeMax = 0.15f;

	// Token: 0x040038A9 RID: 14505
	public float minWhistleDelay = 1f;

	// Token: 0x040038AA RID: 14506
	[Space]
	[NonSerialized]
	private AudioClip _lastWhistle;

	// Token: 0x040038AB RID: 14507
	[NonSerialized]
	private AudioClip _lastBurst;

	// Token: 0x040038AC RID: 14508
	[NonSerialized]
	private Firework[] _launchOrder;

	// Token: 0x040038AD RID: 14509
	[NonSerialized]
	private SRand _rnd;

	// Token: 0x040038AE RID: 14510
	[NonSerialized]
	private FireworksController.ExplosionEvent[] _explosionQueue = new FireworksController.ExplosionEvent[8];

	// Token: 0x040038AF RID: 14511
	[NonSerialized]
	private TimeSince _timeSinceLastWhistle = 10f;

	// Token: 0x040038B0 RID: 14512
	[Space]
	public string seed = "Fireworks.Summer23";

	// Token: 0x040038B1 RID: 14513
	[Space]
	public uint roundNumVolleys = 6U;

	// Token: 0x040038B2 RID: 14514
	public uint roundLength = 6U;

	// Token: 0x040038B3 RID: 14515
	[FormerlySerializedAs("_timeOfDayEvent")]
	[FormerlySerializedAs("_timeOfDay")]
	[Space]
	[SerializeField]
	private TimeEvent _fireworksEvent;

	// Token: 0x020008C7 RID: 2247
	[Serializable]
	public struct ExplosionEvent
	{
		// Token: 0x040038B4 RID: 14516
		public TimeSince timeSince;

		// Token: 0x040038B5 RID: 14517
		public double delay;

		// Token: 0x040038B6 RID: 14518
		public int explosionIndex;

		// Token: 0x040038B7 RID: 14519
		public int burstIndex;

		// Token: 0x040038B8 RID: 14520
		public bool active;

		// Token: 0x040038B9 RID: 14521
		public Firework firework;
	}
}
