using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200051D RID: 1309
public class GorillaScoreCounter : MonoBehaviour
{
	// Token: 0x06001FD3 RID: 8147 RVA: 0x000459F3 File Offset: 0x00043BF3
	private void Awake()
	{
		this.text = base.gameObject.GetComponent<Text>();
		if (this.isRedTeam)
		{
			this.attribute = "redScore";
			return;
		}
		this.attribute = "blueScore";
	}

	// Token: 0x06001FD4 RID: 8148 RVA: 0x000F0484 File Offset: 0x000EE684
	private void Update()
	{
		if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.CustomProperties[this.attribute] != null)
		{
			this.text.text = ((int)PhotonNetwork.CurrentRoom.CustomProperties[this.attribute]).ToString();
		}
	}

	// Token: 0x0400239A RID: 9114
	public bool isRedTeam;

	// Token: 0x0400239B RID: 9115
	public Text text;

	// Token: 0x0400239C RID: 9116
	public string attribute;
}
