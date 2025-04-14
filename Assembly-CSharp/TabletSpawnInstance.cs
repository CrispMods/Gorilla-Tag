using System;
using UnityEngine;

// Token: 0x02000233 RID: 563
public class TabletSpawnInstance : IDisposable
{
	// Token: 0x14000022 RID: 34
	// (add) Token: 0x06000CD6 RID: 3286 RVA: 0x0004342C File Offset: 0x0004162C
	// (remove) Token: 0x06000CD7 RID: 3287 RVA: 0x00043464 File Offset: 0x00041664
	public event Action onGrabbed;

	// Token: 0x14000023 RID: 35
	// (add) Token: 0x06000CD8 RID: 3288 RVA: 0x0004349C File Offset: 0x0004169C
	// (remove) Token: 0x06000CD9 RID: 3289 RVA: 0x000434D4 File Offset: 0x000416D4
	public event Action onReleased;

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x06000CDA RID: 3290 RVA: 0x00043509 File Offset: 0x00041709
	public LckDirectGrabbable directGrabbable
	{
		get
		{
			return this._lckSocialCameraManager.lckDirectGrabbable;
		}
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x00043516 File Offset: 0x00041716
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

	// Token: 0x06000CDC RID: 3292 RVA: 0x00043549 File Offset: 0x00041749
	public bool ResetParent()
	{
		if (this._cameraSpawnInstanceTransform == null)
		{
			return false;
		}
		this._cameraSpawnInstanceTransform.SetParent(this._cameraSpawnParentTransform);
		return true;
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x0004356D File Offset: 0x0004176D
	public bool SetParent(Transform transform)
	{
		if (this._cameraSpawnInstanceTransform == null)
		{
			return false;
		}
		this._cameraSpawnInstanceTransform.SetParent(transform);
		return true;
	}

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x06000CDE RID: 3294 RVA: 0x0004358C File Offset: 0x0004178C
	// (set) Token: 0x06000CDF RID: 3295 RVA: 0x00043594 File Offset: 0x00041794
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

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x000435BC File Offset: 0x000417BC
	// (set) Token: 0x06000CE1 RID: 3297 RVA: 0x000435C4 File Offset: 0x000417C4
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

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x000435EC File Offset: 0x000417EC
	public bool isSpawned
	{
		get
		{
			return this._cameraGameObjectInstance != null;
		}
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x000435FA File Offset: 0x000417FA
	public TabletSpawnInstance(GameObject cameraSpawnPrefab, Transform cameraSpawnParentTransform)
	{
		this._cameraSpawnPrefab = cameraSpawnPrefab;
		this._cameraSpawnParentTransform = cameraSpawnParentTransform;
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x00043610 File Offset: 0x00041810
	public void SpawnCamera()
	{
		if (!this.isSpawned)
		{
			this._cameraGameObjectInstance = Object.Instantiate<GameObject>(this._cameraSpawnPrefab, this._cameraSpawnParentTransform);
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

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x000436AE File Offset: 0x000418AE
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

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x000436CF File Offset: 0x000418CF
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

	// Token: 0x06000CE7 RID: 3303 RVA: 0x000436F0 File Offset: 0x000418F0
	public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
	{
		if (this._cameraSpawnInstanceTransform == null)
		{
			return;
		}
		this._cameraSpawnInstanceTransform.SetPositionAndRotation(position, rotation);
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x0004370E File Offset: 0x0004190E
	public void SetLocalScale(Vector3 scale)
	{
		if (this._cameraSpawnInstanceTransform == null)
		{
			return;
		}
		this._cameraSpawnInstanceTransform.localScale = scale;
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x0004372B File Offset: 0x0004192B
	public void Dispose()
	{
		if (this._cameraGameObjectInstance != null)
		{
			Object.Destroy(this._cameraGameObjectInstance);
			this._cameraGameObjectInstance = null;
		}
	}

	// Token: 0x0400103A RID: 4154
	private GameObject _cameraGameObjectInstance;

	// Token: 0x0400103B RID: 4155
	private GameObject _cameraSpawnPrefab;

	// Token: 0x0400103C RID: 4156
	private GameEvents _GtCamera;

	// Token: 0x0400103D RID: 4157
	private Transform _cameraSpawnParentTransform;

	// Token: 0x0400103E RID: 4158
	private Transform _cameraSpawnInstanceTransform;

	// Token: 0x0400103F RID: 4159
	private LckSocialCameraManager _lckSocialCameraManager;

	// Token: 0x04001040 RID: 4160
	private bool _cameraActive;

	// Token: 0x04001041 RID: 4161
	private bool _uiVisible;
}
