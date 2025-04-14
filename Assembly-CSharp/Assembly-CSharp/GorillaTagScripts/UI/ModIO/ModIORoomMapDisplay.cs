using System;
using GorillaTagScripts.ModIO;
using ModIO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts.UI.ModIO
{
	// Token: 0x020009D8 RID: 2520
	public class ModIORoomMapDisplay : MonoBehaviour
	{
		// Token: 0x06003ECC RID: 16076 RVA: 0x00129F98 File Offset: 0x00128198
		public void Start()
		{
			this.loginToModioText.gameObject.SetActive(true);
			this.roomMapNameText.text = this.noRoomMapString;
			this.roomMapStatusText.text = this.notLoadedStatusString;
			this.roomMapLabelText.gameObject.SetActive(false);
			this.roomMapNameText.gameObject.SetActive(false);
			this.roomMapStatusLabelText.gameObject.SetActive(false);
			this.roomMapStatusText.gameObject.SetActive(false);
			NetworkSystem.Instance.OnMultiplayerStarted += this.OnJoinedRoom;
			NetworkSystem.Instance.OnReturnedToSinglePlayer += this.OnDisconnectedFromRoom;
			GameEvents.OnModIOLoggedIn.AddListener(new UnityAction(this.OnModIOLoggedIn));
			GameEvents.OnModIOLoggedOut.AddListener(new UnityAction(this.OnModIOLoggedOut));
			CustomMapManager.OnRoomMapChanged.AddListener(new UnityAction<ModId>(this.OnRoomMapChanged));
			CustomMapManager.OnMapLoadStatusChanged.AddListener(new UnityAction<MapLoadStatus, int, string>(this.OnMapLoadProgress));
			CustomMapManager.OnMapLoadComplete.AddListener(new UnityAction<bool>(this.OnMapLoadComplete));
		}

		// Token: 0x06003ECD RID: 16077 RVA: 0x0012A0B8 File Offset: 0x001282B8
		public void OnDestroy()
		{
			NetworkSystem.Instance.OnMultiplayerStarted -= this.OnJoinedRoom;
			NetworkSystem.Instance.OnReturnedToSinglePlayer -= this.OnDisconnectedFromRoom;
			CustomMapManager.OnRoomMapChanged.RemoveListener(new UnityAction<ModId>(this.OnRoomMapChanged));
		}

		// Token: 0x06003ECE RID: 16078 RVA: 0x0012A108 File Offset: 0x00128308
		private void OnModIOLoggedOut()
		{
			this.roomMapLabelText.gameObject.SetActive(false);
			this.roomMapNameText.gameObject.SetActive(false);
			this.roomMapStatusText.gameObject.SetActive(false);
			this.roomMapStatusLabelText.gameObject.SetActive(false);
			this.loginToModioText.gameObject.SetActive(true);
		}

		// Token: 0x06003ECF RID: 16079 RVA: 0x0012A16A File Offset: 0x0012836A
		private void OnModIOLoggedIn()
		{
			this.loginToModioText.gameObject.SetActive(false);
			this.roomMapLabelText.gameObject.SetActive(true);
			this.roomMapNameText.gameObject.SetActive(true);
			this.UpdateRoomMap();
		}

		// Token: 0x06003ED0 RID: 16080 RVA: 0x0012A1A5 File Offset: 0x001283A5
		private void OnJoinedRoom()
		{
			this.UpdateRoomMap();
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x0012A1A5 File Offset: 0x001283A5
		private void OnDisconnectedFromRoom()
		{
			this.UpdateRoomMap();
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x0012A1A5 File Offset: 0x001283A5
		private void OnRoomMapChanged(ModId roomMapModId)
		{
			this.UpdateRoomMap();
		}

		// Token: 0x06003ED3 RID: 16083 RVA: 0x0012A1B0 File Offset: 0x001283B0
		private void UpdateRoomMap()
		{
			if (!ModIODataStore.IsLoggedIn())
			{
				return;
			}
			ModId currentRoomMap = CustomMapManager.GetRoomMapId();
			if (currentRoomMap == ModId.Null)
			{
				this.roomMapNameText.text = this.noRoomMapString;
				this.roomMapStatusLabelText.gameObject.SetActive(false);
				this.roomMapStatusText.gameObject.SetActive(false);
				return;
			}
			ModIODataStore.GetModProfile(currentRoomMap, delegate(ModIORequestResultAnd<ModProfile> result)
			{
				if (!ModIODataStore.IsLoggedIn())
				{
					return;
				}
				if (!result.result.success)
				{
					this.roomMapNameText.text = "Failed to retrieve mod info.";
					return;
				}
				this.roomMapNameText.text = result.data.name;
				this.roomMapStatusLabelText.gameObject.SetActive(true);
				if (ModIOMapLoader.IsModLoaded(currentRoomMap.id))
				{
					this.roomMapStatusText.text = this.readyToPlayStatusString;
					this.roomMapStatusText.color = this.readyToPlayStatusStringColor;
				}
				else if (CustomMapManager.IsLoading(currentRoomMap.id))
				{
					this.roomMapStatusText.text = this.loadingStatusString;
					this.roomMapStatusText.color = this.loadingStatusStringColor;
				}
				else
				{
					this.roomMapStatusText.text = this.notLoadedStatusString;
					this.roomMapStatusText.color = this.notLoadedStatusStringColor;
				}
				this.roomMapStatusText.gameObject.SetActive(true);
			});
		}

		// Token: 0x06003ED4 RID: 16084 RVA: 0x0012A23C File Offset: 0x0012843C
		private void OnMapLoadComplete(bool success)
		{
			if (!ModIODataStore.IsLoggedIn())
			{
				return;
			}
			if (success)
			{
				this.roomMapStatusText.text = this.readyToPlayStatusString;
				this.roomMapStatusText.color = this.readyToPlayStatusStringColor;
				return;
			}
			this.roomMapStatusText.text = this.loadFailedStatusString;
			this.roomMapStatusText.color = this.loadFailedStatusStringColor;
		}

		// Token: 0x06003ED5 RID: 16085 RVA: 0x0012A299 File Offset: 0x00128499
		private void OnMapLoadProgress(MapLoadStatus status, int progress, string message)
		{
			if (!ModIODataStore.IsLoggedIn())
			{
				return;
			}
			if (status - MapLoadStatus.Downloading <= 1)
			{
				this.roomMapStatusText.text = this.loadingStatusString;
				this.roomMapStatusText.color = this.loadingStatusStringColor;
			}
		}

		// Token: 0x04004013 RID: 16403
		[SerializeField]
		private TMP_Text roomMapLabelText;

		// Token: 0x04004014 RID: 16404
		[SerializeField]
		private TMP_Text roomMapNameText;

		// Token: 0x04004015 RID: 16405
		[SerializeField]
		private TMP_Text roomMapStatusLabelText;

		// Token: 0x04004016 RID: 16406
		[SerializeField]
		private TMP_Text roomMapStatusText;

		// Token: 0x04004017 RID: 16407
		[SerializeField]
		private TMP_Text loginToModioText;

		// Token: 0x04004018 RID: 16408
		[SerializeField]
		private string noRoomMapString = "NONE";

		// Token: 0x04004019 RID: 16409
		[SerializeField]
		private string notLoadedStatusString = "NOT LOADED";

		// Token: 0x0400401A RID: 16410
		[SerializeField]
		private string loadingStatusString = "LOADING...";

		// Token: 0x0400401B RID: 16411
		[SerializeField]
		private string readyToPlayStatusString = "READY!";

		// Token: 0x0400401C RID: 16412
		[SerializeField]
		private string loadFailedStatusString = "LOAD FAILED";

		// Token: 0x0400401D RID: 16413
		[SerializeField]
		private Color notLoadedStatusStringColor = Color.red;

		// Token: 0x0400401E RID: 16414
		[SerializeField]
		private Color loadingStatusStringColor = Color.yellow;

		// Token: 0x0400401F RID: 16415
		[SerializeField]
		private Color readyToPlayStatusStringColor = Color.green;

		// Token: 0x04004020 RID: 16416
		[SerializeField]
		private Color loadFailedStatusStringColor = Color.red;
	}
}
