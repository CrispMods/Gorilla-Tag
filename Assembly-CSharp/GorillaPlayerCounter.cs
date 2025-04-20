using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200051C RID: 1308
public class GorillaPlayerCounter : MonoBehaviour
{
	// Token: 0x06001FD0 RID: 8144 RVA: 0x000459E0 File Offset: 0x00043BE0
	private void Awake()
	{
		this.text = base.gameObject.GetComponent<Text>();
	}

	// Token: 0x06001FD1 RID: 8145 RVA: 0x000F03F0 File Offset: 0x000EE5F0
	private void Update()
	{
		if (PhotonNetwork.CurrentRoom != null)
		{
			int num = 0;
			foreach (KeyValuePair<int, Player> keyValuePair in PhotonNetwork.CurrentRoom.Players)
			{
				if ((bool)keyValuePair.Value.CustomProperties["isRedTeam"] == this.isRedTeam)
				{
					num++;
				}
			}
			this.text.text = num.ToString();
		}
	}

	// Token: 0x04002397 RID: 9111
	public bool isRedTeam;

	// Token: 0x04002398 RID: 9112
	public Text text;

	// Token: 0x04002399 RID: 9113
	public string attribute;
}
