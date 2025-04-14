using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C30 RID: 3120
	public class DrillFX : MonoBehaviour
	{
		// Token: 0x06004DED RID: 19949 RVA: 0x0017E130 File Offset: 0x0017C330
		protected void Awake()
		{
			if (!DrillFX.appIsQuittingHandlerIsSubscribed)
			{
				DrillFX.appIsQuittingHandlerIsSubscribed = true;
				Application.quitting += DrillFX.HandleApplicationQuitting;
			}
			this.hasFX = (this.fx != null);
			if (this.hasFX)
			{
				this.fxEmissionModule = this.fx.emission;
				this.fxEmissionMaxRate = this.fxEmissionModule.rateOverTimeMultiplier;
				this.fxShapeModule = this.fx.shape;
				this.fxShapeMaxRadius = this.fxShapeModule.radius;
			}
			this.hasAudio = (this.loopAudio != null);
			if (this.hasAudio)
			{
				this.audioMaxVolume = this.loopAudio.volume;
				this.loopAudio.volume = 0f;
				this.loopAudio.loop = true;
				this.loopAudio.GTPlay();
			}
		}

		// Token: 0x06004DEE RID: 19950 RVA: 0x0017E20C File Offset: 0x0017C40C
		protected void OnEnable()
		{
			if (DrillFX.appIsQuitting)
			{
				return;
			}
			if (this.hasFX)
			{
				this.fxEmissionModule.rateOverTimeMultiplier = 0f;
			}
			if (this.hasAudio)
			{
				this.loopAudio.volume = 0f;
				this.loopAudio.loop = true;
				this.loopAudio.GTPlay();
			}
			this.ValidateLineCastPositions();
		}

		// Token: 0x06004DEF RID: 19951 RVA: 0x0017E270 File Offset: 0x0017C470
		protected void OnDisable()
		{
			if (DrillFX.appIsQuitting)
			{
				return;
			}
			if (this.hasFX)
			{
				this.fxEmissionModule.rateOverTimeMultiplier = 0f;
			}
			if (this.hasAudio)
			{
				this.loopAudio.volume = 0f;
				this.loopAudio.GTStop();
			}
		}

		// Token: 0x06004DF0 RID: 19952 RVA: 0x0017E2C0 File Offset: 0x0017C4C0
		protected void LateUpdate()
		{
			if (DrillFX.appIsQuitting)
			{
				return;
			}
			Transform transform = base.transform;
			RaycastHit raycastHit;
			Vector3 position = Physics.Linecast(transform.TransformPoint(this.lineCastStart), transform.TransformPoint(this.lineCastEnd), out raycastHit, this.lineCastLayerMask, QueryTriggerInteraction.Ignore) ? raycastHit.point : this.lineCastEnd;
			Vector3 vector = transform.InverseTransformPoint(position);
			float num = Mathf.Clamp01(Vector3.Distance(this.lineCastStart, vector) / this.maxDepth);
			if (this.hasFX)
			{
				this.fxEmissionModule.rateOverTimeMultiplier = this.fxEmissionMaxRate * this.fxEmissionCurve.Evaluate(num);
				this.fxShapeModule.position = vector;
				this.fxShapeModule.radius = Mathf.Lerp(this.fxShapeMaxRadius, this.fxMinRadiusScale * this.fxShapeMaxRadius, num);
			}
			if (this.hasAudio)
			{
				this.loopAudio.volume = Mathf.MoveTowards(this.loopAudio.volume, this.audioMaxVolume * this.loopAudioVolumeCurve.Evaluate(num), this.loopAudioVolumeTransitionSpeed * Time.deltaTime);
			}
		}

		// Token: 0x06004DF1 RID: 19953 RVA: 0x0017E3D6 File Offset: 0x0017C5D6
		private static void HandleApplicationQuitting()
		{
			DrillFX.appIsQuitting = true;
		}

		// Token: 0x06004DF2 RID: 19954 RVA: 0x0017E3E0 File Offset: 0x0017C5E0
		private bool ValidateLineCastPositions()
		{
			this.maxDepth = Vector3.Distance(this.lineCastStart, this.lineCastEnd);
			if (this.maxDepth > 1E-45f)
			{
				return true;
			}
			if (Application.isPlaying)
			{
				Debug.Log("DrillFX: lineCastStart and End are too close together. Disabling component.", this);
				base.enabled = false;
			}
			return false;
		}

		// Token: 0x040050ED RID: 20717
		[SerializeField]
		private ParticleSystem fx;

		// Token: 0x040050EE RID: 20718
		[SerializeField]
		private AnimationCurve fxEmissionCurve;

		// Token: 0x040050EF RID: 20719
		[SerializeField]
		private float fxMinRadiusScale = 0.01f;

		// Token: 0x040050F0 RID: 20720
		[Tooltip("Right click menu has custom menu items. Anything starting with \"- \" is custom.")]
		[SerializeField]
		private AudioSource loopAudio;

		// Token: 0x040050F1 RID: 20721
		[SerializeField]
		private AnimationCurve loopAudioVolumeCurve;

		// Token: 0x040050F2 RID: 20722
		[Tooltip("Higher value makes it reach the target volume faster.")]
		[SerializeField]
		private float loopAudioVolumeTransitionSpeed = 3f;

		// Token: 0x040050F3 RID: 20723
		[FormerlySerializedAs("layerMask")]
		[Tooltip("The collision layers the line cast should intersect with")]
		[SerializeField]
		private LayerMask lineCastLayerMask;

		// Token: 0x040050F4 RID: 20724
		[Tooltip("The position in local space that the line cast starts.")]
		[SerializeField]
		private Vector3 lineCastStart = Vector3.zero;

		// Token: 0x040050F5 RID: 20725
		[Tooltip("The position in local space that the line cast ends.")]
		[SerializeField]
		private Vector3 lineCastEnd = Vector3.forward;

		// Token: 0x040050F6 RID: 20726
		private static bool appIsQuitting;

		// Token: 0x040050F7 RID: 20727
		private static bool appIsQuittingHandlerIsSubscribed;

		// Token: 0x040050F8 RID: 20728
		private float maxDepth;

		// Token: 0x040050F9 RID: 20729
		private bool hasFX;

		// Token: 0x040050FA RID: 20730
		private ParticleSystem.EmissionModule fxEmissionModule;

		// Token: 0x040050FB RID: 20731
		private float fxEmissionMaxRate;

		// Token: 0x040050FC RID: 20732
		private ParticleSystem.ShapeModule fxShapeModule;

		// Token: 0x040050FD RID: 20733
		private float fxShapeMaxRadius;

		// Token: 0x040050FE RID: 20734
		private bool hasAudio;

		// Token: 0x040050FF RID: 20735
		private float audioMaxVolume;
	}
}
