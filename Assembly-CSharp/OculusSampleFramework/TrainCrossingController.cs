using System;
using System.Collections;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A70 RID: 2672
	public class TrainCrossingController : MonoBehaviour
	{
		// Token: 0x0600429C RID: 17052 RVA: 0x0013A24E File Offset: 0x0013844E
		private void Awake()
		{
			this._lightsSide1Mat = this._lightSide1Renderer.material;
			this._lightsSide2Mat = this._lightSide2Renderer.material;
		}

		// Token: 0x0600429D RID: 17053 RVA: 0x0013A272 File Offset: 0x00138472
		private void OnDestroy()
		{
			if (this._lightsSide1Mat != null)
			{
				Object.Destroy(this._lightsSide1Mat);
			}
			if (this._lightsSide2Mat != null)
			{
				Object.Destroy(this._lightsSide2Mat);
			}
		}

		// Token: 0x0600429E RID: 17054 RVA: 0x0013A2A6 File Offset: 0x001384A6
		public void CrossingButtonStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this.ActivateTrainCrossing();
			}
			this._toolInteractingWithMe = ((obj.NewInteractableState > InteractableState.Default) ? obj.Tool : null);
		}

		// Token: 0x0600429F RID: 17055 RVA: 0x0013A2D4 File Offset: 0x001384D4
		private void Update()
		{
			if (this._toolInteractingWithMe == null)
			{
				this._selectionCylinder.CurrSelectionState = SelectionCylinder.SelectionState.Off;
				return;
			}
			this._selectionCylinder.CurrSelectionState = ((this._toolInteractingWithMe.ToolInputState == ToolInputState.PrimaryInputDown || this._toolInteractingWithMe.ToolInputState == ToolInputState.PrimaryInputDownStay) ? SelectionCylinder.SelectionState.Highlighted : SelectionCylinder.SelectionState.Selected);
		}

		// Token: 0x060042A0 RID: 17056 RVA: 0x0013A328 File Offset: 0x00138528
		private void ActivateTrainCrossing()
		{
			int num = this._crossingSounds.Length - 1;
			AudioClip audioClip = this._crossingSounds[(int)(Random.value * (float)num)];
			this._audioSource.clip = audioClip;
			this._audioSource.timeSamples = 0;
			this._audioSource.Play();
			if (this._xingAnimationCr != null)
			{
				base.StopCoroutine(this._xingAnimationCr);
			}
			this._xingAnimationCr = base.StartCoroutine(this.AnimateCrossing(audioClip.length * 0.75f));
		}

		// Token: 0x060042A1 RID: 17057 RVA: 0x0013A3A6 File Offset: 0x001385A6
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

		// Token: 0x060042A2 RID: 17058 RVA: 0x0013A3BC File Offset: 0x001385BC
		private void AffectMaterials(Material[] materials, Color newColor)
		{
			for (int i = 0; i < materials.Length; i++)
			{
				materials[i].SetColor(this._colorId, newColor);
			}
		}

		// Token: 0x060042A3 RID: 17059 RVA: 0x0013A3E8 File Offset: 0x001385E8
		private void ToggleLightObjects(bool enableState)
		{
			this._lightSide1Renderer.gameObject.SetActive(enableState);
			this._lightSide2Renderer.gameObject.SetActive(enableState);
		}

		// Token: 0x04004398 RID: 17304
		[SerializeField]
		private AudioSource _audioSource;

		// Token: 0x04004399 RID: 17305
		[SerializeField]
		private AudioClip[] _crossingSounds;

		// Token: 0x0400439A RID: 17306
		[SerializeField]
		private MeshRenderer _lightSide1Renderer;

		// Token: 0x0400439B RID: 17307
		[SerializeField]
		private MeshRenderer _lightSide2Renderer;

		// Token: 0x0400439C RID: 17308
		[SerializeField]
		private SelectionCylinder _selectionCylinder;

		// Token: 0x0400439D RID: 17309
		private Material _lightsSide1Mat;

		// Token: 0x0400439E RID: 17310
		private Material _lightsSide2Mat;

		// Token: 0x0400439F RID: 17311
		private int _colorId = Shader.PropertyToID("_Color");

		// Token: 0x040043A0 RID: 17312
		private Coroutine _xingAnimationCr;

		// Token: 0x040043A1 RID: 17313
		private InteractableTool _toolInteractingWithMe;
	}
}
