using System;
using System.Collections;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A9D RID: 2717
	public class TrainCrossingController : MonoBehaviour
	{
		// Token: 0x060043E1 RID: 17377 RVA: 0x0005C3E8 File Offset: 0x0005A5E8
		private void Awake()
		{
			this._lightsSide1Mat = this._lightSide1Renderer.material;
			this._lightsSide2Mat = this._lightSide2Renderer.material;
		}

		// Token: 0x060043E2 RID: 17378 RVA: 0x0005C40C File Offset: 0x0005A60C
		private void OnDestroy()
		{
			if (this._lightsSide1Mat != null)
			{
				UnityEngine.Object.Destroy(this._lightsSide1Mat);
			}
			if (this._lightsSide2Mat != null)
			{
				UnityEngine.Object.Destroy(this._lightsSide2Mat);
			}
		}

		// Token: 0x060043E3 RID: 17379 RVA: 0x0005C440 File Offset: 0x0005A640
		public void CrossingButtonStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this.ActivateTrainCrossing();
			}
			this._toolInteractingWithMe = ((obj.NewInteractableState > InteractableState.Default) ? obj.Tool : null);
		}

		// Token: 0x060043E4 RID: 17380 RVA: 0x001796B0 File Offset: 0x001778B0
		private void Update()
		{
			if (this._toolInteractingWithMe == null)
			{
				this._selectionCylinder.CurrSelectionState = SelectionCylinder.SelectionState.Off;
				return;
			}
			this._selectionCylinder.CurrSelectionState = ((this._toolInteractingWithMe.ToolInputState == ToolInputState.PrimaryInputDown || this._toolInteractingWithMe.ToolInputState == ToolInputState.PrimaryInputDownStay) ? SelectionCylinder.SelectionState.Highlighted : SelectionCylinder.SelectionState.Selected);
		}

		// Token: 0x060043E5 RID: 17381 RVA: 0x00179704 File Offset: 0x00177904
		private void ActivateTrainCrossing()
		{
			int num = this._crossingSounds.Length - 1;
			AudioClip audioClip = this._crossingSounds[(int)(UnityEngine.Random.value * (float)num)];
			this._audioSource.clip = audioClip;
			this._audioSource.timeSamples = 0;
			this._audioSource.Play();
			if (this._xingAnimationCr != null)
			{
				base.StopCoroutine(this._xingAnimationCr);
			}
			this._xingAnimationCr = base.StartCoroutine(this.AnimateCrossing(audioClip.length * 0.75f));
		}

		// Token: 0x060043E6 RID: 17382 RVA: 0x0005C46B File Offset: 0x0005A66B
		private IEnumerator AnimateCrossing(float animationLength)
		{
			this.ToggleLightObjects(true);
			float animationEndTime = Time.time + animationLength;
			float lightBlinkDuration = animationLength * 0.1f;
			float lightBlinkStartTime = Time.time;
			float lightBlinkEndTime = Time.time + lightBlinkDuration;
			Material lightToBlinkOn = this._lightsSide1Mat;
			Material lightToBlinkOff = this._lightsSide2Mat;
			Color onColor = new Color(1f, 1f, 1f, 1f);
			Color offColor = new Color(1f, 1f, 1f, 0f);
			while (Time.time < animationEndTime)
			{
				float t = (Time.time - lightBlinkStartTime) / lightBlinkDuration;
				lightToBlinkOn.SetColor(this._colorId, Color.Lerp(offColor, onColor, t));
				lightToBlinkOff.SetColor(this._colorId, Color.Lerp(onColor, offColor, t));
				if (Time.time > lightBlinkEndTime)
				{
					Material material = lightToBlinkOn;
					lightToBlinkOn = lightToBlinkOff;
					lightToBlinkOff = material;
					lightBlinkStartTime = Time.time;
					lightBlinkEndTime = Time.time + lightBlinkDuration;
				}
				yield return null;
			}
			this.ToggleLightObjects(false);
			yield break;
		}

		// Token: 0x060043E7 RID: 17383 RVA: 0x00179784 File Offset: 0x00177984
		private void AffectMaterials(Material[] materials, Color newColor)
		{
			for (int i = 0; i < materials.Length; i++)
			{
				materials[i].SetColor(this._colorId, newColor);
			}
		}

		// Token: 0x060043E8 RID: 17384 RVA: 0x0005C481 File Offset: 0x0005A681
		private void ToggleLightObjects(bool enableState)
		{
			this._lightSide1Renderer.gameObject.SetActive(enableState);
			this._lightSide2Renderer.gameObject.SetActive(enableState);
		}

		// Token: 0x04004492 RID: 17554
		[SerializeField]
		private AudioSource _audioSource;

		// Token: 0x04004493 RID: 17555
		[SerializeField]
		private AudioClip[] _crossingSounds;

		// Token: 0x04004494 RID: 17556
		[SerializeField]
		private MeshRenderer _lightSide1Renderer;

		// Token: 0x04004495 RID: 17557
		[SerializeField]
		private MeshRenderer _lightSide2Renderer;

		// Token: 0x04004496 RID: 17558
		[SerializeField]
		private SelectionCylinder _selectionCylinder;

		// Token: 0x04004497 RID: 17559
		private Material _lightsSide1Mat;

		// Token: 0x04004498 RID: 17560
		private Material _lightsSide2Mat;

		// Token: 0x04004499 RID: 17561
		private int _colorId = Shader.PropertyToID("_Color");

		// Token: 0x0400449A RID: 17562
		private Coroutine _xingAnimationCr;

		// Token: 0x0400449B RID: 17563
		private InteractableTool _toolInteractingWithMe;
	}
}
