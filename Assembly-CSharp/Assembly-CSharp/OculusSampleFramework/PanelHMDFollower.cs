using System;
using System.Collections;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A69 RID: 2665
	public class PanelHMDFollower : MonoBehaviour
	{
		// Token: 0x06004264 RID: 16996 RVA: 0x0013963A File Offset: 0x0013783A
		private void Awake()
		{
			this._cameraRig = Object.FindObjectOfType<OVRCameraRig>();
			this._panelInitialPosition = base.transform.position;
		}

		// Token: 0x06004265 RID: 16997 RVA: 0x00139658 File Offset: 0x00137858
		private void Update()
		{
			Vector3 position = this._cameraRig.centerEyeAnchor.position;
			Vector3 position2 = base.transform.position;
			float num = Vector3.Distance(position, this._lastMovedToPos);
			float num2 = (this._cameraRig.centerEyeAnchor.position - this._prevPos).magnitude / Time.deltaTime;
			Vector3 vector = base.transform.position - position;
			float magnitude = vector.magnitude;
			if ((num > this._maxDistance || this._minZDistance > vector.z || this._minDistance > magnitude) && num2 < 0.3f && this._coroutine == null && this._coroutine == null)
			{
				this._coroutine = base.StartCoroutine(this.LerpToHMD());
			}
			this._prevPos = this._cameraRig.centerEyeAnchor.position;
		}

		// Token: 0x06004266 RID: 16998 RVA: 0x00139732 File Offset: 0x00137932
		private Vector3 CalculateIdealAnchorPosition()
		{
			return this._cameraRig.centerEyeAnchor.position + this._panelInitialPosition;
		}

		// Token: 0x06004267 RID: 16999 RVA: 0x0013974F File Offset: 0x0013794F
		private IEnumerator LerpToHMD()
		{
			Vector3 newPanelPosition = this.CalculateIdealAnchorPosition();
			this._lastMovedToPos = this._cameraRig.centerEyeAnchor.position;
			float startTime = Time.time;
			float endTime = Time.time + 3f;
			while (Time.time < endTime)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, newPanelPosition, (Time.time - startTime) / 3f);
				yield return null;
			}
			base.transform.position = newPanelPosition;
			this._coroutine = null;
			yield break;
		}

		// Token: 0x0400435B RID: 17243
		private const float TOTAL_DURATION = 3f;

		// Token: 0x0400435C RID: 17244
		private const float HMD_MOVEMENT_THRESHOLD = 0.3f;

		// Token: 0x0400435D RID: 17245
		[SerializeField]
		private float _maxDistance = 0.3f;

		// Token: 0x0400435E RID: 17246
		[SerializeField]
		private float _minDistance = 0.05f;

		// Token: 0x0400435F RID: 17247
		[SerializeField]
		private float _minZDistance = 0.05f;

		// Token: 0x04004360 RID: 17248
		private OVRCameraRig _cameraRig;

		// Token: 0x04004361 RID: 17249
		private Vector3 _panelInitialPosition = Vector3.zero;

		// Token: 0x04004362 RID: 17250
		private Coroutine _coroutine;

		// Token: 0x04004363 RID: 17251
		private Vector3 _prevPos = Vector3.zero;

		// Token: 0x04004364 RID: 17252
		private Vector3 _lastMovedToPos = Vector3.zero;
	}
}
