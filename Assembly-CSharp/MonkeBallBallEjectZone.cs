using System;
using UnityEngine;

// Token: 0x0200049A RID: 1178
public class MonkeBallBallEjectZone : MonoBehaviour
{
	// Token: 0x06001C8D RID: 7309 RVA: 0x0008AFC8 File Offset: 0x000891C8
	private void OnCollisionEnter(Collision collision)
	{
		GameBall component = collision.gameObject.GetComponent<GameBall>();
		if (component != null && collision.contacts.Length != 0)
		{
			component.SetVelocity(collision.contacts[0].impulse.normalized * this.ejectVelocity);
		}
	}

	// Token: 0x04001F84 RID: 8068
	public Transform target;

	// Token: 0x04001F85 RID: 8069
	public float ejectVelocity = 15f;
}
