using System;
using Liv.Lck;
using Liv.Lck.GorillaTag;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x0200023F RID: 575
public class LckBodyCameraSpawner : MonoBehaviour
{
	// Token: 0x06000D37 RID: 3383 RVA: 0x00039568 File Offset: 0x00037768
	public void SetFollowTransform(Transform transform)
	{
		this._followTransform = transform;
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06000D38 RID: 3384 RVA: 0x00039571 File Offset: 0x00037771
	public TabletSpawnInstance tabletSpawnInstance
	{
		get
		{
			return this._tabletSpawnInstance;
		}
	}

	// Token: 0x14000025 RID: 37
	// (add) Token: 0x06000D39 RID: 3385 RVA: 0x000A1380 File Offset: 0x0009F580
	// (remove) Token: 0x06000D3A RID: 3386 RVA: 0x000A13B4 File Offset: 0x0009F5B4
	public static event LckBodyCameraSpawner.CameraStateDelegate OnCameraStateChange;

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06000D3B RID: 3387 RVA: 0x00039579 File Offset: 0x00037779
	// (set) Token: 0x06000D3C RID: 3388 RVA: 0x000A13E8 File Offset: 0x0009F5E8
	public LckBodyCameraSpawner.CameraState cameraState
	{
		get
		{
			return this._cameraState;
		}
		set
		{
			switch (value)
			{
			case LckBodyCameraSpawner.CameraState.CameraDisabled:
				this.cameraPosition = LckBodyCameraSpawner.CameraPosition.NotVisible;
				this._tabletSpawnInstance.uiVisible = false;
				this._tabletSpawnInstance.cameraActive = false;
				this.ResetCameraModel();
				this.cameraVisible = false;
				this._shouldMoveCameraToNeck = false;
				break;
			case LckBodyCameraSpawner.CameraState.CameraOnNeck:
				this.cameraPosition = LckBodyCameraSpawner.CameraPosition.CameraDefault;
				this._tabletSpawnInstance.uiVisible = false;
				this._tabletSpawnInstance.cameraActive = true;
				this.ResetCameraModel();
				if (Application.platform == RuntimePlatform.Android)
				{
					this.SetPreviewActive(false);
				}
				this.cameraVisible = true;
				this._shouldMoveCameraToNeck = false;
				this._dummyTablet.SetTabletIsSpawned(false);
				break;
			case LckBodyCameraSpawner.CameraState.CameraSpawned:
				this.cameraPosition = LckBodyCameraSpawner.CameraPosition.CameraDefault;
				this._tabletSpawnInstance.uiVisible = true;
				this._tabletSpawnInstance.cameraActive = true;
				if (Application.platform == RuntimePlatform.Android)
				{
					this.SetPreviewActive(true);
				}
				this.ResetCameraModel();
				this.cameraVisible = true;
				this._shouldMoveCameraToNeck = false;
				this._dummyTablet.SetTabletIsSpawned(true);
				break;
			}
			this._cameraState = value;
			LckBodyCameraSpawner.CameraStateDelegate onCameraStateChange = LckBodyCameraSpawner.OnCameraStateChange;
			if (onCameraStateChange == null)
			{
				return;
			}
			onCameraStateChange(this._cameraState);
		}
	}

	// Token: 0x06000D3D RID: 3389 RVA: 0x000A1500 File Offset: 0x0009F700
	private void SetPreviewActive(bool isActive)
	{
		LckResult<LckService> service = LckService.GetService();
		if (!service.Success)
		{
			Debug.LogError("LCK Could not get Service" + service.Error.ToString());
			return;
		}
		LckService result = service.Result;
		if (result == null)
		{
			return;
		}
		result.SetPreviewActive(isActive);
	}

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06000D3E RID: 3390 RVA: 0x00039581 File Offset: 0x00037781
	// (set) Token: 0x06000D3F RID: 3391 RVA: 0x000A1554 File Offset: 0x0009F754
	public LckBodyCameraSpawner.CameraPosition cameraPosition
	{
		get
		{
			return this._cameraPosition;
		}
		set
		{
			if (this._cameraModelTransform != null && this._cameraPosition != value)
			{
				switch (value)
				{
				case LckBodyCameraSpawner.CameraPosition.CameraDefault:
					this.ChangeCameraModelParent(this._cameraPositionDefault);
					this._cameraPosition = LckBodyCameraSpawner.CameraPosition.CameraDefault;
					return;
				case LckBodyCameraSpawner.CameraPosition.CameraSlingshot:
					this.ChangeCameraModelParent(this._cameraPositionSlingshot);
					this._cameraPosition = LckBodyCameraSpawner.CameraPosition.CameraSlingshot;
					break;
				case LckBodyCameraSpawner.CameraPosition.NotVisible:
					break;
				default:
					return;
				}
			}
		}
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06000D40 RID: 3392 RVA: 0x00039589 File Offset: 0x00037789
	// (set) Token: 0x06000D41 RID: 3393 RVA: 0x0003959B File Offset: 0x0003779B
	private bool cameraVisible
	{
		get
		{
			return this._cameraModelTransform.gameObject.activeSelf;
		}
		set
		{
			this._cameraModelTransform.gameObject.SetActive(value);
			this._cameraStrapRenderer.enabled = value;
		}
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x000395BA File Offset: 0x000377BA
	private void Awake()
	{
		this._tabletSpawnInstance = new TabletSpawnInstance(this._cameraSpawnPrefab, this._cameraSpawnParentTransform);
	}

	// Token: 0x06000D43 RID: 3395 RVA: 0x000395D3 File Offset: 0x000377D3
	private void OnEnable()
	{
		this.InitCameraStrap();
		this.cameraState = LckBodyCameraSpawner.CameraState.CameraDisabled;
		this.cameraPosition = LckBodyCameraSpawner.CameraPosition.CameraDefault;
		ZoneManagement.OnZoneChange += this.OnZoneChanged;
	}

	// Token: 0x06000D44 RID: 3396 RVA: 0x000A15B4 File Offset: 0x0009F7B4
	private void Update()
	{
		if (this._followTransform != null && base.transform.parent != null)
		{
			Matrix4x4 localToWorldMatrix = base.transform.parent.localToWorldMatrix;
			Vector3 position = localToWorldMatrix.MultiplyPoint(this._followTransform.localPosition + this._followTransform.localRotation * new Vector3(0f, -0.05f, 0.1f));
			Quaternion rotation = Quaternion.LookRotation(localToWorldMatrix.MultiplyVector(this._followTransform.localRotation * Vector3.forward), localToWorldMatrix.MultiplyVector(this._followTransform.localRotation * Vector3.up));
			base.transform.SetPositionAndRotation(position, rotation);
		}
		LckBodyCameraSpawner.CameraState cameraState = this._cameraState;
		if (cameraState != LckBodyCameraSpawner.CameraState.CameraOnNeck)
		{
			if (cameraState == LckBodyCameraSpawner.CameraState.CameraSpawned)
			{
				this.UpdateCameraStrap();
				if (this._cameraModelGrabbable.isGrabbed)
				{
					GorillaGrabber grabber = this._cameraModelGrabbable.grabber;
					Transform transform = grabber.transform;
					if (this.ShouldSpawnCamera(transform))
					{
						this.SpawnCamera(grabber, transform);
					}
				}
				else
				{
					this.ResetCameraModel();
				}
				if (this._tabletSpawnInstance.isSpawned)
				{
					Transform transform3;
					if (this._tabletSpawnInstance.directGrabbable.isGrabbed)
					{
						GorillaGrabber grabber2 = this._tabletSpawnInstance.directGrabbable.grabber;
						Transform transform2 = grabber2.transform;
						if (!this.ShouldSpawnCamera(transform2))
						{
							this.cameraState = LckBodyCameraSpawner.CameraState.CameraOnNeck;
							this._cameraModelGrabbable.target.SetPositionAndRotation(transform2.position, transform2.rotation * Quaternion.Euler(0f, 180f, 0f));
							this._tabletSpawnInstance.directGrabbable.ForceRelease();
							this._tabletSpawnInstance.SetParent(this._cameraModelTransform);
							this._tabletSpawnInstance.ResetLocalPose();
							this._cameraModelGrabbable.ForceGrab(grabber2);
							this._cameraModelGrabbable.onReleased += this.OnCameraModelReleased;
						}
					}
					else if (this._shouldMoveCameraToNeck && GtTag.TryGetTransform(GtTagType.HMD, out transform3) && Vector3.SqrMagnitude(transform3.position - this.tabletSpawnInstance.position) >= this._snapToNeckDistance * this._snapToNeckDistance)
					{
						this.cameraState = LckBodyCameraSpawner.CameraState.CameraOnNeck;
						this._tabletSpawnInstance.SetParent(this._cameraModelTransform);
						this._tabletSpawnInstance.ResetLocalPose();
						this._shouldMoveCameraToNeck = false;
					}
				}
			}
		}
		else
		{
			this.UpdateCameraStrap();
			if (this._cameraModelGrabbable.isGrabbed)
			{
				GorillaGrabber grabber3 = this._cameraModelGrabbable.grabber;
				Transform transform4 = grabber3.transform;
				if (this.ShouldSpawnCamera(transform4))
				{
					this.SpawnCamera(grabber3, transform4);
				}
			}
			else
			{
				this.ResetCameraModel();
			}
		}
		if (!this.IsSlingshotActiveInHierarchy())
		{
			this.cameraPosition = LckBodyCameraSpawner.CameraPosition.CameraDefault;
			return;
		}
		this.cameraPosition = LckBodyCameraSpawner.CameraPosition.CameraSlingshot;
	}

	// Token: 0x06000D45 RID: 3397 RVA: 0x000395FA File Offset: 0x000377FA
	private void OnDisable()
	{
		ZoneManagement.OnZoneChange -= this.OnZoneChanged;
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x0003960D File Offset: 0x0003780D
	private void OnDestroy()
	{
		this._tabletSpawnInstance.Dispose();
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x0003961A File Offset: 0x0003781A
	public void ManuallySetCameraOnNeck()
	{
		if (this._tabletSpawnInstance.isSpawned)
		{
			this.cameraState = LckBodyCameraSpawner.CameraState.CameraOnNeck;
		}
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x00039630 File Offset: 0x00037830
	private void OnZoneChanged(ZoneData[] zones)
	{
		if (!this._tabletSpawnInstance.isSpawned || this._tabletSpawnInstance.directGrabbable.isGrabbed)
		{
			return;
		}
		this._shouldMoveCameraToNeck = true;
	}

	// Token: 0x06000D49 RID: 3401 RVA: 0x00039659 File Offset: 0x00037859
	private void OnCameraModelReleased()
	{
		this._cameraModelGrabbable.onReleased -= this.OnCameraModelReleased;
		this.ResetCameraModel();
	}

	// Token: 0x06000D4A RID: 3402 RVA: 0x000A1888 File Offset: 0x0009FA88
	public void SpawnCamera(GorillaGrabber overrideGorillaGrabber, Transform transform)
	{
		if (!this._tabletSpawnInstance.isSpawned)
		{
			this._tabletSpawnInstance.SpawnCamera();
		}
		Vector3 zero = Vector3.zero;
		XRNode xrNode = overrideGorillaGrabber.XrNode;
		if (xrNode != XRNode.LeftHand)
		{
			if (xrNode == XRNode.RightHand)
			{
				zero = new Vector3(0.25f, -0.12f, 0.03f);
			}
		}
		else
		{
			zero = new Vector3(-0.25f, -0.12f, 0.03f);
		}
		this.cameraState = LckBodyCameraSpawner.CameraState.CameraSpawned;
		this._cameraModelGrabbable.ForceRelease();
		this._tabletSpawnInstance.ResetParent();
		Quaternion rotation = transform.rotation * Quaternion.Euler(0f, 180f, 0f);
		Matrix4x4 lhs = transform.localToWorldMatrix;
		lhs *= Matrix4x4.Rotate(Quaternion.Euler(0f, 180f, 0f));
		this._tabletSpawnInstance.SetPositionAndRotation(lhs.MultiplyPoint(zero), rotation);
		this._tabletSpawnInstance.directGrabbable.ForceGrab(overrideGorillaGrabber);
		this._tabletSpawnInstance.SetLocalScale(Vector3.one);
	}

	// Token: 0x06000D4B RID: 3403 RVA: 0x000A198C File Offset: 0x0009FB8C
	private bool ShouldSpawnCamera(Transform gorillaGrabberTransform)
	{
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		Vector3 a = worldToLocalMatrix.MultiplyPoint(this._cameraModelOriginTransform.position);
		Vector3 b = worldToLocalMatrix.MultiplyPoint(gorillaGrabberTransform.position);
		return Vector3.SqrMagnitude(a - b) >= this._activateDistance * this._activateDistance;
	}

	// Token: 0x06000D4C RID: 3404 RVA: 0x000A19E4 File Offset: 0x0009FBE4
	private void ChangeCameraModelParent(Transform transform)
	{
		if (this._cameraModelTransform != null)
		{
			this._cameraModelGrabbable.SetOriginalTargetParent(transform);
			if (!this._cameraModelGrabbable.isGrabbed)
			{
				this._cameraModelTransform.transform.parent = transform;
				this._cameraModelTransform.transform.localPosition = Vector3.zero;
			}
		}
	}

	// Token: 0x06000D4D RID: 3405 RVA: 0x00039678 File Offset: 0x00037878
	private void InitCameraStrap()
	{
		this._cameraStrapRenderer.positionCount = this._cameraStrapPoints.Length;
		this._cameraStrapPositions = new Vector3[this._cameraStrapPoints.Length];
	}

	// Token: 0x06000D4E RID: 3406 RVA: 0x000A1A40 File Offset: 0x0009FC40
	private void UpdateCameraStrap()
	{
		for (int i = 0; i < this._cameraStrapPoints.Length; i++)
		{
			this._cameraStrapPositions[i] = this._cameraStrapPoints[i].position;
		}
		this._cameraStrapRenderer.SetPositions(this._cameraStrapPositions);
		Vector3 lossyScale = base.transform.lossyScale;
		float num = (lossyScale.x + lossyScale.y + lossyScale.z) * 0.3333333f;
		this._cameraStrapRenderer.widthMultiplier = num * 0.02f;
		Color color = (this.cameraState == LckBodyCameraSpawner.CameraState.CameraSpawned) ? this._ghostColor : this._normalColor;
		this._cameraStrapRenderer.startColor = color;
		this._cameraStrapRenderer.endColor = color;
	}

	// Token: 0x06000D4F RID: 3407 RVA: 0x000396A0 File Offset: 0x000378A0
	private void ResetCameraModel()
	{
		this._cameraModelTransform.localPosition = Vector3.zero;
		this._cameraModelTransform.localRotation = Quaternion.identity;
	}

	// Token: 0x06000D50 RID: 3408 RVA: 0x000396C2 File Offset: 0x000378C2
	private VRRig GetLocalRig()
	{
		if (this._localRig == null)
		{
			this._localRig = VRRigCache.Instance.localRig.Rig;
		}
		return this._localRig;
	}

	// Token: 0x06000D51 RID: 3409 RVA: 0x000A1AF4 File Offset: 0x0009FCF4
	private bool IsSlingshotHeldInHand(out bool leftHand, out bool rightHand)
	{
		VRRig localRig = this.GetLocalRig();
		if (localRig == null)
		{
			leftHand = false;
			rightHand = false;
			return false;
		}
		leftHand = localRig.projectileWeapon.InLeftHand();
		rightHand = localRig.projectileWeapon.InRightHand();
		return localRig.projectileWeapon.InHand();
	}

	// Token: 0x06000D52 RID: 3410 RVA: 0x000A1B40 File Offset: 0x0009FD40
	private bool IsSlingshotActiveInHierarchy()
	{
		VRRig localRig = this.GetLocalRig();
		return !(localRig == null) && !(localRig.projectileWeapon == null) && localRig.projectileWeapon.gameObject.activeInHierarchy;
	}

	// Token: 0x04001088 RID: 4232
	[SerializeField]
	private GameObject _cameraSpawnPrefab;

	// Token: 0x04001089 RID: 4233
	[SerializeField]
	private Transform _cameraSpawnParentTransform;

	// Token: 0x0400108A RID: 4234
	[SerializeField]
	private Transform _cameraModelOriginTransform;

	// Token: 0x0400108B RID: 4235
	[SerializeField]
	private Transform _cameraModelTransform;

	// Token: 0x0400108C RID: 4236
	[SerializeField]
	private LckDirectGrabbable _cameraModelGrabbable;

	// Token: 0x0400108D RID: 4237
	[SerializeField]
	private Transform _cameraPositionDefault;

	// Token: 0x0400108E RID: 4238
	[SerializeField]
	private Transform _cameraPositionSlingshot;

	// Token: 0x0400108F RID: 4239
	[SerializeField]
	private float _activateDistance = 0.25f;

	// Token: 0x04001090 RID: 4240
	[SerializeField]
	private float _snapToNeckDistance = 15f;

	// Token: 0x04001091 RID: 4241
	[SerializeField]
	private LineRenderer _cameraStrapRenderer;

	// Token: 0x04001092 RID: 4242
	[SerializeField]
	private Transform[] _cameraStrapPoints;

	// Token: 0x04001093 RID: 4243
	[SerializeField]
	private Color _normalColor = Color.red;

	// Token: 0x04001094 RID: 4244
	[SerializeField]
	private Color _ghostColor = Color.gray;

	// Token: 0x04001095 RID: 4245
	[SerializeField]
	private GtDummyTablet _dummyTablet;

	// Token: 0x04001096 RID: 4246
	private Transform _followTransform;

	// Token: 0x04001097 RID: 4247
	private Vector3[] _cameraStrapPositions;

	// Token: 0x04001098 RID: 4248
	private TabletSpawnInstance _tabletSpawnInstance;

	// Token: 0x04001099 RID: 4249
	private VRRig _localRig;

	// Token: 0x0400109A RID: 4250
	private bool _shouldMoveCameraToNeck;

	// Token: 0x0400109C RID: 4252
	private LckBodyCameraSpawner.CameraState _cameraState;

	// Token: 0x0400109D RID: 4253
	private LckBodyCameraSpawner.CameraPosition _cameraPosition;

	// Token: 0x02000240 RID: 576
	public enum CameraState
	{
		// Token: 0x0400109F RID: 4255
		CameraDisabled,
		// Token: 0x040010A0 RID: 4256
		CameraOnNeck,
		// Token: 0x040010A1 RID: 4257
		CameraSpawned
	}

	// Token: 0x02000241 RID: 577
	public enum CameraPosition
	{
		// Token: 0x040010A3 RID: 4259
		CameraDefault,
		// Token: 0x040010A4 RID: 4260
		CameraSlingshot,
		// Token: 0x040010A5 RID: 4261
		NotVisible
	}

	// Token: 0x02000242 RID: 578
	// (Invoke) Token: 0x06000D55 RID: 3413
	public delegate void CameraStateDelegate(LckBodyCameraSpawner.CameraState state);
}
