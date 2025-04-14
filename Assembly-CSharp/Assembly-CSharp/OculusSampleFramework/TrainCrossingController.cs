using System;
using System.Collections;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A73 RID: 2675
	public class TrainCrossingController : MonoBehaviour
	{
		// Token: 0x060042A8 RID: 17064 RVA: 0x0013A816 File Offset: 0x00138A16
		private void Awake()
		{
			this._lightsSide1Mat = this._lightSide1Renderer.material;
			this._lightsSide2Mat = this._lightSide2Renderer.material;
		}

		// Token: 0x060042A9 RID: 17065 RVA: 0x0013A83A File Offset: 0x00138A3A
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

		// Token: 0x060042AA RID: 17066 RVA: 0x0013A86E File Offset: 0x00138A6E
		public void CrossingButtonStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this.ActivateTrainCrossing();
			}
			this._toolInteractingWithMe = ((obj.NewInteractableState > InteractableState.Default) ? obj.Tool : null);
		}

		// Token: 0x060042AB RID: 17067 RVA: 0x0013A89C File Offset: 0x00138A9C
		private void Update()
		{
			if (this._toolInteractingWithMe == null)
			{
				this._selectionCylinder.CurrSelectionState = SelectionCylinder.SelectionState.Off;
				return;
			}
			this._selectionCylinder.CurrSelectionState = ((this._toolInteractingWithMe.ToolInputState == ToolInputState.PrimaryInputDown || this._toolInteractingWithMe.ToolInputState == ToolInputState.PrimaryInputDownStay) ? SelectionCylinder.SelectionState.Highlighted : SelectionCylinder.SelectionState.Selected);
		}

		// Token: 0x060042AC RID: 17068 RVA: 0x0013A8F0 File Offset: 0x00138AF0
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

		// Token: 0x060042AD RID: 17069 RVA: 0x0013A96E File Offset: 0x00138B6E
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

		// Token: 0x060042AE RID: 17070 RVA: 0x0013A984 File Offset: 0x00138B84
		private void AffectMaterials(Material[] materials, Color newColor)
		{
			for (int i = 0; i < materials.Length; i++)
			{
				materials[i].SetColor(this._colorId, newColor);
			}
		}

		// Token: 0x060042AF RID: 17071 RVA: 0x0013A9B0 File Offset: 0x00138BB0
		private void ToggleLightObjects(bool enableState)
		{
			this._lightSide1Renderer.gameObject.SetActive(enableState);
			this._lightSide2Renderer.gameObject.SetActive(enableState);
		}

		// Token: 0x040043AA RID: 17322
		[SerializeField]
		private AudioSource _audioSource;

		// Token: 0x040043AB RID: 17323
		[SerializeField]
		private AudioClip[] _crossingSounds;

		// Token: 0x040043AC RID: 17324
		[SerializeField]
		private MeshRenderer _lightSide1Renderer;

		// Token: 0x040043AD RID: 17325
		[SerializeField]
		private MeshRenderer _lightSide2Renderer;

		// Token: 0x040043AE RID: 17326
		[SerializeField]
		private SelectionCylinder _selectionCylinder;

		// Token: 0x040043AF RID: 17327
		private Material _lightsSide1Mat;

		// Token: 0x040043B0 RID: 17328
		private Material _lightsSide2Mat;

		// Token: 0x040043B1 RID: 17329
		private int _colorId = Shader.PropertyToID("_Color");

		// Token: 0x040043B2 RID: 17330
		private Coroutine _xingAnimationCr;

		// Token: 0x040043B3 RID: 17331
		private InteractableTool _toolInteractingWithMe;
	}
}
