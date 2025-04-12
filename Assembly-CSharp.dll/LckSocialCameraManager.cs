using System;
using Liv.Lck;
using Liv.Lck.GorillaTag;
using UnityEngine;

// Token: 0x02000241 RID: 577
public class LckSocialCameraManager : MonoBehaviour
{
	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06000D44 RID: 3396 RVA: 0x00038724 File Offset: 0x00036924
	public LckDirectGrabbable lckDirectGrabbable
	{
		get
		{
			return this._lckDirectGrabbable;
		}
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06000D45 RID: 3397 RVA: 0x0003872C File Offset: 0x0003692C
	public static LckSocialCameraManager Instance
	{
		get
		{
			return LckSocialCameraManager._instance;
		}
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x00038733 File Offset: 0x00036933
	private void Awake()
	{
		this.SetManagerInstance();
		this._lckCamera = this._gtLckController.GetActiveCamera();
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x0003874C File Offset: 0x0003694C
	public void SetLckSocialCamera(LckSocialCamera socialCamera)
	{
		this._socialCameraInstance = socialCamera;
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x00038755 File Offset: 0x00036955
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

	// Token: 0x06000D49 RID: 3401 RVA: 0x0009F81C File Offset: 0x0009DA1C
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

	// Token: 0x06000D4A RID: 3402 RVA: 0x0009F87C File Offset: 0x0009DA7C
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

	// Token: 0x06000D4B RID: 3403 RVA: 0x0009F9C0 File Offset: 0x0009DBC0
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

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x06000D4C RID: 3404 RVA: 0x0003876D File Offset: 0x0003696D
	// (set) Token: 0x06000D4D RID: 3405 RVA: 0x0003877A File Offset: 0x0003697A
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

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06000D4E RID: 3406 RVA: 0x00038797 File Offset: 0x00036997
	// (set) Token: 0x06000D4F RID: 3407 RVA: 0x000387A4 File Offset: 0x000369A4
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

	// Token: 0x06000D50 RID: 3408 RVA: 0x000387B2 File Offset: 0x000369B2
	private void OnRecordingStarted(LckResult result)
	{
		this._recording = result.Success;
	}

	// Token: 0x06000D51 RID: 3409 RVA: 0x000387C0 File Offset: 0x000369C0
	private void OnRecordingStopped(LckResult result)
	{
		this._recording = false;
	}

	// Token: 0x06000D52 RID: 3410 RVA: 0x000387C9 File Offset: 0x000369C9
	private void OnCameraModeChanged(CameraMode mode, ILckCamera lckCamera)
	{
		this._lckCamera = lckCamera.GetCameraComponent();
		this._lckActiveCameraMode = mode;
	}

	// Token: 0x0400107F RID: 4223
	[SerializeField]
	private GameObject _localUi;

	// Token: 0x04001080 RID: 4224
	[SerializeField]
	private GameObject _localCameras;

	// Token: 0x04001081 RID: 4225
	[SerializeField]
	private GTLckController _gtLckController;

	// Token: 0x04001082 RID: 4226
	[SerializeField]
	private LckDirectGrabbable _lckDirectGrabbable;

	// Token: 0x04001083 RID: 4227
	[SerializeField]
	public CoconutCamera CoconutCamera;

	// Token: 0x04001084 RID: 4228
	private LckSocialCamera _socialCameraInstance;

	// Token: 0x04001085 RID: 4229
	private Camera _lckCamera;

	// Token: 0x04001086 RID: 4230
	private CameraMode _lckActiveCameraMode;

	// Token: 0x04001087 RID: 4231
	[OnEnterPlay_SetNull]
	private static LckSocialCameraManager _instance;

	// Token: 0x04001088 RID: 4232
	public static Action<LckSocialCameraManager> OnManagerSpawned;

	// Token: 0x04001089 RID: 4233
	private bool _recording;
}
