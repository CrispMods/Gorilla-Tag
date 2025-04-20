using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x0200009D RID: 157
[NetworkBehaviourWeaved(3)]
public class BarrelCannon : NetworkComponent
{
	// Token: 0x060003F2 RID: 1010 RVA: 0x00032F8A File Offset: 0x0003118A
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

	// Token: 0x060003F3 RID: 1011 RVA: 0x0007A794 File Offset: 0x00078994
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

	// Token: 0x060003F4 RID: 1012 RVA: 0x00032FA8 File Offset: 0x000311A8
	private void ClientUpdate()
	{
		if (!this.syncedState.hasAuthorityPassenger && this.syncedState.currentState == BarrelCannon.BarrelCannonState.Idle && this.localPlayerInside)
		{
			base.RequestOwnership();
		}
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x0007AA98 File Offset: 0x00078C98
	private void SharedUpdate()
	{
		if (this.syncedState.firingPositionLerpValue != this.localFiringPositionLerpValue)
		{
			this.localFiringPositionLerpValue = this.syncedState.firingPositionLerpValue;
			base.transform.localPosition = Vector3.Lerp(Vector3.zero, this.firingPositionOffset, this.firePositionAnimationCurve.Evaluate(this.localFiringPositionLerpValue));
			base.transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, this.firingRotationOffset, this.fireRotationAnimationCurve.Evaluate(this.localFiringPositionLerpValue)));
		}
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x00032FD2 File Offset: 0x000311D2
	[PunRPC]
	private void FireBarrelCannonRPC(Vector3 cannonCenter, Vector3 firingDirection)
	{
		this.FireBarrelCannonLocal(cannonCenter, firingDirection);
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x0007AB28 File Offset: 0x00078D28
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

	// Token: 0x060003F8 RID: 1016 RVA: 0x0007ABB4 File Offset: 0x00078DB4
	private void OnTriggerEnter(Collider other)
	{
		Rigidbody rigidbody;
		if (this.LocalPlayerTriggerFilter(other, out rigidbody))
		{
			this.localPlayerInside = true;
			this.localPlayerRigidbody = rigidbody;
		}
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x0007ABDC File Offset: 0x00078DDC
	private void OnTriggerExit(Collider other)
	{
		Rigidbody rigidbody;
		if (this.LocalPlayerTriggerFilter(other, out rigidbody))
		{
			this.localPlayerInside = false;
			this.localPlayerRigidbody = null;
		}
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x00032FDC File Offset: 0x000311DC
	private bool LocalPlayerTriggerFilter(Collider other, out Rigidbody rb)
	{
		rb = null;
		if (other.gameObject == GorillaTagger.Instance.headCollider.gameObject)
		{
			rb = GorillaTagger.Instance.GetComponent<Rigidbody>();
		}
		return rb != null;
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x0007AC04 File Offset: 0x00078E04
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

	// Token: 0x060003FC RID: 1020 RVA: 0x0007AC64 File Offset: 0x00078E64
	private void GetCapsulePoints(CapsuleCollider capsule, out Vector3 pointA, out Vector3 pointB)
	{
		float d = capsule.height * 0.5f - capsule.radius;
		pointA = capsule.transform.position + capsule.transform.up * d;
		pointB = capsule.transform.position - capsule.transform.up * d;
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x060003FD RID: 1021 RVA: 0x00033011 File Offset: 0x00031211
	// (set) Token: 0x060003FE RID: 1022 RVA: 0x0003303B File Offset: 0x0003123B
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

	// Token: 0x060003FF RID: 1023 RVA: 0x00033066 File Offset: 0x00031266
	public override void WriteDataFusion()
	{
		this.Data = this.syncedState;
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x0007ACD4 File Offset: 0x00078ED4
	public override void ReadDataFusion()
	{
		this.syncedState.currentState = this.Data.CurrentState;
		this.syncedState.hasAuthorityPassenger = this.Data.HasAuthorityPassenger;
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x00033079 File Offset: 0x00031279
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(this.syncedState.currentState);
		stream.SendNext(this.syncedState.hasAuthorityPassenger);
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x000330A7 File Offset: 0x000312A7
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		this.syncedState.currentState = (BarrelCannon.BarrelCannonState)stream.ReceiveNext();
		this.syncedState.hasAuthorityPassenger = (bool)stream.ReceiveNext();
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x000330D5 File Offset: 0x000312D5
	public override void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
	{
		if (!this.localPlayerInside)
		{
			targetView.TransferOwnership(requestingPlayer);
		}
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x000330E6 File Offset: 0x000312E6
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x000330FE File Offset: 0x000312FE
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x04000474 RID: 1140
	[SerializeField]
	private float firingSpeed = 10f;

	// Token: 0x04000475 RID: 1141
	[Header("Cannon's Movement Before Firing")]
	[SerializeField]
	private Vector3 firingPositionOffset = Vector3.zero;

	// Token: 0x04000476 RID: 1142
	[SerializeField]
	private Vector3 firingRotationOffset = Vector3.zero;

	// Token: 0x04000477 RID: 1143
	[SerializeField]
	private AnimationCurve firePositionAnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04000478 RID: 1144
	[SerializeField]
	private AnimationCurve fireRotationAnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04000479 RID: 1145
	[Header("Cannon State Change Timing Parameters")]
	[SerializeField]
	private float moveToFiringPositionTime = 0.5f;

	// Token: 0x0400047A RID: 1146
	[SerializeField]
	[Tooltip("The minimum time to wait after a gorilla enters the cannon before it starts moving into the firing position.")]
	private float cannonEntryDelayTime = 0.25f;

	// Token: 0x0400047B RID: 1147
	[SerializeField]
	[Tooltip("The minimum time to wait after a gorilla enters the cannon before it starts moving into the firing position.")]
	private float preFiringDelayTime = 0.25f;

	// Token: 0x0400047C RID: 1148
	[SerializeField]
	[Tooltip("The minimum time to wait after the cannon fires before it starts moving back to the idle position.")]
	private float postFiringCooldownTime = 0.25f;

	// Token: 0x0400047D RID: 1149
	[SerializeField]
	private float returnToIdlePositionTime = 1f;

	// Token: 0x0400047E RID: 1150
	[Header("Component References")]
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x0400047F RID: 1151
	[SerializeField]
	private CapsuleCollider triggerCollider;

	// Token: 0x04000480 RID: 1152
	[SerializeField]
	private Collider[] colliders;

	// Token: 0x04000481 RID: 1153
	private BarrelCannon.BarrelCannonSyncedState syncedState = new BarrelCannon.BarrelCannonSyncedState();

	// Token: 0x04000482 RID: 1154
	private Collider[] triggerOverlapResults = new Collider[16];

	// Token: 0x04000483 RID: 1155
	private bool localPlayerInside;

	// Token: 0x04000484 RID: 1156
	private Rigidbody localPlayerRigidbody;

	// Token: 0x04000485 RID: 1157
	private float stateStartTime;

	// Token: 0x04000486 RID: 1158
	private float localFiringPositionLerpValue;

	// Token: 0x04000487 RID: 1159
	[WeaverGenerated]
	[DefaultForProperty("Data", 0, 3)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private BarrelCannon.BarrelCannonSyncedStateData _Data;

	// Token: 0x0200009E RID: 158
	private enum BarrelCannonState
	{
		// Token: 0x04000489 RID: 1161
		Idle,
		// Token: 0x0400048A RID: 1162
		Loaded,
		// Token: 0x0400048B RID: 1163
		MovingToFirePosition,
		// Token: 0x0400048C RID: 1164
		Firing,
		// Token: 0x0400048D RID: 1165
		PostFireCooldown,
		// Token: 0x0400048E RID: 1166
		ReturningToIdlePosition
	}

	// Token: 0x0200009F RID: 159
	private class BarrelCannonSyncedState
	{
		// Token: 0x0400048F RID: 1167
		public BarrelCannon.BarrelCannonState currentState;

		// Token: 0x04000490 RID: 1168
		public bool hasAuthorityPassenger;

		// Token: 0x04000491 RID: 1169
		public float firingPositionLerpValue;
	}

	// Token: 0x020000A0 RID: 160
	[NetworkStructWeaved(3)]
	[StructLayout(LayoutKind.Explicit, Size = 12)]
	private struct BarrelCannonSyncedStateData : INetworkStruct
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x00033112 File Offset: 0x00031312
		// (set) Token: 0x06000409 RID: 1033 RVA: 0x00033124 File Offset: 0x00031324
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x00033137 File Offset: 0x00031337
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x00033149 File Offset: 0x00031349
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x0003315C File Offset: 0x0003135C
		// (set) Token: 0x0600040D RID: 1037 RVA: 0x00033164 File Offset: 0x00031364
		public float FiringPositionLerpValue { readonly get; set; }

		// Token: 0x0600040E RID: 1038 RVA: 0x0003316D File Offset: 0x0003136D
		public BarrelCannonSyncedStateData(BarrelCannon.BarrelCannonState state, bool hasAuthPassenger, float firingPosLerpVal)
		{
			this.CurrentState = state;
			this.HasAuthorityPassenger = hasAuthPassenger;
			this.FiringPositionLerpValue = firingPosLerpVal;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00033189 File Offset: 0x00031389
		public static implicit operator BarrelCannon.BarrelCannonSyncedStateData(BarrelCannon.BarrelCannonSyncedState state)
		{
			return new BarrelCannon.BarrelCannonSyncedStateData(state.currentState, state.hasAuthorityPassenger, state.firingPositionLerpValue);
		}

		// Token: 0x04000492 RID: 1170
		[FixedBufferProperty(typeof(BarrelCannon.BarrelCannonState), typeof(UnityValueSurrogate@ReaderWriter@BarrelCannon__BarrelCannonState), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(0)]
		private FixedStorage@1 _CurrentState;

		// Token: 0x04000493 RID: 1171
		[FixedBufferProperty(typeof(NetworkBool), typeof(UnityValueSurrogate@ReaderWriter@Fusion_NetworkBool), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(4)]
		private FixedStorage@1 _HasAuthorityPassenger;
	}
}
