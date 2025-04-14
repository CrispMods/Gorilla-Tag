using System;
using UnityEngine;

// Token: 0x02000233 RID: 563
public class TabletSpawnInstance : IDisposable
{
	// Token: 0x14000022 RID: 34
	// (add) Token: 0x06000CD8 RID: 3288 RVA: 0x00043770 File Offset: 0x00041970
	// (remove) Token: 0x06000CD9 RID: 3289 RVA: 0x000437A8 File Offset: 0x000419A8
	public event Action onGrabbed;

	// Token: 0x14000023 RID: 35
	// (add) Token: 0x06000CDA RID: 3290 RVA: 0x000437E0 File Offset: 0x000419E0
	// (remove) Token: 0x06000CDB RID: 3291 RVA: 0x00043818 File Offset: 0x00041A18
	public event Action onReleased;

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x06000CDC RID: 3292 RVA: 0x0004384D File Offset: 0x00041A4D
	public LckDirectGrabbable directGrabbable
	{
		get
		{
			return this._lckSocialCameraManager.lckDirectGrabbable;
		}
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x0004385A File Offset: 0x00041A5A
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

	// Token: 0x06000CDE RID: 3294 RVA: 0x0004388D File Offset: 0x00041A8D
	public bool ResetParent()
	{
		if (this._cameraSpawnInstanceTransform == null)
		{
			return false;
		}
		this._cameraSpawnInstanceTransform.SetParent(this._cameraSpawnParentTransform);
		return true;
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x000438B1 File Offset: 0x00041AB1
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
	// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x000438D0 File Offset: 0x00041AD0
	// (set) Token: 0x06000CE1 RID: 3297 RVA: 0x000438D8 File Offset: 0x00041AD8
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
	// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x00043900 File Offset: 0x00041B00
	// (set) Token: 0x06000CE3 RID: 3299 RVA: 0x00043908 File Offset: 0x00041B08
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
	// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x00043930 File Offset: 0x00041B30
	public bool isSpawned
	{
		get
		{
			return this._cameraGameObjectInstance != null;
		}
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x0004393E File Offset: 0x00041B3E
	public TabletSpawnInstance(GameObject cameraSpawnPrefab, Transform cameraSpawnParentTransform)
	{
		this._cameraSpawnPrefab = cameraSpawnPrefab;
		this._cameraSpawnParentTransform = cameraSpawnParentTransform;
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x00043954 File Offset: 0x00041B54
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
	// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x000439F2 File Offset: 0x00041BF2
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
	// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x00043A13 File Offset: 0x00041C13
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

	// Token: 0x06000CE9 RID: 3305 RVA: 0x00043A34 File Offset: 0x00041C34
	public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
	{
		if (this._cameraSpawnInstanceTransform == null)
		{
			return;
		}
		this._cameraSpawnInstanceTransform.SetPositionAndRotation(position, rotation);
	}

	// Token: 0x06000CEA RID: 3306 RVA: 0x00043A52 File Offset: 0x00041C52
	public void SetLocalScale(Vector3 scale)
	{
		if (this._cameraSpawnInstanceTransform == null)
		{
			return;
		}
		this._cameraSpawnInstanceTransform.localScale = scale;
	}

	// Token: 0x06000CEB RID: 3307 RVA: 0x00043A6F File Offset: 0x00041C6F
	public void Dispose()
	{
		if (this._cameraGameObjectInstance != null)
		{
			Object.Destroy(this._cameraGameObjectInstance);
			this._cameraGameObjectInstance = null;
		}
	}

	// Token: 0x0400103B RID: 4155
	private GameObject _cameraGameObjectInstance;

	// Token: 0x0400103C RID: 4156
	private GameObject _cameraSpawnPrefab;

	// Token: 0x0400103D RID: 4157
	private GameEvents _GtCamera;

	// Token: 0x0400103E RID: 4158
	private Transform _cameraSpawnParentTransform;

	// Token: 0x0400103F RID: 4159
	private Transform _cameraSpawnInstanceTransform;

	// Token: 0x04001040 RID: 4160
	private LckSocialCameraManager _lckSocialCameraManager;

	// Token: 0x04001041 RID: 4161
	private bool _cameraActive;

	// Token: 0x04001042 RID: 4162
	private bool _uiVisible;
}
