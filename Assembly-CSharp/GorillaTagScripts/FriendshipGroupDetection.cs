using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fusion;
using GorillaExtensions;
using GorillaGameModes;
using GorillaNetworking;
using GorillaTag;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Scripting;

namespace GorillaTagScripts
{
	// Token: 0x020009DC RID: 2524
	public class FriendshipGroupDetection : NetworkSceneObject
	{
		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06003EB8 RID: 16056 RVA: 0x00058E0C File Offset: 0x0005700C
		// (set) Token: 0x06003EB9 RID: 16057 RVA: 0x00058E13 File Offset: 0x00057013
		public static FriendshipGroupDetection Instance { get; private set; }

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06003EBA RID: 16058 RVA: 0x00058E1B File Offset: 0x0005701B
		// (set) Token: 0x06003EBB RID: 16059 RVA: 0x00058E23 File Offset: 0x00057023
		public List<Color> myBeadColors { get; private set; } = new List<Color>();

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06003EBC RID: 16060 RVA: 0x00058E2C File Offset: 0x0005702C
		// (set) Token: 0x06003EBD RID: 16061 RVA: 0x00058E34 File Offset: 0x00057034
		public Color myBraceletColor { get; private set; }

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06003EBE RID: 16062 RVA: 0x00058E3D File Offset: 0x0005703D
		// (set) Token: 0x06003EBF RID: 16063 RVA: 0x00058E45 File Offset: 0x00057045
		public int MyBraceletSelfIndex { get; private set; }

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06003EC0 RID: 16064 RVA: 0x00058E4E File Offset: 0x0005704E
		public List<string> PartyMemberIDs
		{
			get
			{
				return this.myPartyMemberIDs;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06003EC1 RID: 16065 RVA: 0x00058E56 File Offset: 0x00057056
		public bool IsInParty
		{
			get
			{
				return this.myPartyMemberIDs != null;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06003EC2 RID: 16066 RVA: 0x00058E61 File Offset: 0x00057061
		// (set) Token: 0x06003EC3 RID: 16067 RVA: 0x00058E69 File Offset: 0x00057069
		public GroupJoinZoneAB partyZone { get; private set; }

		// Token: 0x06003EC4 RID: 16068 RVA: 0x00058E72 File Offset: 0x00057072
		private void Awake()
		{
			FriendshipGroupDetection.Instance = this;
			if (this.friendshipBubble)
			{
				this.particleSystem = this.friendshipBubble.GetComponent<ParticleSystem>();
				this.audioSource = this.friendshipBubble.GetComponent<AudioSource>();
			}
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x00058EA9 File Offset: 0x000570A9
		public void AddGroupZoneCallback(Action<GroupJoinZoneAB> callback)
		{
			this.groupZoneCallbacks.Add(callback);
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x00058EB7 File Offset: 0x000570B7
		public void RemoveGroupZoneCallback(Action<GroupJoinZoneAB> callback)
		{
			this.groupZoneCallbacks.Remove(callback);
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x00058EC6 File Offset: 0x000570C6
		public bool IsInMyGroup(string userID)
		{
			return this.myPartyMemberIDs != null && this.myPartyMemberIDs.Contains(userID);
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x00164E08 File Offset: 0x00163008
		public bool AnyPartyMembersOutsideFriendCollider()
		{
			if (!this.IsInParty)
			{
				return false;
			}
			foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
			{
				if (vrrig.IsLocalPartyMember && !GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(vrrig.creator.UserId))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06003EC9 RID: 16073 RVA: 0x00058EDE File Offset: 0x000570DE
		// (set) Token: 0x06003ECA RID: 16074 RVA: 0x00058EE6 File Offset: 0x000570E6
		public bool DidJoinLeftHanded { get; private set; }

		// Token: 0x06003ECB RID: 16075 RVA: 0x00164E98 File Offset: 0x00163098
		private void Update()
		{
			List<int> list = this.playersInProvisionalGroup;
			List<int> list2 = this.playersInProvisionalGroup;
			List<int> list3 = this.tempIntList;
			this.tempIntList = list2;
			this.playersInProvisionalGroup = list3;
			Vector3 position;
			this.UpdateProvisionalGroup(out position);
			if (this.playersInProvisionalGroup.Count > 0)
			{
				this.friendshipBubble.transform.position = position;
			}
			bool flag = false;
			if (list.Count == this.playersInProvisionalGroup.Count)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != this.playersInProvisionalGroup[i])
					{
						flag = true;
						break;
					}
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				this.groupCreateAfterTimestamp = Time.time + this.groupTime;
				this.amFirstProvisionalPlayer = (this.playersInProvisionalGroup.Count > 0 && this.playersInProvisionalGroup[0] == NetworkSystem.Instance.LocalPlayer.ActorNumber);
				if (this.playersInProvisionalGroup.Count > 0 && !this.amFirstProvisionalPlayer)
				{
					List<int> list4 = this.tempIntList;
					list4.Clear();
					NetPlayer netPlayer = null;
					foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
					{
						if (vrrig.creator.ActorNumber == this.playersInProvisionalGroup[0])
						{
							netPlayer = vrrig.creator;
							if (vrrig.IsLocalPartyMember)
							{
								list4.Clear();
								break;
							}
						}
						else if (vrrig.IsLocalPartyMember)
						{
							list4.Add(vrrig.creator.ActorNumber);
						}
					}
					if (list4.Count > 0)
					{
						this.photonView.RPC("NotifyPartyMerging", netPlayer.GetPlayerRef(), new object[]
						{
							list4.ToArray()
						});
					}
					else
					{
						this.photonView.RPC("NotifyNoPartyToMerge", netPlayer.GetPlayerRef(), Array.Empty<object>());
					}
				}
				if (this.playersInProvisionalGroup.Count == 0)
				{
					if (Time.time > this.suppressPartyCreationUntilTimestamp && this.playEffectsAfterTimestamp == 0f)
					{
						this.audioSource.GTStop();
						this.audioSource.GTPlayOneShot(this.fistBumpInterruptedAudio, 1f);
					}
					this.particleSystem.Stop();
					this.playEffectsAfterTimestamp = 0f;
				}
				else
				{
					this.playEffectsAfterTimestamp = Time.time + this.playEffectsDelay;
				}
			}
			else if (this.playEffectsAfterTimestamp > 0f && Time.time > this.playEffectsAfterTimestamp)
			{
				this.audioSource.time = 0f;
				this.audioSource.GTPlay();
				this.particleSystem.Play();
				this.playEffectsAfterTimestamp = 0f;
			}
			else if (this.playersInProvisionalGroup.Count > 0 && Time.time > this.groupCreateAfterTimestamp && this.amFirstProvisionalPlayer)
			{
				List<int> list5 = this.tempIntList;
				list5.Clear();
				list5.AddRange(this.playersInProvisionalGroup);
				int num = 0;
				if (this.IsInParty)
				{
					foreach (VRRig vrrig2 in GorillaParent.instance.vrrigs)
					{
						if (vrrig2.IsLocalPartyMember)
						{
							list5.Add(vrrig2.creator.ActorNumber);
							num++;
						}
					}
				}
				int num2 = 0;
				foreach (int key in this.playersInProvisionalGroup)
				{
					int[] collection;
					if (this.partyMergeIDs.TryGetValue(key, out collection))
					{
						list5.AddRange(collection);
						num2++;
					}
				}
				list5.Sort();
				int[] memberIDs = list5.Distinct<int>().ToArray<int>();
				this.myBraceletColor = GTColor.RandomHSV(this.braceletRandomColorHSVRanges);
				this.SendPartyFormedRPC(FriendshipGroupDetection.PackColor(this.myBraceletColor), memberIDs, false);
				this.groupCreateAfterTimestamp = Time.time + this.cooldownAfterCreatingGroup;
			}
			if (this.myPartyMemberIDs != null)
			{
				this.UpdateWarningSigns();
			}
		}

		// Token: 0x06003ECC RID: 16076 RVA: 0x001652D8 File Offset: 0x001634D8
		private void UpdateProvisionalGroup(out Vector3 midpoint)
		{
			this.playersInProvisionalGroup.Clear();
			bool willJoinLeftHanded;
			VRMap makingFist = GorillaTagger.Instance.offlineVRRig.GetMakingFist(this.debug, out willJoinLeftHanded);
			if (makingFist == null || !NetworkSystem.Instance.InRoom || GorillaParent.instance.vrrigs.Count == 0 || Time.time < this.suppressPartyCreationUntilTimestamp || (GorillaGameModes.GameMode.ActiveGameMode != null && !GorillaGameModes.GameMode.ActiveGameMode.CanJoinFrienship(NetworkSystem.Instance.LocalPlayer)))
			{
				midpoint = Vector3.zero;
				return;
			}
			this.WillJoinLeftHanded = willJoinLeftHanded;
			this.playersToPropagateFrom.Clear();
			this.provisionalGroupUsingLeftHands.Clear();
			this.playersMakingFists.Clear();
			int actorNumber = NetworkSystem.Instance.LocalPlayer.ActorNumber;
			int num = -1;
			foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
			{
				bool isLeftHand;
				VRMap makingFist2 = vrrig.GetMakingFist(this.debug, out isLeftHand);
				if (makingFist2 != null && (!(GorillaGameModes.GameMode.ActiveGameMode != null) || GorillaGameModes.GameMode.ActiveGameMode.CanJoinFrienship(vrrig.OwningNetPlayer)))
				{
					FriendshipGroupDetection.PlayerFist item = new FriendshipGroupDetection.PlayerFist
					{
						actorNumber = vrrig.creator.ActorNumber,
						position = makingFist2.rigTarget.position,
						isLeftHand = isLeftHand
					};
					if (vrrig.isOfflineVRRig)
					{
						num = this.playersMakingFists.Count;
					}
					this.playersMakingFists.Add(item);
				}
			}
			if (this.playersMakingFists.Count <= 1)
			{
				midpoint = Vector3.zero;
				return;
			}
			this.playersToPropagateFrom.Enqueue(this.playersMakingFists[num]);
			this.playersInProvisionalGroup.Add(actorNumber);
			midpoint = makingFist.rigTarget.position;
			int num2 = 1 << num;
			FriendshipGroupDetection.PlayerFist playerFist;
			while (this.playersToPropagateFrom.TryDequeue(out playerFist))
			{
				for (int i = 0; i < this.playersMakingFists.Count; i++)
				{
					if ((num2 & 1 << i) == 0)
					{
						FriendshipGroupDetection.PlayerFist playerFist2 = this.playersMakingFists[i];
						if ((playerFist.position - playerFist2.position).IsShorterThan(this.detectionRadius))
						{
							int index = ~this.playersInProvisionalGroup.BinarySearch(playerFist2.actorNumber);
							num2 |= 1 << i;
							this.playersInProvisionalGroup.Insert(index, playerFist2.actorNumber);
							if (playerFist2.isLeftHand)
							{
								this.provisionalGroupUsingLeftHands.Add(playerFist2.actorNumber);
							}
							this.playersToPropagateFrom.Enqueue(playerFist2);
							midpoint += playerFist2.position;
						}
					}
				}
			}
			if (this.playersInProvisionalGroup.Count == 1)
			{
				this.playersInProvisionalGroup.Clear();
			}
			if (this.playersInProvisionalGroup.Count > 0)
			{
				midpoint /= (float)this.playersInProvisionalGroup.Count;
			}
		}

		// Token: 0x06003ECD RID: 16077 RVA: 0x00165604 File Offset: 0x00163804
		private void UpdateWarningSigns()
		{
			ZoneEntity zoneEntity = GorillaTagger.Instance.offlineVRRig.zoneEntity;
			GTZone currentRoomZone = PhotonNetworkController.Instance.CurrentRoomZone;
			GroupJoinZoneAB groupJoinZoneAB = 0;
			if (this.myPartyMemberIDs != null)
			{
				foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
				{
					if (vrrig.IsLocalPartyMember && !vrrig.isOfflineVRRig)
					{
						groupJoinZoneAB |= vrrig.zoneEntity.GroupZone;
					}
				}
			}
			if (groupJoinZoneAB != this.partyZone)
			{
				this.debugStr.Clear();
				foreach (VRRig vrrig2 in GorillaParent.instance.vrrigs)
				{
					if (vrrig2.IsLocalPartyMember && !vrrig2.isOfflineVRRig)
					{
						this.debugStr.Append(string.Format("{0} in {1};", vrrig2.playerNameVisible, vrrig2.zoneEntity.GroupZone));
					}
				}
				this.partyZone = groupJoinZoneAB;
				foreach (Action<GroupJoinZoneAB> action in this.groupZoneCallbacks)
				{
					action(this.partyZone);
				}
			}
		}

		// Token: 0x06003ECE RID: 16078 RVA: 0x00058EEF File Offset: 0x000570EF
		[PunRPC]
		private void NotifyNoPartyToMerge(PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "NotifyNoPartyToMerge");
			if (info.Sender == null || this.partyMergeIDs == null)
			{
				return;
			}
			this.partyMergeIDs.Remove(info.Sender.ActorNumber);
		}

		// Token: 0x06003ECF RID: 16079 RVA: 0x0016578C File Offset: 0x0016398C
		[Rpc]
		private unsafe static void RPC_NotifyNoPartyToMerge(NetworkRunner runner, RpcInfo info = default(RpcInfo))
		{
			if (NetworkBehaviourUtils.InvokeRpc)
			{
				NetworkBehaviourUtils.InvokeRpc = false;
			}
			else
			{
				if (runner == null)
				{
					throw new ArgumentNullException("runner");
				}
				if (runner.Stage == SimulationStages.Resimulate)
				{
					return;
				}
				if (runner.HasAnyActiveConnections())
				{
					int capacityInBytes = 8;
					SimulationMessage* ptr = SimulationMessage.Allocate(runner.Simulation, capacityInBytes);
					byte* data = SimulationMessage.GetData(ptr);
					int num = RpcHeader.Write(RpcHeader.Create(NetworkBehaviourUtils.GetRpcStaticIndexOrThrow("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_NotifyNoPartyToMerge(Fusion.NetworkRunner,Fusion.RpcInfo)")), data);
					ptr->Offset = num * 8;
					ptr->SetStatic();
					runner.SendRpc(ptr);
				}
				info = RpcInfo.FromLocal(runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
			}
			FriendshipGroupDetection.Instance.partyMergeIDs.Remove(info.Source.PlayerId);
		}

		// Token: 0x06003ED0 RID: 16080 RVA: 0x00058F24 File Offset: 0x00057124
		[PunRPC]
		private void NotifyPartyMerging(int[] memberIDs, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "NotifyPartyMerging");
			if (memberIDs.Length > 10)
			{
				return;
			}
			this.partyMergeIDs[info.Sender.ActorNumber] = memberIDs;
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x0016586C File Offset: 0x00163A6C
		[Rpc]
		private unsafe static void RPC_NotifyPartyMerging(NetworkRunner runner, [RpcTarget] PlayerRef playerRef, int[] memberIDs, RpcInfo info = default(RpcInfo))
		{
			if (NetworkBehaviourUtils.InvokeRpc)
			{
				NetworkBehaviourUtils.InvokeRpc = false;
			}
			else
			{
				if (runner == null)
				{
					throw new ArgumentNullException("runner");
				}
				if (runner.Stage != SimulationStages.Resimulate)
				{
					RpcTargetStatus rpcTargetStatus = runner.GetRpcTargetStatus(playerRef);
					if (rpcTargetStatus == RpcTargetStatus.Unreachable)
					{
						NetworkBehaviourUtils.NotifyRpcTargetUnreachable(playerRef, "System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_NotifyPartyMerging(Fusion.NetworkRunner,Fusion.PlayerRef,System.Int32[],Fusion.RpcInfo)");
					}
					else
					{
						if (rpcTargetStatus == RpcTargetStatus.Self)
						{
							info = RpcInfo.FromLocal(runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
							goto IL_10;
						}
						int num = 8;
						num += (memberIDs.Length * 4 + 4 + 3 & -4);
						SimulationMessage* ptr = SimulationMessage.Allocate(runner.Simulation, num);
						byte* data = SimulationMessage.GetData(ptr);
						int num2 = RpcHeader.Write(RpcHeader.Create(NetworkBehaviourUtils.GetRpcStaticIndexOrThrow("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_NotifyPartyMerging(Fusion.NetworkRunner,Fusion.PlayerRef,System.Int32[],Fusion.RpcInfo)")), data);
						*(int*)(data + num2) = memberIDs.Length;
						num2 += 4;
						num2 = (Native.CopyFromArray<int>((void*)(data + num2), memberIDs) + 3 & -4) + num2;
						ptr->Offset = num2 * 8;
						ptr->SetTarget(playerRef);
						ptr->SetStatic();
						runner.SendRpc(ptr);
					}
				}
				return;
			}
			IL_10:
			if (memberIDs.Length > 10)
			{
				return;
			}
			FriendshipGroupDetection.Instance.partyMergeIDs[info.Source.PlayerId] = memberIDs;
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x001659D8 File Offset: 0x00163BD8
		public void SendAboutToGroupJoin()
		{
			foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"Sending group join to ",
					GorillaParent.instance.vrrigs.Count.ToString(),
					" players. Party member:",
					vrrig.OwningNetPlayer.NickName,
					"Is offline rig",
					vrrig.isOfflineVRRig.ToString()
				}));
				if (vrrig.IsLocalPartyMember && !vrrig.isOfflineVRRig)
				{
					this.photonView.RPC("PartyMemberIsAboutToGroupJoin", vrrig.Creator.GetPlayerRef(), Array.Empty<object>());
				}
			}
		}

		// Token: 0x06003ED3 RID: 16083 RVA: 0x00058F50 File Offset: 0x00057150
		[PunRPC]
		private void PartyMemberIsAboutToGroupJoin(PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "PartyMemberIsAboutToGroupJoin");
			this.PartMemberIsAboutToGroupJoinWrapped(new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003ED4 RID: 16084 RVA: 0x00165AC0 File Offset: 0x00163CC0
		[Rpc]
		private unsafe static void RPC_PartyMemberIsAboutToGroupJoin(NetworkRunner runner, [RpcTarget] PlayerRef targetPlayer, RpcInfo info = default(RpcInfo))
		{
			if (NetworkBehaviourUtils.InvokeRpc)
			{
				NetworkBehaviourUtils.InvokeRpc = false;
			}
			else
			{
				if (runner == null)
				{
					throw new ArgumentNullException("runner");
				}
				if (runner.Stage != SimulationStages.Resimulate)
				{
					RpcTargetStatus rpcTargetStatus = runner.GetRpcTargetStatus(targetPlayer);
					if (rpcTargetStatus == RpcTargetStatus.Unreachable)
					{
						NetworkBehaviourUtils.NotifyRpcTargetUnreachable(targetPlayer, "System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_PartyMemberIsAboutToGroupJoin(Fusion.NetworkRunner,Fusion.PlayerRef,Fusion.RpcInfo)");
					}
					else
					{
						if (rpcTargetStatus == RpcTargetStatus.Self)
						{
							info = RpcInfo.FromLocal(runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
							goto IL_10;
						}
						int capacityInBytes = 8;
						SimulationMessage* ptr = SimulationMessage.Allocate(runner.Simulation, capacityInBytes);
						byte* data = SimulationMessage.GetData(ptr);
						int num = RpcHeader.Write(RpcHeader.Create(NetworkBehaviourUtils.GetRpcStaticIndexOrThrow("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_PartyMemberIsAboutToGroupJoin(Fusion.NetworkRunner,Fusion.PlayerRef,Fusion.RpcInfo)")), data);
						ptr->Offset = num * 8;
						ptr->SetTarget(targetPlayer);
						ptr->SetStatic();
						runner.SendRpc(ptr);
					}
				}
				return;
			}
			IL_10:
			FriendshipGroupDetection.Instance.PartMemberIsAboutToGroupJoinWrapped(new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003ED5 RID: 16085 RVA: 0x00165BC0 File Offset: 0x00163DC0
		private void PartMemberIsAboutToGroupJoinWrapped(PhotonMessageInfoWrapped wrappedInfo)
		{
			float time = Time.time;
			float num = this.aboutToGroupJoin_CooldownUntilTimestamp;
			if (wrappedInfo.senderID < NetworkSystem.Instance.LocalPlayer.ActorNumber)
			{
				this.aboutToGroupJoin_CooldownUntilTimestamp = Time.time + 5f;
				if (this.myPartyMembersHash.Contains(wrappedInfo.Sender.UserId))
				{
					PhotonNetworkController.Instance.DeferJoining(2f);
				}
			}
		}

		// Token: 0x06003ED6 RID: 16086 RVA: 0x00165C2C File Offset: 0x00163E2C
		private void SendPartyFormedRPC(short braceletColor, int[] memberIDs, bool forceDebug)
		{
			string text = Enum.Parse<GameModeType>(GorillaComputer.instance.currentGameMode.Value, true).ToString();
			foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
			{
				if (this.playersInProvisionalGroup.BinarySearch(vrrig.creator.ActorNumber) >= 0)
				{
					this.photonView.RPC("PartyFormedSuccessfully", vrrig.Creator.GetPlayerRef(), new object[]
					{
						text,
						braceletColor,
						memberIDs,
						forceDebug
					});
				}
			}
		}

		// Token: 0x06003ED7 RID: 16087 RVA: 0x00165CF8 File Offset: 0x00163EF8
		[Rpc]
		private unsafe static void RPC_PartyFormedSuccessfully(NetworkRunner runner, [RpcTarget] PlayerRef targetPlayer, string partyGameMode, short braceletColor, int[] memberIDs, bool forceDebug, RpcInfo info = default(RpcInfo))
		{
			if (NetworkBehaviourUtils.InvokeRpc)
			{
				NetworkBehaviourUtils.InvokeRpc = false;
			}
			else
			{
				if (runner == null)
				{
					throw new ArgumentNullException("runner");
				}
				if (runner.Stage != SimulationStages.Resimulate)
				{
					RpcTargetStatus rpcTargetStatus = runner.GetRpcTargetStatus(targetPlayer);
					if (rpcTargetStatus == RpcTargetStatus.Unreachable)
					{
						NetworkBehaviourUtils.NotifyRpcTargetUnreachable(targetPlayer, "System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_PartyFormedSuccessfully(Fusion.NetworkRunner,Fusion.PlayerRef,System.String,System.Int16,System.Int32[],System.Boolean,Fusion.RpcInfo)");
					}
					else
					{
						if (rpcTargetStatus == RpcTargetStatus.Self)
						{
							info = RpcInfo.FromLocal(runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
							goto IL_10;
						}
						int num = 8;
						num += (ReadWriteUtilsForWeaver.GetByteCountUtf8NoHash(partyGameMode) + 3 & -4);
						num += 4;
						num += (memberIDs.Length * 4 + 4 + 3 & -4);
						num += 4;
						SimulationMessage* ptr = SimulationMessage.Allocate(runner.Simulation, num);
						byte* data = SimulationMessage.GetData(ptr);
						int num2 = RpcHeader.Write(RpcHeader.Create(NetworkBehaviourUtils.GetRpcStaticIndexOrThrow("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_PartyFormedSuccessfully(Fusion.NetworkRunner,Fusion.PlayerRef,System.String,System.Int16,System.Int32[],System.Boolean,Fusion.RpcInfo)")), data);
						num2 = (ReadWriteUtilsForWeaver.WriteStringUtf8NoHash((void*)(data + num2), partyGameMode) + 3 & -4) + num2;
						*(short*)(data + num2) = braceletColor;
						num2 += (2 + 3 & -4);
						*(int*)(data + num2) = memberIDs.Length;
						num2 += 4;
						num2 = (Native.CopyFromArray<int>((void*)(data + num2), memberIDs) + 3 & -4) + num2;
						ReadWriteUtilsForWeaver.WriteBoolean((int*)(data + num2), forceDebug);
						num2 += 4;
						ptr->Offset = num2 * 8;
						ptr->SetTarget(targetPlayer);
						ptr->SetStatic();
						runner.SendRpc(ptr);
					}
				}
				return;
			}
			IL_10:
			GorillaNot.IncrementRPCCall(info, "PartyFormedSuccessfully");
			FriendshipGroupDetection.Instance.PartyFormedSuccesfullyWrapped(partyGameMode, braceletColor, memberIDs, forceDebug, new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x00058F69 File Offset: 0x00057169
		[PunRPC]
		private void PartyFormedSuccessfully(string partyGameMode, short braceletColor, int[] memberIDs, bool forceDebug, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "PartyFormedSuccessfully");
			this.PartyFormedSuccesfullyWrapped(partyGameMode, braceletColor, memberIDs, forceDebug, new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x00165EF4 File Offset: 0x001640F4
		private void PartyFormedSuccesfullyWrapped(string partyGameMode, short braceletColor, int[] memberIDs, bool forceDebug, PhotonMessageInfoWrapped info)
		{
			if (memberIDs == null || memberIDs.Length > 10 || !memberIDs.Contains(info.Sender.ActorNumber) || this.playersInProvisionalGroup.IndexOf(info.Sender.ActorNumber) != 0 || Mathf.Abs(this.groupCreateAfterTimestamp - Time.time) > this.m_maxGroupJoinTimeDifference || !GorillaGameModes.GameMode.IsValidGameMode(partyGameMode))
			{
				return;
			}
			if (this.IsInParty)
			{
				string text = Enum.Parse<GameModeType>(GorillaComputer.instance.currentGameMode.Value, true).ToString();
				foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
				{
					if (vrrig.IsLocalPartyMember && !vrrig.isOfflineVRRig)
					{
						this.photonView.RPC("AddPartyMembers", vrrig.Creator.GetPlayerRef(), new object[]
						{
							text,
							braceletColor,
							memberIDs
						});
					}
				}
			}
			this.suppressPartyCreationUntilTimestamp = Time.time + this.cooldownAfterCreatingGroup;
			this.DidJoinLeftHanded = this.WillJoinLeftHanded;
			this.SetNewParty(partyGameMode, braceletColor, memberIDs);
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x00058F89 File Offset: 0x00057189
		[PunRPC]
		private void AddPartyMembers(string partyGameMode, short braceletColor, int[] memberIDs, PhotonMessageInfo info)
		{
			this.AddPartyMembersWrapped(partyGameMode, braceletColor, memberIDs, new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x0016603C File Offset: 0x0016423C
		[Rpc]
		private unsafe static void RPC_AddPartyMembers(NetworkRunner runner, [RpcTarget] PlayerRef rpcTarget, string partyGameMode, short braceletColor, int[] memberIDs, RpcInfo info = default(RpcInfo))
		{
			if (NetworkBehaviourUtils.InvokeRpc)
			{
				NetworkBehaviourUtils.InvokeRpc = false;
			}
			else
			{
				if (runner == null)
				{
					throw new ArgumentNullException("runner");
				}
				if (runner.Stage != SimulationStages.Resimulate)
				{
					RpcTargetStatus rpcTargetStatus = runner.GetRpcTargetStatus(rpcTarget);
					if (rpcTargetStatus == RpcTargetStatus.Unreachable)
					{
						NetworkBehaviourUtils.NotifyRpcTargetUnreachable(rpcTarget, "System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_AddPartyMembers(Fusion.NetworkRunner,Fusion.PlayerRef,System.String,System.Int16,System.Int32[],Fusion.RpcInfo)");
					}
					else
					{
						if (rpcTargetStatus == RpcTargetStatus.Self)
						{
							info = RpcInfo.FromLocal(runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
							goto IL_10;
						}
						int num = 8;
						num += (ReadWriteUtilsForWeaver.GetByteCountUtf8NoHash(partyGameMode) + 3 & -4);
						num += 4;
						num += (memberIDs.Length * 4 + 4 + 3 & -4);
						SimulationMessage* ptr = SimulationMessage.Allocate(runner.Simulation, num);
						byte* data = SimulationMessage.GetData(ptr);
						int num2 = RpcHeader.Write(RpcHeader.Create(NetworkBehaviourUtils.GetRpcStaticIndexOrThrow("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_AddPartyMembers(Fusion.NetworkRunner,Fusion.PlayerRef,System.String,System.Int16,System.Int32[],Fusion.RpcInfo)")), data);
						num2 = (ReadWriteUtilsForWeaver.WriteStringUtf8NoHash((void*)(data + num2), partyGameMode) + 3 & -4) + num2;
						*(short*)(data + num2) = braceletColor;
						num2 += (2 + 3 & -4);
						*(int*)(data + num2) = memberIDs.Length;
						num2 += 4;
						num2 = (Native.CopyFromArray<int>((void*)(data + num2), memberIDs) + 3 & -4) + num2;
						ptr->Offset = num2 * 8;
						ptr->SetTarget(rpcTarget);
						ptr->SetStatic();
						runner.SendRpc(ptr);
					}
				}
				return;
			}
			IL_10:
			FriendshipGroupDetection.Instance.AddPartyMembersWrapped(partyGameMode, braceletColor, memberIDs, new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x00166200 File Offset: 0x00164400
		private void AddPartyMembersWrapped(string partyGameMode, short braceletColor, int[] memberIDs, PhotonMessageInfoWrapped infoWrapped)
		{
			GorillaNot.IncrementRPCCall(infoWrapped, "AddPartyMembersWrapped");
			if (memberIDs.Length > 10 || !this.IsInParty || !this.myPartyMembersHash.Contains(NetworkSystem.Instance.GetUserID(infoWrapped.senderID)) || !GorillaGameModes.GameMode.IsValidGameMode(partyGameMode))
			{
				return;
			}
			Debug.Log("Adding party members: [" + string.Join<int>(",", memberIDs) + "]");
			this.SetNewParty(partyGameMode, braceletColor, memberIDs);
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x00166278 File Offset: 0x00164478
		private void SetNewParty(string partyGameMode, short braceletColor, int[] memberIDs)
		{
			GorillaComputer.instance.SetGameModeWithoutButton(partyGameMode);
			this.myPartyMemberIDs = new List<string>();
			FriendshipGroupDetection.userIdLookup.Clear();
			foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
			{
				FriendshipGroupDetection.userIdLookup.Add(vrrig.creator.ActorNumber, vrrig.creator.UserId);
			}
			foreach (int key in memberIDs)
			{
				string item;
				if (FriendshipGroupDetection.userIdLookup.TryGetValue(key, out item))
				{
					this.myPartyMemberIDs.Add(item);
				}
			}
			this.myBraceletColor = FriendshipGroupDetection.UnpackColor(braceletColor);
			GorillaTagger.Instance.StartVibration(this.DidJoinLeftHanded, this.hapticStrength, this.hapticDuration);
			this.OnPartyMembershipChanged();
			PlayerGameEvents.MiscEvent("FriendshipGroupJoined");
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x00166378 File Offset: 0x00164578
		public void LeaveParty()
		{
			if (this.myPartyMemberIDs == null)
			{
				return;
			}
			foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
			{
				if (vrrig.IsLocalPartyMember && !vrrig.isOfflineVRRig)
				{
					this.photonView.RPC("PlayerLeftParty", vrrig.Creator.GetPlayerRef(), Array.Empty<object>());
				}
			}
			this.myPartyMemberIDs = null;
			this.OnPartyMembershipChanged();
			PhotonNetworkController.Instance.ClearDeferredJoin();
			GorillaTagger.Instance.StartVibration(false, this.hapticStrength, this.hapticDuration);
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x00166434 File Offset: 0x00164634
		[Rpc]
		private unsafe static void RPC_PlayerLeftParty(NetworkRunner runner, [RpcTarget] PlayerRef player, RpcInfo info = default(RpcInfo))
		{
			if (NetworkBehaviourUtils.InvokeRpc)
			{
				NetworkBehaviourUtils.InvokeRpc = false;
			}
			else
			{
				if (runner == null)
				{
					throw new ArgumentNullException("runner");
				}
				if (runner.Stage != SimulationStages.Resimulate)
				{
					RpcTargetStatus rpcTargetStatus = runner.GetRpcTargetStatus(player);
					if (rpcTargetStatus == RpcTargetStatus.Unreachable)
					{
						NetworkBehaviourUtils.NotifyRpcTargetUnreachable(player, "System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_PlayerLeftParty(Fusion.NetworkRunner,Fusion.PlayerRef,Fusion.RpcInfo)");
					}
					else
					{
						if (rpcTargetStatus == RpcTargetStatus.Self)
						{
							info = RpcInfo.FromLocal(runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
							goto IL_10;
						}
						int capacityInBytes = 8;
						SimulationMessage* ptr = SimulationMessage.Allocate(runner.Simulation, capacityInBytes);
						byte* data = SimulationMessage.GetData(ptr);
						int num = RpcHeader.Write(RpcHeader.Create(NetworkBehaviourUtils.GetRpcStaticIndexOrThrow("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_PlayerLeftParty(Fusion.NetworkRunner,Fusion.PlayerRef,Fusion.RpcInfo)")), data);
						ptr->Offset = num * 8;
						ptr->SetTarget(player);
						ptr->SetStatic();
						runner.SendRpc(ptr);
					}
				}
				return;
			}
			IL_10:
			GorillaNot.IncrementRPCCall(info, "PlayerLeftParty");
			FriendshipGroupDetection.Instance.PlayerLeftPartyWrapped(new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x00058F9B File Offset: 0x0005719B
		[PunRPC]
		private void PlayerLeftParty(PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "PlayerLeftParty");
			this.PlayerLeftPartyWrapped(new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x00166544 File Offset: 0x00164744
		private void PlayerLeftPartyWrapped(PhotonMessageInfoWrapped infoWrapped)
		{
			if (this.myPartyMemberIDs == null)
			{
				return;
			}
			if (!this.myPartyMemberIDs.Remove(infoWrapped.Sender.UserId))
			{
				return;
			}
			if (this.myPartyMemberIDs.Count <= 1)
			{
				this.myPartyMemberIDs = null;
			}
			this.OnPartyMembershipChanged();
			GorillaTagger.Instance.StartVibration(this.DidJoinLeftHanded, this.hapticStrength, this.hapticDuration);
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x00058FB4 File Offset: 0x000571B4
		public void SendVerifyPartyMember(NetPlayer player)
		{
			this.photonView.RPC("VerifyPartyMember", player.GetPlayerRef(), Array.Empty<object>());
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x00058FD1 File Offset: 0x000571D1
		[PunRPC]
		private void VerifyPartyMember(PhotonMessageInfo info)
		{
			this.VerifyPartyMemberWrapped(new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x001665AC File Offset: 0x001647AC
		[Rpc]
		private unsafe static void RPC_VerifyPartyMember(NetworkRunner runner, [RpcTarget] PlayerRef rpcTarget, RpcInfo info = default(RpcInfo))
		{
			if (NetworkBehaviourUtils.InvokeRpc)
			{
				NetworkBehaviourUtils.InvokeRpc = false;
			}
			else
			{
				if (runner == null)
				{
					throw new ArgumentNullException("runner");
				}
				if (runner.Stage != SimulationStages.Resimulate)
				{
					RpcTargetStatus rpcTargetStatus = runner.GetRpcTargetStatus(rpcTarget);
					if (rpcTargetStatus == RpcTargetStatus.Unreachable)
					{
						NetworkBehaviourUtils.NotifyRpcTargetUnreachable(rpcTarget, "System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_VerifyPartyMember(Fusion.NetworkRunner,Fusion.PlayerRef,Fusion.RpcInfo)");
					}
					else
					{
						if (rpcTargetStatus == RpcTargetStatus.Self)
						{
							info = RpcInfo.FromLocal(runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
							goto IL_10;
						}
						int capacityInBytes = 8;
						SimulationMessage* ptr = SimulationMessage.Allocate(runner.Simulation, capacityInBytes);
						byte* data = SimulationMessage.GetData(ptr);
						int num = RpcHeader.Write(RpcHeader.Create(NetworkBehaviourUtils.GetRpcStaticIndexOrThrow("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_VerifyPartyMember(Fusion.NetworkRunner,Fusion.PlayerRef,Fusion.RpcInfo)")), data);
						ptr->Offset = num * 8;
						ptr->SetTarget(rpcTarget);
						ptr->SetStatic();
						runner.SendRpc(ptr);
					}
				}
				return;
			}
			IL_10:
			FriendshipGroupDetection.Instance.VerifyPartyMemberWrapped(new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x001666AC File Offset: 0x001648AC
		private void VerifyPartyMemberWrapped(PhotonMessageInfoWrapped infoWrapped)
		{
			GorillaNot.IncrementRPCCall(infoWrapped, "VerifyPartyMemberWrapped");
			RigContainer rigContainer;
			if (!VRRigCache.Instance.TryGetVrrig(infoWrapped.Sender, out rigContainer) || !FXSystem.CheckCallSpam(rigContainer.Rig.fxSettings, 15, infoWrapped.SentServerTime))
			{
				return;
			}
			if (this.myPartyMemberIDs == null || !this.myPartyMemberIDs.Contains(NetworkSystem.Instance.GetUserID(infoWrapped.senderID)))
			{
				this.photonView.RPC("PlayerLeftParty", infoWrapped.Sender.GetPlayerRef(), Array.Empty<object>());
			}
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x0016673C File Offset: 0x0016493C
		public void SendRequestPartyGameMode(string gameMode)
		{
			int num = int.MaxValue;
			NetPlayer netPlayer = null;
			foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
			{
				if (vrrig.IsLocalPartyMember && vrrig.creator.ActorNumber < num)
				{
					netPlayer = vrrig.creator;
					num = vrrig.creator.ActorNumber;
				}
			}
			if (netPlayer != null)
			{
				this.photonView.RPC("RequestPartyGameMode", netPlayer.GetPlayerRef(), new object[]
				{
					gameMode
				});
			}
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x001667E4 File Offset: 0x001649E4
		[Rpc]
		private unsafe static void RPC_RequestPartyGameMode(NetworkRunner runner, [RpcTarget] PlayerRef targetPlayer, string gameMode, RpcInfo info = default(RpcInfo))
		{
			if (NetworkBehaviourUtils.InvokeRpc)
			{
				NetworkBehaviourUtils.InvokeRpc = false;
			}
			else
			{
				if (runner == null)
				{
					throw new ArgumentNullException("runner");
				}
				if (runner.Stage != SimulationStages.Resimulate)
				{
					RpcTargetStatus rpcTargetStatus = runner.GetRpcTargetStatus(targetPlayer);
					if (rpcTargetStatus == RpcTargetStatus.Unreachable)
					{
						NetworkBehaviourUtils.NotifyRpcTargetUnreachable(targetPlayer, "System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_RequestPartyGameMode(Fusion.NetworkRunner,Fusion.PlayerRef,System.String,Fusion.RpcInfo)");
					}
					else
					{
						if (rpcTargetStatus == RpcTargetStatus.Self)
						{
							info = RpcInfo.FromLocal(runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
							goto IL_10;
						}
						int num = 8;
						num += (ReadWriteUtilsForWeaver.GetByteCountUtf8NoHash(gameMode) + 3 & -4);
						SimulationMessage* ptr = SimulationMessage.Allocate(runner.Simulation, num);
						byte* data = SimulationMessage.GetData(ptr);
						int num2 = RpcHeader.Write(RpcHeader.Create(NetworkBehaviourUtils.GetRpcStaticIndexOrThrow("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_RequestPartyGameMode(Fusion.NetworkRunner,Fusion.PlayerRef,System.String,Fusion.RpcInfo)")), data);
						num2 = (ReadWriteUtilsForWeaver.WriteStringUtf8NoHash((void*)(data + num2), gameMode) + 3 & -4) + num2;
						ptr->Offset = num2 * 8;
						ptr->SetTarget(targetPlayer);
						ptr->SetStatic();
						runner.SendRpc(ptr);
					}
				}
				return;
			}
			IL_10:
			FriendshipGroupDetection.Instance.RequestPartyGameModeWrapped(gameMode, new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x00058FDF File Offset: 0x000571DF
		[PunRPC]
		private void RequestPartyGameMode(string gameMode, PhotonMessageInfo info)
		{
			this.RequestPartyGameModeWrapped(gameMode, new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x00166924 File Offset: 0x00164B24
		private void RequestPartyGameModeWrapped(string gameMode, PhotonMessageInfoWrapped info)
		{
			GorillaNot.IncrementRPCCall(info, "RequestPartyGameModeWrapped");
			if (!this.IsInParty || !this.IsInMyGroup(info.Sender.UserId) || !GorillaGameModes.GameMode.IsValidGameMode(gameMode))
			{
				return;
			}
			foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
			{
				if (vrrig.IsLocalPartyMember)
				{
					this.photonView.RPC("NotifyPartyGameModeChanged", vrrig.creator.GetPlayerRef(), new object[]
					{
						gameMode
					});
				}
			}
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x001669D4 File Offset: 0x00164BD4
		[Rpc]
		private unsafe static void RPC_NotifyPartyGameModeChanged(NetworkRunner runner, [RpcTarget] PlayerRef targetPlayer, string gameMode, RpcInfo info = default(RpcInfo))
		{
			if (NetworkBehaviourUtils.InvokeRpc)
			{
				NetworkBehaviourUtils.InvokeRpc = false;
			}
			else
			{
				if (runner == null)
				{
					throw new ArgumentNullException("runner");
				}
				if (runner.Stage != SimulationStages.Resimulate)
				{
					RpcTargetStatus rpcTargetStatus = runner.GetRpcTargetStatus(targetPlayer);
					if (rpcTargetStatus == RpcTargetStatus.Unreachable)
					{
						NetworkBehaviourUtils.NotifyRpcTargetUnreachable(targetPlayer, "System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_NotifyPartyGameModeChanged(Fusion.NetworkRunner,Fusion.PlayerRef,System.String,Fusion.RpcInfo)");
					}
					else
					{
						if (rpcTargetStatus == RpcTargetStatus.Self)
						{
							info = RpcInfo.FromLocal(runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
							goto IL_10;
						}
						int num = 8;
						num += (ReadWriteUtilsForWeaver.GetByteCountUtf8NoHash(gameMode) + 3 & -4);
						SimulationMessage* ptr = SimulationMessage.Allocate(runner.Simulation, num);
						byte* data = SimulationMessage.GetData(ptr);
						int num2 = RpcHeader.Write(RpcHeader.Create(NetworkBehaviourUtils.GetRpcStaticIndexOrThrow("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_NotifyPartyGameModeChanged(Fusion.NetworkRunner,Fusion.PlayerRef,System.String,Fusion.RpcInfo)")), data);
						num2 = (ReadWriteUtilsForWeaver.WriteStringUtf8NoHash((void*)(data + num2), gameMode) + 3 & -4) + num2;
						ptr->Offset = num2 * 8;
						ptr->SetTarget(targetPlayer);
						ptr->SetStatic();
						runner.SendRpc(ptr);
					}
				}
				return;
			}
			IL_10:
			FriendshipGroupDetection.Instance.NotifyPartyGameModeChangedWrapped(gameMode, new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x00058FEE File Offset: 0x000571EE
		[PunRPC]
		private void NotifyPartyGameModeChanged(string gameMode, PhotonMessageInfo info)
		{
			this.NotifyPartyGameModeChangedWrapped(gameMode, new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x00058FFD File Offset: 0x000571FD
		private void NotifyPartyGameModeChangedWrapped(string gameMode, PhotonMessageInfoWrapped info)
		{
			GorillaNot.IncrementRPCCall(info, "NotifyPartyGameModeChangedWrapped");
			if (!this.IsInParty || !this.IsInMyGroup(info.Sender.UserId) || !GorillaGameModes.GameMode.IsValidGameMode(gameMode))
			{
				return;
			}
			GorillaComputer.instance.SetGameModeWithoutButton(gameMode);
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x00166B14 File Offset: 0x00164D14
		private void OnPartyMembershipChanged()
		{
			this.myPartyMembersHash.Clear();
			if (this.myPartyMemberIDs != null)
			{
				foreach (string item in this.myPartyMemberIDs)
				{
					this.myPartyMembersHash.Add(item);
				}
			}
			this.myBeadColors.Clear();
			FriendshipGroupDetection.tempColorLookup.Clear();
			foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
			{
				vrrig.ClearPartyMemberStatus();
				if (vrrig.IsLocalPartyMember)
				{
					FriendshipGroupDetection.tempColorLookup.Add(vrrig.Creator.UserId, vrrig.playerColor);
				}
			}
			this.MyBraceletSelfIndex = 0;
			if (this.myPartyMemberIDs != null)
			{
				using (List<string>.Enumerator enumerator = this.myPartyMemberIDs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						Color item2;
						if (FriendshipGroupDetection.tempColorLookup.TryGetValue(text, out item2))
						{
							if (text == PhotonNetwork.LocalPlayer.UserId)
							{
								this.MyBraceletSelfIndex = this.myBeadColors.Count;
							}
							this.myBeadColors.Add(item2);
						}
					}
					goto IL_15A;
				}
			}
			GorillaComputer.instance.SetGameModeWithoutButton(GorillaComputer.instance.lastPressedGameMode);
			IL_15A:
			this.myBeadColors.Add(this.myBraceletColor);
			GorillaTagger.Instance.offlineVRRig.UpdateFriendshipBracelet();
			this.UpdateWarningSigns();
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x00166CCC File Offset: 0x00164ECC
		public bool IsPartyWithinCollider(GorillaFriendCollider friendCollider)
		{
			foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
			{
				if (vrrig.IsLocalPartyMember && !vrrig.isOfflineVRRig && !friendCollider.playerIDsCurrentlyTouching.Contains(vrrig.Creator.UserId))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003EEF RID: 16111 RVA: 0x0005903C File Offset: 0x0005723C
		public static short PackColor(Color col)
		{
			return (short)(Mathf.RoundToInt(col.r * 9f) + Mathf.RoundToInt(col.g * 9f) * 10 + Mathf.RoundToInt(col.b * 9f) * 100);
		}

		// Token: 0x06003EF0 RID: 16112 RVA: 0x00166D50 File Offset: 0x00164F50
		public static Color UnpackColor(short data)
		{
			return new Color
			{
				r = (float)(data % 10) / 9f,
				g = (float)(data / 10 % 10) / 9f,
				b = (float)(data / 100 % 10) / 9f
			};
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x00166E78 File Offset: 0x00165078
		[NetworkRpcStaticWeavedInvoker("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_NotifyNoPartyToMerge(Fusion.NetworkRunner,Fusion.RpcInfo)")]
		[Preserve]
		[WeaverGenerated]
		protected unsafe static void RPC_NotifyNoPartyToMerge@Invoker(NetworkRunner runner, SimulationMessage* message)
		{
			byte* data = SimulationMessage.GetData(message);
			int num = RpcHeader.ReadSize(data) + 3 & -4;
			RpcInfo info = RpcInfo.FromMessage(runner, message, RpcHostMode.SourceIsServer);
			NetworkBehaviourUtils.InvokeRpc = true;
			FriendshipGroupDetection.RPC_NotifyNoPartyToMerge(runner, info);
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x00166EC8 File Offset: 0x001650C8
		[NetworkRpcStaticWeavedInvoker("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_NotifyPartyMerging(Fusion.NetworkRunner,Fusion.PlayerRef,System.Int32[],Fusion.RpcInfo)")]
		[Preserve]
		[WeaverGenerated]
		protected unsafe static void RPC_NotifyPartyMerging@Invoker(NetworkRunner runner, SimulationMessage* message)
		{
			byte* data = SimulationMessage.GetData(message);
			int num = RpcHeader.ReadSize(data) + 3 & -4;
			PlayerRef target = message->Target;
			int[] array = new int[*(int*)(data + num)];
			num += 4;
			num = (Native.CopyToArray<int>(array, (void*)(data + num)) + 3 & -4) + num;
			RpcInfo info = RpcInfo.FromMessage(runner, message, RpcHostMode.SourceIsServer);
			NetworkBehaviourUtils.InvokeRpc = true;
			FriendshipGroupDetection.RPC_NotifyPartyMerging(runner, target, array, info);
		}

		// Token: 0x06003EF5 RID: 16117 RVA: 0x00166F6C File Offset: 0x0016516C
		[NetworkRpcStaticWeavedInvoker("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_PartyMemberIsAboutToGroupJoin(Fusion.NetworkRunner,Fusion.PlayerRef,Fusion.RpcInfo)")]
		[Preserve]
		[WeaverGenerated]
		protected unsafe static void RPC_PartyMemberIsAboutToGroupJoin@Invoker(NetworkRunner runner, SimulationMessage* message)
		{
			byte* data = SimulationMessage.GetData(message);
			int num = RpcHeader.ReadSize(data) + 3 & -4;
			PlayerRef target = message->Target;
			RpcInfo info = RpcInfo.FromMessage(runner, message, RpcHostMode.SourceIsServer);
			NetworkBehaviourUtils.InvokeRpc = true;
			FriendshipGroupDetection.RPC_PartyMemberIsAboutToGroupJoin(runner, target, info);
		}

		// Token: 0x06003EF6 RID: 16118 RVA: 0x00166FCC File Offset: 0x001651CC
		[NetworkRpcStaticWeavedInvoker("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_PartyFormedSuccessfully(Fusion.NetworkRunner,Fusion.PlayerRef,System.String,System.Int16,System.Int32[],System.Boolean,Fusion.RpcInfo)")]
		[Preserve]
		[WeaverGenerated]
		protected unsafe static void RPC_PartyFormedSuccessfully@Invoker(NetworkRunner runner, SimulationMessage* message)
		{
			byte* data = SimulationMessage.GetData(message);
			int num = RpcHeader.ReadSize(data) + 3 & -4;
			PlayerRef target = message->Target;
			string partyGameMode;
			num = (ReadWriteUtilsForWeaver.ReadStringUtf8NoHash((void*)(data + num), out partyGameMode) + 3 & -4) + num;
			short num2 = *(short*)(data + num);
			num += (2 + 3 & -4);
			short braceletColor = num2;
			int[] array = new int[*(int*)(data + num)];
			num += 4;
			num = (Native.CopyToArray<int>(array, (void*)(data + num)) + 3 & -4) + num;
			bool flag = ReadWriteUtilsForWeaver.ReadBoolean((int*)(data + num));
			num += 4;
			bool forceDebug = flag;
			RpcInfo info = RpcInfo.FromMessage(runner, message, RpcHostMode.SourceIsServer);
			NetworkBehaviourUtils.InvokeRpc = true;
			FriendshipGroupDetection.RPC_PartyFormedSuccessfully(runner, target, partyGameMode, braceletColor, array, forceDebug, info);
		}

		// Token: 0x06003EF7 RID: 16119 RVA: 0x001670DC File Offset: 0x001652DC
		[NetworkRpcStaticWeavedInvoker("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_AddPartyMembers(Fusion.NetworkRunner,Fusion.PlayerRef,System.String,System.Int16,System.Int32[],Fusion.RpcInfo)")]
		[Preserve]
		[WeaverGenerated]
		protected unsafe static void RPC_AddPartyMembers@Invoker(NetworkRunner runner, SimulationMessage* message)
		{
			byte* data = SimulationMessage.GetData(message);
			int num = RpcHeader.ReadSize(data) + 3 & -4;
			PlayerRef target = message->Target;
			string partyGameMode;
			num = (ReadWriteUtilsForWeaver.ReadStringUtf8NoHash((void*)(data + num), out partyGameMode) + 3 & -4) + num;
			short num2 = *(short*)(data + num);
			num += (2 + 3 & -4);
			short braceletColor = num2;
			int[] array = new int[*(int*)(data + num)];
			num += 4;
			num = (Native.CopyToArray<int>(array, (void*)(data + num)) + 3 & -4) + num;
			RpcInfo info = RpcInfo.FromMessage(runner, message, RpcHostMode.SourceIsServer);
			NetworkBehaviourUtils.InvokeRpc = true;
			FriendshipGroupDetection.RPC_AddPartyMembers(runner, target, partyGameMode, braceletColor, array, info);
		}

		// Token: 0x06003EF8 RID: 16120 RVA: 0x001671CC File Offset: 0x001653CC
		[NetworkRpcStaticWeavedInvoker("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_PlayerLeftParty(Fusion.NetworkRunner,Fusion.PlayerRef,Fusion.RpcInfo)")]
		[Preserve]
		[WeaverGenerated]
		protected unsafe static void RPC_PlayerLeftParty@Invoker(NetworkRunner runner, SimulationMessage* message)
		{
			byte* data = SimulationMessage.GetData(message);
			int num = RpcHeader.ReadSize(data) + 3 & -4;
			PlayerRef target = message->Target;
			RpcInfo info = RpcInfo.FromMessage(runner, message, RpcHostMode.SourceIsServer);
			NetworkBehaviourUtils.InvokeRpc = true;
			FriendshipGroupDetection.RPC_PlayerLeftParty(runner, target, info);
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x0016722C File Offset: 0x0016542C
		[NetworkRpcStaticWeavedInvoker("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_VerifyPartyMember(Fusion.NetworkRunner,Fusion.PlayerRef,Fusion.RpcInfo)")]
		[Preserve]
		[WeaverGenerated]
		protected unsafe static void RPC_VerifyPartyMember@Invoker(NetworkRunner runner, SimulationMessage* message)
		{
			byte* data = SimulationMessage.GetData(message);
			int num = RpcHeader.ReadSize(data) + 3 & -4;
			PlayerRef target = message->Target;
			RpcInfo info = RpcInfo.FromMessage(runner, message, RpcHostMode.SourceIsServer);
			NetworkBehaviourUtils.InvokeRpc = true;
			FriendshipGroupDetection.RPC_VerifyPartyMember(runner, target, info);
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x0016728C File Offset: 0x0016548C
		[NetworkRpcStaticWeavedInvoker("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_RequestPartyGameMode(Fusion.NetworkRunner,Fusion.PlayerRef,System.String,Fusion.RpcInfo)")]
		[Preserve]
		[WeaverGenerated]
		protected unsafe static void RPC_RequestPartyGameMode@Invoker(NetworkRunner runner, SimulationMessage* message)
		{
			byte* data = SimulationMessage.GetData(message);
			int num = RpcHeader.ReadSize(data) + 3 & -4;
			PlayerRef target = message->Target;
			string gameMode;
			num = (ReadWriteUtilsForWeaver.ReadStringUtf8NoHash((void*)(data + num), out gameMode) + 3 & -4) + num;
			RpcInfo info = RpcInfo.FromMessage(runner, message, RpcHostMode.SourceIsServer);
			NetworkBehaviourUtils.InvokeRpc = true;
			FriendshipGroupDetection.RPC_RequestPartyGameMode(runner, target, gameMode, info);
		}

		// Token: 0x06003EFB RID: 16123 RVA: 0x00167314 File Offset: 0x00165514
		[NetworkRpcStaticWeavedInvoker("System.Void GorillaTagScripts.FriendshipGroupDetection::RPC_NotifyPartyGameModeChanged(Fusion.NetworkRunner,Fusion.PlayerRef,System.String,Fusion.RpcInfo)")]
		[Preserve]
		[WeaverGenerated]
		protected unsafe static void RPC_NotifyPartyGameModeChanged@Invoker(NetworkRunner runner, SimulationMessage* message)
		{
			byte* data = SimulationMessage.GetData(message);
			int num = RpcHeader.ReadSize(data) + 3 & -4;
			PlayerRef target = message->Target;
			string gameMode;
			num = (ReadWriteUtilsForWeaver.ReadStringUtf8NoHash((void*)(data + num), out gameMode) + 3 & -4) + num;
			RpcInfo info = RpcInfo.FromMessage(runner, message, RpcHostMode.SourceIsServer);
			NetworkBehaviourUtils.InvokeRpc = true;
			FriendshipGroupDetection.RPC_NotifyPartyGameModeChanged(runner, target, gameMode, info);
		}

		// Token: 0x04003FD3 RID: 16339
		[SerializeField]
		private float detectionRadius = 0.5f;

		// Token: 0x04003FD4 RID: 16340
		[SerializeField]
		private float groupTime = 5f;

		// Token: 0x04003FD5 RID: 16341
		[SerializeField]
		private float cooldownAfterCreatingGroup = 5f;

		// Token: 0x04003FD6 RID: 16342
		[SerializeField]
		private float hapticStrength = 1.5f;

		// Token: 0x04003FD7 RID: 16343
		[SerializeField]
		private float hapticDuration = 2f;

		// Token: 0x04003FD8 RID: 16344
		public bool debug;

		// Token: 0x04003FD9 RID: 16345
		public double offset = 0.5;

		// Token: 0x04003FDA RID: 16346
		[SerializeField]
		private float m_maxGroupJoinTimeDifference = 1f;

		// Token: 0x04003FDB RID: 16347
		private List<string> myPartyMemberIDs;

		// Token: 0x04003FDC RID: 16348
		private HashSet<string> myPartyMembersHash = new HashSet<string>();

		// Token: 0x04003FE1 RID: 16353
		private List<Action<GroupJoinZoneAB>> groupZoneCallbacks = new List<Action<GroupJoinZoneAB>>();

		// Token: 0x04003FE2 RID: 16354
		[SerializeField]
		private GTColor.HSVRanges braceletRandomColorHSVRanges;

		// Token: 0x04003FE3 RID: 16355
		public GameObject friendshipBubble;

		// Token: 0x04003FE4 RID: 16356
		public AudioClip fistBumpInterruptedAudio;

		// Token: 0x04003FE5 RID: 16357
		private ParticleSystem particleSystem;

		// Token: 0x04003FE6 RID: 16358
		private AudioSource audioSource;

		// Token: 0x04003FE7 RID: 16359
		private Queue<FriendshipGroupDetection.PlayerFist> playersToPropagateFrom = new Queue<FriendshipGroupDetection.PlayerFist>();

		// Token: 0x04003FE8 RID: 16360
		private List<int> playersInProvisionalGroup = new List<int>();

		// Token: 0x04003FE9 RID: 16361
		private List<int> provisionalGroupUsingLeftHands = new List<int>();

		// Token: 0x04003FEA RID: 16362
		private List<int> tempIntList = new List<int>();

		// Token: 0x04003FEB RID: 16363
		private bool amFirstProvisionalPlayer;

		// Token: 0x04003FEC RID: 16364
		private Dictionary<int, int[]> partyMergeIDs = new Dictionary<int, int[]>();

		// Token: 0x04003FED RID: 16365
		private float groupCreateAfterTimestamp;

		// Token: 0x04003FEE RID: 16366
		private float playEffectsAfterTimestamp;

		// Token: 0x04003FEF RID: 16367
		[SerializeField]
		private float playEffectsDelay;

		// Token: 0x04003FF0 RID: 16368
		private float suppressPartyCreationUntilTimestamp;

		// Token: 0x04003FF2 RID: 16370
		private bool WillJoinLeftHanded;

		// Token: 0x04003FF3 RID: 16371
		private List<FriendshipGroupDetection.PlayerFist> playersMakingFists = new List<FriendshipGroupDetection.PlayerFist>();

		// Token: 0x04003FF4 RID: 16372
		private StringBuilder debugStr = new StringBuilder();

		// Token: 0x04003FF5 RID: 16373
		private float aboutToGroupJoin_CooldownUntilTimestamp;

		// Token: 0x04003FF6 RID: 16374
		private static Dictionary<int, string> userIdLookup = new Dictionary<int, string>();

		// Token: 0x04003FF7 RID: 16375
		private static Dictionary<string, Color> tempColorLookup = new Dictionary<string, Color>();

		// Token: 0x020009DD RID: 2525
		private struct PlayerFist
		{
			// Token: 0x04003FF8 RID: 16376
			public int actorNumber;

			// Token: 0x04003FF9 RID: 16377
			public Vector3 position;

			// Token: 0x04003FFA RID: 16378
			public bool isLeftHand;
		}
	}
}
