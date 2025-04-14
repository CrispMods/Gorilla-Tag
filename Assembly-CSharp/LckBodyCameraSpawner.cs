using System;
using Liv.Lck;
using Liv.Lck.GorillaTag;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000234 RID: 564
public class LckBodyCameraSpawner : MonoBehaviour
{
	// Token: 0x06000CEC RID: 3308 RVA: 0x00043771 File Offset: 0x00041971
	public void SetFollowTransform(Transform transform)
	{
		this._followTransform = transform;
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06000CED RID: 3309 RVA: 0x0004377A File Offset: 0x0004197A
	public TabletSpawnInstance tabletSpawnInstance
	{
		get
		{
			return this._tabletSpawnInstance;
		}
	}

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x06000CEE RID: 3310 RVA: 0x00043784 File Offset: 0x00041984
	// (remove) Token: 0x06000CEF RID: 3311 RVA: 0x000437B8 File Offset: 0x000419B8
	public static event LckBodyCameraSpawner.CameraStateDelegate OnCameraStateChange;

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x000437EB File Offset: 0x000419EB
	// (set) Token: 0x06000CF1 RID: 3313 RVA: 0x000437F4 File Offset: 0x000419F4
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

	// Token: 0x06000CF2 RID: 3314 RVA: 0x0004390C File Offset: 0x00041B0C
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

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x0004395D File Offset: 0x00041B5D
	// (set) Token: 0x06000CF4 RID: 3316 RVA: 0x00043968 File Offset: 0x00041B68
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

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x000439C6 File Offset: 0x00041BC6
	// (set) Token: 0x06000CF6 RID: 3318 RVA: 0x000439D8 File Offset: 0x00041BD8
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

	// Token: 0x06000CF7 RID: 3319 RVA: 0x000439F7 File Offset: 0x00041BF7
	private void Awake()
	{
		this._tabletSpawnInstance = new TabletSpawnInstance(this._cameraSpawnPrefab, this._cameraSpawnParentTransform);
	}

	// Token: 0x06000CF8 RID: 3320 RVA: 0x00043A10 File Offset: 0x00041C10
	private void OnEnable()
	{
		this.InitCameraStrap();
		this.cameraState = LckBodyCameraSpawner.CameraState.CameraDisabled;
		this.cameraPosition = LckBodyCameraSpawner.CameraPosition.CameraDefault;
		ZoneManagement.OnZoneChange += this.OnZoneChanged;
	}

	// Token: 0x06000CF9 RID: 3321 RVA: 0x00043A38 File Offset: 0x00041C38
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

	// Token: 0x06000CFA RID: 3322 RVA: 0x00043D0C File Offset: 0x00041F0C
	private void OnDisable()
	{
		ZoneManagement.OnZoneChange -= this.OnZoneChanged;
	}

	// Token: 0x06000CFB RID: 3323 RVA: 0x00043D1F File Offset: 0x00041F1F
	private void OnDestroy()
	{
		this._tabletSpawnInstance.Dispose();
	}

	// Token: 0x06000CFC RID: 3324 RVA: 0x00043D2C File Offset: 0x00041F2C
	public void ManuallySetCameraOnNeck()
	{
		if (this._tabletSpawnInstance.isSpawned)
		{
			this.cameraState = LckBodyCameraSpawner.CameraState.CameraOnNeck;
		}
	}

	// Token: 0x06000CFD RID: 3325 RVA: 0x00043D42 File Offset: 0x00041F42
	private void OnZoneChanged(ZoneData[] zones)
	{
		if (!this._tabletSpawnInstance.isSpawned || this._tabletSpawnInstance.directGrabbable.isGrabbed)
		{
			return;
		}
		this._shouldMoveCameraToNeck = true;
	}

	// Token: 0x06000CFE RID: 3326 RVA: 0x00043D6B File Offset: 0x00041F6B
	private void OnCameraModelReleased()
	{
		this._cameraModelGrabbable.onReleased -= this.OnCameraModelReleased;
		this.ResetCameraModel();
	}

	// Token: 0x06000CFF RID: 3327 RVA: 0x00043D8C File Offset: 0x00041F8C
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

	// Token: 0x06000D00 RID: 3328 RVA: 0x00043E90 File Offset: 0x00042090
	private bool ShouldSpawnCamera(Transform gorillaGrabberTransform)
	{
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		Vector3 a = worldToLocalMatrix.MultiplyPoint(this._cameraModelOriginTransform.position);
		Vector3 b = worldToLocalMatrix.MultiplyPoint(gorillaGrabberTransform.position);
		return Vector3.SqrMagnitude(a - b) >= this._activateDistance * this._activateDistance;
	}

	// Token: 0x06000D01 RID: 3329 RVA: 0x00043EE8 File Offset: 0x000420E8
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

	// Token: 0x06000D02 RID: 3330 RVA: 0x00043F42 File Offset: 0x00042142
	private void InitCameraStrap()
	{
		this._cameraStrapRenderer.positionCount = this._cameraStrapPoints.Length;
		this._cameraStrapPositions = new Vector3[this._cameraStrapPoints.Length];
	}

	// Token: 0x06000D03 RID: 3331 RVA: 0x00043F6C File Offset: 0x0004216C
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

	// Token: 0x06000D04 RID: 3332 RVA: 0x0004401F File Offset: 0x0004221F
	private void ResetCameraModel()
	{
		this._cameraModelTransform.localPosition = Vector3.zero;
		this._cameraModelTransform.localRotation = Quaternion.identity;
	}

	// Token: 0x06000D05 RID: 3333 RVA: 0x00044041 File Offset: 0x00042241
	private VRRig GetLocalRig()
	{
		if (this._localRig == null)
		{
			this._localRig = VRRigCache.Instance.localRig.Rig;
		}
		return this._localRig;
	}

	// Token: 0x06000D06 RID: 3334 RVA: 0x0004406C File Offset: 0x0004226C
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

	// Token: 0x06000D07 RID: 3335 RVA: 0x000440B8 File Offset: 0x000422B8
	private bool IsSlingshotActiveInHierarchy()
	{
		VRRig localRig = this.GetLocalRig();
		return !(localRig == null) && !(localRig.projectileWeapon == null) && localRig.projectileWeapon.gameObject.activeInHierarchy;
	}

	// Token: 0x04001042 RID: 4162
	[SerializeField]
	private GameObject _cameraSpawnPrefab;

	// Token: 0x04001043 RID: 4163
	[SerializeField]
	private Transform _cameraSpawnParentTransform;

	// Token: 0x04001044 RID: 4164
	[SerializeField]
	private Transform _cameraModelOriginTransform;

	// Token: 0x04001045 RID: 4165
	[SerializeField]
	private Transform _cameraModelTransform;

	// Token: 0x04001046 RID: 4166
	[SerializeField]
	private LckDirectGrabbable _cameraModelGrabbable;

	// Token: 0x04001047 RID: 4167
	[SerializeField]
	private Transform _cameraPositionDefault;

	// Token: 0x04001048 RID: 4168
	[SerializeField]
	private Transform _cameraPositionSlingshot;

	// Token: 0x04001049 RID: 4169
	[SerializeField]
	private float _activateDistance = 0.25f;

	// Token: 0x0400104A RID: 4170
	[SerializeField]
	private float _snapToNeckDistance = 15f;

	// Token: 0x0400104B RID: 4171
	[SerializeField]
	private LineRenderer _cameraStrapRenderer;

	// Token: 0x0400104C RID: 4172
	[SerializeField]
	private Transform[] _cameraStrapPoints;

	// Token: 0x0400104D RID: 4173
	[SerializeField]
	private Color _normalColor = Color.red;

	// Token: 0x0400104E RID: 4174
	[SerializeField]
	private Color _ghostColor = Color.gray;

	// Token: 0x0400104F RID: 4175
	[SerializeField]
	private GtDummyTablet _dummyTablet;

	// Token: 0x04001050 RID: 4176
	private Transform _followTransform;

	// Token: 0x04001051 RID: 4177
	private Vector3[] _cameraStrapPositions;

	// Token: 0x04001052 RID: 4178
	private TabletSpawnInstance _tabletSpawnInstance;

	// Token: 0x04001053 RID: 4179
	private VRRig _localRig;

	// Token: 0x04001054 RID: 4180
	private bool _shouldMoveCameraToNeck;

	// Token: 0x04001056 RID: 4182
	private LckBodyCameraSpawner.CameraState _cameraState;

	// Token: 0x04001057 RID: 4183
	private LckBodyCameraSpawner.CameraPosition _cameraPosition;

	// Token: 0x02000235 RID: 565
	public enum CameraState
	{
		// Token: 0x04001059 RID: 4185
		CameraDisabled,
		// Token: 0x0400105A RID: 4186
		CameraOnNeck,
		// Token: 0x0400105B RID: 4187
		CameraSpawned
	}

	// Token: 0x02000236 RID: 566
	public enum CameraPosition
	{
		// Token: 0x0400105D RID: 4189
		CameraDefault,
		// Token: 0x0400105E RID: 4190
		CameraSlingshot,
		// Token: 0x0400105F RID: 4191
		NotVisible
	}

	// Token: 0x02000237 RID: 567
	// (Invoke) Token: 0x06000D0A RID: 3338
	public delegate void CameraStateDelegate(LckBodyCameraSpawner.CameraState state);
}
