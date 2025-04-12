using System;
using System.Linq;
using UnityEngine;

// Token: 0x020008AB RID: 2219
public class Firework : MonoBehaviour
{
	// Token: 0x060035C1 RID: 13761 RVA: 0x000529A0 File Offset: 0x00050BA0
	private void Launch()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this._controller)
		{
			this._controller.Launch(this);
		}
	}

	// Token: 0x060035C2 RID: 13762 RVA: 0x0013F1F0 File Offset: 0x0013D3F0
	private void OnValidate()
	{
		if (!this._controller)
		{
			this._controller = base.GetComponentInParent<FireworksController>();
		}
		if (!this._controller)
		{
			return;
		}
		Firework[] array = this._controller.fireworks;
		if (array.Contains(this))
		{
			return;
		}
		array = (from x in array.Concat(new Firework[]
		{
			this
		})
		where x != null
		select x).ToArray<Firework>();
		this._controller.fireworks = array;
	}

	// Token: 0x060035C3 RID: 13763 RVA: 0x000529C3 File Offset: 0x00050BC3
	private void OnDrawGizmos()
	{
		if (!this._controller)
		{
			return;
		}
		this._controller.RenderGizmo(this, Color.cyan);
	}

	// Token: 0x060035C4 RID: 13764 RVA: 0x000529E4 File Offset: 0x00050BE4
	private void OnDrawGizmosSelected()
	{
		if (!this._controller)
		{
			return;
		}
		this._controller.RenderGizmo(this, Color.yellow);
	}

	// Token: 0x040037E7 RID: 14311
	[SerializeField]
	private FireworksController _controller;

	// Token: 0x040037E8 RID: 14312
	[Space]
	public Transform origin;

	// Token: 0x040037E9 RID: 14313
	public Transform target;

	// Token: 0x040037EA RID: 14314
	[Space]
	public Color colorOrigin = Color.cyan;

	// Token: 0x040037EB RID: 14315
	public Color colorTarget = Color.magenta;

	// Token: 0x040037EC RID: 14316
	[Space]
	public AudioSource sourceOrigin;

	// Token: 0x040037ED RID: 14317
	public AudioSource sourceTarget;

	// Token: 0x040037EE RID: 14318
	[Space]
	public ParticleSystem trail;

	// Token: 0x040037EF RID: 14319
	[Space]
	public ParticleSystem[] explosions;

	// Token: 0x040037F0 RID: 14320
	[Space]
	public bool doTrail = true;

	// Token: 0x040037F1 RID: 14321
	public bool doTrailAudio = true;

	// Token: 0x040037F2 RID: 14322
	public bool doExplosion = true;
}
