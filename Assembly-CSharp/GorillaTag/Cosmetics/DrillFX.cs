using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C61 RID: 3169
	public class DrillFX : MonoBehaviour
	{
		// Token: 0x06004F4D RID: 20301 RVA: 0x001B6BE4 File Offset: 0x001B4DE4
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

		// Token: 0x06004F4E RID: 20302 RVA: 0x001B6CC0 File Offset: 0x001B4EC0
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

		// Token: 0x06004F4F RID: 20303 RVA: 0x001B6D24 File Offset: 0x001B4F24
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

		// Token: 0x06004F50 RID: 20304 RVA: 0x001B6D74 File Offset: 0x001B4F74
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

		// Token: 0x06004F51 RID: 20305 RVA: 0x00063C7D File Offset: 0x00061E7D
		private static void HandleApplicationQuitting()
		{
			DrillFX.appIsQuitting = true;
		}

		// Token: 0x06004F52 RID: 20306 RVA: 0x001B6E8C File Offset: 0x001B508C
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

		// Token: 0x040051F9 RID: 20985
		[SerializeField]
		private ParticleSystem fx;

		// Token: 0x040051FA RID: 20986
		[SerializeField]
		private AnimationCurve fxEmissionCurve;

		// Token: 0x040051FB RID: 20987
		[SerializeField]
		private float fxMinRadiusScale = 0.01f;

		// Token: 0x040051FC RID: 20988
		[Tooltip("Right click menu has custom menu items. Anything starting with \"- \" is custom.")]
		[SerializeField]
		private AudioSource loopAudio;

		// Token: 0x040051FD RID: 20989
		[SerializeField]
		private AnimationCurve loopAudioVolumeCurve;

		// Token: 0x040051FE RID: 20990
		[Tooltip("Higher value makes it reach the target volume faster.")]
		[SerializeField]
		private float loopAudioVolumeTransitionSpeed = 3f;

		// Token: 0x040051FF RID: 20991
		[FormerlySerializedAs("layerMask")]
		[Tooltip("The collision layers the line cast should intersect with")]
		[SerializeField]
		private LayerMask lineCastLayerMask;

		// Token: 0x04005200 RID: 20992
		[Tooltip("The position in local space that the line cast starts.")]
		[SerializeField]
		private Vector3 lineCastStart = Vector3.zero;

		// Token: 0x04005201 RID: 20993
		[Tooltip("The position in local space that the line cast ends.")]
		[SerializeField]
		private Vector3 lineCastEnd = Vector3.forward;

		// Token: 0x04005202 RID: 20994
		private static bool appIsQuitting;

		// Token: 0x04005203 RID: 20995
		private static bool appIsQuittingHandlerIsSubscribed;

		// Token: 0x04005204 RID: 20996
		private float maxDepth;

		// Token: 0x04005205 RID: 20997
		private bool hasFX;

		// Token: 0x04005206 RID: 20998
		private ParticleSystem.EmissionModule fxEmissionModule;

		// Token: 0x04005207 RID: 20999
		private float fxEmissionMaxRate;

		// Token: 0x04005208 RID: 21000
		private ParticleSystem.ShapeModule fxShapeModule;

		// Token: 0x04005209 RID: 21001
		private float fxShapeMaxRadius;

		// Token: 0x0400520A RID: 21002
		private bool hasAudio;

		// Token: 0x0400520B RID: 21003
		private float audioMaxVolume;
	}
}
