using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000093 RID: 147
[NetworkBehaviourWeaved(3)]
public class BarrelCannon : NetworkComponent
{
	// Token: 0x060003B6 RID: 950 RVA: 0x00016689 File Offset: 0x00014889
	private void Update()
	{
		if (base.IsMine)
		{
			this.AuthorityUpdate();
		}
		else
		{
			this.ClientUpdate();
		}
		this.SharedUpdate();
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x000166A8 File Offset: 0x000148A8
	private void AuthorityUpdate()
	{
		float time = Time.time;
		this.syncedState.hasAuthorityPassenger = this.localPlayerInside;
		switch (this.syncedState.currentState)
		{
		default:
			if (this.localPlayerInside)
			{
				this.stateStartTime = time;
				this.syncedState.currentState = BarrelCannon.BarrelCannonState.Loaded;
				return;
			}
			break;
		case BarrelCannon.BarrelCannonState.Loaded:
			if (time - this.stateStartTime > this.cannonEntryDelayTime)
			{
				this.stateStartTime = time;
				this.syncedState.currentState = BarrelCannon.BarrelCannonState.MovingToFirePosition;
				return;
			}
			break;
		case BarrelCannon.BarrelCannonState.MovingToFirePosition:
			if (this.moveToFiringPositionTime > Mathf.Epsilon)
			{
				this.syncedState.firingPositionLerpValue = Mathf.Clamp01((time - this.stateStartTime) / this.moveToFiringPositionTime);
			}
			else
			{
				this.syncedState.firingPositionLerpValue = 1f;
			}
			if (this.syncedState.firingPositionLerpValue >= 1f - Mathf.Epsilon)
			{
				this.syncedState.firingPositionLerpValue = 1f;
				this.stateStartTime = time;
				this.syncedState.currentState = BarrelCannon.BarrelCannonState.Firing;
				return;
			}
			break;
		case BarrelCannon.BarrelCannonState.Firing:
			if (this.localPlayerInside && this.localPlayerRigidbody != null)
			{
				Vector3 b = base.transform.position - GorillaTagger.Instance.headCollider.transform.position;
				this.localPlayerRigidbody.MovePosition(this.localPlayerRigidbody.position + b);
			}
			if (time - this.stateStartTime > this.preFiringDelayTime)
			{
				base.transform.localPosition = this.firingPositionOffset;
				base.transform.localRotation = Quaternion.Euler(this.firingRotationOffset);
				this.FireBarrelCannonLocal(base.transform.position, base.transform.up);
				if (PhotonNetwork.InRoom && GorillaGameManager.instance != null)
				{
					base.SendRPC("FireBarrelCannonRPC", RpcTarget.Others, new object[]
					{
						base.transform.position,
						base.transform.up
					});
				}
				Collider[] array = this.colliders;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].enabled = false;
				}
				this.stateStartTime = time;
				this.syncedState.currentState = BarrelCannon.BarrelCannonState.PostFireCooldown;
				return;
			}
			break;
		case BarrelCannon.BarrelCannonState.PostFireCooldown:
			if (time - this.stateStartTime > this.postFiringCooldownTime)
			{
				Collider[] array = this.colliders;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].enabled = true;
				}
				this.stateStartTime = time;
				this.syncedState.currentState = BarrelCannon.BarrelCannonState.ReturningToIdlePosition;
				return;
			}
			break;
		case BarrelCannon.BarrelCannonState.ReturningToIdlePosition:
			if (this.returnToIdlePositionTime > Mathf.Epsilon)
			{
				this.syncedState.firingPositionLerpValue = 1f - Mathf.Clamp01((time - this.stateStartTime) / this.returnToIdlePositionTime);
			}
			else
			{
				this.syncedState.firingPositionLerpValue = 0f;
			}
			if (this.syncedState.firingPositionLerpValue <= Mathf.Epsilon)
			{
				this.syncedState.firingPositionLerpValue = 0f;
				this.stateStartTime = time;
				this.syncedState.currentState = BarrelCannon.BarrelCannonState.Idle;
			}
			break;
		}
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x000169AC File Offset: 0x00014BAC
	private void ClientUpdate()
	{
		if (!this.syncedState.hasAuthorityPassenger && this.syncedState.currentState == BarrelCannon.BarrelCannonState.Idle && this.localPlayerInside)
		{
			base.RequestOwnership();
		}
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x000169D8 File Offset: 0x00014BD8
	private void SharedUpdate()
	{
		if (this.syncedState.firingPositionLerpValue != this.localFiringPositionLerpValue)
		{
			this.localFiringPositionLerpValue = this.syncedState.firingPositionLerpValue;
			base.transform.localPosition = Vector3.Lerp(Vector3.zero, this.firingPositionOffset, this.firePositionAnimationCurve.Evaluate(this.localFiringPositionLerpValue));
			base.transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, this.firingRotationOffset, this.fireRotationAnimationCurve.Evaluate(this.localFiringPositionLerpValue)));
		}
	}

	// Token: 0x060003BA RID: 954 RVA: 0x00016A66 File Offset: 0x00014C66
	[PunRPC]
	private void FireBarrelCannonRPC(Vector3 cannonCenter, Vector3 firingDirection)
	{
		this.FireBarrelCannonLocal(cannonCenter, firingDirection);
	}

	// Token: 0x060003BB RID: 955 RVA: 0x00016A70 File Offset: 0x00014C70
	private void FireBarrelCannonLocal(Vector3 cannonCenter, Vector3 firingDirection)
	{
		if (this.audioSource != null)
		{
			this.audioSource.GTPlay();
		}
		if (this.localPlayerInside && this.localPlayerRigidbody != null)
		{
			Vector3 b = cannonCenter - GorillaTagger.Instance.headCollider.transform.position;
			this.localPlayerRigidbody.position = this.localPlayerRigidbody.position + b;
			this.localPlayerRigidbody.velocity = firingDirection * this.firingSpeed;
		}
	}

	// Token: 0x060003BC RID: 956 RVA: 0x00016AFC File Offset: 0x00014CFC
	private void OnTriggerEnter(Collider other)
	{
		Rigidbody rigidbody;
		if (this.LocalPlayerTriggerFilter(other, out rigidbody))
		{
			this.localPlayerInside = true;
			this.localPlayerRigidbody = rigidbody;
		}
	}

	// Token: 0x060003BD RID: 957 RVA: 0x00016B24 File Offset: 0x00014D24
	private void OnTriggerExit(Collider other)
	{
		Rigidbody rigidbody;
		if (this.LocalPlayerTriggerFilter(other, out rigidbody))
		{
			this.localPlayerInside = false;
			this.localPlayerRigidbody = null;
		}
	}

	// Token: 0x060003BE RID: 958 RVA: 0x00016B4A File Offset: 0x00014D4A
	private bool LocalPlayerTriggerFilter(Collider other, out Rigidbody rb)
	{
		rb = null;
		if (other.gameObject == GorillaTagger.Instance.headCollider.gameObject)
		{
			rb = GorillaTagger.Instance.GetComponent<Rigidbody>();
		}
		return rb != null;
	}

	// Token: 0x060003BF RID: 959 RVA: 0x00016B80 File Offset: 0x00014D80
	private bool IsLocalPlayerInCannon()
	{
		Vector3 point;
		Vector3 point2;
		this.GetCapsulePoints(this.triggerCollider, out point, out point2);
		Physics.OverlapCapsuleNonAlloc(point, point2, this.triggerCollider.radius, this.triggerOverlapResults);
		for (int i = 0; i < this.triggerOverlapResults.Length; i++)
		{
			Rigidbody rigidbody;
			if (this.LocalPlayerTriggerFilter(this.triggerOverlapResults[i], out rigidbody))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x00016BE0 File Offset: 0x00014DE0
	private void GetCapsulePoints(CapsuleCollider capsule, out Vector3 pointA, out Vector3 pointB)
	{
		float d = capsule.height * 0.5f - capsule.radius;
		pointA = capsule.transform.position + capsule.transform.up * d;
		pointB = capsule.transform.position - capsule.transform.up * d;
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x060003C1 RID: 961 RVA: 0x00016C4F File Offset: 0x00014E4F
	// (set) Token: 0x060003C2 RID: 962 RVA: 0x00016C79 File Offset: 0x00014E79
	[Networked]
	[NetworkedWeaved(0, 3)]
	private unsafe BarrelCannon.BarrelCannonSyncedStateData Data
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing BarrelCannon.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(BarrelCannon.BarrelCannonSyncedStateData*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing BarrelCannon.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(BarrelCannon.BarrelCannonSyncedStateData*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x00016CA4 File Offset: 0x00014EA4
	public override void WriteDataFusion()
	{
		this.Data = this.syncedState;
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x00016CB8 File Offset: 0x00014EB8
	public override void ReadDataFusion()
	{
		this.syncedState.currentState = this.Data.CurrentState;
		this.syncedState.hasAuthorityPassenger = this.Data.HasAuthorityPassenger;
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x00016CFC File Offset: 0x00014EFC
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(this.syncedState.currentState);
		stream.SendNext(this.syncedState.hasAuthorityPassenger);
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x00016D2A File Offset: 0x00014F2A
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		this.syncedState.currentState = (BarrelCannon.BarrelCannonState)stream.ReceiveNext();
		this.syncedState.hasAuthorityPassenger = (bool)stream.ReceiveNext();
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x00016D58 File Offset: 0x00014F58
	public override void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
	{
		if (!this.localPlayerInside)
		{
			targetView.TransferOwnership(requestingPlayer);
		}
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x00016E2D File Offset: 0x0001502D
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x060003CA RID: 970 RVA: 0x00016E45 File Offset: 0x00015045
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x04000434 RID: 1076
	[SerializeField]
	private float firingSpeed = 10f;

	// Token: 0x04000435 RID: 1077
	[Header("Cannon's Movement Before Firing")]
	[SerializeField]
	private Vector3 firingPositionOffset = Vector3.zero;

	// Token: 0x04000436 RID: 1078
	[SerializeField]
	private Vector3 firingRotationOffset = Vector3.zero;

	// Token: 0x04000437 RID: 1079
	[SerializeField]
	private AnimationCurve firePositionAnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04000438 RID: 1080
	[SerializeField]
	private AnimationCurve fireRotationAnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04000439 RID: 1081
	[Header("Cannon State Change Timing Parameters")]
	[SerializeField]
	private float moveToFiringPositionTime = 0.5f;

	// Token: 0x0400043A RID: 1082
	[SerializeField]
	[Tooltip("The minimum time to wait after a gorilla enters the cannon before it starts moving into the firing position.")]
	private float cannonEntryDelayTime = 0.25f;

	// Token: 0x0400043B RID: 1083
	[SerializeField]
	[Tooltip("The minimum time to wait after a gorilla enters the cannon before it starts moving into the firing position.")]
	private float preFiringDelayTime = 0.25f;

	// Token: 0x0400043C RID: 1084
	[SerializeField]
	[Tooltip("The minimum time to wait after the cannon fires before it starts moving back to the idle position.")]
	private float postFiringCooldownTime = 0.25f;

	// Token: 0x0400043D RID: 1085
	[SerializeField]
	private float returnToIdlePositionTime = 1f;

	// Token: 0x0400043E RID: 1086
	[Header("Component References")]
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x0400043F RID: 1087
	[SerializeField]
	private CapsuleCollider triggerCollider;

	// Token: 0x04000440 RID: 1088
	[SerializeField]
	private Collider[] colliders;

	// Token: 0x04000441 RID: 1089
	private BarrelCannon.BarrelCannonSyncedState syncedState = new BarrelCannon.BarrelCannonSyncedState();

	// Token: 0x04000442 RID: 1090
	private Collider[] triggerOverlapResults = new Collider[16];

	// Token: 0x04000443 RID: 1091
	private bool localPlayerInside;

	// Token: 0x04000444 RID: 1092
	private Rigidbody localPlayerRigidbody;

	// Token: 0x04000445 RID: 1093
	private float stateStartTime;

	// Token: 0x04000446 RID: 1094
	private float localFiringPositionLerpValue;

	// Token: 0x04000447 RID: 1095
	[WeaverGenerated]
	[DefaultForProperty("Data", 0, 3)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private BarrelCannon.BarrelCannonSyncedStateData _Data;

	// Token: 0x02000094 RID: 148
	private enum BarrelCannonState
	{
		// Token: 0x04000449 RID: 1097
		Idle,
		// Token: 0x0400044A RID: 1098
		Loaded,
		// Token: 0x0400044B RID: 1099
		MovingToFirePosition,
		// Token: 0x0400044C RID: 1100
		Firing,
		// Token: 0x0400044D RID: 1101
		PostFireCooldown,
		// Token: 0x0400044E RID: 1102
		ReturningToIdlePosition
	}

	// Token: 0x02000095 RID: 149
	private class BarrelCannonSyncedState
	{
		// Token: 0x0400044F RID: 1103
		public BarrelCannon.BarrelCannonState currentState;

		// Token: 0x04000450 RID: 1104
		public bool hasAuthorityPassenger;

		// Token: 0x04000451 RID: 1105
		public float firingPositionLerpValue;
	}

	// Token: 0x02000096 RID: 150
	[NetworkStructWeaved(3)]
	[StructLayout(LayoutKind.Explicit, Size = 12)]
	private struct BarrelCannonSyncedStateData : INetworkStruct
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060003CC RID: 972 RVA: 0x00016E59 File Offset: 0x00015059
		// (set) Token: 0x060003CD RID: 973 RVA: 0x00016E6B File Offset: 0x0001506B
		[Networked]
		public unsafe BarrelCannon.BarrelCannonState CurrentState
		{
			readonly get
			{
				return *(BarrelCannon.BarrelCannonState*)Native.ReferenceToPointer<FixedStorage@1>(ref this._CurrentState);
			}
			set
			{
				*(BarrelCannon.BarrelCannonState*)Native.ReferenceToPointer<FixedStorage@1>(ref this._CurrentState) = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060003CE RID: 974 RVA: 0x00016E7E File Offset: 0x0001507E
		// (set) Token: 0x060003CF RID: 975 RVA: 0x00016E90 File Offset: 0x00015090
		[Networked]
		public unsafe NetworkBool HasAuthorityPassenger
		{
			readonly get
			{
				return *(NetworkBool*)Native.ReferenceToPointer<FixedStorage@1>(ref this._HasAuthorityPassenger);
			}
			set
			{
				*(NetworkBool*)Native.ReferenceToPointer<FixedStorage@1>(ref this._HasAuthorityPassenger) = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x00016EA3 File Offset: 0x000150A3
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x00016EAB File Offset: 0x000150AB
		public float FiringPositionLerpValue { readonly get; set; }

		// Token: 0x060003D2 RID: 978 RVA: 0x00016EB4 File Offset: 0x000150B4
		public BarrelCannonSyncedStateData(BarrelCannon.BarrelCannonState state, bool hasAuthPassenger, float firingPosLerpVal)
		{
			this.CurrentState = state;
			this.HasAuthorityPassenger = hasAuthPassenger;
			this.FiringPositionLerpValue = firingPosLerpVal;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00016ED0 File Offset: 0x000150D0
		public static implicit operator BarrelCannon.BarrelCannonSyncedStateData(BarrelCannon.BarrelCannonSyncedState state)
		{
			return new BarrelCannon.BarrelCannonSyncedStateData(state.currentState, state.hasAuthorityPassenger, state.firingPositionLerpValue);
		}

		// Token: 0x04000452 RID: 1106
		[FixedBufferProperty(typeof(BarrelCannon.BarrelCannonState), typeof(UnityValueSurrogate@ReaderWriter@BarrelCannon__BarrelCannonState), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(0)]
		private FixedStorage@1 _CurrentState;

		// Token: 0x04000453 RID: 1107
		[FixedBufferProperty(typeof(NetworkBool), typeof(UnityValueSurrogate@ReaderWriter@Fusion_NetworkBool), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(4)]
		private FixedStorage@1 _HasAuthorityPassenger;
	}
}
