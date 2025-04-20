using System;
using BoingKit;
using Fusion;
using GorillaTag;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;

// Token: 0x020001C0 RID: 448
public class GTDoor : NetworkSceneObject
{
	// Token: 0x06000A9A RID: 2714 RVA: 0x00097958 File Offset: 0x00095B58
	protected override void Start()
	{
		base.Start();
		Collider[] array = this.doorColliders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}
		this.tLastOpened = 0f;
		GTDoorTrigger[] array2 = this.doorButtonTriggers;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].TriggeredEvent.AddListener(new UnityAction(this.DoorButtonTriggered));
		}
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x000979C4 File Offset: 0x00095BC4
	private void Update()
	{
		if (this.currentState == GTDoor.DoorState.Open || this.currentState == GTDoor.DoorState.Closed)
		{
			if (Time.time < this.lastChecked + this.secondsCheck)
			{
				return;
			}
			this.lastChecked = Time.time;
		}
		this.UpdateDoorState();
		this.UpdateDoorAnimation();
		Collider[] array;
		if (this.currentState == GTDoor.DoorState.Closed)
		{
			array = this.doorColliders;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = true;
			}
			return;
		}
		array = this.doorColliders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x00097A54 File Offset: 0x00095C54
	private void UpdateDoorState()
	{
		this.peopleInHoldOpenVolume = false;
		foreach (GTDoorTrigger gtdoorTrigger in this.doorHoldOpenTriggers)
		{
			gtdoorTrigger.ValidateOverlappingColliders();
			if (gtdoorTrigger.overlapCount > 0)
			{
				this.peopleInHoldOpenVolume = true;
				break;
			}
		}
		switch (this.currentState)
		{
		case GTDoor.DoorState.Closed:
			if (this.buttonTriggeredThisFrame)
			{
				this.buttonTriggeredThisFrame = false;
				if (!NetworkSystem.Instance.InRoom)
				{
					this.OpenDoor();
				}
				else
				{
					this.currentState = GTDoor.DoorState.OpeningWaitingOnRPC;
					this.photonView.RPC("ChangeDoorState", RpcTarget.AllViaServer, new object[]
					{
						GTDoor.DoorState.Opening
					});
				}
			}
			break;
		case GTDoor.DoorState.ClosingWaitingOnRPC:
		case GTDoor.DoorState.OpeningWaitingOnRPC:
			break;
		case GTDoor.DoorState.Closing:
			if (this.doorSpring.Value < 1f)
			{
				this.currentState = GTDoor.DoorState.Closed;
			}
			if (this.peopleInHoldOpenVolume)
			{
				this.currentState = GTDoor.DoorState.HeldOpenLocally;
				if (NetworkSystem.Instance.InRoom && base.IsMine)
				{
					this.photonView.RPC("ChangeDoorState", RpcTarget.AllViaServer, new object[]
					{
						GTDoor.DoorState.HeldOpen
					});
				}
				this.audioSource.GTPlayOneShot(this.openSound, 1f);
			}
			break;
		case GTDoor.DoorState.Open:
			if (Time.time - this.tLastOpened > this.timeUntilDoorCloses)
			{
				if (this.peopleInHoldOpenVolume)
				{
					this.currentState = GTDoor.DoorState.HeldOpenLocally;
					if (NetworkSystem.Instance.InRoom && base.IsMine)
					{
						this.photonView.RPC("ChangeDoorState", RpcTarget.AllViaServer, new object[]
						{
							GTDoor.DoorState.HeldOpen
						});
					}
				}
				else if (!NetworkSystem.Instance.InRoom)
				{
					this.CloseDoor();
				}
				else if (base.IsMine)
				{
					this.currentState = GTDoor.DoorState.ClosingWaitingOnRPC;
					this.photonView.RPC("ChangeDoorState", RpcTarget.AllViaServer, new object[]
					{
						GTDoor.DoorState.Closing
					});
				}
			}
			break;
		case GTDoor.DoorState.Opening:
			if (this.doorSpring.Value > 89f)
			{
				this.currentState = GTDoor.DoorState.Open;
			}
			break;
		case GTDoor.DoorState.HeldOpen:
			if (!this.peopleInHoldOpenVolume)
			{
				if (!NetworkSystem.Instance.InRoom)
				{
					this.CloseDoor();
				}
				else if (base.IsMine)
				{
					this.currentState = GTDoor.DoorState.ClosingWaitingOnRPC;
					this.photonView.RPC("ChangeDoorState", RpcTarget.AllViaServer, new object[]
					{
						GTDoor.DoorState.Closing
					});
				}
			}
			break;
		case GTDoor.DoorState.HeldOpenLocally:
			if (!this.peopleInHoldOpenVolume)
			{
				this.CloseDoor();
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		if (!NetworkSystem.Instance.InRoom)
		{
			GTDoor.DoorState doorState = this.currentState;
			if (doorState == GTDoor.DoorState.ClosingWaitingOnRPC)
			{
				this.CloseDoor();
				return;
			}
			if (doorState != GTDoor.DoorState.OpeningWaitingOnRPC)
			{
				return;
			}
			this.OpenDoor();
		}
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x00097CF8 File Offset: 0x00095EF8
	private void DoorButtonTriggered()
	{
		GTDoor.DoorState doorState = this.currentState;
		if (doorState - GTDoor.DoorState.Open <= 4)
		{
			return;
		}
		this.buttonTriggeredThisFrame = true;
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x00097D1C File Offset: 0x00095F1C
	private void OpenDoor()
	{
		switch (this.currentState)
		{
		case GTDoor.DoorState.Closed:
		case GTDoor.DoorState.OpeningWaitingOnRPC:
			this.ResetDoorOpenedTime();
			this.audioSource.GTPlayOneShot(this.openSound, 1f);
			this.currentState = GTDoor.DoorState.Opening;
			return;
		case GTDoor.DoorState.ClosingWaitingOnRPC:
		case GTDoor.DoorState.Closing:
		case GTDoor.DoorState.Open:
		case GTDoor.DoorState.Opening:
		case GTDoor.DoorState.HeldOpen:
		case GTDoor.DoorState.HeldOpenLocally:
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x00097D84 File Offset: 0x00095F84
	private void CloseDoor()
	{
		switch (this.currentState)
		{
		case GTDoor.DoorState.Closed:
		case GTDoor.DoorState.Closing:
		case GTDoor.DoorState.OpeningWaitingOnRPC:
		case GTDoor.DoorState.Opening:
			return;
		case GTDoor.DoorState.ClosingWaitingOnRPC:
		case GTDoor.DoorState.Open:
		case GTDoor.DoorState.HeldOpen:
		case GTDoor.DoorState.HeldOpenLocally:
			this.audioSource.GTPlayOneShot(this.closeSound, 1f);
			this.currentState = GTDoor.DoorState.Closing;
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x00097DE4 File Offset: 0x00095FE4
	private void UpdateDoorAnimation()
	{
		switch (this.currentState)
		{
		case GTDoor.DoorState.ClosingWaitingOnRPC:
		case GTDoor.DoorState.Open:
		case GTDoor.DoorState.Opening:
		case GTDoor.DoorState.HeldOpen:
		case GTDoor.DoorState.HeldOpenLocally:
			this.doorSpring.TrackDampingRatio(90f, 3.1415927f * this.doorOpenSpeed, 1f, Time.deltaTime);
			this.doorTransform.localRotation = Quaternion.Euler(new Vector3(0f, this.doorSpring.Value, 0f));
			return;
		}
		this.doorSpring.TrackDampingRatio(0f, 3.1415927f * this.doorCloseSpeed, 1f, Time.deltaTime);
		this.doorTransform.localRotation = Quaternion.Euler(new Vector3(0f, this.doorSpring.Value, 0f));
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x000376D8 File Offset: 0x000358D8
	public void ResetDoorOpenedTime()
	{
		this.tLastOpened = Time.time;
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x000376E5 File Offset: 0x000358E5
	[PunRPC]
	public void ChangeDoorState(GTDoor.DoorState shouldOpenState)
	{
		this.ChangeDoorStateShared(shouldOpenState);
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x00097EC4 File Offset: 0x000960C4
	[Rpc]
	public unsafe static void RPC_ChangeDoorState(NetworkRunner runner, GTDoor.DoorState shouldOpenState, int doorId)
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
				int num = 8;
				num += 4;
				num += 4;
				SimulationMessage* ptr = SimulationMessage.Allocate(runner.Simulation, num);
				byte* data = SimulationMessage.GetData(ptr);
				int num2 = RpcHeader.Write(RpcHeader.Create(NetworkBehaviourUtils.GetRpcStaticIndexOrThrow("System.Void GTDoor::RPC_ChangeDoorState(Fusion.NetworkRunner,GTDoor/DoorState,System.Int32)")), data);
				*(GTDoor.DoorState*)(data + num2) = shouldOpenState;
				num2 += 4;
				*(int*)(data + num2) = doorId;
				num2 += 4;
				ptr->Offset = num2 * 8;
				ptr->SetStatic();
				runner.SendRpc(ptr);
			}
		}
		GTDoor[] array = UnityEngine.Object.FindObjectsOfType<GTDoor>(true);
		if (array == null || array.Length == 0)
		{
			return;
		}
		foreach (GTDoor gtdoor in array)
		{
			if (gtdoor.GTDoorID == doorId)
			{
				gtdoor.ChangeDoorStateShared(shouldOpenState);
			}
		}
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x00097FFC File Offset: 0x000961FC
	private void ChangeDoorStateShared(GTDoor.DoorState shouldOpenState)
	{
		switch (shouldOpenState)
		{
		case GTDoor.DoorState.Closed:
		case GTDoor.DoorState.ClosingWaitingOnRPC:
		case GTDoor.DoorState.Open:
		case GTDoor.DoorState.OpeningWaitingOnRPC:
		case GTDoor.DoorState.HeldOpenLocally:
			break;
		case GTDoor.DoorState.Closing:
			switch (this.currentState)
			{
			case GTDoor.DoorState.Closed:
			case GTDoor.DoorState.Closing:
			case GTDoor.DoorState.OpeningWaitingOnRPC:
			case GTDoor.DoorState.Opening:
			case GTDoor.DoorState.HeldOpenLocally:
				break;
			case GTDoor.DoorState.ClosingWaitingOnRPC:
			case GTDoor.DoorState.Open:
			case GTDoor.DoorState.HeldOpen:
				this.CloseDoor();
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
			break;
		case GTDoor.DoorState.Opening:
			switch (this.currentState)
			{
			case GTDoor.DoorState.Closed:
			case GTDoor.DoorState.OpeningWaitingOnRPC:
				this.OpenDoor();
				return;
			case GTDoor.DoorState.ClosingWaitingOnRPC:
			case GTDoor.DoorState.Closing:
			case GTDoor.DoorState.Open:
			case GTDoor.DoorState.Opening:
			case GTDoor.DoorState.HeldOpen:
			case GTDoor.DoorState.HeldOpenLocally:
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			break;
		case GTDoor.DoorState.HeldOpen:
			switch (this.currentState)
			{
			case GTDoor.DoorState.Closed:
			case GTDoor.DoorState.ClosingWaitingOnRPC:
			case GTDoor.DoorState.OpeningWaitingOnRPC:
			case GTDoor.DoorState.Opening:
			case GTDoor.DoorState.HeldOpen:
				break;
			case GTDoor.DoorState.Closing:
				this.audioSource.GTPlayOneShot(this.openSound, 1f);
				this.currentState = GTDoor.DoorState.HeldOpen;
				return;
			case GTDoor.DoorState.Open:
			case GTDoor.DoorState.HeldOpenLocally:
				this.currentState = GTDoor.DoorState.HeldOpen;
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
			break;
		default:
			throw new ArgumentOutOfRangeException("shouldOpenState", shouldOpenState, null);
		}
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x0009811C File Offset: 0x0009631C
	public void SetupDoorIDs()
	{
		GTDoor[] array = UnityEngine.Object.FindObjectsOfType<GTDoor>(true);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GTDoorID = i + 1;
		}
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x0009814C File Offset: 0x0009634C
	[NetworkRpcStaticWeavedInvoker("System.Void GTDoor::RPC_ChangeDoorState(Fusion.NetworkRunner,GTDoor/DoorState,System.Int32)")]
	[Preserve]
	[WeaverGenerated]
	protected unsafe static void RPC_ChangeDoorState@Invoker(NetworkRunner runner, SimulationMessage* message)
	{
		byte* data = SimulationMessage.GetData(message);
		int num = RpcHeader.ReadSize(data) + 3 & -4;
		GTDoor.DoorState doorState = *(GTDoor.DoorState*)(data + num);
		num += 4;
		GTDoor.DoorState shouldOpenState = doorState;
		int num2 = *(int*)(data + num);
		num += 4;
		int doorId = num2;
		NetworkBehaviourUtils.InvokeRpc = true;
		GTDoor.RPC_ChangeDoorState(runner, shouldOpenState, doorId);
	}

	// Token: 0x04000CD7 RID: 3287
	[SerializeField]
	private Transform doorTransform;

	// Token: 0x04000CD8 RID: 3288
	[SerializeField]
	private Collider[] doorColliders;

	// Token: 0x04000CD9 RID: 3289
	[SerializeField]
	private GTDoorTrigger[] doorButtonTriggers;

	// Token: 0x04000CDA RID: 3290
	[SerializeField]
	private GTDoorTrigger[] doorHoldOpenTriggers;

	// Token: 0x04000CDB RID: 3291
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04000CDC RID: 3292
	[SerializeField]
	private AudioClip openSound;

	// Token: 0x04000CDD RID: 3293
	[SerializeField]
	private AudioClip closeSound;

	// Token: 0x04000CDE RID: 3294
	[SerializeField]
	private float doorOpenSpeed = 1f;

	// Token: 0x04000CDF RID: 3295
	[SerializeField]
	private float doorCloseSpeed = 1f;

	// Token: 0x04000CE0 RID: 3296
	[SerializeField]
	[Range(1.5f, 10f)]
	private float timeUntilDoorCloses = 3f;

	// Token: 0x04000CE1 RID: 3297
	private int GTDoorID;

	// Token: 0x04000CE2 RID: 3298
	[DebugOption]
	private GTDoor.DoorState currentState;

	// Token: 0x04000CE3 RID: 3299
	private float tLastOpened;

	// Token: 0x04000CE4 RID: 3300
	private FloatSpring doorSpring;

	// Token: 0x04000CE5 RID: 3301
	[DebugOption]
	private bool peopleInHoldOpenVolume;

	// Token: 0x04000CE6 RID: 3302
	[DebugOption]
	private bool buttonTriggeredThisFrame;

	// Token: 0x04000CE7 RID: 3303
	private float lastChecked;

	// Token: 0x04000CE8 RID: 3304
	private float secondsCheck = 1f;

	// Token: 0x020001C1 RID: 449
	public enum DoorState
	{
		// Token: 0x04000CEA RID: 3306
		Closed,
		// Token: 0x04000CEB RID: 3307
		ClosingWaitingOnRPC,
		// Token: 0x04000CEC RID: 3308
		Closing,
		// Token: 0x04000CED RID: 3309
		Open,
		// Token: 0x04000CEE RID: 3310
		OpeningWaitingOnRPC,
		// Token: 0x04000CEF RID: 3311
		Opening,
		// Token: 0x04000CF0 RID: 3312
		HeldOpen,
		// Token: 0x04000CF1 RID: 3313
		HeldOpenLocally
	}
}
