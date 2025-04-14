using System;
using System.Linq;
using UnityEngine;

// Token: 0x020008A8 RID: 2216
public class Firework : MonoBehaviour
{
	// Token: 0x060035B5 RID: 13749 RVA: 0x000FE91E File Offset: 0x000FCB1E
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

	// Token: 0x060035B6 RID: 13750 RVA: 0x000FE944 File Offset: 0x000FCB44
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

	// Token: 0x060035B7 RID: 13751 RVA: 0x000FE9D4 File Offset: 0x000FCBD4
	private void OnDrawGizmos()
	{
		if (!this._controller)
		{
			return;
		}
		this._controller.RenderGizmo(this, Color.cyan);
	}

	// Token: 0x060035B8 RID: 13752 RVA: 0x000FE9F5 File Offset: 0x000FCBF5
	private void OnDrawGizmosSelected()
	{
		if (!this._controller)
		{
			return;
		}
		this._controller.RenderGizmo(this, Color.yellow);
	}

	// Token: 0x040037D5 RID: 14293
	[SerializeField]
	private FireworksController _controller;

	// Token: 0x040037D6 RID: 14294
	[Space]
	public Transform origin;

	// Token: 0x040037D7 RID: 14295
	public Transform target;

	// Token: 0x040037D8 RID: 14296
	[Space]
	public Color colorOrigin = Color.cyan;

	// Token: 0x040037D9 RID: 14297
	public Color colorTarget = Color.magenta;

	// Token: 0x040037DA RID: 14298
	[Space]
	public AudioSource sourceOrigin;

	// Token: 0x040037DB RID: 14299
	public AudioSource sourceTarget;

	// Token: 0x040037DC RID: 14300
	[Space]
	public ParticleSystem trail;

	// Token: 0x040037DD RID: 14301
	[Space]
	public ParticleSystem[] explosions;

	// Token: 0x040037DE RID: 14302
	[Space]
	public bool doTrail = true;

	// Token: 0x040037DF RID: 14303
	public bool doTrailAudio = true;

	// Token: 0x040037E0 RID: 14304
	public bool doExplosion = true;
}
