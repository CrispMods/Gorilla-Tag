using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006E3 RID: 1763
public class ParticleCollisionListener : MonoBehaviour
{
	// Token: 0x06002BAE RID: 11182 RVA: 0x0004D8F6 File Offset: 0x0004BAF6
	private void Awake()
	{
		this._events = new List<ParticleCollisionEvent>();
	}

	// Token: 0x06002BAF RID: 11183 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnCollisionEvent(ParticleCollisionEvent ev)
	{
	}

	// Token: 0x06002BB0 RID: 11184 RVA: 0x00121290 File Offset: 0x0011F490
	public void OnParticleCollision(GameObject other)
	{
		int collisionEvents = this.target.GetCollisionEvents(other, this._events);
		for (int i = 0; i < collisionEvents; i++)
		{
			this.OnCollisionEvent(this._events[i]);
		}
	}

	// Token: 0x04003137 RID: 12599
	public ParticleSystem target;

	// Token: 0x04003138 RID: 12600
	[SerializeReference]
	private List<ParticleCollisionEvent> _events = new List<ParticleCollisionEvent>();
}
