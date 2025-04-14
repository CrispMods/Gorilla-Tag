using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A62 RID: 2658
	public class RayToolView : MonoBehaviour, InteractableToolView
	{
		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06004237 RID: 16951 RVA: 0x00138B6B File Offset: 0x00136D6B
		// (set) Token: 0x06004238 RID: 16952 RVA: 0x00138B78 File Offset: 0x00136D78
		public bool EnableState
		{
			get
			{
				return this._lineRenderer.enabled;
			}
			set
			{
				this._targetTransform.gameObject.SetActive(value);
				this._lineRenderer.enabled = value;
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06004239 RID: 16953 RVA: 0x00138B97 File Offset: 0x00136D97
		// (set) Token: 0x0600423A RID: 16954 RVA: 0x00138B9F File Offset: 0x00136D9F
		public bool ToolActivateState
		{
			get
			{
				return this._toolActivateState;
			}
			set
			{
				this._toolActivateState = value;
				this._lineRenderer.colorGradient = (this._toolActivateState ? this._highLightColorGradient : this._oldColorGradient);
			}
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x00138BCC File Offset: 0x00136DCC
		private void Awake()
		{
			this._lineRenderer.positionCount = 25;
			this._oldColorGradient = this._lineRenderer.colorGradient;
			this._highLightColorGradient = new Gradient();
			this._highLightColorGradient.SetKeys(new GradientColorKey[]
			{
				new GradientColorKey(new Color(0.9f, 0.9f, 0.9f), 0f),
				new GradientColorKey(new Color(0.9f, 0.9f, 0.9f), 1f)
			}, new GradientAlphaKey[]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 1f)
			});
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x0600423C RID: 16956 RVA: 0x00138C8F File Offset: 0x00136E8F
		// (set) Token: 0x0600423D RID: 16957 RVA: 0x00138C97 File Offset: 0x00136E97
		public InteractableTool InteractableTool { get; set; }

		// Token: 0x0600423E RID: 16958 RVA: 0x00138CA0 File Offset: 0x00136EA0
		public void SetFocusedInteractable(Interactable interactable)
		{
			if (interactable == null)
			{
				this._focusedTransform = null;
				return;
			}
			this._focusedTransform = interactable.transform;
		}

		// Token: 0x0600423F RID: 16959 RVA: 0x00138CC0 File Offset: 0x00136EC0
		private void Update()
		{
			Vector3 position = this.InteractableTool.ToolTransform.position;
			Vector3 forward = this.InteractableTool.ToolTransform.forward;
			Vector3 vector = (this._focusedTransform != null) ? this._focusedTransform.position : (position + forward * 3f);
			float magnitude = (vector - position).magnitude;
			Vector3 p = position;
			Vector3 p2 = position + forward * magnitude * 0.3333333f;
			Vector3 p3 = position + forward * magnitude * 0.6666667f;
			Vector3 p4 = vector;
			for (int i = 0; i < 25; i++)
			{
				this.linePositions[i] = RayToolView.GetPointOnBezierCurve(p, p2, p3, p4, (float)i / 25f);
			}
			this._lineRenderer.SetPositions(this.linePositions);
			this._targetTransform.position = vector;
		}

		// Token: 0x06004240 RID: 16960 RVA: 0x00138DB8 File Offset: 0x00136FB8
		public static Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			t = Mathf.Clamp01(t);
			float num = 1f - t;
			float num2 = num * num;
			float num3 = t * t;
			return num * num2 * p0 + 3f * num2 * t * p1 + 3f * num * num3 * p2 + t * num3 * p3;
		}

		// Token: 0x04004338 RID: 17208
		private const int NUM_RAY_LINE_POSITIONS = 25;

		// Token: 0x04004339 RID: 17209
		private const float DEFAULT_RAY_CAST_DISTANCE = 3f;

		// Token: 0x0400433A RID: 17210
		[SerializeField]
		private Transform _targetTransform;

		// Token: 0x0400433B RID: 17211
		[SerializeField]
		private LineRenderer _lineRenderer;

		// Token: 0x0400433C RID: 17212
		private bool _toolActivateState;

		// Token: 0x0400433D RID: 17213
		private Transform _focusedTransform;

		// Token: 0x0400433E RID: 17214
		private Vector3[] linePositions = new Vector3[25];

		// Token: 0x0400433F RID: 17215
		private Gradient _oldColorGradient;

		// Token: 0x04004340 RID: 17216
		private Gradient _highLightColorGradient;
	}
}
