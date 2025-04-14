using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200050F RID: 1295
public class GorillaPlayerCounter : MonoBehaviour
{
	// Token: 0x06001F77 RID: 8055 RVA: 0x0009E830 File Offset: 0x0009CA30
	private void Awake()
	{
		this.text = base.gameObject.GetComponent<Text>();
	}

	// Token: 0x06001F78 RID: 8056 RVA: 0x0009E844 File Offset: 0x0009CA44
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

	// Token: 0x04002344 RID: 9028
	public bool isRedTeam;

	// Token: 0x04002345 RID: 9029
	public Text text;

	// Token: 0x04002346 RID: 9030
	public string attribute;
}
