﻿using System;
using Liv.Lck;
using Liv.Lck.GorillaTag;
using UnityEngine;

// Token: 0x02000241 RID: 577
public class LckSocialCameraManager : MonoBehaviour
{
	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06000D42 RID: 3394 RVA: 0x0004490E File Offset: 0x00042B0E
	public LckDirectGrabbable lckDirectGrabbable
	{
		get
		{
			return this._lckDirectGrabbable;
		}
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06000D43 RID: 3395 RVA: 0x00044916 File Offset: 0x00042B16
	public static LckSocialCameraManager Instance
	{
		get
		{
			return LckSocialCameraManager._instance;
		}
	}

	// Token: 0x06000D44 RID: 3396 RVA: 0x0004491D File Offset: 0x00042B1D
	private void Awake()
	{
		this.SetManagerInstance();
		this._lckCamera = this._gtLckController.GetActiveCamera();
	}

	// Token: 0x06000D45 RID: 3397 RVA: 0x00044936 File Offset: 0x00042B36
	public void SetLckSocialCamera(LckSocialCamera socialCamera)
	{
		this._socialCameraInstance = socialCamera;
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x0004493F File Offset: 0x00042B3F
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

	// Token: 0x06000D47 RID: 3399 RVA: 0x00044958 File Offset: 0x00042B58
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

	// Token: 0x06000D48 RID: 3400 RVA: 0x000449B8 File Offset: 0x00042BB8
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

	// Token: 0x06000D49 RID: 3401 RVA: 0x00044AFC File Offset: 0x00042CFC
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
	// (get) Token: 0x06000D4A RID: 3402 RVA: 0x00044B5C File Offset: 0x00042D5C
	// (set) Token: 0x06000D4B RID: 3403 RVA: 0x00044B69 File Offset: 0x00042D69
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
	// (get) Token: 0x06000D4C RID: 3404 RVA: 0x00044B86 File Offset: 0x00042D86
	// (set) Token: 0x06000D4D RID: 3405 RVA: 0x00044B93 File Offset: 0x00042D93
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

	// Token: 0x06000D4E RID: 3406 RVA: 0x00044BA1 File Offset: 0x00042DA1
	private void OnRecordingStarted(LckResult result)
	{
		this._recording = result.Success;
	}

	// Token: 0x06000D4F RID: 3407 RVA: 0x00044BAF File Offset: 0x00042DAF
	private void OnRecordingStopped(LckResult result)
	{
		this._recording = false;
	}

	// Token: 0x06000D50 RID: 3408 RVA: 0x00044BB8 File Offset: 0x00042DB8
	private void OnCameraModeChanged(CameraMode mode, ILckCamera lckCamera)
	{
		this._lckCamera = lckCamera.GetCameraComponent();
		this._lckActiveCameraMode = mode;
	}

	// Token: 0x0400107E RID: 4222
	[SerializeField]
	private GameObject _localUi;

	// Token: 0x0400107F RID: 4223
	[SerializeField]
	private GameObject _localCameras;

	// Token: 0x04001080 RID: 4224
	[SerializeField]
	private GTLckController _gtLckController;

	// Token: 0x04001081 RID: 4225
	[SerializeField]
	private LckDirectGrabbable _lckDirectGrabbable;

	// Token: 0x04001082 RID: 4226
	[SerializeField]
	public CoconutCamera CoconutCamera;

	// Token: 0x04001083 RID: 4227
	private LckSocialCamera _socialCameraInstance;

	// Token: 0x04001084 RID: 4228
	private Camera _lckCamera;

	// Token: 0x04001085 RID: 4229
	private CameraMode _lckActiveCameraMode;

	// Token: 0x04001086 RID: 4230
	[OnEnterPlay_SetNull]
	private static LckSocialCameraManager _instance;

	// Token: 0x04001087 RID: 4231
	public static Action<LckSocialCameraManager> OnManagerSpawned;

	// Token: 0x04001088 RID: 4232
	private bool _recording;
}
