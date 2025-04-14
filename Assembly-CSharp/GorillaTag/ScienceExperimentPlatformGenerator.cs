using System;
using System.Collections.Generic;
using GorillaExtensions;
using GorillaTag.GuidedRefs;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag
{
	// Token: 0x02000BAC RID: 2988
	public class ScienceExperimentPlatformGenerator : MonoBehaviourPun, ITickSystemPost, IGuidedRefReceiverMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x06004B87 RID: 19335 RVA: 0x0016F4FB File Offset: 0x0016D6FB
		private void Awake()
		{
			((IGuidedRefObject)this).GuidedRefInitialize();
			this.scienceExperimentManager = base.GetComponent<ScienceExperimentManager>();
		}

		// Token: 0x06004B88 RID: 19336 RVA: 0x0016F50F File Offset: 0x0016D70F
		private void OnEnable()
		{
			if (((IGuidedRefReceiverMono)this).GuidedRefsWaitingToResolveCount > 0)
			{
				return;
			}
			TickSystem<object>.AddPostTickCallback(this);
		}

		// Token: 0x06004B89 RID: 19337 RVA: 0x000C141F File Offset: 0x000BF61F
		protected void OnDisable()
		{
			TickSystem<object>.RemovePostTickCallback(this);
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06004B8A RID: 19338 RVA: 0x0016F521 File Offset: 0x0016D721
		// (set) Token: 0x06004B8B RID: 19339 RVA: 0x0016F529 File Offset: 0x0016D729
		bool ITickSystemPost.PostTickRunning { get; set; }

		// Token: 0x06004B8C RID: 19340 RVA: 0x0016F534 File Offset: 0x0016D734
		void ITickSystemPost.PostTick()
		{
			double currentTime = PhotonNetwork.InRoom ? PhotonNetwork.Time : Time.unscaledTimeAsDouble;
			this.UpdateTrails(currentTime);
			this.RemoveExpiredBubbles(currentTime);
			this.SpawnNewBubbles(currentTime);
			this.UpdateActiveBubbles(currentTime);
		}

		// Token: 0x06004B8D RID: 19341 RVA: 0x0016F574 File Offset: 0x0016D774
		private void RemoveExpiredBubbles(double currentTime)
		{
			for (int i = this.activeBubbles.Count - 1; i >= 0; i--)
			{
				if (Mathf.Clamp01((float)(currentTime - this.activeBubbles[i].spawnTime) / this.activeBubbles[i].lifetime) >= 1f)
				{
					this.activeBubbles[i].bubble.Pop();
					this.activeBubbles.RemoveAt(i);
				}
			}
		}

		// Token: 0x06004B8E RID: 19342 RVA: 0x0016F5F0 File Offset: 0x0016D7F0
		private void SpawnNewBubbles(double currentTime)
		{
			if (base.photonView.IsMine && this.scienceExperimentManager.GameState == ScienceExperimentManager.RisingLiquidState.Rising)
			{
				int num = Mathf.Min((int)(this.rockCountVsLavaProgress.Evaluate(this.scienceExperimentManager.RiseProgressLinear) * this.bubbleCountMultiplier), this.maxBubbleCount) - this.activeBubbles.Count;
				if (this.activeBubbles.Count < this.maxBubbleCount)
				{
					for (int i = 0; i < num; i++)
					{
						this.SpawnRockAuthority(currentTime, this.scienceExperimentManager.RiseProgressLinear);
					}
				}
			}
		}

		// Token: 0x06004B8F RID: 19343 RVA: 0x0016F680 File Offset: 0x0016D880
		private void UpdateActiveBubbles(double currentTime)
		{
			if (this.liquidSurfacePlane == null)
			{
				return;
			}
			float y = this.liquidSurfacePlane.transform.position.y;
			float num = this.bubblePopWobbleAmplitude * Mathf.Sin(this.bubblePopWobbleFrequency * 0.5f * 3.1415927f * Time.time);
			for (int i = 0; i < this.activeBubbles.Count; i++)
			{
				ScienceExperimentPlatformGenerator.BubbleData bubbleData = this.activeBubbles[i];
				float time = Mathf.Clamp01((float)(currentTime - bubbleData.spawnTime) / bubbleData.lifetime);
				float d = bubbleData.spawnSize * this.rockSizeVsLifetime.Evaluate(time) * this.scaleFactor;
				bubbleData.position.y = y;
				bubbleData.bubble.body.gameObject.transform.localScale = Vector3.one * d;
				bubbleData.bubble.body.MovePosition(bubbleData.position);
				float num2 = (float)((double)bubbleData.lifetime + bubbleData.spawnTime - currentTime);
				if (num2 < this.bubblePopAnticipationTime)
				{
					float num3 = Mathf.Clamp01(1f - num2 / this.bubblePopAnticipationTime);
					bubbleData.bubble.bubbleMesh.transform.localScale = Vector3.one * (1f + num3 * num);
				}
				this.activeBubbles[i] = bubbleData;
			}
		}

		// Token: 0x06004B90 RID: 19344 RVA: 0x0016F7E8 File Offset: 0x0016D9E8
		private void UpdateTrails(double currentTime)
		{
			if (base.photonView.IsMine)
			{
				int num = (int)(this.trailCountVsProgress.Evaluate(this.scienceExperimentManager.RiseProgressLinear) * this.trailCountMultiplier) - this.trailHeads.Count;
				if (num > 0 && this.scienceExperimentManager.GameState == ScienceExperimentManager.RisingLiquidState.Rising)
				{
					for (int i = 0; i < num; i++)
					{
						this.SpawnTrailAuthority(currentTime, this.scienceExperimentManager.RiseProgressLinear);
					}
				}
				else if (num < 0)
				{
					for (int j = 0; j > num; j--)
					{
						this.trailHeads.RemoveAt(0);
					}
				}
				float num2 = this.trailSpawnRateVsProgress.Evaluate(this.scienceExperimentManager.RiseProgressLinear) * this.trailSpawnRateMultiplier;
				float num3 = this.trailBubbleBoundaryRadiusVsProgress.Evaluate(this.scienceExperimentManager.RiseProgressLinear) * this.surfaceRadiusSpawnRange.y;
				for (int k = this.trailHeads.Count - 1; k >= 0; k--)
				{
					if ((float)(currentTime - this.trailHeads[k].spawnTime) > num2)
					{
						float num4 = -this.trailMaxTurnAngle;
						float num5 = this.trailMaxTurnAngle;
						float num6 = Vector3.SignedAngle(this.trailHeads[k].direction, this.trailHeads[k].position - this.liquidSurfacePlane.transform.position, Vector3.up);
						float num7 = num3 - Vector3.Distance(this.trailHeads[k].position, this.liquidSurfacePlane.transform.position);
						if (num7 < this.trailEdgeAvoidanceSpawnsMinMax.x * this.trailDistanceBetweenSpawns * this.scaleFactor)
						{
							float num8 = Mathf.InverseLerp(this.trailEdgeAvoidanceSpawnsMinMax.x * this.trailDistanceBetweenSpawns * this.scaleFactor, this.trailEdgeAvoidanceSpawnsMinMax.y * this.trailDistanceBetweenSpawns * this.scaleFactor, num7);
							if (num6 > 0f)
							{
								float b = num6 - 90f * num8;
								num5 = Mathf.Min(num5, b);
								num4 = Mathf.Min(num4, num5 - this.trailMaxTurnAngle);
							}
							else
							{
								float b2 = num6 + 90f * num8;
								num4 = Mathf.Max(num4, b2);
								num5 = Mathf.Max(num5, num4 + this.trailMaxTurnAngle);
							}
						}
						Vector3 vector = Quaternion.AngleAxis(Random.Range(num4, num5), Vector3.up) * this.trailHeads[k].direction;
						Vector3 vector2 = this.trailHeads[k].position + vector * this.trailDistanceBetweenSpawns * this.scaleFactor - this.liquidSurfacePlane.transform.position;
						if (vector2.sqrMagnitude > this.surfaceRadiusSpawnRange.y * this.surfaceRadiusSpawnRange.y)
						{
							vector2 = vector2.normalized * this.surfaceRadiusSpawnRange.y;
						}
						Vector2 vector3 = new Vector2(vector2.x, vector2.z);
						float num9 = this.trailBubbleSize;
						float num10 = this.trailBubbleLifetimeVsProgress.Evaluate(this.scienceExperimentManager.RiseProgressLinear) * this.trailBubbleLifetimeMultiplier;
						this.trailHeads.RemoveAt(k);
						base.photonView.RPC("SpawnSodaBubbleRPC", RpcTarget.Others, new object[]
						{
							vector3,
							num9,
							num10,
							currentTime
						});
						this.SpawnSodaBubbleLocal(vector3, num9, num10, currentTime, true, vector);
					}
				}
			}
		}

		// Token: 0x06004B91 RID: 19345 RVA: 0x0016FB80 File Offset: 0x0016DD80
		private void SpawnRockAuthority(double currentTime, float lavaProgress)
		{
			if (base.photonView.IsMine)
			{
				float num = this.rockLifetimeMultiplierVsLavaProgress.Evaluate(lavaProgress);
				float num2 = this.rockMaxSizeMultiplierVsLavaProgress.Evaluate(lavaProgress);
				float num3 = Random.Range(this.lifetimeRange.x, this.lifetimeRange.y) * num;
				float num4 = Random.Range(this.sizeRange.x, this.sizeRange.y * num2);
				float d = this.spawnRadiusMultiplierVsLavaProgress.Evaluate(lavaProgress);
				Vector2 vector = Random.insideUnitCircle.normalized * Random.Range(this.surfaceRadiusSpawnRange.x, this.surfaceRadiusSpawnRange.y) * d;
				vector = this.GetSpawnPositionWithClearance(vector, num4 * this.scaleFactor, this.surfaceRadiusSpawnRange.y, this.liquidSurfacePlane.transform.position);
				base.photonView.RPC("SpawnSodaBubbleRPC", RpcTarget.Others, new object[]
				{
					vector,
					num4,
					num3,
					currentTime
				});
				this.SpawnSodaBubbleLocal(vector, num4, num3, currentTime, false, default(Vector3));
			}
		}

		// Token: 0x06004B92 RID: 19346 RVA: 0x0016FCB8 File Offset: 0x0016DEB8
		private void SpawnTrailAuthority(double currentTime, float lavaProgress)
		{
			if (base.photonView.IsMine)
			{
				float num = this.trailBubbleLifetimeVsProgress.Evaluate(this.scienceExperimentManager.RiseProgressLinear) * this.trailBubbleLifetimeMultiplier;
				float num2 = this.trailBubbleSize;
				Vector2 vector = Random.insideUnitCircle.normalized * Random.Range(this.surfaceRadiusSpawnRange.x, this.surfaceRadiusSpawnRange.y);
				vector = this.GetSpawnPositionWithClearance(vector, num2 * this.scaleFactor, this.surfaceRadiusSpawnRange.y, this.liquidSurfacePlane.transform.position);
				Vector3 direction = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * Vector3.forward;
				base.photonView.RPC("SpawnSodaBubbleRPC", RpcTarget.Others, new object[]
				{
					vector,
					num2,
					num,
					currentTime
				});
				this.SpawnSodaBubbleLocal(vector, num2, num, currentTime, true, direction);
			}
		}

		// Token: 0x06004B93 RID: 19347 RVA: 0x0016FDC0 File Offset: 0x0016DFC0
		private void SpawnSodaBubbleLocal(Vector2 surfacePosLocal, float spawnSize, float lifetime, double spawnTime, bool addAsTrail = false, Vector3 direction = default(Vector3))
		{
			if (this.activeBubbles.Count < this.maxBubbleCount)
			{
				Vector3 position = this.liquidSurfacePlane.transform.position + new Vector3(surfacePosLocal.x, 0f, surfacePosLocal.y);
				ScienceExperimentPlatformGenerator.BubbleData bubbleData = new ScienceExperimentPlatformGenerator.BubbleData
				{
					position = position,
					spawnSize = spawnSize,
					lifetime = lifetime,
					spawnTime = spawnTime,
					isTrail = false
				};
				bubbleData.bubble = ObjectPools.instance.Instantiate(this.spawnedPrefab, bubbleData.position, Quaternion.identity, 0f).GetComponent<SodaBubble>();
				if (base.photonView.IsMine && addAsTrail)
				{
					bubbleData.direction = direction;
					bubbleData.isTrail = true;
					this.trailHeads.Add(bubbleData);
				}
				this.activeBubbles.Add(bubbleData);
			}
		}

		// Token: 0x06004B94 RID: 19348 RVA: 0x0016FEA8 File Offset: 0x0016E0A8
		[PunRPC]
		public void SpawnSodaBubbleRPC(Vector2 surfacePosLocal, float spawnSize, float lifetime, double spawnTime, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "SpawnSodaBubbleRPC");
			if (info.Sender == PhotonNetwork.MasterClient)
			{
				if (!float.IsFinite(spawnSize) || !float.IsFinite(lifetime) || !double.IsFinite(spawnTime))
				{
					return;
				}
				float time = Mathf.Clamp01(this.scienceExperimentManager.RiseProgressLinear);
				ref surfacePosLocal.ClampThisMagnitudeSafe(this.surfaceRadiusSpawnRange.y);
				spawnSize = Mathf.Clamp(spawnSize, this.sizeRange.x, this.sizeRange.y * this.rockMaxSizeMultiplierVsLavaProgress.Evaluate(time));
				lifetime = Mathf.Clamp(lifetime, this.lifetimeRange.x, this.lifetimeRange.y * this.rockLifetimeMultiplierVsLavaProgress.Evaluate(time));
				double num = PhotonNetwork.InRoom ? PhotonNetwork.Time : Time.unscaledTimeAsDouble;
				spawnTime = ((Mathf.Abs((float)(spawnTime - num)) < 10f) ? spawnTime : num);
				this.SpawnSodaBubbleLocal(surfacePosLocal, spawnSize, lifetime, spawnTime, false, default(Vector3));
			}
		}

		// Token: 0x06004B95 RID: 19349 RVA: 0x0016FFA8 File Offset: 0x0016E1A8
		private Vector2 GetSpawnPositionWithClearance(Vector2 inputPosition, float inputSize, float maxDistance, Vector3 lavaSurfaceOrigin)
		{
			Vector2 vector = inputPosition;
			for (int i = 0; i < this.activeBubbles.Count; i++)
			{
				Vector3 vector2 = this.activeBubbles[i].position - lavaSurfaceOrigin;
				Vector2 b = new Vector2(vector2.x, vector2.z);
				Vector2 a = vector - b;
				float num = (inputSize + this.activeBubbles[i].spawnSize * this.scaleFactor) * 0.5f;
				if (a.sqrMagnitude < num * num)
				{
					float magnitude = a.magnitude;
					if (magnitude > 0.001f)
					{
						Vector2 a2 = a / magnitude;
						vector += a2 * (num - magnitude);
						if (vector.sqrMagnitude > maxDistance * maxDistance)
						{
							vector = vector.normalized * maxDistance;
						}
					}
				}
			}
			if (vector.sqrMagnitude > this.surfaceRadiusSpawnRange.y * this.surfaceRadiusSpawnRange.y)
			{
				vector = vector.normalized * this.surfaceRadiusSpawnRange.y;
			}
			return vector;
		}

		// Token: 0x06004B96 RID: 19350 RVA: 0x001700BB File Offset: 0x0016E2BB
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterReceiverField<ScienceExperimentPlatformGenerator>(this, "liquidSurfacePlane", ref this.liquidSurfacePlane_gRef);
			GuidedRefHub.ReceiverFullyRegistered<ScienceExperimentPlatformGenerator>(this);
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06004B97 RID: 19351 RVA: 0x001700D4 File Offset: 0x0016E2D4
		// (set) Token: 0x06004B98 RID: 19352 RVA: 0x001700DC File Offset: 0x0016E2DC
		int IGuidedRefReceiverMono.GuidedRefsWaitingToResolveCount { get; set; }

		// Token: 0x06004B99 RID: 19353 RVA: 0x001700E5 File Offset: 0x0016E2E5
		bool IGuidedRefReceiverMono.GuidedRefTryResolveReference(GuidedRefTryResolveInfo target)
		{
			return GuidedRefHub.TryResolveField<ScienceExperimentPlatformGenerator, Transform>(this, ref this.liquidSurfacePlane, this.liquidSurfacePlane_gRef, target);
		}

		// Token: 0x06004B9A RID: 19354 RVA: 0x001700FA File Offset: 0x0016E2FA
		void IGuidedRefReceiverMono.OnAllGuidedRefsResolved()
		{
			if (!base.enabled)
			{
				return;
			}
			TickSystem<object>.AddPostTickCallback(this);
		}

		// Token: 0x06004B9B RID: 19355 RVA: 0x000C141F File Offset: 0x000BF61F
		void IGuidedRefReceiverMono.OnGuidedRefTargetDestroyed(int fieldId)
		{
			TickSystem<object>.RemovePostTickCallback(this);
		}

		// Token: 0x06004B9D RID: 19357 RVA: 0x00042E29 File Offset: 0x00041029
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004B9E RID: 19358 RVA: 0x00015AA9 File Offset: 0x00013CA9
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004D2B RID: 19755
		[SerializeField]
		private GameObject spawnedPrefab;

		// Token: 0x04004D2C RID: 19756
		[SerializeField]
		private float scaleFactor = 0.03f;

		// Token: 0x04004D2D RID: 19757
		[Header("Random Bubbles")]
		[SerializeField]
		private Vector2 surfaceRadiusSpawnRange = new Vector2(0.1f, 0.7f);

		// Token: 0x04004D2E RID: 19758
		[SerializeField]
		private Vector2 lifetimeRange = new Vector2(5f, 10f);

		// Token: 0x04004D2F RID: 19759
		[SerializeField]
		private Vector2 sizeRange = new Vector2(0.5f, 2f);

		// Token: 0x04004D30 RID: 19760
		[SerializeField]
		private AnimationCurve rockCountVsLavaProgress = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004D31 RID: 19761
		[SerializeField]
		[FormerlySerializedAs("rockCountMultiplier")]
		private float bubbleCountMultiplier = 80f;

		// Token: 0x04004D32 RID: 19762
		[SerializeField]
		private int maxBubbleCount = 100;

		// Token: 0x04004D33 RID: 19763
		[SerializeField]
		private AnimationCurve rockLifetimeMultiplierVsLavaProgress = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x04004D34 RID: 19764
		[SerializeField]
		private AnimationCurve rockMaxSizeMultiplierVsLavaProgress = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x04004D35 RID: 19765
		[SerializeField]
		private AnimationCurve spawnRadiusMultiplierVsLavaProgress = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x04004D36 RID: 19766
		[SerializeField]
		private AnimationCurve rockSizeVsLifetime = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004D37 RID: 19767
		[Header("Bubble Trails")]
		[SerializeField]
		private AnimationCurve trailSpawnRateVsProgress = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004D38 RID: 19768
		[SerializeField]
		private float trailSpawnRateMultiplier = 1f;

		// Token: 0x04004D39 RID: 19769
		[SerializeField]
		private AnimationCurve trailBubbleLifetimeVsProgress = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004D3A RID: 19770
		[SerializeField]
		private AnimationCurve trailBubbleBoundaryRadiusVsProgress = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004D3B RID: 19771
		[SerializeField]
		private float trailBubbleLifetimeMultiplier = 6f;

		// Token: 0x04004D3C RID: 19772
		[SerializeField]
		private float trailDistanceBetweenSpawns = 3f;

		// Token: 0x04004D3D RID: 19773
		[SerializeField]
		private float trailMaxTurnAngle = 55f;

		// Token: 0x04004D3E RID: 19774
		[SerializeField]
		private float trailBubbleSize = 1.5f;

		// Token: 0x04004D3F RID: 19775
		[SerializeField]
		private AnimationCurve trailCountVsProgress = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004D40 RID: 19776
		[SerializeField]
		private float trailCountMultiplier = 12f;

		// Token: 0x04004D41 RID: 19777
		[SerializeField]
		private Vector2 trailEdgeAvoidanceSpawnsMinMax = new Vector2(3f, 1f);

		// Token: 0x04004D42 RID: 19778
		[Header("Feedback Effects")]
		[SerializeField]
		private float bubblePopAnticipationTime = 2f;

		// Token: 0x04004D43 RID: 19779
		[SerializeField]
		private float bubblePopWobbleFrequency = 25f;

		// Token: 0x04004D44 RID: 19780
		[SerializeField]
		private float bubblePopWobbleAmplitude = 0.01f;

		// Token: 0x04004D45 RID: 19781
		[SerializeField]
		private Transform liquidSurfacePlane;

		// Token: 0x04004D46 RID: 19782
		[SerializeField]
		private GuidedRefReceiverFieldInfo liquidSurfacePlane_gRef = new GuidedRefReceiverFieldInfo(true);

		// Token: 0x04004D47 RID: 19783
		private List<ScienceExperimentPlatformGenerator.BubbleData> activeBubbles = new List<ScienceExperimentPlatformGenerator.BubbleData>();

		// Token: 0x04004D48 RID: 19784
		private List<ScienceExperimentPlatformGenerator.BubbleData> trailHeads = new List<ScienceExperimentPlatformGenerator.BubbleData>();

		// Token: 0x04004D49 RID: 19785
		private List<ScienceExperimentPlatformGenerator.BubbleSpawnDebug> bubbleSpawnDebug = new List<ScienceExperimentPlatformGenerator.BubbleSpawnDebug>();

		// Token: 0x04004D4A RID: 19786
		private ScienceExperimentManager scienceExperimentManager;

		// Token: 0x02000BAD RID: 2989
		private struct BubbleData
		{
			// Token: 0x04004D4D RID: 19789
			public Vector3 position;

			// Token: 0x04004D4E RID: 19790
			public Vector3 direction;

			// Token: 0x04004D4F RID: 19791
			public float spawnSize;

			// Token: 0x04004D50 RID: 19792
			public float lifetime;

			// Token: 0x04004D51 RID: 19793
			public double spawnTime;

			// Token: 0x04004D52 RID: 19794
			public bool isTrail;

			// Token: 0x04004D53 RID: 19795
			public SodaBubble bubble;
		}

		// Token: 0x02000BAE RID: 2990
		private struct BubbleSpawnDebug
		{
			// Token: 0x04004D54 RID: 19796
			public Vector3 initialPosition;

			// Token: 0x04004D55 RID: 19797
			public Vector3 initialDirection;

			// Token: 0x04004D56 RID: 19798
			public Vector3 spawnPosition;

			// Token: 0x04004D57 RID: 19799
			public float minAngle;

			// Token: 0x04004D58 RID: 19800
			public float maxAngle;

			// Token: 0x04004D59 RID: 19801
			public float edgeCorrectionAngle;

			// Token: 0x04004D5A RID: 19802
			public double spawnTime;
		}
	}
}
