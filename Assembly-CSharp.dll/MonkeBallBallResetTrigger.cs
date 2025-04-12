using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200049C RID: 1180
public class MonkeBallBallResetTrigger : MonoBehaviour
{
	// Token: 0x06001C94 RID: 7316 RVA: 0x000DBA94 File Offset: 0x000D9C94
	private void OnTriggerEnter(Collider other)
	{
		GameBall component = other.transform.GetComponent<GameBall>();
		if (component != null)
		{
			GamePlayer gamePlayer = (component.heldByActorNumber < 0) ? null : GamePlayer.GetGamePlayer(component.heldByActorNumber);
			if (gamePlayer == null)
			{
				gamePlayer = ((component.lastHeldByActorNumber < 0) ? null : GamePlayer.GetGamePlayer(component.lastHeldByActorNumber));
				if (gamePlayer == null)
				{
					return;
				}
			}
			this._lastBall = component;
			int num = gamePlayer.teamId;
			if (num == -1)
			{
				num = component.lastHeldByTeamId;
			}
			if (num >= 0 && num < this.teamMaterials.Length)
			{
				this.trigger.sharedMaterial = this.teamMaterials[num];
			}
			if (PhotonNetwork.IsMasterClient)
			{
				MonkeBallGame.Instance.ToggleResetButton(true, num);
			}
		}
	}

	// Token: 0x06001C95 RID: 7317 RVA: 0x000DBB4C File Offset: 0x000D9D4C
	private void OnTriggerExit(Collider other)
	{
		GameBall component = other.transform.GetComponent<GameBall>();
		if (component != null)
		{
			if (component == this._lastBall)
			{
				this.trigger.sharedMaterial = this.neutralMaterial;
				this._lastBall = null;
			}
			if (PhotonNetwork.IsMasterClient)
			{
				MonkeBallGame.Instance.ToggleResetButton(false, -1);
			}
		}
	}

	// Token: 0x04001F87 RID: 8071
	public Renderer trigger;

	// Token: 0x04001F88 RID: 8072
	public Material[] teamMaterials;

	// Token: 0x04001F89 RID: 8073
	public Material neutralMaterial;

	// Token: 0x04001F8A RID: 8074
	private GameBall _lastBall;
}
