using System;
using GorillaTagScripts.ModIO;
using ModIO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts.UI.ModIO
{
	// Token: 0x020009FE RID: 2558
	public class CustomMapsRoomMapDisplay : MonoBehaviour
	{
		// Token: 0x06003FEC RID: 16364 RVA: 0x0016AD38 File Offset: 0x00168F38
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

		// Token: 0x06003FED RID: 16365 RVA: 0x0016AE58 File Offset: 0x00169058
		public void OnDestroy()
		{
			NetworkSystem.Instance.OnMultiplayerStarted -= this.OnJoinedRoom;
			NetworkSystem.Instance.OnReturnedToSinglePlayer -= this.OnDisconnectedFromRoom;
			CustomMapManager.OnRoomMapChanged.RemoveListener(new UnityAction<ModId>(this.OnRoomMapChanged));
		}

		// Token: 0x06003FEE RID: 16366 RVA: 0x0016AEA8 File Offset: 0x001690A8
		private void OnModIOLoggedOut()
		{
			this.roomMapLabelText.gameObject.SetActive(false);
			this.roomMapNameText.gameObject.SetActive(false);
			this.roomMapStatusText.gameObject.SetActive(false);
			this.roomMapStatusLabelText.gameObject.SetActive(false);
			this.loginToModioText.gameObject.SetActive(true);
		}

		// Token: 0x06003FEF RID: 16367 RVA: 0x00059C4F File Offset: 0x00057E4F
		private void OnModIOLoggedIn()
		{
			this.loginToModioText.gameObject.SetActive(false);
			this.roomMapLabelText.gameObject.SetActive(true);
			this.roomMapNameText.gameObject.SetActive(true);
			this.UpdateRoomMap();
		}

		// Token: 0x06003FF0 RID: 16368 RVA: 0x00059C8A File Offset: 0x00057E8A
		private void OnJoinedRoom()
		{
			this.UpdateRoomMap();
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x00059C8A File Offset: 0x00057E8A
		private void OnDisconnectedFromRoom()
		{
			this.UpdateRoomMap();
		}

		// Token: 0x06003FF2 RID: 16370 RVA: 0x00059C8A File Offset: 0x00057E8A
		private void OnRoomMapChanged(ModId roomMapModId)
		{
			this.UpdateRoomMap();
		}

		// Token: 0x06003FF3 RID: 16371 RVA: 0x0016AF0C File Offset: 0x0016910C
		private void UpdateRoomMap()
		{
			if (!ModIOManager.IsLoggedIn())
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
			ModIOManager.GetModProfile(currentRoomMap, delegate(ModIORequestResultAnd<ModProfile> result)
			{
				if (!ModIOManager.IsLoggedIn())
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
				if (CustomMapLoader.IsModLoaded(currentRoomMap.id))
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

		// Token: 0x06003FF4 RID: 16372 RVA: 0x0016AF98 File Offset: 0x00169198
		private void OnMapLoadComplete(bool success)
		{
			if (!ModIOManager.IsLoggedIn())
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

		// Token: 0x06003FF5 RID: 16373 RVA: 0x00059C92 File Offset: 0x00057E92
		private void OnMapLoadProgress(MapLoadStatus status, int progress, string message)
		{
			if (!ModIOManager.IsLoggedIn())
			{
				return;
			}
			if (status - MapLoadStatus.Downloading <= 1)
			{
				this.roomMapStatusText.text = this.loadingStatusString;
				this.roomMapStatusText.color = this.loadingStatusStringColor;
			}
		}

		// Token: 0x040040E5 RID: 16613
		[SerializeField]
		private TMP_Text roomMapLabelText;

		// Token: 0x040040E6 RID: 16614
		[SerializeField]
		private TMP_Text roomMapNameText;

		// Token: 0x040040E7 RID: 16615
		[SerializeField]
		private TMP_Text roomMapStatusLabelText;

		// Token: 0x040040E8 RID: 16616
		[SerializeField]
		private TMP_Text roomMapStatusText;

		// Token: 0x040040E9 RID: 16617
		[SerializeField]
		private TMP_Text loginToModioText;

		// Token: 0x040040EA RID: 16618
		[SerializeField]
		private string noRoomMapString = "NONE";

		// Token: 0x040040EB RID: 16619
		[SerializeField]
		private string notLoadedStatusString = "NOT LOADED";

		// Token: 0x040040EC RID: 16620
		[SerializeField]
		private string loadingStatusString = "LOADING...";

		// Token: 0x040040ED RID: 16621
		[SerializeField]
		private string readyToPlayStatusString = "READY!";

		// Token: 0x040040EE RID: 16622
		[SerializeField]
		private string loadFailedStatusString = "LOAD FAILED";

		// Token: 0x040040EF RID: 16623
		[SerializeField]
		private Color notLoadedStatusStringColor = Color.red;

		// Token: 0x040040F0 RID: 16624
		[SerializeField]
		private Color loadingStatusStringColor = Color.yellow;

		// Token: 0x040040F1 RID: 16625
		[SerializeField]
		private Color readyToPlayStatusStringColor = Color.green;

		// Token: 0x040040F2 RID: 16626
		[SerializeField]
		private Color loadFailedStatusStringColor = Color.red;
	}
}
