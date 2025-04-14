using System;
using UnityEngine;

// Token: 0x0200049A RID: 1178
public class MonkeBallBallEjectZone : MonoBehaviour
{
	// Token: 0x06001C90 RID: 7312 RVA: 0x0008B34C File Offset: 0x0008954C
	private void OnCollisionEnter(Collision collision)
	{
		GameBall component = collision.gameObject.GetComponent<GameBall>();
		if (component != null && collision.contacts.Length != 0)
		{
			component.SetVelocity(collision.contacts[0].impulse.normalized * this.ejectVelocity);
		}
	}

	// Token: 0x04001F85 RID: 8069
	public Transform target;

	// Token: 0x04001F86 RID: 8070
	public float ejectVelocity = 15f;
}
