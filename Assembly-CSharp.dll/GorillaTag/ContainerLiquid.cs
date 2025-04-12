using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B7D RID: 2941
	[AddComponentMenu("GorillaTag/ContainerLiquid (GTag)")]
	[ExecuteInEditMode]
	public class ContainerLiquid : MonoBehaviour
	{
		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06004A71 RID: 19057 RVA: 0x0006022F File Offset: 0x0005E42F
		[DebugReadout]
		public bool isEmpty
		{
			get
			{
				return this.fillAmount <= this.refillThreshold;
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06004A72 RID: 19058 RVA: 0x00060242 File Offset: 0x0005E442
		// (set) Token: 0x06004A73 RID: 19059 RVA: 0x0006024A File Offset: 0x0005E44A
		public Vector3 cupTopWorldPos { get; private set; }

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06004A74 RID: 19060 RVA: 0x00060253 File Offset: 0x0005E453
		// (set) Token: 0x06004A75 RID: 19061 RVA: 0x0006025B File Offset: 0x0005E45B
		public Vector3 bottomLipWorldPos { get; private set; }

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06004A76 RID: 19062 RVA: 0x00060264 File Offset: 0x0005E464
		// (set) Token: 0x06004A77 RID: 19063 RVA: 0x0006026C File Offset: 0x0005E46C
		public Vector3 liquidPlaneWorldPos { get; private set; }

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06004A78 RID: 19064 RVA: 0x00060275 File Offset: 0x0005E475
		// (set) Token: 0x06004A79 RID: 19065 RVA: 0x0006027D File Offset: 0x0005E47D
		public Vector3 liquidPlaneWorldNormal { get; private set; }

		// Token: 0x06004A7A RID: 19066 RVA: 0x0019B5AC File Offset: 0x001997AC
		protected bool IsValidLiquidSurfaceValues()
		{
			return this.meshRenderer != null && this.meshFilter != null && this.spillParticleSystem != null && !string.IsNullOrEmpty(this.liquidColorShaderPropertyName) && !string.IsNullOrEmpty(this.liquidPlaneNormalShaderPropertyName) && !string.IsNullOrEmpty(this.liquidPlanePositionShaderPropertyName);
		}

		// Token: 0x06004A7B RID: 19067 RVA: 0x0019B610 File Offset: 0x00199810
		protected void InitializeLiquidSurface()
		{
			this.liquidColorShaderProp = Shader.PropertyToID(this.liquidColorShaderPropertyName);
			this.liquidPlaneNormalShaderProp = Shader.PropertyToID(this.liquidPlaneNormalShaderPropertyName);
			this.liquidPlanePositionShaderProp = Shader.PropertyToID(this.liquidPlanePositionShaderPropertyName);
			this.localMeshBounds = this.meshFilter.sharedMesh.bounds;
		}

		// Token: 0x06004A7C RID: 19068 RVA: 0x0019B668 File Offset: 0x00199868
		protected void InitializeParticleSystem()
		{
			this.spillParticleSystem.main.startColor = this.liquidColor;
		}

		// Token: 0x06004A7D RID: 19069 RVA: 0x00060286 File Offset: 0x0005E486
		protected void Awake()
		{
			this.matPropBlock = new MaterialPropertyBlock();
			this.topVerts = this.GetTopVerts();
		}

		// Token: 0x06004A7E RID: 19070 RVA: 0x0019B694 File Offset: 0x00199894
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

		// Token: 0x06004A7F RID: 19071 RVA: 0x0019B6E8 File Offset: 0x001998E8
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

		// Token: 0x06004A80 RID: 19072 RVA: 0x0019BC78 File Offset: 0x00199E78
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

		// Token: 0x06004A81 RID: 19073 RVA: 0x0019BCD4 File Offset: 0x00199ED4
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

		// Token: 0x04004BFC RID: 19452
		[Tooltip("Used to determine the world space bounds of the container.")]
		public MeshRenderer meshRenderer;

		// Token: 0x04004BFD RID: 19453
		[Tooltip("Used to determine the local space bounds of the container.")]
		public MeshFilter meshFilter;

		// Token: 0x04004BFE RID: 19454
		[Tooltip("If you are only using the liquid mesh to calculate the volume of the container and do not need visuals then set this to true.")]
		public bool keepMeshHidden;

		// Token: 0x04004BFF RID: 19455
		[Tooltip("The object that will float on top of the liquid.")]
		public Transform floater;

		// Token: 0x04004C00 RID: 19456
		public bool useLiquidShader = true;

		// Token: 0x04004C01 RID: 19457
		public bool useLiquidVolume;

		// Token: 0x04004C02 RID: 19458
		public Vector2 liquidVolumeMinMax = Vector2.up;

		// Token: 0x04004C03 RID: 19459
		public string liquidColorShaderPropertyName = "_BaseColor";

		// Token: 0x04004C04 RID: 19460
		public string liquidPlaneNormalShaderPropertyName = "_LiquidPlaneNormal";

		// Token: 0x04004C05 RID: 19461
		public string liquidPlanePositionShaderPropertyName = "_LiquidPlanePosition";

		// Token: 0x04004C06 RID: 19462
		[Tooltip("Emits drips when pouring.")]
		public ParticleSystem spillParticleSystem;

		// Token: 0x04004C07 RID: 19463
		[SoundBankInfo]
		public SoundBankPlayer emptySoundBankPlayer;

		// Token: 0x04004C08 RID: 19464
		[SoundBankInfo]
		public SoundBankPlayer refillSoundBankPlayer;

		// Token: 0x04004C09 RID: 19465
		[SoundBankInfo]
		public SoundBankPlayer spillSoundBankPlayer;

		// Token: 0x04004C0A RID: 19466
		public Color liquidColor = new Color(0.33f, 0.25f, 0.21f, 1f);

		// Token: 0x04004C0B RID: 19467
		[Tooltip("The amount of liquid currently in the container. This value is passed to the shader.")]
		[Range(0f, 1f)]
		public float fillAmount = 0.85f;

		// Token: 0x04004C0C RID: 19468
		[Tooltip("This is what fillAmount will be after automatic refilling.")]
		public float refillAmount = 0.85f;

		// Token: 0x04004C0D RID: 19469
		[Tooltip("Set to a negative value to disable.")]
		public float refillDelay = 10f;

		// Token: 0x04004C0E RID: 19470
		[Tooltip("The point that the liquid should be considered empty and should be auto refilled.")]
		public float refillThreshold = 0.1f;

		// Token: 0x04004C0F RID: 19471
		public float wobbleMax = 0.2f;

		// Token: 0x04004C10 RID: 19472
		public float wobbleFrequency = 1f;

		// Token: 0x04004C11 RID: 19473
		public float recovery = 1f;

		// Token: 0x04004C12 RID: 19474
		public float thickness = 1f;

		// Token: 0x04004C13 RID: 19475
		public float maxSpillRate = 100f;

		// Token: 0x04004C18 RID: 19480
		[DebugReadout]
		private bool wasEmptyLastFrame;

		// Token: 0x04004C19 RID: 19481
		private int liquidColorShaderProp;

		// Token: 0x04004C1A RID: 19482
		private int liquidPlaneNormalShaderProp;

		// Token: 0x04004C1B RID: 19483
		private int liquidPlanePositionShaderProp;

		// Token: 0x04004C1C RID: 19484
		private float refillTimer;

		// Token: 0x04004C1D RID: 19485
		private float lastSineWave;

		// Token: 0x04004C1E RID: 19486
		private float lastWobble;

		// Token: 0x04004C1F RID: 19487
		private Vector2 temporalWobbleAmp;

		// Token: 0x04004C20 RID: 19488
		private Vector3 lastPos;

		// Token: 0x04004C21 RID: 19489
		private Vector3 lastVelocity;

		// Token: 0x04004C22 RID: 19490
		private Vector3 lastAngularVelocity;

		// Token: 0x04004C23 RID: 19491
		private Quaternion lastRot;

		// Token: 0x04004C24 RID: 19492
		private MaterialPropertyBlock matPropBlock;

		// Token: 0x04004C25 RID: 19493
		private Bounds localMeshBounds;

		// Token: 0x04004C26 RID: 19494
		private bool useFloater;

		// Token: 0x04004C27 RID: 19495
		private Vector3[] topVerts;
	}
}
