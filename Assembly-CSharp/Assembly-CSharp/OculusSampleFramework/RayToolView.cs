using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A65 RID: 2661
	public class RayToolView : MonoBehaviour, InteractableToolView
	{
		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06004243 RID: 16963 RVA: 0x00139133 File Offset: 0x00137333
		// (set) Token: 0x06004244 RID: 16964 RVA: 0x00139140 File Offset: 0x00137340
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

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06004245 RID: 16965 RVA: 0x0013915F File Offset: 0x0013735F
		// (set) Token: 0x06004246 RID: 16966 RVA: 0x00139167 File Offset: 0x00137367
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

		// Token: 0x06004247 RID: 16967 RVA: 0x00139194 File Offset: 0x00137394
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

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06004248 RID: 16968 RVA: 0x00139257 File Offset: 0x00137457
		// (set) Token: 0x06004249 RID: 16969 RVA: 0x0013925F File Offset: 0x0013745F
		public InteractableTool InteractableTool { get; set; }

		// Token: 0x0600424A RID: 16970 RVA: 0x00139268 File Offset: 0x00137468
		public void SetFocusedInteractable(Interactable interactable)
		{
			if (interactable == null)
			{
				this._focusedTransform = null;
				return;
			}
			this._focusedTransform = interactable.transform;
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x00139288 File Offset: 0x00137488
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

		// Token: 0x0600424C RID: 16972 RVA: 0x00139380 File Offset: 0x00137580
		public static Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			t = Mathf.Clamp01(t);
			float num = 1f - t;
			float num2 = num * num;
			float num3 = t * t;
			return num * num2 * p0 + 3f * num2 * t * p1 + 3f * num * num3 * p2 + t * num3 * p3;
		}

		// Token: 0x0400434A RID: 17226
		private const int NUM_RAY_LINE_POSITIONS = 25;

		// Token: 0x0400434B RID: 17227
		private const float DEFAULT_RAY_CAST_DISTANCE = 3f;

		// Token: 0x0400434C RID: 17228
		[SerializeField]
		private Transform _targetTransform;

		// Token: 0x0400434D RID: 17229
		[SerializeField]
		private LineRenderer _lineRenderer;

		// Token: 0x0400434E RID: 17230
		private bool _toolActivateState;

		// Token: 0x0400434F RID: 17231
		private Transform _focusedTransform;

		// Token: 0x04004350 RID: 17232
		private Vector3[] linePositions = new Vector3[25];

		// Token: 0x04004351 RID: 17233
		private Gradient _oldColorGradient;

		// Token: 0x04004352 RID: 17234
		private Gradient _highLightColorGradient;
	}
}
