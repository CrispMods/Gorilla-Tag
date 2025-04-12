using System;
using System.Collections.Generic;
using GorillaTag;
using GorillaTagScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x020000CA RID: 202
public class SnowballThrowable : HoldableObject
{
	// Token: 0x17000067 RID: 103
	// (get) Token: 0x0600053C RID: 1340 RVA: 0x00032E0A File Offset: 0x0003100A
	internal int ProjectileHash
	{
		get
		{
			return PoolUtils.GameObjHashCode(this.randomModelSelection ? this.localModels[this.randModelIndex].projectilePrefab : this.projectilePrefab);
		}
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x0007F9D0 File Offset: 0x0007DBD0
	protected virtual void Awake()
	{
		if (this.awakeHasBeenCalled)
		{
			return;
		}
		this.awakeHasBeenCalled = true;
		this.targetRig = base.GetComponentInParent<VRRig>(true);
		this.isOfflineRig = (this.targetRig != null && this.targetRig.isOfflineVRRig);
		this.renderers = base.GetComponentsInChildren<Renderer>();
		this.matPropBlock = new MaterialPropertyBlock();
		this.randModelIndex = -1;
		foreach (BucketThrowable bucketThrowable in this.localModels)
		{
			if (bucketThrowable != null)
			{
				BucketThrowable bucketThrowable2 = bucketThrowable;
				bucketThrowable2.OnTriggerEntered = (UnityAction<bool>)Delegate.Combine(bucketThrowable2.OnTriggerEntered, new UnityAction<bool>(this.HandleOnGorillaHeadTriggerEntered));
			}
		}
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x00032E37 File Offset: 0x00031037
	public bool IsMine()
	{
		return this.targetRig != null && this.targetRig.isOfflineVRRig;
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x0007FAA4 File Offset: 0x0007DCA4
	public virtual void OnEnable()
	{
		if (this.targetRig == null)
		{
			Debug.LogError("SnowballThrowable: targetRig is null! Deactivating.");
			base.gameObject.SetActive(false);
			return;
		}
		if (!this.targetRig.isOfflineVRRig)
		{
			if (this.targetRig.netView != null && this.targetRig.netView.IsMine)
			{
				base.gameObject.SetActive(false);
				return;
			}
			Color32 throwableProjectileColor = this.targetRig.GetThrowableProjectileColor(this.isLeftHanded);
			this.ApplyColor(throwableProjectileColor);
			if (this.randomModelSelection)
			{
				foreach (BucketThrowable bucketThrowable in this.localModels)
				{
					bucketThrowable.gameObject.SetActive(false);
				}
				this.randModelIndex = this.targetRig.GetRandomThrowableModelIndex();
				this.EnableRandomModel(this.randModelIndex, true);
			}
		}
		this.AnchorToHand();
		this.OnEnableHasBeenCalled = true;
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void OnDisable()
	{
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void OnDestroy()
	{
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x0007FBB4 File Offset: 0x0007DDB4
	public void SetSnowballActiveLocal(bool enabled)
	{
		if (!this.awakeHasBeenCalled)
		{
			this.Awake();
		}
		if (!this.OnEnableHasBeenCalled)
		{
			this.OnEnable();
		}
		if (this.isLeftHanded)
		{
			this.targetRig.LeftThrowableProjectileIndex = (enabled ? this.throwableMakerIndex : -1);
		}
		else
		{
			this.targetRig.RightThrowableProjectileIndex = (enabled ? this.throwableMakerIndex : -1);
		}
		bool flag = !base.gameObject.activeSelf && enabled;
		base.gameObject.SetActive(enabled);
		if (flag && this.pickupSoundBankPlayer != null)
		{
			this.pickupSoundBankPlayer.Play();
		}
		if (this.randomModelSelection)
		{
			if (enabled)
			{
				this.EnableRandomModel(this.GetRandomModelIndex(), true);
			}
			else
			{
				this.EnableRandomModel(this.randModelIndex, false);
			}
			this.targetRig.SetRandomThrowableModelIndex(this.randModelIndex);
		}
		EquipmentInteractor.instance.UpdateHandEquipment(enabled ? this : null, this.isLeftHanded);
		if (this.randomizeColor)
		{
			Color color = enabled ? GTColor.RandomHSV(this.randomColorHSVRanges) : Color.white;
			this.targetRig.SetThrowableProjectileColor(this.isLeftHanded, color);
			this.ApplyColor(color);
		}
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x0007FCDC File Offset: 0x0007DEDC
	private int GetRandomModelIndex()
	{
		if (this.localModels.Count == 0)
		{
			return -1;
		}
		this.randModelIndex = UnityEngine.Random.Range(0, this.localModels.Count);
		if ((float)UnityEngine.Random.Range(1, 100) <= this.localModels[this.randModelIndex].weightedChance * 100f)
		{
			return this.randModelIndex;
		}
		return this.GetRandomModelIndex();
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0007FD44 File Offset: 0x0007DF44
	private void EnableRandomModel(int index, bool enable)
	{
		if (this.randModelIndex >= 0 && this.randModelIndex < this.localModels.Count)
		{
			this.localModels[this.randModelIndex].gameObject.SetActive(enable);
			if (enable && this.localModels[this.randModelIndex].destroyAfterSeconds > 0f)
			{
				this.destroyTimer = 0f;
			}
			return;
		}
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x0007FDB8 File Offset: 0x0007DFB8
	protected virtual void LateUpdateLocal()
	{
		if (this.randomModelSelection && this.randModelIndex > -1 && this.localModels[this.randModelIndex].destroyAfterSeconds > 0f)
		{
			this.destroyTimer += Time.deltaTime;
			if (this.destroyTimer > this.localModels[this.randModelIndex].destroyAfterSeconds)
			{
				if (this.localModels[this.randModelIndex].gameObject.activeSelf)
				{
					this.PerformSnowballThrowAuthority();
				}
				this.destroyTimer = -1f;
			}
		}
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected void LateUpdateReplicated()
	{
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected void LateUpdateShared()
	{
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x00032E54 File Offset: 0x00031054
	private Transform Anchor()
	{
		return base.transform.parent;
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x0007FE54 File Offset: 0x0007E054
	private void AnchorToHand()
	{
		BodyDockPositions myBodyDockPositions = this.targetRig.myBodyDockPositions;
		Transform transform = this.Anchor();
		if (this.isLeftHanded)
		{
			transform.parent = myBodyDockPositions.leftHandTransform;
		}
		else
		{
			transform.parent = myBodyDockPositions.rightHandTransform;
		}
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x00032E61 File Offset: 0x00031061
	protected void LateUpdate()
	{
		if (this.IsMine())
		{
			this.LateUpdateLocal();
		}
		else
		{
			this.LateUpdateReplicated();
		}
		this.LateUpdateShared();
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x00032E7F File Offset: 0x0003107F
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (!base.OnRelease(zoneReleased, releasingHand))
		{
			return false;
		}
		this.OnSnowballRelease();
		return true;
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x00032E94 File Offset: 0x00031094
	protected virtual void OnSnowballRelease()
	{
		this.PerformSnowballThrowAuthority();
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x0007FEAC File Offset: 0x0007E0AC
	protected virtual void PerformSnowballThrowAuthority()
	{
		if (!(this.targetRig != null) || this.targetRig.creator == null || !this.targetRig.creator.IsLocal)
		{
			return;
		}
		Vector3 b = Vector3.zero;
		Rigidbody component = GorillaTagger.Instance.GetComponent<Rigidbody>();
		if (component != null)
		{
			b = component.velocity;
		}
		Vector3 a = this.velocityEstimator.linearVelocity - b;
		float magnitude = a.magnitude;
		if (magnitude > 0.001f)
		{
			float num = Mathf.Clamp(magnitude * this.linSpeedMultiplier, 0f, this.maxLinSpeed);
			a *= num / magnitude;
		}
		Vector3 velocity = a + b;
		Color32 throwableProjectileColor = this.targetRig.GetThrowableProjectileColor(this.isLeftHanded);
		Transform transform = base.transform;
		Vector3 position = transform.position;
		float x = transform.lossyScale.x;
		SlingshotProjectile slingshotProjectile = this.LaunchSnowballLocal(position, velocity, x);
		this.SetSnowballActiveLocal(false);
		if (this.randModelIndex > -1 && this.randModelIndex < this.localModels.Count && this.localModels[this.randModelIndex].destroyAfterRelease)
		{
			slingshotProjectile.DestroyAfterRelease();
		}
		if (NetworkSystem.Instance.InRoom)
		{
			int projectileCount = ProjectileTracker.IncrementLocalPlayerProjectileCount();
			slingshotProjectile.Launch(base.transform.position, velocity, NetworkSystem.Instance.LocalPlayer, false, false, projectileCount, x, this.randomizeColor, throwableProjectileColor);
			RoomSystem.SendLaunchProjectile(position, velocity, this.isLeftHanded ? RoomSystem.ProjectileSource.LeftHand : RoomSystem.ProjectileSource.RightHand, projectileCount, this.randomizeColor, throwableProjectileColor.r, throwableProjectileColor.g, throwableProjectileColor.b, throwableProjectileColor.a);
			return;
		}
		slingshotProjectile.Launch(position, velocity, NetworkSystem.Instance.LocalPlayer, false, false, 0, x, this.randomizeColor, throwableProjectileColor);
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x00080080 File Offset: 0x0007E280
	protected virtual SlingshotProjectile LaunchSnowballLocal(Vector3 location, Vector3 velocity, float scale)
	{
		SlingshotProjectile component = ObjectPools.instance.Instantiate(this.randomModelSelection ? this.localModels[this.randModelIndex].projectilePrefab : this.projectilePrefab).GetComponent<SlingshotProjectile>();
		component.Launch(location, velocity, NetworkSystem.Instance.LocalPlayer, false, false, 0, scale, false, Color.white);
		if (string.IsNullOrEmpty(this.throwEventName))
		{
			PlayerGameEvents.LaunchedProjectile(this.projectilePrefab.name);
		}
		else
		{
			PlayerGameEvents.LaunchedProjectile(this.throwEventName);
		}
		component.OnImpact += this.OnProjectileImpact;
		return component;
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x0008011C File Offset: 0x0007E31C
	protected virtual void OnProjectileImpact(SlingshotProjectile projectile, Vector3 impactPos, NetPlayer hitPlayer)
	{
		if (hitPlayer != null)
		{
			ScienceExperimentManager instance = ScienceExperimentManager.instance;
			if (instance != null && this.projectilePrefab != null && this.projectilePrefab == instance.waterBalloonPrefab)
			{
				instance.OnWaterBalloonHitPlayer(hitPlayer);
			}
		}
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x00080168 File Offset: 0x0007E368
	private void ApplyColor(Color newColor)
	{
		foreach (Renderer renderer in this.renderers)
		{
			if (renderer)
			{
				foreach (Material material in renderer.materials)
				{
					if (!(material == null))
					{
						if (material.HasProperty("_BaseColor"))
						{
							material.SetColor("_BaseColor", newColor);
						}
						if (material.HasProperty("_Color"))
						{
							material.SetColor("_Color", newColor);
						}
					}
				}
			}
		}
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void DropItemCleanup()
	{
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x00032E9C File Offset: 0x0003109C
	private void HandleOnGorillaHeadTriggerEntered(bool enable)
	{
		this.SetSnowballActiveLocal(enable);
	}

	// Token: 0x0400060D RID: 1549
	[GorillaSoundLookup]
	public List<int> matDataIndexes = new List<int>
	{
		32
	};

	// Token: 0x0400060E RID: 1550
	public GameObject projectilePrefab;

	// Token: 0x0400060F RID: 1551
	[FormerlySerializedAs("shouldColorize")]
	public bool randomizeColor;

	// Token: 0x04000610 RID: 1552
	public GTColor.HSVRanges randomColorHSVRanges = new GTColor.HSVRanges(0f, 1f, 0.7f, 1f, 1f, 1f);

	// Token: 0x04000611 RID: 1553
	public GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04000612 RID: 1554
	public SoundBankPlayer pickupSoundBankPlayer;

	// Token: 0x04000613 RID: 1555
	public float linSpeedMultiplier = 1f;

	// Token: 0x04000614 RID: 1556
	public float maxLinSpeed = 12f;

	// Token: 0x04000615 RID: 1557
	public float maxWristSpeed = 4f;

	// Token: 0x04000616 RID: 1558
	public bool isLeftHanded;

	// Token: 0x04000617 RID: 1559
	[Tooltip("Check this part only if we want to randomize the prefab meshes and projectile")]
	public bool randomModelSelection;

	// Token: 0x04000618 RID: 1560
	public List<BucketThrowable> localModels;

	// Token: 0x04000619 RID: 1561
	[Tooltip("This needs to match the index of the projectilePrefab in Body Dock Position")]
	public int throwableMakerIndex;

	// Token: 0x0400061A RID: 1562
	public string throwEventName;

	// Token: 0x0400061B RID: 1563
	protected VRRig targetRig;

	// Token: 0x0400061C RID: 1564
	protected bool isOfflineRig;

	// Token: 0x0400061D RID: 1565
	private bool awakeHasBeenCalled;

	// Token: 0x0400061E RID: 1566
	private bool OnEnableHasBeenCalled;

	// Token: 0x0400061F RID: 1567
	private MaterialPropertyBlock matPropBlock;

	// Token: 0x04000620 RID: 1568
	private static readonly int colorShaderProp = Shader.PropertyToID("_Color");

	// Token: 0x04000621 RID: 1569
	private Renderer[] renderers;

	// Token: 0x04000622 RID: 1570
	protected int randModelIndex;

	// Token: 0x04000623 RID: 1571
	private float destroyTimer = -1f;
}
