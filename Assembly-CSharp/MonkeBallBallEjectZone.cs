using System;
using UnityEngine;

// Token: 0x020004A6 RID: 1190
public class MonkeBallBallEjectZone : MonoBehaviour
{
	// Token: 0x06001CE1 RID: 7393 RVA: 0x000DE69C File Offset: 0x000DC89C
	private void OnCollisionEnter(Collision collision)
	{
		GameBall component = collision.gameObject.GetComponent<GameBall>();
		if (component != null && collision.contacts.Length != 0)
		{
			component.SetVelocity(collision.contacts[0].impulse.normalized * this.ejectVelocity);
		}
	}

	// Token: 0x04001FD3 RID: 8147
	public Transform target;

	// Token: 0x04001FD4 RID: 8148
	public float ejectVelocity = 15f;
}
