using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000508 RID: 1288
public class CheatUpdate : MonoBehaviour
{
	// Token: 0x06001F42 RID: 8002 RVA: 0x0004518A File Offset: 0x0004338A
	private void Start()
	{
		base.StartCoroutine(this.UpdateNumberOfPlayers());
	}

	// Token: 0x06001F43 RID: 8003 RVA: 0x00045199 File Offset: 0x00043399
	public IEnumerator UpdateNumberOfPlayers()
	{
		for (;;)
		{
			base.StartCoroutine(this.UpdatePlayerCount());
			yield return new WaitForSeconds(10f);
		}
		yield break;
	}

	// Token: 0x06001F44 RID: 8004 RVA: 0x000451A8 File Offset: 0x000433A8
	private IEnumerator UpdatePlayerCount()
	{
		WWWForm wwwform = new WWWForm();
		wwwform.AddField("player_count", PhotonNetwork.CountOfPlayers - 1);
		wwwform.AddField("game_version", "live");
		wwwform.AddField("game_name", Application.productName);
		Debug.Log(PhotonNetwork.CountOfPlayers - 1);
		using (UnityWebRequest www = UnityWebRequest.Post("http://ntsfranz.crabdance.com/update_monke_count", wwwform))
		{
			yield return www.SendWebRequest();
			if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
			{
				Debug.Log(www.error);
			}
		}
		UnityWebRequest www = null;
		yield break;
		yield break;
	}
}
