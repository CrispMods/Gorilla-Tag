using System;
using UnityEngine;

// Token: 0x0200023E RID: 574
public class TabletSpawnInstance : IDisposable
{
	// Token: 0x14000023 RID: 35
	// (add) Token: 0x06000D21 RID: 3361 RVA: 0x000A1200 File Offset: 0x0009F400
	// (remove) Token: 0x06000D22 RID: 3362 RVA: 0x000A1238 File Offset: 0x0009F438
	public event Action onGrabbed;

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x06000D23 RID: 3363 RVA: 0x000A1270 File Offset: 0x0009F470
	// (remove) Token: 0x06000D24 RID: 3364 RVA: 0x000A12A8 File Offset: 0x0009F4A8
	public event Action onReleased;

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06000D25 RID: 3365 RVA: 0x0003939E File Offset: 0x0003759E
	public LckDirectGrabbable directGrabbable
	{
		get
		{
			return this._lckSocialCameraManager.lckDirectGrabbable;
		}
	}

	// Token: 0x06000D26 RID: 3366 RVA: 0x000393AB File Offset: 0x000375AB
	public bool ResetLocalPose()
	{
		if (this._cameraSpawnInstanceTransform == null)
		{
			return false;
		}
		this._cameraSpawnInstanceTransform.localPosition = Vector3.zero;
		this._cameraSpawnInstanceTransform.localRotation = Quaternion.identity;
		return true;
	}

	// Token: 0x06000D27 RID: 3367 RVA: 0x000393DE File Offset: 0x000375DE
	public bool ResetParent()
	{
		if (this._cameraSpawnInstanceTransform == null)
		{
			return false;
		}
		this._cameraSpawnInstanceTransform.SetParent(this._cameraSpawnParentTransform);
		return true;
	}

	// Token: 0x06000D28 RID: 3368 RVA: 0x00039402 File Offset: 0x00037602
	public bool SetParent(Transform transform)
	{
		if (this._cameraSpawnInstanceTransform == null)
		{
			return false;
		}
		this._cameraSpawnInstanceTransform.SetParent(transform);
		return true;
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06000D29 RID: 3369 RVA: 0x00039421 File Offset: 0x00037621
	// (set) Token: 0x06000D2A RID: 3370 RVA: 0x00039429 File Offset: 0x00037629
	public bool cameraActive
	{
		get
		{
			return this._cameraActive;
		}
		set
		{
			this._cameraActive = value;
			if (this._lckSocialCameraManager != null)
			{
				this._lckSocialCameraManager.cameraActive = this._cameraActive;
			}
		}
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000D2B RID: 3371 RVA: 0x00039451 File Offset: 0x00037651
	// (set) Token: 0x06000D2C RID: 3372 RVA: 0x00039459 File Offset: 0x00037659
	public bool uiVisible
	{
		get
		{
			return this._uiVisible;
		}
		set
		{
			this._uiVisible = value;
			if (this._lckSocialCameraManager != null)
			{
				this._lckSocialCameraManager.uiVisible = this._uiVisible;
			}
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000D2D RID: 3373 RVA: 0x00039481 File Offset: 0x00037681
	public bool isSpawned
	{
		get
		{
			return this._cameraGameObjectInstance != null;
		}
	}

	// Token: 0x06000D2E RID: 3374 RVA: 0x0003948F File Offset: 0x0003768F
	public TabletSpawnInstance(GameObject cameraSpawnPrefab, Transform cameraSpawnParentTransform)
	{
		this._cameraSpawnPrefab = cameraSpawnPrefab;
		this._cameraSpawnParentTransform = cameraSpawnParentTransform;
	}

	// Token: 0x06000D2F RID: 3375 RVA: 0x000A12E0 File Offset: 0x0009F4E0
	public void SpawnCamera()
	{
		if (!this.isSpawned)
		{
			this._cameraGameObjectInstance = UnityEngine.Object.Instantiate<GameObject>(this._cameraSpawnPrefab, this._cameraSpawnParentTransform);
			this._lckSocialCameraManager = this._cameraGameObjectInstance.GetComponent<LckSocialCameraManager>();
			this._lckSocialCameraManager.lckDirectGrabbable.onGrabbed += delegate()
			{
				Action action = this.onGrabbed;
				if (action == null)
				{
					return;
				}
				action();
			};
			this._lckSocialCameraManager.lckDirectGrabbable.onReleased += delegate()
			{
				Action action = this.onReleased;
				if (action == null)
				{
					return;
				}
				action();
			};
			this._cameraSpawnInstanceTransform = this._cameraGameObjectInstance.transform;
		}
		this.uiVisible = this.uiVisible;
		this.cameraActive = this.cameraActive;
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000D30 RID: 3376 RVA: 0x000394A5 File Offset: 0x000376A5
	public Vector3 position
	{
		get
		{
			if (this._cameraSpawnInstanceTransform == null)
			{
				return Vector3.zero;
			}
			return this._cameraSpawnInstanceTransform.position;
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000D31 RID: 3377 RVA: 0x000394C6 File Offset: 0x000376C6
	public Quaternion rotation
	{
		get
		{
			if (this._cameraSpawnInstanceTransform == null)
			{
				return Quaternion.identity;
			}
			return this._cameraSpawnInstanceTransform.rotation;
		}
	}

	// Token: 0x06000D32 RID: 3378 RVA: 0x000394E7 File Offset: 0x000376E7
	public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
	{
		if (this._cameraSpawnInstanceTransform == null)
		{
			return;
		}
		this._cameraSpawnInstanceTransform.SetPositionAndRotation(position, rotation);
	}

	// Token: 0x06000D33 RID: 3379 RVA: 0x00039505 File Offset: 0x00037705
	public void SetLocalScale(Vector3 scale)
	{
		if (this._cameraSpawnInstanceTransform == null)
		{
			return;
		}
		this._cameraSpawnInstanceTransform.localScale = scale;
	}

	// Token: 0x06000D34 RID: 3380 RVA: 0x00039522 File Offset: 0x00037722
	public void Dispose()
	{
		if (this._cameraGameObjectInstance != null)
		{
			UnityEngine.Object.Destroy(this._cameraGameObjectInstance);
			this._cameraGameObjectInstance = null;
		}
	}

	// Token: 0x04001080 RID: 4224
	private GameObject _cameraGameObjectInstance;

	// Token: 0x04001081 RID: 4225
	private GameObject _cameraSpawnPrefab;

	// Token: 0x04001082 RID: 4226
	private GameEvents _GtCamera;

	// Token: 0x04001083 RID: 4227
	private Transform _cameraSpawnParentTransform;

	// Token: 0x04001084 RID: 4228
	private Transform _cameraSpawnInstanceTransform;

	// Token: 0x04001085 RID: 4229
	private LckSocialCameraManager _lckSocialCameraManager;

	// Token: 0x04001086 RID: 4230
	private bool _cameraActive;

	// Token: 0x04001087 RID: 4231
	private bool _uiVisible;
}
