using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x020005BF RID: 1471
public class FreeHoverboardManager : NetworkSceneObject
{
	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x06002488 RID: 9352 RVA: 0x000B5FA3 File Offset: 0x000B41A3
	// (set) Token: 0x06002489 RID: 9353 RVA: 0x000B5FAA File Offset: 0x000B41AA
	public static FreeHoverboardManager instance { get; private set; }

	// Token: 0x0600248A RID: 9354 RVA: 0x000B5FB4 File Offset: 0x000B41B4
	private FreeHoverboardManager.DataPerPlayer GetOrCreatePlayerData(int actorNumber)
	{
		FreeHoverboardManager.DataPerPlayer dataPerPlayer;
		if (!this.perPlayerData.TryGetValue(actorNumber, out dataPerPlayer))
		{
			dataPerPlayer = default(FreeHoverboardManager.DataPerPlayer);
			dataPerPlayer.Init(actorNumber, this.freeBoardPool);
			this.perPlayerData.Add(actorNumber, dataPerPlayer);
		}
		return dataPerPlayer;
	}

	// Token: 0x0600248B RID: 9355 RVA: 0x000B5FF8 File Offset: 0x000B41F8
	private void Awake()
	{
		FreeHoverboardManager.instance = this;
		for (int i = 0; i < 20; i++)
		{
			FreeHoverboardInstance freeHoverboardInstance = UnityEngine.Object.Instantiate<FreeHoverboardInstance>(this.freeHoverboardPrefab);
			freeHoverboardInstance.gameObject.SetActive(false);
			this.freeBoardPool.Push(freeHoverboardInstance);
		}
		NetworkSystem.Instance.OnPlayerLeft += this.OnPlayerLeftRoom;
		NetworkSystem.Instance.OnReturnedToSinglePlayer += this.OnLeftRoom;
	}

	// Token: 0x0600248C RID: 9356 RVA: 0x000B6068 File Offset: 0x000B4268
	private void OnPlayerLeftRoom(NetPlayer netPlayer)
	{
		FreeHoverboardManager.DataPerPlayer dataPerPlayer;
		if (this.perPlayerData.TryGetValue(netPlayer.ActorNumber, out dataPerPlayer))
		{
			dataPerPlayer.ReturnBoards(this.freeBoardPool);
			this.perPlayerData.Remove(netPlayer.ActorNumber);
		}
	}

	// Token: 0x0600248D RID: 9357 RVA: 0x000B60AC File Offset: 0x000B42AC
	private void OnLeftRoom()
	{
		foreach (KeyValuePair<int, FreeHoverboardManager.DataPerPlayer> keyValuePair in this.perPlayerData)
		{
			keyValuePair.Value.ReturnBoards(this.freeBoardPool);
		}
		this.perPlayerData.Clear();
	}

	// Token: 0x0600248E RID: 9358 RVA: 0x000B6118 File Offset: 0x000B4318
	private void SpawnBoard(FreeHoverboardManager.DataPerPlayer playerData, int boardIndex, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 avelocity, Color boardColor)
	{
		FreeHoverboardInstance freeHoverboardInstance = (boardIndex == 0) ? playerData.board0 : playerData.board1;
		freeHoverboardInstance.transform.position = position;
		freeHoverboardInstance.transform.rotation = rotation;
		freeHoverboardInstance.Rigidbody.velocity = velocity;
		freeHoverboardInstance.Rigidbody.angularVelocity = avelocity;
		freeHoverboardInstance.SetColor(boardColor);
		freeHoverboardInstance.gameObject.SetActive(true);
		int ownerActorNumber = freeHoverboardInstance.ownerActorNumber;
		NetPlayer localPlayer = NetworkSystem.Instance.LocalPlayer;
		int? num = (localPlayer != null) ? new int?(localPlayer.ActorNumber) : null;
		if (ownerActorNumber == num.GetValueOrDefault() & num != null)
		{
			this.localPlayerLastSpawnedBoardIndex = boardIndex;
		}
	}

	// Token: 0x0600248F RID: 9359 RVA: 0x000B61C0 File Offset: 0x000B43C0
	public void SendDropBoardRPC(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 avelocity, Color boardColor)
	{
		FreeHoverboardManager.DataPerPlayer orCreatePlayerData = this.GetOrCreatePlayerData(NetworkSystem.Instance.LocalPlayer.ActorNumber);
		int num = (!orCreatePlayerData.board0.gameObject.activeSelf) ? 0 : ((!orCreatePlayerData.board1.gameObject.activeSelf) ? 1 : (1 - this.localPlayerLastSpawnedBoardIndex));
		if (PhotonNetwork.InRoom)
		{
			long num2 = BitPackUtils.PackWorldPosForNetwork(position);
			int num3 = BitPackUtils.PackQuaternionForNetwork(rotation);
			long num4 = BitPackUtils.PackWorldPosForNetwork(velocity);
			long num5 = BitPackUtils.PackWorldPosForNetwork(avelocity);
			short num6 = BitPackUtils.PackColorForNetwork(boardColor);
			this.photonView.RPC("DropBoard_RPC", RpcTarget.All, new object[]
			{
				num == 1,
				num2,
				num3,
				num4,
				num5,
				num6
			});
			return;
		}
		this.SpawnBoard(orCreatePlayerData, num, position, rotation, velocity, avelocity, boardColor);
	}

