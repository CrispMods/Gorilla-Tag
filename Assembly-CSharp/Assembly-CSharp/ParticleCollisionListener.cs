using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006CF RID: 1743
public class ParticleCollisionListener : MonoBehaviour
{
	// Token: 0x06002B20 RID: 11040 RVA: 0x000D5BDC File Offset: 0x000D3DDC
	private void Awake()
	{
		this._events = new List<ParticleCollisionEvent>();
	}

	// Token: 0x06002B21 RID: 11041 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnCollisionEvent(ParticleCollisionEvent ev)
	{
	}

	// Token: 0x06002B22 RID: 11042 RVA: 0x000D5BEC File Offset: 0x000D3DEC
	public void OnParticleCollision(GameObject other)
	{
		int collisionEvents = this.target.GetCollisionEvents(other, this._events);
		for (int i = 0; i < collisionEvents; i++)
		{
			this.OnCollisionEvent(this._events[i]);
		}
	}

	// Token: 0x040030A0 RID: 12448
	public ParticleSystem target;

	// Token: 0x040030A1 RID: 12449
	[SerializeReference]
	private List<ParticleCollisionEvent> _events = new List<ParticleCollisionEvent>();
}
