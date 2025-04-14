using System;
using BoingKit;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts.Builder
{
	// Token: 0x020009FE RID: 2558
	public class BuilderPieceDoor : MonoBehaviour, IBuilderPieceComponent, IBuilderPieceFunctional
	{
		// Token: 0x06003FF9 RID: 16377 RVA: 0x0012FE80 File Offset: 0x0012E080
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

		// Token: 0x06003FFA RID: 16378 RVA: 0x0012FEF8 File Offset: 0x0012E0F8
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

		// Token: 0x06003FFB RID: 16379 RVA: 0x0012FF70 File Offset: 0x0012E170
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

		// Token: 0x06003FFC RID: 16380 RVA: 0x0012FFD4 File Offset: 0x0012E1D4
		private void UpdateDoorStateMaster()
		{
			switch (this.currentState)
			{
			case BuilderPieceDoor.DoorState.Closing:
				if (this.doorSpring.Value < 1f)
				{
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

		// Token: 0x06003FFD RID: 16381 RVA: 0x00130180 File Offset: 0x0012E380
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
				this.SetDoorState(BuilderPieceDoor.DoorState.Closed);
			}
		}

		// Token: 0x06003FFE RID: 16382 RVA: 0x001301D0 File Offset: 0x0012E3D0
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

		// Token: 0x06003FFF RID: 16383 RVA: 0x0013021C File Offset: 0x0012E41C
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

		// Token: 0x06004000 RID: 16384 RVA: 0x00130260 File Offset: 0x0012E460
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

		// Token: 0x06004001 RID: 16385 RVA: 0x001303A0 File Offset: 0x0012E5A0
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

		// Token: 0x06004002 RID: 16386 RVA: 0x00130458 File Offset: 0x0012E658
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

		// Token: 0x06004003 RID: 16387 RVA: 0x001304F0 File Offset: 0x0012E6F0
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

		// Token: 0x06004004 RID: 16388 RVA: 0x00130578 File Offset: 0x0012E778
		public void OnPieceCreate(int pieceType, int pieceId)
		{
			this.tLastOpened = 0f;
			this.SetDoorState(BuilderPieceDoor.DoorState.Closed);
			this.doorSpring.Reset();
			Collider[] array = this.triggerVolumes;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPieceDestroy()
		{
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x001305C0 File Offset: 0x0012E7C0
		public void OnPieceActivate()
		{
			Collider[] array = this.triggerVolumes;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = true;
			}
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x001305EC File Offset: 0x0012E7EC
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
			this.doorTransform.localRotation = Quaternion.Euler(this.rotateAxis * this.doorSpring.Value);
			if (this.isDoubleDoor && this.doorTransformB != null)
			{
				this.doorTransformB.localRotation = Quaternion.Euler(this.rotateAxisB * this.doorSpring.Value);
			}
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x00130698 File Offset: 0x0012E898
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

		// Token: 0x0600400A RID: 16394 RVA: 0x001306E4 File Offset: 0x0012E8E4
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

		// Token: 0x0600400B RID: 16395 RVA: 0x0013075C File Offset: 0x0012E95C
		public bool IsStateValid(byte state)
		{
			return state < 5;
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x00130764 File Offset: 0x0012E964
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

		// Token: 0x0400411D RID: 16669
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x0400411E RID: 16670
		[SerializeField]
		private Vector3 rotateAxis = Vector3.up;

		// Token: 0x0400411F RID: 16671
		[Tooltip("True if the door stays open until the button is triggered again")]
		[SerializeField]
		private bool IsToggled;

		// Token: 0x04004120 RID: 16672
		[Tooltip("True if the door opens when players enter the Keep Open Trigger")]
		[SerializeField]
		private bool isAutomatic;

		// Token: 0x04004121 RID: 16673
		[SerializeField]
		private Transform doorTransform;

		// Token: 0x04004122 RID: 16674
		[SerializeField]
		private Collider[] triggerVolumes;

		// Token: 0x04004123 RID: 16675
		[SerializeField]
		private BuilderSmallHandTrigger[] doorButtonTriggers;

		// Token: 0x04004124 RID: 16676
		[SerializeField]
		private BuilderSmallMonkeTrigger[] doorHoldTriggers;

		// Token: 0x04004125 RID: 16677
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04004126 RID: 16678
		[SerializeField]
		private SoundBankPlayer openSound;

		// Token: 0x04004127 RID: 16679
		[SerializeField]
		private SoundBankPlayer closeSound;

		// Token: 0x04004128 RID: 16680
		[SerializeField]
		private float doorOpenSpeed = 1f;

		// Token: 0x04004129 RID: 16681
		[SerializeField]
		private float doorCloseSpeed = 1f;

		// Token: 0x0400412A RID: 16682
		[SerializeField]
		[Range(1.5f, 10f)]
		private float timeUntilDoorCloses = 3f;

		// Token: 0x0400412B RID: 16683
		[Header("Double Door Settings")]
		[SerializeField]
		private bool isDoubleDoor;

		// Token: 0x0400412C RID: 16684
		[SerializeField]
		private Vector3 rotateAxisB = Vector3.down;

		// Token: 0x0400412D RID: 16685
		[SerializeField]
		private Transform doorTransformB;

		// Token: 0x0400412E RID: 16686
		private BuilderPieceDoor.DoorState currentState;

		// Token: 0x0400412F RID: 16687
		private float tLastOpened;

		// Token: 0x04004130 RID: 16688
		private FloatSpring doorSpring;

		// Token: 0x04004131 RID: 16689
		private bool peopleInHoldOpenVolume;

		// Token: 0x04004132 RID: 16690
		private double CheckHoldTriggersTime;

		// Token: 0x04004133 RID: 16691
		private float checkHoldTriggersDelay = 3f;

		// Token: 0x020009FF RID: 2559
		public enum DoorState
		{
			// Token: 0x04004135 RID: 16693
			Closed,
			// Token: 0x04004136 RID: 16694
			Closing,
			// Token: 0x04004137 RID: 16695
			Open,
			// Token: 0x04004138 RID: 16696
			Opening,
			// Token: 0x04004139 RID: 16697
			HeldOpen
		}
	}
}
