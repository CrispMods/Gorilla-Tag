using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004B3 RID: 1203
public class MonkeBallTeamSelector : MonoBehaviour
{
	// Token: 0x06001D44 RID: 7492 RVA: 0x00044093 File Offset: 0x00042293
	public void Awake()
	{
		this._setTeamButton.onPressButton.AddListener(new UnityAction(this.OnSelect));
	}

	// Token: 0x06001D45 RID: 7493 RVA: 0x000440B1 File Offset: 0x000422B1
	public void OnDestroy()
	{
		this._setTeamButton.onPressButton.RemoveListener(new UnityAction(this.OnSelect));
	}

	// Token: 0x06001D46 RID: 7494 RVA: 0x000440CF File Offset: 0x000422CF
	private void OnSelect()
	{
		MonkeBallGame.Instance.RequestSetTeam(this.teamId);
	}

	// Token: 0x04002030 RID: 8240
	public int teamId;

	// Token: 0x04002031 RID: 8241
	[SerializeField]
	private GorillaPressableButton _setTeamButton;
}
