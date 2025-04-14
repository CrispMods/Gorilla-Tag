using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C33 RID: 3123
	public class DrillFX : MonoBehaviour
	{
		// Token: 0x06004DF9 RID: 19961 RVA: 0x0017E6F8 File Offset: 0x0017C8F8
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

		// Token: 0x06004DFA RID: 19962 RVA: 0x0017E7D4 File Offset: 0x0017C9D4
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

		// Token: 0x06004DFB RID: 19963 RVA: 0x0017E838 File Offset: 0x0017CA38
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

		// Token: 0x06004DFC RID: 19964 RVA: 0x0017E888 File Offset: 0x0017CA88
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

		// Token: 0x06004DFD RID: 19965 RVA: 0x0017E99E File Offset: 0x0017CB9E
		private static void HandleApplicationQuitting()
		{
			DrillFX.appIsQuitting = true;
		}

		// Token: 0x06004DFE RID: 19966 RVA: 0x0017E9A8 File Offset: 0x0017CBA8
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

		// Token: 0x040050FF RID: 20735
		[SerializeField]
		private ParticleSystem fx;

		// Token: 0x04005100 RID: 20736
		[SerializeField]
		private AnimationCurve fxEmissionCurve;

		// Token: 0x04005101 RID: 20737
		[SerializeField]
		private float fxMinRadiusScale = 0.01f;

		// Token: 0x04005102 RID: 20738
		[Tooltip("Right click menu has custom menu items. Anything starting with \"- \" is custom.")]
		[SerializeField]
		private AudioSource loopAudio;

		// Token: 0x04005103 RID: 20739
		[SerializeField]
		private AnimationCurve loopAudioVolumeCurve;

		// Token: 0x04005104 RID: 20740
		[Tooltip("Higher value makes it reach the target volume faster.")]
		[SerializeField]
		private float loopAudioVolumeTransitionSpeed = 3f;

		// Token: 0x04005105 RID: 20741
		[FormerlySerializedAs("layerMask")]
		[Tooltip("The collision layers the line cast should intersect with")]
		[SerializeField]
		private LayerMask lineCastLayerMask;

		// Token: 0x04005106 RID: 20742
		[Tooltip("The position in local space that the line cast starts.")]
		[SerializeField]
		private Vector3 lineCastStart = Vector3.zero;

		// Token: 0x04005107 RID: 20743
		[Tooltip("The position in local space that the line cast ends.")]
		[SerializeField]
		private Vector3 lineCastEnd = Vector3.forward;

		// Token: 0x04005108 RID: 20744
		private static bool appIsQuitting;

		// Token: 0x04005109 RID: 20745
		private static bool appIsQuittingHandlerIsSubscribed;

		// Token: 0x0400510A RID: 20746
		private float maxDepth;

		// Token: 0x0400510B RID: 20747
		private bool hasFX;

		// Token: 0x0400510C RID: 20748
		private ParticleSystem.EmissionModule fxEmissionModule;

		// Token: 0x0400510D RID: 20749
		private float fxEmissionMaxRate;

		// Token: 0x0400510E RID: 20750
		private ParticleSystem.ShapeModule fxShapeModule;

		// Token: 0x0400510F RID: 20751
		private float fxShapeMaxRadius;

		// Token: 0x04005110 RID: 20752
		private bool hasAudio;

		// Token: 0x04005111 RID: 20753
		private float audioMaxVolume;
	}
}
