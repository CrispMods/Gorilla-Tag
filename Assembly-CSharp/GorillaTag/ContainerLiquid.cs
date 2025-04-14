using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B7A RID: 2938
	[AddComponentMenu("GorillaTag/ContainerLiquid (GTag)")]
	[ExecuteInEditMode]
	public class ContainerLiquid : MonoBehaviour
	{
		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06004A65 RID: 19045 RVA: 0x001689B4 File Offset: 0x00166BB4
		[DebugReadout]
		public bool isEmpty
		{
			get
			{
				return this.fillAmount <= this.refillThreshold;
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06004A66 RID: 19046 RVA: 0x001689C7 File Offset: 0x00166BC7
		// (set) Token: 0x06004A67 RID: 19047 RVA: 0x001689CF File Offset: 0x00166BCF
		public Vector3 cupTopWorldPos { get; private set; }

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06004A68 RID: 19048 RVA: 0x001689D8 File Offset: 0x00166BD8
		// (set) Token: 0x06004A69 RID: 19049 RVA: 0x001689E0 File Offset: 0x00166BE0
		public Vector3 bottomLipWorldPos { get; private set; }

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06004A6A RID: 19050 RVA: 0x001689E9 File Offset: 0x00166BE9
		// (set) Token: 0x06004A6B RID: 19051 RVA: 0x001689F1 File Offset: 0x00166BF1
		public Vector3 liquidPlaneWorldPos { get; private set; }

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06004A6C RID: 19052 RVA: 0x001689FA File Offset: 0x00166BFA
		// (set) Token: 0x06004A6D RID: 19053 RVA: 0x00168A02 File Offset: 0x00166C02
		public Vector3 liquidPlaneWorldNormal { get; private set; }

		// Token: 0x06004A6E RID: 19054 RVA: 0x00168A0C File Offset: 0x00166C0C
		protected bool IsValidLiquidSurfaceValues()
		{
			return this.meshRenderer != null && this.meshFilter != null && this.spillParticleSystem != null && !string.IsNullOrEmpty(this.liquidColorShaderPropertyName) && !string.IsNullOrEmpty(this.liquidPlaneNormalShaderPropertyName) && !string.IsNullOrEmpty(this.liquidPlanePositionShaderPropertyName);
		}

		// Token: 0x06004A6F RID: 19055 RVA: 0x00168A70 File Offset: 0x00166C70
		protected void InitializeLiquidSurface()
		{
			this.liquidColorShaderProp = Shader.PropertyToID(this.liquidColorShaderPropertyName);
			this.liquidPlaneNormalShaderProp = Shader.PropertyToID(this.liquidPlaneNormalShaderPropertyName);
			this.liquidPlanePositionShaderProp = Shader.PropertyToID(this.liquidPlanePositionShaderPropertyName);
			this.localMeshBounds = this.meshFilter.sharedMesh.bounds;
		}

		// Token: 0x06004A70 RID: 19056 RVA: 0x00168AC8 File Offset: 0x00166CC8
		protected void InitializeParticleSystem()
		{
			this.spillParticleSystem.main.startColor = this.liquidColor;
		}

		// Token: 0x06004A71 RID: 19057 RVA: 0x00168AF3 File Offset: 0x00166CF3
		protected void Awake()
		{
			this.matPropBlock = new MaterialPropertyBlock();
			this.topVerts = this.GetTopVerts();
		}

		// Token: 0x06004A72 RID: 19058 RVA: 0x00168B0C File Offset: 0x00166D0C
		protected void OnEnable()
		{
			if (Application.isPlaying)
			{
				base.enabled = (this.useLiquidShader && this.IsValidLiquidSurfaceValues());
				if (base.enabled)
				{
					this.InitializeLiquidSurface();
				}
				this.InitializeParticleSystem();
				this.useFloater = (this.floater != null);
			}
		}

		// Token: 0x06004A73 RID: 19059 RVA: 0x00168B60 File Offset: 0x00166D60
		protected void LateUpdate()
		{
			this.UpdateRefillTimer();
			Transform transform = base.transform;
			Vector3 position = transform.position;
			Quaternion rotation = transform.rotation;
			Bounds bounds = this.meshRenderer.bounds;
			Vector3 a = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
			Vector3 b = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
			this.liquidPlaneWorldPos = Vector3.Lerp(a, b, this.fillAmount);
			Vector3 v = transform.InverseTransformPoint(this.liquidPlaneWorldPos);
			float deltaTime = Time.deltaTime;
			this.temporalWobbleAmp = Vector2.Lerp(this.temporalWobbleAmp, Vector2.zero, deltaTime * this.recovery);
			float num = 6.2831855f * this.wobbleFrequency;
			float d = Mathf.Lerp(this.lastSineWave, Mathf.Sin(num * Time.realtimeSinceStartup), deltaTime * Mathf.Clamp(this.lastVelocity.magnitude + this.lastAngularVelocity.magnitude, this.thickness, 10f));
			Vector2 vector = this.temporalWobbleAmp * d;
			this.liquidPlaneWorldNormal = new Vector3(vector.x, -1f, vector.y).normalized;
			Vector3 v2 = transform.InverseTransformDirection(this.liquidPlaneWorldNormal);
			if (this.useLiquidShader)
			{
				this.matPropBlock.SetVector(this.liquidPlaneNormalShaderProp, v2);
				this.matPropBlock.SetVector(this.liquidPlanePositionShaderProp, v);
				this.matPropBlock.SetVector(this.liquidColorShaderProp, this.liquidColor.linear);
				if (this.useLiquidVolume)
				{
					float value = MathUtils.Linear(this.fillAmount, 0f, 1f, this.liquidVolumeMinMax.x, this.liquidVolumeMinMax.y);
					this.matPropBlock.SetFloat(Shader.PropertyToID("_LiquidFill"), value);
				}
				this.meshRenderer.SetPropertyBlock(this.matPropBlock);
			}
			if (this.useFloater)
			{
				float y = Mathf.Lerp(this.localMeshBounds.min.y, this.localMeshBounds.max.y, this.fillAmount);
				this.floater.localPosition = this.floater.localPosition.WithY(y);
			}
			Vector3 vector2 = (this.lastPos - position) / deltaTime;
			Vector3 angularVelocity = GorillaMath.GetAngularVelocity(this.lastRot, rotation);
			this.temporalWobbleAmp.x = this.temporalWobbleAmp.x + Mathf.Clamp((vector2.x + vector2.y * 0.2f + angularVelocity.z + angularVelocity.y) * this.wobbleMax, -this.wobbleMax, this.wobbleMax);
			this.temporalWobbleAmp.y = this.temporalWobbleAmp.y + Mathf.Clamp((vector2.z + vector2.y * 0.2f + angularVelocity.x + angularVelocity.y) * this.wobbleMax, -this.wobbleMax, this.wobbleMax);
			this.lastPos = position;
			this.lastRot = rotation;
			this.lastSineWave = d;
			this.lastVelocity = vector2;
			this.lastAngularVelocity = angularVelocity;
			this.meshRenderer.enabled = (!this.keepMeshHidden && !this.isEmpty);
			float x = transform.lossyScale.x;
			float num2 = this.localMeshBounds.extents.x * x;
			float y2 = this.localMeshBounds.extents.y;
			Vector3 position2 = this.localMeshBounds.center + new Vector3(0f, y2, 0f);
			this.cupTopWorldPos = transform.TransformPoint(position2);
			Vector3 up = transform.up;
			Vector3 rhs = transform.InverseTransformDirection(Vector3.down);
			float num3 = float.MinValue;
			Vector3 position3 = Vector3.zero;
			for (int i = 0; i < this.topVerts.Length; i++)
			{
				float num4 = Vector3.Dot(this.topVerts[i], rhs);
				if (num4 > num3)
				{
					num3 = num4;
					position3 = this.topVerts[i];
				}
			}
			this.bottomLipWorldPos = transform.TransformPoint(position3);
			float num5 = Mathf.Clamp01((this.liquidPlaneWorldPos.y - this.bottomLipWorldPos.y) / (num2 * 2f));
			bool flag = num5 > 1E-05f;
			ParticleSystem.EmissionModule emission = this.spillParticleSystem.emission;
			emission.enabled = flag;
			if (flag)
			{
				if (!this.spillSoundBankPlayer.isPlaying)
				{
					this.spillSoundBankPlayer.Play();
				}
				this.spillParticleSystem.transform.position = Vector3.Lerp(this.bottomLipWorldPos, this.cupTopWorldPos, num5);
				this.spillParticleSystem.shape.radius = num2 * num5;
				ParticleSystem.MinMaxCurve rateOverTime = emission.rateOverTime;
				float num6 = num5 * this.maxSpillRate;
				rateOverTime.constant = num6;
				emission.rateOverTime = rateOverTime;
				this.fillAmount -= num6 * deltaTime * 0.01f;
			}
			if (this.isEmpty && !this.wasEmptyLastFrame && !this.emptySoundBankPlayer.isPlaying)
			{
				this.emptySoundBankPlayer.Play();
			}
			else if (!this.isEmpty && this.wasEmptyLastFrame && !this.refillSoundBankPlayer.isPlaying)
			{
				this.refillSoundBankPlayer.Play();
			}
			this.wasEmptyLastFrame = this.isEmpty;
		}

		// Token: 0x06004A74 RID: 19060 RVA: 0x001690F0 File Offset: 0x001672F0
		public void UpdateRefillTimer()
		{
			if (this.refillDelay < 0f || !this.isEmpty)
			{
				return;
			}
			if (this.refillTimer < 0f)
			{
				this.refillTimer = this.refillDelay;
				this.fillAmount = this.refillAmount;
				return;
			}
			this.refillTimer -= Time.deltaTime;
		}

		// Token: 0x06004A75 RID: 19061 RVA: 0x0016914C File Offset: 0x0016734C
		private Vector3[] GetTopVerts()
		{
			Vector3[] vertices = this.meshFilter.sharedMesh.vertices;
			List<Vector3> list = new List<Vector3>(vertices.Length);
			float num = float.MinValue;
			foreach (Vector3 vector in vertices)
			{
				if (vector.y > num)
				{
					num = vector.y;
				}
			}
			foreach (Vector3 vector2 in vertices)
			{
				if (Mathf.Abs(vector2.y - num) < 0.001f)
				{
					list.Add(vector2);
				}
			}
			return list.ToArray();
		}

		// Token: 0x04004BEA RID: 19434
		[Tooltip("Used to determine the world space bounds of the container.")]
		public MeshRenderer meshRenderer;

		// Token: 0x04004BEB RID: 19435
		[Tooltip("Used to determine the local space bounds of the container.")]
		public MeshFilter meshFilter;

		// Token: 0x04004BEC RID: 19436
		[Tooltip("If you are only using the liquid mesh to calculate the volume of the container and do not need visuals then set this to true.")]
		public bool keepMeshHidden;

		// Token: 0x04004BED RID: 19437
		[Tooltip("The object that will float on top of the liquid.")]
		public Transform floater;

		// Token: 0x04004BEE RID: 19438
		public bool useLiquidShader = true;

		// Token: 0x04004BEF RID: 19439
		public bool useLiquidVolume;

		// Token: 0x04004BF0 RID: 19440
		public Vector2 liquidVolumeMinMax = Vector2.up;

		// Token: 0x04004BF1 RID: 19441
		public string liquidColorShaderPropertyName = "_BaseColor";

		// Token: 0x04004BF2 RID: 19442
		public string liquidPlaneNormalShaderPropertyName = "_LiquidPlaneNormal";

		// Token: 0x04004BF3 RID: 19443
		public string liquidPlanePositionShaderPropertyName = "_LiquidPlanePosition";

		// Token: 0x04004BF4 RID: 19444
		[Tooltip("Emits drips when pouring.")]
		public ParticleSystem spillParticleSystem;

		// Token: 0x04004BF5 RID: 19445
		[SoundBankInfo]
		public SoundBankPlayer emptySoundBankPlayer;

		// Token: 0x04004BF6 RID: 19446
		[SoundBankInfo]
		public SoundBankPlayer refillSoundBankPlayer;

		// Token: 0x04004BF7 RID: 19447
		[SoundBankInfo]
		public SoundBankPlayer spillSoundBankPlayer;

		// Token: 0x04004BF8 RID: 19448
		public Color liquidColor = new Color(0.33f, 0.25f, 0.21f, 1f);

		// Token: 0x04004BF9 RID: 19449
		[Tooltip("The amount of liquid currently in the container. This value is passed to the shader.")]
		[Range(0f, 1f)]
		public float fillAmount = 0.85f;

		// Token: 0x04004BFA RID: 19450
		[Tooltip("This is what fillAmount will be after automatic refilling.")]
		public float refillAmount = 0.85f;

		// Token: 0x04004BFB RID: 19451
		[Tooltip("Set to a negative value to disable.")]
		public float refillDelay = 10f;

		// Token: 0x04004BFC RID: 19452
		[Tooltip("The point that the liquid should be considered empty and should be auto refilled.")]
		public float refillThreshold = 0.1f;

		// Token: 0x04004BFD RID: 19453
		public float wobbleMax = 0.2f;

		// Token: 0x04004BFE RID: 19454
		public float wobbleFrequency = 1f;

		// Token: 0x04004BFF RID: 19455
		public float recovery = 1f;

		// Token: 0x04004C00 RID: 19456
		public float thickness = 1f;

		// Token: 0x04004C01 RID: 19457
		public float maxSpillRate = 100f;

		// Token: 0x04004C06 RID: 19462
		[DebugReadout]
		private bool wasEmptyLastFrame;

		// Token: 0x04004C07 RID: 19463
		private int liquidColorShaderProp;

		// Token: 0x04004C08 RID: 19464
		private int liquidPlaneNormalShaderProp;

		// Token: 0x04004C09 RID: 19465
		private int liquidPlanePositionShaderProp;

		// Token: 0x04004C0A RID: 19466
		private float refillTimer;

		// Token: 0x04004C0B RID: 19467
		private float lastSineWave;

		// Token: 0x04004C0C RID: 19468
		private float lastWobble;

		// Token: 0x04004C0D RID: 19469
		private Vector2 temporalWobbleAmp;

		// Token: 0x04004C0E RID: 19470
		private Vector3 lastPos;

		// Token: 0x04004C0F RID: 19471
		private Vector3 lastVelocity;

		// Token: 0x04004C10 RID: 19472
		private Vector3 lastAngularVelocity;

		// Token: 0x04004C11 RID: 19473
		private Quaternion lastRot;

		// Token: 0x04004C12 RID: 19474
		private MaterialPropertyBlock matPropBlock;

		// Token: 0x04004C13 RID: 19475
		private Bounds localMeshBounds;

		// Token: 0x04004C14 RID: 19476
		private bool useFloater;

		// Token: 0x04004C15 RID: 19477
		private Vector3[] topVerts;
	}
}
