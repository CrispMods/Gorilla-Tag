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
	// Token: 0x020009A4 RID: 2468
	public class BuilderTableNetworking : MonoBehaviourPunCallbacks
	{
		// Token: 0x06003CF6 RID: 15606 RVA: 0x0011F21C File Offset: 0x0011D41C
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

		// Token: 0x06003CF7 RID: 15607 RVA: 0x0011F516 File Offset: 0x0011D716
		public void SetTable(BuilderTable table)
		{
			this.currTable = table;
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x0011F51F File Offset: 0x0011D71F
		private BuilderTable GetTable()
		{
			return this.currTable;
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x0011F527 File Offset: 0x0011D727
		private int CreateLocalCommandId()
		{
			int result = this.nextLocalCommandId;
			this.nextLocalCommandId++;
			return result;
		}

		// Token: 0x06003CFA RID: 15610 RVA: 0x0011F53D File Offset: 0x0011D73D
		public BuilderTableNetworking.PlayerTableInitState GetLocalTableInit()
		{
			return this.localClientTableInit;
		}

		// Token: 0x06003CFB RID: 15611 RVA: 0x0011F548 File Offset: 0x0011D748
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

		// Token: 0x06003CFC RID: 15612 RVA: 0x0011F784 File Offset: 0x0011D984
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

		// Token: 0x06003CFD RID: 15613 RVA: 0x0011F80F File Offset: 0x0011DA0F
		public override void OnJoinedRoom()
		{
			base.OnJoinedRoom();
			this.GetTable().SetInRoom(true);
		}

		// Token: 0x06003CFE RID: 15614 RVA: 0x0011F823 File Offset: 0x0011DA23
		public override void OnLeftRoom()
		{
			this.PlayerExitBuilder();
			this.GetTable().SetInRoom(false);
			this.armShelfRequests.Clear();
		}

		// Token: 0x06003CFF RID: 15615 RVA: 0x0011F842 File Offset: 0x0011DA42
		private void Update()
		{
			if (PhotonNetwork.IsMasterClient)
			{
				this.UpdateNewPlayerInit();
			}
		}

		// Token: 0x06003D00 RID: 15616 RVA: 0x0011F854 File Offset: 0x0011DA54
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

		// Token: 0x06003D01 RID: 15617 RVA: 0x0011F8BC File Offset: 0x0011DABC
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

		// Token: 0x06003D02 RID: 15618 RVA: 0x0011F934 File Offset: 0x0011DB34
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

		// Token: 0x06003D03 RID: 15619 RVA: 0x0011F998 File Offset: 0x0011DB98
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

		// Token: 0x06003D04 RID: 15620 RVA: 0x0011F9FA File Offset: 0x0011DBFA
		public bool IsPrivateMasterClient()
		{
			return PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient && NetworkSystem.Instance.SessionIsPrivate;
		}

		// Token: 0x06003D05 RID: 15621 RVA: 0x0011FA14 File Offset: 0x0011DC14
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

		// Token: 0x06003D06 RID: 15622 RVA: 0x0011FBA4 File Offset: 0x0011DDA4
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

		// Token: 0x06003D07 RID: 15623 RVA: 0x0011FC2C File Offset: 0x0011DE2C
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

		// Token: 0x06003D08 RID: 15624 RVA: 0x0011FCE4 File Offset: 0x0011DEE4
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

		// Token: 0x06003D09 RID: 15625 RVA: 0x0011FD88 File Offset: 0x0011DF88
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

		// Token: 0x06003D0A RID: 15626 RVA: 0x0011FE70 File Offset: 0x0011E070
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

		// Token: 0x06003D0B RID: 15627 RVA: 0x0011FEB4 File Offset: 0x0011E0B4
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

		// Token: 0x06003D0C RID: 15628 RVA: 0x0011FF2D File Offset: 0x0011E12D
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

		// Token: 0x06003D0D RID: 15629 RVA: 0x0011FF58 File Offset: 0x0011E158
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

		// Token: 0x06003D0E RID: 15630 RVA: 0x0011FFAC File Offset: 0x0011E1AC
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

		// Token: 0x06003D0F RID: 15631 RVA: 0x0011FFFC File Offset: 0x0011E1FC
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

		// Token: 0x06003D10 RID: 15632 RVA: 0x0012004C File Offset: 0x0011E24C
		private bool ValidateCallLimits(BuilderTableNetworking.RPC rpcCall, PhotonMessageInfo info)
		{
			return rpcCall >= BuilderTableNetworking.RPC.PlayerEnterMaster && rpcCall < BuilderTableNetworking.RPC.Count && this.callLimiters[(int)rpcCall].CheckCallTime(Time.time);
		}

		// Token: 0x06003D11 RID: 15633 RVA: 0x0012007A File Offset: 0x0011E27A
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

		// Token: 0x06003D12 RID: 15634 RVA: 0x000023F4 File Offset: 0x000005F4
		public void RequestCreatePiece(int newPieceType, Vector3 position, Quaternion rotation, int materialType)
		{
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x000023F4 File Offset: 0x000005F4
		public void RequestCreatePieceRPC(int newPieceType, long packedPosition, int packedRotation, int materialType, PhotonMessageInfo info)
		{
		}

		// Token: 0x06003D14 RID: 15636 RVA: 0x000023F4 File Offset: 0x000005F4
		public void PieceCreatedRPC(int pieceType, int pieceId, long packedPosition, int packedRotation, int materialType, Player creatingPlayer, PhotonMessageInfo info)
		{
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x001200B0 File Offset: 0x0011E2B0
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

		// Token: 0x06003D16 RID: 15638 RVA: 0x001201A0 File Offset: 0x0011E3A0
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

		// Token: 0x06003D17 RID: 15639 RVA: 0x0012021C File Offset: 0x0011E41C
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

		// Token: 0x06003D18 RID: 15640 RVA: 0x001202C0 File Offset: 0x0011E4C0
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

		// Token: 0x06003D19 RID: 15641 RVA: 0x00120338 File Offset: 0x0011E538
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

		// Token: 0x06003D1A RID: 15642 RVA: 0x0012045C File Offset: 0x0011E65C
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

		// Token: 0x06003D1B RID: 15643 RVA: 0x00120600 File Offset: 0x0011E800
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

		// Token: 0x06003D1C RID: 15644 RVA: 0x001206AC File Offset: 0x0011E8AC
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

		// Token: 0x06003D1D RID: 15645 RVA: 0x0012077C File Offset: 0x0011E97C
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

		// Token: 0x06003D1E RID: 15646 RVA: 0x001208AC File Offset: 0x0011EAAC
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

		// Token: 0x06003D1F RID: 15647 RVA: 0x00120944 File Offset: 0x0011EB44
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

		// Token: 0x06003D20 RID: 15648 RVA: 0x0012099C File Offset: 0x0011EB9C
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

		// Token: 0x06003D21 RID: 15649 RVA: 0x00120ACC File Offset: 0x0011ECCC
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

		// Token: 0x06003D22 RID: 15650 RVA: 0x00120BF0 File Offset: 0x0011EDF0
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

		// Token: 0x06003D23 RID: 15651 RVA: 0x00120C80 File Offset: 0x0011EE80
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

		// Token: 0x06003D24 RID: 15652 RVA: 0x00120D10 File Offset: 0x0011EF10
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

		// Token: 0x06003D25 RID: 15653 RVA: 0x00120D7F File Offset: 0x0011EF7F
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

		// Token: 0x06003D26 RID: 15654 RVA: 0x00120DB3 File Offset: 0x0011EFB3
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

		// Token: 0x06003D27 RID: 15655 RVA: 0x00120DE8 File Offset: 0x0011EFE8
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

		// Token: 0x06003D28 RID: 15656 RVA: 0x00120E94 File Offset: 0x0011F094
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

		// Token: 0x06003D29 RID: 15657 RVA: 0x00120EF0 File Offset: 0x0011F0F0
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

		// Token: 0x06003D2A RID: 15658 RVA: 0x00120F68 File Offset: 0x0011F168
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

		// Token: 0x06003D2B RID: 15659 RVA: 0x00120FFC File Offset: 0x0011F1FC
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

		// Token: 0x06003D2C RID: 15660 RVA: 0x00121074 File Offset: 0x0011F274
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

		// Token: 0x06003D2D RID: 15661 RVA: 0x001210D0 File Offset: 0x0011F2D0
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

		// Token: 0x06003D2E RID: 15662 RVA: 0x0012114C File Offset: 0x0011F34C
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

		// Token: 0x06003D2F RID: 15663 RVA: 0x001211C0 File Offset: 0x0011F3C0
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

		// Token: 0x06003D30 RID: 15664 RVA: 0x000023F4 File Offset: 0x000005F4
		public void RequestPaintPiece(int pieceID, int materialType)
		{
		}

		// Token: 0x04003E5B RID: 15963
		public static BuilderTableNetworking instance;

		// Token: 0x04003E5C RID: 15964
		public PhotonView tablePhotonView;

		// Token: 0x04003E5D RID: 15965
		private const int MAX_TABLE_BYTES = 1048576;

		// Token: 0x04003E5E RID: 15966
		private const int MAX_TABLE_CHUNK_BYTES = 1000;

		// Token: 0x04003E5F RID: 15967
		private const float DELAY_CLIENT_TABLE_CREATION_TIME = 1f;

		// Token: 0x04003E60 RID: 15968
		private const float SEND_INIT_DATA_COOLDOWN = 0f;

		// Token: 0x04003E61 RID: 15969
		private const int PIECE_SYNC_BYTES = 128;

		// Token: 0x04003E62 RID: 15970
		private BuilderTable currTable;

		// Token: 0x04003E63 RID: 15971
		private int nextLocalCommandId;

		// Token: 0x04003E64 RID: 15972
		private List<BuilderTableNetworking.PlayerTableInitState> masterClientTableInit;

		// Token: 0x04003E65 RID: 15973
		private List<BuilderTableNetworking.PlayerTableInitState> masterClientTableValidators;

		// Token: 0x04003E66 RID: 15974
		private BuilderTableNetworking.PlayerTableInitState localClientTableInit;

		// Token: 0x04003E67 RID: 15975
		private BuilderTableNetworking.PlayerTableInitState localValidationTable;

		// Token: 0x04003E68 RID: 15976
		[HideInInspector]
		public List<Player> armShelfRequests;

		// Token: 0x04003E69 RID: 15977
		private CallLimiter[] callLimiters;

		// Token: 0x020009A5 RID: 2469
		public class PlayerTableInitState
		{
			// Token: 0x06003D32 RID: 15666 RVA: 0x0012126A File Offset: 0x0011F46A
			public PlayerTableInitState()
			{
				this.serializedTableState = new byte[1048576];
				this.chunk = new byte[1000];
				this.Reset();
			}

			// Token: 0x06003D33 RID: 15667 RVA: 0x00121298 File Offset: 0x0011F498
			public void Reset()
			{
				this.player = null;
				this.numSerializedBytes = 0;
				this.totalSerializedBytes = 0;
			}

			// Token: 0x04003E6A RID: 15978
			public Player player;

			// Token: 0x04003E6B RID: 15979
			public int numSerializedBytes;

			// Token: 0x04003E6C RID: 15980
			public int totalSerializedBytes;

			// Token: 0x04003E6D RID: 15981
			public byte[] serializedTableState;

			// Token: 0x04003E6E RID: 15982
			public byte[] chunk;

			// Token: 0x04003E6F RID: 15983
			public float waitForInitTimeRemaining;

			// Token: 0x04003E70 RID: 15984
			public float sendNextChunkTimeRemaining;
		}

		// Token: 0x020009A6 RID: 2470
		private enum RPC
		{
			// Token: 0x04003E72 RID: 15986
			PlayerEnterMaster,
			// Token: 0x04003E73 RID: 15987
			PlayerExitMaster,
			// Token: 0x04003E74 RID: 15988
			TableDataMaster,
			// Token: 0x04003E75 RID: 15989
			TableData,
			// Token: 0x04003E76 RID: 15990
			TableDataStart,
			// Token: 0x04003E77 RID: 15991
			PlacePieceMaster,
			// Token: 0x04003E78 RID: 15992
			PlacePiece,
			// Token: 0x04003E79 RID: 15993
			GrabPieceMaster,
			// Token: 0x04003E7A RID: 15994
			GrabPiece,
			// Token: 0x04003E7B RID: 15995
			DropPieceMaster,
			// Token: 0x04003E7C RID: 15996
			DropPiece,
			// Token: 0x04003E7D RID: 15997
			RequestFailed,
			// Token: 0x04003E7E RID: 15998
			PieceDropZone,
			// Token: 0x04003E7F RID: 15999
			CreatePiece,
			// Token: 0x04003E80 RID: 16000
			CreatePieceMaster,
			// Token: 0x04003E81 RID: 16001
			CreateShelfPieceMaster,
			// Token: 0x04003E82 RID: 16002
			RecyclePieceMaster,
			// Token: 0x04003E83 RID: 16003
			PlotClaimedMaster,
			// Token: 0x04003E84 RID: 16004
			PlotFreedMaster,
			// Token: 0x04003E85 RID: 16005
			ArmShelfCreated,
			// Token: 0x04003E86 RID: 16006
			ShelfSelection,
			// Token: 0x04003E87 RID: 16007
			ShelfSelectionMaster,
			// Token: 0x04003E88 RID: 16008
			SetFunctionalState,
			// Token: 0x04003E89 RID: 16009
			SetFunctionalStateMaster,
			// Token: 0x04003E8A RID: 16010
			ClientStartValidationClient,
			// Token: 0x04003E8B RID: 16011
			StartBuildingClientsValidationTableMaster,
			// Token: 0x04003E8C RID: 16012
			RequestTableDataValidateClient,
			// Token: 0x04003E8D RID: 16013
			SendValidationTableDataMaster,
			// Token: 0x04003E8E RID: 16014
			Count
		}
	}
}
