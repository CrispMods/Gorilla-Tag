using System;
using Cinemachine;
using Liv.Lck;
using Liv.Lck.GorillaTag;
using UnityEngine;

// Token: 0x02000251 RID: 593
public class MonitorOutputController : MonoBehaviour
{
	// Token: 0x06000DBB RID: 3515 RVA: 0x00039CBF File Offset: 0x00037EBF
	private void Awake()
	{
		this._lckCamera = this._gtLckController.GetActiveCamera();
	}

	// Token: 0x06000DBC RID: 3516 RVA: 0x00039CD2 File Offset: 0x00037ED2
	private void OnEnable()
	{
		this._gtLckController.OnCameraModeChanged += this.OnCameraModeChanged;
		LckBodyCameraSpawner.OnCameraStateChange += this.CameraStateChanged;
	}

	// Token: 0x06000DBD RID: 3517 RVA: 0x000A28F8 File Offset: 0x000A0AF8
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

	// Token: 0x06000DBE RID: 3518 RVA: 0x00039CFC File Offset: 0x00037EFC
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

	// Token: 0x06000DBF RID: 3519 RVA: 0x00039D25 File Offset: 0x00037F25
	private void OnDisable()
	{
		this._gtLckController.OnCameraModeChanged -= this.OnCameraModeChanged;
		this._shoulderCamera.gameObject.GetComponentInChildren<CinemachineBrain>().enabled = true;
		LckBodyCameraSpawner.OnCameraStateChange -= this.CameraStateChanged;
	}

	// Token: 0x06000DC0 RID: 3520 RVA: 0x00039D65 File Offset: 0x00037F65
	private void OnCameraModeChanged(CameraMode mode, ILckCamera lckCamera)
	{
		this._lckCamera = lckCamera.GetCameraComponent();
		this._lckActiveCameraMode = mode;
	}

	// Token: 0x06000DC1 RID: 3521 RVA: 0x00039D7A File Offset: 0x00037F7A
	private void TakeOverShoulderCamera()
	{
		this.FindShoulderCamera();
		this._shoulderCamera.gameObject.GetComponentInChildren<CinemachineBrain>().enabled = false;
		this._shoulderCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("LCKHide"));
	}

	// Token: 0x06000DC2 RID: 3522 RVA: 0x000A29A0 File Offset: 0x000A0BA0
	private void RestoreShoulderCamera()
	{
		this.FindShoulderCamera();
		this._shoulderCamera.gameObject.GetComponentInChildren<CinemachineBrain>().enabled = true;
		this._shoulderCamera.cullingMask |= 1 << LayerMask.NameToLayer("LCKHide");
		this._shoulderCamera.fieldOfView = this._shoulderCameraFov;
	}

	// Token: 0x06000DC3 RID: 3523 RVA: 0x000A29FC File Offset: 0x000A0BFC
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

	// Token: 0x040010E6 RID: 4326
	[SerializeField]
	private GTLckController _gtLckController;

	// Token: 0x040010E7 RID: 4327
	private Camera _lckCamera;

	// Token: 0x040010E8 RID: 4328
	private CameraMode _lckActiveCameraMode;

	// Token: 0x040010E9 RID: 4329
	private Camera _shoulderCamera;

	// Token: 0x040010EA RID: 4330
	private float _shoulderCameraFov;
}
