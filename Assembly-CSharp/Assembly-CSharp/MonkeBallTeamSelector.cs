using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004A7 RID: 1191
public class MonkeBallTeamSelector : MonoBehaviour
{
	// Token: 0x06001CF3 RID: 7411 RVA: 0x0008D3DE File Offset: 0x0008B5DE
	public void Awake()
	{
		this._setTeamButton.onPressButton.AddListener(new UnityAction(this.OnSelect));
	}

	// Token: 0x06001CF4 RID: 7412 RVA: 0x0008D3FC File Offset: 0x0008B5FC
	public void OnDestroy()
	{
		this._setTeamButton.onPressButton.RemoveListener(new UnityAction(this.OnSelect));
	}

	// Token: 0x06001CF5 RID: 7413 RVA: 0x0008D41A File Offset: 0x0008B61A
	private void OnSelect()
	{
		MonkeBallGame.Instance.RequestSetTeam(this.teamId);
	}

	// Token: 0x04001FE2 RID: 8162
	public int teamId;

	// Token: 0x04001FE3 RID: 8163
	[SerializeField]
	private GorillaPressableButton _setTeamButton;
}
