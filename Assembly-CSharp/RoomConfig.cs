using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Fusion;
using Photon.Realtime;

// Token: 0x020002A1 RID: 673
public class RoomConfig
{
	// Token: 0x170001CC RID: 460
	// (get) Token: 0x0600104F RID: 4175 RVA: 0x0003B34B File Offset: 0x0003954B
	public bool IsJoiningWithFriends
	{
		get
		{
			return this.joinFriendIDs != null && this.joinFriendIDs.Length != 0;
		}
	}

	// Token: 0x06001050 RID: 4176 RVA: 0x000A9220 File Offset: 0x000A7420
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

	// Token: 0x06001051 RID: 4177 RVA: 0x0003B361 File Offset: 0x00039561
	public void ClearExpectedUsers()
	{
		if (this.joinFriendIDs == null || this.joinFriendIDs.Length == 0)
		{
			return;
		}
		this.joinFriendIDs = new string[0];
	}

	// Token: 0x06001052 RID: 4178 RVA: 0x000A9294 File Offset: 0x000A7494
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

	// Token: 0x06001053 RID: 4179 RVA: 0x0003B381 File Offset: 0x00039581
	public void SetFusionOpts(NetworkRunner runnerInst)
	{
		runnerInst.SessionInfo.IsVisible = this.isPublic;
		runnerInst.SessionInfo.IsOpen = this.isJoinable;
	}

	// Token: 0x06001054 RID: 4180 RVA: 0x0003B3A5 File Offset: 0x000395A5
	public static RoomConfig SPConfig()
	{
		return new RoomConfig
		{
			isPublic = false,
			isJoinable = false,
			MaxPlayers = 1
		};
	}

	// Token: 0x06001055 RID: 4181 RVA: 0x0003B3C1 File Offset: 0x000395C1
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

	// Token: 0x06001056 RID: 4182 RVA: 0x000A92EC File Offset: 0x000A74EC
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

	// Token: 0x0400125E RID: 4702
	public const string Room_GameModePropKey = "gameMode";

	// Token: 0x0400125F RID: 4703
	public const string Room_PlatformPropKey = "platform";

	// Token: 0x04001260 RID: 4704
	public bool isPublic;

	// Token: 0x04001261 RID: 4705
	public bool isJoinable;

	// Token: 0x04001262 RID: 4706
	public byte MaxPlayers;

	// Token: 0x04001263 RID: 4707
	public ExitGames.Client.Photon.Hashtable CustomProps = new ExitGames.Client.Photon.Hashtable();

	// Token: 0x04001264 RID: 4708
	public bool createIfMissing;

	// Token: 0x04001265 RID: 4709
	public string[] joinFriendIDs;
}
