using System;
using GorillaTagScripts.ModIO;
using ModIO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts.UI.ModIO
{
	// Token: 0x020009D5 RID: 2517
	public class ModIORoomMapDisplay : MonoBehaviour
	{
		// Token: 0x06003EC0 RID: 16064 RVA: 0x001299D0 File Offset: 0x00127BD0
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

		// Token: 0x06003EC1 RID: 16065 RVA: 0x00129AF0 File Offset: 0x00127CF0
		public void OnDestroy()
		{
			NetworkSystem.Instance.OnMultiplayerStarted -= this.OnJoinedRoom;
			NetworkSystem.Instance.OnReturnedToSinglePlayer -= this.OnDisconnectedFromRoom;
			CustomMapManager.OnRoomMapChanged.RemoveListener(new UnityAction<ModId>(this.OnRoomMapChanged));
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x00129B40 File Offset: 0x00127D40
		private void OnModIOLoggedOut()
		{
			this.roomMapLabelText.gameObject.SetActive(false);
			this.roomMapNameText.gameObject.SetActive(false);
			this.roomMapStatusText.gameObject.SetActive(false);
			this.roomMapStatusLabelText.gameObject.SetActive(false);
			this.loginToModioText.gameObject.SetActive(true);
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x00129BA2 File Offset: 0x00127DA2
		private void OnModIOLoggedIn()
		{
			this.loginToModioText.gameObject.SetActive(false);
			this.roomMapLabelText.gameObject.SetActive(true);
			this.roomMapNameText.gameObject.SetActive(true);
			this.UpdateRoomMap();
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x00129BDD File Offset: 0x00127DDD
		private void OnJoinedRoom()
		{
			this.UpdateRoomMap();
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x00129BDD File Offset: 0x00127DDD
		private void OnDisconnectedFromRoom()
		{
			this.UpdateRoomMap();
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x00129BDD File Offset: 0x00127DDD
		private void OnRoomMapChanged(ModId roomMapModId)
		{
			this.UpdateRoomMap();
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x00129BE8 File Offset: 0x00127DE8
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

		// Token: 0x06003EC8 RID: 16072 RVA: 0x00129C74 File Offset: 0x00127E74
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

		// Token: 0x06003EC9 RID: 16073 RVA: 0x00129CD1 File Offset: 0x00127ED1
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

		// Token: 0x04004001 RID: 16385
		[SerializeField]
		private TMP_Text roomMapLabelText;

		// Token: 0x04004002 RID: 16386
		[SerializeField]
		private TMP_Text roomMapNameText;

		// Token: 0x04004003 RID: 16387
		[SerializeField]
		private TMP_Text roomMapStatusLabelText;

		// Token: 0x04004004 RID: 16388
		[SerializeField]
		private TMP_Text roomMapStatusText;

		// Token: 0x04004005 RID: 16389
		[SerializeField]
		private TMP_Text loginToModioText;

		// Token: 0x04004006 RID: 16390
		[SerializeField]
		private string noRoomMapString = "NONE";

		// Token: 0x04004007 RID: 16391
		[SerializeField]
		private string notLoadedStatusString = "NOT LOADED";

		// Token: 0x04004008 RID: 16392
		[SerializeField]
		private string loadingStatusString = "LOADING...";

		// Token: 0x04004009 RID: 16393
		[SerializeField]
		private string readyToPlayStatusString = "READY!";

		// Token: 0x0400400A RID: 16394
		[SerializeField]
		private string loadFailedStatusString = "LOAD FAILED";

		// Token: 0x0400400B RID: 16395
		[SerializeField]
		private Color notLoadedStatusStringColor = Color.red;

		// Token: 0x0400400C RID: 16396
		[SerializeField]
		private Color loadingStatusStringColor = Color.yellow;

		// Token: 0x0400400D RID: 16397
		[SerializeField]
		private Color readyToPlayStatusStringColor = Color.green;

		// Token: 0x0400400E RID: 16398
		[SerializeField]
		private Color loadFailedStatusStringColor = Color.red;
	}
}
