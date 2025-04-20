using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x020004A7 RID: 1191
public class MonkeBallBallKillZone : MonoBehaviour
{
	// Token: 0x06001CE3 RID: 7395 RVA: 0x000DE6F4 File Offset: 0x000DC8F4
	private void OnTriggerEnter(Collider other)
	{
		GameBall component = other.transform.GetComponent<GameBall>();
		if (component != null)
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				MonkeBallGame.Instance.RequestResetBall(component.id, -1);
				return;
			}
			GameBallManager.Instance.RequestSetBallPosition(component.id);
		}
	}
}