	// Token: 0x06002490 RID: 9360 RVA: 0x000B62A8 File Offset: 0x000B44A8
	[PunRPC]
	public void DropBoard_RPC(bool boardIndex1, long positionPacked, int rotationPacked, long velocityPacked, long avelocityPacked, short colorPacked, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "DropBoard_RPC");
		int boardIndex2 = boardIndex1 ? 1 : 0;
		FreeHoverboardManager.DataPerPlayer orCreatePlayerData = this.GetOrCreatePlayerData(info.Sender.ActorNumber);
		if (info.Sender != PhotonNetwork.LocalPlayer && !orCreatePlayerData.spamCheck.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer))
		{
			return;
		}
		Vector3 position = BitPackUtils.UnpackWorldPosFromNetwork(positionPacked);
		if (!rigContainer.Rig.IsPositionInRange(position, 5f))
		{
			return;
		}
		this.SpawnBoard(orCreatePlayerData, boardIndex2, position, BitPackUtils.UnpackQuaternionFromNetwork(rotationPacked), BitPackUtils.UnpackWorldPosFromNetwork(velocityPacked), BitPackUtils.UnpackWorldPosFromNetwork(avelocityPacked), BitPackUtils.UnpackColorFromNetwork(colorPacked));
	}

	// Token: 0x06002491 RID: 9361 RVA: 0x000B6354 File Offset: 0x000B4554
	public void SendGrabBoardRPC(FreeHoverboardInstance board)
	{
		if (PhotonNetwork.InRoom)
		{
			this.photonView.RPC("GrabBoard_RPC", RpcTarget.All, new object[]
			{
				board.ownerActorNumber,
				board.boardIndex == 1
			});
			board.gameObject.SetActive(false);
			return;
		}
		board.gameObject.SetActive(false);
	}

	// Token: 0x06002492 RID: 9362 RVA: 0x000B63B8 File Offset: 0x000B45B8
	[PunRPC]
	public void GrabBoard_RPC(int ownerActorNumber, bool boardIndex1, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "GrabBoard_RPC");
		int boardIndex2 = boardIndex1 ? 1 : 0;
		if (NetworkSystem.Instance.GetNetPlayerByID(ownerActorNumber) == null)
		{
			return;
		}
		FreeHoverboardManager.DataPerPlayer orCreatePlayerData = this.GetOrCreatePlayerData(ownerActorNumber);
		if (info.Sender != PhotonNetwork.LocalPlayer && !orCreatePlayerData.spamCheck.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		FreeHoverboardInstance board = orCreatePlayerData.GetBoard(boardIndex2);
		if (board.IsNull())
		{
			return;
		}
		if (info.Sender.ActorNumber != ownerActorNumber)
		{
			RigContainer rigContainer;
			if (!VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer))
			{
				return;
			}
			if (!rigContainer.Rig.IsPositionInRange(board.transform.position, 5f))
			{
				return;
			}
		}
		board.gameObject.SetActive(false);
	}

	// Token: 0x06002493 RID: 9363 RVA: 0x000B6470 File Offset: 0x000B4670
	public void PreserveMaxHoverboardsConstraint(int actorNumber)
	{
		FreeHoverboardManager.DataPerPlayer dataPerPlayer;
		if (!this.perPlayerData.TryGetValue(actorNumber, out dataPerPlayer))
		{
			return;
		}
		if (dataPerPlayer.board0.gameObject.activeSelf && dataPerPlayer.board1.gameObject.activeSelf)
		{
			FreeHoverboardInstance board = dataPerPlayer.GetBoard(1 - this.localPlayerLastSpawnedBoardIndex);
			this.SendGrabBoardRPC(board);
		}
	}

	// Token: 0x040028B7 RID: 10423
	[SerializeField]
	private FreeHoverboardInstance freeHoverboardPrefab;

	// Token: 0x040028B8 RID: 10424
	private Stack<FreeHoverboardInstance> freeBoardPool = new Stack<FreeHoverboardInstance>(20);

	// Token: 0x040028B9 RID: 10425
	private const int NumPlayers = 10;

	// Token: 0x040028BA RID: 10426
	private const int NumFreeBoardsPerPlayer = 2;

	// Token: 0x040028BB RID: 10427
	private int localPlayerLastSpawnedBoardIndex;

	// Token: 0x040028BC RID: 10428
	private Dictionary<int, FreeHoverboardManager.DataPerPlayer> perPlayerData = new Dictionary<int, FreeHoverboardManager.DataPerPlayer>();

	// Token: 0x020005C0 RID: 1472
	private struct DataPerPlayer
	{
		// Token: 0x06002495 RID: 9365 RVA: 0x000B64EC File Offset: 0x000B46EC
		public void Init(int actorNumber, Stack<FreeHoverboardInstance> freeBoardPool)
		{
			this.board0 = freeBoardPool.Pop();
			this.board0.ownerActorNumber = actorNumber;
			this.board0.boardIndex = 0;
			this.board1 = freeBoardPool.Pop();
			this.board1.ownerActorNumber = actorNumber;
			this.board1.boardIndex = 1;
			this.spamCheck = new CallLimiterWithCooldown(5f, 10, 1f);
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x000B6558 File Offset: 0x000B4758
		public void ReturnBoards(Stack<FreeHoverboardInstance> freeBoardPool)
		{
			this.board0.gameObject.SetActive(false);
			freeBoardPool.Push(this.board0);
			this.board1.gameObject.SetActive(false);
			freeBoardPool.Push(this.board1);
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x000B6594 File Offset: 0x000B4794
		public FreeHoverboardInstance GetBoard(int boardIndex)
		{
			if (boardIndex != 1)
			{
				return this.board0;
			}
			return this.board1;
		}

		// Token: 0x040028BD RID: 10429
		public FreeHoverboardInstance board0;

		// Token: 0x040028BE RID: 10430
		public FreeHoverboardInstance board1;

		// Token: 0x040028BF RID: 10431
		public CallLimiterWithCooldown spamCheck;
	}
}
