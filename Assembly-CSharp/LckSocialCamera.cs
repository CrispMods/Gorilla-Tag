using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Fusion;
using GorillaExtensions;
using Liv.Lck.GorillaTag;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000248 RID: 584
[NetworkBehaviourWeaved(1)]
public class LckSocialCamera : NetworkComponent, IGorillaSliceableSimple
{
	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06000D73 RID: 3443 RVA: 0x00039877 File Offset: 0x00037A77
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

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06000D74 RID: 3444 RVA: 0x0003989C File Offset: 0x00037A9C
	// (set) Token: 0x06000D75 RID: 3445 RVA: 0x000A1F28 File Offset: 0x000A0128
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

	// Token: 0x06000D76 RID: 3446 RVA: 0x000398A9 File Offset: 0x00037AA9
	private static bool GetFlag(LckSocialCamera.CameraState cameraState, LckSocialCamera.CameraState flag)
	{
		return (cameraState & flag) == flag;
	}

	// Token: 0x06000D77 RID: 3447 RVA: 0x000398B1 File Offset: 0x00037AB1
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

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000D78 RID: 3448 RVA: 0x000398C4 File Offset: 0x00037AC4
	// (set) Token: 0x06000D79 RID: 3449 RVA: 0x000398D2 File Offset: 0x00037AD2
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

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06000D7A RID: 3450 RVA: 0x000398E7 File Offset: 0x00037AE7
	// (set) Token: 0x06000D7B RID: 3451 RVA: 0x000398F5 File Offset: 0x00037AF5
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

	// Token: 0x06000D7C RID: 3452 RVA: 0x0003990A File Offset: 0x00037B0A
	public unsafe override void WriteDataFusion()
	{
		*this._networkedData = new LckSocialCamera.CameraData(this._localData.currentState);
	}

	// Token: 0x06000D7D RID: 3453 RVA: 0x00039927 File Offset: 0x00037B27
	public override void ReadDataFusion()
	{
		this.ReadDataShared(this._networkedData.currentState);
	}

	// Token: 0x06000D7E RID: 3454 RVA: 0x0003993A File Offset: 0x00037B3A
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(this.currentState);
	}

	// Token: 0x06000D7F RID: 3455 RVA: 0x000A1F84 File Offset: 0x000A0184
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (info.Sender != info.photonView.Owner)
		{
			return;
		}
		LckSocialCamera.CameraState newState = (LckSocialCamera.CameraState)stream.ReceiveNext();
		this.ReadDataShared(newState);
	}

	// Token: 0x06000D80 RID: 3456 RVA: 0x000A1FB8 File Offset: 0x000A01B8
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

	// Token: 0x06000D81 RID: 3457 RVA: 0x0003994D File Offset: 0x00037B4D
	public override void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		base.OnPhotonInstantiate(info);
		if (!info.photonView.IsMine)
		{
			this.StoreRigReference();
		}
	}

	// Token: 0x06000D82 RID: 3458 RVA: 0x000A201C File Offset: 0x000A021C
	private void StoreRigReference()
	{
		RigContainer rigContainer;
		if (VRRigCache.Instance.TryGetVrrig(base.Owner, out rigContainer))
		{
			this._vrrig = rigContainer.Rig;
		}
	}

	// Token: 0x06000D83 RID: 3459 RVA: 0x000A204C File Offset: 0x000A024C
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

	// Token: 0x06000D84 RID: 3460 RVA: 0x00039969 File Offset: 0x00037B69
	public new void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
		base.OnEnable();
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06000D85 RID: 3461 RVA: 0x0003997E File Offset: 0x00037B7E
	public new void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		base.OnDisable();
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x00039993 File Offset: 0x00037B93
	private void OnManagerSpawned(LckSocialCameraManager manager)
	{
		manager.SetLckSocialCamera(this);
	}

	// Token: 0x06000D87 RID: 3463 RVA: 0x0003999C File Offset: 0x00037B9C
	private void ReadDataShared(LckSocialCamera.CameraState newState)
	{
		this.currentState = newState;
	}

	// Token: 0x06000D89 RID: 3465 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x06000D8A RID: 3466 RVA: 0x000399A5 File Offset: 0x00037BA5
	[WeaverGenerated]
	public unsafe override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		*this._networkedData = this.__networkedData;
	}

	// Token: 0x06000D8B RID: 3467 RVA: 0x000399C2 File Offset: 0x00037BC2
	[WeaverGenerated]
	public unsafe override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this.__networkedData = *this._networkedData;
	}

	// Token: 0x040010B8 RID: 4280
	[SerializeField]
	private Transform _scaleTransform;

	// Token: 0x040010B9 RID: 4281
	[SerializeField]
	public CoconutCamera CoconutCamera;

	// Token: 0x040010BA RID: 4282
	[SerializeField]
	private List<GameObject> _visualObjects;

	// Token: 0x040010BB RID: 4283
	[SerializeField]
	private VRRig _vrrig;

	// Token: 0x040010BC RID: 4284
	private LckSocialCamera.CameraDataLocal _localData;

	// Token: 0x040010BD RID: 4285
	[WeaverGenerated]
	[DefaultForProperty("_networkedData", 0, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private LckSocialCamera.CameraData __networkedData;

	// Token: 0x02000249 RID: 585
	private enum CameraState
	{
		// Token: 0x040010BF RID: 4287
		Empty,
		// Token: 0x040010C0 RID: 4288
		Visible,
		// Token: 0x040010C1 RID: 4289
		Recording
	}

	// Token: 0x0200024A RID: 586
	[NetworkStructWeaved(1)]
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	private struct CameraData : INetworkStruct
	{
		// Token: 0x06000D8C RID: 3468 RVA: 0x000399DB File Offset: 0x00037BDB
		public CameraData(LckSocialCamera.CameraState currentState)
		{
			this.currentState = currentState;
		}

		// Token: 0x040010C2 RID: 4290
		[FieldOffset(0)]
		public LckSocialCamera.CameraState currentState;
	}

	// Token: 0x0200024B RID: 587
	private struct CameraDataLocal
	{
		// Token: 0x040010C3 RID: 4291
		public LckSocialCamera.CameraState currentState;
	}
}
