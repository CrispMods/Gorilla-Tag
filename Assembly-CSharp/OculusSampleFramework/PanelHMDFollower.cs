using System;
using System.Collections;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A66 RID: 2662
	public class PanelHMDFollower : MonoBehaviour
	{
		// Token: 0x06004258 RID: 16984 RVA: 0x00139072 File Offset: 0x00137272
		private void Awake()
		{
			this._cameraRig = Object.FindObjectOfType<OVRCameraRig>();
			this._panelInitialPosition = base.transform.position;
		}

		// Token: 0x06004259 RID: 16985 RVA: 0x00139090 File Offset: 0x00137290
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

		// Token: 0x0600425A RID: 16986 RVA: 0x0013916A File Offset: 0x0013736A
		private Vector3 CalculateIdealAnchorPosition()
		{
			return this._cameraRig.centerEyeAnchor.position + this._panelInitialPosition;
		}

		// Token: 0x0600425B RID: 16987 RVA: 0x00139187 File Offset: 0x00137387
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

		// Token: 0x04004349 RID: 17225
		private const float TOTAL_DURATION = 3f;

		// Token: 0x0400434A RID: 17226
		private const float HMD_MOVEMENT_THRESHOLD = 0.3f;

		// Token: 0x0400434B RID: 17227
		[SerializeField]
		private float _maxDistance = 0.3f;

		// Token: 0x0400434C RID: 17228
		[SerializeField]
		private float _minDistance = 0.05f;

		// Token: 0x0400434D RID: 17229
		[SerializeField]
		private float _minZDistance = 0.05f;

		// Token: 0x0400434E RID: 17230
		private OVRCameraRig _cameraRig;

		// Token: 0x0400434F RID: 17231
		private Vector3 _panelInitialPosition = Vector3.zero;

		// Token: 0x04004350 RID: 17232
		private Coroutine _coroutine;

		// Token: 0x04004351 RID: 17233
		private Vector3 _prevPos = Vector3.zero;

		// Token: 0x04004352 RID: 17234
		private Vector3 _lastMovedToPos = Vector3.zero;
	}
}
