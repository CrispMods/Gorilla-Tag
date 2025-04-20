using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A8F RID: 2703
	public class RayToolView : MonoBehaviour, InteractableToolView
	{
		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x0600437C RID: 17276 RVA: 0x0005BFC5 File Offset: 0x0005A1C5
		// (set) Token: 0x0600437D RID: 17277 RVA: 0x0005BFD2 File Offset: 0x0005A1D2
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

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x0600437E RID: 17278 RVA: 0x0005BFF1 File Offset: 0x0005A1F1
		// (set) Token: 0x0600437F RID: 17279 RVA: 0x0005BFF9 File Offset: 0x0005A1F9
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

		// Token: 0x06004380 RID: 17280 RVA: 0x001783F0 File Offset: 0x001765F0
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

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06004381 RID: 17281 RVA: 0x0005C023 File Offset: 0x0005A223
		// (set) Token: 0x06004382 RID: 17282 RVA: 0x0005C02B File Offset: 0x0005A22B
		public InteractableTool InteractableTool { get; set; }

		// Token: 0x06004383 RID: 17283 RVA: 0x0005C034 File Offset: 0x0005A234
		public void SetFocusedInteractable(Interactable interactable)
		{
			if (interactable == null)
			{
				this._focusedTransform = null;
				return;
			}
			this._focusedTransform = interactable.transform;
		}

		// Token: 0x06004384 RID: 17284 RVA: 0x001784B4 File Offset: 0x001766B4
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

		// Token: 0x06004385 RID: 17285 RVA: 0x001785AC File Offset: 0x001767AC
		public static Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			t = Mathf.Clamp01(t);
			float num = 1f - t;
			float num2 = num * num;
			float num3 = t * t;
			return num * num2 * p0 + 3f * num2 * t * p1 + 3f * num * num3 * p2 + t * num3 * p3;
		}

		// Token: 0x04004432 RID: 17458
		private const int NUM_RAY_LINE_POSITIONS = 25;

		// Token: 0x04004433 RID: 17459
		private const float DEFAULT_RAY_CAST_DISTANCE = 3f;

		// Token: 0x04004434 RID: 17460
		[SerializeField]
		private Transform _targetTransform;

		// Token: 0x04004435 RID: 17461
		[SerializeField]
		private LineRenderer _lineRenderer;

		// Token: 0x04004436 RID: 17462
		private bool _toolActivateState;

		// Token: 0x04004437 RID: 17463
		private Transform _focusedTransform;

		// Token: 0x04004438 RID: 17464
		private Vector3[] linePositions = new Vector3[25];

		// Token: 0x04004439 RID: 17465
		private Gradient _oldColorGradient;

		// Token: 0x0400443A RID: 17466
		private Gradient _highLightColorGradient;
	}
}
