using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x020004A8 RID: 1192
public class MonkeBallBallResetTrigger : MonoBehaviour
{
	// Token: 0x06001CE5 RID: 7397 RVA: 0x000DE744 File Offset: 0x000DC944
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

	// Token: 0x06001CE6 RID: 7398 RVA: 0x000DE7FC File Offset: 0x000DC9FC
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

	// Token: 0x04001FD5 RID: 8149
	public Renderer trigger;

	// Token: 0x04001FD6 RID: 8150
	public Material[] teamMaterials;

	// Token: 0x04001FD7 RID: 8151
	public Material neutralMaterial;

	// Token: 0x04001FD8 RID: 8152
	private GameBall _lastBall;
}
