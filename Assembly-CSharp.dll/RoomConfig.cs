using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Fusion;
using Photon.Realtime;

// Token: 0x02000296 RID: 662
public class RoomConfig
{
	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x06001006 RID: 4102 RVA: 0x0003A08B File Offset: 0x0003828B
	public bool IsJoiningWithFriends
	{
		get
		{
			return this.joinFriendIDs != null && this.joinFriendIDs.Length != 0;
		}
	}

	// Token: 0x06001007 RID: 4103 RVA: 0x000A6994 File Offset: 0x000A4B94
	public void SetFriendIDs(List<string> friendIDs)
	{
		for (int i = 0; i < friendIDs.Count; i++)
		{
			if (friendIDs[i] == NetworkSystem.Instance.GetMyNickName())
			{
				friendIDs.RemoveAt(i);
				i--;
			}
		}
		this.joinFriendIDs = new string[friendIDs.Count];
		for (int j = 0; j < friendIDs.Count; j++)
		{
			this.joinFriendIDs[j] = friendIDs[j];
		}
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x0003A0A1 File Offset: 0x000382A1
	public void ClearExpectedUsers()
	{
		if (this.joinFriendIDs == null || this.joinFriendIDs.Length == 0)
		{
			return;
		}
		this.joinFriendIDs = new string[0];
	}

	// Token: 0x06001009 RID: 4105 RVA: 0x000A6A08 File Offset: 0x000A4C08
	public RoomOptions ToPUNOpts()
	{
		return new RoomOptions
		{
			IsVisible = this.isPublic,
			IsOpen = this.isJoinable,
			MaxPlayers = this.MaxPlayers,
			CustomRoomProperties = this.CustomProps,
			PublishUserId = true,
			CustomRoomPropertiesForLobby = this.AutoCustomLobbyProps()
		};
	}

	// Token: 0x0600100A RID: 4106 RVA: 0x0003A0C1 File Offset: 0x000382C1
	public void SetFusionOpts(NetworkRunner runnerInst)
	{
		runnerInst.SessionInfo.IsVisible = this.isPublic;
		runnerInst.SessionInfo.IsOpen = this.isJoinable;
	}

	// Token: 0x0600100B RID: 4107 RVA: 0x0003A0E5 File Offset: 0x000382E5
	public static RoomConfig SPConfig()
	{
		return new RoomConfig
		{
			isPublic = false,
			isJoinable = false,
			MaxPlayers = 1
		};
	}

	// Token: 0x0600100C RID: 4108 RVA: 0x0003A101 File Offset: 0x00038301
	public static RoomConfig AnyPublicConfig()
	{
		return new RoomConfig
		{
			isPublic = true,
			isJoinable = true,
			createIfMissing = true,
			MaxPlayers = 10
		};
	}

	// Token: 0x0600100D RID: 4109 RVA: 0x000A6A60 File Offset: 0x000A4C60
	private string[] AutoCustomLobbyProps()
	{
		string[] array = new string[this.CustomProps.Count];
		int num = 0;
		foreach (DictionaryEntry dictionaryEntry in this.CustomProps)
		{
			array[num] = (string)dictionaryEntry.Key;
			num++;
		}
		return array;
	}

	// Token: 0x04001217 RID: 4631
	public const string Room_GameModePropKey = "gameMode";

	// Token: 0x04001218 RID: 4632
	public const string Room_PlatformPropKey = "platform";

	// Token: 0x04001219 RID: 4633
	public bool isPublic;

	// Token: 0x0400121A RID: 4634
	public bool isJoinable;

	// Token: 0x0400121B RID: 4635
	public byte MaxPlayers;

	// Token: 0x0400121C RID: 4636
	public ExitGames.Client.Photon.Hashtable CustomProps = new ExitGames.Client.Photon.Hashtable();

	// Token: 0x0400121D RID: 4637
	public bool createIfMissing;

	// Token: 0x0400121E RID: 4638
	public string[] joinFriendIDs;
}
