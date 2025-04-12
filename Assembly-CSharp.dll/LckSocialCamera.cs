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
	// (get) Token: 0x06000D2A RID: 3370 RVA: 0x000385B7 File Offset: 0x000367B7
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
	// (get) Token: 0x06000D2B RID: 3371 RVA: 0x000385DC File Offset: 0x000367DC
	// (set) Token: 0x06000D2C RID: 3372 RVA: 0x0009F69C File Offset: 0x0009D89C
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

	// Token: 0x06000D2D RID: 3373 RVA: 0x000385E9 File Offset: 0x000367E9
	private static bool GetFlag(LckSocialCamera.CameraState cameraState, LckSocialCamera.CameraState flag)
	{
		return (cameraState & flag) == flag;
	}

	// Token: 0x06000D2E RID: 3374 RVA: 0x000385F1 File Offset: 0x000367F1
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
	// (get) Token: 0x06000D2F RID: 3375 RVA: 0x00038604 File Offset: 0x00036804
	// (set) Token: 0x06000D30 RID: 3376 RVA: 0x00038612 File Offset: 0x00036812
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
	// (get) Token: 0x06000D31 RID: 3377 RVA: 0x00038627 File Offset: 0x00036827
	// (set) Token: 0x06000D32 RID: 3378 RVA: 0x00038635 File Offset: 0x00036835
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

	// Token: 0x06000D33 RID: 3379 RVA: 0x0003864A File Offset: 0x0003684A
	public unsafe override void WriteDataFusion()
	{
		*this._networkedData = new LckSocialCamera.CameraData(this._localData.currentState);
	}

	// Token: 0x06000D34 RID: 3380 RVA: 0x00038667 File Offset: 0x00036867
	public override void ReadDataFusion()
	{
		this.ReadDataShared(this._networkedData.currentState);
	}

	// Token: 0x06000D35 RID: 3381 RVA: 0x0003867A File Offset: 0x0003687A
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(this.currentState);
	}

	// Token: 0x06000D36 RID: 3382 RVA: 0x0009F6F8 File Offset: 0x0009D8F8
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (info.Sender != info.photonView.Owner)
		{
			return;
		}
		LckSocialCamera.CameraState newState = (LckSocialCamera.CameraState)stream.ReceiveNext();
		this.ReadDataShared(newState);
	}

	// Token: 0x06000D37 RID: 3383 RVA: 0x0009F72C File Offset: 0x0009D92C
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

	// Token: 0x06000D38 RID: 3384 RVA: 0x0003868D File Offset: 0x0003688D
	public override void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		base.OnPhotonInstantiate(info);
		if (!info.photonView.IsMine)
		{
			this.StoreRigReference();
		}
	}

	// Token: 0x06000D39 RID: 3385 RVA: 0x0009F790 File Offset: 0x0009D990
	private void StoreRigReference()
	{
		RigContainer rigContainer;
		if (VRRigCache.Instance.TryGetVrrig(base.Owner, out rigContainer))
		{
			this._vrrig = rigContainer.Rig;
		}
	}

	// Token: 0x06000D3A RID: 3386 RVA: 0x0009F7C0 File Offset: 0x0009D9C0
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

	// Token: 0x06000D3B RID: 3387 RVA: 0x000386A9 File Offset: 0x000368A9
	public new void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
		base.OnEnable();
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06000D3C RID: 3388 RVA: 0x000386BE File Offset: 0x000368BE
	public new void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		base.OnDisable();
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06000D3D RID: 3389 RVA: 0x000386D3 File Offset: 0x000368D3
	private void OnManagerSpawned(LckSocialCameraManager manager)
	{
		manager.SetLckSocialCamera(this);
	}

	// Token: 0x06000D3E RID: 3390 RVA: 0x000386DC File Offset: 0x000368DC
	private void ReadDataShared(LckSocialCamera.CameraState newState)
	{
		this.currentState = newState;
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x00030F9B File Offset: 0x0002F19B
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x000386E5 File Offset: 0x000368E5
	[WeaverGenerated]
	public unsafe override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		*this._networkedData = this.__networkedData;
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x00038702 File Offset: 0x00036902
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
		// Token: 0x06000D43 RID: 3395 RVA: 0x0003871B File Offset: 0x0003691B
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
