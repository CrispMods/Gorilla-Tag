using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000510 RID: 1296
public class GorillaScoreCounter : MonoBehaviour
{
	// Token: 0x06001F7A RID: 8058 RVA: 0x0009E8D8 File Offset: 0x0009CAD8
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

	// Token: 0x06001F7B RID: 8059 RVA: 0x0009E90C File Offset: 0x0009CB0C
	private void Update()
	{
		if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.CustomProperties[this.attribute] != null)
		{
			this.text.text = ((int)PhotonNetwork.CurrentRoom.CustomProperties[this.attribute]).ToString();
		}
	}

	// Token: 0x04002347 RID: 9031
	public bool isRedTeam;

	// Token: 0x04002348 RID: 9032
	public Text text;

	// Token: 0x04002349 RID: 9033
	public string attribute;
}
