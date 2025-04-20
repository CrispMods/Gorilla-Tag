using System;
using System.Collections.Generic;
using GorillaExtensions;
using GorillaGameModes;
using Ionic.Zlib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009CA RID: 2506
	public class BuilderTableNetworking : MonoBehaviourPunCallbacks
	{
		// Token: 0x06003E0E RID: 15886 RVA: 0x00161628 File Offset: 0x0015F828
		private void Awake()
		{
			BuilderTableNetworking.instance = this;
			this.masterClientTableInit = new List<BuilderTableNetworking.PlayerTableInitState>(10);
			this.masterClientTableValidators = new List<BuilderTableNetworking.PlayerTableInitState>(10);
			this.localClientTableInit = new BuilderTableNetworking.PlayerTableInitState();
			this.localValidationTable = new BuilderTableNetworking.PlayerTableInitState();
			this.callLimiters = new CallLimiter[28];
			this.callLimiters[0] = new CallLimiter(20, 30f, 0.5f);
			this.callLimiters[1] = new CallLimiter(20, 30f, 0.5f);
			this.callLimiters[2] = new CallLimiter(200, 1f, 0.5f);
			this.callLimiters[3] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[4] = new CallLimiter(2, 1f, 0.5f);
			this.callLimiters[5] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[6] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[7] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[8] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[9] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[10] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[11] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[12] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[13] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[14] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[15] = new CallLimiter(100, 1f, 0.5f);
			this.callLimiters[16] = new CallLimiter(100, 1f, 0.5f);
			this.callLimiters[17] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[18] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[19] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[20] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[21] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[22] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[23] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[24] = new CallLimiter(50, 1f, 0.5f);
			this.callLimiters[25] = new CallLimiter(50, 1f, 0.5f);
			this.armShelfRequests = new List<Player>(10);
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x0005867C File Offset: 0x0005687C
		public void SetTable(BuilderTable table)
		{
			this.currTable = table;
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x00058685 File Offset: 0x00056885
		private BuilderTable GetTable()
		{
			return this.currTable;
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x0005868D File Offset: 0x0005688D
		private int CreateLocalCommandId()
		{
			int result = this.nextLocalCommandId;
			this.nextLocalCommandId++;
			return result;
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x000586A3 File Offset: 0x000568A3
		public BuilderTableNetworking.PlayerTableInitState GetLocalTableInit()
		{
			return this.localClientTableInit;
		}

		// Token: 0x06003E13 RID: 15891 RVA: 0x00161924 File Offset: 0x0015FB24
		public override void OnMasterClientSwitched(Player newMasterClient)
		{
			if (!newMasterClient.IsLocal)
			{
				this.localClientTableInit.Reset();
				BuilderTable table = this.GetTable();
				if (table.GetTableState() != BuilderTable.TableState.WaitingForZoneAndRoom)
				{
					if (table.GetTableState() == BuilderTable.TableState.Ready)
					{
						table.SetTableState(BuilderTable.TableState.WaitForMasterResync);
					}
					else if (table.GetTableState() == BuilderTable.TableState.WaitForMasterResync || table.GetTableState() == BuilderTable.TableState.ReceivingMasterResync)
					{
						table.SetTableState(BuilderTable.TableState.WaitForMasterResync);
					}
					else
					{
						table.SetTableState(BuilderTable.TableState.WaitingForInitalBuild);
					}
					this.PlayerEnterBuilder();
				}
				return;
			}
			this.masterClientTableInit.Clear();
			this.localClientTableInit.Reset();
			BuilderTable table2 = this.GetTable();
			BuilderTable.TableState tableState = table2.GetTableState();
			bool flag = (tableState != BuilderTable.TableState.Ready && tableState != BuilderTable.TableState.WaitingForZoneAndRoom && tableState != BuilderTable.TableState.WaitForMasterResync && tableState != BuilderTable.TableState.ReceivingMasterResync) || table2.pieces.Count <= 0;
			if (!flag)
			{
				flag |= (table2.pieces.Count <= 0);
			}
			if (flag)
			{
				table2.ClearTable();
				table2.ClearQueuedCommands();
				table2.SetTableState(BuilderTable.TableState.WaitForInitialBuildMaster);
				return;
			}
			for (int i = 0; i < table2.pieces.Count; i++)
			{
				BuilderPiece builderPiece = table2.pieces[i];
				Player player = PhotonNetwork.CurrentRoom.GetPlayer(builderPiece.heldByPlayerActorNumber, false);
				if (table2.pieces[i].state == BuilderPiece.State.Grabbed && player == null)
				{
					Vector3 position = builderPiece.transform.position;
					Quaternion rotation = builderPiece.transform.rotation;
					Debug.LogErrorFormat("We have a piece {0} {1} held by an invalid player {2} dropping", new object[]
					{
						builderPiece.name,
						builderPiece.pieceId,
						builderPiece.heldByPlayerActorNumber
					});
					this.CreateLocalCommandId();
					builderPiece.ClearParentHeld();
					builderPiece.ClearParentPiece(false);
					builderPiece.transform.localScale = Vector3.one;
					builderPiece.SetState(BuilderPiece.State.Dropped, false);
					builderPiece.transform.SetLocalPositionAndRotation(position, rotation);
					if (builderPiece.rigidBody != null)
					{
						builderPiece.rigidBody.position = position;
						builderPiece.rigidBody.rotation = rotation;
						builderPiece.rigidBody.velocity = Vector3.zero;
						builderPiece.rigidBody.angularVelocity = Vector3.zero;
					}
				}
			}
			table2.ClearQueuedCommands();
			table2.SetTableState(BuilderTable.TableState.Ready);
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x00161B60 File Offset: 0x0015FD60
		public override void OnPlayerLeftRoom(Player player)
		{
			Debug.LogFormat("Player {0} left room", new object[]
			{
				player.ActorNumber
			});
			BuilderTable table = this.GetTable();
			if (table.GetTableState() != BuilderTable.TableState.WaitingForZoneAndRoom)
			{
				if (!PhotonNetwork.IsMasterClient)
				{
					table.DropAllPiecesForPlayerLeaving(player.ActorNumber);
				}
				else
				{
					table.RecycleAllPiecesForPlayerLeaving(player.ActorNumber);
				}
				table.PlayerLeftRoom(player.ActorNumber);
			}
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			table.RemoveArmShelfForPlayer(player);
			table.VerifySetSelections();
			if (player != PhotonNetwork.LocalPlayer)
			{
				this.DestroyPlayerTableInit(player);
			}
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x000586AB File Offset: 0x000568AB
		public override void OnJoinedRoom()
		{
			base.OnJoinedRoom();
			this.GetTable().SetInRoom(true);
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x000586BF File Offset: 0x000568BF
		public override void OnLeftRoom()
		{
			this.PlayerExitBuilder();
			this.GetTable().SetInRoom(false);
			this.armShelfRequests.Clear();
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x000586DE File Offset: 0x000568DE
		private void Update()
		{
			if (PhotonNetwork.IsMasterClient)
			{
				this.UpdateNewPlayerInit();
			}
		}

		// Token: 0x06003E18 RID: 15896 RVA: 0x00161BEC File Offset: 0x0015FDEC
		public void PlayerEnterBuilder()
		{
			this.tablePhotonView.RPC("PlayerEnterBuilderRPC", PhotonNetwork.MasterClient, new object[]
			{
				PhotonNetwork.LocalPlayer
			});
			GorillaGuardianManager gorillaGuardianManager = GameMode.ActiveGameMode as GorillaGuardianManager;
			if (gorillaGuardianManager != null && gorillaGuardianManager.isPlaying && gorillaGuardianManager.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
			{
				gorillaGuardianManager.RequestEjectGuardian(NetworkSystem.Instance.LocalPlayer);
			}
		}

		// Token: 0x06003E19 RID: 15897 RVA: 0x00161C54 File Offset: 0x0015FE54
		[PunRPC]
		public void PlayerEnterBuilderRPC(Player player, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "PlayerEnterBuilderRPC");
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.PlayerEnterMaster, info))
			{
				return;
			}
			if (player == null || !player.Equals(info.Sender))
			{
				return;
			}
			BuilderTable table = this.GetTable();
			BuilderTable.TableState tableState = table.GetTableState();
			if (tableState == BuilderTable.TableState.WaitingForInitalBuild || (this.IsPrivateMasterClient() && tableState == BuilderTable.TableState.WaitingForZoneAndRoom))
			{
				table.SetTableState(BuilderTable.TableState.WaitForInitialBuildMaster);
			}
			if (player != PhotonNetwork.LocalPlayer)
			{
				this.CreateSerializedTableForNewPlayerInit(player);
			}
			this.RequestCreateArmShelfForPlayer(player);
		}

		// Token: 0x06003E1A RID: 15898 RVA: 0x00161CCC File Offset: 0x0015FECC
		public void PlayerExitBuilder()
		{
			this.tablePhotonView.RPC("PlayerExitBuilderRPC", PhotonNetwork.MasterClient, new object[]
			{
				PhotonNetwork.LocalPlayer
			});
			BuilderTable table = this.GetTable();
			table.ClearTable();
			table.ClearQueuedCommands();
			this.localClientTableInit.Reset();
			this.armShelfRequests.Clear();
			this.masterClientTableInit.Clear();
		}

		// Token: 0x06003E1B RID: 15899 RVA: 0x00161D30 File Offset: 0x0015FF30
		[PunRPC]
		public void PlayerExitBuilderRPC(Player player, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "PlayerExitBuilderRPC");
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.PlayerExitMaster, info))
			{
				return;
			}
			if (player == null || !player.Equals(info.Sender))
			{
				return;
			}
			if (player.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
			{
				this.DestroyPlayerTableInit(player);
			}
			this.GetTable().RemoveArmShelfForPlayer(player);
		}

		// Token: 0x06003E1C RID: 15900 RVA: 0x000586ED File Offset: 0x000568ED
		public bool IsPrivateMasterClient()
		{
			return PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient && NetworkSystem.Instance.SessionIsPrivate;
		}

		// Token: 0x06003E1D RID: 15901 RVA: 0x00161D94 File Offset: 0x0015FF94
		private void UpdateNewPlayerInit()
		{
			if (this.GetTable().GetTableState() == BuilderTable.TableState.Ready)
			{
				for (int i = 0; i < this.masterClientTableInit.Count; i++)
				{
					if (this.masterClientTableInit[i].waitForInitTimeRemaining >= 0f)
					{
						this.masterClientTableInit[i].waitForInitTimeRemaining -= Time.deltaTime;
						if (this.masterClientTableInit[i].waitForInitTimeRemaining <= 0f)
						{
							this.StartCreatingSerializedTable(this.masterClientTableInit[i].player);
							this.masterClientTableInit[i].waitForInitTimeRemaining = -1f;
							this.masterClientTableInit[i].sendNextChunkTimeRemaining = 0f;
						}
					}
					else if (this.masterClientTableInit[i].sendNextChunkTimeRemaining >= 0f)
					{
						this.masterClientTableInit[i].sendNextChunkTimeRemaining -= Time.deltaTime;
						if (this.masterClientTableInit[i].sendNextChunkTimeRemaining <= 0f)
						{
							this.SendNextTableData(this.masterClientTableInit[i].player);
							if (this.masterClientTableInit[i].numSerializedBytes < this.masterClientTableInit[i].totalSerializedBytes)
							{
								this.masterClientTableInit[i].sendNextChunkTimeRemaining = 0f;
							}
							else
							{
								this.masterClientTableInit[i].sendNextChunkTimeRemaining = -1f;
							}
						}
					}
				}
			}
		}

		// Token: 0x06003E1E RID: 15902 RVA: 0x00161F24 File Offset: 0x00160124
		private void StartCreatingSerializedTable(Player newPlayer)
		{
			BuilderTable table = this.GetTable();
			BuilderTableNetworking.PlayerTableInitState playerTableInit = this.GetPlayerTableInit(newPlayer);
			playerTableInit.totalSerializedBytes = table.SerializeTableState(playerTableInit.serializedTableState, 1048576);
			byte[] array = GZipStream.CompressBuffer(playerTableInit.serializedTableState);
			playerTableInit.totalSerializedBytes = array.Length;
			Array.Copy(array, 0, playerTableInit.serializedTableState, 0, playerTableInit.totalSerializedBytes);
			playerTableInit.numSerializedBytes = 0;
			this.tablePhotonView.RPC("StartBuildTableRPC", newPlayer, new object[]
			{
				playerTableInit.totalSerializedBytes
			});
		}

		// Token: 0x06003E1F RID: 15903 RVA: 0x00161FAC File Offset: 0x001601AC
		[PunRPC]
		public void StartBuildTableRPC(int totalBytes, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "StartBuildTableRPC");
			if (PhotonNetwork.IsMasterClient)
			{
				return;
			}
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.TableDataStart, info))
			{
				return;
			}
			if (totalBytes <= 0 || totalBytes > 1048576)
			{
				Debug.LogError("Builder Table Bytes is too large: " + totalBytes.ToString());
				return;
			}
			BuilderTable table = this.GetTable();
			if (table.GetTableState() != BuilderTable.TableState.WaitForMasterResync && table.GetTableState() != BuilderTable.TableState.WaitingForInitalBuild)
			{
				return;
			}
			if (table.GetTableState() == BuilderTable.TableState.WaitForMasterResync)
			{
				table.SetTableState(BuilderTable.TableState.ReceivingMasterResync);
			}
			else
			{
				table.SetTableState(BuilderTable.TableState.ReceivingInitialBuild);
			}
			this.localClientTableInit.Reset();
			BuilderTableNetworking.PlayerTableInitState playerTableInitState = this.localClientTableInit;
			playerTableInitState.player = PhotonNetwork.LocalPlayer;
			playerTableInitState.totalSerializedBytes = totalBytes;
			table.ClearQueuedCommands();
		}

		// Token: 0x06003E20 RID: 15904 RVA: 0x00162064 File Offset: 0x00160264
		private void SendNextTableData(Player requestingPlayer)
		{
			BuilderTableNetworking.PlayerTableInitState playerTableInit = this.GetPlayerTableInit(requestingPlayer);
			if (playerTableInit == null)
			{
				Debug.LogErrorFormat("No Table init found for player {0}", new object[]
				{
					requestingPlayer.ActorNumber
				});
				return;
			}
			int num = Mathf.Min(1000, playerTableInit.totalSerializedBytes - playerTableInit.numSerializedBytes);
			if (num <= 0)
			{
				return;
			}
			Array.Copy(playerTableInit.serializedTableState, playerTableInit.numSerializedBytes, playerTableInit.chunk, 0, num);
			playerTableInit.numSerializedBytes += num;
			this.tablePhotonView.RPC("SendTableDataRPC", requestingPlayer, new object[]
			{
				num,
				playerTableInit.chunk
			});
		}

		// Token: 0x06003E21 RID: 15905 RVA: 0x00162108 File Offset: 0x00160308
		[PunRPC]
		public void SendTableDataRPC(int numBytes, byte[] bytes, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			if (numBytes <= 0 || numBytes > 1000)
			{
				Debug.LogErrorFormat("Builder Table Send Data numBytes is too large {0}", new object[]
				{
					numBytes
				});
				return;
			}
			if (bytes.Length > 1000)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "SendTableDataRPC");
			if (PhotonNetwork.IsMasterClient)
			{
				return;
			}
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.TableData, info))
			{
				return;
			}
			BuilderTableNetworking.PlayerTableInitState playerTableInitState = this.localClientTableInit;
			if (playerTableInitState.numSerializedBytes + bytes.Length > 1048576)
			{
				Debug.LogErrorFormat("Builder Table serialized bytes is larger than buffer {0}", new object[]
				{
					playerTableInitState.numSerializedBytes + bytes.Length
				});
				return;
			}
			Array.Copy(bytes, 0, playerTableInitState.serializedTableState, playerTableInitState.numSerializedBytes, bytes.Length);
			playerTableInitState.numSerializedBytes += bytes.Length;
			if (playerTableInitState.numSerializedBytes >= playerTableInitState.totalSerializedBytes)
			{
				this.GetTable().SetTableState(BuilderTable.TableState.InitialBuild);
			}
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x001621F0 File Offset: 0x001603F0
		private bool DoesTableInitExist(Player player)
		{
			for (int i = 0; i < this.masterClientTableInit.Count; i++)
			{
				if (this.masterClientTableInit[i].player.ActorNumber == player.ActorNumber)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003E23 RID: 15907 RVA: 0x00162234 File Offset: 0x00160434
		private BuilderTableNetworking.PlayerTableInitState CreatePlayerTableInit(Player player)
		{
			for (int i = 0; i < this.masterClientTableInit.Count; i++)
			{
				if (this.masterClientTableInit[i].player.ActorNumber == player.ActorNumber)
				{
					this.masterClientTableInit[i].Reset();
					return this.masterClientTableInit[i];
				}
			}
			BuilderTableNetworking.PlayerTableInitState playerTableInitState = new BuilderTableNetworking.PlayerTableInitState();
			playerTableInitState.player = player;
			this.masterClientTableInit.Add(playerTableInitState);
			return playerTableInitState;
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x00058707 File Offset: 0x00056907
		private void CreateSerializedTableForNewPlayerInit(Player newPlayer)
		{
			if (this.DoesTableInitExist(newPlayer))
			{
				return;
			}
			BuilderTableNetworking.PlayerTableInitState playerTableInitState = this.CreatePlayerTableInit(newPlayer);
			playerTableInitState.waitForInitTimeRemaining = 1f;
			playerTableInitState.sendNextChunkTimeRemaining = -1f;
		}

		// Token: 0x06003E25 RID: 15909 RVA: 0x001622B0 File Offset: 0x001604B0
		private void DestroyPlayerTableInit(Player player)
		{
			for (int i = 0; i < this.masterClientTableInit.Count; i++)
			{
				if (this.masterClientTableInit[i].player.ActorNumber == player.ActorNumber)
				{
					this.masterClientTableInit.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06003E26 RID: 15910 RVA: 0x00162304 File Offset: 0x00160504
		private BuilderTableNetworking.PlayerTableInitState GetPlayerTableInit(Player player)
		{
			for (int i = 0; i < this.masterClientTableInit.Count; i++)
			{
				if (this.masterClientTableInit[i].player.ActorNumber == player.ActorNumber)
				{
					return this.masterClientTableInit[i];
				}
			}
			return null;
		}

		// Token: 0x06003E27 RID: 15911 RVA: 0x00162354 File Offset: 0x00160554
		private bool ValidateMasterClientIsReady(Player player)
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				return false;
			}
			if (player != null && !player.IsMasterClient)
			{
				BuilderTableNetworking.PlayerTableInitState playerTableInit = this.GetPlayerTableInit(player);
				if (playerTableInit != null && playerTableInit.numSerializedBytes < playerTableInit.totalSerializedBytes)
				{
					return false;
				}
			}
			return this.GetTable().GetTableState() == BuilderTable.TableState.Ready;
		}

		// Token: 0x06003E28 RID: 15912 RVA: 0x001623A4 File Offset: 0x001605A4
		private bool ValidateCallLimits(BuilderTableNetworking.RPC rpcCall, PhotonMessageInfo info)
		{
			return rpcCall >= BuilderTableNetworking.RPC.PlayerEnterMaster && rpcCall < BuilderTableNetworking.RPC.Count && this.callLimiters[(int)rpcCall].CheckCallTime(Time.time);
		}

		// Token: 0x06003E29 RID: 15913 RVA: 0x0005872F File Offset: 0x0005692F
		[PunRPC]
		public void RequestFailedRPC(int localCommandId, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "RequestFailedRPC");
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.RequestFailed, info))
			{
				return;
			}
			this.GetTable().RollbackFailedCommand(localCommandId);
		}

		// Token: 0x06003E2A RID: 15914 RVA: 0x00030607 File Offset: 0x0002E807
		public void RequestCreatePiece(int newPieceType, Vector3 position, Quaternion rotation, int materialType)
		{
		}

		// Token: 0x06003E2B RID: 15915 RVA: 0x00030607 File Offset: 0x0002E807
		public void RequestCreatePieceRPC(int newPieceType, long packedPosition, int packedRotation, int materialType, PhotonMessageInfo info)
		{
		}

		// Token: 0x06003E2C RID: 15916 RVA: 0x00030607 File Offset: 0x0002E807
		public void PieceCreatedRPC(int pieceType, int pieceId, long packedPosition, int packedRotation, int materialType, Player creatingPlayer, PhotonMessageInfo info)
		{
		}

		// Token: 0x06003E2D RID: 15917 RVA: 0x001623D4 File Offset: 0x001605D4
		public void CreateShelfPiece(int pieceType, Vector3 position, Quaternion rotation, int materialType, BuilderPiece.State state, int shelfID)
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			BuilderTable table = this.GetTable();
			if (table.GetTableState() != BuilderTable.TableState.Ready)
			{
				return;
			}
			BuilderPiece piecePrefab = table.GetPiecePrefab(pieceType);
			if (!table.HasEnoughResources(piecePrefab))
			{
				Debug.Log("Not Enough Resources");
				return;
			}
			if (state != BuilderPiece.State.OnShelf)
			{
				if (state != BuilderPiece.State.OnConveyor)
				{
					return;
				}
				if (shelfID < 0 || shelfID >= table.conveyors.Count)
				{
					return;
				}
			}
			else if (shelfID < 0 || shelfID >= table.dispenserShelves.Count)
			{
				return;
			}
			int num = table.CreatePieceId();
			long num2 = BitPackUtils.PackWorldPosForNetwork(position);
			int num3 = BitPackUtils.PackQuaternionForNetwork(rotation);
			base.photonView.RPC("PieceCreatedByShelfRPC", RpcTarget.All, new object[]
			{
				pieceType,
				num,
				num2,
				num3,
				materialType,
				(byte)state,
				shelfID,
				PhotonNetwork.LocalPlayer
			});
		}

		// Token: 0x06003E2E RID: 15918 RVA: 0x001624C4 File Offset: 0x001606C4
		[PunRPC]
		public void PieceCreatedByShelfRPC(int pieceType, int pieceId, long packedPosition, int packedRotation, int materialType, byte state, int shelfID, Player creatingPlayer, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.CreateShelfPieceMaster, info))
			{
				return;
			}
			BuilderTable table = this.GetTable();
			Vector3 position = BitPackUtils.UnpackWorldPosFromNetwork(packedPosition);
			Quaternion rotation = BitPackUtils.UnpackQuaternionFromNetwork(packedRotation);
			if (!table.ValidatePieceWorldTransform(position, rotation))
			{
				return;
			}
			if (state == 4)
			{
				table.CreateDispenserShelfPiece(pieceType, pieceId, position, rotation, materialType, shelfID);
				return;
			}
			if (state != 7)
			{
				return;
			}
			table.CreateConveyorPiece(pieceType, pieceId, position, rotation, materialType, shelfID, info.SentServerTimestamp);
		}

		// Token: 0x06003E2F RID: 15919 RVA: 0x00162540 File Offset: 0x00160740
		public void RequestRecyclePiece(int pieceId, Vector3 position, Quaternion rotation, bool playFX, int recyclerID)
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			if (this.GetTable().GetTableState() != BuilderTable.TableState.Ready)
			{
				return;
			}
			float num = 10000f;
			if (!position.IsValid(num) || !rotation.IsValid())
			{
				return;
			}
			if (recyclerID > 32767 || recyclerID < -1)
			{
				return;
			}
			long num2 = BitPackUtils.PackWorldPosForNetwork(position);
			int num3 = BitPackUtils.PackQuaternionForNetwork(rotation);
			base.photonView.RPC("PieceDestroyedRPC", RpcTarget.All, new object[]
			{
				pieceId,
				num2,
				num3,
				playFX,
				(short)recyclerID
			});
		}

		// Token: 0x06003E30 RID: 15920 RVA: 0x001625E4 File Offset: 0x001607E4
		[PunRPC]
		public void PieceDestroyedRPC(int pieceId, long packedPosition, int packedRotation, bool playFX, short recyclerID, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "PieceDestroyedRPC");
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.RecyclePieceMaster, info))
			{
				return;
			}
			Vector3 position = BitPackUtils.UnpackWorldPosFromNetwork(packedPosition);
			Quaternion rotation = BitPackUtils.UnpackQuaternionFromNetwork(packedRotation);
			float num = 10000f;
			if (!position.IsValid(num) || !rotation.IsValid())
			{
				return;
			}
			this.GetTable().RecyclePiece(pieceId, position, rotation, playFX, (int)recyclerID, info.Sender);
		}

		// Token: 0x06003E31 RID: 15921 RVA: 0x0016265C File Offset: 0x0016085C
		public void RequestPlacePiece(BuilderPiece piece, BuilderPiece attachPiece, sbyte bumpOffsetX, sbyte bumpOffsetZ, byte twist, BuilderPiece parentPiece, int attachIndex, int parentAttachIndex)
		{
			if (piece == null)
			{
				return;
			}
			int pieceId = piece.pieceId;
			int num = (parentPiece != null) ? parentPiece.pieceId : -1;
			int num2 = (attachPiece != null) ? attachPiece.pieceId : -1;
			BuilderTable table = this.GetTable();
			if (!table.ValidatePlacePieceParams(pieceId, num2, bumpOffsetX, bumpOffsetZ, twist, num, attachIndex, parentAttachIndex, NetPlayer.Get(PhotonNetwork.LocalPlayer)))
			{
				return;
			}
			int num3 = this.CreateLocalCommandId();
			attachPiece.requestedParentPiece = parentPiece;
			BuilderTable.instance.UpdatePieceData(attachPiece);
			table.PlacePiece(num3, pieceId, num2, bumpOffsetX, bumpOffsetZ, twist, num, attachIndex, parentAttachIndex, NetPlayer.Get(PhotonNetwork.LocalPlayer), PhotonNetwork.ServerTimestamp, true);
			int num4 = BuilderTable.PackPiecePlacement(twist, bumpOffsetX, bumpOffsetZ);
			if (table.GetTableState() == BuilderTable.TableState.Ready)
			{
				base.photonView.RPC("RequestPlacePieceRPC", RpcTarget.MasterClient, new object[]
				{
					num3,
					pieceId,
					num2,
					num4,
					num,
					attachIndex,
					parentAttachIndex,
					PhotonNetwork.LocalPlayer
				});
			}
		}

		// Token: 0x06003E32 RID: 15922 RVA: 0x00162780 File Offset: 0x00160980
		[PunRPC]
		public void RequestPlacePieceRPC(int localCommandId, int pieceId, int attachPieceId, int placement, int parentPieceId, int attachIndex, int parentAttachIndex, Player placedByPlayer, PhotonMessageInfo info)
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "RequestPlacePieceRPC");
			if (!this.ValidateMasterClientIsReady(info.Sender))
			{
				return;
			}
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.PlacePieceMaster, info) || placedByPlayer == null || !placedByPlayer.Equals(info.Sender))
			{
				return;
			}
			BuilderTable table = this.GetTable();
			bool isMasterClient = info.Sender.IsMasterClient;
			byte twist;
			sbyte bumpOffsetX;
			sbyte bumpOffsetZ;
			BuilderTable.UnpackPiecePlacement(placement, out twist, out bumpOffsetX, out bumpOffsetZ);
			bool flag = isMasterClient || table.ValidatePlacePieceParams(pieceId, attachPieceId, bumpOffsetX, bumpOffsetZ, twist, parentPieceId, attachIndex, parentAttachIndex, NetPlayer.Get(placedByPlayer));
			if (flag)
			{
				flag &= (isMasterClient || table.ValidatePlacePieceState(pieceId, attachPieceId, bumpOffsetX, bumpOffsetZ, twist, parentPieceId, attachIndex, parentAttachIndex, placedByPlayer));
			}
			if (flag)
			{
				BuilderPiece piece = table.GetPiece(parentPieceId);
				BuilderPiecePrivatePlot builderPiecePrivatePlot;
				if (piece != null && piece.TryGetPlotComponent(out builderPiecePrivatePlot) && !builderPiecePrivatePlot.IsPlotClaimed())
				{
					base.photonView.RPC("PlotClaimedRPC", RpcTarget.All, new object[]
					{
						parentPieceId,
						placedByPlayer
					});
				}
				base.photonView.RPC("PiecePlacedRPC", RpcTarget.All, new object[]
				{
					localCommandId,
					pieceId,
					attachPieceId,
					placement,
					parentPieceId,
					attachIndex,
					parentAttachIndex,
					placedByPlayer,
					info.SentServerTimestamp
				});
				return;
			}
			base.photonView.RPC("RequestFailedRPC", info.Sender, new object[]
			{
				localCommandId
			});
		}

		// Token: 0x06003E33 RID: 15923 RVA: 0x00162924 File Offset: 0x00160B24
		[PunRPC]
		public void PiecePlacedRPC(int localCommandId, int pieceId, int attachPieceId, int placement, int parentPieceId, int attachIndex, int parentAttachIndex, Player placedByPlayer, int timeStamp, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "PiecePlacedRPC");
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.PlacePiece, info))
			{
				return;
			}
			if (placedByPlayer == null)
			{
				return;
			}
			if ((ulong)(PhotonNetwork.ServerTimestamp - info.SentServerTimestamp) > (ulong)((long)PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout) || (ulong)(info.SentServerTimestamp - timeStamp) > (ulong)((long)PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout))
			{
				timeStamp = PhotonNetwork.ServerTimestamp;
			}
			byte twist;
			sbyte bumpOffsetX;
			sbyte bumpOffsetZ;
			BuilderTable.UnpackPiecePlacement(placement, out twist, out bumpOffsetX, out bumpOffsetZ);
			this.GetTable().PlacePiece(localCommandId, pieceId, attachPieceId, bumpOffsetX, bumpOffsetZ, twist, parentPieceId, attachIndex, parentAttachIndex, NetPlayer.Get(placedByPlayer), timeStamp, false);
		}

		// Token: 0x06003E34 RID: 15924 RVA: 0x001629D0 File Offset: 0x00160BD0
		public void RequestGrabPiece(BuilderPiece piece, bool isLefHand, Vector3 localPosition, Quaternion localRotation)
		{
			if (piece == null)
			{
				return;
			}
			BuilderTable table = this.GetTable();
			if (!table.ValidateGrabPieceParams(piece.pieceId, isLefHand, localPosition, localRotation, NetPlayer.Get(PhotonNetwork.LocalPlayer)))
			{
				return;
			}
			if (PhotonNetwork.IsMasterClient)
			{
				this.CheckForFreedPlot(piece.pieceId, PhotonNetwork.LocalPlayer);
			}
			int num = this.CreateLocalCommandId();
			table.GrabPiece(num, piece.pieceId, isLefHand, localPosition, localRotation, NetPlayer.Get(PhotonNetwork.LocalPlayer), true);
			if (table.GetTableState() == BuilderTable.TableState.Ready)
			{
				long num2 = BitPackUtils.PackHandPosRotForNetwork(localPosition, localRotation);
				base.photonView.RPC("RequestGrabPieceRPC", RpcTarget.MasterClient, new object[]
				{
					num,
					piece.pieceId,
					isLefHand,
					num2,
					PhotonNetwork.LocalPlayer
				});
			}
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x00162AA0 File Offset: 0x00160CA0
		[PunRPC]
		public void RequestGrabPieceRPC(int localCommandId, int pieceId, bool isLeftHand, long packedPosRot, Player grabbedByPlayer, PhotonMessageInfo info)
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "RequestGrabPieceRPC");
			if (!this.ValidateMasterClientIsReady(info.Sender))
			{
				return;
			}
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.GrabPieceMaster, info) || !grabbedByPlayer.Equals(info.Sender))
			{
				return;
			}
			Vector3 localPosition;
			Quaternion localRotation;
			BitPackUtils.UnpackHandPosRotFromNetwork(packedPosRot, out localPosition, out localRotation);
			BuilderTable table = this.GetTable();
			if (table.GetTableState() == BuilderTable.TableState.Ready)
			{
				bool isMasterClient = info.Sender.IsMasterClient;
				bool flag = isMasterClient || table.ValidateGrabPieceParams(pieceId, isLeftHand, localPosition, localRotation, NetPlayer.Get(grabbedByPlayer));
				if (flag)
				{
					flag &= (isMasterClient || table.ValidateGrabPieceState(pieceId, isLeftHand, localPosition, localRotation, grabbedByPlayer));
				}
				if (flag)
				{
					if (!info.Sender.IsMasterClient)
					{
						this.CheckForFreedPlot(pieceId, grabbedByPlayer);
					}
					base.photonView.RPC("PieceGrabbedRPC", RpcTarget.All, new object[]
					{
						localCommandId,
						pieceId,
						isLeftHand,
						packedPosRot,
						grabbedByPlayer
					});
					return;
				}
				base.photonView.RPC("RequestFailedRPC", info.Sender, new object[]
				{
					localCommandId
				});
			}
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x00162BD0 File Offset: 0x00160DD0
		private void CheckForFreedPlot(int pieceId, Player grabbedByPlayer)
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			BuilderPiece piece = this.GetTable().GetPiece(pieceId);
			if (piece != null && piece.parentPiece != null && piece.parentPiece.IsPrivatePlot() && piece.parentPiece.firstChildPiece.Equals(piece) && piece.nextSiblingPiece == null)
			{
				base.photonView.RPC("PlotFreedRPC", RpcTarget.All, new object[]
				{
					piece.parentPiece.pieceId,
					grabbedByPlayer
				});
			}
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x00162C68 File Offset: 0x00160E68
		[PunRPC]
		public void PieceGrabbedRPC(int localCommandId, int pieceId, bool isLeftHand, long packedPosRot, Player grabbedByPlayer, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "PieceGrabbedRPC");
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.GrabPiece, info))
			{
				return;
			}
			Vector3 localPosition;
			Quaternion localRotation;
			BitPackUtils.UnpackHandPosRotFromNetwork(packedPosRot, out localPosition, out localRotation);
			this.GetTable().GrabPiece(localCommandId, pieceId, isLeftHand, localPosition, localRotation, NetPlayer.Get(grabbedByPlayer), false);
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x00162CC0 File Offset: 0x00160EC0
		public void RequestDropPiece(BuilderPiece piece, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angVelocity)
		{
			if (piece == null)
			{
				return;
			}
			int pieceId = piece.pieceId;
			float num = 10000f;
			if (velocity.IsValid(num) && velocity.sqrMagnitude > BuilderTable.MAX_DROP_VELOCITY * BuilderTable.MAX_DROP_VELOCITY)
			{
				velocity = velocity.normalized * BuilderTable.MAX_DROP_VELOCITY;
			}
			num = 10000f;
			if (angVelocity.IsValid(num) && angVelocity.sqrMagnitude > BuilderTable.MAX_DROP_ANG_VELOCITY * BuilderTable.MAX_DROP_ANG_VELOCITY)
			{
				angVelocity = angVelocity.normalized * BuilderTable.MAX_DROP_ANG_VELOCITY;
			}
			BuilderTable table = this.GetTable();
			if (!table.ValidateDropPieceParams(pieceId, position, rotation, velocity, angVelocity, NetPlayer.Get(PhotonNetwork.LocalPlayer)))
			{
				return;
			}
			int num2 = this.CreateLocalCommandId();
			table.DropPiece(num2, pieceId, position, rotation, velocity, angVelocity, NetPlayer.Get(PhotonNetwork.LocalPlayer), true);
			if (table.GetTableState() == BuilderTable.TableState.Ready)
			{
				base.photonView.RPC("RequestDropPieceRPC", RpcTarget.MasterClient, new object[]
				{
					num2,
					pieceId,
					position,
					rotation,
					velocity,
					angVelocity,
					PhotonNetwork.LocalPlayer
				});
			}
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x00162DF0 File Offset: 0x00160FF0
		[PunRPC]
		public void RequestDropPieceRPC(int localCommandId, int pieceId, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angVelocity, Player droppedByPlayer, PhotonMessageInfo info)
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "RequestDropPieceRPC");
			if (!this.ValidateMasterClientIsReady(info.Sender))
			{
				return;
			}
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.DropPieceMaster, info) || !droppedByPlayer.Equals(info.Sender))
			{
				return;
			}
			BuilderTable table = this.GetTable();
			if (table.GetTableState() != BuilderTable.TableState.Ready)
			{
				return;
			}
			bool isMasterClient = info.Sender.IsMasterClient;
			bool flag = isMasterClient || table.ValidateDropPieceParams(pieceId, position, rotation, velocity, angVelocity, NetPlayer.Get(droppedByPlayer));
			if (flag)
			{
				flag &= (isMasterClient || table.ValidateDropPieceState(pieceId, position, rotation, velocity, angVelocity, droppedByPlayer));
			}
			if (flag)
			{
				base.photonView.RPC("PieceDroppedRPC", RpcTarget.All, new object[]
				{
					localCommandId,
					pieceId,
					position,
					rotation,
					velocity,
					angVelocity,
					droppedByPlayer
				});
				return;
			}
			base.photonView.RPC("RequestFailedRPC", info.Sender, new object[]
			{
				localCommandId
			});
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x00162F14 File Offset: 0x00161114
		[PunRPC]
		public void PieceDroppedRPC(int localCommandId, int pieceId, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angVelocity, Player droppedByPlayer, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "PieceDroppedRPC");
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.DropPiece, info))
			{
				return;
			}
			float num = 10000f;
			if (position.IsValid(num) && rotation.IsValid())
			{
				float num2 = 10000f;
				if (velocity.IsValid(num2))
				{
					float num3 = 10000f;
					if (angVelocity.IsValid(num3))
					{
						this.GetTable().DropPiece(localCommandId, pieceId, position, rotation, velocity, angVelocity, NetPlayer.Get(droppedByPlayer), false);
						return;
					}
				}
			}
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x00162FA4 File Offset: 0x001611A4
		public void PieceEnteredDropZone(BuilderPiece piece, BuilderDropZone.DropType dropType, int dropZoneId)
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			BuilderTable table = this.GetTable();
			BuilderPiece rootPiece = piece.GetRootPiece();
			if (!table.ValidateRepelPiece(rootPiece))
			{
				return;
			}
			long num = BitPackUtils.PackWorldPosForNetwork(rootPiece.transform.position);
			int num2 = BitPackUtils.PackQuaternionForNetwork(rootPiece.transform.rotation);
			base.photonView.RPC("PieceEnteredDropZoneRPC", RpcTarget.All, new object[]
			{
				rootPiece.pieceId,
				num,
				num2,
				dropZoneId
			});
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x00163034 File Offset: 0x00161234
		[PunRPC]
		public void PieceEnteredDropZoneRPC(int pieceId, long position, int rotation, int dropZoneId, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "PieceEnteredDropZoneRPC");
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.PieceDropZone, info))
			{
				return;
			}
			Vector3 worldPos = BitPackUtils.UnpackWorldPosFromNetwork(position);
			float num = 10000f;
			if (!worldPos.IsValid(num))
			{
				return;
			}
			Quaternion worldRot = BitPackUtils.UnpackQuaternionFromNetwork(rotation);
			if (!worldRot.IsValid())
			{
				return;
			}
			this.GetTable().PieceEnteredDropZone(pieceId, worldPos, worldRot, dropZoneId);
		}

		// Token: 0x06003E3D RID: 15933 RVA: 0x00058762 File Offset: 0x00056962
		[PunRPC]
		public void PlotClaimedRPC(int pieceId, Player claimingPlayer, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "PlotClaimedRPC");
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.PlotClaimedMaster, info))
			{
				return;
			}
			this.GetTable().PlotClaimed(pieceId, claimingPlayer);
		}

		// Token: 0x06003E3E RID: 15934 RVA: 0x00058796 File Offset: 0x00056996
		[PunRPC]
		public void PlotFreedRPC(int pieceId, Player claimingPlayer, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "PlotFreedRPC");
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.PlotFreedMaster, info))
			{
				return;
			}
			this.GetTable().PlotFreed(pieceId, claimingPlayer);
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x001630A4 File Offset: 0x001612A4
		public void RequestCreateArmShelfForPlayer(Player player)
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			BuilderTable table = this.GetTable();
			if (table.GetTableState() != BuilderTable.TableState.Ready)
			{
				if (!this.armShelfRequests.Contains(player))
				{
					this.armShelfRequests.Add(player);
				}
				return;
			}
			if (table.playerToArmShelfLeft.ContainsKey(player.ActorNumber))
			{
				return;
			}
			int num = table.CreatePieceId();
			int num2 = table.CreatePieceId();
			int staticHash = table.armShelfPieceType.name.GetStaticHash();
			base.photonView.RPC("ArmShelfCreatedRPC", RpcTarget.All, new object[]
			{
				num,
				num2,
				staticHash,
				player
			});
		}

		// Token: 0x06003E40 RID: 15936 RVA: 0x00163150 File Offset: 0x00161350
		[PunRPC]
		public void ArmShelfCreatedRPC(int pieceIdLeft, int pieceIdRight, int pieceType, Player owningPlayer, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "ArmShelfCreatedRPC");
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.ArmShelfCreated, info))
			{
				return;
			}
			BuilderTable table = this.GetTable();
			if (pieceType != table.armShelfPieceType.name.GetStaticHash())
			{
				return;
			}
			table.CreateArmShelf(pieceIdLeft, pieceIdRight, pieceType, owningPlayer);
		}

		// Token: 0x06003E41 RID: 15937 RVA: 0x001631AC File Offset: 0x001613AC
		public void RequestShelfSelection(int shelfID, int setId, bool isConveyor)
		{
			BuilderTable table = this.GetTable();
			if (isConveyor)
			{
				if (shelfID < 0 || shelfID >= table.conveyors.Count)
				{
					return;
				}
			}
			else if (shelfID < 0 || shelfID >= table.dispenserShelves.Count)
			{
				return;
			}
			if (table.GetTableState() == BuilderTable.TableState.Ready)
			{
				base.photonView.RPC("RequestShelfSelectionRPC", RpcTarget.MasterClient, new object[]
				{
					shelfID,
					setId,
					isConveyor
				});
			}
		}

		// Token: 0x06003E42 RID: 15938 RVA: 0x00163224 File Offset: 0x00161424
		[PunRPC]
		public void RequestShelfSelectionRPC(int shelfId, int setId, bool isConveyor, PhotonMessageInfo info)
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "RequestShelfSelectionRPC");
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.ShelfSelection, info))
			{
				return;
			}
			if (!this.ValidateMasterClientIsReady(info.Sender))
			{
				return;
			}
			if (!this.GetTable().ValidateShelfSelectionParams(shelfId, setId, isConveyor, info.Sender))
			{
				return;
			}
			base.photonView.RPC("ShelfSelectionChangedRPC", RpcTarget.All, new object[]
			{
				shelfId,
				setId,
				isConveyor,
				info.Sender
			});
		}

		// Token: 0x06003E43 RID: 15939 RVA: 0x001632B8 File Offset: 0x001614B8
		[PunRPC]
		public void ShelfSelectionChangedRPC(int shelfId, int setId, bool isConveyor, Player caller, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "ShelfSelectionChangedRPC");
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.ShelfSelectionMaster, info))
			{
				return;
			}
			BuilderTable table = this.GetTable();
			if (shelfId < 0 || ((!isConveyor || shelfId >= table.conveyors.Count) && (isConveyor || shelfId >= table.dispenserShelves.Count)))
			{
				return;
			}
			table.ChangeSetSelection(shelfId, setId, isConveyor);
		}

		// Token: 0x06003E44 RID: 15940 RVA: 0x00163330 File Offset: 0x00161530
		public void RequestFunctionalPieceStateChange(int pieceID, byte state)
		{
			BuilderTable table = this.GetTable();
			if (!table.ValidateFunctionalPieceState(pieceID, state, NetworkSystem.Instance.LocalPlayer))
			{
				return;
			}
			if (table.GetTableState() == BuilderTable.TableState.Ready)
			{
				base.photonView.RPC("RequestFunctionalPieceStateChangeRPC", RpcTarget.MasterClient, new object[]
				{
					pieceID,
					state
				});
			}
		}

		// Token: 0x06003E45 RID: 15941 RVA: 0x0016338C File Offset: 0x0016158C
		[PunRPC]
		public void RequestFunctionalPieceStateChangeRPC(int pieceID, byte state, PhotonMessageInfo info)
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "RequestFunctionalPieceStateChangeRPC");
			if (!this.ValidateMasterClientIsReady(info.Sender))
			{
				return;
			}
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.SetFunctionalState, info))
			{
				return;
			}
			BuilderTable table = this.GetTable();
			if (table.GetTableState() != BuilderTable.TableState.Ready)
			{
				return;
			}
			if (table.ValidateFunctionalPieceState(pieceID, state, NetPlayer.Get(info.Sender)))
			{
				table.OnFunctionalStateRequest(pieceID, state, NetPlayer.Get(info.Sender), info.SentServerTimestamp);
			}
		}

		// Token: 0x06003E46 RID: 15942 RVA: 0x00163408 File Offset: 0x00161608
		public void FunctionalPieceStateChangeMaster(int pieceID, byte state, Player instigator, int timeStamp)
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			BuilderTable table = this.GetTable();
			if (table.ValidateFunctionalPieceState(pieceID, state, NetPlayer.Get(instigator)) && state != table.GetPiece(pieceID).functionalPieceState)
			{
				base.photonView.RPC("FunctionalPieceStateChangeRPC", RpcTarget.All, new object[]
				{
					pieceID,
					state,
					instigator,
					timeStamp
				});
			}
		}

		// Token: 0x06003E47 RID: 15943 RVA: 0x0016347C File Offset: 0x0016167C
		[PunRPC]
		public void FunctionalPieceStateChangeRPC(int pieceID, byte state, Player caller, int timeStamp, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "FunctionalPieceStateChangeRPC");
			if (!this.ValidateCallLimits(BuilderTableNetworking.RPC.SetFunctionalStateMaster, info))
			{
				return;
			}
			if (caller == null)
			{
				return;
			}
			if ((ulong)(PhotonNetwork.ServerTimestamp - info.SentServerTimestamp) > (ulong)((long)PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout) || (ulong)(info.SentServerTimestamp - timeStamp) > (ulong)((long)PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout))
			{
				timeStamp = PhotonNetwork.ServerTimestamp;
			}
			BuilderTable table = this.GetTable();
			if (table.ValidateFunctionalPieceState(pieceID, state, NetPlayer.Get(info.Sender)))
			{
				table.SetFunctionalPieceState(pieceID, state, NetPlayer.Get(caller), timeStamp);
			}
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x00030607 File Offset: 0x0002E807
		public void RequestPaintPiece(int pieceID, int materialType)
		{
		}

		// Token: 0x04003F35 RID: 16181
		public static BuilderTableNetworking instance;

		// Token: 0x04003F36 RID: 16182
		public PhotonView tablePhotonView;

		// Token: 0x04003F37 RID: 16183
		private const int MAX_TABLE_BYTES = 1048576;

		// Token: 0x04003F38 RID: 16184
		private const int MAX_TABLE_CHUNK_BYTES = 1000;

		// Token: 0x04003F39 RID: 16185
		private const float DELAY_CLIENT_TABLE_CREATION_TIME = 1f;

		// Token: 0x04003F3A RID: 16186
		private const float SEND_INIT_DATA_COOLDOWN = 0f;

		// Token: 0x04003F3B RID: 16187
		private const int PIECE_SYNC_BYTES = 128;

		// Token: 0x04003F3C RID: 16188
		private BuilderTable currTable;

		// Token: 0x04003F3D RID: 16189
		private int nextLocalCommandId;

		// Token: 0x04003F3E RID: 16190
		private List<BuilderTableNetworking.PlayerTableInitState> masterClientTableInit;

		// Token: 0x04003F3F RID: 16191
		private List<BuilderTableNetworking.PlayerTableInitState> masterClientTableValidators;

		// Token: 0x04003F40 RID: 16192
		private BuilderTableNetworking.PlayerTableInitState localClientTableInit;

		// Token: 0x04003F41 RID: 16193
		private BuilderTableNetworking.PlayerTableInitState localValidationTable;

		// Token: 0x04003F42 RID: 16194
		[HideInInspector]
		public List<Player> armShelfRequests;

		// Token: 0x04003F43 RID: 16195
		private CallLimiter[] callLimiters;

		// Token: 0x020009CB RID: 2507
		public class PlayerTableInitState
		{
			// Token: 0x06003E4A RID: 15946 RVA: 0x000587CA File Offset: 0x000569CA
			public PlayerTableInitState()
			{
				this.serializedTableState = new byte[1048576];
				this.chunk = new byte[1000];
				this.Reset();
			}

			// Token: 0x06003E4B RID: 15947 RVA: 0x000587F8 File Offset: 0x000569F8
			public void Reset()
			{
				this.player = null;
				this.numSerializedBytes = 0;
				this.totalSerializedBytes = 0;
			}

			// Token: 0x04003F44 RID: 16196
			public Player player;

			// Token: 0x04003F45 RID: 16197
			public int numSerializedBytes;

			// Token: 0x04003F46 RID: 16198
			public int totalSerializedBytes;

			// Token: 0x04003F47 RID: 16199
			public byte[] serializedTableState;

			// Token: 0x04003F48 RID: 16200
			public byte[] chunk;

			// Token: 0x04003F49 RID: 16201
			public float waitForInitTimeRemaining;

			// Token: 0x04003F4A RID: 16202
			public float sendNextChunkTimeRemaining;
		}

		// Token: 0x020009CC RID: 2508
		private enum RPC
		{
			// Token: 0x04003F4C RID: 16204
			PlayerEnterMaster,
			// Token: 0x04003F4D RID: 16205
			PlayerExitMaster,
			// Token: 0x04003F4E RID: 16206
			TableDataMaster,
			// Token: 0x04003F4F RID: 16207
			TableData,
			// Token: 0x04003F50 RID: 16208
			TableDataStart,
			// Token: 0x04003F51 RID: 16209
			PlacePieceMaster,
			// Token: 0x04003F52 RID: 16210
			PlacePiece,
			// Token: 0x04003F53 RID: 16211
			GrabPieceMaster,
			// Token: 0x04003F54 RID: 16212
			GrabPiece,
			// Token: 0x04003F55 RID: 16213
			DropPieceMaster,
			// Token: 0x04003F56 RID: 16214
			DropPiece,
			// Token: 0x04003F57 RID: 16215
			RequestFailed,
			// Token: 0x04003F58 RID: 16216
			PieceDropZone,
			// Token: 0x04003F59 RID: 16217
			CreatePiece,
			// Token: 0x04003F5A RID: 16218
			CreatePieceMaster,
			// Token: 0x04003F5B RID: 16219
			CreateShelfPieceMaster,
			// Token: 0x04003F5C RID: 16220
			RecyclePieceMaster,
			// Token: 0x04003F5D RID: 16221
			PlotClaimedMaster,
			// Token: 0x04003F5E RID: 16222
			PlotFreedMaster,
			// Token: 0x04003F5F RID: 16223
			ArmShelfCreated,
			// Token: 0x04003F60 RID: 16224
			ShelfSelection,
			// Token: 0x04003F61 RID: 16225
			ShelfSelectionMaster,
			// Token: 0x04003F62 RID: 16226
			SetFunctionalState,
			// Token: 0x04003F63 RID: 16227
			SetFunctionalStateMaster,
			// Token: 0x04003F64 RID: 16228
			ClientStartValidationClient,
			// Token: 0x04003F65 RID: 16229
			StartBuildingClientsValidationTableMaster,
			// Token: 0x04003F66 RID: 16230
			RequestTableDataValidateClient,
			// Token: 0x04003F67 RID: 16231
			SendValidationTableDataMaster,
			// Token: 0x04003F68 RID: 16232
			Count
		}
	}
}
