using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200049C RID: 1180
public class MonkeBallBallResetTrigger : MonoBehaviour
{
	// Token: 0x06001C91 RID: 7313 RVA: 0x0008B080 File Offset: 0x00089280
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

	// Token: 0x06001C92 RID: 7314 RVA: 0x0008B138 File Offset: 0x00089338
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

	// Token: 0x04001F86 RID: 8070
	public Renderer trigger;

	// Token: 0x04001F87 RID: 8071
	public Material[] teamMaterials;

	// Token: 0x04001F88 RID: 8072
	public Material neutralMaterial;

	// Token: 0x04001F89 RID: 8073
	private GameBall _lastBall;
}
