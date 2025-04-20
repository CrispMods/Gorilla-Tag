using System;
using System.Linq;
using UnityEngine;

// Token: 0x020008C4 RID: 2244
public class Firework : MonoBehaviour
{
	// Token: 0x0600367D RID: 13949 RVA: 0x00053EBD File Offset: 0x000520BD
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

	// Token: 0x0600367E RID: 13950 RVA: 0x001447B0 File Offset: 0x001429B0
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

	// Token: 0x0600367F RID: 13951 RVA: 0x00053EE0 File Offset: 0x000520E0
	private void OnDrawGizmos()
	{
		if (!this._controller)
		{
			return;
		}
		this._controller.RenderGizmo(this, Color.cyan);
	}

	// Token: 0x06003680 RID: 13952 RVA: 0x00053F01 File Offset: 0x00052101
	private void OnDrawGizmosSelected()
	{
		if (!this._controller)
		{
			return;
		}
		this._controller.RenderGizmo(this, Color.yellow);
	}

	// Token: 0x04003896 RID: 14486
	[SerializeField]
	private FireworksController _controller;

	// Token: 0x04003897 RID: 14487
	[Space]
	public Transform origin;

	// Token: 0x04003898 RID: 14488
	public Transform target;

	// Token: 0x04003899 RID: 14489
	[Space]
	public Color colorOrigin = Color.cyan;

	// Token: 0x0400389A RID: 14490
	public Color colorTarget = Color.magenta;

	// Token: 0x0400389B RID: 14491
	[Space]
	public AudioSource sourceOrigin;

	// Token: 0x0400389C RID: 14492
	public AudioSource sourceTarget;

	// Token: 0x0400389D RID: 14493
	[Space]
	public ParticleSystem trail;

	// Token: 0x0400389E RID: 14494
	[Space]
	public ParticleSystem[] explosions;

	// Token: 0x0400389F RID: 14495
	[Space]
	public bool doTrail = true;

	// Token: 0x040038A0 RID: 14496
	public bool doTrailAudio = true;

	// Token: 0x040038A1 RID: 14497
	public bool doExplosion = true;
}
