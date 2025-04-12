using System;
using Liv.Lck;
using Liv.Lck.GorillaTag;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000234 RID: 564
public class LckBodyCameraSpawner : MonoBehaviour
{
	// Token: 0x06000CEE RID: 3310 RVA: 0x000382A8 File Offset: 0x000364A8
	public void SetFollowTransform(Transform transform)
	{
		this._followTransform = transform;
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06000CEF RID: 3311 RVA: 0x000382B1 File Offset: 0x000364B1
	public TabletSpawnInstance tabletSpawnInstance
	{
		get
		{
			return this._tabletSpawnInstance;
		}
	}

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x06000CF0 RID: 3312 RVA: 0x0009EAF4 File Offset: 0x0009CCF4
	// (remove) Token: 0x06000CF1 RID: 3313 RVA: 0x0009EB28 File Offset: 0x0009CD28
	public static event LckBodyCameraSpawner.CameraStateDelegate OnCameraStateChange;

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x000382B9 File Offset: 0x000364B9
	// (set) Token: 0x06000CF3 RID: 3315 RVA: 0x0009EB5C File Offset: 0x0009CD5C
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

	// Token: 0x06000CF4 RID: 3316 RVA: 0x0009EC74 File Offset: 0x0009CE74
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
	// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x000382C1 File Offset: 0x000364C1
	// (set) Token: 0x06000CF6 RID: 3318 RVA: 0x0009ECC8 File Offset: 0x0009CEC8
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
	// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x000382C9 File Offset: 0x000364C9
	// (set) Token: 0x06000CF8 RID: 3320 RVA: 0x000382DB File Offset: 0x000364DB
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

	// Token: 0x06000CF9 RID: 3321 RVA: 0x000382FA File Offset: 0x000364FA
	private void Awake()
	{
		this._tabletSpawnInstance = new TabletSpawnInstance(this._cameraSpawnPrefab, this._cameraSpawnParentTransform);
	}

	// Token: 0x06000CFA RID: 3322 RVA: 0x00038313 File Offset: 0x00036513
	private void OnEnable()
	{
		this.InitCameraStrap();
		this.cameraState = LckBodyCameraSpawner.CameraState.CameraDisabled;
		this.cameraPosition = LckBodyCameraSpawner.CameraPosition.CameraDefault;
		ZoneManagement.OnZoneChange += this.OnZoneChanged;
	}

	// Token: 0x06000CFB RID: 3323 RVA: 0x0009ED28 File Offset: 0x0009CF28
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

	// Token: 0x06000CFC RID: 3324 RVA: 0x0003833A File Offset: 0x0003653A
	private void OnDisable()
	{
		ZoneManagement.OnZoneChange -= this.OnZoneChanged;
	}

	// Token: 0x06000CFD RID: 3325 RVA: 0x0003834D File Offset: 0x0003654D
	private void OnDestroy()
	{
		this._tabletSpawnInstance.Dispose();
	}

	// Token: 0x06000CFE RID: 3326 RVA: 0x0003835A File Offset: 0x0003655A
	public void ManuallySetCameraOnNeck()
	{
		if (this._tabletSpawnInstance.isSpawned)
		{
			this.cameraState = LckBodyCameraSpawner.CameraState.CameraOnNeck;
		}
	}

	// Token: 0x06000CFF RID: 3327 RVA: 0x00038370 File Offset: 0x00036570
	private void OnZoneChanged(ZoneData[] zones)
	{
		if (!this._tabletSpawnInstance.isSpawned || this._tabletSpawnInstance.directGrabbable.isGrabbed)
		{
			return;
		}
		this._shouldMoveCameraToNeck = true;
	}

	// Token: 0x06000D00 RID: 3328 RVA: 0x00038399 File Offset: 0x00036599
	private void OnCameraModelReleased()
	{
		this._cameraModelGrabbable.onReleased -= this.OnCameraModelReleased;
		this.ResetCameraModel();
	}

	// Token: 0x06000D01 RID: 3329 RVA: 0x0009EFFC File Offset: 0x0009D1FC
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

	// Token: 0x06000D02 RID: 3330 RVA: 0x0009F100 File Offset: 0x0009D300
	private bool ShouldSpawnCamera(Transform gorillaGrabberTransform)
	{
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		Vector3 a = worldToLocalMatrix.MultiplyPoint(this._cameraModelOriginTransform.position);
		Vector3 b = worldToLocalMatrix.MultiplyPoint(gorillaGrabberTransform.position);
		return Vector3.SqrMagnitude(a - b) >= this._activateDistance * this._activateDistance;
	}

	// Token: 0x06000D03 RID: 3331 RVA: 0x0009F158 File Offset: 0x0009D358
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

	// Token: 0x06000D04 RID: 3332 RVA: 0x000383B8 File Offset: 0x000365B8
	private void InitCameraStrap()
	{
		this._cameraStrapRenderer.positionCount = this._cameraStrapPoints.Length;
		this._cameraStrapPositions = new Vector3[this._cameraStrapPoints.Length];
	}

	// Token: 0x06000D05 RID: 3333 RVA: 0x0009F1B4 File Offset: 0x0009D3B4
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

	// Token: 0x06000D06 RID: 3334 RVA: 0x000383E0 File Offset: 0x000365E0
	private void ResetCameraModel()
	{
		this._cameraModelTransform.localPosition = Vector3.zero;
		this._cameraModelTransform.localRotation = Quaternion.identity;
	}

	// Token: 0x06000D07 RID: 3335 RVA: 0x00038402 File Offset: 0x00036602
	private VRRig GetLocalRig()
	{
		if (this._localRig == null)
		{
			this._localRig = VRRigCache.Instance.localRig.Rig;
		}
		return this._localRig;
	}

	// Token: 0x06000D08 RID: 3336 RVA: 0x0009F268 File Offset: 0x0009D468
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

	// Token: 0x06000D09 RID: 3337 RVA: 0x0009F2B4 File Offset: 0x0009D4B4
	private bool IsSlingshotActiveInHierarchy()
	{
		VRRig localRig = this.GetLocalRig();
		return !(localRig == null) && !(localRig.projectileWeapon == null) && localRig.projectileWeapon.gameObject.activeInHierarchy;
	}

	// Token: 0x04001043 RID: 4163
	[SerializeField]
	private GameObject _cameraSpawnPrefab;

	// Token: 0x04001044 RID: 4164
	[SerializeField]
	private Transform _cameraSpawnParentTransform;

	// Token: 0x04001045 RID: 4165
	[SerializeField]
	private Transform _cameraModelOriginTransform;

	// Token: 0x04001046 RID: 4166
	[SerializeField]
	private Transform _cameraModelTransform;

	// Token: 0x04001047 RID: 4167
	[SerializeField]
	private LckDirectGrabbable _cameraModelGrabbable;

	// Token: 0x04001048 RID: 4168
	[SerializeField]
	private Transform _cameraPositionDefault;

	// Token: 0x04001049 RID: 4169
	[SerializeField]
	private Transform _cameraPositionSlingshot;

	// Token: 0x0400104A RID: 4170
	[SerializeField]
	private float _activateDistance = 0.25f;

	// Token: 0x0400104B RID: 4171
	[SerializeField]
	private float _snapToNeckDistance = 15f;

	// Token: 0x0400104C RID: 4172
	[SerializeField]
	private LineRenderer _cameraStrapRenderer;

	// Token: 0x0400104D RID: 4173
	[SerializeField]
	private Transform[] _cameraStrapPoints;

	// Token: 0x0400104E RID: 4174
	[SerializeField]
	private Color _normalColor = Color.red;

	// Token: 0x0400104F RID: 4175
	[SerializeField]
	private Color _ghostColor = Color.gray;

	// Token: 0x04001050 RID: 4176
	[SerializeField]
	private GtDummyTablet _dummyTablet;

	// Token: 0x04001051 RID: 4177
	private Transform _followTransform;

	// Token: 0x04001052 RID: 4178
	private Vector3[] _cameraStrapPositions;

	// Token: 0x04001053 RID: 4179
	private TabletSpawnInstance _tabletSpawnInstance;

	// Token: 0x04001054 RID: 4180
	private VRRig _localRig;

	// Token: 0x04001055 RID: 4181
	private bool _shouldMoveCameraToNeck;

	// Token: 0x04001057 RID: 4183
	private LckBodyCameraSpawner.CameraState _cameraState;

	// Token: 0x04001058 RID: 4184
	private LckBodyCameraSpawner.CameraPosition _cameraPosition;

	// Token: 0x02000235 RID: 565
	public enum CameraState
	{
		// Token: 0x0400105A RID: 4186
		CameraDisabled,
		// Token: 0x0400105B RID: 4187
		CameraOnNeck,
		// Token: 0x0400105C RID: 4188
		CameraSpawned
	}

	// Token: 0x02000236 RID: 566
	public enum CameraPosition
	{
		// Token: 0x0400105E RID: 4190
		CameraDefault,
		// Token: 0x0400105F RID: 4191
		CameraSlingshot,
		// Token: 0x04001060 RID: 4192
		NotVisible
	}

	// Token: 0x02000237 RID: 567
	// (Invoke) Token: 0x06000D0C RID: 3340
	public delegate void CameraStateDelegate(LckBodyCameraSpawner.CameraState state);
}
