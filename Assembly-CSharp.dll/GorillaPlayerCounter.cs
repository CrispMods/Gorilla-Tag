using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200050F RID: 1295
public class GorillaPlayerCounter : MonoBehaviour
{
	// Token: 0x06001F7A RID: 8058 RVA: 0x00044641 File Offset: 0x00042841
	private void Awake()
	{
		this.text = base.gameObject.GetComponent<Text>();
	}

	// Token: 0x06001F7B RID: 8059 RVA: 0x000ED66C File Offset: 0x000EB86C
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

	// Token: 0x04002345 RID: 9029
	public bool isRedTeam;

	// Token: 0x04002346 RID: 9030
	public Text text;

	// Token: 0x04002347 RID: 9031
	public string attribute;
}
