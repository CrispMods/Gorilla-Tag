using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200049B RID: 1179
public class MonkeBallBallKillZone : MonoBehaviour
{
	// Token: 0x06001C92 RID: 7314 RVA: 0x0008B3B4 File Offset: 0x000895B4
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
