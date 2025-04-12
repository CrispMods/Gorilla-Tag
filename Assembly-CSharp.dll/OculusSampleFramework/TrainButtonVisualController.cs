using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace OculusSampleFramework
{
	// Token: 0x02000A6F RID: 2671
	public class TrainButtonVisualController : MonoBehaviour
	{
		// Token: 0x06004288 RID: 17032 RVA: 0x00172254 File Offset: 0x00170454
		private void Awake()
		{
			this._materialColorId = Shader.PropertyToID("_Color");
			this._buttonMaterial = this._meshRenderer.material;
			this._buttonDefaultColor = this._buttonMaterial.GetColor(this._materialColorId);
			this._oldPosition = base.transform.localPosition;
		}

		// Token: 0x06004289 RID: 17033 RVA: 0x0005A8B4 File Offset: 0x00058AB4
		private void OnDestroy()
		{
			if (this._buttonMaterial != null)
			{
				UnityEngine.Object.Destroy(this._buttonMaterial);
			}
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x001722AC File Offset: 0x001704AC
		private void OnEnable()
		{
			this._buttonController.InteractableStateChanged.AddListener(new UnityAction<InteractableStateArgs>(this.InteractableStateChanged));
			this._buttonController.ContactZoneEvent += this.ActionOrInContactZoneStayEvent;
			this._buttonController.ActionZoneEvent += this.ActionOrInContactZoneStayEvent;
			this._buttonInContactOrActionStates = false;
		}

		// Token: 0x0600428B RID: 17035 RVA: 0x0017230C File Offset: 0x0017050C
		private void OnDisable()
		{
			if (this._buttonController != null)
			{
				this._buttonController.InteractableStateChanged.RemoveListener(new UnityAction<InteractableStateArgs>(this.InteractableStateChanged));
				this._buttonController.ContactZoneEvent -= this.ActionOrInContactZoneStayEvent;
				this._buttonController.ActionZoneEvent -= this.ActionOrInContactZoneStayEvent;
			}
		}

		// Token: 0x0600428C RID: 17036 RVA: 0x00172374 File Offset: 0x00170574
		private void ActionOrInContactZoneStayEvent(ColliderZoneArgs collisionArgs)
		{
			if (!this._buttonInContactOrActionStates || collisionArgs.CollidingTool.IsFarFieldTool)
			{
				return;
			}
			Vector3 localScale = this._buttonContactTransform.localScale;
			Vector3 interactionPosition = collisionArgs.CollidingTool.InteractionPosition;
			float num = (this._buttonContactTransform.InverseTransformPoint(interactionPosition) - 0.5f * Vector3.one).y * localScale.y;
			if (num > -this._contactMaxDisplacementDistance && num <= 0f)
			{
				base.transform.localPosition = new Vector3(this._oldPosition.x, this._oldPosition.y + num, this._oldPosition.z);
			}
		}

		// Token: 0x0600428D RID: 17037 RVA: 0x00172424 File Offset: 0x00170624
		private void InteractableStateChanged(InteractableStateArgs obj)
		{
			this._buttonInContactOrActionStates = false;
			this._glowRenderer.gameObject.SetActive(obj.NewInteractableState > InteractableState.Default);
			switch (obj.NewInteractableState)
			{
			case InteractableState.ProximityState:
				this._buttonMaterial.SetColor(this._materialColorId, this._buttonDefaultColor);
				this.LerpToOldPosition();
				return;
			case InteractableState.ContactState:
				this.StopResetLerping();
				this._buttonMaterial.SetColor(this._materialColorId, this._buttonContactColor);
				this._buttonInContactOrActionStates = true;
				return;
			case InteractableState.ActionState:
				this.StopResetLerping();
				this._buttonMaterial.SetColor(this._materialColorId, this._buttonActionColor);
				this.PlaySound(this._actionSoundEffect);
				this._buttonInContactOrActionStates = true;
				return;
			default:
				this._buttonMaterial.SetColor(this._materialColorId, this._buttonDefaultColor);
				this.LerpToOldPosition();
				return;
			}
		}

		// Token: 0x0600428E RID: 17038 RVA: 0x0005A8CF File Offset: 0x00058ACF
		private void PlaySound(AudioClip clip)
		{
			this._audioSource.timeSamples = 0;
			this._audioSource.clip = clip;
			this._audioSource.Play();
		}

		// Token: 0x0600428F RID: 17039 RVA: 0x0005A8F4 File Offset: 0x00058AF4
		private void StopResetLerping()
		{
			if (this._lerpToOldPositionCr != null)
			{
				base.StopCoroutine(this._lerpToOldPositionCr);
			}
		}

		// Token: 0x06004290 RID: 17040 RVA: 0x00172500 File Offset: 0x00170700
		private void LerpToOldPosition()
		{
			if ((base.transform.localPosition - this._oldPosition).sqrMagnitude < Mathf.Epsilon)
			{
				return;
			}
			this.StopResetLerping();
			this._lerpToOldPositionCr = base.StartCoroutine(this.ResetPosition());
		}

		// Token: 0x06004291 RID: 17041 RVA: 0x0005A90A File Offset: 0x00058B0A
		private IEnumerator ResetPosition()
		{
			float startTime = Time.time;
			float endTime = Time.time + 1f;
			while (Time.time < endTime)
			{
				base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, this._oldPosition, (Time.time - startTime) / 1f);
				yield return null;
			}
			base.transform.localPosition = this._oldPosition;
			this._lerpToOldPositionCr = null;
			yield break;
		}

		// Token: 0x04004387 RID: 17287
		private const float LERP_TO_OLD_POS_DURATION = 1f;

		// Token: 0x04004388 RID: 17288
		private const float LOCAL_SIZE_HALVED = 0.5f;

		// Token: 0x04004389 RID: 17289
		[SerializeField]
		private MeshRenderer _meshRenderer;

		// Token: 0x0400438A RID: 17290
		[SerializeField]
		private MeshRenderer _glowRenderer;

		// Token: 0x0400438B RID: 17291
		[SerializeField]
		private ButtonController _buttonController;

		// Token: 0x0400438C RID: 17292
		[SerializeField]
		private Color _buttonContactColor = new Color(0.51f, 0.78f, 0.92f, 1f);

		// Token: 0x0400438D RID: 17293
		[SerializeField]
		private Color _buttonActionColor = new Color(0.24f, 0.72f, 0.98f, 1f);

		// Token: 0x0400438E RID: 17294
		[SerializeField]
		private AudioSource _audioSource;

		// Token: 0x0400438F RID: 17295
		[SerializeField]
		private AudioClip _actionSoundEffect;

		// Token: 0x04004390 RID: 17296
		[SerializeField]
		private Transform _buttonContactTransform;

		// Token: 0x04004391 RID: 17297
		[SerializeField]
		private float _contactMaxDisplacementDistance = 0.0141f;

		// Token: 0x04004392 RID: 17298
		private Material _buttonMaterial;

		// Token: 0x04004393 RID: 17299
		private Color _buttonDefaultColor;

		// Token: 0x04004394 RID: 17300
		private int _materialColorId;

		// Token: 0x04004395 RID: 17301
		private bool _buttonInContactOrActionStates;

		// Token: 0x04004396 RID: 17302
		private Coroutine _lerpToOldPositionCr;

		// Token: 0x04004397 RID: 17303
		private Vector3 _oldPosition;
	}
}
