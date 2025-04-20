using System;
using System.Collections.Generic;
using GorillaExtensions;
using GorillaTag.GuidedRefs;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag
{
	// Token: 0x02000BD8 RID: 3032
	public class ScienceExperimentPlatformGenerator : MonoBehaviourPun, ITickSystemPost, IGuidedRefReceiverMono, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x06004CC7 RID: 19655 RVA: 0x000626BA File Offset: 0x000608BA
		private void Awake()
		{
			((IGuidedRefObject)this).GuidedRefInitialize();
			this.scienceExperimentManager = base.GetComponent<ScienceExperimentManager>();
		}

		// Token: 0x06004CC8 RID: 19656 RVA: 0x000626CE File Offset: 0x000608CE
		private void OnEnable()
		{
			if (((IGuidedRefReceiverMono)this).GuidedRefsWaitingToResolveCount > 0)
			{
				return;
			}
			TickSystem<object>.AddPostTickCallback(this);
		}

		// Token: 0x06004CC9 RID: 19657 RVA: 0x0004A56E File Offset: 0x0004876E
		protected void OnDisable()
		{
			TickSystem<object>.RemovePostTickCallback(this);
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06004CCA RID: 19658 RVA: 0x000626E0 File Offset: 0x000608E0
		// (set) Token: 0x06004CCB RID: 19659 RVA: 0x000626E8 File Offset: 0x000608E8
		bool ITickSystemPost.PostTickRunning { get; set; }

		// Token: 0x06004CCC RID: 19660 RVA: 0x001A8480 File Offset: 0x001A6680
		void ITickSystemPost.PostTick()
		{
			double currentTime = PhotonNetwork.InRoom ? PhotonNetwork.Time : Time.unscaledTimeAsDouble;
			this.UpdateTrails(currentTime);
			this.RemoveExpiredBubbles(currentTime);
			this.SpawnNewBubbles(currentTime);
			this.UpdateActiveBubbles(currentTime);
		}

		// Token: 0x06004CCD RID: 19661 RVA: 0x001A84C0 File Offset: 0x001A66C0
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

		// Token: 0x06004CCE RID: 19662 RVA: 0x001A853C File Offset: 0x001A673C
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

		// Token: 0x06004CCF RID: 19663 RVA: 0x001A85CC File Offset: 0x001A67CC
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

		// Token: 0x06004CD0 RID: 19664 RVA: 0x001A8734 File Offset: 0x001A6934
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
						Vector3 vector = Quaternion.AngleAxis(UnityEngine.Random.Range(num4, num5), Vector3.up) * this.trailHeads[k].direction;
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

		// Token: 0x06004CD1 RID: 19665 RVA: 0x001A8ACC File Offset: 0x001A6CCC
		private void SpawnRockAuthority(double currentTime, float lavaProgress)
		{
			if (base.photonView.IsMine)
			{
				float num = this.rockLifetimeMultiplierVsLavaProgress.Evaluate(lavaProgress);
				float num2 = this.rockMaxSizeMultiplierVsLavaProgress.Evaluate(lavaProgress);
				float num3 = UnityEngine.Random.Range(this.lifetimeRange.x, this.lifetimeRange.y) * num;
				float num4 = UnityEngine.Random.Range(this.sizeRange.x, this.sizeRange.y * num2);
				float d = this.spawnRadiusMultiplierVsLavaProgress.Evaluate(lavaProgress);
				Vector2 vector = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(this.surfaceRadiusSpawnRange.x, this.surfaceRadiusSpawnRange.y) * d;
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

		// Token: 0x06004CD2 RID: 19666 RVA: 0x001A8C04 File Offset: 0x001A6E04
		private void SpawnTrailAuthority(double currentTime, float lavaProgress)
		{
			if (base.photonView.IsMine)
			{
				float num = this.trailBubbleLifetimeVsProgress.Evaluate(this.scienceExperimentManager.RiseProgressLinear) * this.trailBubbleLifetimeMultiplier;
				float num2 = this.trailBubbleSize;
				Vector2 vector = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(this.surfaceRadiusSpawnRange.x, this.surfaceRadiusSpawnRange.y);
				vector = this.GetSpawnPositionWithClearance(vector, num2 * this.scaleFactor, this.surfaceRadiusSpawnRange.y, this.liquidSurfacePlane.transform.position);
				Vector3 direction = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), Vector3.up) * Vector3.forward;
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

		// Token: 0x06004CD3 RID: 19667 RVA: 0x001A8D0C File Offset: 0x001A6F0C
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

		// Token: 0x06004CD4 RID: 19668 RVA: 0x001A8DF4 File Offset: 0x001A6FF4
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

		// Token: 0x06004CD5 RID: 19669 RVA: 0x001A8EF4 File Offset: 0x001A70F4
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

		// Token: 0x06004CD6 RID: 19670 RVA: 0x000626F1 File Offset: 0x000608F1
		void IGuidedRefObject.GuidedRefInitialize()
		{
			GuidedRefHub.RegisterReceiverField<ScienceExperimentPlatformGenerator>(this, "liquidSurfacePlane", ref this.liquidSurfacePlane_gRef);
			GuidedRefHub.ReceiverFullyRegistered<ScienceExperimentPlatformGenerator>(this);
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06004CD7 RID: 19671 RVA: 0x0006270A File Offset: 0x0006090A
		// (set) Token: 0x06004CD8 RID: 19672 RVA: 0x00062712 File Offset: 0x00060912
		int IGuidedRefReceiverMono.GuidedRefsWaitingToResolveCount { get; set; }

		// Token: 0x06004CD9 RID: 19673 RVA: 0x0006271B File Offset: 0x0006091B
		bool IGuidedRefReceiverMono.GuidedRefTryResolveReference(GuidedRefTryResolveInfo target)
		{
			return GuidedRefHub.TryResolveField<ScienceExperimentPlatformGenerator, Transform>(this, ref this.liquidSurfacePlane, this.liquidSurfacePlane_gRef, target);
		}

		// Token: 0x06004CDA RID: 19674 RVA: 0x00062730 File Offset: 0x00060930
		void IGuidedRefReceiverMono.OnAllGuidedRefsResolved()
		{
			if (!base.enabled)
			{
				return;
			}
			TickSystem<object>.AddPostTickCallback(this);
		}

		// Token: 0x06004CDB RID: 19675 RVA: 0x0004A56E File Offset: 0x0004876E
		void IGuidedRefReceiverMono.OnGuidedRefTargetDestroyed(int fieldId)
		{
			TickSystem<object>.RemovePostTickCallback(this);
		}

		// Token: 0x06004CDD RID: 19677 RVA: 0x00039243 File Offset: 0x00037443
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004CDE RID: 19678 RVA: 0x00032CAE File Offset: 0x00030EAE
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004E1E RID: 19998
		[SerializeField]
		private GameObject spawnedPrefab;

		// Token: 0x04004E1F RID: 19999
		[SerializeField]
		private float scaleFactor = 0.03f;

		// Token: 0x04004E20 RID: 20000
		[Header("Random Bubbles")]
		[SerializeField]
		private Vector2 surfaceRadiusSpawnRange = new Vector2(0.1f, 0.7f);

		// Token: 0x04004E21 RID: 20001
		[SerializeField]
		private Vector2 lifetimeRange = new Vector2(5f, 10f);

		// Token: 0x04004E22 RID: 20002
		[SerializeField]
		private Vector2 sizeRange = new Vector2(0.5f, 2f);

		// Token: 0x04004E23 RID: 20003
		[SerializeField]
		private AnimationCurve rockCountVsLavaProgress = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004E24 RID: 20004
		[SerializeField]
		[FormerlySerializedAs("rockCountMultiplier")]
		private float bubbleCountMultiplier = 80f;

		// Token: 0x04004E25 RID: 20005
		[SerializeField]
		private int maxBubbleCount = 100;

		// Token: 0x04004E26 RID: 20006
		[SerializeField]
		private AnimationCurve rockLifetimeMultiplierVsLavaProgress = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x04004E27 RID: 20007
		[SerializeField]
		private AnimationCurve rockMaxSizeMultiplierVsLavaProgress = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x04004E28 RID: 20008
		[SerializeField]
		private AnimationCurve spawnRadiusMultiplierVsLavaProgress = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x04004E29 RID: 20009
		[SerializeField]
		private AnimationCurve rockSizeVsLifetime = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004E2A RID: 20010
		[Header("Bubble Trails")]
		[SerializeField]
		private AnimationCurve trailSpawnRateVsProgress = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004E2B RID: 20011
		[SerializeField]
		private float trailSpawnRateMultiplier = 1f;

		// Token: 0x04004E2C RID: 20012
		[SerializeField]
		private AnimationCurve trailBubbleLifetimeVsProgress = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004E2D RID: 20013
		[SerializeField]
		private AnimationCurve trailBubbleBoundaryRadiusVsProgress = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004E2E RID: 20014
		[SerializeField]
		private float trailBubbleLifetimeMultiplier = 6f;

		// Token: 0x04004E2F RID: 20015
		[SerializeField]
		private float trailDistanceBetweenSpawns = 3f;

		// Token: 0x04004E30 RID: 20016
		[SerializeField]
		private float trailMaxTurnAngle = 55f;

		// Token: 0x04004E31 RID: 20017
		[SerializeField]
		private float trailBubbleSize = 1.5f;

		// Token: 0x04004E32 RID: 20018
		[SerializeField]
		private AnimationCurve trailCountVsProgress = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004E33 RID: 20019
		[SerializeField]
		private float trailCountMultiplier = 12f;

		// Token: 0x04004E34 RID: 20020
		[SerializeField]
		private Vector2 trailEdgeAvoidanceSpawnsMinMax = new Vector2(3f, 1f);

		// Token: 0x04004E35 RID: 20021
		[Header("Feedback Effects")]
		[SerializeField]
		private float bubblePopAnticipationTime = 2f;

		// Token: 0x04004E36 RID: 20022
		[SerializeField]
		private float bubblePopWobbleFrequency = 25f;

		// Token: 0x04004E37 RID: 20023
		[SerializeField]
		private float bubblePopWobbleAmplitude = 0.01f;

		// Token: 0x04004E38 RID: 20024
		[SerializeField]
		private Transform liquidSurfacePlane;

		// Token: 0x04004E39 RID: 20025
		[SerializeField]
		private GuidedRefReceiverFieldInfo liquidSurfacePlane_gRef = new GuidedRefReceiverFieldInfo(true);

		// Token: 0x04004E3A RID: 20026
		private List<ScienceExperimentPlatformGenerator.BubbleData> activeBubbles = new List<ScienceExperimentPlatformGenerator.BubbleData>();

		// Token: 0x04004E3B RID: 20027
		private List<ScienceExperimentPlatformGenerator.BubbleData> trailHeads = new List<ScienceExperimentPlatformGenerator.BubbleData>();

		// Token: 0x04004E3C RID: 20028
		private List<ScienceExperimentPlatformGenerator.BubbleSpawnDebug> bubbleSpawnDebug = new List<ScienceExperimentPlatformGenerator.BubbleSpawnDebug>();

		// Token: 0x04004E3D RID: 20029
		private ScienceExperimentManager scienceExperimentManager;

		// Token: 0x02000BD9 RID: 3033
		private struct BubbleData
		{
			// Token: 0x04004E40 RID: 20032
			public Vector3 position;

			// Token: 0x04004E41 RID: 20033
			public Vector3 direction;

			// Token: 0x04004E42 RID: 20034
			public float spawnSize;

			// Token: 0x04004E43 RID: 20035
			public float lifetime;

			// Token: 0x04004E44 RID: 20036
			public double spawnTime;

			// Token: 0x04004E45 RID: 20037
			public bool isTrail;

			// Token: 0x04004E46 RID: 20038
			public SodaBubble bubble;
		}

		// Token: 0x02000BDA RID: 3034
		private struct BubbleSpawnDebug
		{
			// Token: 0x04004E47 RID: 20039
			public Vector3 initialPosition;

			// Token: 0x04004E48 RID: 20040
			public Vector3 initialDirection;

			// Token: 0x04004E49 RID: 20041
			public Vector3 spawnPosition;

			// Token: 0x04004E4A RID: 20042
			public float minAngle;

			// Token: 0x04004E4B RID: 20043
			public float maxAngle;

			// Token: 0x04004E4C RID: 20044
			public float edgeCorrectionAngle;

			// Token: 0x04004E4D RID: 20045
			public double spawnTime;
		}
	}
}
