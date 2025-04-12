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

// Token: 0x0200057E RID: 1406
public class Gorillanalytics : MonoBehaviour
{
	// Token: 0x06002292 RID: 8850 RVA: 0x0004666C File Offset: 0x0004486C
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

	// Token: 0x06002293 RID: 8851 RVA: 0x000F87A8 File Offset: 0x000F69A8
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

	// Token: 0x06002294 RID: 8852 RVA: 0x000F8978 File Offset: 0x000F6B78
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

	// Token: 0x04002631 RID: 9777
	public float interval = 60f;

	// Token: 0x04002632 RID: 9778
	public double oneOverChance = 4320.0;

	// Token: 0x04002633 RID: 9779
	public PhotonNetworkController photonNetworkController;

	// Token: 0x04002634 RID: 9780
	public GameModeZoneMapping gameModeData;

	// Token: 0x04002635 RID: 9781
	public List<string> maps;

	// Token: 0x04002636 RID: 9782
	public List<string> modes;

	// Token: 0x04002637 RID: 9783
	public List<string> queues;

	// Token: 0x04002638 RID: 9784
	private readonly Gorillanalytics.UploadData uploadData = new Gorillanalytics.UploadData();

	// Token: 0x0200057F RID: 1407
	private class UploadData
	{
		// Token: 0x04002639 RID: 9785
		public string version;

		// Token: 0x0400263A RID: 9786
		public double upload_chance;

		// Token: 0x0400263B RID: 9787
		public string map;

		// Token: 0x0400263C RID: 9788
		public string mode;

		// Token: 0x0400263D RID: 9789
		public string queue;

		// Token: 0x0400263E RID: 9790
		public int player_count;

		// Token: 0x0400263F RID: 9791
		public float pos_x;

		// Token: 0x04002640 RID: 9792
		public float pos_y;

		// Token: 0x04002641 RID: 9793
		public float pos_z;

		// Token: 0x04002642 RID: 9794
		public float vel_x;

		// Token: 0x04002643 RID: 9795
		public float vel_y;

		// Token: 0x04002644 RID: 9796
		public float vel_z;

		// Token: 0x04002645 RID: 9797
		public string cosmetics_owned;

		// Token: 0x04002646 RID: 9798
		public string cosmetics_worn;
	}
}
