using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace OculusSampleFramework
{
	// Token: 0x02000A99 RID: 2713
	public class TrainButtonVisualController : MonoBehaviour
	{
		// Token: 0x060043C1 RID: 17345 RVA: 0x001790D8 File Offset: 0x001772D8
		private void Awake()
		{
			this._materialColorId = Shader.PropertyToID("_Color");
			this._buttonMaterial = this._meshRenderer.material;
			this._buttonDefaultColor = this._buttonMaterial.GetColor(this._materialColorId);
			this._oldPosition = base.transform.localPosition;
		}

		// Token: 0x060043C2 RID: 17346 RVA: 0x0005C2B6 File Offset: 0x0005A4B6
		private void OnDestroy()
		{
			if (this._buttonMaterial != null)
			{
				UnityEngine.Object.Destroy(this._buttonMaterial);
			}
		}

		// Token: 0x060043C3 RID: 17347 RVA: 0x00179130 File Offset: 0x00177330
		private void OnEnable()
		{
			this._buttonController.InteractableStateChanged.AddListener(new UnityAction<InteractableStateArgs>(this.InteractableStateChanged));
			this._buttonController.ContactZoneEvent += this.ActionOrInContactZoneStayEvent;
			this._buttonController.ActionZoneEvent += this.ActionOrInContactZoneStayEvent;
			this._buttonInContactOrActionStates = false;
		}

		// Token: 0x060043C4 RID: 17348 RVA: 0x00179190 File Offset: 0x00177390
		private void OnDisable()
		{
			if (this._buttonController != null)
			{
				this._buttonController.InteractableStateChanged.RemoveListener(new UnityAction<InteractableStateArgs>(this.InteractableStateChanged));
				this._buttonController.ContactZoneEvent -= this.ActionOrInContactZoneStayEvent;
				this._buttonController.ActionZoneEvent -= this.ActionOrInContactZoneStayEvent;
			}
		}

		// Token: 0x060043C5 RID: 17349 RVA: 0x001791F8 File Offset: 0x001773F8
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

		// Token: 0x060043C6 RID: 17350 RVA: 0x001792A8 File Offset: 0x001774A8
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

		// Token: 0x060043C7 RID: 17351 RVA: 0x0005C2D1 File Offset: 0x0005A4D1
		private void PlaySound(AudioClip clip)
		{
			this._audioSource.timeSamples = 0;
			this._audioSource.clip = clip;
			this._audioSource.Play();
		}

		// Token: 0x060043C8 RID: 17352 RVA: 0x0005C2F6 File Offset: 0x0005A4F6
		private void StopResetLerping()
		{
			if (this._lerpToOldPositionCr != null)
			{
				base.StopCoroutine(this._lerpToOldPositionCr);
			}
		}

		// Token: 0x060043C9 RID: 17353 RVA: 0x00179384 File Offset: 0x00177584
		private void LerpToOldPosition()
		{
			if ((base.transform.localPosition - this._oldPosition).sqrMagnitude < Mathf.Epsilon)
			{
				return;
			}
			this.StopResetLerping();
			this._lerpToOldPositionCr = base.StartCoroutine(this.ResetPosition());
		}

		// Token: 0x060043CA RID: 17354 RVA: 0x0005C30C File Offset: 0x0005A50C
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

		// Token: 0x0400446F RID: 17519
		private const float LERP_TO_OLD_POS_DURATION = 1f;

		// Token: 0x04004470 RID: 17520
		private const float LOCAL_SIZE_HALVED = 0.5f;

		// Token: 0x04004471 RID: 17521
		[SerializeField]
		private MeshRenderer _meshRenderer;

		// Token: 0x04004472 RID: 17522
		[SerializeField]
		private MeshRenderer _glowRenderer;

		// Token: 0x04004473 RID: 17523
		[SerializeField]
		private ButtonController _buttonController;

		// Token: 0x04004474 RID: 17524
		[SerializeField]
		private Color _buttonContactColor = new Color(0.51f, 0.78f, 0.92f, 1f);

		// Token: 0x04004475 RID: 17525
		[SerializeField]
		private Color _buttonActionColor = new Color(0.24f, 0.72f, 0.98f, 1f);

		// Token: 0x04004476 RID: 17526
		[SerializeField]
		private AudioSource _audioSource;

		// Token: 0x04004477 RID: 17527
		[SerializeField]
		private AudioClip _actionSoundEffect;

		// Token: 0x04004478 RID: 17528
		[SerializeField]
		private Transform _buttonContactTransform;

		// Token: 0x04004479 RID: 17529
		[SerializeField]
		private float _contactMaxDisplacementDistance = 0.0141f;

		// Token: 0x0400447A RID: 17530
		private Material _buttonMaterial;

		// Token: 0x0400447B RID: 17531
		private Color _buttonDefaultColor;

		// Token: 0x0400447C RID: 17532
		private int _materialColorId;

		// Token: 0x0400447D RID: 17533
		private bool _buttonInContactOrActionStates;

		// Token: 0x0400447E RID: 17534
		private Coroutine _lerpToOldPositionCr;

		// Token: 0x0400447F RID: 17535
		private Vector3 _oldPosition;
	}
}
