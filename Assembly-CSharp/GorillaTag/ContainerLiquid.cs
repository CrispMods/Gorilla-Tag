using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BA7 RID: 2983
	[AddComponentMenu("GorillaTag/ContainerLiquid (GTag)")]
	[ExecuteInEditMode]
	public class ContainerLiquid : MonoBehaviour
	{
		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06004BB0 RID: 19376 RVA: 0x00061C67 File Offset: 0x0005FE67
		[DebugReadout]
		public bool isEmpty
		{
			get
			{
				return this.fillAmount <= this.refillThreshold;
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06004BB1 RID: 19377 RVA: 0x00061C7A File Offset: 0x0005FE7A
		// (set) Token: 0x06004BB2 RID: 19378 RVA: 0x00061C82 File Offset: 0x0005FE82
		public Vector3 cupTopWorldPos { get; private set; }

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06004BB3 RID: 19379 RVA: 0x00061C8B File Offset: 0x0005FE8B
		// (set) Token: 0x06004BB4 RID: 19380 RVA: 0x00061C93 File Offset: 0x0005FE93
		public Vector3 bottomLipWorldPos { get; private set; }

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06004BB5 RID: 19381 RVA: 0x00061C9C File Offset: 0x0005FE9C
		// (set) Token: 0x06004BB6 RID: 19382 RVA: 0x00061CA4 File Offset: 0x0005FEA4
		public Vector3 liquidPlaneWorldPos { get; private set; }

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06004BB7 RID: 19383 RVA: 0x00061CAD File Offset: 0x0005FEAD
		// (set) Token: 0x06004BB8 RID: 19384 RVA: 0x00061CB5 File Offset: 0x0005FEB5
		public Vector3 liquidPlaneWorldNormal { get; private set; }

		// Token: 0x06004BB9 RID: 19385 RVA: 0x001A25C4 File Offset: 0x001A07C4
		protected bool IsValidLiquidSurfaceValues()
		{
			return this.meshRenderer != null && this.meshFilter != null && this.spillParticleSystem != null && !string.IsNullOrEmpty(this.liquidColorShaderPropertyName) && !string.IsNullOrEmpty(this.liquidPlaneNormalShaderPropertyName) && !string.IsNullOrEmpty(this.liquidPlanePositionShaderPropertyName);
		}

		// Token: 0x06004BBA RID: 19386 RVA: 0x001A2628 File Offset: 0x001A0828
		protected void InitializeLiquidSurface()
		{
			this.liquidColorShaderProp = Shader.PropertyToID(this.liquidColorShaderPropertyName);
			this.liquidPlaneNormalShaderProp = Shader.PropertyToID(this.liquidPlaneNormalShaderPropertyName);
			this.liquidPlanePositionShaderProp = Shader.PropertyToID(this.liquidPlanePositionShaderPropertyName);
			this.localMeshBounds = this.meshFilter.sharedMesh.bounds;
		}

		// Token: 0x06004BBB RID: 19387 RVA: 0x001A2680 File Offset: 0x001A0880
		protected void InitializeParticleSystem()
		{
			this.spillParticleSystem.main.startColor = this.liquidColor;
		}

		// Token: 0x06004BBC RID: 19388 RVA: 0x00061CBE File Offset: 0x0005FEBE
		protected void Awake()
		{
			this.matPropBlock = new MaterialPropertyBlock();
			this.topVerts = this.GetTopVerts();
		}

		// Token: 0x06004BBD RID: 19389 RVA: 0x001A26AC File Offset: 0x001A08AC
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

		// Token: 0x06004BBE RID: 19390 RVA: 0x001A2700 File Offset: 0x001A0900
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

		// Token: 0x06004BBF RID: 19391 RVA: 0x001A2C90 File Offset: 0x001A0E90
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

		// Token: 0x06004BC0 RID: 19392 RVA: 0x001A2CEC File Offset: 0x001A0EEC
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

		// Token: 0x04004CE0 RID: 19680
		[Tooltip("Used to determine the world space bounds of the container.")]
		public MeshRenderer meshRenderer;

		// Token: 0x04004CE1 RID: 19681
		[Tooltip("Used to determine the local space bounds of the container.")]
		public MeshFilter meshFilter;

		// Token: 0x04004CE2 RID: 19682
		[Tooltip("If you are only using the liquid mesh to calculate the volume of the container and do not need visuals then set this to true.")]
		public bool keepMeshHidden;

		// Token: 0x04004CE3 RID: 19683
		[Tooltip("The object that will float on top of the liquid.")]
		public Transform floater;

		// Token: 0x04004CE4 RID: 19684
		public bool useLiquidShader = true;

		// Token: 0x04004CE5 RID: 19685
		public bool useLiquidVolume;

		// Token: 0x04004CE6 RID: 19686
		public Vector2 liquidVolumeMinMax = Vector2.up;

		// Token: 0x04004CE7 RID: 19687
		public string liquidColorShaderPropertyName = "_BaseColor";

		// Token: 0x04004CE8 RID: 19688
		public string liquidPlaneNormalShaderPropertyName = "_LiquidPlaneNormal";

		// Token: 0x04004CE9 RID: 19689
		public string liquidPlanePositionShaderPropertyName = "_LiquidPlanePosition";

		// Token: 0x04004CEA RID: 19690
		[Tooltip("Emits drips when pouring.")]
		public ParticleSystem spillParticleSystem;

		// Token: 0x04004CEB RID: 19691
		[SoundBankInfo]
		public SoundBankPlayer emptySoundBankPlayer;

		// Token: 0x04004CEC RID: 19692
		[SoundBankInfo]
		public SoundBankPlayer refillSoundBankPlayer;

		// Token: 0x04004CED RID: 19693
		[SoundBankInfo]
		public SoundBankPlayer spillSoundBankPlayer;

		// Token: 0x04004CEE RID: 19694
		public Color liquidColor = new Color(0.33f, 0.25f, 0.21f, 1f);

		// Token: 0x04004CEF RID: 19695
		[Tooltip("The amount of liquid currently in the container. This value is passed to the shader.")]
		[Range(0f, 1f)]
		public float fillAmount = 0.85f;

		// Token: 0x04004CF0 RID: 19696
		[Tooltip("This is what fillAmount will be after automatic refilling.")]
		public float refillAmount = 0.85f;

		// Token: 0x04004CF1 RID: 19697
		[Tooltip("Set to a negative value to disable.")]
		public float refillDelay = 10f;

		// Token: 0x04004CF2 RID: 19698
		[Tooltip("The point that the liquid should be considered empty and should be auto refilled.")]
		public float refillThreshold = 0.1f;

		// Token: 0x04004CF3 RID: 19699
		public float wobbleMax = 0.2f;

		// Token: 0x04004CF4 RID: 19700
		public float wobbleFrequency = 1f;

		// Token: 0x04004CF5 RID: 19701
		public float recovery = 1f;

		// Token: 0x04004CF6 RID: 19702
		public float thickness = 1f;

		// Token: 0x04004CF7 RID: 19703
		public float maxSpillRate = 100f;

		// Token: 0x04004CFC RID: 19708
		[DebugReadout]
		private bool wasEmptyLastFrame;

		// Token: 0x04004CFD RID: 19709
		private int liquidColorShaderProp;

		// Token: 0x04004CFE RID: 19710
		private int liquidPlaneNormalShaderProp;

		// Token: 0x04004CFF RID: 19711
		private int liquidPlanePositionShaderProp;

		// Token: 0x04004D00 RID: 19712
		private float refillTimer;

		// Token: 0x04004D01 RID: 19713
		private float lastSineWave;

		// Token: 0x04004D02 RID: 19714
		private float lastWobble;

		// Token: 0x04004D03 RID: 19715
		private Vector2 temporalWobbleAmp;

		// Token: 0x04004D04 RID: 19716
		private Vector3 lastPos;

		// Token: 0x04004D05 RID: 19717
		private Vector3 lastVelocity;

		// Token: 0x04004D06 RID: 19718
		private Vector3 lastAngularVelocity;

		// Token: 0x04004D07 RID: 19719
		private Quaternion lastRot;

		// Token: 0x04004D08 RID: 19720
		private MaterialPropertyBlock matPropBlock;

		// Token: 0x04004D09 RID: 19721
		private Bounds localMeshBounds;

		// Token: 0x04004D0A RID: 19722
		private bool useFloater;

		// Token: 0x04004D0B RID: 19723
		private Vector3[] topVerts;
	}
}
