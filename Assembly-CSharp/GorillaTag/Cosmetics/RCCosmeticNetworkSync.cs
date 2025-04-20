using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C4E RID: 3150
	public class RCCosmeticNetworkSync : MonoBehaviourPun, IPunObservable, IPunInstantiateMagicCallback
	{
		// Token: 0x06004EB7 RID: 20151 RVA: 0x001B2658 File Offset: 0x001B0858
		public void OnPhotonInstantiate(PhotonMessageInfo info)
		{
			if (info.Sender == null)
			{
				this.DestroyThis();
				return;
			}
			if (info.Sender != base.photonView.Owner || base.photonView.IsRoomView)
			{
				GorillaNot.instance.SendReport("spoofed rc instantiate", info.Sender.UserId, info.Sender.NickName);
				this.DestroyThis();
				return;
			}
			object[] instantiationData = info.photonView.InstantiationData;
			if (instantiationData != null && instantiationData.Length >= 1)
			{
				object obj = instantiationData[0];
				if (obj is int)
				{
					int num = (int)obj;
					RigContainer rigContainer;
					if (VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.GetPlayer(info.Sender.ActorNumber), out rigContainer) && num > -1 && num < rigContainer.Rig.myBodyDockPositions.allObjects.Length)
					{
						this.rcRemote = (rigContainer.Rig.myBodyDockPositions.allObjects[num] as RCRemoteHoldable);
						if (this.rcRemote != null)
						{
							this.rcRemote.networkSync = this;
							this.rcRemote.WakeUpRemoteVehicle();
						}
					}
					if (this.rcRemote == null)
					{
						this.DestroyThis();
					}
					return;
				}
			}
			this.DestroyThis();
		}

		// Token: 0x06004EB8 RID: 20152 RVA: 0x001B2788 File Offset: 0x001B0988
		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (info.Sender != base.photonView.Owner)
			{
				return;
			}
			if (stream.IsWriting)
			{
				stream.SendNext(this.syncedState.state);
				stream.SendNext(this.syncedState.position);
				stream.SendNext((int)BitPackUtils.PackRotation(this.syncedState.rotation, true));
				stream.SendNext(this.syncedState.dataA);
				stream.SendNext(this.syncedState.dataB);
				stream.SendNext(this.syncedState.dataC);
				return;
			}
			if (stream.IsReading)
			{
				byte state = this.syncedState.state;
				this.syncedState.state = (byte)stream.ReceiveNext();
				Vector3 vector = (Vector3)stream.ReceiveNext();
				ref this.syncedState.position.SetValueSafe(vector);
				Quaternion quaternion = BitPackUtils.UnpackRotation((uint)((int)stream.ReceiveNext()));
				ref this.syncedState.rotation.SetValueSafe(quaternion);
				this.syncedState.dataA = (byte)stream.ReceiveNext();
				this.syncedState.dataB = (byte)stream.ReceiveNext();
				this.syncedState.dataC = (byte)stream.ReceiveNext();
				if (state != this.syncedState.state && this.rcRemote != null && this.rcRemote.Vehicle != null && !this.rcRemote.Vehicle.enabled)
				{
					this.rcRemote.WakeUpRemoteVehicle();
				}
			}
		}

		// Token: 0x06004EB9 RID: 20153 RVA: 0x001B293C File Offset: 0x001B0B3C
		[PunRPC]
		public void HitRCVehicleRPC(Vector3 hitVelocity, bool isProjectile, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "HitRCVehicleRPC");
			float num = 10000f;
			if (!hitVelocity.IsValid(num))
			{
				GorillaNot.instance.SendReport("nan rc hit", info.Sender.UserId, info.Sender.NickName);
				return;
			}
			if (this.rcRemote != null && this.rcRemote.Vehicle != null)
			{
				this.rcRemote.Vehicle.AuthorityApplyImpact(hitVelocity, isProjectile);
			}
		}

		// Token: 0x06004EBA RID: 20154 RVA: 0x0006375A File Offset: 0x0006195A
		private void DestroyThis()
		{
			if (base.photonView.IsMine)
			{
				PhotonNetwork.Destroy(base.gameObject);
				return;
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x04005107 RID: 20743
		public RCCosmeticNetworkSync.SyncedState syncedState;

		// Token: 0x04005108 RID: 20744
		private RCRemoteHoldable rcRemote;

		// Token: 0x02000C4F RID: 3151
		public struct SyncedState
		{
			// Token: 0x04005109 RID: 20745
			public byte state;

			// Token: 0x0400510A RID: 20746
			public Vector3 position;

			// Token: 0x0400510B RID: 20747
			public Quaternion rotation;

			// Token: 0x0400510C RID: 20748
			public byte dataA;

			// Token: 0x0400510D RID: 20749
			public byte dataB;

			// Token: 0x0400510E RID: 20750
			public byte dataC;
		}
	}
}
