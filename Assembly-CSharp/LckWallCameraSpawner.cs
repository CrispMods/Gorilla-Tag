using System;
using System.Collections;
using GorillaLocomotion;
using Liv.Lck.GorillaTag;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

// Token: 0x0200024E RID: 590
public class LckWallCameraSpawner : MonoBehaviour
{
	// Token: 0x06000D9F RID: 3487 RVA: 0x000A2440 File Offset: 0x000A0640
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
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._lckBodySpawnerPrefab, transform.parent);
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

	// Token: 0x06000DA0 RID: 3488 RVA: 0x00039ACB File Offset: 0x00037CCB
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

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x00039AEE File Offset: 0x00037CEE
	// (set) Token: 0x06000DA2 RID: 3490 RVA: 0x000A2528 File Offset: 0x000A0728
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

	// Token: 0x06000DA3 RID: 3491 RVA: 0x00039AF6 File Offset: 0x00037CF6
	private void Awake()
	{
		this.InitCameraStrap();
	}

	// Token: 0x06000DA4 RID: 3492 RVA: 0x00039AFE File Offset: 0x00037CFE
	private void OnEnable()
	{
		this._cameraHandleGrabbable.onGrabbed += this.OnGrabbed;
		this._cameraHandleGrabbable.onReleased += this.OnReleased;
		this.wallSpawnerState = LckWallCameraSpawner.WallSpawnerState.CameraOnHook;
	}

	// Token: 0x06000DA5 RID: 3493 RVA: 0x00039B35 File Offset: 0x00037D35
	private void Start()
	{
		this.CreatePrewarmCamera();
	}

	// Token: 0x06000DA6 RID: 3494 RVA: 0x000A2578 File Offset: 0x000A0778
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

	// Token: 0x06000DA7 RID: 3495 RVA: 0x00039B3D File Offset: 0x00037D3D
	private void OnDisable()
	{
		this._cameraHandleGrabbable.onGrabbed -= this.OnGrabbed;
		this._cameraHandleGrabbable.onReleased -= this.OnReleased;
	}

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x00039B6D File Offset: 0x00037D6D
	// (set) Token: 0x06000DA9 RID: 3497 RVA: 0x00039B7F File Offset: 0x00037D7F
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

	// Token: 0x06000DAA RID: 3498 RVA: 0x00039BA3 File Offset: 0x00037DA3
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

	// Token: 0x06000DAB RID: 3499 RVA: 0x00039BE0 File Offset: 0x00037DE0
	private void InitCameraStrap()
	{
		this._cameraStrapRenderer.positionCount = this._cameraStrapPoints.Length;
		this._cameraStrapPositions = new Vector3[this._cameraStrapPoints.Length];
	}

	// Token: 0x06000DAC RID: 3500 RVA: 0x000A26B0 File Offset: 0x000A08B0
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

	// Token: 0x06000DAD RID: 3501 RVA: 0x00039C08 File Offset: 0x00037E08
	private void ResetCameraModel()
	{
		this._cameraModelTransform.localPosition = Vector3.zero;
		this._cameraModelTransform.localRotation = Quaternion.identity;
	}

	// Token: 0x06000DAE RID: 3502 RVA: 0x000A2754 File Offset: 0x000A0954
	private bool ShouldSpawnCamera(Transform gorillaGrabberTransform)
	{
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		Vector3 a = worldToLocalMatrix.MultiplyPoint(this._cameraModelOriginTransform.position);
		Vector3 b = worldToLocalMatrix.MultiplyPoint(gorillaGrabberTransform.position);
		return Vector3.SqrMagnitude(a - b) >= this._activateDistance * this._activateDistance;
	}

	// Token: 0x06000DAF RID: 3503 RVA: 0x00039C2A File Offset: 0x00037E2A
	private void OnGrabbed()
	{
		this.wallSpawnerState = LckWallCameraSpawner.WallSpawnerState.CameraDragging;
	}

	// Token: 0x06000DB0 RID: 3504 RVA: 0x00039C33 File Offset: 0x00037E33
	private void OnReleased()
	{
		this.wallSpawnerState = LckWallCameraSpawner.WallSpawnerState.CameraOnHook;
	}

	// Token: 0x06000DB1 RID: 3505 RVA: 0x000A27AC File Offset: 0x000A09AC
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

	// Token: 0x06000DB2 RID: 3506 RVA: 0x00039C3C File Offset: 0x00037E3C
	private IEnumerator DestroyPrewarmCameraDelayed()
	{
		yield return new WaitForSeconds(1f);
		this.DestroyPrewarmCamera();
		yield break;
	}

	// Token: 0x06000DB3 RID: 3507 RVA: 0x00039C4B File Offset: 0x00037E4B
	private void DestroyPrewarmCamera()
	{
		if (LckWallCameraSpawner._prewarmCamera == null)
		{
			return;
		}
		RenderTexture targetTexture = LckWallCameraSpawner._prewarmCamera.targetTexture;
		LckWallCameraSpawner._prewarmCamera.targetTexture = null;
		targetTexture.Release();
		UnityEngine.Object.Destroy(LckWallCameraSpawner._prewarmCamera.gameObject);
		LckWallCameraSpawner._prewarmCamera = null;
	}

	// Token: 0x040010D3 RID: 4307
	[SerializeField]
	private GameObject _lckBodySpawnerPrefab;

	// Token: 0x040010D4 RID: 4308
	[SerializeField]
	private LckDirectGrabbable _cameraHandleGrabbable;

	// Token: 0x040010D5 RID: 4309
	[SerializeField]
	private Transform _cameraModelOriginTransform;

	// Token: 0x040010D6 RID: 4310
	[SerializeField]
	private Transform _cameraModelTransform;

	// Token: 0x040010D7 RID: 4311
	[SerializeField]
	private LineRenderer _cameraStrapRenderer;

	// Token: 0x040010D8 RID: 4312
	[SerializeField]
	private float _activateDistance = 0.25f;

	// Token: 0x040010D9 RID: 4313
	[SerializeField]
	private Transform[] _cameraStrapPoints;

	// Token: 0x040010DA RID: 4314
	private Vector3[] _cameraStrapPositions;

	// Token: 0x040010DB RID: 4315
	[SerializeField]
	private Color _normalColor = Color.red;

	// Token: 0x040010DC RID: 4316
	private static LckBodyCameraSpawner _bodySpawner;

	// Token: 0x040010DD RID: 4317
	private static Camera _prewarmCamera;

	// Token: 0x040010DE RID: 4318
	private LckWallCameraSpawner.WallSpawnerState _wallSpawnerState;

	// Token: 0x0200024F RID: 591
	public enum WallSpawnerState
	{
		// Token: 0x040010E0 RID: 4320
		CameraOnHook,
		// Token: 0x040010E1 RID: 4321
		CameraDragging,
		// Token: 0x040010E2 RID: 4322
		CameraOffHook
	}
}
