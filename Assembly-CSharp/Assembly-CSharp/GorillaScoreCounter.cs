using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000510 RID: 1296
public class GorillaScoreCounter : MonoBehaviour
{
	// Token: 0x06001F7D RID: 8061 RVA: 0x0009EC5C File Offset: 0x0009CE5C
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

	// Token: 0x06001F7E RID: 8062 RVA: 0x0009EC90 File Offset: 0x0009CE90
	private void Update()
	{
		if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.CustomProperties[this.attribute] != null)
		{
			this.text.text = ((int)PhotonNetwork.CurrentRoom.CustomProperties[this.attribute]).ToString();
		}
	}

	// Token: 0x04002348 RID: 9032
	public bool isRedTeam;

	// Token: 0x04002349 RID: 9033
	public Text text;

	// Token: 0x0400234A RID: 9034
	public string attribute;
}
