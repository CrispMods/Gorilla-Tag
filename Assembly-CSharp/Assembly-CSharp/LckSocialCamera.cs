using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Fusion;
using GorillaExtensions;
using Liv.Lck.GorillaTag;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200023D RID: 573
[NetworkBehaviourWeaved(1)]
public class LckSocialCamera : NetworkComponent, IGorillaSliceableSimple
{
	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000D2A RID: 3370 RVA: 0x00044965 File Offset: 0x00042B65
	[Networked]
	[NetworkedWeaved(0, 1)]
	private unsafe ref LckSocialCamera.CameraData _networkedData
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing LckSocialCamera._networkedData. Networked properties can only be accessed when Spawned() has been called.");
			}
			return ref *(LckSocialCamera.CameraData*)(this.Ptr + 0);
		}
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06000D2B RID: 3371 RVA: 0x0004498A File Offset: 0x00042B8A
	// (set) Token: 0x06000D2C RID: 3372 RVA: 0x00044998 File Offset: 0x00042B98
	private LckSocialCamera.CameraState currentState
	{
		get
		{
			return this._localData.currentState;
		}
		set
		{
			this._localData.currentState = value;
			if (base.IsLocallyOwned)
			{
				this.CoconutCamera.SetVisualsActive(false);
				this.CoconutCamera.SetRecordingState(false);
				return;
			}
			this.CoconutCamera.SetVisualsActive(this.visible);
			this.CoconutCamera.SetRecordingState(this.recording);
		}
	}

	// Token: 0x06000D2D RID: 3373 RVA: 0x000449F4 File Offset: 0x00042BF4
	private static bool GetFlag(LckSocialCamera.CameraState cameraState, LckSocialCamera.CameraState flag)
	{
		return (cameraState & flag) == flag;
	}

	// Token: 0x06000D2E RID: 3374 RVA: 0x000449FC File Offset: 0x00042BFC
	private static LckSocialCamera.CameraState SetFlag(LckSocialCamera.CameraState cameraState, LckSocialCamera.CameraState flag, bool value)
	{
		if (value)
		{
			cameraState |= flag;
		}
		else
		{
			cameraState &= ~flag;
		}
		return cameraState;
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06000D2F RID: 3375 RVA: 0x00044A0F File Offset: 0x00042C0F
	// (set) Token: 0x06000D30 RID: 3376 RVA: 0x00044A1D File Offset: 0x00042C1D
	public bool visible
	{
		get
		{
			return LckSocialCamera.GetFlag(this.currentState, LckSocialCamera.CameraState.Visible);
		}
		set
		{
			this.currentState = LckSocialCamera.SetFlag(this.currentState, LckSocialCamera.CameraState.Visible, value);
		}
	}

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06000D31 RID: 3377 RVA: 0x00044A32 File Offset: 0x00042C32
	// (set) Token: 0x06000D32 RID: 3378 RVA: 0x00044A40 File Offset: 0x00042C40
	public bool recording
	{
		get
		{
			return LckSocialCamera.GetFlag(this.currentState, LckSocialCamera.CameraState.Recording);
		}
		set
		{
			this.currentState = LckSocialCamera.SetFlag(this.currentState, LckSocialCamera.CameraState.Recording, value);
		}
	}

	// Token: 0x06000D33 RID: 3379 RVA: 0x00044A55 File Offset: 0x00042C55
	public unsafe override void WriteDataFusion()
	{
		*this._networkedData = new LckSocialCamera.CameraData(this._localData.currentState);
	}

	// Token: 0x06000D34 RID: 3380 RVA: 0x00044A72 File Offset: 0x00042C72
	public override void ReadDataFusion()
	{
		this.ReadDataShared(this._networkedData.currentState);
	}

	// Token: 0x06000D35 RID: 3381 RVA: 0x00044A85 File Offset: 0x00042C85
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(this.currentState);
	}

	// Token: 0x06000D36 RID: 3382 RVA: 0x00044A98 File Offset: 0x00042C98
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (info.Sender != info.photonView.Owner)
		{
			return;
		}
		LckSocialCamera.CameraState newState = (LckSocialCamera.CameraState)stream.ReceiveNext();
		this.ReadDataShared(newState);
	}

	// Token: 0x06000D37 RID: 3383 RVA: 0x00044ACC File Offset: 0x00042CCC
	protected override void Start()
	{
		base.Start();
		this.visible = this.visible;
		if (base.IsLocallyOwned)
		{
			this.StoreRigReference();
			LckSocialCameraManager instance = LckSocialCameraManager.Instance;
			if (instance != null)
			{
				instance.SetLckSocialCamera(this);
				return;
			}
			LckSocialCameraManager.OnManagerSpawned = (Action<LckSocialCameraManager>)Delegate.Combine(LckSocialCameraManager.OnManagerSpawned, new Action<LckSocialCameraManager>(this.OnManagerSpawned));
		}
	}

	// Token: 0x06000D38 RID: 3384 RVA: 0x00044B30 File Offset: 0x00042D30
	public override void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		base.OnPhotonInstantiate(info);
		if (!info.photonView.IsMine)
		{
			this.StoreRigReference();
		}
	}

	// Token: 0x06000D39 RID: 3385 RVA: 0x00044B4C File Offset: 0x00042D4C
	private void StoreRigReference()
	{
		RigContainer rigContainer;
		if (VRRigCache.Instance.TryGetVrrig(base.Owner, out rigContainer))
		{
			this._vrrig = rigContainer.Rig;
		}
	}

	// Token: 0x06000D3A RID: 3386 RVA: 0x00044B7C File Offset: 0x00042D7C
	public void SliceUpdate()
	{
		if (this._vrrig == null)
		{
			this.StoreRigReference();
			if (this._vrrig.IsNull())
			{
				base.enabled = false;
				return;
			}
		}
		else
		{
			this.CoconutCamera.transform.localScale = Vector3.one * this._vrrig.scaleFactor;
		}
	}

	// Token: 0x06000D3B RID: 3387 RVA: 0x00044BD7 File Offset: 0x00042DD7
	public new void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
		base.OnEnable();
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06000D3C RID: 3388 RVA: 0x00044BEC File Offset: 0x00042DEC
	public new void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		base.OnDisable();
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06000D3D RID: 3389 RVA: 0x00044C01 File Offset: 0x00042E01
	private void OnManagerSpawned(LckSocialCameraManager manager)
	{
		manager.SetLckSocialCamera(this);
	}

	// Token: 0x06000D3E RID: 3390 RVA: 0x00044C0A File Offset: 0x00042E0A
	private void ReadDataShared(LckSocialCamera.CameraState newState)
	{
		this.currentState = newState;
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x0000FD18 File Offset: 0x0000DF18
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x00044C13 File Offset: 0x00042E13
	[WeaverGenerated]
	public unsafe override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		*this._networkedData = this.__networkedData;
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x00044C30 File Offset: 0x00042E30
	[WeaverGenerated]
	public unsafe override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this.__networkedData = *this._networkedData;
	}

	// Token: 0x04001073 RID: 4211
	[SerializeField]
	private Transform _scaleTransform;

	// Token: 0x04001074 RID: 4212
	[SerializeField]
	public CoconutCamera CoconutCamera;

	// Token: 0x04001075 RID: 4213
	[SerializeField]
	private List<GameObject> _visualObjects;

	// Token: 0x04001076 RID: 4214
	[SerializeField]
	private VRRig _vrrig;

	// Token: 0x04001077 RID: 4215
	private LckSocialCamera.CameraDataLocal _localData;

	// Token: 0x04001078 RID: 4216
	[WeaverGenerated]
	[DefaultForProperty("_networkedData", 0, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private LckSocialCamera.CameraData __networkedData;

	// Token: 0x0200023E RID: 574
	private enum CameraState
	{
		// Token: 0x0400107A RID: 4218
		Empty,
		// Token: 0x0400107B RID: 4219
		Visible,
		// Token: 0x0400107C RID: 4220
		Recording
	}

	// Token: 0x0200023F RID: 575
	[NetworkStructWeaved(1)]
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	private struct CameraData : INetworkStruct
	{
		// Token: 0x06000D43 RID: 3395 RVA: 0x00044C49 File Offset: 0x00042E49
		public CameraData(LckSocialCamera.CameraState currentState)
		{
			this.currentState = currentState;
		}

		// Token: 0x0400107D RID: 4221
		[FieldOffset(0)]
		public LckSocialCamera.CameraState currentState;
	}

	// Token: 0x02000240 RID: 576
	private struct CameraDataLocal
	{
		// Token: 0x0400107E RID: 4222
		public LckSocialCamera.CameraState currentState;
	}
}
