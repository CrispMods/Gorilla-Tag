using System;
using System.Collections.Generic;
using GorillaTag;
using GorillaTagScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x020000D4 RID: 212
public class SnowballThrowable : HoldableObject
{
	// Token: 0x1700006C RID: 108
	// (get) Token: 0x0600057A RID: 1402 RVA: 0x00034037 File Offset: 0x00032237
	internal int ProjectileHash
	{
		get
		{
			return PoolUtils.GameObjHashCode(this.randomModelSelection ? this.localModels[this.randModelIndex].projectilePrefab : this.projectilePrefab);
		}
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x00082320 File Offset: 0x00080520
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

	// Token: 0x0600057C RID: 1404 RVA: 0x00034064 File Offset: 0x00032264
	public bool IsMine()
	{
		return this.targetRig != null && this.targetRig.isOfflineVRRig;
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x000823F4 File Offset: 0x000805F4
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

	// Token: 0x0600057E RID: 1406 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void OnDisable()
	{
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnDestroy()
	{
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x00082504 File Offset: 0x00080704
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

	// Token: 0x06000581 RID: 1409 RVA: 0x0008262C File Offset: 0x0008082C
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

	// Token: 0x06000582 RID: 1410 RVA: 0x00082694 File Offset: 0x00080894
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

	// Token: 0x06000583 RID: 1411 RVA: 0x00082708 File Offset: 0x00080908
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

	// Token: 0x06000584 RID: 1412 RVA: 0x00030607 File Offset: 0x0002E807
	protected void LateUpdateReplicated()
	{
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x00030607 File Offset: 0x0002E807
	protected void LateUpdateShared()
	{
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x00034081 File Offset: 0x00032281
	private Transform Anchor()
	{
		return base.transform.parent;
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x000827A4 File Offset: 0x000809A4
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

	// Token: 0x06000588 RID: 1416 RVA: 0x0003408E File Offset: 0x0003228E
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

	// Token: 0x06000589 RID: 1417 RVA: 0x000340AC File Offset: 0x000322AC
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (!base.OnRelease(zoneReleased, releasingHand))
		{
			return false;
		}
		this.OnSnowballRelease();
		return true;
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x000340C1 File Offset: 0x000322C1
	protected virtual void OnSnowballRelease()
	{
		this.PerformSnowballThrowAuthority();
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x000827FC File Offset: 0x000809FC
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
		SlingshotProjectile slingshotProjectile = this.LaunchSnowballLocal(position, velocity, x, this.randomizeColor, throwableProjectileColor);
		this.SetSnowballActiveLocal(false);
		if (this.randModelIndex > -1 && this.randModelIndex < this.localModels.Count && this.localModels[this.randModelIndex].destroyAfterRelease)
		{
			slingshotProjectile.DestroyAfterRelease();
		}
		if (NetworkSystem.Instance.InRoom)
		{
			RoomSystem.SendLaunchProjectile(position, velocity, this.isLeftHanded ? RoomSystem.ProjectileSource.LeftHand : RoomSystem.ProjectileSource.RightHand, slingshotProjectile.myProjectileCount, this.randomizeColor, throwableProjectileColor.r, throwableProjectileColor.g, throwableProjectileColor.b, throwableProjectileColor.a);
		}
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x00082980 File Offset: 0x00080B80
	protected virtual SlingshotProjectile LaunchSnowballLocal(Vector3 location, Vector3 velocity, float scale, bool randomColour, Color colour)
	{
		SlingshotProjectile component = ObjectPools.instance.Instantiate(this.randomModelSelection ? this.localModels[this.randModelIndex].projectilePrefab : this.projectilePrefab).GetComponent<SlingshotProjectile>();
		int projectileCount = ProjectileTracker.AddAndIncrementLocalProjectile(component, velocity, location, scale);
		component.Launch(location, velocity, NetworkSystem.Instance.LocalPlayer, false, false, projectileCount, scale, randomColour, colour);
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

	// Token: 0x0600058D RID: 1421 RVA: 0x000340C9 File Offset: 0x000322C9
	protected virtual SlingshotProjectile SpawnProjectile()
	{
		return ObjectPools.instance.Instantiate(this.randomModelSelection ? this.localModels[this.randModelIndex].projectilePrefab : this.projectilePrefab).GetComponent<SlingshotProjectile>();
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x00082A24 File Offset: 0x00080C24
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

	// Token: 0x0600058F RID: 1423 RVA: 0x00082A70 File Offset: 0x00080C70
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

	// Token: 0x06000590 RID: 1424 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x00030607 File Offset: 0x0002E807
	public override void DropItemCleanup()
	{
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x00034100 File Offset: 0x00032300
	private void HandleOnGorillaHeadTriggerEntered(bool enable)
	{
		this.SetSnowballActiveLocal(enable);
	}

	// Token: 0x0400064D RID: 1613
	[GorillaSoundLookup]
	public List<int> matDataIndexes = new List<int>
	{
		32
	};

	// Token: 0x0400064E RID: 1614
	public GameObject projectilePrefab;

	// Token: 0x0400064F RID: 1615
	[FormerlySerializedAs("shouldColorize")]
	public bool randomizeColor;

	// Token: 0x04000650 RID: 1616
	public GTColor.HSVRanges randomColorHSVRanges = new GTColor.HSVRanges(0f, 1f, 0.7f, 1f, 1f, 1f);

	// Token: 0x04000651 RID: 1617
	public GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04000652 RID: 1618
	public SoundBankPlayer pickupSoundBankPlayer;

	// Token: 0x04000653 RID: 1619
	public float linSpeedMultiplier = 1f;

	// Token: 0x04000654 RID: 1620
	public float maxLinSpeed = 12f;

	// Token: 0x04000655 RID: 1621
	public float maxWristSpeed = 4f;

	// Token: 0x04000656 RID: 1622
	public bool isLeftHanded;

	// Token: 0x04000657 RID: 1623
	[Tooltip("Check this part only if we want to randomize the prefab meshes and projectile")]
	public bool randomModelSelection;

	// Token: 0x04000658 RID: 1624
	public List<BucketThrowable> localModels;

	// Token: 0x04000659 RID: 1625
	[Tooltip("This needs to match the index of the projectilePrefab in Body Dock Position")]
	public int throwableMakerIndex;

	// Token: 0x0400065A RID: 1626
	public string throwEventName;

	// Token: 0x0400065B RID: 1627
	protected VRRig targetRig;

	// Token: 0x0400065C RID: 1628
	protected bool isOfflineRig;

	// Token: 0x0400065D RID: 1629
	private bool awakeHasBeenCalled;

	// Token: 0x0400065E RID: 1630
	private bool OnEnableHasBeenCalled;

	// Token: 0x0400065F RID: 1631
	private MaterialPropertyBlock matPropBlock;

	// Token: 0x04000660 RID: 1632
	private static readonly int colorShaderProp = Shader.PropertyToID("_Color");

	// Token: 0x04000661 RID: 1633
	private Renderer[] renderers;

	// Token: 0x04000662 RID: 1634
	protected int randModelIndex;

	// Token: 0x04000663 RID: 1635
	private float destroyTimer = -1f;
}
