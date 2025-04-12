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
	// Token: 0x020009A7 RID: 2471
	public class BuilderTableNetworking : MonoBehaviourPunCallbacks
	{
		// Token: 0x06003D02 RID: 15618 RVA: 0x0015B640 File Offset: 0x00159840
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

		// Token: 0x06003D03 RID: 15619 RVA: 0x00056DE5 File Offset: 0x00054FE5
		public void SetTable(BuilderTable table)
		{
			this.currTable = table;
		}

		// Token: 0x06003D04 RID: 15620 RVA: 0x00056DEE File Offset: 0x00054FEE
		private BuilderTable GetTable()
		{
			return this.currTable;
		}

		// Token: 0x06003D05 RID: 15621 RVA: 0x00056DF6 File Offset: 0x00054FF6
		private int CreateLocalCommandId()
		{
			int result = this.nextLocalCommandId;
			this.nextLocalCommandId++;
			return result;
		}

		// Token: 0x06003D06 RID: 15622 RVA: 0x00056E0C File Offset: 0x0005500C
		public BuilderTableNetworking.PlayerTableInitState GetLocalTableInit()
		{
			return this.localClientTableInit;
		}

		// Token: 0x06003D07 RID: 15623 RVA: 0x0015B93C File Offset: 0x00159B3C
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

		// Token: 0x06003D08 RID: 15624 RVA: 0x0015BB78 File Offset: 0x00159D78
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

		// Token: 0x06003D09 RID: 15625 RVA: 0x00056E14 File Offset: 0x00055014
		public override void OnJoinedRoom()
		{
			base.OnJoinedRoom();
			this.GetTable().SetInRoom(true);
		}

		// Token: 0x06003D0A RID: 15626 RVA: 0x00056E28 File Offset: 0x00055028
		public override void OnLeftRoom()
		{
			this.PlayerExitBuilder();
			this.GetTable().SetInRoom(false);
			this.armShelfRequests.Clear();
		}

		// Token: 0x06003D0B RID: 15627 RVA: 0x00056E47 File Offset: 0x00055047
		private void Update()
		{
			if (PhotonNetwork.IsMasterClient)
			{
				this.UpdateNewPlayerInit();
			}
		}

		// Token: 0x06003D0C RID: 15628 RVA: 0x0015BC04 File Offset: 0x00159E04
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

		// Token: 0x06003D0D RID: 15629 RVA: 0x0015BC6C File Offset: 0x00159E6C
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

		// Token: 0x06003D0E RID: 15630 RVA: 0x0015BCE4 File Offset: 0x00159EE4
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

		// Token: 0x06003D0F RID: 15631 RVA: 0x0015BD48 File Offset: 0x00159F48
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

		// Token: 0x06003D10 RID: 15632 RVA: 0x00056E56 File Offset: 0x00055056
		public bool IsPrivateMasterClient()
		{
			return PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient && NetworkSystem.Instance.SessionIsPrivate;
		}

		// Token: 0x06003D11 RID: 15633 RVA: 0x0015BDAC File Offset: 0x00159FAC
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

		// Token: 0x06003D12 RID: 15634 RVA: 0x0015BF3C File Offset: 0x0015A13C
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

		// Token: 0x06003D13 RID: 15635 RVA: 0x0015BFC4 File Offset: 0x0015A1C4
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

		// Token: 0x06003D14 RID: 15636 RVA: 0x0015C07C File Offset: 0x0015A27C
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

		// Token: 0x06003D15 RID: 15637 RVA: 0x0015C120 File Offset: 0x0015A320
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

		// Token: 0x06003D16 RID: 15638 RVA: 0x0015C208 File Offset: 0x0015A408
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

		// Token: 0x06003D17 RID: 15639 RVA: 0x0015C24C File Offset: 0x0015A44C
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

		// Token: 0x06003D18 RID: 15640 RVA: 0x00056E70 File Offset: 0x00055070
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

		// Token: 0x06003D19 RID: 15641 RVA: 0x0015C2C8 File Offset: 0x0015A4C8
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

		// Token: 0x06003D1A RID: 15642 RVA: 0x0015C31C File Offset: 0x0015A51C
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

		// Token: 0x06003D1B RID: 15643 RVA: 0x0015C36C File Offset: 0x0015A56C
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

		// Token: 0x06003D1C RID: 15644 RVA: 0x0015C3BC File Offset: 0x0015A5BC
		private bool ValidateCallLimits(BuilderTableNetworking.RPC rpcCall, PhotonMessageInfo info)
		{
			return rpcCall >= BuilderTableNetworking.RPC.PlayerEnterMaster && rpcCall < BuilderTableNetworking.RPC.Count && this.callLimiters[(int)rpcCall].CheckCallTime(Time.time);
		}

		// Token: 0x06003D1D RID: 15645 RVA: 0x00056E98 File Offset: 0x00055098
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

		// Token: 0x06003D1E RID: 15646 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void RequestCreatePiece(int newPieceType, Vector3 position, Quaternion rotation, int materialType)
		{
		}

		// Token: 0x06003D1F RID: 15647 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void RequestCreatePieceRPC(int newPieceType, long packedPosition, int packedRotation, int materialType, PhotonMessageInfo info)
		{
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void PieceCreatedRPC(int pieceType, int pieceId, long packedPosition, int packedRotation, int materialType, Player creatingPlayer, PhotonMessageInfo info)
		{
		}

		// Token: 0x06003D21 RID: 15649 RVA: 0x0015C3EC File Offset: 0x0015A5EC
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

		// Token: 0x06003D22 RID: 15650 RVA: 0x0015C4DC File Offset: 0x0015A6DC
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

		// Token: 0x06003D23 RID: 15651 RVA: 0x0015C558 File Offset: 0x0015A758
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

		// Token: 0x06003D24 RID: 15652 RVA: 0x0015C5FC File Offset: 0x0015A7FC
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

		// Token: 0x06003D25 RID: 15653 RVA: 0x0015C674 File Offset: 0x0015A874
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

		// Token: 0x06003D26 RID: 15654 RVA: 0x0015C798 File Offset: 0x0015A998
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

		// Token: 0x06003D27 RID: 15655 RVA: 0x0015C93C File Offset: 0x0015AB3C
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

		// Token: 0x06003D28 RID: 15656 RVA: 0x0015C9E8 File Offset: 0x0015ABE8
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

		// Token: 0x06003D29 RID: 15657 RVA: 0x0015CAB8 File Offset: 0x0015ACB8
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

		// Token: 0x06003D2A RID: 15658 RVA: 0x0015CBE8 File Offset: 0x0015ADE8
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

		// Token: 0x06003D2B RID: 15659 RVA: 0x0015CC80 File Offset: 0x0015AE80
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

		// Token: 0x06003D2C RID: 15660 RVA: 0x0015CCD8 File Offset: 0x0015AED8
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

		// Token: 0x06003D2D RID: 15661 RVA: 0x0015CE08 File Offset: 0x0015B008
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

		// Token: 0x06003D2E RID: 15662 RVA: 0x0015CF2C File Offset: 0x0015B12C
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

		// Token: 0x06003D2F RID: 15663 RVA: 0x0015CFBC File Offset: 0x0015B1BC
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

		// Token: 0x06003D30 RID: 15664 RVA: 0x0015D04C File Offset: 0x0015B24C
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

		// Token: 0x06003D31 RID: 15665 RVA: 0x00056ECB File Offset: 0x000550CB
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

		// Token: 0x06003D32 RID: 15666 RVA: 0x00056EFF File Offset: 0x000550FF
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

		// Token: 0x06003D33 RID: 15667 RVA: 0x0015D0BC File Offset: 0x0015B2BC
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

		// Token: 0x06003D34 RID: 15668 RVA: 0x0015D168 File Offset: 0x0015B368
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

		// Token: 0x06003D35 RID: 15669 RVA: 0x0015D1C4 File Offset: 0x0015B3C4
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

		// Token: 0x06003D36 RID: 15670 RVA: 0x0015D23C File Offset: 0x0015B43C
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

		// Token: 0x06003D37 RID: 15671 RVA: 0x0015D2D0 File Offset: 0x0015B4D0
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

		// Token: 0x06003D38 RID: 15672 RVA: 0x0015D348 File Offset: 0x0015B548
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

		// Token: 0x06003D39 RID: 15673 RVA: 0x0015D3A4 File Offset: 0x0015B5A4
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

		// Token: 0x06003D3A RID: 15674 RVA: 0x0015D420 File Offset: 0x0015B620
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

		// Token: 0x06003D3B RID: 15675 RVA: 0x0015D494 File Offset: 0x0015B694
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

		// Token: 0x06003D3C RID: 15676 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void RequestPaintPiece(int pieceID, int materialType)
		{
		}

		// Token: 0x04003E6D RID: 15981
		public static BuilderTableNetworking instance;

		// Token: 0x04003E6E RID: 15982
		public PhotonView tablePhotonView;

		// Token: 0x04003E6F RID: 15983
		private const int MAX_TABLE_BYTES = 1048576;

		// Token: 0x04003E70 RID: 15984
		private const int MAX_TABLE_CHUNK_BYTES = 1000;

		// Token: 0x04003E71 RID: 15985
		private const float DELAY_CLIENT_TABLE_CREATION_TIME = 1f;

		// Token: 0x04003E72 RID: 15986
		private const float SEND_INIT_DATA_COOLDOWN = 0f;

		// Token: 0x04003E73 RID: 15987
		private const int PIECE_SYNC_BYTES = 128;

		// Token: 0x04003E74 RID: 15988
		private BuilderTable currTable;

		// Token: 0x04003E75 RID: 15989
		private int nextLocalCommandId;

		// Token: 0x04003E76 RID: 15990
		private List<BuilderTableNetworking.PlayerTableInitState> masterClientTableInit;

		// Token: 0x04003E77 RID: 15991
		private List<BuilderTableNetworking.PlayerTableInitState> masterClientTableValidators;

		// Token: 0x04003E78 RID: 15992
		private BuilderTableNetworking.PlayerTableInitState localClientTableInit;

		// Token: 0x04003E79 RID: 15993
		private BuilderTableNetworking.PlayerTableInitState localValidationTable;

		// Token: 0x04003E7A RID: 15994
		[HideInInspector]
		public List<Player> armShelfRequests;

		// Token: 0x04003E7B RID: 15995
		private CallLimiter[] callLimiters;

		// Token: 0x020009A8 RID: 2472
		public class PlayerTableInitState
		{
			// Token: 0x06003D3E RID: 15678 RVA: 0x00056F33 File Offset: 0x00055133
			public PlayerTableInitState()
			{
				this.serializedTableState = new byte[1048576];
				this.chunk = new byte[1000];
				this.Reset();
			}

			// Token: 0x06003D3F RID: 15679 RVA: 0x00056F61 File Offset: 0x00055161
			public void Reset()
			{
				this.player = null;
				this.numSerializedBytes = 0;
				this.totalSerializedBytes = 0;
			}

			// Token: 0x04003E7C RID: 15996
			public Player player;

			// Token: 0x04003E7D RID: 15997
			public int numSerializedBytes;

			// Token: 0x04003E7E RID: 15998
			public int totalSerializedBytes;

			// Token: 0x04003E7F RID: 15999
			public byte[] serializedTableState;

			// Token: 0x04003E80 RID: 16000
			public byte[] chunk;

			// Token: 0x04003E81 RID: 16001
			public float waitForInitTimeRemaining;

			// Token: 0x04003E82 RID: 16002
			public float sendNextChunkTimeRemaining;
		}

		// Token: 0x020009A9 RID: 2473
		private enum RPC
		{
			// Token: 0x04003E84 RID: 16004
			PlayerEnterMaster,
			// Token: 0x04003E85 RID: 16005
			PlayerExitMaster,
			// Token: 0x04003E86 RID: 16006
			TableDataMaster,
			// Token: 0x04003E87 RID: 16007
			TableData,
			// Token: 0x04003E88 RID: 16008
			TableDataStart,
			// Token: 0x04003E89 RID: 16009
			PlacePieceMaster,
			// Token: 0x04003E8A RID: 16010
			PlacePiece,
			// Token: 0x04003E8B RID: 16011
			GrabPieceMaster,
			// Token: 0x04003E8C RID: 16012
			GrabPiece,
			// Token: 0x04003E8D RID: 16013
			DropPieceMaster,
			// Token: 0x04003E8E RID: 16014
			DropPiece,
			// Token: 0x04003E8F RID: 16015
			RequestFailed,
			// Token: 0x04003E90 RID: 16016
			PieceDropZone,
			// Token: 0x04003E91 RID: 16017
			CreatePiece,
			// Token: 0x04003E92 RID: 16018
			CreatePieceMaster,
			// Token: 0x04003E93 RID: 16019
			CreateShelfPieceMaster,
			// Token: 0x04003E94 RID: 16020
			RecyclePieceMaster,
			// Token: 0x04003E95 RID: 16021
			PlotClaimedMaster,
			// Token: 0x04003E96 RID: 16022
			PlotFreedMaster,
			// Token: 0x04003E97 RID: 16023
			ArmShelfCreated,
			// Token: 0x04003E98 RID: 16024
			ShelfSelection,
			// Token: 0x04003E99 RID: 16025
			ShelfSelectionMaster,
			// Token: 0x04003E9A RID: 16026
			SetFunctionalState,
			// Token: 0x04003E9B RID: 16027
			SetFunctionalStateMaster,
			// Token: 0x04003E9C RID: 16028
			ClientStartValidationClient,
			// Token: 0x04003E9D RID: 16029
			StartBuildingClientsValidationTableMaster,
			// Token: 0x04003E9E RID: 16030
			RequestTableDataValidateClient,
			// Token: 0x04003E9F RID: 16031
			SendValidationTableDataMaster,
			// Token: 0x04003EA0 RID: 16032
			Count
		}
	}
}
