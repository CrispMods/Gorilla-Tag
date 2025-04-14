using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C20 RID: 3104
	public class RCCosmeticNetworkSync : MonoBehaviourPun, IPunObservable, IPunInstantiateMagicCallback
	{
		// Token: 0x06004D66 RID: 19814 RVA: 0x00179DB0 File Offset: 0x00177FB0
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

		// Token: 0x06004D67 RID: 19815 RVA: 0x00179EE0 File Offset: 0x001780E0
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

		// Token: 0x06004D68 RID: 19816 RVA: 0x0017A094 File Offset: 0x00178294
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

		// Token: 0x06004D69 RID: 19817 RVA: 0x0017A118 File Offset: 0x00178318
		private void DestroyThis()
		{
			if (base.photonView.IsMine)
			{
				PhotonNetwork.Destroy(base.gameObject);
				return;
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x04005011 RID: 20497
		public RCCosmeticNetworkSync.SyncedState syncedState;

		// Token: 0x04005012 RID: 20498
		private RCRemoteHoldable rcRemote;

		// Token: 0x02000C21 RID: 3105
		public struct SyncedState
		{
			// Token: 0x04005013 RID: 20499
			public byte state;

			// Token: 0x04005014 RID: 20500
			public Vector3 position;

			// Token: 0x04005015 RID: 20501
			public Quaternion rotation;

			// Token: 0x04005016 RID: 20502
			public byte dataA;

			// Token: 0x04005017 RID: 20503
			public byte dataB;

			// Token: 0x04005018 RID: 20504
			public byte dataC;
		}
	}
}
