using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AA;
using BoingKit;
using GorillaExtensions;
using GorillaLocomotion.Climbing;
using GorillaLocomotion.Gameplay;
using GorillaLocomotion.Swimming;
using GorillaTag;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

namespace GorillaLocomotion
{
	// Token: 0x02000B3B RID: 2875
	public class GTPlayer : MonoBehaviour
	{
		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06004792 RID: 18322 RVA: 0x001546FE File Offset: 0x001528FE
		public static GTPlayer Instance
		{
			get
			{
				return GTPlayer._instance;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06004793 RID: 18323 RVA: 0x00154705 File Offset: 0x00152905
		public Vector3 InstantaneousVelocity
		{
			get
			{
				return this.currentVelocity;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06004794 RID: 18324 RVA: 0x0015470D File Offset: 0x0015290D
		public Vector3 AveragedVelocity
		{
			get
			{
				return this.averagedVelocity;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06004795 RID: 18325 RVA: 0x00154715 File Offset: 0x00152915
		public Transform CosmeticsHeadTarget
		{
			get
			{
				return this.cosmeticsHeadTarget;
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06004796 RID: 18326 RVA: 0x0015471D File Offset: 0x0015291D
		public float scale
		{
			get
			{
				return this.scaleMultiplier * this.nativeScale;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06004797 RID: 18327 RVA: 0x0015472C File Offset: 0x0015292C
		public float NativeScale
		{
			get
			{
				return this.nativeScale;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06004798 RID: 18328 RVA: 0x00154734 File Offset: 0x00152934
		public float ScaleMultiplier
		{
			get
			{
				return this.scaleMultiplier;
			}
		}

		// Token: 0x06004799 RID: 18329 RVA: 0x0015473C File Offset: 0x0015293C
		public void SetScaleMultiplier(float s)
		{
			this.scaleMultiplier = s;
		}

		// Token: 0x0600479A RID: 18330 RVA: 0x00154748 File Offset: 0x00152948
		public void SetNativeScale(NativeSizeChangerSettings s)
		{
			float num = this.nativeScale;
			if (s != null && s.playerSizeScale > 0f && s.playerSizeScale != 1f)
			{
				this.activeSizeChangerSettings = s;
			}
			else
			{
				this.activeSizeChangerSettings = null;
			}
			if (this.activeSizeChangerSettings == null)
			{
				this.nativeScale = 1f;
			}
			else
			{
				this.nativeScale = this.activeSizeChangerSettings.playerSizeScale;
			}
			if (num != this.nativeScale && NetworkSystem.Instance.InRoom)
			{
				GorillaTagger.Instance.myVRRig != null;
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x0600479B RID: 18331 RVA: 0x001547D3 File Offset: 0x001529D3
		public bool IsDefaultScale
		{
			get
			{
				return Mathf.Abs(1f - this.scale) < 0.001f;
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x0600479C RID: 18332 RVA: 0x001547ED File Offset: 0x001529ED
		public bool turnedThisFrame
		{
			get
			{
				return this.degreesTurnedThisFrame != 0f;
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x0600479D RID: 18333 RVA: 0x001547FF File Offset: 0x001529FF
		public List<GTPlayer.MaterialData> materialData
		{
			get
			{
				return this.materialDatasSO.datas;
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x0600479E RID: 18334 RVA: 0x0015480C File Offset: 0x00152A0C
		// (set) Token: 0x0600479F RID: 18335 RVA: 0x00154814 File Offset: 0x00152A14
		protected bool IsFrozen { get; set; }

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x060047A0 RID: 18336 RVA: 0x0015481D File Offset: 0x00152A1D
		public List<WaterVolume> HeadOverlappingWaterVolumes
		{
			get
			{
				return this.headOverlappingWaterVolumes;
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x060047A1 RID: 18337 RVA: 0x00154825 File Offset: 0x00152A25
		public bool InWater
		{
			get
			{
				return this.bodyInWater;
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x060047A2 RID: 18338 RVA: 0x0015482D File Offset: 0x00152A2D
		public bool HeadInWater
		{
			get
			{
				return this.headInWater;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x060047A3 RID: 18339 RVA: 0x00154835 File Offset: 0x00152A35
		public WaterVolume CurrentWaterVolume
		{
			get
			{
				if (this.bodyOverlappingWaterVolumes.Count <= 0)
				{
					return null;
				}
				return this.bodyOverlappingWaterVolumes[0];
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x060047A4 RID: 18340 RVA: 0x00154853 File Offset: 0x00152A53
		public WaterVolume.SurfaceQuery WaterSurfaceForHead
		{
			get
			{
				return this.waterSurfaceForHead;
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x060047A5 RID: 18341 RVA: 0x0015485B File Offset: 0x00152A5B
		public WaterVolume LeftHandWaterVolume
		{
			get
			{
				return this.leftHandWaterVolume;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x060047A6 RID: 18342 RVA: 0x00154863 File Offset: 0x00152A63
		public WaterVolume RightHandWaterVolume
		{
			get
			{
				return this.rightHandWaterVolume;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x060047A7 RID: 18343 RVA: 0x0015486B File Offset: 0x00152A6B
		public WaterVolume.SurfaceQuery LeftHandWaterSurface
		{
			get
			{
				return this.leftHandWaterSurface;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x060047A8 RID: 18344 RVA: 0x00154873 File Offset: 0x00152A73
		public WaterVolume.SurfaceQuery RightHandWaterSurface
		{
			get
			{
				return this.rightHandWaterSurface;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x060047A9 RID: 18345 RVA: 0x0015487B File Offset: 0x00152A7B
		public Vector3 LastLeftHandPosition
		{
			get
			{
				return this.lastLeftHandPosition;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x060047AA RID: 18346 RVA: 0x00154883 File Offset: 0x00152A83
		public Vector3 LastRightHandPosition
		{
			get
			{
				return this.lastRightHandPosition;
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x060047AB RID: 18347 RVA: 0x0015488B File Offset: 0x00152A8B
		public Vector3 RigidbodyVelocity
		{
			get
			{
				return this.playerRigidBody.velocity;
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x060047AC RID: 18348 RVA: 0x00154898 File Offset: 0x00152A98
		public Vector3 HeadCenterPosition
		{
			get
			{
				return this.headCollider.transform.position + this.headCollider.transform.rotation * new Vector3(0f, 0f, -0.11f);
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x060047AD RID: 18349 RVA: 0x001548D8 File Offset: 0x00152AD8
		public bool HandContactingSurface
		{
			get
			{
				return this.isLeftHandColliding || this.isRightHandColliding;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x060047AE RID: 18350 RVA: 0x001548EA File Offset: 0x00152AEA
		public bool BodyOnGround
		{
			get
			{
				return this.bodyGroundContactTime >= Time.time - 0.05f;
			}
		}

		// Token: 0x17000790 RID: 1936
		// (set) Token: 0x060047AF RID: 18351 RVA: 0x00154902 File Offset: 0x00152B02
		public Quaternion PlayerRotationOverride
		{
			set
			{
				this.playerRotationOverride = value;
				this.playerRotationOverrideFrame = Time.frameCount;
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x060047B0 RID: 18352 RVA: 0x00154916 File Offset: 0x00152B16
		// (set) Token: 0x060047B1 RID: 18353 RVA: 0x0015491E File Offset: 0x00152B1E
		public bool IsBodySliding { get; set; }

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x060047B2 RID: 18354 RVA: 0x00154927 File Offset: 0x00152B27
		public GorillaClimbable CurrentClimbable
		{
			get
			{
				return this.currentClimbable;
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x060047B3 RID: 18355 RVA: 0x0015492F File Offset: 0x00152B2F
		public GorillaHandClimber CurrentClimber
		{
			get
			{
				return this.currentClimber;
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x060047B4 RID: 18356 RVA: 0x00154937 File Offset: 0x00152B37
		// (set) Token: 0x060047B5 RID: 18357 RVA: 0x0015493F File Offset: 0x00152B3F
		public float jumpMultiplier
		{
			get
			{
				return this._jumpMultiplier;
			}
			set
			{
				this._jumpMultiplier = value;
			}
		}

		// Token: 0x060047B6 RID: 18358 RVA: 0x00154948 File Offset: 0x00152B48
		private void Awake()
		{
			if (GTPlayer._instance != null && GTPlayer._instance != this)
			{
				Object.Destroy(base.gameObject);
			}
			else
			{
				GTPlayer._instance = this;
				GTPlayer.hasInstance = true;
			}
			this.InitializeValues();
			this.playerRigidBody.maxAngularVelocity = 0f;
			this.bodyOffsetVector = new Vector3(0f, -this.bodyCollider.height / 2f, 0f);
			this.bodyInitialHeight = this.bodyCollider.height;
			this.bodyInitialRadius = this.bodyCollider.radius;
			this.rayCastNonAllocColliders = new RaycastHit[5];
			this.crazyCheckVectors = new Vector3[7];
			this.emptyHit = default(RaycastHit);
			this.crazyCheckVectors[0] = Vector3.up;
			this.crazyCheckVectors[1] = Vector3.down;
			this.crazyCheckVectors[2] = Vector3.left;
			this.crazyCheckVectors[3] = Vector3.right;
			this.crazyCheckVectors[4] = Vector3.forward;
			this.crazyCheckVectors[5] = Vector3.back;
			this.crazyCheckVectors[6] = Vector3.zero;
			if (this.controllerState == null)
			{
				this.controllerState = base.GetComponent<ConnectedControllerHandler>();
			}
			this.layerChanger = base.GetComponent<LayerChanger>();
			this.bodyTouchedSurfaces = new Dictionary<GameObject, PhysicMaterial>();
		}

		// Token: 0x060047B7 RID: 18359 RVA: 0x00154AB4 File Offset: 0x00152CB4
		protected void Start()
		{
			if (this.mainCamera == null)
			{
				this.mainCamera = Camera.main;
			}
			this.mainCamera.farClipPlane = 500f;
			this.lastScale = this.scale;
			this.layerChanger.InitializeLayers(base.transform);
			float degrees = Quaternion.Angle(Quaternion.identity, GorillaTagger.Instance.offlineVRRig.transform.rotation) * Mathf.Sign(Vector3.Dot(Vector3.up, GorillaTagger.Instance.offlineVRRig.transform.right));
			this.Turn(degrees);
		}

		// Token: 0x060047B8 RID: 18360 RVA: 0x00154B51 File Offset: 0x00152D51
		protected void OnDestroy()
		{
			if (GTPlayer._instance == this)
			{
				GTPlayer._instance = null;
				GTPlayer.hasInstance = false;
			}
			if (this.climbHelper)
			{
				Object.Destroy(this.climbHelper.gameObject);
			}
		}

		// Token: 0x060047B9 RID: 18361 RVA: 0x00154B8C File Offset: 0x00152D8C
		public void InitializeValues()
		{
			Physics.SyncTransforms();
			this.playerRigidBody = base.GetComponent<Rigidbody>();
			this.velocityHistory = new Vector3[this.velocityHistorySize];
			this.slideAverageHistory = new Vector3[this.velocityHistorySize];
			for (int i = 0; i < this.velocityHistory.Length; i++)
			{
				this.velocityHistory[i] = Vector3.zero;
				this.slideAverageHistory[i] = Vector3.zero;
			}
			this.leftHandFollower.transform.position = this.leftControllerTransform.position;
			this.rightHandFollower.transform.position = this.rightControllerTransform.position;
			this.lastLeftHandPosition = this.leftHandFollower.transform.position;
			this.lastRightHandPosition = this.rightHandFollower.transform.position;
			this.lastHeadPosition = this.headCollider.transform.position;
			this.wasLeftHandColliding = false;
			this.wasRightHandColliding = false;
			this.velocityIndex = 0;
			this.averagedVelocity = Vector3.zero;
			this.slideVelocity = Vector3.zero;
			this.lastPosition = base.transform.position;
			this.lastRealTime = Time.realtimeSinceStartup;
			this.lastOpenHeadPosition = this.headCollider.transform.position;
			this.bodyCollider.transform.position = this.PositionWithOffset(this.headCollider.transform, this.bodyOffset) + this.bodyOffsetVector;
			this.bodyCollider.transform.eulerAngles = new Vector3(0f, this.headCollider.transform.eulerAngles.y, 0f);
		}

		// Token: 0x060047BA RID: 18362 RVA: 0x00154D3C File Offset: 0x00152F3C
		public void SetHalloweenLevitation(float levitateStrength, float levitateDuration, float levitateBlendOutDuration, float levitateBonusStrength, float levitateBonusOffAtYSpeed, float levitateBonusFullAtYSpeed)
		{
			this.halloweenLevitationStrength = levitateStrength;
			this.halloweenLevitationFullStrengthDuration = levitateDuration;
			this.halloweenLevitationTotalDuration = levitateDuration + levitateBlendOutDuration;
			this.halloweenLevitateBonusFullAtYSpeed = levitateBonusFullAtYSpeed;
			this.halloweenLevitateBonusOffAtYSpeed = levitateBonusFullAtYSpeed;
			this.halloweenLevitationBonusStrength = levitateBonusStrength;
		}

		// Token: 0x060047BB RID: 18363 RVA: 0x00154D6D File Offset: 0x00152F6D
		public void TeleportToTrain(bool enable)
		{
			this.teleportToTrain = enable;
		}

		// Token: 0x060047BC RID: 18364 RVA: 0x00154D78 File Offset: 0x00152F78
		public void TeleportTo(Vector3 position, Quaternion rotation)
		{
			base.transform.position = position;
			base.transform.rotation = rotation;
			this.leftHandFollower.position = this.leftControllerTransform.position;
			this.leftHandFollower.rotation = this.leftControllerTransform.rotation;
			this.rightHandFollower.position = this.rightControllerTransform.position;
			this.rightHandFollower.rotation = this.rightControllerTransform.rotation;
			this.lastLeftHandPosition = this.leftHandFollower.transform.position;
			this.lastRightHandPosition = this.rightHandFollower.transform.position;
			this.lastHeadPosition = this.headCollider.transform.position;
			this.lastPosition = position;
			this.lastOpenHeadPosition = this.headCollider.transform.position;
			GorillaTagger.Instance.offlineVRRig.transform.position = position;
		}

		// Token: 0x060047BD RID: 18365 RVA: 0x00154E6C File Offset: 0x0015306C
		public void TeleportTo(Transform destination, bool setPlayerObjectRotationToDest = false)
		{
			Vector3 position = base.transform.position;
			Vector3 b = this.mainCamera.transform.position - position;
			Vector3 position2 = destination.position - b;
			this.Turn(destination.rotation.eulerAngles.y - this.mainCamera.transform.rotation.eulerAngles.y);
			this.TeleportTo(position2, setPlayerObjectRotationToDest ? destination.rotation : base.transform.rotation);
		}

		// Token: 0x060047BE RID: 18366 RVA: 0x00154EFD File Offset: 0x001530FD
		public void AddForce(Vector3 force, ForceMode mode)
		{
			this.playerRigidBody.AddForce(force, mode);
		}

		// Token: 0x060047BF RID: 18367 RVA: 0x00154F0C File Offset: 0x0015310C
		public void SetPlayerVelocity(Vector3 newVelocity)
		{
			for (int i = 0; i < this.velocityHistory.Length; i++)
			{
				this.velocityHistory[i] = newVelocity;
			}
			this.playerRigidBody.AddForce(newVelocity - this.playerRigidBody.velocity, ForceMode.VelocityChange);
		}

		// Token: 0x060047C0 RID: 18368 RVA: 0x00154F56 File Offset: 0x00153156
		public void SetGravityOverride(Object caller, Action<GTPlayer> gravityFunction)
		{
			if (!this.gravityOverrides.ContainsKey(caller))
			{
				this.gravityOverrides.Add(caller, gravityFunction);
				return;
			}
			this.gravityOverrides[caller] = gravityFunction;
		}

		// Token: 0x060047C1 RID: 18369 RVA: 0x00154F81 File Offset: 0x00153181
		public void UnsetGravityOverride(Object caller)
		{
			this.gravityOverrides.Remove(caller);
		}

		// Token: 0x060047C2 RID: 18370 RVA: 0x00154F90 File Offset: 0x00153190
		private void ApplyGravityOverrides()
		{
			foreach (KeyValuePair<Object, Action<GTPlayer>> keyValuePair in this.gravityOverrides)
			{
				keyValuePair.Value(this);
			}
		}

		// Token: 0x060047C3 RID: 18371 RVA: 0x00154FEC File Offset: 0x001531EC
		public void ApplyKnockback(Vector3 direction, float speed, bool forceOffTheGround = false)
		{
			if (forceOffTheGround)
			{
				if (this.wasLeftHandColliding || this.wasRightHandColliding)
				{
					this.wasLeftHandColliding = false;
					this.wasRightHandColliding = false;
					this.playerRigidBody.transform.position += this.minimumRaycastDistance * this.scale * Vector3.up;
				}
				this.didAJump = true;
				this.SetMaximumSlipThisFrame();
			}
			if (speed > 0.01f)
			{
				float num = Vector3.Dot(this.averagedVelocity, direction);
				float d = Mathf.InverseLerp(1.5f, 0.5f, num / speed);
				Vector3 vector = this.averagedVelocity + direction * speed * d;
				this.playerRigidBody.velocity = vector;
				for (int i = 0; i < this.velocityHistory.Length; i++)
				{
					this.velocityHistory[i] = vector;
				}
			}
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x001550C8 File Offset: 0x001532C8
		public void FixedUpdate()
		{
			this.AntiTeleportTechnology();
			this.IsFrozen = (GorillaTagger.Instance.offlineVRRig.IsFrozen || this.debugFreezeTag);
			bool isDefaultScale = this.IsDefaultScale;
			this.playerRigidBody.useGravity = false;
			if (this.gravityOverrides.Count > 0)
			{
				this.ApplyGravityOverrides();
			}
			else
			{
				if (!this.isClimbing)
				{
					this.playerRigidBody.AddForce(Physics.gravity * this.scale, ForceMode.Acceleration);
				}
				if (this.halloweenLevitationBonusStrength > 0f || this.halloweenLevitationStrength > 0f)
				{
					float num = Time.time - this.lastTouchedGroundTimestamp;
					if (num < this.halloweenLevitationTotalDuration)
					{
						this.playerRigidBody.AddForce(Vector3.up * this.halloweenLevitationStrength * Mathf.InverseLerp(this.halloweenLevitationFullStrengthDuration, this.halloweenLevitationTotalDuration, num), ForceMode.Acceleration);
					}
					float y = this.playerRigidBody.velocity.y;
					if (y <= this.halloweenLevitateBonusFullAtYSpeed)
					{
						this.playerRigidBody.AddForce(Vector3.up * this.halloweenLevitationBonusStrength, ForceMode.Acceleration);
					}
					else if (y <= this.halloweenLevitateBonusOffAtYSpeed)
					{
						Mathf.InverseLerp(this.halloweenLevitateBonusOffAtYSpeed, this.halloweenLevitateBonusFullAtYSpeed, this.playerRigidBody.velocity.y);
						this.playerRigidBody.AddForce(Vector3.up * this.halloweenLevitationBonusStrength * Mathf.InverseLerp(this.halloweenLevitateBonusOffAtYSpeed, this.halloweenLevitateBonusFullAtYSpeed, this.playerRigidBody.velocity.y), ForceMode.Acceleration);
					}
				}
			}
			if (this.enableHoverMode)
			{
				this.playerRigidBody.velocity = this.HoverboardFixedUpdate(this.playerRigidBody.velocity);
			}
			else
			{
				this.didHoverLastFrame = false;
			}
			float fixedDeltaTime = Time.fixedDeltaTime;
			this.bodyInWater = false;
			Vector3 lhs = this.swimmingVelocity;
			this.swimmingVelocity = Vector3.MoveTowards(this.swimmingVelocity, Vector3.zero, this.swimmingParams.swimmingVelocityOutOfWaterDrainRate * fixedDeltaTime);
			this.leftHandNonDiveHapticsAmount = 0f;
			this.rightHandNonDiveHapticsAmount = 0f;
			if (this.bodyOverlappingWaterVolumes.Count > 0)
			{
				WaterVolume waterVolume = null;
				float num2 = float.MinValue;
				Vector3 vector = this.headCollider.transform.position + Vector3.down * this.swimmingParams.floatingWaterLevelBelowHead * this.scale;
				this.activeWaterCurrents.Clear();
				for (int i = 0; i < this.bodyOverlappingWaterVolumes.Count; i++)
				{
					WaterVolume.SurfaceQuery surfaceQuery;
					if (this.bodyOverlappingWaterVolumes[i].GetSurfaceQueryForPoint(vector, out surfaceQuery, false))
					{
						float num3 = Vector3.Dot(surfaceQuery.surfacePoint - vector, surfaceQuery.surfaceNormal);
						if (num3 > num2)
						{
							num2 = num3;
							waterVolume = this.bodyOverlappingWaterVolumes[i];
							this.waterSurfaceForHead = surfaceQuery;
						}
						WaterCurrent waterCurrent = this.bodyOverlappingWaterVolumes[i].Current;
						if (waterCurrent != null && num3 > 0f && !this.activeWaterCurrents.Contains(waterCurrent))
						{
							this.activeWaterCurrents.Add(waterCurrent);
						}
					}
				}
				if (waterVolume != null)
				{
					Vector3 velocity = this.playerRigidBody.velocity;
					float magnitude = velocity.magnitude;
					bool flag = this.headInWater;
					this.headInWater = (this.headCollider.transform.position.y < this.waterSurfaceForHead.surfacePoint.y && this.headCollider.transform.position.y > this.waterSurfaceForHead.surfacePoint.y - this.waterSurfaceForHead.maxDepth);
					if (this.headInWater && !flag)
					{
						this.audioSetToUnderwater = true;
						this.audioManager.SetMixerSnapshot(this.audioManager.underwaterSnapshot, 0.1f);
					}
					else if (!this.headInWater && flag)
					{
						this.audioSetToUnderwater = false;
						this.audioManager.UnsetMixerSnapshot(0.1f);
					}
					this.bodyInWater = (vector.y < this.waterSurfaceForHead.surfacePoint.y && vector.y > this.waterSurfaceForHead.surfacePoint.y - this.waterSurfaceForHead.maxDepth);
					if (this.bodyInWater)
					{
						GTPlayer.LiquidProperties liquidProperties = this.liquidPropertiesList[(int)waterVolume.LiquidType];
						if (waterVolume != null)
						{
							float d;
							if (this.swimmingParams.extendBouyancyFromSpeed)
							{
								float time = Mathf.Clamp(Vector3.Dot(velocity / this.scale, this.waterSurfaceForHead.surfaceNormal), this.swimmingParams.speedToBouyancyExtensionMinMax.x, this.swimmingParams.speedToBouyancyExtensionMinMax.y);
								float b = this.swimmingParams.speedToBouyancyExtension.Evaluate(time);
								this.buoyancyExtension = Mathf.Max(this.buoyancyExtension, b);
								float num4 = Mathf.InverseLerp(0f, this.swimmingParams.buoyancyFadeDist + this.buoyancyExtension, num2 / this.scale + this.buoyancyExtension);
								this.buoyancyExtension = Spring.DamperDecayExact(this.buoyancyExtension, this.swimmingParams.buoyancyExtensionDecayHalflife, fixedDeltaTime, 1E-05f);
								d = num4;
							}
							else
							{
								d = Mathf.InverseLerp(0f, this.swimmingParams.buoyancyFadeDist, num2 / this.scale);
							}
							Vector3 a = Physics.gravity * this.scale;
							Vector3 vector2 = liquidProperties.buoyancy * -a * d;
							if (this.IsFrozen && GorillaGameManager.instance is GorillaFreezeTagManager)
							{
								vector2 *= this.frozenBodyBuoyancyFactor;
							}
							this.playerRigidBody.AddForce(vector2, ForceMode.Acceleration);
						}
						Vector3 vector3 = Vector3.zero;
						Vector3 vector4 = Vector3.zero;
						for (int j = 0; j < this.activeWaterCurrents.Count; j++)
						{
							WaterCurrent waterCurrent2 = this.activeWaterCurrents[j];
							Vector3 startingVelocity = velocity + vector3;
							Vector3 b2;
							Vector3 b3;
							if (waterCurrent2.GetCurrentAtPoint(this.bodyCollider.transform.position, startingVelocity, fixedDeltaTime, out b2, out b3))
							{
								vector4 += b2;
								vector3 += b3;
							}
						}
						if (magnitude > Mathf.Epsilon)
						{
							float num5 = 0.01f;
							Vector3 vector5 = velocity / magnitude;
							Vector3 right = this.leftHandFollower.right;
							Vector3 dir = -this.rightHandFollower.right;
							Vector3 forward = this.leftHandFollower.forward;
							Vector3 forward2 = this.rightHandFollower.forward;
							Vector3 a2 = vector5;
							float num6 = 0f;
							float num7 = 0f;
							float num8 = 0f;
							if (this.swimmingParams.applyDiveSteering && !this.disableMovement && isDefaultScale)
							{
								float value = Vector3.Dot(velocity - vector4, vector5);
								float time2 = Mathf.Clamp(value, this.swimmingParams.swimSpeedToRedirectAmountMinMax.x, this.swimmingParams.swimSpeedToRedirectAmountMinMax.y);
								float b4 = this.swimmingParams.swimSpeedToRedirectAmount.Evaluate(time2);
								time2 = Mathf.Clamp(value, this.swimmingParams.swimSpeedToMaxRedirectAngleMinMax.x, this.swimmingParams.swimSpeedToMaxRedirectAngleMinMax.y);
								float num9 = this.swimmingParams.swimSpeedToMaxRedirectAngle.Evaluate(time2);
								float value2 = Mathf.Acos(Vector3.Dot(vector5, forward)) / 3.1415927f * -2f + 1f;
								float value3 = Mathf.Acos(Vector3.Dot(vector5, forward2)) / 3.1415927f * -2f + 1f;
								float num10 = Mathf.Clamp(value2, this.swimmingParams.palmFacingToRedirectAmountMinMax.x, this.swimmingParams.palmFacingToRedirectAmountMinMax.y);
								float num11 = Mathf.Clamp(value3, this.swimmingParams.palmFacingToRedirectAmountMinMax.x, this.swimmingParams.palmFacingToRedirectAmountMinMax.y);
								float a3 = (!float.IsNaN(num10)) ? this.swimmingParams.palmFacingToRedirectAmount.Evaluate(num10) : 0f;
								float a4 = (!float.IsNaN(num11)) ? this.swimmingParams.palmFacingToRedirectAmount.Evaluate(num11) : 0f;
								Vector3 a5 = Vector3.ProjectOnPlane(vector5, right);
								Vector3 a6 = Vector3.ProjectOnPlane(vector5, right);
								float num12 = Mathf.Min(a5.magnitude, 1f);
								float num13 = Mathf.Min(a6.magnitude, 1f);
								float magnitude2 = this.leftHandCenterVelocityTracker.GetAverageVelocity(false, this.swimmingParams.diveVelocityAveragingWindow, false).magnitude;
								float magnitude3 = this.rightHandCenterVelocityTracker.GetAverageVelocity(false, this.swimmingParams.diveVelocityAveragingWindow, false).magnitude;
								float time3 = Mathf.Clamp(magnitude2, this.swimmingParams.handSpeedToRedirectAmountMinMax.x, this.swimmingParams.handSpeedToRedirectAmountMinMax.y);
								float time4 = Mathf.Clamp(magnitude3, this.swimmingParams.handSpeedToRedirectAmountMinMax.x, this.swimmingParams.handSpeedToRedirectAmountMinMax.y);
								float a7 = this.swimmingParams.handSpeedToRedirectAmount.Evaluate(time3);
								float a8 = this.swimmingParams.handSpeedToRedirectAmount.Evaluate(time4);
								float averageSpeedChangeMagnitudeInDirection = this.leftHandCenterVelocityTracker.GetAverageSpeedChangeMagnitudeInDirection(right, false, this.swimmingParams.diveVelocityAveragingWindow);
								float averageSpeedChangeMagnitudeInDirection2 = this.rightHandCenterVelocityTracker.GetAverageSpeedChangeMagnitudeInDirection(dir, false, this.swimmingParams.diveVelocityAveragingWindow);
								float time5 = Mathf.Clamp(averageSpeedChangeMagnitudeInDirection, this.swimmingParams.handAccelToRedirectAmountMinMax.x, this.swimmingParams.handAccelToRedirectAmountMinMax.y);
								float time6 = Mathf.Clamp(averageSpeedChangeMagnitudeInDirection2, this.swimmingParams.handAccelToRedirectAmountMinMax.x, this.swimmingParams.handAccelToRedirectAmountMinMax.y);
								float b5 = this.swimmingParams.handAccelToRedirectAmount.Evaluate(time5);
								float b6 = this.swimmingParams.handAccelToRedirectAmount.Evaluate(time6);
								num6 = Mathf.Min(a3, Mathf.Min(a7, b5));
								float num14 = (Vector3.Dot(vector5, forward) > 0f) ? (Mathf.Min(num6, b4) * num12) : 0f;
								num7 = Mathf.Min(a4, Mathf.Min(a8, b6));
								float num15 = (Vector3.Dot(vector5, forward2) > 0f) ? (Mathf.Min(num7, b4) * num13) : 0f;
								if (this.swimmingParams.reduceDiveSteeringBelowVelocityPlane)
								{
									Vector3 rhs;
									if (Vector3.Dot(this.headCollider.transform.up, vector5) > 0.95f)
									{
										rhs = -this.headCollider.transform.forward;
									}
									else
									{
										rhs = Vector3.Cross(Vector3.Cross(vector5, this.headCollider.transform.up), vector5).normalized;
									}
									Vector3 position = this.headCollider.transform.position;
									Vector3 lhs2 = position - this.leftHandFollower.position;
									Vector3 lhs3 = position - this.rightHandFollower.position;
									float reduceDiveSteeringBelowPlaneFadeStartDist = this.swimmingParams.reduceDiveSteeringBelowPlaneFadeStartDist;
									float reduceDiveSteeringBelowPlaneFadeEndDist = this.swimmingParams.reduceDiveSteeringBelowPlaneFadeEndDist;
									float f = Vector3.Dot(lhs2, Vector3.up);
									float f2 = Vector3.Dot(lhs3, Vector3.up);
									float f3 = Vector3.Dot(lhs2, rhs);
									float f4 = Vector3.Dot(lhs3, rhs);
									float num16 = 1f - Mathf.InverseLerp(reduceDiveSteeringBelowPlaneFadeStartDist, reduceDiveSteeringBelowPlaneFadeEndDist, Mathf.Min(Mathf.Abs(f), Mathf.Abs(f3)));
									float num17 = 1f - Mathf.InverseLerp(reduceDiveSteeringBelowPlaneFadeStartDist, reduceDiveSteeringBelowPlaneFadeEndDist, Mathf.Min(Mathf.Abs(f2), Mathf.Abs(f4)));
									num14 *= num16;
									num15 *= num17;
								}
								float num18 = num15 + num14;
								Vector3 vector6 = Vector3.zero;
								if (this.swimmingParams.applyDiveSteering && num18 > num5)
								{
									vector6 = ((num14 * a5 + num15 * a6) / num18).normalized;
									vector6 = Vector3.Lerp(vector5, vector6, num18);
									a2 = Vector3.RotateTowards(vector5, vector6, 0.017453292f * num9 * fixedDeltaTime, 0f);
								}
								else
								{
									a2 = vector5;
								}
								num8 = Mathf.Clamp01((num6 + num7) * 0.5f);
							}
							float num19 = Mathf.Clamp(Vector3.Dot(lhs, vector5), 0f, magnitude);
							float num20 = magnitude - num19;
							if (this.swimmingParams.applyDiveSwimVelocityConversion && !this.disableMovement && num8 > num5 && num19 < this.swimmingParams.diveMaxSwimVelocityConversion)
							{
								float num21 = Mathf.Min(this.swimmingParams.diveSwimVelocityConversionRate * fixedDeltaTime, num20) * num8;
								num19 += num21;
								num20 -= num21;
							}
							float halflife = this.swimmingParams.swimUnderWaterDampingHalfLife * liquidProperties.dampingFactor;
							float halflife2 = this.swimmingParams.baseUnderWaterDampingHalfLife * liquidProperties.dampingFactor;
							float num22 = Spring.DamperDecayExact(num19 / this.scale, halflife, fixedDeltaTime, 1E-05f) * this.scale;
							float num23 = Spring.DamperDecayExact(num20 / this.scale, halflife2, fixedDeltaTime, 1E-05f) * this.scale;
							if (this.swimmingParams.applyDiveDampingMultiplier && !this.disableMovement)
							{
								float t = Mathf.Lerp(1f, this.swimmingParams.diveDampingMultiplier, num8);
								num22 = Mathf.Lerp(num19, num22, t);
								num23 = Mathf.Lerp(num20, num23, t);
								float time7 = Mathf.Clamp((1f - num6) * (num19 + num20), this.swimmingParams.nonDiveDampingHapticsAmountMinMax.x + num5, this.swimmingParams.nonDiveDampingHapticsAmountMinMax.y - num5);
								float time8 = Mathf.Clamp((1f - num7) * (num19 + num20), this.swimmingParams.nonDiveDampingHapticsAmountMinMax.x + num5, this.swimmingParams.nonDiveDampingHapticsAmountMinMax.y - num5);
								this.leftHandNonDiveHapticsAmount = this.swimmingParams.nonDiveDampingHapticsAmount.Evaluate(time7);
								this.rightHandNonDiveHapticsAmount = this.swimmingParams.nonDiveDampingHapticsAmount.Evaluate(time8);
							}
							this.swimmingVelocity = num22 * a2 + vector3 * this.scale;
							this.playerRigidBody.velocity = this.swimmingVelocity + num23 * a2;
						}
					}
				}
			}
			else if (this.audioSetToUnderwater)
			{
				this.audioSetToUnderwater = false;
				this.audioManager.UnsetMixerSnapshot(0.1f);
			}
			this.handleClimbing(Time.fixedDeltaTime);
			this.stuckHandsCheckFixedUpdate();
			this.FixedUpdate_HandHolds(Time.fixedDeltaTime);
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x060047C5 RID: 18373 RVA: 0x00155F13 File Offset: 0x00154113
		// (set) Token: 0x060047C6 RID: 18374 RVA: 0x00155F1B File Offset: 0x0015411B
		public bool isHoverAllowed { get; private set; }

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x060047C7 RID: 18375 RVA: 0x00155F24 File Offset: 0x00154124
		// (set) Token: 0x060047C8 RID: 18376 RVA: 0x00155F2C File Offset: 0x0015412C
		public bool enableHoverMode { get; private set; }

		// Token: 0x060047C9 RID: 18377 RVA: 0x00155F35 File Offset: 0x00154135
		public void SetHoverboardPosRot(Vector3 worldPos, Quaternion worldRot)
		{
			this.hoverboardPlayerLocalPos = this.headCollider.transform.InverseTransformPoint(worldPos);
			this.hoverboardPlayerLocalRot = this.headCollider.transform.InverseTransformRotation(worldRot);
		}

		// Token: 0x060047CA RID: 18378 RVA: 0x00155F68 File Offset: 0x00154168
		private void HoverboardLateUpdate()
		{
			Vector3 eulerAngles = this.headCollider.transform.eulerAngles;
			bool flag = false;
			for (int i = 0; i < this.hoverboardCasts.Length; i++)
			{
				GTPlayer.HoverBoardCast hoverBoardCast = this.hoverboardCasts[i];
				RaycastHit raycastHit;
				hoverBoardCast.didHit = Physics.SphereCast(new Ray(this.hoverboardVisual.transform.TransformPoint(hoverBoardCast.localOrigin), this.hoverboardVisual.transform.rotation * hoverBoardCast.localDirection), hoverBoardCast.sphereRadius, out raycastHit, hoverBoardCast.distance, this.locomotionEnabledLayers);
				if (hoverBoardCast.didHit)
				{
					HoverboardCantHover hoverboardCantHover;
					if (raycastHit.collider.TryGetComponent<HoverboardCantHover>(out hoverboardCantHover))
					{
						hoverBoardCast.didHit = false;
					}
					else
					{
						hoverBoardCast.pointHit = raycastHit.point;
						hoverBoardCast.normalHit = raycastHit.normal;
					}
				}
				this.hoverboardCasts[i] = hoverBoardCast;
				if (hoverBoardCast.didHit)
				{
					flag = true;
				}
			}
			this.hasHoverPoint = flag;
			this.bodyCollider.enabled = (this.bodyCollider.transform.position - this.hoverboardVisual.transform.TransformPoint(Vector3.up * this.hoverBodyCollisionRadiusUpOffset)).IsLongerThan(this.hoverBodyHasCollisionsOutsideRadius);
		}

		// Token: 0x060047CB RID: 18379 RVA: 0x001560B0 File Offset: 0x001542B0
		private Vector3 HoverboardFixedUpdate(Vector3 velocity)
		{
			this.hoverboardVisual.transform.position = this.headCollider.transform.TransformPoint(this.hoverboardPlayerLocalPos);
			this.hoverboardVisual.transform.rotation = this.headCollider.transform.TransformRotation(this.hoverboardPlayerLocalRot);
			if (this.didHoverLastFrame)
			{
				velocity += Vector3.up * this.hoverGeneralUpwardForce * Time.fixedDeltaTime;
			}
			Vector3 position = this.hoverboardVisual.transform.position;
			Vector3 a = position + velocity * Time.fixedDeltaTime;
			Vector3 vector = this.hoverboardVisual.transform.forward;
			Vector3 vector2 = this.hoverboardCasts[0].didHit ? this.hoverboardCasts[0].normalHit : Vector3.up;
			bool flag = false;
			for (int i = 0; i < this.hoverboardCasts.Length; i++)
			{
				GTPlayer.HoverBoardCast hoverBoardCast = this.hoverboardCasts[i];
				if (hoverBoardCast.didHit)
				{
					Vector3 b = position + Vector3.Project(hoverBoardCast.pointHit - position, vector);
					Vector3 b2 = a + Vector3.Project(hoverBoardCast.pointHit - position, vector);
					bool flag2 = hoverBoardCast.isSolid || Vector3.Dot(hoverBoardCast.normalHit, hoverBoardCast.pointHit - b2) + this.hoverIdealHeight > 0f;
					float d = hoverBoardCast.isSolid ? (Vector3.Dot(hoverBoardCast.normalHit, hoverBoardCast.pointHit - this.hoverboardVisual.transform.TransformPoint(hoverBoardCast.localOrigin + hoverBoardCast.localDirection * hoverBoardCast.distance)) + hoverBoardCast.sphereRadius) : (Vector3.Dot(hoverBoardCast.normalHit, hoverBoardCast.pointHit - b) + this.hoverIdealHeight);
					if (flag2)
					{
						flag = true;
						this.boostEnabledUntilTimestamp = Time.time + this.hoverboardBoostGracePeriod;
						if (Vector3.Dot(velocity, hoverBoardCast.normalHit) < 0f)
						{
							velocity = Vector3.ProjectOnPlane(velocity, hoverBoardCast.normalHit);
						}
						this.playerRigidBody.transform.position += hoverBoardCast.normalHit * d;
						Vector3 vector3 = this.turnParent.transform.rotation * (this.hoverboardVisual.IsLeftHanded ? this.leftHandCenterVelocityTracker : this.rightHandCenterVelocityTracker).GetAverageVelocity(false, 0.15f, false);
						if (Vector3.Dot(vector3, hoverBoardCast.normalHit) < 0f)
						{
							velocity -= Vector3.Project(vector3, hoverBoardCast.normalHit) * this.hoverSlamJumpStrengthFactor * Time.fixedDeltaTime;
						}
						a = position + velocity * Time.fixedDeltaTime;
					}
				}
			}
			float time = Mathf.Abs(Mathf.DeltaAngle(0f, Mathf.Acos(Vector3.Dot(this.hoverboardVisual.transform.up, Vector3.ProjectOnPlane(vector2, vector).normalized)) * 57.29578f));
			float num = this.hoverCarveAngleResponsiveness.Evaluate(time);
			vector = (vector + Vector3.ProjectOnPlane(this.hoverboardVisual.transform.up, vector2) * this.hoverTiltAdjustsForwardFactor).normalized;
			if (!flag)
			{
				this.didHoverLastFrame = false;
				num = 0f;
			}
			Vector3 b3 = velocity;
			if (this.enableHoverMode && this.hasHoverPoint)
			{
				Vector3 vector4 = Vector3.ProjectOnPlane(velocity, vector2);
				Vector3 b4 = velocity - vector4;
				Vector3 vector5 = Vector3.Project(vector4, vector);
				float num2 = vector4.magnitude;
				if (num2 <= this.hoveringSlowSpeed)
				{
					num2 *= this.hoveringSlowStoppingFactor;
				}
				Vector3 vector6 = vector4 - vector5;
				float num3 = 0f;
				bool flag3 = false;
				if (num > 0f)
				{
					if (vector6.IsLongerThan(vector5))
					{
						num3 = Mathf.Min((vector6.magnitude - vector5.magnitude) * this.hoverCarveSidewaysSpeedLossFactor * num, num2);
						if (num3 > 0f && num2 > this.hoverMinGrindSpeed)
						{
							flag3 = true;
							this.hoverboardVisual.PlayGrindHaptic();
						}
						num2 -= num3;
					}
					vector6 *= 1f - num * this.sidewaysDrag;
					if (!this.isLeftHandColliding && !this.isRightHandColliding)
					{
						velocity = (vector5 + vector6).normalized * num2 + b4;
					}
				}
				else
				{
					velocity = vector4.normalized * num2 + b4;
				}
				float magnitude = (velocity - b3).magnitude;
				this.hoverboardAudio.UpdateAudioLoop(velocity.magnitude, this.bodyVelocityTracker.GetAverageVelocity(true, 0.15f, false).magnitude, magnitude, flag3 ? num3 : 0f);
				if (magnitude > 0f && !flag3)
				{
					this.hoverboardVisual.PlayCarveHaptic(magnitude);
				}
			}
			else
			{
				this.hoverboardAudio.UpdateAudioLoop(0f, this.bodyVelocityTracker.GetAverageVelocity(true, 0.15f, false).magnitude, 0f, 0f);
			}
			return velocity;
		}

		// Token: 0x060047CC RID: 18380 RVA: 0x0015660C File Offset: 0x0015480C
		public void GrabPersonalHoverboard(bool isLeftHand, Vector3 pos, Quaternion rot, Color col)
		{
			if (this.hoverboardVisual.IsHeld)
			{
				this.hoverboardVisual.DropFreeBoard();
			}
			this.hoverboardVisual.SetIsHeld(isLeftHand, pos, rot, col);
			this.hoverboardVisual.ProxyGrabHandle(isLeftHand);
			FreeHoverboardManager.instance.PreserveMaxHoverboardsConstraint(NetworkSystem.Instance.LocalPlayer.ActorNumber);
		}

		// Token: 0x060047CD RID: 18381 RVA: 0x00156666 File Offset: 0x00154866
		public void SetHoverAllowed(bool allowed)
		{
			this.isHoverAllowed = allowed;
			if (this.enableHoverMode && !allowed)
			{
				this.SetHoverActive(false);
				VRRig.LocalRig.hoverboardVisual.SetNotHeld();
			}
		}

		// Token: 0x060047CE RID: 18382 RVA: 0x00156690 File Offset: 0x00154890
		public void SetHoverActive(bool enable)
		{
			if (enable && !this.isHoverAllowed)
			{
				return;
			}
			this.enableHoverMode = enable;
			if (!enable)
			{
				this.bodyCollider.enabled = true;
				this.hasHoverPoint = false;
				this.didHoverLastFrame = false;
				for (int i = 0; i < this.hoverboardCasts.Length; i++)
				{
					this.hoverboardCasts[i].didHit = false;
				}
				this.hoverboardAudio.Stop();
			}
		}

		// Token: 0x060047CF RID: 18383 RVA: 0x00156700 File Offset: 0x00154900
		private void BodyCollider()
		{
			if (this.MaxSphereSizeForNoOverlap(this.bodyInitialRadius * this.scale, this.PositionWithOffset(this.headCollider.transform, this.bodyOffset), false, out this.bodyMaxRadius))
			{
				if (this.scale > 0f)
				{
					this.bodyCollider.radius = this.bodyMaxRadius / this.scale;
				}
				if (Physics.SphereCast(this.PositionWithOffset(this.headCollider.transform, this.bodyOffset), this.bodyMaxRadius, Vector3.down, out this.bodyHitInfo, this.bodyInitialHeight * this.scale - this.bodyMaxRadius, this.locomotionEnabledLayers))
				{
					this.bodyCollider.height = (this.bodyHitInfo.distance + this.bodyMaxRadius) / this.scale;
				}
				else
				{
					this.bodyCollider.height = this.bodyInitialHeight;
				}
				if (!this.bodyCollider.gameObject.activeSelf)
				{
					this.bodyCollider.gameObject.SetActive(true);
				}
			}
			else
			{
				this.bodyCollider.gameObject.SetActive(false);
			}
			this.bodyCollider.height = Mathf.Lerp(this.bodyCollider.height, this.bodyInitialHeight, this.bodyLerp);
			this.bodyCollider.radius = Mathf.Lerp(this.bodyCollider.radius, this.bodyInitialRadius, this.bodyLerp);
			this.bodyOffsetVector = Vector3.down * this.bodyCollider.height / 2f;
			this.bodyCollider.transform.position = this.PositionWithOffset(this.headCollider.transform, this.bodyOffset) + this.bodyOffsetVector * this.scale;
			this.bodyCollider.transform.eulerAngles = new Vector3(0f, this.headCollider.transform.eulerAngles.y, 0f);
		}

		// Token: 0x060047D0 RID: 18384 RVA: 0x00156908 File Offset: 0x00154B08
		private Vector3 GetCurrentHandPosition(Transform handTransform, Vector3 handOffset)
		{
			if (this.inOverlay)
			{
				return this.headCollider.transform.position + this.headCollider.transform.up * -0.5f * this.scale;
			}
			if ((this.PositionWithOffset(handTransform, handOffset) - this.headCollider.transform.position).magnitude < this.maxArmLength * this.scale)
			{
				return this.PositionWithOffset(handTransform, handOffset);
			}
			return this.headCollider.transform.position + (this.PositionWithOffset(handTransform, handOffset) - this.headCollider.transform.position).normalized * this.maxArmLength * this.scale;
		}

		// Token: 0x060047D1 RID: 18385 RVA: 0x001569E5 File Offset: 0x00154BE5
		private Vector3 GetLastLeftHandPosition()
		{
			return this.lastLeftHandPosition + this.MovingSurfaceMovement();
		}

		// Token: 0x060047D2 RID: 18386 RVA: 0x001569F8 File Offset: 0x00154BF8
		private Vector3 GetLastRightHandPosition()
		{
			return this.lastRightHandPosition + this.MovingSurfaceMovement();
		}

		// Token: 0x060047D3 RID: 18387 RVA: 0x00156A0C File Offset: 0x00154C0C
		private Vector3 GetCurrentLeftHandPosition()
		{
			if (this.inOverlay)
			{
				return this.headCollider.transform.position + this.headCollider.transform.up * -0.5f * this.scale;
			}
			if ((this.PositionWithOffset(this.leftControllerTransform, this.leftHandOffset) - this.headCollider.transform.position).magnitude < this.maxArmLength * this.scale)
			{
				return this.PositionWithOffset(this.leftControllerTransform, this.leftHandOffset);
			}
			return this.headCollider.transform.position + (this.PositionWithOffset(this.leftControllerTransform, this.leftHandOffset) - this.headCollider.transform.position).normalized * this.maxArmLength * this.scale;
		}

		// Token: 0x060047D4 RID: 18388 RVA: 0x00156B08 File Offset: 0x00154D08
		private Vector3 GetCurrentRightHandPosition()
		{
			if (this.inOverlay)
			{
				return this.headCollider.transform.position + this.headCollider.transform.up * -0.5f * this.scale;
			}
			if ((this.PositionWithOffset(this.rightControllerTransform, this.rightHandOffset) - this.headCollider.transform.position).magnitude < this.maxArmLength * this.scale)
			{
				return this.PositionWithOffset(this.rightControllerTransform, this.rightHandOffset);
			}
			return this.headCollider.transform.position + (this.PositionWithOffset(this.rightControllerTransform, this.rightHandOffset) - this.headCollider.transform.position).normalized * this.maxArmLength * this.scale;
		}

		// Token: 0x060047D5 RID: 18389 RVA: 0x00156C03 File Offset: 0x00154E03
		private Vector3 PositionWithOffset(Transform transformToModify, Vector3 offsetVector)
		{
			return transformToModify.position + transformToModify.rotation * offsetVector * this.scale;
		}

		// Token: 0x060047D6 RID: 18390 RVA: 0x00156C28 File Offset: 0x00154E28
		public void ScaleAwayFromPoint(float oldScale, float newScale, Vector3 scaleCenter)
		{
			if (oldScale < newScale)
			{
				this.lastHeadPosition = GTPlayer.ScalePointAwayFromCenter(this.lastHeadPosition, this.headCollider.radius, oldScale, newScale, scaleCenter);
				this.lastLeftHandPosition = GTPlayer.ScalePointAwayFromCenter(this.lastLeftHandPosition, this.minimumRaycastDistance, oldScale, newScale, scaleCenter);
				this.lastRightHandPosition = GTPlayer.ScalePointAwayFromCenter(this.lastRightHandPosition, this.minimumRaycastDistance, oldScale, newScale, scaleCenter);
			}
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x00156C8C File Offset: 0x00154E8C
		private static Vector3 ScalePointAwayFromCenter(Vector3 point, float baseRadius, float oldScale, float newScale, Vector3 scaleCenter)
		{
			float magnitude = (point - scaleCenter).magnitude;
			float d = magnitude + Mathf.Epsilon + baseRadius * (newScale - oldScale);
			return scaleCenter + (point - scaleCenter) * d / magnitude;
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x00156CD4 File Offset: 0x00154ED4
		private void LateUpdate()
		{
			if (this.playerRigidBody.isKinematic)
			{
				return;
			}
			float time = Time.time;
			Vector3 position = this.headCollider.transform.position;
			if (this.playerRotationOverrideFrame < Time.frameCount - 1)
			{
				this.playerRotationOverride = Quaternion.Slerp(Quaternion.identity, this.playerRotationOverride, Mathf.Exp(-this.playerRotationOverrideDecayRate * Time.deltaTime));
			}
			base.transform.rotation = this.playerRotationOverride;
			this.turnParent.transform.localScale = VRRig.LocalRig.transform.localScale;
			this.playerRigidBody.MovePosition(this.playerRigidBody.position + position - this.headCollider.transform.position);
			if (Mathf.Abs(this.lastScale - this.scale) > 0.001f)
			{
				if (this.mainCamera == null)
				{
					this.mainCamera = Camera.main;
				}
				this.mainCamera.nearClipPlane = ((this.scale > 0.5f) ? 0.01f : 0.002f);
			}
			this.lastScale = this.scale;
			this.debugLastRightHandPosition = this.lastRightHandPosition;
			this.debugPlatformDeltaPosition = this.MovingSurfaceMovement();
			if (this.debugMovement)
			{
				this.tempRealTime = Time.time;
				this.calcDeltaTime = Time.deltaTime;
				this.lastRealTime = this.tempRealTime;
			}
			else
			{
				this.tempRealTime = Time.realtimeSinceStartup;
				this.calcDeltaTime = this.tempRealTime - this.lastRealTime;
				this.lastRealTime = this.tempRealTime;
				if (this.calcDeltaTime > 0.1f)
				{
					this.calcDeltaTime = 0.05f;
				}
			}
			Vector3 a;
			if (this.lastFrameHasValidTouchPos && this.lastPlatformTouched != null && GTPlayer.ComputeWorldHitPoint(this.lastHitInfoHand, this.lastFrameTouchPosLocal, out a))
			{
				this.refMovement = a - this.lastFrameTouchPosWorld;
			}
			else
			{
				this.refMovement = Vector3.zero;
			}
			Vector3 vector = Vector3.zero;
			Quaternion quaternion = Quaternion.identity;
			Vector3 pivot = this.headCollider.transform.position;
			Vector3 vector2;
			if (this.lastMovingSurfaceContact != GTPlayer.MovingSurfaceContactPoint.NONE && GTPlayer.ComputeWorldHitPoint(this.lastMovingSurfaceHit, this.lastMovingSurfaceTouchLocal, out vector2))
			{
				if (this.wasMovingSurfaceMonkeBlock && (this.lastMonkeBlock == null || this.lastMonkeBlock.state != BuilderPiece.State.AttachedAndPlaced))
				{
					this.movingSurfaceOffset = Vector3.zero;
				}
				else
				{
					this.movingSurfaceOffset = vector2 - this.lastMovingSurfaceTouchWorld;
					vector = this.movingSurfaceOffset / this.calcDeltaTime;
					quaternion = this.lastMovingSurfaceHit.collider.transform.rotation * Quaternion.Inverse(this.lastMovingSurfaceRot);
					pivot = vector2;
				}
			}
			else
			{
				this.movingSurfaceOffset = Vector3.zero;
			}
			float num = 40f * this.scale;
			if (vector.sqrMagnitude >= num * num)
			{
				this.movingSurfaceOffset = Vector3.zero;
				vector = Vector3.zero;
				quaternion = Quaternion.identity;
			}
			if (!this.didAJump && (this.wasLeftHandColliding || this.wasRightHandColliding))
			{
				base.transform.position = base.transform.position + 4.9f * Vector3.down * this.calcDeltaTime * this.calcDeltaTime * this.scale;
				if (Vector3.Dot(this.averagedVelocity, this.slideAverageNormal) <= 0f && Vector3.Dot(Vector3.up, this.slideAverageNormal) > 0f)
				{
					base.transform.position = base.transform.position - Vector3.Project(Mathf.Min(this.stickDepth * this.scale, Vector3.Project(this.averagedVelocity, this.slideAverageNormal).magnitude * this.calcDeltaTime) * this.slideAverageNormal, Vector3.down);
				}
			}
			if (!this.didAJump && (this.wasLeftHandSliding || this.wasRightHandSliding))
			{
				base.transform.position = base.transform.position + this.slideVelocity * this.calcDeltaTime;
				this.slideVelocity += 9.8f * Vector3.down * this.calcDeltaTime * this.scale;
			}
			float d = (Time.time > this.boostEnabledUntilTimestamp) ? 0f : (Time.deltaTime * Mathf.Clamp(this.playerRigidBody.velocity.magnitude * this.hoverboardPaddleBoostMultiplier, 0f, this.hoverboardPaddleBoostMax));
			Vector3 boostVector = this.enableHoverMode ? (this.turnParent.transform.rotation * -this.leftHandCenterVelocityTracker.GetAverageVelocity(false, 0.15f, false) * d) : Vector3.zero;
			Vector3 b;
			this.FirstHandIteration(this.leftControllerTransform, this.leftHandOffset, this.GetLastLeftHandPosition(), boostVector, this.wasLeftHandSliding, this.wasLeftHandColliding, this.LeftSlipOverriddenToMax(), out b, ref this.leftHandSlipPercentage, ref this.isLeftHandSliding, ref this.leftHandSlideNormal, ref this.isLeftHandColliding, ref this.leftHandMaterialTouchIndex, ref this.leftHandSurfaceOverride, this.leftHandHolding, this.lastMovingSurfaceContact == GTPlayer.MovingSurfaceContactPoint.LEFT);
			this.isLeftHandColliding = (this.isLeftHandColliding && this.controllerState.LeftValid);
			this.isLeftHandSliding = (this.isLeftHandSliding && this.controllerState.LeftValid);
			RaycastHit raycastHit = this.lastHitInfoHand;
			Vector3 boostVector2 = this.enableHoverMode ? (this.turnParent.transform.rotation * -this.rightHandCenterVelocityTracker.GetAverageVelocity(false, 0.15f, false) * d) : Vector3.zero;
			Vector3 b2;
			this.FirstHandIteration(this.rightControllerTransform, this.rightHandOffset, this.GetLastRightHandPosition(), boostVector2, this.wasRightHandSliding, this.wasRightHandColliding, this.RightSlipOverriddenToMax(), out b2, ref this.rightHandSlipPercentage, ref this.isRightHandSliding, ref this.rightHandSlideNormal, ref this.isRightHandColliding, ref this.rightHandMaterialTouchIndex, ref this.rightHandSurfaceOverride, this.rightHandHolding, this.lastMovingSurfaceContact == GTPlayer.MovingSurfaceContactPoint.RIGHT);
			this.isRightHandColliding = (this.isRightHandColliding && this.controllerState.RightValid);
			this.isRightHandSliding = (this.isRightHandSliding && this.controllerState.RightValid);
			this.touchPoints = 0;
			Vector3 vector3 = Vector3.zero;
			if (this.isLeftHandColliding || this.wasLeftHandColliding)
			{
				if (this.leftHandSurfaceOverride && this.leftHandSurfaceOverride.disablePushBackEffect)
				{
					vector3 += Vector3.zero;
				}
				else
				{
					vector3 += b;
				}
				this.touchPoints++;
			}
			if (this.isRightHandColliding || this.wasRightHandColliding)
			{
				if (this.rightHandSurfaceOverride && this.rightHandSurfaceOverride.disablePushBackEffect)
				{
					vector3 += Vector3.zero;
				}
				else
				{
					vector3 += b2;
				}
				this.touchPoints++;
			}
			if (this.touchPoints != 0)
			{
				vector3 /= (float)this.touchPoints;
			}
			if (this.lastMovingSurfaceContact == GTPlayer.MovingSurfaceContactPoint.RIGHT || this.lastMovingSurfaceContact == GTPlayer.MovingSurfaceContactPoint.LEFT)
			{
				vector3 += this.movingSurfaceOffset;
			}
			else if (this.lastMovingSurfaceContact == GTPlayer.MovingSurfaceContactPoint.BODY)
			{
				Vector3 b3 = this.lastHeadPosition + this.movingSurfaceOffset - this.headCollider.transform.position;
				vector3 += b3;
			}
			if (!this.MaxSphereSizeForNoOverlap(this.headCollider.radius * 0.9f * this.scale, this.lastHeadPosition, true, out this.maxSphereSize1) && !this.CrazyCheck2(this.headCollider.radius * 0.9f * 0.75f * this.scale, this.lastHeadPosition))
			{
				this.lastHeadPosition = this.lastOpenHeadPosition;
			}
			Vector3 a2;
			float num2;
			if (this.IterativeCollisionSphereCast(this.lastHeadPosition, this.headCollider.radius * 0.9f * this.scale, this.headCollider.transform.position + vector3 - this.lastHeadPosition, Vector3.zero, out a2, false, out num2, out this.junkHit, true))
			{
				vector3 = a2 - this.headCollider.transform.position;
			}
			if (!this.MaxSphereSizeForNoOverlap(this.headCollider.radius * 0.9f * this.scale, this.lastHeadPosition + vector3, true, out this.maxSphereSize1) || !this.CrazyCheck2(this.headCollider.radius * 0.9f * 0.75f * this.scale, this.lastHeadPosition + vector3))
			{
				this.lastHeadPosition = this.lastOpenHeadPosition;
				vector3 = this.lastHeadPosition - this.headCollider.transform.position;
			}
			else if (this.headCollider.radius * 0.9f * 0.825f * this.scale < this.maxSphereSize1)
			{
				this.lastOpenHeadPosition = this.headCollider.transform.position + vector3;
			}
			if (vector3 != Vector3.zero)
			{
				base.transform.position += vector3;
			}
			if (this.lastMovingSurfaceContact != GTPlayer.MovingSurfaceContactPoint.NONE && quaternion != Quaternion.identity && !this.isClimbing && !this.rightHandHolding && !this.leftHandHolding)
			{
				this.RotateWithSurface(quaternion, pivot);
			}
			this.lastHeadPosition = this.headCollider.transform.position;
			this.areBothTouching = ((!this.isLeftHandColliding && !this.wasLeftHandColliding) || (!this.isRightHandColliding && !this.wasRightHandColliding));
			Vector3 vector4 = this.FinalHandPosition(this.leftControllerTransform, this.leftHandOffset, this.GetLastLeftHandPosition(), boostVector, this.areBothTouching, this.isLeftHandColliding, out this.isLeftHandColliding, this.isLeftHandSliding, out this.isLeftHandSliding, this.leftHandMaterialTouchIndex, out this.leftHandMaterialTouchIndex, this.leftHandSurfaceOverride, out this.leftHandSurfaceOverride, this.leftHandHolding, ref this.leftHandHitInfo);
			this.isLeftHandColliding = (this.isLeftHandColliding && this.controllerState.LeftValid);
			this.isLeftHandSliding = (this.isLeftHandSliding && this.controllerState.LeftValid);
			raycastHit = this.lastHitInfoHand;
			Vector3 vector5 = this.FinalHandPosition(this.rightControllerTransform, this.rightHandOffset, this.GetLastRightHandPosition(), boostVector2, this.areBothTouching, this.isRightHandColliding, out this.isRightHandColliding, this.isRightHandSliding, out this.isRightHandSliding, this.rightHandMaterialTouchIndex, out this.rightHandMaterialTouchIndex, this.rightHandSurfaceOverride, out this.rightHandSurfaceOverride, this.rightHandHolding, ref this.rightHandHitInfo);
			this.isRightHandColliding = (this.isRightHandColliding && this.controllerState.RightValid);
			this.isRightHandSliding = (this.isRightHandSliding && this.controllerState.RightValid);
			Vector3 b4 = this.lastPosition;
			GTPlayer.MovingSurfaceContactPoint movingSurfaceContactPoint = GTPlayer.MovingSurfaceContactPoint.NONE;
			int num3 = -1;
			int num4 = -1;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = this.isRightHandColliding && this.IsTouchingMovingSurface(this.GetLastRightHandPosition(), this.lastHitInfoHand, out num3, out flag, out flag2);
			if (flag4 && !flag)
			{
				movingSurfaceContactPoint = GTPlayer.MovingSurfaceContactPoint.RIGHT;
				this.lastMovingSurfaceHit = this.lastHitInfoHand;
			}
			else
			{
				bool flag5 = false;
				BuilderPiece builderPiece = flag4 ? this.lastMonkeBlock : null;
				if (this.isLeftHandColliding && this.IsTouchingMovingSurface(this.GetLastLeftHandPosition(), raycastHit, out num4, out flag5, out flag3))
				{
					if (flag5 && flag2 == flag3)
					{
						if (flag && num4.Equals(num3) && (double)Vector3.Dot(raycastHit.point - this.GetLastLeftHandPosition(), this.lastHitInfoHand.point - this.GetLastRightHandPosition()) < 0.3)
						{
							movingSurfaceContactPoint = GTPlayer.MovingSurfaceContactPoint.RIGHT;
							this.lastMovingSurfaceHit = this.lastHitInfoHand;
							this.lastMonkeBlock = builderPiece;
						}
					}
					else
					{
						movingSurfaceContactPoint = GTPlayer.MovingSurfaceContactPoint.LEFT;
						this.lastMovingSurfaceHit = raycastHit;
					}
				}
			}
			this.StoreVelocities();
			if (this.InWater)
			{
				PlayerGameEvents.PlayerSwam((this.lastPosition - b4).magnitude, this.currentVelocity.magnitude);
			}
			else
			{
				PlayerGameEvents.PlayerMoved((this.lastPosition - b4).magnitude, this.currentVelocity.magnitude);
			}
			this.didAJump = false;
			bool flag6 = this.exitMovingSurface;
			this.exitMovingSurface = false;
			if (this.LeftSlipOverriddenToMax() && this.RightSlipOverriddenToMax())
			{
				this.didAJump = true;
				this.exitMovingSurface = true;
			}
			else if (this.isRightHandSliding || this.isLeftHandSliding)
			{
				this.slideAverageNormal = Vector3.zero;
				this.touchPoints = 0;
				this.averageSlipPercentage = 0f;
				if (this.isLeftHandSliding)
				{
					this.slideAverageNormal += this.leftHandSlideNormal.normalized;
					this.averageSlipPercentage += this.leftHandSlipPercentage;
					this.touchPoints++;
				}
				if (this.isRightHandSliding)
				{
					this.slideAverageNormal += this.rightHandSlideNormal.normalized;
					this.averageSlipPercentage += this.rightHandSlipPercentage;
					this.touchPoints++;
				}
				this.slideAverageNormal = this.slideAverageNormal.normalized;
				this.averageSlipPercentage /= (float)this.touchPoints;
				if (this.touchPoints == 1)
				{
					this.surfaceDirection = (this.isRightHandSliding ? Vector3.ProjectOnPlane(this.rightControllerTransform.forward, this.rightHandSlideNormal) : Vector3.ProjectOnPlane(this.leftControllerTransform.forward, this.leftHandSlideNormal));
					if (Vector3.Dot(this.slideVelocity, this.surfaceDirection) > 0f)
					{
						this.slideVelocity = Vector3.Project(this.slideVelocity, Vector3.Slerp(this.slideVelocity, this.surfaceDirection.normalized * this.slideVelocity.magnitude, this.slideControl));
					}
					else
					{
						this.slideVelocity = Vector3.Project(this.slideVelocity, Vector3.Slerp(this.slideVelocity, -this.surfaceDirection.normalized * this.slideVelocity.magnitude, this.slideControl));
					}
				}
				if (!this.wasLeftHandSliding && !this.wasRightHandSliding)
				{
					this.slideVelocity = ((Vector3.Dot(this.playerRigidBody.velocity, this.slideAverageNormal) <= 0f) ? Vector3.ProjectOnPlane(this.playerRigidBody.velocity, this.slideAverageNormal) : this.playerRigidBody.velocity);
				}
				else
				{
					this.slideVelocity = ((Vector3.Dot(this.slideVelocity, this.slideAverageNormal) <= 0f) ? Vector3.ProjectOnPlane(this.slideVelocity, this.slideAverageNormal) : this.slideVelocity);
				}
				this.slideVelocity = this.slideVelocity.normalized * Mathf.Min(this.slideVelocity.magnitude, Mathf.Max(0.5f, this.averagedVelocity.magnitude * 2f));
				this.playerRigidBody.velocity = Vector3.zero;
			}
			else if (this.isLeftHandColliding || this.isRightHandColliding)
			{
				if (!this.turnedThisFrame)
				{
					this.playerRigidBody.velocity = Vector3.zero;
				}
				else
				{
					this.playerRigidBody.velocity = this.playerRigidBody.velocity.normalized * Mathf.Min(2f, this.playerRigidBody.velocity.magnitude);
				}
			}
			else if (this.wasLeftHandSliding || this.wasRightHandSliding)
			{
				this.playerRigidBody.velocity = ((Vector3.Dot(this.slideVelocity, this.slideAverageNormal) <= 0f) ? Vector3.ProjectOnPlane(this.slideVelocity, this.slideAverageNormal) : this.slideVelocity);
			}
			if ((this.isRightHandColliding || this.isLeftHandColliding) && !this.disableMovement && !this.turnedThisFrame && !this.didAJump)
			{
				if (this.isRightHandSliding || this.isLeftHandSliding)
				{
					if (Vector3.Project(this.averagedVelocity, this.slideAverageNormal).magnitude > this.slideVelocityLimit * this.scale && Vector3.Dot(this.averagedVelocity, this.slideAverageNormal) > 0f && Vector3.Project(this.averagedVelocity, this.slideAverageNormal).magnitude > Vector3.Project(this.slideVelocity, this.slideAverageNormal).magnitude)
					{
						this.isLeftHandSliding = false;
						this.isRightHandSliding = false;
						this.didAJump = true;
						float num5 = this.ApplyNativeScaleAdjustment(Mathf.Min(this.maxJumpSpeed * this.ExtraVelMaxMultiplier(), this.jumpMultiplier * this.ExtraVelMultiplier() * Vector3.Project(this.averagedVelocity, this.slideAverageNormal).magnitude));
						this.playerRigidBody.velocity = num5 * this.slideAverageNormal.normalized + Vector3.ProjectOnPlane(this.slideVelocity, this.slideAverageNormal);
						if (num5 > this.slideVelocityLimit * this.scale * this.exitMovingSurfaceThreshold)
						{
							this.exitMovingSurface = true;
						}
					}
				}
				else if (this.averagedVelocity.magnitude > this.velocityLimit * this.scale)
				{
					float num6 = (this.InWater && this.CurrentWaterVolume != null) ? this.liquidPropertiesList[(int)this.CurrentWaterVolume.LiquidType].surfaceJumpFactor : 1f;
					float num7 = this.ApplyNativeScaleAdjustment(this.enableHoverMode ? Mathf.Min(this.hoverMaxPaddleSpeed, this.averagedVelocity.magnitude) : Mathf.Min(this.maxJumpSpeed * this.ExtraVelMaxMultiplier(), this.jumpMultiplier * this.ExtraVelMultiplier() * num6 * this.averagedVelocity.magnitude));
					Vector3 vector6 = num7 * this.averagedVelocity.normalized;
					this.didAJump = true;
					this.playerRigidBody.velocity = vector6;
					if (this.InWater)
					{
						this.swimmingVelocity += vector6 * this.swimmingParams.underwaterJumpsAsSwimVelocityFactor;
					}
					if (num7 > this.velocityLimit * this.scale * this.exitMovingSurfaceThreshold)
					{
						this.exitMovingSurface = true;
					}
				}
			}
			this.stuckHandsCheckLateUpdate(ref vector4, ref vector5);
			if (this.lastPlatformTouched != null && this.currentPlatform == null)
			{
				if (!this.playerRigidBody.isKinematic)
				{
					this.playerRigidBody.velocity += this.refMovement / this.calcDeltaTime;
				}
				this.refMovement = Vector3.zero;
			}
			if (this.lastMovingSurfaceContact == GTPlayer.MovingSurfaceContactPoint.NONE)
			{
				if (!this.playerRigidBody.isKinematic)
				{
					this.playerRigidBody.velocity += this.lastMovingSurfaceVelocity;
				}
				this.lastMovingSurfaceVelocity = Vector3.zero;
			}
			if (this.enableHoverMode)
			{
				this.HoverboardLateUpdate();
			}
			else
			{
				this.hasHoverPoint = false;
			}
			Vector3 vector7 = Vector3.zero;
			float a3 = 0f;
			Vector3 b5;
			if (this.GetSwimmingVelocityForHand(this.lastLeftHandPosition, vector4, this.leftControllerTransform.right, this.calcDeltaTime, ref this.leftHandWaterVolume, ref this.leftHandWaterSurface, out b5) && !this.turnedThisFrame)
			{
				a3 = Mathf.InverseLerp(0f, 0.2f, b5.magnitude) * this.swimmingParams.swimmingHapticsStrength;
				vector7 += b5;
			}
			float a4 = 0f;
			Vector3 b6;
			if (this.GetSwimmingVelocityForHand(this.lastRightHandPosition, vector5, -this.rightControllerTransform.right, this.calcDeltaTime, ref this.rightHandWaterVolume, ref this.rightHandWaterSurface, out b6) && !this.turnedThisFrame)
			{
				a4 = Mathf.InverseLerp(0f, 0.15f, b6.magnitude) * this.swimmingParams.swimmingHapticsStrength;
				vector7 += b6;
			}
			Vector3 vector8 = Vector3.zero;
			Vector3 b7;
			if (this.swimmingParams.allowWaterSurfaceJumps && time - this.lastWaterSurfaceJumpTimeLeft > this.waterSurfaceJumpCooldown && this.CheckWaterSurfaceJump(this.lastLeftHandPosition, vector4, this.leftControllerTransform.right, this.leftHandCenterVelocityTracker.GetAverageVelocity(false, 0.1f, false) * this.scale, this.swimmingParams, this.leftHandWaterVolume, this.leftHandWaterSurface, out b7))
			{
				if (time - this.lastWaterSurfaceJumpTimeRight > this.waterSurfaceJumpCooldown)
				{
					vector8 += b7;
				}
				this.lastWaterSurfaceJumpTimeLeft = Time.time;
				GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tapHapticStrength, GorillaTagger.Instance.tapHapticDuration);
			}
			Vector3 b8;
			if (this.swimmingParams.allowWaterSurfaceJumps && time - this.lastWaterSurfaceJumpTimeRight > this.waterSurfaceJumpCooldown && this.CheckWaterSurfaceJump(this.lastRightHandPosition, vector5, -this.rightControllerTransform.right, this.rightHandCenterVelocityTracker.GetAverageVelocity(false, 0.1f, false) * this.scale, this.swimmingParams, this.rightHandWaterVolume, this.rightHandWaterSurface, out b8))
			{
				if (time - this.lastWaterSurfaceJumpTimeLeft > this.waterSurfaceJumpCooldown)
				{
					vector8 += b8;
				}
				this.lastWaterSurfaceJumpTimeRight = Time.time;
				GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tapHapticStrength, GorillaTagger.Instance.tapHapticDuration);
			}
			vector8 = Vector3.ClampMagnitude(vector8, this.swimmingParams.waterSurfaceJumpMaxSpeed * this.scale);
			float num8 = Mathf.Max(a3, this.leftHandNonDiveHapticsAmount);
			if (num8 > 0.001f && time - this.lastWaterSurfaceJumpTimeLeft > GorillaTagger.Instance.tapHapticDuration)
			{
				GorillaTagger.Instance.DoVibration(XRNode.LeftHand, num8, this.calcDeltaTime);
			}
			float num9 = Mathf.Max(a4, this.rightHandNonDiveHapticsAmount);
			if (num9 > 0.001f && time - this.lastWaterSurfaceJumpTimeRight > GorillaTagger.Instance.tapHapticDuration)
			{
				GorillaTagger.Instance.DoVibration(XRNode.RightHand, num9, this.calcDeltaTime);
			}
			if (!this.disableMovement)
			{
				this.swimmingVelocity += vector7;
				if (!this.playerRigidBody.isKinematic)
				{
					this.playerRigidBody.velocity += vector7 + vector8;
				}
			}
			else
			{
				this.swimmingVelocity = Vector3.zero;
			}
			if (GorillaGameManager.instance is GorillaFreezeTagManager)
			{
				if (!this.IsFrozen || !this.primaryButtonPressed)
				{
					this.IsBodySliding = false;
					this.lastSlopeDirection = Vector3.zero;
					if (this.bodyTouchedSurfaces.Count > 0)
					{
						foreach (KeyValuePair<GameObject, PhysicMaterial> keyValuePair in this.bodyTouchedSurfaces)
						{
							MeshCollider meshCollider;
							if (keyValuePair.Key.TryGetComponent<MeshCollider>(out meshCollider))
							{
								meshCollider.material = keyValuePair.Value;
							}
						}
						this.bodyTouchedSurfaces.Clear();
					}
				}
				else if (this.BodyOnGround && this.primaryButtonPressed)
				{
					float y = this.bodyInitialHeight / 2f - this.bodyInitialRadius;
					RaycastHit raycastHit2;
					if (Physics.SphereCast(this.bodyCollider.transform.position - new Vector3(0f, y, 0f), this.bodyInitialRadius - 0.01f, Vector3.down, out raycastHit2, 1f, ~LayerMask.GetMask(new string[]
					{
						"Gorilla Body Collider",
						"GorillaInteractable"
					}), QueryTriggerInteraction.Ignore))
					{
						this.IsBodySliding = true;
						MeshCollider meshCollider2;
						if (!this.bodyTouchedSurfaces.ContainsKey(raycastHit2.transform.gameObject) && raycastHit2.transform.gameObject.TryGetComponent<MeshCollider>(out meshCollider2))
						{
							this.bodyTouchedSurfaces.Add(raycastHit2.transform.gameObject, meshCollider2.material);
							raycastHit2.transform.gameObject.GetComponent<MeshCollider>().material = this.slipperyMaterial;
						}
					}
				}
				else
				{
					this.IsBodySliding = false;
					this.lastSlopeDirection = Vector3.zero;
				}
			}
			else
			{
				this.IsBodySliding = false;
				if (this.bodyTouchedSurfaces.Count > 0)
				{
					foreach (KeyValuePair<GameObject, PhysicMaterial> keyValuePair2 in this.bodyTouchedSurfaces)
					{
						MeshCollider meshCollider3;
						if (keyValuePair2.Key.TryGetComponent<MeshCollider>(out meshCollider3))
						{
							meshCollider3.material = keyValuePair2.Value;
						}
					}
					this.bodyTouchedSurfaces.Clear();
				}
			}
			this.leftHandFollower.position = vector4;
			this.rightHandFollower.position = vector5;
			this.leftHandFollower.rotation = this.leftControllerTransform.rotation * this.leftHandRotOffset;
			this.rightHandFollower.rotation = this.rightControllerTransform.rotation * this.rightHandRotOffset;
			this.wasLeftHandColliding = this.isLeftHandColliding;
			this.wasRightHandColliding = this.isRightHandColliding;
			this.wasLeftHandSliding = this.isLeftHandSliding;
			this.wasRightHandSliding = this.isRightHandSliding;
			if ((this.isLeftHandColliding && !this.isLeftHandSliding) || (this.isRightHandColliding && !this.isRightHandSliding))
			{
				this.lastTouchedGroundTimestamp = Time.time;
			}
			this.degreesTurnedThisFrame = 0f;
			this.lastPlatformTouched = this.currentPlatform;
			this.currentPlatform = null;
			this.lastMovingSurfaceVelocity = vector;
			this.lastLeftHandPosition = vector4;
			this.lastRightHandPosition = vector5;
			Vector3 vector9;
			if (GTPlayer.ComputeLocalHitPoint(this.lastHitInfoHand, out vector9))
			{
				this.lastFrameHasValidTouchPos = true;
				this.lastFrameTouchPosLocal = vector9;
				this.lastFrameTouchPosWorld = this.lastHitInfoHand.point;
			}
			else
			{
				this.lastFrameHasValidTouchPos = false;
				this.lastFrameTouchPosLocal = Vector3.zero;
				this.lastFrameTouchPosWorld = Vector3.zero;
			}
			this.lastRigidbodyPosition = this.playerRigidBody.transform.position;
			RaycastHit raycastHit3 = default(RaycastHit);
			this.BodyCollider();
			if (this.bodyHitInfo.collider != null)
			{
				this.wasBodyOnGround = true;
				raycastHit3 = this.bodyHitInfo;
			}
			else if (movingSurfaceContactPoint == GTPlayer.MovingSurfaceContactPoint.NONE && this.bodyCollider.gameObject.activeSelf)
			{
				bool flag7 = false;
				this.ClearRaycasthitBuffer(ref this.rayCastNonAllocColliders);
				Vector3 origin = this.PositionWithOffset(this.headCollider.transform, this.bodyOffset) + (this.bodyInitialHeight * this.scale - this.bodyMaxRadius) * Vector3.down;
				this.bufferCount = Physics.SphereCastNonAlloc(origin, this.bodyMaxRadius, Vector3.down, this.rayCastNonAllocColliders, this.minimumRaycastDistance * this.scale, this.locomotionEnabledLayers.value);
				if (this.bufferCount > 0)
				{
					this.tempHitInfo = this.rayCastNonAllocColliders[0];
					for (int i = 0; i < this.bufferCount; i++)
					{
						if (this.tempHitInfo.distance > 0f && (!flag7 || this.rayCastNonAllocColliders[i].distance < this.tempHitInfo.distance))
						{
							flag7 = true;
							raycastHit3 = this.rayCastNonAllocColliders[i];
						}
					}
				}
				this.wasBodyOnGround = flag7;
			}
			int num10 = -1;
			bool flag8 = false;
			bool flag9;
			if (this.wasBodyOnGround && movingSurfaceContactPoint == GTPlayer.MovingSurfaceContactPoint.NONE && this.IsTouchingMovingSurface(this.PositionWithOffset(this.headCollider.transform, this.bodyOffset), raycastHit3, out num10, out flag9, out flag8) && !flag9)
			{
				movingSurfaceContactPoint = GTPlayer.MovingSurfaceContactPoint.BODY;
				this.lastMovingSurfaceHit = raycastHit3;
			}
			Vector3 vector10;
			if (movingSurfaceContactPoint != GTPlayer.MovingSurfaceContactPoint.NONE && GTPlayer.ComputeLocalHitPoint(this.lastMovingSurfaceHit, out vector10))
			{
				this.lastMovingSurfaceTouchLocal = vector10;
				this.lastMovingSurfaceTouchWorld = this.lastMovingSurfaceHit.point;
				this.lastMovingSurfaceRot = this.lastMovingSurfaceHit.collider.transform.rotation;
				this.lastAttachedToMovingSurfaceFrame = Time.frameCount;
			}
			else
			{
				movingSurfaceContactPoint = GTPlayer.MovingSurfaceContactPoint.NONE;
				this.lastMovingSurfaceTouchLocal = Vector3.zero;
				this.lastMovingSurfaceTouchWorld = Vector3.zero;
				this.lastMovingSurfaceRot = Quaternion.identity;
			}
			Vector3 position2 = this.lastMovingSurfaceTouchWorld;
			int num11 = -1;
			bool flag10 = false;
			switch (movingSurfaceContactPoint)
			{
			case GTPlayer.MovingSurfaceContactPoint.NONE:
				if (flag6)
				{
					this.exitMovingSurface = true;
				}
				num11 = -1;
				break;
			case GTPlayer.MovingSurfaceContactPoint.RIGHT:
				num11 = num3;
				flag10 = flag2;
				position2 = GorillaTagger.Instance.offlineVRRig.rightHandTransform.position;
				break;
			case GTPlayer.MovingSurfaceContactPoint.LEFT:
				num11 = num4;
				flag10 = flag3;
				position2 = GorillaTagger.Instance.offlineVRRig.leftHandTransform.position;
				break;
			case GTPlayer.MovingSurfaceContactPoint.BODY:
				num11 = num10;
				flag10 = flag8;
				position2 = GorillaTagger.Instance.offlineVRRig.bodyTransform.position;
				break;
			}
			if (!flag10)
			{
				this.lastMonkeBlock = null;
			}
			if (num11 != this.lastMovingSurfaceID || this.lastMovingSurfaceContact != movingSurfaceContactPoint || flag10 != this.wasMovingSurfaceMonkeBlock)
			{
				if (num11 == -1)
				{
					if (Time.frameCount - this.lastAttachedToMovingSurfaceFrame > 3)
					{
						VRRig.DetachLocalPlayerFromMovingSurface();
						this.lastMovingSurfaceID = -1;
					}
				}
				else if (flag10)
				{
					if (this.lastMonkeBlock != null)
					{
						VRRig.AttachLocalPlayerToMovingSurface(num11, movingSurfaceContactPoint == GTPlayer.MovingSurfaceContactPoint.LEFT, movingSurfaceContactPoint == GTPlayer.MovingSurfaceContactPoint.BODY, this.lastMonkeBlock.transform.InverseTransformPoint(position2), flag10);
						this.lastMovingSurfaceID = num11;
					}
					else
					{
						VRRig.DetachLocalPlayerFromMovingSurface();
						this.lastMovingSurfaceID = -1;
					}
				}
				else if (MovingSurfaceManager.instance != null)
				{
					MovingSurface movingSurface;
					if (MovingSurfaceManager.instance.TryGetMovingSurface(num11, out movingSurface))
					{
						VRRig.AttachLocalPlayerToMovingSurface(num11, movingSurfaceContactPoint == GTPlayer.MovingSurfaceContactPoint.LEFT, movingSurfaceContactPoint == GTPlayer.MovingSurfaceContactPoint.BODY, movingSurface.transform.InverseTransformPoint(position2), flag10);
						this.lastMovingSurfaceID = num11;
					}
					else
					{
						VRRig.DetachLocalPlayerFromMovingSurface();
						this.lastMovingSurfaceID = -1;
					}
				}
				else
				{
					VRRig.DetachLocalPlayerFromMovingSurface();
					this.lastMovingSurfaceID = -1;
				}
			}
			if (this.lastMovingSurfaceContact == GTPlayer.MovingSurfaceContactPoint.NONE && movingSurfaceContactPoint != GTPlayer.MovingSurfaceContactPoint.NONE)
			{
				this.SetPlayerVelocity(Vector3.zero);
			}
			this.lastMovingSurfaceContact = movingSurfaceContactPoint;
			this.wasMovingSurfaceMonkeBlock = flag10;
			if (this.activeSizeChangerSettings != null)
			{
				if (this.activeSizeChangerSettings.ExpireOnDistance > 0f && Vector3.Distance(base.transform.position, this.activeSizeChangerSettings.WorldPosition) > this.activeSizeChangerSettings.ExpireOnDistance)
				{
					this.SetNativeScale(null);
				}
				if (this.activeSizeChangerSettings.ExpireAfterSeconds > 0f && Time.time - this.activeSizeChangerSettings.ActivationTime > this.activeSizeChangerSettings.ExpireAfterSeconds)
				{
					this.SetNativeScale(null);
				}
			}
		}

		// Token: 0x060047D9 RID: 18393 RVA: 0x00158B7C File Offset: 0x00156D7C
		private float ApplyNativeScaleAdjustment(float adjustedMagnitude)
		{
			if (this.nativeScale > 0f && this.nativeScale != 1f)
			{
				return adjustedMagnitude *= this.nativeScaleMagnitudeAdjustmentFactor.Evaluate(this.nativeScale);
			}
			return adjustedMagnitude;
		}

		// Token: 0x060047DA RID: 18394 RVA: 0x00158BB0 File Offset: 0x00156DB0
		private float RotateWithSurface(Quaternion rotationDelta, Vector3 pivot)
		{
			Quaternion quaternion;
			Quaternion quaternion2;
			QuaternionUtil.DecomposeSwingTwist(rotationDelta, Vector3.up, out quaternion, out quaternion2);
			float num = quaternion2.eulerAngles.y;
			if (num > 270f)
			{
				num -= 360f;
			}
			else if (num > 90f)
			{
				num -= 180f;
			}
			if (Mathf.Abs(num) < 90f * this.calcDeltaTime)
			{
				this.turnParent.transform.RotateAround(pivot, base.transform.up, num);
				return num;
			}
			return 0f;
		}

		// Token: 0x060047DB RID: 18395 RVA: 0x00158C34 File Offset: 0x00156E34
		private void stuckHandsCheckFixedUpdate()
		{
			this.stuckLeft = (!this.controllerState.LeftValid || (this.isLeftHandColliding && (this.GetCurrentLeftHandPosition() - this.GetLastLeftHandPosition()).magnitude > this.unStickDistance * this.scale && !Physics.Raycast(this.headCollider.transform.position, (this.GetCurrentLeftHandPosition() - this.headCollider.transform.position).normalized, (this.GetCurrentLeftHandPosition() - this.headCollider.transform.position).magnitude, this.locomotionEnabledLayers.value)));
			this.stuckRight = (!this.controllerState.RightValid || (this.isRightHandColliding && (this.GetCurrentRightHandPosition() - this.GetLastRightHandPosition()).magnitude > this.unStickDistance * this.scale && !Physics.Raycast(this.headCollider.transform.position, (this.GetCurrentRightHandPosition() - this.headCollider.transform.position).normalized, (this.GetCurrentRightHandPosition() - this.headCollider.transform.position).magnitude, this.locomotionEnabledLayers.value)));
		}

		// Token: 0x060047DC RID: 18396 RVA: 0x00158DB4 File Offset: 0x00156FB4
		private void stuckHandsCheckLateUpdate(ref Vector3 finalLeftHandPosition, ref Vector3 finalRightHandPosition)
		{
			if (this.stuckLeft)
			{
				finalLeftHandPosition = this.GetCurrentLeftHandPosition();
				this.stuckLeft = (this.isLeftHandColliding = false);
			}
			if (this.stuckRight)
			{
				finalRightHandPosition = this.GetCurrentRightHandPosition();
				this.stuckRight = (this.isRightHandColliding = false);
			}
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x00158E0C File Offset: 0x0015700C
		private void handleClimbing(float deltaTime)
		{
			if (this.isClimbing && (this.inOverlay || this.climbHelper == null || this.currentClimbable == null || !this.currentClimbable.isActiveAndEnabled))
			{
				this.EndClimbing(this.currentClimber, false, false);
			}
			Vector3 vector = Vector3.zero;
			if (this.isClimbing && (this.currentClimber.transform.position - this.climbHelper.position).magnitude > 1f)
			{
				this.EndClimbing(this.currentClimber, false, false);
			}
			if (this.isClimbing)
			{
				this.playerRigidBody.velocity = Vector3.zero;
				this.climbHelper.localPosition = Vector3.MoveTowards(this.climbHelper.localPosition, this.climbHelperTargetPos, deltaTime * 12f);
				vector = this.currentClimber.transform.position - this.climbHelper.position;
				vector = ((vector.sqrMagnitude > this.maxArmLength * this.maxArmLength) ? (vector.normalized * this.maxArmLength) : vector);
				if (this.isClimbableMoving)
				{
					Quaternion rotationDelta = this.currentClimbable.transform.rotation * Quaternion.Inverse(this.lastClimbableRotation);
					this.RotateWithSurface(rotationDelta, this.currentClimber.handRoot.position);
					this.lastClimbableRotation = this.currentClimbable.transform.rotation;
				}
				this.playerRigidBody.MovePosition(this.playerRigidBody.position - vector);
				if (this.currentSwing)
				{
					this.currentSwing.lastGrabTime = Time.time;
				}
			}
		}

		// Token: 0x060047DE RID: 18398 RVA: 0x00158FCC File Offset: 0x001571CC
		private Vector3 FirstHandIteration(Transform handTransform, Vector3 handOffset, Vector3 lastHandPosition, Vector3 boostVector, bool wasHandSlide, bool wasHandTouching, bool fullSlideOverride, out Vector3 pushDisplacement, ref float handSlipPercentage, ref bool handSlide, ref Vector3 slideNormal, ref bool handColliding, ref int materialTouchIndex, ref GorillaSurfaceOverride touchedOverride, bool skipCollisionChecks, bool hitMovingSurface)
		{
			Vector3 vector = this.GetCurrentHandPosition(handTransform, handOffset) + this.movingSurfaceOffset;
			Vector3 result = vector;
			Vector3 a = vector - lastHandPosition;
			if (!this.didAJump && wasHandSlide && Vector3.Dot(slideNormal, Vector3.up) > 0f)
			{
				a += Vector3.Project(-this.slideAverageNormal * this.stickDepth * this.scale, Vector3.down);
			}
			float num = this.minimumRaycastDistance * this.scale;
			if (this.IsFrozen && GorillaGameManager.instance is GorillaFreezeTagManager)
			{
				num = (this.minimumRaycastDistance + VRRig.LocalRig.iceCubeRight.transform.localScale.y / 2f) * this.scale;
			}
			Vector3 vector2 = Vector3.zero;
			if (hitMovingSurface && !this.exitMovingSurface)
			{
				vector2 = Vector3.Project(-this.lastMovingSurfaceHit.normal * (this.stickDepth * this.scale), Vector3.down);
				if (this.scale < 0.5f)
				{
					Vector3 normalized = this.MovingSurfaceMovement().normalized;
					if (normalized != Vector3.zero)
					{
						float num2 = Vector3.Dot(Vector3.up, normalized);
						if ((double)num2 > 0.9 || (double)num2 < -0.9)
						{
							vector2 *= 6f;
							num *= 1.1f;
						}
					}
				}
			}
			Vector3 vector3;
			float num3;
			if (this.IterativeCollisionSphereCast(lastHandPosition, num, a + vector2, boostVector, out vector3, true, out num3, out this.tempHitInfo, fullSlideOverride) && !skipCollisionChecks && !this.InReportMenu)
			{
				if (wasHandTouching && num3 <= this.defaultSlideFactor && !boostVector.IsLongerThan(0f))
				{
					result = lastHandPosition;
					pushDisplacement = lastHandPosition - vector;
				}
				else
				{
					result = vector3;
					pushDisplacement = vector3 - vector;
				}
				handSlipPercentage = num3;
				handSlide = (num3 > this.iceThreshold);
				slideNormal = this.tempHitInfo.normal;
				handColliding = true;
				materialTouchIndex = this.currentMaterialIndex;
				touchedOverride = this.currentOverride;
				this.lastHitInfoHand = this.tempHitInfo;
			}
			else
			{
				pushDisplacement = Vector3.zero;
				handSlipPercentage = 0f;
				handSlide = false;
				slideNormal = Vector3.up;
				handColliding = false;
				materialTouchIndex = 0;
				touchedOverride = null;
			}
			return result;
		}

		// Token: 0x060047DF RID: 18399 RVA: 0x00159244 File Offset: 0x00157444
		private Vector3 FinalHandPosition(Transform handTransform, Vector3 handOffset, Vector3 lastHandPosition, Vector3 boostVector, bool bothTouching, bool isHandTouching, out bool handColliding, bool isHandSlide, out bool handSlide, int currentMaterialTouchIndex, out int materialTouchIndex, GorillaSurfaceOverride currentSurface, out GorillaSurfaceOverride touchedOverride, bool skipCollisionChecks, ref RaycastHit hitInfoCopy)
		{
			handColliding = isHandTouching;
			handSlide = isHandSlide;
			materialTouchIndex = currentMaterialTouchIndex;
			touchedOverride = currentSurface;
			Vector3 movementVector = this.GetCurrentHandPosition(handTransform, handOffset) - lastHandPosition;
			float sphereRadius = this.minimumRaycastDistance * this.scale;
			if (this.IsFrozen && GorillaGameManager.instance is GorillaFreezeTagManager)
			{
				sphereRadius = (this.minimumRaycastDistance + VRRig.LocalRig.iceCubeRight.transform.localScale.y / 2f) * this.scale;
			}
			Vector3 result;
			float num;
			if (this.IterativeCollisionSphereCast(lastHandPosition, sphereRadius, movementVector, boostVector, out result, bothTouching, out num, out this.junkHit, false) && !skipCollisionChecks)
			{
				handColliding = true;
				handSlide = (num > this.iceThreshold);
				materialTouchIndex = this.currentMaterialIndex;
				touchedOverride = this.currentOverride;
				this.lastHitInfoHand = this.junkHit;
				hitInfoCopy = this.junkHit;
				return result;
			}
			return this.GetCurrentHandPosition(handTransform, handOffset);
		}

		// Token: 0x060047E0 RID: 18400 RVA: 0x00159328 File Offset: 0x00157528
		private bool IterativeCollisionSphereCast(Vector3 startPosition, float sphereRadius, Vector3 movementVector, Vector3 boostVector, out Vector3 endPosition, bool singleHand, out float slipPercentage, out RaycastHit iterativeHitInfo, bool fullSlide)
		{
			slipPercentage = this.defaultSlideFactor;
			if (!this.CollisionsSphereCast(startPosition, sphereRadius, movementVector, out endPosition, out this.tempIterativeHit))
			{
				iterativeHitInfo = this.tempIterativeHit;
				endPosition = Vector3.zero;
				return false;
			}
			this.firstPosition = endPosition;
			iterativeHitInfo = this.tempIterativeHit;
			this.slideFactor = this.GetSlidePercentage(iterativeHitInfo);
			slipPercentage = ((this.slideFactor != this.defaultSlideFactor) ? this.slideFactor : ((!singleHand) ? this.defaultSlideFactor : 0.001f));
			if (fullSlide)
			{
				slipPercentage = 1f;
			}
			this.movementToProjectedAboveCollisionPlane = Vector3.ProjectOnPlane(startPosition + movementVector - this.firstPosition, iterativeHitInfo.normal) * slipPercentage;
			Vector3 vector = Vector3.zero;
			if (boostVector.IsLongerThan(0f))
			{
				vector = Vector3.ProjectOnPlane(boostVector, iterativeHitInfo.normal);
				this.movementToProjectedAboveCollisionPlane += vector;
				this.CollisionsSphereCast(this.firstPosition, sphereRadius, vector, out endPosition, out this.tempIterativeHit);
				this.firstPosition = endPosition;
			}
			if (this.CollisionsSphereCast(this.firstPosition, sphereRadius, this.movementToProjectedAboveCollisionPlane, out endPosition, out this.tempIterativeHit))
			{
				iterativeHitInfo = this.tempIterativeHit;
				return true;
			}
			if (this.CollisionsSphereCast(this.movementToProjectedAboveCollisionPlane + this.firstPosition, sphereRadius, startPosition + movementVector + vector - (this.movementToProjectedAboveCollisionPlane + this.firstPosition), out endPosition, out this.tempIterativeHit))
			{
				iterativeHitInfo = this.tempIterativeHit;
				return true;
			}
			endPosition = Vector3.zero;
			return false;
		}

		// Token: 0x060047E1 RID: 18401 RVA: 0x001594E4 File Offset: 0x001576E4
		private bool CollisionsSphereCast(Vector3 startPosition, float sphereRadius, Vector3 movementVector, out Vector3 finalPosition, out RaycastHit collisionsHitInfo)
		{
			this.MaxSphereSizeForNoOverlap(sphereRadius, startPosition, false, out this.maxSphereSize1);
			bool flag = false;
			this.ClearRaycasthitBuffer(ref this.rayCastNonAllocColliders);
			this.bufferCount = Physics.SphereCastNonAlloc(startPosition, this.maxSphereSize1, movementVector.normalized, this.rayCastNonAllocColliders, movementVector.magnitude, this.locomotionEnabledLayers.value);
			if (this.bufferCount > 0)
			{
				this.tempHitInfo = this.rayCastNonAllocColliders[0];
				for (int i = 0; i < this.bufferCount; i++)
				{
					if (this.tempHitInfo.distance > 0f && (!flag || this.rayCastNonAllocColliders[i].distance < this.tempHitInfo.distance))
					{
						flag = true;
						this.tempHitInfo = this.rayCastNonAllocColliders[i];
					}
				}
			}
			if (flag)
			{
				collisionsHitInfo = this.tempHitInfo;
				finalPosition = collisionsHitInfo.point + collisionsHitInfo.normal * sphereRadius;
				this.ClearRaycasthitBuffer(ref this.rayCastNonAllocColliders);
				this.bufferCount = Physics.RaycastNonAlloc(startPosition, (finalPosition - startPosition).normalized, this.rayCastNonAllocColliders, (finalPosition - startPosition).magnitude, this.locomotionEnabledLayers.value, QueryTriggerInteraction.Ignore);
				if (this.bufferCount > 0)
				{
					this.tempHitInfo = this.rayCastNonAllocColliders[0];
					for (int j = 0; j < this.bufferCount; j++)
					{
						if (this.rayCastNonAllocColliders[j].distance < this.tempHitInfo.distance)
						{
							this.tempHitInfo = this.rayCastNonAllocColliders[j];
						}
					}
					finalPosition = startPosition + movementVector.normalized * this.tempHitInfo.distance;
				}
				this.MaxSphereSizeForNoOverlap(sphereRadius, finalPosition, false, out this.maxSphereSize2);
				this.ClearRaycasthitBuffer(ref this.rayCastNonAllocColliders);
				this.bufferCount = Physics.SphereCastNonAlloc(startPosition, Mathf.Min(this.maxSphereSize1, this.maxSphereSize2), (finalPosition - startPosition).normalized, this.rayCastNonAllocColliders, (finalPosition - startPosition).magnitude, this.locomotionEnabledLayers.value);
				if (this.bufferCount > 0)
				{
					this.tempHitInfo = this.rayCastNonAllocColliders[0];
					for (int k = 0; k < this.bufferCount; k++)
					{
						if (this.rayCastNonAllocColliders[k].collider != null && this.rayCastNonAllocColliders[k].distance < this.tempHitInfo.distance)
						{
							this.tempHitInfo = this.rayCastNonAllocColliders[k];
						}
					}
					finalPosition = startPosition + this.tempHitInfo.distance * (finalPosition - startPosition).normalized;
					collisionsHitInfo = this.tempHitInfo;
				}
				this.ClearRaycasthitBuffer(ref this.rayCastNonAllocColliders);
				this.bufferCount = Physics.RaycastNonAlloc(startPosition, (finalPosition - startPosition).normalized, this.rayCastNonAllocColliders, (finalPosition - startPosition).magnitude, this.locomotionEnabledLayers.value);
				if (this.bufferCount > 0)
				{
					this.tempHitInfo = this.rayCastNonAllocColliders[0];
					for (int l = 0; l < this.bufferCount; l++)
					{
						if (this.rayCastNonAllocColliders[l].distance < this.tempHitInfo.distance)
						{
							this.tempHitInfo = this.rayCastNonAllocColliders[l];
						}
					}
					collisionsHitInfo = this.tempHitInfo;
					finalPosition = startPosition;
				}
				return true;
			}
			this.ClearRaycasthitBuffer(ref this.rayCastNonAllocColliders);
			this.bufferCount = Physics.RaycastNonAlloc(startPosition, movementVector.normalized, this.rayCastNonAllocColliders, movementVector.magnitude, this.locomotionEnabledLayers.value);
			if (this.bufferCount > 0)
			{
				this.tempHitInfo = this.rayCastNonAllocColliders[0];
				for (int m = 0; m < this.bufferCount; m++)
				{
					if (this.rayCastNonAllocColliders[m].collider != null && this.rayCastNonAllocColliders[m].distance < this.tempHitInfo.distance)
					{
						this.tempHitInfo = this.rayCastNonAllocColliders[m];
					}
				}
				collisionsHitInfo = this.tempHitInfo;
				finalPosition = startPosition;
				return true;
			}
			finalPosition = startPosition + movementVector;
			collisionsHitInfo = default(RaycastHit);
			return false;
		}

		// Token: 0x060047E2 RID: 18402 RVA: 0x001599A2 File Offset: 0x00157BA2
		public bool IsHandTouching(bool forLeftHand)
		{
			if (forLeftHand)
			{
				return this.wasLeftHandColliding;
			}
			return this.wasRightHandColliding;
		}

		// Token: 0x060047E3 RID: 18403 RVA: 0x001599B4 File Offset: 0x00157BB4
		public bool IsHandSliding(bool forLeftHand)
		{
			if (forLeftHand)
			{
				return this.wasLeftHandSliding || this.isLeftHandSliding;
			}
			return this.wasRightHandSliding || this.isRightHandSliding;
		}

		// Token: 0x060047E4 RID: 18404 RVA: 0x001599DC File Offset: 0x00157BDC
		public float GetSlidePercentage(RaycastHit raycastHit)
		{
			this.currentOverride = raycastHit.collider.gameObject.GetComponent<GorillaSurfaceOverride>();
			BasePlatform component = raycastHit.collider.gameObject.GetComponent<BasePlatform>();
			if (component != null)
			{
				this.currentPlatform = component;
			}
			if (this.currentOverride != null)
			{
				if (this.currentOverride.slidePercentageOverride >= 0f)
				{
					return this.currentOverride.slidePercentageOverride;
				}
				this.currentMaterialIndex = this.currentOverride.overrideIndex;
				if (this.IsFrozen && GorillaGameManager.instance is GorillaFreezeTagManager)
				{
					return this.FreezeTagSlidePercentage();
				}
				if (!this.materialData[this.currentMaterialIndex].overrideSlidePercent)
				{
					return this.defaultSlideFactor;
				}
				return this.materialData[this.currentMaterialIndex].slidePercent;
			}
			else
			{
				this.meshCollider = (raycastHit.collider as MeshCollider);
				if (this.meshCollider == null || this.meshCollider.sharedMesh == null || this.meshCollider.convex)
				{
					return this.defaultSlideFactor;
				}
				this.collidedMesh = this.meshCollider.sharedMesh;
				if (!this.meshTrianglesDict.TryGetValue(this.collidedMesh, out this.sharedMeshTris))
				{
					this.sharedMeshTris = this.collidedMesh.triangles;
					this.meshTrianglesDict.Add(this.collidedMesh, (int[])this.sharedMeshTris.Clone());
				}
				this.vertex1 = this.sharedMeshTris[raycastHit.triangleIndex * 3];
				this.vertex2 = this.sharedMeshTris[raycastHit.triangleIndex * 3 + 1];
				this.vertex3 = this.sharedMeshTris[raycastHit.triangleIndex * 3 + 2];
				this.slideRenderer = raycastHit.collider.GetComponent<Renderer>();
				if (this.slideRenderer != null)
				{
					this.slideRenderer.GetSharedMaterials(this.tempMaterialArray);
				}
				else
				{
					this.tempMaterialArray.Clear();
				}
				if (this.tempMaterialArray.Count > 1)
				{
					for (int i = 0; i < this.tempMaterialArray.Count; i++)
					{
						this.collidedMesh.GetTriangles(this.trianglesList, i);
						int j = 0;
						while (j < this.trianglesList.Count)
						{
							if (this.trianglesList[j] == this.vertex1 && this.trianglesList[j + 1] == this.vertex2 && this.trianglesList[j + 2] == this.vertex3)
							{
								this.findMatName = this.tempMaterialArray[i].name;
								if (this.findMatName.EndsWith("Uber"))
								{
									string text = this.findMatName;
									this.findMatName = text.Substring(0, text.Length - 4);
								}
								this.foundMatData = this.materialData.Find((GTPlayer.MaterialData matData) => matData.matName == this.findMatName);
								this.currentMaterialIndex = this.materialData.FindIndex((GTPlayer.MaterialData matData) => matData.matName == this.findMatName);
								if (this.currentMaterialIndex == -1)
								{
									this.currentMaterialIndex = 0;
								}
								if (this.IsFrozen && GorillaGameManager.instance is GorillaFreezeTagManager)
								{
									return this.FreezeTagSlidePercentage();
								}
								if (!this.foundMatData.overrideSlidePercent)
								{
									return this.defaultSlideFactor;
								}
								return this.foundMatData.slidePercent;
							}
							else
							{
								j += 3;
							}
						}
					}
				}
				else if (this.tempMaterialArray.Count > 0)
				{
					this.findMatName = this.tempMaterialArray[0].name;
					if (this.findMatName.EndsWith("Uber"))
					{
						string text = this.findMatName;
						this.findMatName = text.Substring(0, text.Length - 4);
					}
					this.foundMatData = this.materialData.Find((GTPlayer.MaterialData matData) => matData.matName == this.findMatName);
					this.currentMaterialIndex = this.materialData.FindIndex((GTPlayer.MaterialData matData) => matData.matName == this.findMatName);
					if (this.currentMaterialIndex == -1)
					{
						this.currentMaterialIndex = 0;
					}
					if (this.IsFrozen && GorillaGameManager.instance is GorillaFreezeTagManager)
					{
						return this.FreezeTagSlidePercentage();
					}
					if (!this.foundMatData.overrideSlidePercent)
					{
						return this.defaultSlideFactor;
					}
					return this.foundMatData.slidePercent;
				}
				this.currentMaterialIndex = 0;
				return this.defaultSlideFactor;
			}
		}

		// Token: 0x060047E5 RID: 18405 RVA: 0x00159E38 File Offset: 0x00158038
		public bool IsTouchingMovingSurface(Vector3 rayOrigin, RaycastHit raycastHit, out int movingSurfaceId, out bool sideTouch, out bool isMonkeBlock)
		{
			movingSurfaceId = -1;
			sideTouch = false;
			isMonkeBlock = false;
			float num = Vector3.Dot(rayOrigin - raycastHit.point, Vector3.up);
			if (num < -0.3f)
			{
				return false;
			}
			if (num < 0f)
			{
				sideTouch = true;
			}
			if (raycastHit.collider == null)
			{
				return false;
			}
			MovingSurface component = raycastHit.collider.GetComponent<MovingSurface>();
			if (component != null)
			{
				isMonkeBlock = false;
				movingSurfaceId = component.GetID();
				return true;
			}
			if (!BuilderTable.IsLocalPlayerInBuilderZone())
			{
				return false;
			}
			BuilderPiece builderPieceFromCollider = BuilderPiece.GetBuilderPieceFromCollider(raycastHit.collider);
			if (builderPieceFromCollider != null && builderPieceFromCollider.IsPieceMoving())
			{
				isMonkeBlock = true;
				movingSurfaceId = builderPieceFromCollider.pieceId;
				this.lastMonkeBlock = builderPieceFromCollider;
				return true;
			}
			sideTouch = false;
			return false;
		}

		// Token: 0x060047E6 RID: 18406 RVA: 0x00159EF4 File Offset: 0x001580F4
		public void Turn(float degrees)
		{
			Vector3 position = this.headCollider.transform.position;
			bool flag = this.isRightHandColliding || this.rightHandHolding;
			bool flag2 = this.isLeftHandColliding || this.leftHandHolding;
			if (flag != flag2 && flag)
			{
				position = this.rightControllerTransform.position;
			}
			if (flag != flag2 && flag2)
			{
				position = this.leftControllerTransform.position;
			}
			this.turnParent.transform.RotateAround(position, base.transform.up, degrees);
			this.degreesTurnedThisFrame = degrees;
			this.averagedVelocity = Vector3.zero;
			for (int i = 0; i < this.velocityHistory.Length; i++)
			{
				this.velocityHistory[i] = Quaternion.Euler(0f, degrees, 0f) * this.velocityHistory[i];
				this.averagedVelocity += this.velocityHistory[i];
			}
			this.averagedVelocity /= (float)this.velocityHistorySize;
		}

		// Token: 0x060047E7 RID: 18407 RVA: 0x0015A008 File Offset: 0x00158208
		public void BeginClimbing(GorillaClimbable climbable, GorillaHandClimber hand, GorillaClimbableRef climbableRef = null)
		{
			if (this.currentClimber != null)
			{
				this.EndClimbing(this.currentClimber, true, false);
			}
			try
			{
				Action<GorillaHandClimber, GorillaClimbableRef> onBeforeClimb = climbable.onBeforeClimb;
				if (onBeforeClimb != null)
				{
					onBeforeClimb(hand, climbableRef);
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			Rigidbody rigidbody;
			climbable.TryGetComponent<Rigidbody>(out rigidbody);
			this.VerifyClimbHelper();
			this.climbHelper.SetParent(climbable.transform);
			this.climbHelper.position = hand.transform.position;
			Vector3 localPosition = this.climbHelper.localPosition;
			if (climbable.snapX)
			{
				GTPlayer.<BeginClimbing>g__SnapAxis|379_0(ref localPosition.x, climbable.maxDistanceSnap);
			}
			if (climbable.snapY)
			{
				GTPlayer.<BeginClimbing>g__SnapAxis|379_0(ref localPosition.y, climbable.maxDistanceSnap);
			}
			if (climbable.snapZ)
			{
				GTPlayer.<BeginClimbing>g__SnapAxis|379_0(ref localPosition.z, climbable.maxDistanceSnap);
			}
			this.climbHelperTargetPos = localPosition;
			climbable.isBeingClimbed = true;
			hand.isClimbing = true;
			this.currentClimbable = climbable;
			this.currentClimber = hand;
			this.isClimbing = true;
			if (climbable.climbOnlyWhileSmall)
			{
				BuilderPiece componentInParent = climbable.GetComponentInParent<BuilderPiece>();
				if (componentInParent != null && componentInParent.IsPieceMoving())
				{
					this.isClimbableMoving = true;
					this.lastClimbableRotation = climbable.transform.rotation;
				}
				else
				{
					this.isClimbableMoving = false;
				}
			}
			else
			{
				this.isClimbableMoving = false;
			}
			GorillaRopeSegment gorillaRopeSegment;
			GorillaZipline gorillaZipline;
			PhotonView view;
			PhotonViewXSceneRef photonViewXSceneRef;
			if (climbable.TryGetComponent<GorillaRopeSegment>(out gorillaRopeSegment) && gorillaRopeSegment.swing)
			{
				this.currentSwing = gorillaRopeSegment.swing;
				this.currentSwing.AttachLocalPlayer(hand.xrNode, climbable.transform, this.climbHelperTargetPos, this.averagedVelocity);
			}
			else if (climbable.transform.parent && climbable.transform.parent.TryGetComponent<GorillaZipline>(out gorillaZipline))
			{
				this.currentZipline = gorillaZipline;
			}
			else if (climbable.TryGetComponent<PhotonView>(out view))
			{
				VRRig.AttachLocalPlayerToPhotonView(view, hand.xrNode, this.climbHelperTargetPos, this.averagedVelocity);
			}
			else if (climbable.TryGetComponent<PhotonViewXSceneRef>(out photonViewXSceneRef))
			{
				VRRig.AttachLocalPlayerToPhotonView(photonViewXSceneRef.photonView, hand.xrNode, this.climbHelperTargetPos, this.averagedVelocity);
			}
			GorillaTagger.Instance.StartVibration(this.currentClimber.xrNode == XRNode.LeftHand, 0.6f, 0.06f);
			if (climbable.clip)
			{
				GorillaTagger.Instance.offlineVRRig.PlayClimbSound(climbable.clip, hand.xrNode == XRNode.LeftHand);
			}
		}

		// Token: 0x060047E8 RID: 18408 RVA: 0x0015A274 File Offset: 0x00158474
		private void VerifyClimbHelper()
		{
			if (this.climbHelper == null || this.climbHelper.gameObject == null)
			{
				this.climbHelper = new GameObject("Climb Helper").transform;
			}
		}

		// Token: 0x060047E9 RID: 18409 RVA: 0x0015A2AC File Offset: 0x001584AC
		public GorillaVelocityTracker GetInteractPointVelocityTracker(bool isRightHand)
		{
			if (!isRightHand)
			{
				return this.leftInteractPointVelocityTracker;
			}
			return this.rightInteractPointVelocityTracker;
		}

		// Token: 0x060047EA RID: 18410 RVA: 0x0015A2C0 File Offset: 0x001584C0
		public void EndClimbing(GorillaHandClimber hand, bool startingNewClimb, bool doDontReclimb = false)
		{
			if (hand != this.currentClimber)
			{
				return;
			}
			hand.SetCanRelease(true);
			if (!startingNewClimb)
			{
				this.enablePlayerGravity(true);
			}
			Rigidbody rigidbody = null;
			if (this.currentClimbable)
			{
				this.currentClimbable.TryGetComponent<Rigidbody>(out rigidbody);
				this.currentClimbable.isBeingClimbed = false;
			}
			Vector3 vector = Vector3.zero;
			if (this.currentClimber)
			{
				this.currentClimber.isClimbing = false;
				if (doDontReclimb)
				{
					this.currentClimber.dontReclimbLast = this.currentClimbable;
				}
				else
				{
					this.currentClimber.dontReclimbLast = null;
				}
				this.currentClimber.queuedToBecomeValidToGrabAgain = true;
				this.currentClimber.lastAutoReleasePos = this.currentClimber.handRoot.localPosition;
				if (!startingNewClimb && this.currentClimbable)
				{
					GorillaVelocityTracker gorillaVelocityTracker = (this.currentClimber.xrNode == XRNode.LeftHand) ? this.leftInteractPointVelocityTracker : this.rightInteractPointVelocityTracker;
					if (rigidbody)
					{
						this.playerRigidBody.velocity = rigidbody.velocity;
					}
					else if (this.currentSwing)
					{
						this.playerRigidBody.velocity = this.currentSwing.velocityTracker.GetAverageVelocity(true, 0.25f, false);
					}
					else if (this.currentZipline)
					{
						this.playerRigidBody.velocity = this.currentZipline.GetCurrentDirection() * this.currentZipline.currentSpeed;
					}
					else
					{
						this.playerRigidBody.velocity = Vector3.zero;
					}
					vector = this.turnParent.transform.rotation * -gorillaVelocityTracker.GetAverageVelocity(false, 0.1f, true) * this.scale;
					vector = Vector3.ClampMagnitude(vector, 5.5f * this.scale);
					this.playerRigidBody.AddForce(vector, ForceMode.VelocityChange);
				}
			}
			if (this.currentSwing)
			{
				this.currentSwing.DetachLocalPlayer();
			}
			PhotonView photonView;
			PhotonViewXSceneRef photonViewXSceneRef;
			if (this.currentClimbable.TryGetComponent<PhotonView>(out photonView) || this.currentClimbable.TryGetComponent<PhotonViewXSceneRef>(out photonViewXSceneRef) || this.currentClimbable.IsPlayerAttached)
			{
				VRRig.DetachLocalPlayerFromPhotonView();
			}
			if (!startingNewClimb && vector.magnitude > 2f && this.currentClimbable && this.currentClimbable.clipOnFullRelease)
			{
				GorillaTagger.Instance.offlineVRRig.PlayClimbSound(this.currentClimbable.clipOnFullRelease, hand.xrNode == XRNode.LeftHand);
			}
			this.currentClimbable = null;
			this.currentClimber = null;
			this.currentSwing = null;
			this.currentZipline = null;
			this.isClimbing = false;
		}

		// Token: 0x060047EB RID: 18411 RVA: 0x0015A558 File Offset: 0x00158758
		private void enablePlayerGravity(bool useGravity)
		{
			this.playerRigidBody.useGravity = useGravity;
		}

		// Token: 0x060047EC RID: 18412 RVA: 0x0015A566 File Offset: 0x00158766
		public void SetVelocity(Vector3 velocity)
		{
			this.playerRigidBody.velocity = velocity;
		}

		// Token: 0x060047ED RID: 18413 RVA: 0x0015A574 File Offset: 0x00158774
		private void StoreVelocities()
		{
			this.velocityIndex = (this.velocityIndex + 1) % this.velocityHistorySize;
			this.currentVelocity = (base.transform.position - this.lastPosition - this.MovingSurfaceMovement()) / this.calcDeltaTime;
			this.velocityHistory[this.velocityIndex] = this.currentVelocity;
			this.averagedVelocity = Vector3.zero;
			for (int i = 0; i < this.velocityHistory.Length; i++)
			{
				this.averagedVelocity += this.velocityHistory[i];
			}
			this.averagedVelocity /= (float)this.velocityHistorySize;
			this.lastPosition = base.transform.position;
		}

		// Token: 0x060047EE RID: 18414 RVA: 0x0015A644 File Offset: 0x00158844
		private void AntiTeleportTechnology()
		{
			if ((this.headCollider.transform.position - this.lastHeadPosition).magnitude >= this.teleportThresholdNoVel + this.playerRigidBody.velocity.magnitude * this.calcDeltaTime)
			{
				base.transform.position = base.transform.position + this.lastHeadPosition - this.headCollider.transform.position;
			}
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x0015A6D0 File Offset: 0x001588D0
		private bool MaxSphereSizeForNoOverlap(float testRadius, Vector3 checkPosition, bool ignoreOneWay, out float overlapRadiusTest)
		{
			overlapRadiusTest = testRadius;
			this.overlapAttempts = 0;
			int num = 100;
			while (this.overlapAttempts < num && overlapRadiusTest > testRadius * 0.75f)
			{
				this.ClearColliderBuffer(ref this.overlapColliders);
				this.bufferCount = Physics.OverlapSphereNonAlloc(checkPosition, overlapRadiusTest, this.overlapColliders, this.locomotionEnabledLayers.value, QueryTriggerInteraction.Ignore);
				if (ignoreOneWay)
				{
					int num2 = 0;
					for (int i = 0; i < this.bufferCount; i++)
					{
						if (this.overlapColliders[i].CompareTag("NoCrazyCheck"))
						{
							num2++;
						}
					}
					if (num2 == this.bufferCount)
					{
						return true;
					}
				}
				if (this.bufferCount <= 0)
				{
					overlapRadiusTest *= 0.995f;
					return true;
				}
				overlapRadiusTest = Mathf.Lerp(testRadius, 0f, (float)this.overlapAttempts / (float)num);
				this.overlapAttempts++;
			}
			return false;
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x0015A7B0 File Offset: 0x001589B0
		private bool CrazyCheck2(float sphereSize, Vector3 startPosition)
		{
			for (int i = 0; i < this.crazyCheckVectors.Length; i++)
			{
				if (this.NonAllocRaycast(startPosition, startPosition + this.crazyCheckVectors[i] * sphereSize) > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060047F1 RID: 18417 RVA: 0x0015A7F8 File Offset: 0x001589F8
		private int NonAllocRaycast(Vector3 startPosition, Vector3 endPosition)
		{
			Vector3 direction = endPosition - startPosition;
			int num = Physics.RaycastNonAlloc(startPosition, direction, this.rayCastNonAllocColliders, direction.magnitude, this.locomotionEnabledLayers.value, QueryTriggerInteraction.Ignore);
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				if (!this.rayCastNonAllocColliders[i].collider.gameObject.CompareTag("NoCrazyCheck"))
				{
					num2++;
				}
			}
			return num2;
		}

		// Token: 0x060047F2 RID: 18418 RVA: 0x0015A864 File Offset: 0x00158A64
		private void ClearColliderBuffer(ref Collider[] colliders)
		{
			for (int i = 0; i < colliders.Length; i++)
			{
				colliders[i] = null;
			}
		}

		// Token: 0x060047F3 RID: 18419 RVA: 0x0015A888 File Offset: 0x00158A88
		private void ClearRaycasthitBuffer(ref RaycastHit[] raycastHits)
		{
			for (int i = 0; i < raycastHits.Length; i++)
			{
				raycastHits[i] = this.emptyHit;
			}
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x0015A8B2 File Offset: 0x00158AB2
		private Vector3 MovingSurfaceMovement()
		{
			return this.refMovement + this.movingSurfaceOffset;
		}

		// Token: 0x060047F5 RID: 18421 RVA: 0x0015A8C8 File Offset: 0x00158AC8
		private static bool ComputeLocalHitPoint(RaycastHit hit, out Vector3 localHitPoint)
		{
			if (hit.collider == null || hit.point.sqrMagnitude < 0.001f)
			{
				localHitPoint = Vector3.zero;
				return false;
			}
			localHitPoint = hit.collider.transform.InverseTransformPoint(hit.point);
			return true;
		}

		// Token: 0x060047F6 RID: 18422 RVA: 0x0015A926 File Offset: 0x00158B26
		private static bool ComputeWorldHitPoint(RaycastHit hit, Vector3 localPoint, out Vector3 worldHitPoint)
		{
			if (hit.collider == null)
			{
				worldHitPoint = Vector3.zero;
				return false;
			}
			worldHitPoint = hit.collider.transform.TransformPoint(localPoint);
			return true;
		}

		// Token: 0x060047F7 RID: 18423 RVA: 0x0015A960 File Offset: 0x00158B60
		private float ExtraVelMultiplier()
		{
			float num = 1f;
			if (this.leftHandSurfaceOverride != null)
			{
				num = Mathf.Max(num, this.leftHandSurfaceOverride.extraVelMultiplier);
			}
			if (this.rightHandSurfaceOverride != null)
			{
				num = Mathf.Max(num, this.rightHandSurfaceOverride.extraVelMultiplier);
			}
			return num;
		}

		// Token: 0x060047F8 RID: 18424 RVA: 0x0015A9B4 File Offset: 0x00158BB4
		private float ExtraVelMaxMultiplier()
		{
			float num = 1f;
			if (this.leftHandSurfaceOverride != null)
			{
				num = Mathf.Max(num, this.leftHandSurfaceOverride.extraVelMaxMultiplier);
			}
			if (this.rightHandSurfaceOverride != null)
			{
				num = Mathf.Max(num, this.rightHandSurfaceOverride.extraVelMaxMultiplier);
			}
			return num * this.scale;
		}

		// Token: 0x060047F9 RID: 18425 RVA: 0x0015AA11 File Offset: 0x00158C11
		public void SetMaximumSlipThisFrame()
		{
			this.leftSlipSetToMaxFrameIdx = Time.frameCount;
			this.rightSlipSetToMaxFrameIdx = Time.frameCount;
		}

		// Token: 0x060047FA RID: 18426 RVA: 0x0015AA29 File Offset: 0x00158C29
		public void SetLeftMaximumSlipThisFrame()
		{
			this.leftSlipSetToMaxFrameIdx = Time.frameCount;
		}

		// Token: 0x060047FB RID: 18427 RVA: 0x0015AA36 File Offset: 0x00158C36
		public void SetRightMaximumSlipThisFrame()
		{
			this.rightSlipSetToMaxFrameIdx = Time.frameCount;
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x0015AA43 File Offset: 0x00158C43
		public bool LeftSlipOverriddenToMax()
		{
			return this.leftSlipSetToMaxFrameIdx == Time.frameCount;
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x0015AA52 File Offset: 0x00158C52
		public bool RightSlipOverriddenToMax()
		{
			return this.rightSlipSetToMaxFrameIdx == Time.frameCount;
		}

		// Token: 0x060047FE RID: 18430 RVA: 0x0015AA61 File Offset: 0x00158C61
		public void ChangeLayer(string layerName)
		{
			if (this.layerChanger != null)
			{
				this.layerChanger.ChangeLayer(base.transform.parent, layerName);
			}
		}

		// Token: 0x060047FF RID: 18431 RVA: 0x0015AA88 File Offset: 0x00158C88
		public void RestoreLayer()
		{
			if (this.layerChanger != null)
			{
				this.layerChanger.RestoreOriginalLayers();
			}
		}

		// Token: 0x06004800 RID: 18432 RVA: 0x0015AAA4 File Offset: 0x00158CA4
		public void OnEnterWaterVolume(Collider playerCollider, WaterVolume volume)
		{
			if (this.activeSizeChangerSettings != null && this.activeSizeChangerSettings.ExpireInWater)
			{
				this.SetNativeScale(null);
			}
			if (playerCollider == this.headCollider)
			{
				if (!this.headOverlappingWaterVolumes.Contains(volume))
				{
					this.headOverlappingWaterVolumes.Add(volume);
					return;
				}
			}
			else if (playerCollider == this.bodyCollider && !this.bodyOverlappingWaterVolumes.Contains(volume))
			{
				this.bodyOverlappingWaterVolumes.Add(volume);
			}
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x0015AB1E File Offset: 0x00158D1E
		public void OnExitWaterVolume(Collider playerCollider, WaterVolume volume)
		{
			if (playerCollider == this.headCollider)
			{
				this.headOverlappingWaterVolumes.Remove(volume);
				return;
			}
			if (playerCollider == this.bodyCollider)
			{
				this.bodyOverlappingWaterVolumes.Remove(volume);
			}
		}

		// Token: 0x06004802 RID: 18434 RVA: 0x0015AB58 File Offset: 0x00158D58
		private bool GetSwimmingVelocityForHand(Vector3 startingHandPosition, Vector3 endingHandPosition, Vector3 palmForwardDirection, float dt, ref WaterVolume contactingWaterVolume, ref WaterVolume.SurfaceQuery waterSurface, out Vector3 swimmingVelocityChange)
		{
			contactingWaterVolume = null;
			this.bufferCount = Physics.OverlapSphereNonAlloc(endingHandPosition, this.minimumRaycastDistance, this.overlapColliders, this.waterLayer.value, QueryTriggerInteraction.Collide);
			if (this.bufferCount > 0)
			{
				float num = float.MinValue;
				for (int i = 0; i < this.bufferCount; i++)
				{
					WaterVolume component = this.overlapColliders[i].GetComponent<WaterVolume>();
					WaterVolume.SurfaceQuery surfaceQuery;
					if (component != null && component.GetSurfaceQueryForPoint(endingHandPosition, out surfaceQuery, false) && surfaceQuery.surfacePoint.y > num)
					{
						num = surfaceQuery.surfacePoint.y;
						contactingWaterVolume = component;
						waterSurface = surfaceQuery;
					}
				}
			}
			if (contactingWaterVolume != null)
			{
				Vector3 a = endingHandPosition - startingHandPosition;
				Vector3 b = Vector3.zero;
				Vector3 b2 = this.playerRigidBody.transform.position - this.lastRigidbodyPosition;
				if (this.turnedThisFrame)
				{
					Vector3 vector = startingHandPosition - this.headCollider.transform.position;
					b = Quaternion.AngleAxis(this.degreesTurnedThisFrame, Vector3.up) * vector - vector;
				}
				float num2 = Vector3.Dot(a - b - b2, palmForwardDirection);
				float num3 = 0f;
				if (num2 > 0f)
				{
					Plane surfacePlane = waterSurface.surfacePlane;
					float distanceToPoint = surfacePlane.GetDistanceToPoint(startingHandPosition);
					float distanceToPoint2 = surfacePlane.GetDistanceToPoint(endingHandPosition);
					if (distanceToPoint <= 0f && distanceToPoint2 <= 0f)
					{
						num3 = 1f;
					}
					else if (distanceToPoint > 0f && distanceToPoint2 <= 0f)
					{
						num3 = -distanceToPoint2 / (distanceToPoint - distanceToPoint2);
					}
					else if (distanceToPoint <= 0f && distanceToPoint2 > 0f)
					{
						num3 = -distanceToPoint / (distanceToPoint2 - distanceToPoint);
					}
					if (num3 > Mathf.Epsilon)
					{
						float resistance = this.liquidPropertiesList[(int)contactingWaterVolume.LiquidType].resistance;
						swimmingVelocityChange = -palmForwardDirection * num2 * 2f * resistance * num3;
						Vector3 forward = this.mainCamera.transform.forward;
						if (forward.y < 0f)
						{
							Vector3 vector2 = forward.x0z();
							float magnitude = vector2.magnitude;
							vector2 /= magnitude;
							float num4 = Vector3.Dot(swimmingVelocityChange, vector2);
							if (num4 > 0f)
							{
								Vector3 vector3 = vector2 * num4;
								swimmingVelocityChange = swimmingVelocityChange - vector3 + vector3 * magnitude + Vector3.up * forward.y * num4;
							}
						}
						return true;
					}
				}
			}
			swimmingVelocityChange = Vector3.zero;
			return false;
		}

		// Token: 0x06004803 RID: 18435 RVA: 0x0015AE14 File Offset: 0x00159014
		private bool CheckWaterSurfaceJump(Vector3 startingHandPosition, Vector3 endingHandPosition, Vector3 palmForwardDirection, Vector3 handAvgVelocity, PlayerSwimmingParameters parameters, WaterVolume contactingWaterVolume, WaterVolume.SurfaceQuery waterSurface, out Vector3 jumpVelocity)
		{
			if (contactingWaterVolume != null)
			{
				Plane surfacePlane = waterSurface.surfacePlane;
				bool flag = handAvgVelocity.sqrMagnitude > parameters.waterSurfaceJumpHandSpeedThreshold * parameters.waterSurfaceJumpHandSpeedThreshold;
				if (surfacePlane.GetSide(startingHandPosition) && !surfacePlane.GetSide(endingHandPosition) && flag)
				{
					float value = Vector3.Dot(palmForwardDirection, -waterSurface.surfaceNormal);
					float value2 = Vector3.Dot(handAvgVelocity.normalized, -waterSurface.surfaceNormal);
					float d = parameters.waterSurfaceJumpPalmFacingCurve.Evaluate(Mathf.Clamp(value, 0.01f, 0.99f));
					float d2 = parameters.waterSurfaceJumpHandVelocityFacingCurve.Evaluate(Mathf.Clamp(value2, 0.01f, 0.99f));
					jumpVelocity = -handAvgVelocity * parameters.waterSurfaceJumpAmount * d * d2;
					return true;
				}
			}
			jumpVelocity = Vector3.zero;
			return false;
		}

		// Token: 0x06004804 RID: 18436 RVA: 0x0015AF0D File Offset: 0x0015910D
		private bool TryNormalize(Vector3 input, out Vector3 normalized, out float magnitude, float eps = 0.0001f)
		{
			magnitude = input.magnitude;
			if (magnitude > eps)
			{
				normalized = input / magnitude;
				return true;
			}
			normalized = Vector3.zero;
			return false;
		}

		// Token: 0x06004805 RID: 18437 RVA: 0x0015AF3A File Offset: 0x0015913A
		private bool TryNormalizeDown(Vector3 input, out Vector3 normalized, out float magnitude, float eps = 0.0001f)
		{
			magnitude = input.magnitude;
			if (magnitude > 1f)
			{
				normalized = input / magnitude;
				return true;
			}
			if (magnitude >= eps)
			{
				normalized = input;
				return true;
			}
			normalized = Vector3.zero;
			return false;
		}

		// Token: 0x06004806 RID: 18438 RVA: 0x0015AF7C File Offset: 0x0015917C
		private float FreezeTagSlidePercentage()
		{
			if (this.materialData[this.currentMaterialIndex].overrideSlidePercent && this.materialData[this.currentMaterialIndex].slidePercent > this.freezeTagHandSlidePercent)
			{
				return this.materialData[this.currentMaterialIndex].slidePercent;
			}
			return this.freezeTagHandSlidePercent;
		}

		// Token: 0x06004807 RID: 18439 RVA: 0x0015AFDC File Offset: 0x001591DC
		private void OnCollisionStay(UnityEngine.Collision collision)
		{
			this.bodyCollisionContactsCount = collision.GetContacts(this.bodyCollisionContacts);
			float num = -1f;
			for (int i = 0; i < this.bodyCollisionContactsCount; i++)
			{
				float num2 = Vector3.Dot(this.bodyCollisionContacts[i].normal, Vector3.up);
				if (num2 > num)
				{
					this.bodyGroundContact = this.bodyCollisionContacts[i];
					num = num2;
				}
			}
			float num3 = 0.5f;
			if (num > num3)
			{
				this.bodyGroundContactTime = Time.time;
			}
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x0015B064 File Offset: 0x00159264
		public void DoLaunch(Vector3 velocity)
		{
			GTPlayer.<DoLaunch>d__412 <DoLaunch>d__;
			<DoLaunch>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<DoLaunch>d__.<>4__this = this;
			<DoLaunch>d__.velocity = velocity;
			<DoLaunch>d__.<>1__state = -1;
			<DoLaunch>d__.<>t__builder.Start<GTPlayer.<DoLaunch>d__412>(ref <DoLaunch>d__);
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x0015B0A3 File Offset: 0x001592A3
		private void OnEnable()
		{
			RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.OnJoinedRoom));
		}

		// Token: 0x0600480A RID: 18442 RVA: 0x0015B0C5 File Offset: 0x001592C5
		private void OnJoinedRoom()
		{
			if (this.activeSizeChangerSettings != null && this.activeSizeChangerSettings.ExpireOnRoomJoin)
			{
				this.SetNativeScale(null);
			}
		}

		// Token: 0x0600480B RID: 18443 RVA: 0x0015B0E3 File Offset: 0x001592E3
		private void OnDisable()
		{
			RoomSystem.JoinedRoomEvent = (Action)Delegate.Remove(RoomSystem.JoinedRoomEvent, new Action(this.OnJoinedRoom));
		}

		// Token: 0x0600480C RID: 18444 RVA: 0x0015B108 File Offset: 0x00159308
		internal void AddHandHold(Transform objectHeld, Vector3 localPositionHeld, GorillaGrabber grabber, bool rightHand, bool rotatePlayerWhenHeld, out Vector3 grabbedVelocity)
		{
			if (!this.leftHandHolding && !this.rightHandHolding)
			{
				grabbedVelocity = -this.bodyCollider.attachedRigidbody.velocity;
				this.playerRigidBody.AddForce(grabbedVelocity, ForceMode.VelocityChange);
			}
			else
			{
				grabbedVelocity = Vector3.zero;
			}
			this.secondaryHandHold = this.activeHandHold;
			Vector3 position = grabber.transform.position;
			this.activeHandHold = new GTPlayer.HandHoldState
			{
				grabber = grabber,
				objectHeld = objectHeld,
				localPositionHeld = localPositionHeld,
				localRotationalOffset = grabber.transform.rotation.eulerAngles.y - objectHeld.rotation.eulerAngles.y,
				applyRotation = rotatePlayerWhenHeld
			};
			if (rightHand)
			{
				this.rightHandHolding = true;
			}
			else
			{
				this.leftHandHolding = true;
			}
			this.OnChangeActiveHandhold();
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x0015B1F8 File Offset: 0x001593F8
		internal void RemoveHandHold(GorillaGrabber grabber, bool rightHand)
		{
			this.activeHandHold.objectHeld == grabber;
			if (this.activeHandHold.grabber == grabber)
			{
				this.activeHandHold = this.secondaryHandHold;
			}
			this.secondaryHandHold = default(GTPlayer.HandHoldState);
			if (rightHand)
			{
				this.rightHandHolding = false;
			}
			else
			{
				this.leftHandHolding = false;
			}
			this.OnChangeActiveHandhold();
		}

		// Token: 0x0600480E RID: 18446 RVA: 0x0015B260 File Offset: 0x00159460
		private void OnChangeActiveHandhold()
		{
			if (this.activeHandHold.objectHeld != null)
			{
				PhotonView view;
				if (this.activeHandHold.objectHeld.TryGetComponent<PhotonView>(out view))
				{
					VRRig.AttachLocalPlayerToPhotonView(view, this.activeHandHold.grabber.XrNode, this.activeHandHold.localPositionHeld, this.averagedVelocity);
					return;
				}
				PhotonViewXSceneRef photonViewXSceneRef;
				if (this.activeHandHold.objectHeld.TryGetComponent<PhotonViewXSceneRef>(out photonViewXSceneRef))
				{
					PhotonView photonView = photonViewXSceneRef.photonView;
					if (photonView != null)
					{
						VRRig.AttachLocalPlayerToPhotonView(photonView, this.activeHandHold.grabber.XrNode, this.activeHandHold.localPositionHeld, this.averagedVelocity);
						return;
					}
				}
				BuilderPieceHandHold builderPieceHandHold;
				if (this.activeHandHold.objectHeld.TryGetComponent<BuilderPieceHandHold>(out builderPieceHandHold) && builderPieceHandHold.IsHandHoldMoving())
				{
					this.isHandHoldMoving = true;
					this.lastHandHoldRotation = builderPieceHandHold.transform.rotation;
					this.movingHandHoldReleaseVelocity = this.playerRigidBody.velocity;
				}
				else
				{
					this.isHandHoldMoving = false;
					this.lastHandHoldRotation = Quaternion.identity;
					this.movingHandHoldReleaseVelocity = Vector3.zero;
				}
			}
			VRRig.DetachLocalPlayerFromPhotonView();
		}

		// Token: 0x0600480F RID: 18447 RVA: 0x0015B370 File Offset: 0x00159570
		private void FixedUpdate_HandHolds(float timeDelta)
		{
			if (this.activeHandHold.objectHeld == null)
			{
				if (this.wasHoldingHandhold)
				{
					this.playerRigidBody.velocity = Vector3.ClampMagnitude(this.secondLastPreHandholdVelocity, 5.5f * this.scale);
				}
				this.wasHoldingHandhold = false;
				return;
			}
			Vector3 vector = this.activeHandHold.objectHeld.TransformPoint(this.activeHandHold.localPositionHeld);
			Vector3 position = this.activeHandHold.grabber.transform.position;
			this.secondLastPreHandholdVelocity = this.lastPreHandholdVelocity;
			this.lastPreHandholdVelocity = this.playerRigidBody.velocity;
			this.wasHoldingHandhold = true;
			if (this.isHandHoldMoving)
			{
				this.lastPreHandholdVelocity = this.movingHandHoldReleaseVelocity;
				this.playerRigidBody.velocity = Vector3.zero;
				Vector3 vector2 = vector - position;
				this.playerRigidBody.transform.position += vector2;
				this.movingHandHoldReleaseVelocity = vector2 / timeDelta;
				Quaternion rotationDelta = this.activeHandHold.objectHeld.rotation * Quaternion.Inverse(this.lastHandHoldRotation);
				this.RotateWithSurface(rotationDelta, vector);
				this.lastHandHoldRotation = this.activeHandHold.objectHeld.rotation;
				return;
			}
			this.playerRigidBody.velocity = (vector - position) / timeDelta;
			if (this.activeHandHold.applyRotation)
			{
				this.turnParent.transform.RotateAround(vector, base.transform.up, this.activeHandHold.localRotationalOffset - (this.activeHandHold.grabber.transform.rotation.eulerAngles.y - this.activeHandHold.objectHeld.rotation.eulerAngles.y));
			}
		}

		// Token: 0x06004815 RID: 18453 RVA: 0x0015B929 File Offset: 0x00159B29
		[CompilerGenerated]
		internal static void <BeginClimbing>g__SnapAxis|379_0(ref float val, float maxDist)
		{
			if (val > maxDist)
			{
				val = maxDist;
				return;
			}
			if (val < -maxDist)
			{
				val = -maxDist;
			}
		}

		// Token: 0x0400492F RID: 18735
		private static GTPlayer _instance;

		// Token: 0x04004930 RID: 18736
		public static bool hasInstance;

		// Token: 0x04004931 RID: 18737
		public SphereCollider headCollider;

		// Token: 0x04004932 RID: 18738
		public CapsuleCollider bodyCollider;

		// Token: 0x04004933 RID: 18739
		private float bodyInitialRadius;

		// Token: 0x04004934 RID: 18740
		private float bodyInitialHeight;

		// Token: 0x04004935 RID: 18741
		private RaycastHit bodyHitInfo;

		// Token: 0x04004936 RID: 18742
		private RaycastHit lastHitInfoHand;

		// Token: 0x04004937 RID: 18743
		public Transform leftHandFollower;

		// Token: 0x04004938 RID: 18744
		public Transform rightHandFollower;

		// Token: 0x04004939 RID: 18745
		public Transform rightControllerTransform;

		// Token: 0x0400493A RID: 18746
		public Transform leftControllerTransform;

		// Token: 0x0400493B RID: 18747
		public GorillaVelocityTracker rightHandCenterVelocityTracker;

		// Token: 0x0400493C RID: 18748
		public GorillaVelocityTracker leftHandCenterVelocityTracker;

		// Token: 0x0400493D RID: 18749
		public GorillaVelocityTracker rightInteractPointVelocityTracker;

		// Token: 0x0400493E RID: 18750
		public GorillaVelocityTracker leftInteractPointVelocityTracker;

		// Token: 0x0400493F RID: 18751
		public GorillaVelocityTracker bodyVelocityTracker;

		// Token: 0x04004940 RID: 18752
		public PlayerAudioManager audioManager;

		// Token: 0x04004941 RID: 18753
		private Vector3 lastLeftHandPosition;

		// Token: 0x04004942 RID: 18754
		private Vector3 lastRightHandPosition;

		// Token: 0x04004943 RID: 18755
		public Vector3 lastHeadPosition;

		// Token: 0x04004944 RID: 18756
		private Vector3 lastRigidbodyPosition;

		// Token: 0x04004945 RID: 18757
		private Rigidbody playerRigidBody;

		// Token: 0x04004946 RID: 18758
		private Camera mainCamera;

		// Token: 0x04004947 RID: 18759
		public int velocityHistorySize;

		// Token: 0x04004948 RID: 18760
		public float maxArmLength = 1f;

		// Token: 0x04004949 RID: 18761
		public float unStickDistance = 1f;

		// Token: 0x0400494A RID: 18762
		public float velocityLimit;

		// Token: 0x0400494B RID: 18763
		public float slideVelocityLimit;

		// Token: 0x0400494C RID: 18764
		public float maxJumpSpeed;

		// Token: 0x0400494D RID: 18765
		private float _jumpMultiplier;

		// Token: 0x0400494E RID: 18766
		public float minimumRaycastDistance = 0.05f;

		// Token: 0x0400494F RID: 18767
		public float defaultSlideFactor = 0.03f;

		// Token: 0x04004950 RID: 18768
		public float slidingMinimum = 0.9f;

		// Token: 0x04004951 RID: 18769
		public float defaultPrecision = 0.995f;

		// Token: 0x04004952 RID: 18770
		public float teleportThresholdNoVel = 1f;

		// Token: 0x04004953 RID: 18771
		public float frictionConstant = 1f;

		// Token: 0x04004954 RID: 18772
		public float slideControl = 0.00425f;

		// Token: 0x04004955 RID: 18773
		public float stickDepth = 0.01f;

		// Token: 0x04004956 RID: 18774
		private Vector3[] velocityHistory;

		// Token: 0x04004957 RID: 18775
		private Vector3[] slideAverageHistory;

		// Token: 0x04004958 RID: 18776
		private int velocityIndex;

		// Token: 0x04004959 RID: 18777
		private Vector3 currentVelocity;

		// Token: 0x0400495A RID: 18778
		private Vector3 averagedVelocity;

		// Token: 0x0400495B RID: 18779
		private Vector3 lastPosition;

		// Token: 0x0400495C RID: 18780
		public Vector3 rightHandOffset;

		// Token: 0x0400495D RID: 18781
		public Vector3 leftHandOffset;

		// Token: 0x0400495E RID: 18782
		public Quaternion rightHandRotOffset = Quaternion.identity;

		// Token: 0x0400495F RID: 18783
		public Quaternion leftHandRotOffset = Quaternion.identity;

		// Token: 0x04004960 RID: 18784
		public Vector3 bodyOffset;

		// Token: 0x04004961 RID: 18785
		public LayerMask locomotionEnabledLayers;

		// Token: 0x04004962 RID: 18786
		public LayerMask waterLayer;

		// Token: 0x04004963 RID: 18787
		public bool wasLeftHandColliding;

		// Token: 0x04004964 RID: 18788
		public bool wasRightHandColliding;

		// Token: 0x04004965 RID: 18789
		public bool wasHeadTouching;

		// Token: 0x04004966 RID: 18790
		public int currentMaterialIndex;

		// Token: 0x04004967 RID: 18791
		public bool isLeftHandSliding;

		// Token: 0x04004968 RID: 18792
		public Vector3 leftHandSlideNormal;

		// Token: 0x04004969 RID: 18793
		public bool isRightHandSliding;

		// Token: 0x0400496A RID: 18794
		public Vector3 rightHandSlideNormal;

		// Token: 0x0400496B RID: 18795
		public Vector3 headSlideNormal;

		// Token: 0x0400496C RID: 18796
		public float rightHandSlipPercentage;

		// Token: 0x0400496D RID: 18797
		public float leftHandSlipPercentage;

		// Token: 0x0400496E RID: 18798
		public float headSlipPercentage;

		// Token: 0x0400496F RID: 18799
		public bool wasLeftHandSliding;

		// Token: 0x04004970 RID: 18800
		public bool wasRightHandSliding;

		// Token: 0x04004971 RID: 18801
		public Vector3 rightHandHitPoint;

		// Token: 0x04004972 RID: 18802
		public Vector3 leftHandHitPoint;

		// Token: 0x04004973 RID: 18803
		[SerializeField]
		private Transform cosmeticsHeadTarget;

		// Token: 0x04004974 RID: 18804
		[SerializeField]
		private float nativeScale = 1f;

		// Token: 0x04004975 RID: 18805
		[SerializeField]
		private float scaleMultiplier = 1f;

		// Token: 0x04004976 RID: 18806
		private NativeSizeChangerSettings activeSizeChangerSettings;

		// Token: 0x04004977 RID: 18807
		public bool debugMovement;

		// Token: 0x04004978 RID: 18808
		public bool disableMovement;

		// Token: 0x04004979 RID: 18809
		[NonSerialized]
		public bool inOverlay;

		// Token: 0x0400497A RID: 18810
		[NonSerialized]
		public bool isUserPresent;

		// Token: 0x0400497B RID: 18811
		public GameObject turnParent;

		// Token: 0x0400497C RID: 18812
		public int leftHandMaterialTouchIndex;

		// Token: 0x0400497D RID: 18813
		public GorillaSurfaceOverride leftHandSurfaceOverride;

		// Token: 0x0400497E RID: 18814
		public RaycastHit leftHandHitInfo;

		// Token: 0x0400497F RID: 18815
		public int rightHandMaterialTouchIndex;

		// Token: 0x04004980 RID: 18816
		public GorillaSurfaceOverride rightHandSurfaceOverride;

		// Token: 0x04004981 RID: 18817
		public RaycastHit rightHandHitInfo;

		// Token: 0x04004982 RID: 18818
		public GorillaSurfaceOverride currentOverride;

		// Token: 0x04004983 RID: 18819
		public MaterialDatasSO materialDatasSO;

		// Token: 0x04004984 RID: 18820
		private bool isLeftHandColliding;

		// Token: 0x04004985 RID: 18821
		private bool isRightHandColliding;

		// Token: 0x04004986 RID: 18822
		private float degreesTurnedThisFrame;

		// Token: 0x04004987 RID: 18823
		private Vector3 bodyOffsetVector;

		// Token: 0x04004988 RID: 18824
		private Vector3 movementToProjectedAboveCollisionPlane;

		// Token: 0x04004989 RID: 18825
		private MeshCollider meshCollider;

		// Token: 0x0400498A RID: 18826
		private Mesh collidedMesh;

		// Token: 0x0400498B RID: 18827
		private GTPlayer.MaterialData foundMatData;

		// Token: 0x0400498C RID: 18828
		private string findMatName;

		// Token: 0x0400498D RID: 18829
		private int vertex1;

		// Token: 0x0400498E RID: 18830
		private int vertex2;

		// Token: 0x0400498F RID: 18831
		private int vertex3;

		// Token: 0x04004990 RID: 18832
		private List<int> trianglesList = new List<int>(1000000);

		// Token: 0x04004991 RID: 18833
		private Dictionary<Mesh, int[]> meshTrianglesDict = new Dictionary<Mesh, int[]>(128);

		// Token: 0x04004992 RID: 18834
		private int[] sharedMeshTris;

		// Token: 0x04004993 RID: 18835
		private float lastRealTime;

		// Token: 0x04004994 RID: 18836
		private float calcDeltaTime;

		// Token: 0x04004995 RID: 18837
		private float tempRealTime;

		// Token: 0x04004996 RID: 18838
		private Vector3 slideVelocity;

		// Token: 0x04004997 RID: 18839
		private Vector3 slideAverageNormal;

		// Token: 0x04004998 RID: 18840
		private RaycastHit tempHitInfo;

		// Token: 0x04004999 RID: 18841
		private RaycastHit junkHit;

		// Token: 0x0400499A RID: 18842
		private Vector3 firstPosition;

		// Token: 0x0400499B RID: 18843
		private RaycastHit tempIterativeHit;

		// Token: 0x0400499C RID: 18844
		private float maxSphereSize1;

		// Token: 0x0400499D RID: 18845
		private float maxSphereSize2;

		// Token: 0x0400499E RID: 18846
		private Collider[] overlapColliders = new Collider[10];

		// Token: 0x0400499F RID: 18847
		private int overlapAttempts;

		// Token: 0x040049A0 RID: 18848
		private int touchPoints;

		// Token: 0x040049A1 RID: 18849
		private float averageSlipPercentage;

		// Token: 0x040049A2 RID: 18850
		private Vector3 surfaceDirection;

		// Token: 0x040049A3 RID: 18851
		public float iceThreshold = 0.9f;

		// Token: 0x040049A4 RID: 18852
		private float bodyMaxRadius;

		// Token: 0x040049A5 RID: 18853
		public float bodyLerp = 0.17f;

		// Token: 0x040049A6 RID: 18854
		private bool areBothTouching;

		// Token: 0x040049A7 RID: 18855
		private float slideFactor;

		// Token: 0x040049A8 RID: 18856
		[DebugOption]
		public bool didAJump;

		// Token: 0x040049A9 RID: 18857
		private Renderer slideRenderer;

		// Token: 0x040049AA RID: 18858
		private RaycastHit[] rayCastNonAllocColliders;

		// Token: 0x040049AB RID: 18859
		private Vector3[] crazyCheckVectors;

		// Token: 0x040049AC RID: 18860
		private RaycastHit emptyHit;

		// Token: 0x040049AD RID: 18861
		private int bufferCount;

		// Token: 0x040049AE RID: 18862
		private Vector3 lastOpenHeadPosition;

		// Token: 0x040049AF RID: 18863
		private List<Material> tempMaterialArray = new List<Material>(16);

		// Token: 0x040049B0 RID: 18864
		private int leftSlipSetToMaxFrameIdx = -1;

		// Token: 0x040049B1 RID: 18865
		private int rightSlipSetToMaxFrameIdx = -1;

		// Token: 0x040049B2 RID: 18866
		private const float CameraFarClipDefault = 500f;

		// Token: 0x040049B3 RID: 18867
		private const float CameraNearClipDefault = 0.01f;

		// Token: 0x040049B4 RID: 18868
		private const float CameraNearClipTiny = 0.002f;

		// Token: 0x040049B5 RID: 18869
		private Dictionary<GameObject, PhysicMaterial> bodyTouchedSurfaces;

		// Token: 0x040049B6 RID: 18870
		private bool primaryButtonPressed = true;

		// Token: 0x040049B7 RID: 18871
		[Header("Swimming")]
		public PlayerSwimmingParameters swimmingParams;

		// Token: 0x040049B8 RID: 18872
		public WaterParameters waterParams;

		// Token: 0x040049B9 RID: 18873
		public List<GTPlayer.LiquidProperties> liquidPropertiesList = new List<GTPlayer.LiquidProperties>(16);

		// Token: 0x040049BA RID: 18874
		public bool debugDrawSwimming;

		// Token: 0x040049BB RID: 18875
		[Header("Slam/Hit effects")]
		public GameObject wizardStaffSlamEffects;

		// Token: 0x040049BC RID: 18876
		public GameObject geodeHitEffects;

		// Token: 0x040049BD RID: 18877
		[Header("Freeze Tag")]
		public float freezeTagHandSlidePercent = 0.88f;

		// Token: 0x040049BE RID: 18878
		public bool debugFreezeTag;

		// Token: 0x040049BF RID: 18879
		public float frozenBodyBuoyancyFactor = 1.5f;

		// Token: 0x040049C1 RID: 18881
		[Space]
		private WaterVolume leftHandWaterVolume;

		// Token: 0x040049C2 RID: 18882
		private WaterVolume rightHandWaterVolume;

		// Token: 0x040049C3 RID: 18883
		private WaterVolume.SurfaceQuery leftHandWaterSurface;

		// Token: 0x040049C4 RID: 18884
		private WaterVolume.SurfaceQuery rightHandWaterSurface;

		// Token: 0x040049C5 RID: 18885
		private Vector3 swimmingVelocity = Vector3.zero;

		// Token: 0x040049C6 RID: 18886
		private WaterVolume.SurfaceQuery waterSurfaceForHead;

		// Token: 0x040049C7 RID: 18887
		private bool bodyInWater;

		// Token: 0x040049C8 RID: 18888
		private bool headInWater;

		// Token: 0x040049C9 RID: 18889
		private bool audioSetToUnderwater;

		// Token: 0x040049CA RID: 18890
		private float buoyancyExtension;

		// Token: 0x040049CB RID: 18891
		private float lastWaterSurfaceJumpTimeLeft = -1f;

		// Token: 0x040049CC RID: 18892
		private float lastWaterSurfaceJumpTimeRight = -1f;

		// Token: 0x040049CD RID: 18893
		private float waterSurfaceJumpCooldown = 0.1f;

		// Token: 0x040049CE RID: 18894
		private float leftHandNonDiveHapticsAmount;

		// Token: 0x040049CF RID: 18895
		private float rightHandNonDiveHapticsAmount;

		// Token: 0x040049D0 RID: 18896
		private List<WaterVolume> headOverlappingWaterVolumes = new List<WaterVolume>(16);

		// Token: 0x040049D1 RID: 18897
		private List<WaterVolume> bodyOverlappingWaterVolumes = new List<WaterVolume>(16);

		// Token: 0x040049D2 RID: 18898
		private List<WaterCurrent> activeWaterCurrents = new List<WaterCurrent>(16);

		// Token: 0x040049D3 RID: 18899
		private Quaternion playerRotationOverride;

		// Token: 0x040049D4 RID: 18900
		private int playerRotationOverrideFrame = -1;

		// Token: 0x040049D5 RID: 18901
		private float playerRotationOverrideDecayRate = Mathf.Exp(1.5f);

		// Token: 0x040049D7 RID: 18903
		private ContactPoint[] bodyCollisionContacts = new ContactPoint[8];

		// Token: 0x040049D8 RID: 18904
		private int bodyCollisionContactsCount;

		// Token: 0x040049D9 RID: 18905
		private ContactPoint bodyGroundContact;

		// Token: 0x040049DA RID: 18906
		private float bodyGroundContactTime;

		// Token: 0x040049DB RID: 18907
		private const float movingSurfaceVelocityLimit = 40f;

		// Token: 0x040049DC RID: 18908
		private bool exitMovingSurface;

		// Token: 0x040049DD RID: 18909
		private float exitMovingSurfaceThreshold = 6f;

		// Token: 0x040049DE RID: 18910
		private bool isClimbableMoving;

		// Token: 0x040049DF RID: 18911
		private Quaternion lastClimbableRotation;

		// Token: 0x040049E0 RID: 18912
		private int lastAttachedToMovingSurfaceFrame;

		// Token: 0x040049E1 RID: 18913
		private const int MIN_FRAMES_OFF_SURFACE_TO_DETACH = 3;

		// Token: 0x040049E2 RID: 18914
		private bool isHandHoldMoving;

		// Token: 0x040049E3 RID: 18915
		private Quaternion lastHandHoldRotation;

		// Token: 0x040049E4 RID: 18916
		private Vector3 movingHandHoldReleaseVelocity;

		// Token: 0x040049E5 RID: 18917
		private GTPlayer.MovingSurfaceContactPoint lastMovingSurfaceContact;

		// Token: 0x040049E6 RID: 18918
		private int lastMovingSurfaceID = -1;

		// Token: 0x040049E7 RID: 18919
		private BuilderPiece lastMonkeBlock;

		// Token: 0x040049E8 RID: 18920
		private Quaternion lastMovingSurfaceRot;

		// Token: 0x040049E9 RID: 18921
		private RaycastHit lastMovingSurfaceHit;

		// Token: 0x040049EA RID: 18922
		private Vector3 lastMovingSurfaceTouchLocal;

		// Token: 0x040049EB RID: 18923
		private Vector3 lastMovingSurfaceTouchWorld;

		// Token: 0x040049EC RID: 18924
		private Vector3 movingSurfaceOffset;

		// Token: 0x040049ED RID: 18925
		private bool wasMovingSurfaceMonkeBlock;

		// Token: 0x040049EE RID: 18926
		private Vector3 lastMovingSurfaceVelocity;

		// Token: 0x040049EF RID: 18927
		private bool wasBodyOnGround;

		// Token: 0x040049F0 RID: 18928
		private BasePlatform currentPlatform;

		// Token: 0x040049F1 RID: 18929
		private BasePlatform lastPlatformTouched;

		// Token: 0x040049F2 RID: 18930
		private Vector3 lastFrameTouchPosLocal;

		// Token: 0x040049F3 RID: 18931
		private Vector3 lastFrameTouchPosWorld;

		// Token: 0x040049F4 RID: 18932
		private bool lastFrameHasValidTouchPos;

		// Token: 0x040049F5 RID: 18933
		private Vector3 refMovement = Vector3.zero;

		// Token: 0x040049F6 RID: 18934
		private Vector3 platformTouchOffset;

		// Token: 0x040049F7 RID: 18935
		private Vector3 debugLastRightHandPosition;

		// Token: 0x040049F8 RID: 18936
		private Vector3 debugPlatformDeltaPosition;

		// Token: 0x040049F9 RID: 18937
		private const float climbingMaxThrowSpeed = 5.5f;

		// Token: 0x040049FA RID: 18938
		private const float climbHelperSmoothSnapSpeed = 12f;

		// Token: 0x040049FB RID: 18939
		[NonSerialized]
		public bool isClimbing;

		// Token: 0x040049FC RID: 18940
		private GorillaClimbable currentClimbable;

		// Token: 0x040049FD RID: 18941
		private GorillaHandClimber currentClimber;

		// Token: 0x040049FE RID: 18942
		private Vector3 climbHelperTargetPos = Vector3.zero;

		// Token: 0x040049FF RID: 18943
		private Transform climbHelper;

		// Token: 0x04004A00 RID: 18944
		private GorillaRopeSwing currentSwing;

		// Token: 0x04004A01 RID: 18945
		private GorillaZipline currentZipline;

		// Token: 0x04004A02 RID: 18946
		[SerializeField]
		private ConnectedControllerHandler controllerState;

		// Token: 0x04004A03 RID: 18947
		public int sizeLayerMask;

		// Token: 0x04004A04 RID: 18948
		public bool InReportMenu;

		// Token: 0x04004A05 RID: 18949
		private LayerChanger layerChanger;

		// Token: 0x04004A06 RID: 18950
		private float halloweenLevitationStrength;

		// Token: 0x04004A07 RID: 18951
		private float halloweenLevitationFullStrengthDuration;

		// Token: 0x04004A08 RID: 18952
		private float halloweenLevitationTotalDuration = 1f;

		// Token: 0x04004A09 RID: 18953
		private float halloweenLevitationBonusStrength;

		// Token: 0x04004A0A RID: 18954
		private float halloweenLevitateBonusOffAtYSpeed;

		// Token: 0x04004A0B RID: 18955
		private float halloweenLevitateBonusFullAtYSpeed = 1f;

		// Token: 0x04004A0C RID: 18956
		private float lastTouchedGroundTimestamp;

		// Token: 0x04004A0D RID: 18957
		private bool teleportToTrain;

		// Token: 0x04004A0E RID: 18958
		public bool isAttachedToTrain;

		// Token: 0x04004A0F RID: 18959
		private bool stuckLeft;

		// Token: 0x04004A10 RID: 18960
		private bool stuckRight;

		// Token: 0x04004A11 RID: 18961
		private float lastScale;

		// Token: 0x04004A12 RID: 18962
		private Vector3 currentSlopDirection;

		// Token: 0x04004A13 RID: 18963
		private Vector3 lastSlopeDirection = Vector3.zero;

		// Token: 0x04004A14 RID: 18964
		private Dictionary<Object, Action<GTPlayer>> gravityOverrides = new Dictionary<Object, Action<GTPlayer>>();

		// Token: 0x04004A17 RID: 18967
		[Header("Hoverboard Proto")]
		[SerializeField]
		private float hoverIdealHeight = 0.5f;

		// Token: 0x04004A18 RID: 18968
		[SerializeField]
		private float hoverCarveSidewaysSpeedLossFactor = 1f;

		// Token: 0x04004A19 RID: 18969
		[SerializeField]
		private AnimationCurve hoverCarveAngleResponsiveness;

		// Token: 0x04004A1A RID: 18970
		[SerializeField]
		private HoverboardVisual hoverboardVisual;

		// Token: 0x04004A1B RID: 18971
		[SerializeField]
		private float sidewaysDrag = 0.1f;

		// Token: 0x04004A1C RID: 18972
		[SerializeField]
		private float hoveringSlowSpeed = 0.1f;

		// Token: 0x04004A1D RID: 18973
		[SerializeField]
		private float hoveringSlowStoppingFactor = 0.95f;

		// Token: 0x04004A1E RID: 18974
		[SerializeField]
		private float hoverboardPaddleBoostMultiplier = 0.1f;

		// Token: 0x04004A1F RID: 18975
		[SerializeField]
		private float hoverboardPaddleBoostMax = 10f;

		// Token: 0x04004A20 RID: 18976
		[SerializeField]
		private float hoverboardBoostGracePeriod = 1f;

		// Token: 0x04004A21 RID: 18977
		[SerializeField]
		private float hoverBodyHasCollisionsOutsideRadius = 0.5f;

		// Token: 0x04004A22 RID: 18978
		[SerializeField]
		private float hoverBodyCollisionRadiusUpOffset = 0.2f;

		// Token: 0x04004A23 RID: 18979
		[SerializeField]
		private float hoverGeneralUpwardForce = 8f;

		// Token: 0x04004A24 RID: 18980
		[SerializeField]
		private float hoverTiltAdjustsForwardFactor = 0.2f;

		// Token: 0x04004A25 RID: 18981
		[SerializeField]
		private float hoverMinGrindSpeed = 1f;

		// Token: 0x04004A26 RID: 18982
		[SerializeField]
		private float hoverSlamJumpStrengthFactor = 25f;

		// Token: 0x04004A27 RID: 18983
		[SerializeField]
		private float hoverMaxPaddleSpeed = 35f;

		// Token: 0x04004A28 RID: 18984
		[SerializeField]
		private HoverboardAudio hoverboardAudio;

		// Token: 0x04004A29 RID: 18985
		private bool hasHoverPoint;

		// Token: 0x04004A2A RID: 18986
		private float boostEnabledUntilTimestamp;

		// Token: 0x04004A2B RID: 18987
		private GTPlayer.HoverBoardCast[] hoverboardCasts = new GTPlayer.HoverBoardCast[]
		{
			new GTPlayer.HoverBoardCast
			{
				localOrigin = new Vector3(0f, 1f, 0.36f),
				localDirection = Vector3.down,
				distance = 1f,
				sphereRadius = 0.2f,
				intersectToVelocityCap = 0.1f
			},
			new GTPlayer.HoverBoardCast
			{
				localOrigin = new Vector3(0f, 0.05f, 0.36f),
				localDirection = Vector3.forward,
				distance = 0.25f,
				sphereRadius = 0.01f,
				intersectToVelocityCap = 0f,
				isSolid = true
			},
			new GTPlayer.HoverBoardCast
			{
				localOrigin = new Vector3(0f, 0.05f, -0.1f),
				localDirection = -Vector3.forward,
				distance = 0.24f,
				sphereRadius = 0.01f,
				intersectToVelocityCap = 0f,
				isSolid = true
			}
		};

		// Token: 0x04004A2C RID: 18988
		private Vector3 hoverboardPlayerLocalPos;

		// Token: 0x04004A2D RID: 18989
		private Quaternion hoverboardPlayerLocalRot;

		// Token: 0x04004A2E RID: 18990
		private bool didHoverLastFrame;

		// Token: 0x04004A2F RID: 18991
		private GTPlayer.HandHoldState activeHandHold;

		// Token: 0x04004A30 RID: 18992
		private GTPlayer.HandHoldState secondaryHandHold;

		// Token: 0x04004A31 RID: 18993
		private bool rightHandHolding;

		// Token: 0x04004A32 RID: 18994
		private bool leftHandHolding;

		// Token: 0x04004A33 RID: 18995
		public PhysicMaterial slipperyMaterial;

		// Token: 0x04004A34 RID: 18996
		private bool wasHoldingHandhold;

		// Token: 0x04004A35 RID: 18997
		private Vector3 secondLastPreHandholdVelocity;

		// Token: 0x04004A36 RID: 18998
		private Vector3 lastPreHandholdVelocity;

		// Token: 0x04004A37 RID: 18999
		[Header("Native Scale Adjustment")]
		[SerializeField]
		private AnimationCurve nativeScaleMagnitudeAdjustmentFactor;

		// Token: 0x02000B3C RID: 2876
		private enum MovingSurfaceContactPoint
		{
			// Token: 0x04004A39 RID: 19001
			NONE,
			// Token: 0x04004A3A RID: 19002
			RIGHT,
			// Token: 0x04004A3B RID: 19003
			LEFT,
			// Token: 0x04004A3C RID: 19004
			BODY
		}

		// Token: 0x02000B3D RID: 2877
		[Serializable]
		public struct MaterialData
		{
			// Token: 0x04004A3D RID: 19005
			public string matName;

			// Token: 0x04004A3E RID: 19006
			public bool overrideAudio;

			// Token: 0x04004A3F RID: 19007
			public AudioClip audio;

			// Token: 0x04004A40 RID: 19008
			public bool overrideSlidePercent;

			// Token: 0x04004A41 RID: 19009
			public float slidePercent;

			// Token: 0x04004A42 RID: 19010
			public int surfaceEffectIndex;
		}

		// Token: 0x02000B3E RID: 2878
		[Serializable]
		public struct LiquidProperties
		{
			// Token: 0x04004A43 RID: 19011
			[Range(0f, 2f)]
			[Tooltip("0: no resistance just like air, 1: full resistance like solid geometry")]
			public float resistance;

			// Token: 0x04004A44 RID: 19012
			[Range(0f, 3f)]
			[Tooltip("0: no buoyancy. 1: Fully compensates gravity. 2: net force is upwards equal to gravity")]
			public float buoyancy;

			// Token: 0x04004A45 RID: 19013
			[Range(0f, 3f)]
			[Tooltip("Damping Half-life Multiplier")]
			public float dampingFactor;

			// Token: 0x04004A46 RID: 19014
			[Range(0f, 1f)]
			public float surfaceJumpFactor;
		}

		// Token: 0x02000B3F RID: 2879
		public enum LiquidType
		{
			// Token: 0x04004A48 RID: 19016
			Water,
			// Token: 0x04004A49 RID: 19017
			Lava
		}

		// Token: 0x02000B40 RID: 2880
		private struct HoverBoardCast
		{
			// Token: 0x04004A4A RID: 19018
			public Vector3 localOrigin;

			// Token: 0x04004A4B RID: 19019
			public Vector3 localDirection;

			// Token: 0x04004A4C RID: 19020
			public float sphereRadius;

			// Token: 0x04004A4D RID: 19021
			public float distance;

			// Token: 0x04004A4E RID: 19022
			public float intersectToVelocityCap;

			// Token: 0x04004A4F RID: 19023
			public bool isSolid;

			// Token: 0x04004A50 RID: 19024
			public bool didHit;

			// Token: 0x04004A51 RID: 19025
			public Vector3 pointHit;

			// Token: 0x04004A52 RID: 19026
			public Vector3 normalHit;
		}

		// Token: 0x02000B41 RID: 2881
		private struct HandHoldState
		{
			// Token: 0x04004A53 RID: 19027
			public GorillaGrabber grabber;

			// Token: 0x04004A54 RID: 19028
			public Transform objectHeld;

			// Token: 0x04004A55 RID: 19029
			public Vector3 localPositionHeld;

			// Token: 0x04004A56 RID: 19030
			public float localRotationalOffset;

			// Token: 0x04004A57 RID: 19031
			public bool applyRotation;
		}
	}
}
