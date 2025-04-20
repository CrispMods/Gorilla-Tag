using System;
using System.Collections;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A93 RID: 2707
	public class PanelHMDFollower : MonoBehaviour
	{
		// Token: 0x0600439D RID: 17309 RVA: 0x0005C17B File Offset: 0x0005A37B
		private void Awake()
		{
			this._cameraRig = UnityEngine.Object.FindObjectOfType<OVRCameraRig>();
			this._panelInitialPosition = base.transform.position;
		}

		// Token: 0x0600439E RID: 17310 RVA: 0x00178740 File Offset: 0x00176940
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

		// Token: 0x0600439F RID: 17311 RVA: 0x0005C199 File Offset: 0x0005A399
		private Vector3 CalculateIdealAnchorPosition()
		{
			return this._cameraRig.centerEyeAnchor.position + this._panelInitialPosition;
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x0005C1B6 File Offset: 0x0005A3B6
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

		// Token: 0x04004443 RID: 17475
		private const float TOTAL_DURATION = 3f;

		// Token: 0x04004444 RID: 17476
		private const float HMD_MOVEMENT_THRESHOLD = 0.3f;

		// Token: 0x04004445 RID: 17477
		[SerializeField]
		private float _maxDistance = 0.3f;

		// Token: 0x04004446 RID: 17478
		[SerializeField]
		private float _minDistance = 0.05f;

		// Token: 0x04004447 RID: 17479
		[SerializeField]
		private float _minZDistance = 0.05f;

		// Token: 0x04004448 RID: 17480
		private OVRCameraRig _cameraRig;

		// Token: 0x04004449 RID: 17481
		private Vector3 _panelInitialPosition = Vector3.zero;

		// Token: 0x0400444A RID: 17482
		private Coroutine _coroutine;

		// Token: 0x0400444B RID: 17483
		private Vector3 _prevPos = Vector3.zero;

		// Token: 0x0400444C RID: 17484
		private Vector3 _lastMovedToPos = Vector3.zero;
	}
}
