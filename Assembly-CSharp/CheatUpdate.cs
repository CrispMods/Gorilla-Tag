using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x020004FB RID: 1275
public class CheatUpdate : MonoBehaviour
{
	// Token: 0x06001EE9 RID: 7913 RVA: 0x0009CBBD File Offset: 0x0009ADBD
	private void Start()
	{
		base.StartCoroutine(this.UpdateNumberOfPlayers());
	}

	// Token: 0x06001EEA RID: 7914 RVA: 0x0009CBCC File Offset: 0x0009ADCC
	public IEnumerator UpdateNumberOfPlayers()
	{
		for (;;)
		{
			base.StartCoroutine(this.UpdatePlayerCount());
			yield return new WaitForSeconds(10f);
		}
		yield break;
	}

	// Token: 0x06001EEB RID: 7915 RVA: 0x0009CBDB File Offset: 0x0009ADDB
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
