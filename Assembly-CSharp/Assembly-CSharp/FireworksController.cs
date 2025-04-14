using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020008AD RID: 2221
public class FireworksController : MonoBehaviour
{
	// Token: 0x060035C9 RID: 13769 RVA: 0x000FF026 File Offset: 0x000FD226
	private void Awake()
	{
		this._launchOrder = this.fireworks.ToArray<Firework>();
		this._rnd = new SRand(this.seed);
	}

	// Token: 0x060035CA RID: 13770 RVA: 0x000FF04C File Offset: 0x000FD24C
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

	// Token: 0x060035CB RID: 13771 RVA: 0x000FF0B0 File Offset: 0x000FD2B0
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

	// Token: 0x060035CC RID: 13772 RVA: 0x000FF0F4 File Offset: 0x000FD2F4
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

	// Token: 0x060035CD RID: 13773 RVA: 0x000FF324 File Offset: 0x000FD524
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

	// Token: 0x060035CE RID: 13774 RVA: 0x000FF365 File Offset: 0x000FD565
	private void Update()
	{
		this.ProcessEvents();
	}

	// Token: 0x060035CF RID: 13775 RVA: 0x000FF370 File Offset: 0x000FD570
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

	// Token: 0x060035D0 RID: 13776 RVA: 0x000FF3E4 File Offset: 0x000FD5E4
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

	// Token: 0x060035D1 RID: 13777 RVA: 0x000FF474 File Offset: 0x000FD674
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

	// Token: 0x040037F5 RID: 14325
	public Firework[] fireworks;

	// Token: 0x040037F6 RID: 14326
	public AudioClip[] whistles;

	// Token: 0x040037F7 RID: 14327
	public AudioClip[] bursts;

	// Token: 0x040037F8 RID: 14328
	[Space]
	[Range(0f, 1f)]
	public float whistleVolumeMin = 0.1f;

	// Token: 0x040037F9 RID: 14329
	[Range(0f, 1f)]
	public float whistleVolumeMax = 0.15f;

	// Token: 0x040037FA RID: 14330
	public float minWhistleDelay = 1f;

	// Token: 0x040037FB RID: 14331
	[Space]
	[NonSerialized]
	private AudioClip _lastWhistle;

	// Token: 0x040037FC RID: 14332
	[NonSerialized]
	private AudioClip _lastBurst;

	// Token: 0x040037FD RID: 14333
	[NonSerialized]
	private Firework[] _launchOrder;

	// Token: 0x040037FE RID: 14334
	[NonSerialized]
	private SRand _rnd;

	// Token: 0x040037FF RID: 14335
	[NonSerialized]
	private FireworksController.ExplosionEvent[] _explosionQueue = new FireworksController.ExplosionEvent[8];

	// Token: 0x04003800 RID: 14336
	[NonSerialized]
	private TimeSince _timeSinceLastWhistle = 10f;

	// Token: 0x04003801 RID: 14337
	[Space]
	public string seed = "Fireworks.Summer23";

	// Token: 0x04003802 RID: 14338
	[Space]
	public uint roundNumVolleys = 6U;

	// Token: 0x04003803 RID: 14339
	public uint roundLength = 6U;

	// Token: 0x04003804 RID: 14340
	[FormerlySerializedAs("_timeOfDayEvent")]
	[FormerlySerializedAs("_timeOfDay")]
	[Space]
	[SerializeField]
	private TimeEvent _fireworksEvent;

	// Token: 0x020008AE RID: 2222
	[Serializable]
	public struct ExplosionEvent
	{
		// Token: 0x04003805 RID: 14341
		public TimeSince timeSince;

		// Token: 0x04003806 RID: 14342
		public double delay;

		// Token: 0x04003807 RID: 14343
		public int explosionIndex;

		// Token: 0x04003808 RID: 14344
		public int burstIndex;

		// Token: 0x04003809 RID: 14345
		public bool active;

		// Token: 0x0400380A RID: 14346
		public Firework firework;
	}
}
