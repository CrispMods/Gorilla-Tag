using System;
using Cinemachine;
using Liv.Lck;
using Liv.Lck.GorillaTag;
using UnityEngine;

// Token: 0x02000246 RID: 582
public class MonitorOutputController : MonoBehaviour
{
	// Token: 0x06000D72 RID: 3442 RVA: 0x000389FF File Offset: 0x00036BFF
	private void Awake()
	{
		this._lckCamera = this._gtLckController.GetActiveCamera();
	}

	// Token: 0x06000D73 RID: 3443 RVA: 0x00038A12 File Offset: 0x00036C12
	private void OnEnable()
	{
		this._gtLckController.OnCameraModeChanged += this.OnCameraModeChanged;
		LckBodyCameraSpawner.OnCameraStateChange += this.CameraStateChanged;
	}

	// Token: 0x06000D74 RID: 3444 RVA: 0x000A006C File Offset: 0x0009E26C
	private void Update()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			UnityEngine.Object.Destroy(this);
		}
		if (this._shoulderCamera == null)
		{
			this.FindShoulderCamera();
		}
		if (this._lckCamera != null)
		{
			this._shoulderCamera.transform.position = this._lckCamera.transform.position;
			this._shoulderCamera.transform.rotation = this._lckCamera.transform.rotation;
			this._shoulderCamera.fieldOfView = this._lckCamera.fieldOfView;
			return;
		}
		this._lckCamera = this._gtLckController.GetActiveCamera();
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x00038A3C File Offset: 0x00036C3C
	private void CameraStateChanged(LckBodyCameraSpawner.CameraState state)
	{
		switch (state)
		{
		case LckBodyCameraSpawner.CameraState.CameraDisabled:
			this.RestoreShoulderCamera();
			return;
		case LckBodyCameraSpawner.CameraState.CameraOnNeck:
			this.TakeOverShoulderCamera();
			return;
		case LckBodyCameraSpawner.CameraState.CameraSpawned:
			this.TakeOverShoulderCamera();
			return;
		default:
			return;
		}
	}

	// Token: 0x06000D76 RID: 3446 RVA: 0x00038A65 File Offset: 0x00036C65
	private void OnDisable()
	{
		this._gtLckController.OnCameraModeChanged -= this.OnCameraModeChanged;
		this._shoulderCamera.gameObject.GetComponentInChildren<CinemachineBrain>().enabled = true;
		LckBodyCameraSpawner.OnCameraStateChange -= this.CameraStateChanged;
	}

	// Token: 0x06000D77 RID: 3447 RVA: 0x00038AA5 File Offset: 0x00036CA5
	private void OnCameraModeChanged(CameraMode mode, ILckCamera lckCamera)
	{
		this._lckCamera = lckCamera.GetCameraComponent();
		this._lckActiveCameraMode = mode;
	}

	// Token: 0x06000D78 RID: 3448 RVA: 0x00038ABA File Offset: 0x00036CBA
	private void TakeOverShoulderCamera()
	{
		this.FindShoulderCamera();
		this._shoulderCamera.gameObject.GetComponentInChildren<CinemachineBrain>().enabled = false;
		this._shoulderCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("LCKHide"));
	}

	// Token: 0x06000D79 RID: 3449 RVA: 0x000A0114 File Offset: 0x0009E314
	private void RestoreShoulderCamera()
	{
		this.FindShoulderCamera();
		this._shoulderCamera.gameObject.GetComponentInChildren<CinemachineBrain>().enabled = true;
		this._shoulderCamera.cullingMask |= 1 << LayerMask.NameToLayer("LCKHide");
		this._shoulderCamera.fieldOfView = this._shoulderCameraFov;
	}

	// Token: 0x06000D7A RID: 3450 RVA: 0x000A0170 File Offset: 0x0009E370
	private void FindShoulderCamera()
	{
		if (this._shoulderCamera != null)
		{
			return;
		}
		if (!GorillaTagger.hasInstance || !base.isActiveAndEnabled)
		{
			return;
		}
		this._shoulderCamera = GorillaTagger.Instance.thirdPersonCamera.GetComponentInChildren<Camera>();
		this._shoulderCameraFov = this._shoulderCamera.fieldOfView;
	}

	// Token: 0x040010A1 RID: 4257
	[SerializeField]
	private GTLckController _gtLckController;

	// Token: 0x040010A2 RID: 4258
	private Camera _lckCamera;

	// Token: 0x040010A3 RID: 4259
	private CameraMode _lckActiveCameraMode;

	// Token: 0x040010A4 RID: 4260
	private Camera _shoulderCamera;

	// Token: 0x040010A5 RID: 4261
	private float _shoulderCameraFov;
}
