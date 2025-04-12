using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004A7 RID: 1191
public class MonkeBallTeamSelector : MonoBehaviour
{
	// Token: 0x06001CF3 RID: 7411 RVA: 0x00042D5A File Offset: 0x00040F5A
	public void Awake()
	{
		this._setTeamButton.onPressButton.AddListener(new UnityAction(this.OnSelect));
	}

	// Token: 0x06001CF4 RID: 7412 RVA: 0x00042D78 File Offset: 0x00040F78
	public void OnDestroy()
	{
		this._setTeamButton.onPressButton.RemoveListener(new UnityAction(this.OnSelect));
	}

	// Token: 0x06001CF5 RID: 7413 RVA: 0x00042D96 File Offset: 0x00040F96
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
