using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004A7 RID: 1191
public class MonkeBallTeamSelector : MonoBehaviour
{
	// Token: 0x06001CF0 RID: 7408 RVA: 0x0008D05A File Offset: 0x0008B25A
	public void Awake()
	{
		this._setTeamButton.onPressButton.AddListener(new UnityAction(this.OnSelect));
	}

	// Token: 0x06001CF1 RID: 7409 RVA: 0x0008D078 File Offset: 0x0008B278
	public void OnDestroy()
	{
		this._setTeamButton.onPressButton.RemoveListener(new UnityAction(this.OnSelect));
	}

	// Token: 0x06001CF2 RID: 7410 RVA: 0x0008D096 File Offset: 0x0008B296
	private void OnSelect()
	{
		MonkeBallGame.Instance.RequestSetTeam(this.teamId);
	}

	// Token: 0x04001FE1 RID: 8161
	public int teamId;

	// Token: 0x04001FE2 RID: 8162
	[SerializeField]
	private GorillaPressableButton _setTeamButton;
}
