using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace OculusSampleFramework
{
	// Token: 0x02000A6C RID: 2668
	public class TrainButtonVisualController : MonoBehaviour
	{
		// Token: 0x0600427C RID: 17020 RVA: 0x00139B44 File Offset: 0x00137D44
		private void Awake()
		{
			this._materialColorId = Shader.PropertyToID("_Color");
			this._buttonMaterial = this._meshRenderer.material;
			this._buttonDefaultColor = this._buttonMaterial.GetColor(this._materialColorId);
			this._oldPosition = base.transform.localPosition;
		}

		// Token: 0x0600427D RID: 17021 RVA: 0x00139B9A File Offset: 0x00137D9A
		private void OnDestroy()
		{
			if (this._buttonMaterial != null)
			{
				Object.Destroy(this._buttonMaterial);
			}
		}

		// Token: 0x0600427E RID: 17022 RVA: 0x00139BB8 File Offset: 0x00137DB8
		private void OnEnable()
		{
			this._buttonController.InteractableStateChanged.AddListener(new UnityAction<InteractableStateArgs>(this.InteractableStateChanged));
			this._buttonController.ContactZoneEvent += this.ActionOrInContactZoneStayEvent;
			this._buttonController.ActionZoneEvent += this.ActionOrInContactZoneStayEvent;
			this._buttonInContactOrActionStates = false;
		}

		// Token: 0x0600427F RID: 17023 RVA: 0x00139C18 File Offset: 0x00137E18
		private void OnDisable()
		{
			if (this._buttonController != null)
			{
				this._buttonController.InteractableStateChanged.RemoveListener(new UnityAction<InteractableStateArgs>(this.InteractableStateChanged));
				this._buttonController.ContactZoneEvent -= this.ActionOrInContactZoneStayEvent;
				this._buttonController.ActionZoneEvent -= this.ActionOrInContactZoneStayEvent;
			}
		}

		// Token: 0x06004280 RID: 17024 RVA: 0x00139C80 File Offset: 0x00137E80
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

		// Token: 0x06004281 RID: 17025 RVA: 0x00139D30 File Offset: 0x00137F30
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

		// Token: 0x06004282 RID: 17026 RVA: 0x00139E0B File Offset: 0x0013800B
		private void PlaySound(AudioClip clip)
		{
			this._audioSource.timeSamples = 0;
			this._audioSource.clip = clip;
			this._audioSource.Play();
		}

		// Token: 0x06004283 RID: 17027 RVA: 0x00139E30 File Offset: 0x00138030
		private void StopResetLerping()
		{
			if (this._lerpToOldPositionCr != null)
			{
				base.StopCoroutine(this._lerpToOldPositionCr);
			}
		}

		// Token: 0x06004284 RID: 17028 RVA: 0x00139E48 File Offset: 0x00138048
		private void LerpToOldPosition()
		{
			if ((base.transform.localPosition - this._oldPosition).sqrMagnitude < Mathf.Epsilon)
			{
				return;
			}
			this.StopResetLerping();
			this._lerpToOldPositionCr = base.StartCoroutine(this.ResetPosition());
		}

		// Token: 0x06004285 RID: 17029 RVA: 0x00139E93 File Offset: 0x00138093
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

		// Token: 0x04004375 RID: 17269
		private const float LERP_TO_OLD_POS_DURATION = 1f;

		// Token: 0x04004376 RID: 17270
		private const float LOCAL_SIZE_HALVED = 0.5f;

		// Token: 0x04004377 RID: 17271
		[SerializeField]
		private MeshRenderer _meshRenderer;

		// Token: 0x04004378 RID: 17272
		[SerializeField]
		private MeshRenderer _glowRenderer;

		// Token: 0x04004379 RID: 17273
		[SerializeField]
		private ButtonController _buttonController;

		// Token: 0x0400437A RID: 17274
		[SerializeField]
		private Color _buttonContactColor = new Color(0.51f, 0.78f, 0.92f, 1f);

		// Token: 0x0400437B RID: 17275
		[SerializeField]
		private Color _buttonActionColor = new Color(0.24f, 0.72f, 0.98f, 1f);

		// Token: 0x0400437C RID: 17276
		[SerializeField]
		private AudioSource _audioSource;

		// Token: 0x0400437D RID: 17277
		[SerializeField]
		private AudioClip _actionSoundEffect;

		// Token: 0x0400437E RID: 17278
		[SerializeField]
		private Transform _buttonContactTransform;

		// Token: 0x0400437F RID: 17279
		[SerializeField]
		private float _contactMaxDisplacementDistance = 0.0141f;

		// Token: 0x04004380 RID: 17280
		private Material _buttonMaterial;

		// Token: 0x04004381 RID: 17281
		private Color _buttonDefaultColor;

		// Token: 0x04004382 RID: 17282
		private int _materialColorId;

		// Token: 0x04004383 RID: 17283
		private bool _buttonInContactOrActionStates;

		// Token: 0x04004384 RID: 17284
		private Coroutine _lerpToOldPositionCr;

		// Token: 0x04004385 RID: 17285
		private Vector3 _oldPosition;
	}
}
