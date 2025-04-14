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
	// Token: 0x020009B6 RID: 2486
	public class FriendshipGroupDetection : NetworkSceneObject
	{
		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06003DA0 RID: 15776 RVA: 0x001231A8 File Offset: 0x001213A8
		// (set) Token: 0x06003DA1 RID: 15777 RVA: 0x001231AF File Offset: 0x001213AF
		public static FriendshipGroupDetection Instance { get; private set; }

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06003DA2 RID: 15778 RVA: 0x001231B7 File Offset: 0x001213B7
		// (set) Token: 0x06003DA3 RID: 15779 RVA: 0x001231BF File Offset: 0x001213BF
		public List<Color> myBeadColors { get; private set; } = new List<Color>();

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06003DA4 RID: 15780 RVA: 0x001231C8 File Offset: 0x001213C8
		// (set) Token: 0x06003DA5 RID: 15781 RVA: 0x001231D0 File Offset: 0x001213D0
		public Color myBraceletColor { get; private set; }

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06003DA6 RID: 15782 RVA: 0x001231D9 File Offset: 0x001213D9
		// (set) Token: 0x06003DA7 RID: 15783 RVA: 0x001231E1 File Offset: 0x001213E1
		public int MyBraceletSelfIndex { get; private set; }

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06003DA8 RID: 15784 RVA: 0x001231EA File Offset: 0x001213EA
		public List<string> PartyMemberIDs
		{
			get
			{
				return this.myPartyMemberIDs;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06003DA9 RID: 15785 RVA: 0x001231F2 File Offset: 0x001213F2
		public bool IsInParty
		{
			get
			{
				return this.myPartyMemberIDs != null;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06003DAA RID: 15786 RVA: 0x001231FD File Offset: 0x001213FD
		// (set) Token: 0x06003DAB RID: 15787 RVA: 0x00123205 File Offset: 0x00121405
		public GroupJoinZoneAB partyZone { get; private set; }

		// Token: 0x06003DAC RID: 15788 RVA: 0x0012320E File Offset: 0x0012140E
		private void Awake()
		{
			FriendshipGroupDetection.Instance = this;
			if (this.friendshipBubble)
			{
				this.particleSystem = this.friendshipBubble.GetComponent<ParticleSystem>();
				this.audioSource = this.friendshipBubble.GetComponent<AudioSource>();
			}
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x00123245 File Offset: 0x00121445
		public void AddGroupZoneCallback(Action<GroupJoinZoneAB> callback)
		{
			this.groupZoneCallbacks.Add(callback);
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x00123253 File Offset: 0x00121453
		public void RemoveGroupZoneCallback(Action<GroupJoinZoneAB> callback)
		{
			this.groupZoneCallbacks.Remove(callback);
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x00123262 File Offset: 0x00121462
		public bool IsInMyGroup(string userID)
		{
			return this.myPartyMemberIDs != null && this.myPartyMemberIDs.Contains(userID);
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x0012327C File Offset: 0x0012147C
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

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06003DB1 RID: 15793 RVA: 0x0012330C File Offset: 0x0012150C
		// (set) Token: 0x06003DB2 RID: 15794 RVA: 0x00123314 File Offset: 0x00121514
		public bool DidJoinLeftHanded { get; private set; }

		// Token: 0x06003DB3 RID: 15795 RVA: 0x00123320 File Offset: 0x00121520
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

		// Token: 0x06003DB4 RID: 15796 RVA: 0x00123760 File Offset: 0x00121960
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

		// Token: 0x06003DB5 RID: 15797 RVA: 0x00123A8C File Offset: 0x00121C8C
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

		// Token: 0x06003DB6 RID: 15798 RVA: 0x00123C14 File Offset: 0x00121E14
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

		// Token: 0x06003DB7 RID: 15799 RVA: 0x00123C4C File Offset: 0x00121E4C
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

		// Token: 0x06003DB8 RID: 15800 RVA: 0x00123D2C File Offset: 0x00121F2C
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

		// Token: 0x06003DB9 RID: 15801 RVA: 0x00123D58 File Offset: 0x00121F58
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

		// Token: 0x06003DBA RID: 15802 RVA: 0x00123EC4 File Offset: 0x001220C4
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

		// Token: 0x06003DBB RID: 15803 RVA: 0x00123FAC File Offset: 0x001221AC
		[PunRPC]
		private void PartyMemberIsAboutToGroupJoin(PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "PartyMemberIsAboutToGroupJoin");
			this.PartMemberIsAboutToGroupJoinWrapped(new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x00123FC8 File Offset: 0x001221C8
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

		// Token: 0x06003DBD RID: 15805 RVA: 0x001240C8 File Offset: 0x001222C8
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

		// Token: 0x06003DBE RID: 15806 RVA: 0x00124134 File Offset: 0x00122334
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

		// Token: 0x06003DBF RID: 15807 RVA: 0x00124200 File Offset: 0x00122400
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

		// Token: 0x06003DC0 RID: 15808 RVA: 0x001243FC File Offset: 0x001225FC
		[PunRPC]
		private void PartyFormedSuccessfully(string partyGameMode, short braceletColor, int[] memberIDs, bool forceDebug, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "PartyFormedSuccessfully");
			this.PartyFormedSuccesfullyWrapped(partyGameMode, braceletColor, memberIDs, forceDebug, new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x0012441C File Offset: 0x0012261C
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

		// Token: 0x06003DC2 RID: 15810 RVA: 0x00124564 File Offset: 0x00122764
		[PunRPC]
		private void AddPartyMembers(string partyGameMode, short braceletColor, int[] memberIDs, PhotonMessageInfo info)
		{
			this.AddPartyMembersWrapped(partyGameMode, braceletColor, memberIDs, new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x00124578 File Offset: 0x00122778
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

		// Token: 0x06003DC4 RID: 15812 RVA: 0x0012473C File Offset: 0x0012293C
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

		// Token: 0x06003DC5 RID: 15813 RVA: 0x001247B4 File Offset: 0x001229B4
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

		// Token: 0x06003DC6 RID: 15814 RVA: 0x001248B4 File Offset: 0x00122AB4
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

		// Token: 0x06003DC7 RID: 15815 RVA: 0x00124970 File Offset: 0x00122B70
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

		// Token: 0x06003DC8 RID: 15816 RVA: 0x00124A7F File Offset: 0x00122C7F
		[PunRPC]
		private void PlayerLeftParty(PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "PlayerLeftParty");
			this.PlayerLeftPartyWrapped(new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x00124A98 File Offset: 0x00122C98
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

		// Token: 0x06003DCA RID: 15818 RVA: 0x00124AFF File Offset: 0x00122CFF
		public void SendVerifyPartyMember(NetPlayer player)
		{
			this.photonView.RPC("VerifyPartyMember", player.GetPlayerRef(), Array.Empty<object>());
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x00124B1C File Offset: 0x00122D1C
		[PunRPC]
		private void VerifyPartyMember(PhotonMessageInfo info)
		{
			this.VerifyPartyMemberWrapped(new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x00124B2C File Offset: 0x00122D2C
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

		// Token: 0x06003DCD RID: 15821 RVA: 0x00124C2C File Offset: 0x00122E2C
		private void VerifyPartyMemberWrapped(PhotonMessageInfoWrapped infoWrapped)
		{
			if (this.myPartyMemberIDs == null || !this.myPartyMemberIDs.Contains(NetworkSystem.Instance.GetUserID(infoWrapped.senderID)))
			{
				this.photonView.RPC("PlayerLeftParty", infoWrapped.Sender.GetPlayerRef(), Array.Empty<object>());
			}
		}

		// Token: 0x06003DCE RID: 15822 RVA: 0x00124C80 File Offset: 0x00122E80
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

		// Token: 0x06003DCF RID: 15823 RVA: 0x00124D28 File Offset: 0x00122F28
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

		// Token: 0x06003DD0 RID: 15824 RVA: 0x00124E65 File Offset: 0x00123065
		[PunRPC]
		private void RequestPartyGameMode(string gameMode, PhotonMessageInfo info)
		{
			this.RequestPartyGameModeWrapped(gameMode, new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x00124E74 File Offset: 0x00123074
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

		// Token: 0x06003DD2 RID: 15826 RVA: 0x00124F24 File Offset: 0x00123124
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

		// Token: 0x06003DD3 RID: 15827 RVA: 0x00125061 File Offset: 0x00123261
		[PunRPC]
		private void NotifyPartyGameModeChanged(string gameMode, PhotonMessageInfo info)
		{
			this.NotifyPartyGameModeChangedWrapped(gameMode, new PhotonMessageInfoWrapped(info));
		}

		// Token: 0x06003DD4 RID: 15828 RVA: 0x00125070 File Offset: 0x00123270
		private void NotifyPartyGameModeChangedWrapped(string gameMode, PhotonMessageInfoWrapped info)
		{
			GorillaNot.IncrementRPCCall(info, "NotifyPartyGameModeChangedWrapped");
			if (!this.IsInParty || !this.IsInMyGroup(info.Sender.UserId) || !GorillaGameModes.GameMode.IsValidGameMode(gameMode))
			{
				return;
			}
			GorillaComputer.instance.SetGameModeWithoutButton(gameMode);
		}

		// Token: 0x06003DD5 RID: 15829 RVA: 0x001250B0 File Offset: 0x001232B0
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

		// Token: 0x06003DD6 RID: 15830 RVA: 0x00125268 File Offset: 0x00123468
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

		// Token: 0x06003DD7 RID: 15831 RVA: 0x001252EC File Offset: 0x001234EC
		public static short PackColor(Color col)
		{
			return (short)(Mathf.RoundToInt(col.r * 9f) + Mathf.RoundToInt(col.g * 9f) * 10 + Mathf.RoundToInt(col.b * 9f) * 100);
		}

		// Token: 0x06003DD8 RID: 15832 RVA: 0x0012532C File Offset: 0x0012352C
		public static Color UnpackColor(short data)
		{
			return new Color
			{
				r = (float)(data % 10) / 9f,
				g = (float)(data / 10 % 10) / 9f,
				b = (float)(data / 100 % 10) / 9f
			};
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x00125468 File Offset: 0x00123668
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

		// Token: 0x06003DDC RID: 15836 RVA: 0x001254B8 File Offset: 0x001236B8
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

		// Token: 0x06003DDD RID: 15837 RVA: 0x0012555C File Offset: 0x0012375C
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

		// Token: 0x06003DDE RID: 15838 RVA: 0x001255BC File Offset: 0x001237BC
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

		// Token: 0x06003DDF RID: 15839 RVA: 0x001256CC File Offset: 0x001238CC
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

		// Token: 0x06003DE0 RID: 15840 RVA: 0x001257BC File Offset: 0x001239BC
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

		// Token: 0x06003DE1 RID: 15841 RVA: 0x0012581C File Offset: 0x00123A1C
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

		// Token: 0x06003DE2 RID: 15842 RVA: 0x0012587C File Offset: 0x00123A7C
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

		// Token: 0x06003DE3 RID: 15843 RVA: 0x00125904 File Offset: 0x00123B04
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

		// Token: 0x04003EF9 RID: 16121
		[SerializeField]
		private float detectionRadius = 0.5f;

		// Token: 0x04003EFA RID: 16122
		[SerializeField]
		private float groupTime = 5f;

		// Token: 0x04003EFB RID: 16123
		[SerializeField]
		private float cooldownAfterCreatingGroup = 5f;

		// Token: 0x04003EFC RID: 16124
		[SerializeField]
		private float hapticStrength = 1.5f;

		// Token: 0x04003EFD RID: 16125
		[SerializeField]
		private float hapticDuration = 2f;

		// Token: 0x04003EFE RID: 16126
		public bool debug;

		// Token: 0x04003EFF RID: 16127
		public double offset = 0.5;

		// Token: 0x04003F00 RID: 16128
		[SerializeField]
		private float m_maxGroupJoinTimeDifference = 1f;

		// Token: 0x04003F01 RID: 16129
		private List<string> myPartyMemberIDs;

		// Token: 0x04003F02 RID: 16130
		private HashSet<string> myPartyMembersHash = new HashSet<string>();

		// Token: 0x04003F07 RID: 16135
		private List<Action<GroupJoinZoneAB>> groupZoneCallbacks = new List<Action<GroupJoinZoneAB>>();

		// Token: 0x04003F08 RID: 16136
		[SerializeField]
		private GTColor.HSVRanges braceletRandomColorHSVRanges;

		// Token: 0x04003F09 RID: 16137
		public GameObject friendshipBubble;

		// Token: 0x04003F0A RID: 16138
		public AudioClip fistBumpInterruptedAudio;

		// Token: 0x04003F0B RID: 16139
		private ParticleSystem particleSystem;

		// Token: 0x04003F0C RID: 16140
		private AudioSource audioSource;

		// Token: 0x04003F0D RID: 16141
		private Queue<FriendshipGroupDetection.PlayerFist> playersToPropagateFrom = new Queue<FriendshipGroupDetection.PlayerFist>();

		// Token: 0x04003F0E RID: 16142
		private List<int> playersInProvisionalGroup = new List<int>();

		// Token: 0x04003F0F RID: 16143
		private List<int> provisionalGroupUsingLeftHands = new List<int>();

		// Token: 0x04003F10 RID: 16144
		private List<int> tempIntList = new List<int>();

		// Token: 0x04003F11 RID: 16145
		private bool amFirstProvisionalPlayer;

		// Token: 0x04003F12 RID: 16146
		private Dictionary<int, int[]> partyMergeIDs = new Dictionary<int, int[]>();

		// Token: 0x04003F13 RID: 16147
		private float groupCreateAfterTimestamp;

		// Token: 0x04003F14 RID: 16148
		private float playEffectsAfterTimestamp;

		// Token: 0x04003F15 RID: 16149
		[SerializeField]
		private float playEffectsDelay;

		// Token: 0x04003F16 RID: 16150
		private float suppressPartyCreationUntilTimestamp;

		// Token: 0x04003F18 RID: 16152
		private bool WillJoinLeftHanded;

		// Token: 0x04003F19 RID: 16153
		private List<FriendshipGroupDetection.PlayerFist> playersMakingFists = new List<FriendshipGroupDetection.PlayerFist>();

		// Token: 0x04003F1A RID: 16154
		private StringBuilder debugStr = new StringBuilder();

		// Token: 0x04003F1B RID: 16155
		private float aboutToGroupJoin_CooldownUntilTimestamp;

		// Token: 0x04003F1C RID: 16156
		private static Dictionary<int, string> userIdLookup = new Dictionary<int, string>();

		// Token: 0x04003F1D RID: 16157
		private static Dictionary<string, Color> tempColorLookup = new Dictionary<string, Color>();

		// Token: 0x020009B7 RID: 2487
		private struct PlayerFist
		{
			// Token: 0x04003F1E RID: 16158
			public int actorNumber;

			// Token: 0x04003F1F RID: 16159
			public Vector3 position;

			// Token: 0x04003F20 RID: 16160
			public bool isLeftHand;
		}
	}
}
