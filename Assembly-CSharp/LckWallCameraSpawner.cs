using System;
using System.Collections;
using GorillaLocomotion;
using Liv.Lck.GorillaTag;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

// Token: 0x02000243 RID: 579
public class LckWallCameraSpawner : MonoBehaviour
{
	// Token: 0x06000D54 RID: 3412 RVA: 0x00044D90 File Offset: 0x00042F90
	private LckBodyCameraSpawner GetOrCreateBodyCameraSpawner()
	{
		if (LckWallCameraSpawner._bodySpawner != null)
		{
			return LckWallCameraSpawner._bodySpawner;
		}
		GTPlayer instance = GTPlayer.Instance;
		if (instance == null)
		{
			Debug.LogError("Unable to find Player!");
			return null;
		}
		LckWallCameraSpawner.AddGTag(Camera.main.gameObject, GtTagType.HMD);
		LckWallCameraSpawner.AddGTag(instance.gameObject, GtTagType.Player);
		Transform transform = instance.bodyCollider.transform;
		GameObject gameObject = Object.Instantiate<GameObject>(this._lckBodySpawnerPrefab, transform.parent);
		Transform transform2 = gameObject.transform;
		transform2.localPosition = Vector3.zero;
		transform2.localRotation = Quaternion.identity;
		transform2.localScale = Vector3.one;
		LckWallCameraSpawner._bodySpawner = gameObject.GetComponent<LckBodyCameraSpawner>();
		LckWallCameraSpawner._bodySpawner.SetFollowTransform(transform);
		GorillaTagger instance2 = GorillaTagger.Instance;
		if (instance2 != null)
		{
			LckWallCameraSpawner.AddGTag(instance2.leftHandTriggerCollider, GtTagType.LeftHand);
			LckWallCameraSpawner.AddGTag(instance2.rightHandTriggerCollider, GtTagType.RightHand);
		}
		else
		{
			Debug.LogError("Unable to find GorillaTagger!");
		}
		return LckWallCameraSpawner._bodySpawner;
	}

