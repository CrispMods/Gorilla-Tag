using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GorillaGameModes;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using UnityEngine;

// Token: 0x0200058B RID: 1419
public class Gorillanalytics : MonoBehaviour
{
	// Token: 0x060022EA RID: 8938 RVA: 0x00047A6A File Offset: 0x00045C6A
	private IEnumerator Start()
	{
		PlayFabTitleDataCache.Instance.GetTitleData("GorillanalyticsChance", delegate(string s)
		{
			double num;
			if (double.TryParse(s, out num))
			{
				this.oneOverChance = num;
			}
		}, delegate(PlayFabError e)
		{
		});
		for (;;)
		{
			yield return new WaitForSeconds(this.interval);
			if ((double)UnityEngine.Random.Range(0f, 1f) < 1.0 / this.oneOverChance && PlayFabClientAPI.IsClientLoggedIn())
			{
				this.UploadGorillanalytics();
			}
		}
		yield break;
	}

	// Token: 0x060022EB RID: 8939 RVA: 0x000FB524 File Offset: 0x000F9724
	private void UploadGorillanalytics()
	{
		try
		{
			string map;
			string mode;
			string queue;
			this.GetMapModeQueue(out map, out mode, out queue);
			Vector3 position = GTPlayer.Instance.headCollider.transform.position;
			Vector3 averagedVelocity = GTPlayer.Instance.AveragedVelocity;
			this.uploadData.version = NetworkSystemConfig.AppVersion;
			this.uploadData.upload_chance = this.oneOverChance;
			this.uploadData.map = map;
			this.uploadData.mode = mode;
			this.uploadData.queue = queue;
			this.uploadData.player_count = (int)(PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.PlayerCount : 0);
			this.uploadData.pos_x = position.x;
			this.uploadData.pos_y = position.y;
			this.uploadData.pos_z = position.z;
			this.uploadData.vel_x = averagedVelocity.x;
			this.uploadData.vel_y = averagedVelocity.y;
			this.uploadData.vel_z = averagedVelocity.z;
			this.uploadData.cosmetics_owned = string.Join(";", from c in CosmeticsController.instance.unlockedCosmetics
			select c.itemName);
			this.uploadData.cosmetics_worn = string.Join(";", from c in CosmeticsController.instance.currentWornSet.items
			select c.itemName);
			GorillaServer.Instance.UploadGorillanalytics(this.uploadData);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x060022EC RID: 8940 RVA: 0x000FB6F4 File Offset: 0x000F98F4
	private void GetMapModeQueue(out string map, out string mode, out string queue)
	{
		if (!PhotonNetwork.InRoom)
		{
			map = "none";
			mode = "none";
			queue = "none";
			return;
		}
		object obj = null;
		Room currentRoom = PhotonNetwork.CurrentRoom;
		if (currentRoom != null)
		{
			currentRoom.CustomProperties.TryGetValue("gameMode", out obj);
		}
		string gameMode = ((obj != null) ? obj.ToString() : null) ?? "";
		map = (this.maps.FirstOrDefault((string s) => gameMode.Contains(s)) ?? "unknown");
		mode = (this.modes.FirstOrDefault((string s) => gameMode.Contains(s)) ?? "unknown");
		queue = (this.queues.FirstOrDefault((string s) => gameMode.Contains(s)) ?? "unknown");
	}

	// Token: 0x04002686 RID: 9862
	public float interval = 60f;

	// Token: 0x04002687 RID: 9863
	public double oneOverChance = 4320.0;

	// Token: 0x04002688 RID: 9864
	public PhotonNetworkController photonNetworkController;

	// Token: 0x04002689 RID: 9865
	public GameModeZoneMapping gameModeData;

	// Token: 0x0400268A RID: 9866
	public List<string> maps;

	// Token: 0x0400268B RID: 9867
	public List<string> modes;

	// Token: 0x0400268C RID: 9868
	public List<string> queues;

	// Token: 0x0400268D RID: 9869
	private readonly Gorillanalytics.UploadData uploadData = new Gorillanalytics.UploadData();

	// Token: 0x0200058C RID: 1420
	private class UploadData
	{
		// Token: 0x0400268E RID: 9870
		public string version;

		// Token: 0x0400268F RID: 9871
		public double upload_chance;

		// Token: 0x04002690 RID: 9872
		public string map;

		// Token: 0x04002691 RID: 9873
		public string mode;

		// Token: 0x04002692 RID: 9874
		public string queue;

		// Token: 0x04002693 RID: 9875
		public int player_count;

		// Token: 0x04002694 RID: 9876
		public float pos_x;

		// Token: 0x04002695 RID: 9877
		public float pos_y;

		// Token: 0x04002696 RID: 9878
		public float pos_z;

		// Token: 0x04002697 RID: 9879
		public float vel_x;

		// Token: 0x04002698 RID: 9880
		public float vel_y;

		// Token: 0x04002699 RID: 9881
		public float vel_z;

		// Token: 0x0400269A RID: 9882
		public string cosmetics_owned;

		// Token: 0x0400269B RID: 9883
		public string cosmetics_worn;
	}
}
