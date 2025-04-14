using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006CE RID: 1742
public class ParticleCollisionListener : MonoBehaviour
{
	// Token: 0x06002B18 RID: 11032 RVA: 0x000D575C File Offset: 0x000D395C
	private void Awake()
	{
		this._events = new List<ParticleCollisionEvent>();
	}

	// Token: 0x06002B19 RID: 11033 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnCollisionEvent(ParticleCollisionEvent ev)
	{
	}

	// Token: 0x06002B1A RID: 11034 RVA: 0x000D576C File Offset: 0x000D396C
	public void OnParticleCollision(GameObject other)
	{
		int collisionEvents = this.target.GetCollisionEvents(other, this._events);
		for (int i = 0; i < collisionEvents; i++)
		{
			this.OnCollisionEvent(this._events[i]);
		}
	}

	// Token: 0x0400309A RID: 12442
	public ParticleSystem target;

	// Token: 0x0400309B RID: 12443
	[SerializeReference]
	private List<ParticleCollisionEvent> _events = new List<ParticleCollisionEvent>();
}
