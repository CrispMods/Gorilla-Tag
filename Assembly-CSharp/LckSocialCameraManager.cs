using System;
using Liv.Lck;
using Liv.Lck.GorillaTag;
using UnityEngine;

// Token: 0x0200024C RID: 588
public class LckSocialCameraManager : MonoBehaviour
{
	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000D8D RID: 3469 RVA: 0x000399E4 File Offset: 0x00037BE4
	public LckDirectGrabbable lckDirectGrabbable
	{
		get
		{
			return this._lckDirectGrabbable;
		}
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000D8E RID: 3470 RVA: 0x000399EC File Offset: 0x00037BEC
	public static LckSocialCameraManager Instance
	{
		get
		{
			return LckSocialCameraManager._instance;
		}
	}

	// Token: 0x06000D8F RID: 3471 RVA: 0x000399F3 File Offset: 0x00037BF3
	private void Awake()
	{
		this.SetManagerInstance();
		this._lckCamera = this._gtLckController.GetActiveCamera();
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x00039A0C File Offset: 0x00037C0C
	public void SetLckSocialCamera(LckSocialCamera socialCamera)
	{
		this._socialCameraInstance = socialCamera;
	}

	// Token: 0x06000D91 RID: 3473 RVA: 0x00039A15 File Offset: 0x00037C15
	private void SetManagerInstance()
	{
		LckSocialCameraManager._instance = this;
		Action<LckSocialCameraManager> onManagerSpawned = LckSocialCameraManager.OnManagerSpawned;
		if (onManagerSpawned == null)
		{
			return;
		}
		onManagerSpawned(this);
	}

	// Token: 0x06000D92 RID: 3474 RVA: 0x000A20A8 File Offset: 0x000A02A8
	private void OnEnable()
	{
		LckResult<LckService> service = LckService.GetService();
		if (service.Result != null)
		{
			service.Result.OnRecordingStarted += this.OnRecordingStarted;
			service.Result.OnRecordingStopped += this.OnRecordingStopped;
		}
		this._gtLckController.OnCameraModeChanged += this.OnCameraModeChanged;
	}

	// Token: 0x06000D93 RID: 3475 RVA: 0x000A2108 File Offset: 0x000A0308
	private void Update()
	{
		if (this._socialCameraInstance != null)
		{
			if (this._lckCamera != null)
			{
				Transform transform = this._lckCamera.transform;
				this._socialCameraInstance.transform.position = transform.position;
				this._socialCameraInstance.transform.rotation = transform.rotation;
				Camera main = Camera.main;
				if (main != null)
				{
					this._lckCamera.nearClipPlane = main.nearClipPlane;
					this._lckCamera.farClipPlane = main.farClipPlane;
				}
			}
			CameraMode lckActiveCameraMode = this._lckActiveCameraMode;
			if (lckActiveCameraMode != CameraMode.Selfie)
			{
				if (lckActiveCameraMode == CameraMode.ThirdPerson)
				{
					this._socialCameraInstance.visible = this.cameraActive;
				}
				else
				{
					this._socialCameraInstance.visible = false;
				}
			}
			else
			{
				this._socialCameraInstance.visible = this.cameraActive;
			}
			this._socialCameraInstance.recording = this._recording;
		}
		if (this.CoconutCamera.gameObject.activeSelf)
		{
			CameraMode lckActiveCameraMode = this._lckActiveCameraMode;
			if (lckActiveCameraMode != CameraMode.Selfie)
			{
				if (lckActiveCameraMode == CameraMode.ThirdPerson)
				{
					this.CoconutCamera.SetVisualsActive(this.cameraActive);
				}
				else
				{
					this.CoconutCamera.SetVisualsActive(false);
				}
			}
			else
			{
				this.CoconutCamera.SetVisualsActive(false);
			}
			this.CoconutCamera.SetRecordingState(this._recording);
		}
	}

	// Token: 0x06000D94 RID: 3476 RVA: 0x000A224C File Offset: 0x000A044C
	private void OnDisable()
	{
		LckResult<LckService> service = LckService.GetService();
		if (service.Result != null)
		{
			service.Result.OnRecordingStarted -= this.OnRecordingStarted;
			service.Result.OnRecordingStopped -= this.OnRecordingStopped;
		}
		this._gtLckController.OnCameraModeChanged -= this.OnCameraModeChanged;
	}

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06000D95 RID: 3477 RVA: 0x00039A2D File Offset: 0x00037C2D
	// (set) Token: 0x06000D96 RID: 3478 RVA: 0x00039A3A File Offset: 0x00037C3A
	public bool cameraActive
	{
		get
		{
			return this._localCameras.activeSelf;
		}
		set
		{
			this._localCameras.SetActive(value);
			if (!value)
			{
				this._gtLckController.StopRecording();
			}
		}
	}

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06000D97 RID: 3479 RVA: 0x00039A57 File Offset: 0x00037C57
	// (set) Token: 0x06000D98 RID: 3480 RVA: 0x00039A64 File Offset: 0x00037C64
	public bool uiVisible
	{
		get
		{
			return this._localUi.activeSelf;
		}
		set
		{
			this._localUi.SetActive(value);
		}
	}

	// Token: 0x06000D99 RID: 3481 RVA: 0x00039A72 File Offset: 0x00037C72
	private void OnRecordingStarted(LckResult result)
	{
		this._recording = result.Success;
	}

	// Token: 0x06000D9A RID: 3482 RVA: 0x00039A80 File Offset: 0x00037C80
	private void OnRecordingStopped(LckResult result)
	{
		this._recording = false;
	}

	// Token: 0x06000D9B RID: 3483 RVA: 0x00039A89 File Offset: 0x00037C89
	private void OnCameraModeChanged(CameraMode mode, ILckCamera lckCamera)
	{
		this._lckCamera = lckCamera.GetCameraComponent();
		this._lckActiveCameraMode = mode;
	}

	// Token: 0x040010C4 RID: 4292
	[SerializeField]
	private GameObject _localUi;

	// Token: 0x040010C5 RID: 4293
	[SerializeField]
	private GameObject _localCameras;

	// Token: 0x040010C6 RID: 4294
	[SerializeField]
	private GTLckController _gtLckController;

	// Token: 0x040010C7 RID: 4295
	[SerializeField]
	private LckDirectGrabbable _lckDirectGrabbable;

	// Token: 0x040010C8 RID: 4296
	[SerializeField]
	public CoconutCamera CoconutCamera;

	// Token: 0x040010C9 RID: 4297
	private LckSocialCamera _socialCameraInstance;

	// Token: 0x040010CA RID: 4298
	private Camera _lckCamera;

	// Token: 0x040010CB RID: 4299
	private CameraMode _lckActiveCameraMode;

	// Token: 0x040010CC RID: 4300
	[OnEnterPlay_SetNull]
	private static LckSocialCameraManager _instance;

	// Token: 0x040010CD RID: 4301
	public static Action<LckSocialCameraManager> OnManagerSpawned;

	// Token: 0x040010CE RID: 4302
	private bool _recording;
}
