using System;
using BoingKit;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A2B RID: 2603
	public class BuilderPieceDoor : MonoBehaviour, IBuilderPieceComponent, IBuilderPieceFunctional
	{
		// Token: 0x0600413E RID: 16702 RVA: 0x00170D7C File Offset: 0x0016EF7C
		private void Awake()
		{
			foreach (BuilderSmallMonkeTrigger builderSmallMonkeTrigger in this.doorHoldTriggers)
			{
				builderSmallMonkeTrigger.onTriggerFirstEntered += this.OnHoldTriggerEntered;
				builderSmallMonkeTrigger.onTriggerLastExited += this.OnHoldTriggerExited;
			}
			BuilderSmallHandTrigger[] array2 = this.doorButtonTriggers;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].TriggeredEvent.AddListener(new UnityAction(this.OnDoorButtonTriggered));
			}
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x00170DF4 File Offset: 0x0016EFF4
		private void OnDestroy()
		{
			foreach (BuilderSmallMonkeTrigger builderSmallMonkeTrigger in this.doorHoldTriggers)
			{
				builderSmallMonkeTrigger.onTriggerFirstEntered -= this.OnHoldTriggerEntered;
				builderSmallMonkeTrigger.onTriggerLastExited -= this.OnHoldTriggerExited;
			}
			BuilderSmallHandTrigger[] array2 = this.doorButtonTriggers;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].TriggeredEvent.RemoveListener(new UnityAction(this.OnDoorButtonTriggered));
			}
		}

		// Token: 0x06004140 RID: 16704 RVA: 0x00170E6C File Offset: 0x0016F06C
		private void SetDoorState(BuilderPieceDoor.DoorState value)
		{
			bool flag = this.currentState == BuilderPieceDoor.DoorState.Closed || (this.currentState == BuilderPieceDoor.DoorState.Open && this.IsToggled);
			bool flag2 = value == BuilderPieceDoor.DoorState.Closed || (value == BuilderPieceDoor.DoorState.Open && this.IsToggled);
			this.currentState = value;
			if (flag != flag2)
			{
				if (flag2)
				{
					BuilderTable.instance.UnregisterFunctionalPiece(this);
					return;
				}
				BuilderTable.instance.RegisterFunctionalPiece(this);
			}
		}

		// Token: 0x06004141 RID: 16705 RVA: 0x00170ED0 File Offset: 0x0016F0D0
		private void UpdateDoorStateMaster()
		{
			switch (this.currentState)
			{
			case BuilderPieceDoor.DoorState.Closing:
				if (this.doorSpring.Value < 1f)
				{
					this.doorSpring.Reset();
					this.doorTransform.localRotation = Quaternion.identity;
					if (this.isDoubleDoor && this.doorTransformB != null)
					{
						this.doorTransformB.localRotation = Quaternion.identity;
					}
					this.SetDoorState(BuilderPieceDoor.DoorState.Closed);
					return;
				}
				break;
			case BuilderPieceDoor.DoorState.Open:
				if (!this.IsToggled && Time.time - this.tLastOpened > this.timeUntilDoorCloses)
				{
					this.peopleInHoldOpenVolume = false;
					foreach (BuilderSmallMonkeTrigger builderSmallMonkeTrigger in this.doorHoldTriggers)
					{
						builderSmallMonkeTrigger.ValidateOverlappingColliders();
						if (builderSmallMonkeTrigger.overlapCount > 0)
						{
							this.peopleInHoldOpenVolume = true;
							break;
						}
					}
					if (this.peopleInHoldOpenVolume)
					{
						this.CheckHoldTriggersTime = (double)(Time.time + this.checkHoldTriggersDelay);
						BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 4, PhotonNetwork.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
						return;
					}
					BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 1, PhotonNetwork.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
					return;
				}
				break;
			case BuilderPieceDoor.DoorState.Opening:
				if (this.doorSpring.Value > 89f)
				{
					this.SetDoorState(BuilderPieceDoor.DoorState.Open);
					return;
				}
				break;
			case BuilderPieceDoor.DoorState.HeldOpen:
				if (!this.IsToggled && (double)Time.time > this.CheckHoldTriggersTime)
				{
					foreach (BuilderSmallMonkeTrigger builderSmallMonkeTrigger2 in this.doorHoldTriggers)
					{
						builderSmallMonkeTrigger2.ValidateOverlappingColliders();
						if (builderSmallMonkeTrigger2.overlapCount > 0)
						{
							this.peopleInHoldOpenVolume = true;
							break;
						}
					}
					if (this.peopleInHoldOpenVolume)
					{
						this.CheckHoldTriggersTime = (double)(Time.time + this.checkHoldTriggersDelay);
						return;
					}
					BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 1, PhotonNetwork.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06004142 RID: 16706 RVA: 0x001710C0 File Offset: 0x0016F2C0
		private void UpdateDoorState()
		{
			BuilderPieceDoor.DoorState doorState = this.currentState;
			if (doorState != BuilderPieceDoor.DoorState.Closing)
			{
				if (doorState == BuilderPieceDoor.DoorState.Opening && this.doorSpring.Value > 89f)
				{
					this.SetDoorState(BuilderPieceDoor.DoorState.Open);
					return;
				}
			}
			else if (this.doorSpring.Value < 1f)
			{
				this.doorSpring.Reset();
				this.doorTransform.localRotation = Quaternion.identity;
				if (this.isDoubleDoor && this.doorTransformB != null)
				{
					this.doorTransformB.localRotation = Quaternion.identity;
				}
				this.SetDoorState(BuilderPieceDoor.DoorState.Closed);
			}
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x00171150 File Offset: 0x0016F350
		private void CloseDoor()
		{
			switch (this.currentState)
			{
			case BuilderPieceDoor.DoorState.Closed:
			case BuilderPieceDoor.DoorState.Closing:
			case BuilderPieceDoor.DoorState.Opening:
				return;
			case BuilderPieceDoor.DoorState.Open:
			case BuilderPieceDoor.DoorState.HeldOpen:
				this.closeSound.Play();
				this.SetDoorState(BuilderPieceDoor.DoorState.Closing);
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06004144 RID: 16708 RVA: 0x0017119C File Offset: 0x0016F39C
		private void OpenDoor()
		{
			BuilderPieceDoor.DoorState doorState = this.currentState;
			if (doorState == BuilderPieceDoor.DoorState.Closed)
			{
				this.tLastOpened = Time.time;
				this.openSound.Play();
				this.SetDoorState(BuilderPieceDoor.DoorState.Opening);
				return;
			}
			if (doorState - BuilderPieceDoor.DoorState.Closing > 3)
			{
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06004145 RID: 16709 RVA: 0x001711E0 File Offset: 0x0016F3E0
		private void UpdateDoorAnimation()
		{
			BuilderPieceDoor.DoorState doorState = this.currentState;
			if (doorState > BuilderPieceDoor.DoorState.Closing && doorState - BuilderPieceDoor.DoorState.Open <= 2)
			{
				this.doorSpring.TrackDampingRatio(90f, 3.1415927f * this.doorOpenSpeed, 1f, Time.deltaTime);
				this.doorTransform.localRotation = Quaternion.Euler(this.rotateAxis * this.doorSpring.Value);
				if (this.isDoubleDoor && this.doorTransformB != null)
				{
					this.doorTransformB.localRotation = Quaternion.Euler(this.rotateAxisB * this.doorSpring.Value);
					return;
				}
			}
			else
			{
				this.doorSpring.TrackDampingRatio(0f, 3.1415927f * this.doorCloseSpeed, 1f, Time.deltaTime);
				this.doorTransform.localRotation = Quaternion.Euler(this.rotateAxis * this.doorSpring.Value);
				if (this.isDoubleDoor && this.doorTransformB != null)
				{
					this.doorTransformB.localRotation = Quaternion.Euler(this.rotateAxisB * this.doorSpring.Value);
				}
			}
		}

		// Token: 0x06004146 RID: 16710 RVA: 0x00171320 File Offset: 0x0016F520
		private void OnDoorButtonTriggered()
		{
			BuilderPieceDoor.DoorState doorState = this.currentState;
			if (doorState != BuilderPieceDoor.DoorState.Closed)
			{
				if (doorState != BuilderPieceDoor.DoorState.Open)
				{
					return;
				}
				if (this.IsToggled)
				{
					if (NetworkSystem.Instance.IsMasterClient)
					{
						BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 1, PhotonNetwork.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
						return;
					}
					BuilderTableNetworking.instance.RequestFunctionalPieceStateChange(this.myPiece.pieceId, 1);
				}
				return;
			}
			else
			{
				if (NetworkSystem.Instance.IsMasterClient)
				{
					BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 3, PhotonNetwork.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
					return;
				}
				BuilderTableNetworking.instance.RequestFunctionalPieceStateChange(this.myPiece.pieceId, 3);
				return;
			}
		}

		// Token: 0x06004147 RID: 16711 RVA: 0x001713D8 File Offset: 0x0016F5D8
		private void OnHoldTriggerEntered()
		{
			this.peopleInHoldOpenVolume = true;
			if (!NetworkSystem.Instance.IsMasterClient)
			{
				return;
			}
			BuilderPieceDoor.DoorState doorState = this.currentState;
			if (doorState != BuilderPieceDoor.DoorState.Closed)
			{
				if (doorState != BuilderPieceDoor.DoorState.Closing)
				{
					return;
				}
				if (!this.IsToggled)
				{
					this.openSound.Play();
					BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 4, PhotonNetwork.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
				}
			}
			else if (this.isAutomatic)
			{
				BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 3, PhotonNetwork.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
				return;
			}
		}

		// Token: 0x06004148 RID: 16712 RVA: 0x00171470 File Offset: 0x0016F670
		private void OnHoldTriggerExited()
		{
			this.peopleInHoldOpenVolume = false;
			foreach (BuilderSmallMonkeTrigger builderSmallMonkeTrigger in this.doorHoldTriggers)
			{
				builderSmallMonkeTrigger.ValidateOverlappingColliders();
				if (builderSmallMonkeTrigger.overlapCount > 0)
				{
					this.peopleInHoldOpenVolume = true;
					break;
				}
			}
			if (!NetworkSystem.Instance.IsMasterClient)
			{
				return;
			}
			if (this.currentState == BuilderPieceDoor.DoorState.HeldOpen && !this.peopleInHoldOpenVolume)
			{
				BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 1, PhotonNetwork.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
			}
		}

		// Token: 0x06004149 RID: 16713 RVA: 0x001714F8 File Offset: 0x0016F6F8
		public void OnPieceCreate(int pieceType, int pieceId)
		{
			this.tLastOpened = 0f;
			this.SetDoorState(BuilderPieceDoor.DoorState.Closed);
			this.doorSpring.Reset();
			this.doorTransform.localRotation = Quaternion.identity;
			if (this.isDoubleDoor && this.doorTransformB != null)
			{
				this.doorTransformB.localRotation = Quaternion.identity;
			}
			Collider[] array = this.triggerVolumes;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
		}

		// Token: 0x0600414A RID: 16714 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPieceDestroy()
		{
		}

		// Token: 0x0600414B RID: 16715 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x0600414C RID: 16716 RVA: 0x00171578 File Offset: 0x0016F778
		public void OnPieceActivate()
		{
			Collider[] array = this.triggerVolumes;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = true;
			}
		}

		// Token: 0x0600414D RID: 16717 RVA: 0x001715A4 File Offset: 0x0016F7A4
		public void OnPieceDeactivate()
		{
			Collider[] array = this.triggerVolumes;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
			this.myPiece.functionalPieceState = 0;
			this.SetDoorState(BuilderPieceDoor.DoorState.Closed);
			this.doorSpring.Reset();
			this.doorTransform.localRotation = Quaternion.identity;
			if (this.isDoubleDoor && this.doorTransformB != null)
			{
				this.doorTransformB.localRotation = Quaternion.identity;
			}
		}

		// Token: 0x0600414E RID: 16718 RVA: 0x00171624 File Offset: 0x0016F824
		public void OnStateRequest(byte newState, NetPlayer instigator, int timeStamp)
		{
			if (!NetworkSystem.Instance.IsMasterClient)
			{
				return;
			}
			if (this.IsStateValid(newState) && instigator != null && this.currentState != (BuilderPieceDoor.DoorState)newState)
			{
				BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, newState, instigator.GetPlayerRef(), timeStamp);
			}
		}

		// Token: 0x0600414F RID: 16719 RVA: 0x00171670 File Offset: 0x0016F870
		public void OnStateChanged(byte newState, NetPlayer instigator, int timeStamp)
		{
			if (!this.IsStateValid(newState))
			{
				return;
			}
			switch (newState)
			{
			case 1:
				if (this.currentState == BuilderPieceDoor.DoorState.Open || this.currentState == BuilderPieceDoor.DoorState.HeldOpen)
				{
					this.CloseDoor();
				}
				break;
			case 3:
				if (this.currentState == BuilderPieceDoor.DoorState.Closed)
				{
					this.OpenDoor();
				}
				break;
			case 4:
				if (this.currentState == BuilderPieceDoor.DoorState.Closing)
				{
					this.openSound.Play();
				}
				break;
			}
			this.SetDoorState((BuilderPieceDoor.DoorState)newState);
		}

		// Token: 0x06004150 RID: 16720 RVA: 0x0005AA86 File Offset: 0x00058C86
		public bool IsStateValid(byte state)
		{
			return state < 5;
		}

		// Token: 0x06004151 RID: 16721 RVA: 0x001716E8 File Offset: 0x0016F8E8
		public void FunctionalPieceUpdate()
		{
			if (this.myPiece != null && this.myPiece.state == BuilderPiece.State.AttachedAndPlaced)
			{
				if (!NetworkSystem.Instance.InRoom && this.currentState != BuilderPieceDoor.DoorState.Closed)
				{
					this.CloseDoor();
				}
				else if (NetworkSystem.Instance.IsMasterClient)
				{
					this.UpdateDoorStateMaster();
				}
				else
				{
					this.UpdateDoorState();
				}
				this.UpdateDoorAnimation();
			}
		}

		// Token: 0x04004217 RID: 16919
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x04004218 RID: 16920
		[SerializeField]
		private Vector3 rotateAxis = Vector3.up;

		// Token: 0x04004219 RID: 16921
		[Tooltip("True if the door stays open until the button is triggered again")]
		[SerializeField]
		private bool IsToggled;

		// Token: 0x0400421A RID: 16922
		[Tooltip("True if the door opens when players enter the Keep Open Trigger")]
		[SerializeField]
		private bool isAutomatic;

		// Token: 0x0400421B RID: 16923
		[SerializeField]
		private Transform doorTransform;

		// Token: 0x0400421C RID: 16924
		[SerializeField]
		private Collider[] triggerVolumes;

		// Token: 0x0400421D RID: 16925
		[SerializeField]
		private BuilderSmallHandTrigger[] doorButtonTriggers;

		// Token: 0x0400421E RID: 16926
		[SerializeField]
		private BuilderSmallMonkeTrigger[] doorHoldTriggers;

		// Token: 0x0400421F RID: 16927
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04004220 RID: 16928
		[SerializeField]
		private SoundBankPlayer openSound;

		// Token: 0x04004221 RID: 16929
		[SerializeField]
		private SoundBankPlayer closeSound;

		// Token: 0x04004222 RID: 16930
		[SerializeField]
		private float doorOpenSpeed = 1f;

		// Token: 0x04004223 RID: 16931
		[SerializeField]
		private float doorCloseSpeed = 1f;

		// Token: 0x04004224 RID: 16932
		[SerializeField]
		[Range(1.5f, 10f)]
		private float timeUntilDoorCloses = 3f;

		// Token: 0x04004225 RID: 16933
		[Header("Double Door Settings")]
		[SerializeField]
		private bool isDoubleDoor;

		// Token: 0x04004226 RID: 16934
		[SerializeField]
		private Vector3 rotateAxisB = Vector3.down;

		// Token: 0x04004227 RID: 16935
		[SerializeField]
		private Transform doorTransformB;

		// Token: 0x04004228 RID: 16936
		private BuilderPieceDoor.DoorState currentState;

		// Token: 0x04004229 RID: 16937
		private float tLastOpened;

		// Token: 0x0400422A RID: 16938
		private FloatSpring doorSpring;

		// Token: 0x0400422B RID: 16939
		private bool peopleInHoldOpenVolume;

		// Token: 0x0400422C RID: 16940
		private double CheckHoldTriggersTime;

		// Token: 0x0400422D RID: 16941
		private float checkHoldTriggersDelay = 3f;

		// Token: 0x02000A2C RID: 2604
		public enum DoorState
		{
			// Token: 0x0400422F RID: 16943
			Closed,
			// Token: 0x04004230 RID: 16944
			Closing,
			// Token: 0x04004231 RID: 16945
			Open,
			// Token: 0x04004232 RID: 16946
			Opening,
			// Token: 0x04004233 RID: 16947
			HeldOpen
		}
	}
}