	// Token: 0x06000D55 RID: 3413 RVA: 0x00044E77 File Offset: 0x00043077
	private static void AddGTag(GameObject go, GtTagType gtTagType)
	{
		if (go.GetComponent<GtTag>())
		{
			return;
		}
		GtTag gtTag = go.AddComponent<GtTag>();
		gtTag.gtTagType = gtTagType;
		gtTag.enabled = true;
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06000D56 RID: 3414 RVA: 0x00044E9A File Offset: 0x0004309A
	// (set) Token: 0x06000D57 RID: 3415 RVA: 0x00044EA4 File Offset: 0x000430A4
	public LckWallCameraSpawner.WallSpawnerState wallSpawnerState
	{
		get
		{
			return this._wallSpawnerState;
		}
		set
		{
			switch (value)
			{
			case LckWallCameraSpawner.WallSpawnerState.CameraOnHook:
				this.ResetCameraModel();
				this.UpdateCameraStrap();
				this.cameraVisible = true;
				break;
			case LckWallCameraSpawner.WallSpawnerState.CameraOffHook:
				this.ResetCameraModel();
				this.UpdateCameraStrap();
				this.cameraVisible = true;
				break;
			}
			this._wallSpawnerState = value;
		}
	}

	// Token: 0x06000D58 RID: 3416 RVA: 0x00044EF4 File Offset: 0x000430F4
	private void Awake()
	{
		this.InitCameraStrap();
	}

	// Token: 0x06000D59 RID: 3417 RVA: 0x00044EFC File Offset: 0x000430FC
	private void OnEnable()
	{
		this._cameraHandleGrabbable.onGrabbed += this.OnGrabbed;
		this._cameraHandleGrabbable.onReleased += this.OnReleased;
		this.wallSpawnerState = LckWallCameraSpawner.WallSpawnerState.CameraOnHook;
	}

	// Token: 0x06000D5A RID: 3418 RVA: 0x00044F33 File Offset: 0x00043133
	private void Start()
	{
		this.CreatePrewarmCamera();
	}

	// Token: 0x06000D5B RID: 3419 RVA: 0x00044F3C File Offset: 0x0004313C
	private void Update()
	{
		LckWallCameraSpawner.WallSpawnerState wallSpawnerState = this._wallSpawnerState;
		if (wallSpawnerState != LckWallCameraSpawner.WallSpawnerState.CameraOnHook)
		{
			if (wallSpawnerState != LckWallCameraSpawner.WallSpawnerState.CameraDragging)
			{
				return;
			}
			this.UpdateCameraStrap();
			if (this.ShouldSpawnCamera(this._cameraHandleGrabbable.grabber.transform))
			{
				this.SpawnCamera(this._cameraHandleGrabbable.grabber);
			}
		}
		else
		{
			if (this.GetOrCreateBodyCameraSpawner() == null)
			{
				Debug.LogError("Lck, Unable to find LckBodyCameraSpawner");
				base.gameObject.SetActive(false);
				return;
			}
			if (LckWallCameraSpawner._bodySpawner.cameraState == LckBodyCameraSpawner.CameraState.CameraSpawned && LckWallCameraSpawner._bodySpawner.tabletSpawnInstance.isSpawned && LckWallCameraSpawner._bodySpawner.tabletSpawnInstance.directGrabbable.isGrabbed)
			{
				LckDirectGrabbable directGrabbable = LckWallCameraSpawner._bodySpawner.tabletSpawnInstance.directGrabbable;
				GorillaGrabber grabber = directGrabbable.grabber;
				if (!this.ShouldSpawnCamera(grabber.transform))
				{
					directGrabbable.ForceRelease();
					LckWallCameraSpawner._bodySpawner.cameraState = LckBodyCameraSpawner.CameraState.CameraDisabled;
					this._cameraHandleGrabbable.target.SetPositionAndRotation(grabber.transform.position, grabber.transform.rotation * Quaternion.Euler(0f, 180f, 0f));
					this._cameraHandleGrabbable.ForceGrab(grabber);
					return;
				}
			}
		}
	}

	// Token: 0x06000D5C RID: 3420 RVA: 0x00045071 File Offset: 0x00043271
	private void OnDisable()
	{
		this._cameraHandleGrabbable.onGrabbed -= this.OnGrabbed;
		this._cameraHandleGrabbable.onReleased -= this.OnReleased;
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000D5D RID: 3421 RVA: 0x000450A1 File Offset: 0x000432A1
	// (set) Token: 0x06000D5E RID: 3422 RVA: 0x000450B3 File Offset: 0x000432B3
	private bool cameraVisible
	{
		get
		{
			return this._cameraModelTransform.gameObject.activeSelf;
		}
		set
		{
			this._cameraModelTransform.gameObject.SetActive(value);
			this._cameraStrapRenderer.gameObject.SetActive(value);
		}
	}

	// Token: 0x06000D5F RID: 3423 RVA: 0x000450D7 File Offset: 0x000432D7
	private void SpawnCamera(GorillaGrabber lastGorillaGrabber)
	{
		if (LckWallCameraSpawner._bodySpawner == null)
		{
			Debug.LogError("Lck, unable to spawn camera, body spawner is null!");
			return;
		}
		this.cameraVisible = false;
		this._cameraHandleGrabbable.ForceRelease();
		LckWallCameraSpawner._bodySpawner.SpawnCamera(lastGorillaGrabber, lastGorillaGrabber.transform);
	}

	// Token: 0x06000D60 RID: 3424 RVA: 0x00045114 File Offset: 0x00043314
	private void InitCameraStrap()
	{
		this._cameraStrapRenderer.positionCount = this._cameraStrapPoints.Length;
		this._cameraStrapPositions = new Vector3[this._cameraStrapPoints.Length];
	}

	// Token: 0x06000D61 RID: 3425 RVA: 0x0004513C File Offset: 0x0004333C
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
		this._cameraStrapRenderer.startColor = (this._cameraStrapRenderer.endColor = this._normalColor);
	}

	// Token: 0x06000D62 RID: 3426 RVA: 0x000451DE File Offset: 0x000433DE
	private void ResetCameraModel()
	{
		this._cameraModelTransform.localPosition = Vector3.zero;
		this._cameraModelTransform.localRotation = Quaternion.identity;
	}

	// Token: 0x06000D63 RID: 3427 RVA: 0x00045200 File Offset: 0x00043400
	private bool ShouldSpawnCamera(Transform gorillaGrabberTransform)
	{
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		Vector3 a = worldToLocalMatrix.MultiplyPoint(this._cameraModelOriginTransform.position);
		Vector3 b = worldToLocalMatrix.MultiplyPoint(gorillaGrabberTransform.position);
		return Vector3.SqrMagnitude(a - b) >= this._activateDistance * this._activateDistance;
	}

	// Token: 0x06000D64 RID: 3428 RVA: 0x00045256 File Offset: 0x00043456
	private void OnGrabbed()
	{
		this.wallSpawnerState = LckWallCameraSpawner.WallSpawnerState.CameraDragging;
	}

	// Token: 0x06000D65 RID: 3429 RVA: 0x0004525F File Offset: 0x0004345F
	private void OnReleased()
	{
		this.wallSpawnerState = LckWallCameraSpawner.WallSpawnerState.CameraOnHook;
	}

	// Token: 0x06000D66 RID: 3430 RVA: 0x00045268 File Offset: 0x00043468
	private void CreatePrewarmCamera()
	{
		if (LckWallCameraSpawner._prewarmCamera != null)
		{
			return;
		}
		GameObject gameObject = new GameObject("prewarm camera");
		gameObject.transform.SetParent(base.transform);
		LckWallCameraSpawner._prewarmCamera = gameObject.AddComponent<Camera>();
		Camera main = Camera.main;
		LckWallCameraSpawner._prewarmCamera.clearFlags = main.clearFlags;
		LckWallCameraSpawner._prewarmCamera.fieldOfView = main.fieldOfView;
		LckWallCameraSpawner._prewarmCamera.nearClipPlane = main.nearClipPlane;
		LckWallCameraSpawner._prewarmCamera.farClipPlane = main.farClipPlane;
		LckWallCameraSpawner._prewarmCamera.cullingMask = main.cullingMask;
		LckWallCameraSpawner._prewarmCamera.tag = "Untagged";
		LckWallCameraSpawner._prewarmCamera.stereoTargetEye = StereoTargetEyeMask.None;
		LckWallCameraSpawner._prewarmCamera.targetTexture = new RenderTexture(32, 32, GraphicsFormat.R8G8B8A8_UNorm, GraphicsFormat.D32_SFloat_S8_UInt);
		LckWallCameraSpawner._prewarmCamera.transform.SetPositionAndRotation(main.transform.position, main.transform.rotation);
		base.StartCoroutine(this.DestroyPrewarmCameraDelayed());
	}

	// Token: 0x06000D67 RID: 3431 RVA: 0x00045360 File Offset: 0x00043560
	private IEnumerator DestroyPrewarmCameraDelayed()
	{
		yield return new WaitForSeconds(1f);
		this.DestroyPrewarmCamera();
		yield break;
	}

	// Token: 0x06000D68 RID: 3432 RVA: 0x0004536F File Offset: 0x0004356F
	private void DestroyPrewarmCamera()
	{
		if (LckWallCameraSpawner._prewarmCamera == null)
		{
			return;
		}
		RenderTexture targetTexture = LckWallCameraSpawner._prewarmCamera.targetTexture;
		LckWallCameraSpawner._prewarmCamera.targetTexture = null;
		targetTexture.Release();
		Object.Destroy(LckWallCameraSpawner._prewarmCamera.gameObject);
		LckWallCameraSpawner._prewarmCamera = null;
	}

	// Token: 0x0400108D RID: 4237
	[SerializeField]
	private GameObject _lckBodySpawnerPrefab;

	// Token: 0x0400108E RID: 4238
	[SerializeField]
	private LckDirectGrabbable _cameraHandleGrabbable;

	// Token: 0x0400108F RID: 4239
	[SerializeField]
	private Transform _cameraModelOriginTransform;

	// Token: 0x04001090 RID: 4240
	[SerializeField]
	private Transform _cameraModelTransform;

	// Token: 0x04001091 RID: 4241
	[SerializeField]
	private LineRenderer _cameraStrapRenderer;

	// Token: 0x04001092 RID: 4242
	[SerializeField]
	private float _activateDistance = 0.25f;

	// Token: 0x04001093 RID: 4243
	[SerializeField]
	private Transform[] _cameraStrapPoints;

	// Token: 0x04001094 RID: 4244
	private Vector3[] _cameraStrapPositions;

	// Token: 0x04001095 RID: 4245
	[SerializeField]
	private Color _normalColor = Color.red;

	// Token: 0x04001096 RID: 4246
	private static LckBodyCameraSpawner _bodySpawner;

	// Token: 0x04001097 RID: 4247
	private static Camera _prewarmCamera;

	// Token: 0x04001098 RID: 4248
	private LckWallCameraSpawner.WallSpawnerState _wallSpawnerState;

	// Token: 0x02000244 RID: 580
	public enum WallSpawnerState
	{
		// Token: 0x0400109A RID: 4250
		CameraOnHook,
		// Token: 0x0400109B RID: 4251
		CameraDragging,
		// Token: 0x0400109C RID: 4252
		CameraOffHook
	}
}
