using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006CF RID: 1743
public class ParticleCollisionListener : MonoBehaviour
{
	// Token: 0x06002B20 RID: 11040 RVA: 0x0004C5B1 File Offset: 0x0004A7B1
	private void Awake()
	{
		this._events = new List<ParticleCollisionEvent>();
	}

	// Token: 0x06002B21 RID: 11041 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void OnCollisionEvent(ParticleCollisionEvent ev)
	{
	}

	// Token: 0x06002B22 RID: 11042 RVA: 0x0011C6D8 File Offset: 0x0011A8D8
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
