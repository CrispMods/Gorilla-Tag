using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000622 RID: 1570
[DisallowMultipleComponent]
public class TappableGuardianIdol : Tappable
{
	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x060026F6 RID: 9974 RVA: 0x0004A975 File Offset: 0x00048B75
	// (set) Token: 0x060026F7 RID: 9975 RVA: 0x0004A97D File Offset: 0x00048B7D
	public bool isChangingPositions { get; private set; }

	// Token: 0x060026F8 RID: 9976 RVA: 0x0004A986 File Offset: 0x00048B86
	protected override void OnEnable()
	{
		base.OnEnable();
		this._colliderBaseRadius = this.tapCollision.radius;
	}

	// Token: 0x060026F9 RID: 9977 RVA: 0x0004A99F File Offset: 0x00048B9F
	protected override void OnDisable()
	{
		base.OnDisable();
		this.isChangingPositions = false;
		this._activationState = -1;
		this.isActivationReady = true;
		this.tapCollision.radius = this._colliderBaseRadius;
	}

	// Token: 0x060026FA RID: 9978 RVA: 0x0004A9CD File Offset: 0x00048BCD
	public void OnZoneActiveStateChanged(bool zoneActive)
	{
		GTDev.Log<string>(string.Format("OnZoneActiveStateChanged({0}->{1})", this._zoneIsActive, zoneActive), this, null);
		this._zoneIsActive = zoneActive;
		this.idolVisualRoot.SetActive(this._zoneIsActive);
	}

	// Token: 0x060026FB RID: 9979 RVA: 0x0010A700 File Offset: 0x00108900
	public override void OnTapLocal(float tapStrength, float tapTime, PhotonMessageInfoWrapped info)
	{
		if (info.Sender.IsLocal)
		{
			this.zoneManager.SetScaleCenterPoint(base.transform);
		}
		if (!this.isChangingPositions)
		{
			if (!this.zoneManager.IsZoneValid())
			{
				return;
			}
			RigContainer rigContainer;
			if (PhotonNetwork.LocalPlayer.IsMasterClient && VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer))
			{
				if (Vector3.Magnitude(rigContainer.Rig.transform.position - base.transform.position) > this.requiredTapDistance + Mathf.Epsilon)
				{
					return;
				}
				this.zoneManager.IdolWasTapped(info.Sender);
			}
			if (!this.zoneManager.IsPlayerGuardian(info.Sender))
			{
				this.tapFX.Play();
			}
		}
	}

	// Token: 0x060026FC RID: 9980 RVA: 0x0010A7CC File Offset: 0x001089CC
	public void SetPosition(Vector3 position)
	{
		base.transform.position = position + new Vector3(0f, this.activeHeight, 0f);
		this.UpdateStageActivatedObjects();
		this._audio.GTPlayOneShot(this._activateSound, this._audio.volume);
		base.StartCoroutine(this.<SetPosition>g__Unshrink|49_0());
	}

	// Token: 0x060026FD RID: 9981 RVA: 0x0004AA09 File Offset: 0x00048C09
	public void MovePositions(Vector3 finalPosition)
	{
		if (this.isChangingPositions)
		{
			return;
		}
		this.transitionPos = finalPosition + this.fallStartOffset;
		this.finalPos = finalPosition;
		base.StartCoroutine(this.TransitionToNextIdol());
	}

	// Token: 0x060026FE RID: 9982 RVA: 0x0010A830 File Offset: 0x00108A30
	public void UpdateActivationProgress(float rawProgress, bool progressing)
	{
		this.isActivationReady = !progressing;
		if (rawProgress <= 0f && !progressing)
		{
			if (this._activationState >= 0)
			{
				if (this._activationRoutine != null)
				{
					base.StopCoroutine(this._activationRoutine);
					this._activationRoutine = null;
				}
				this.idolMeshRoot.transform.localScale = Vector3.one;
			}
			this._activationState = -1;
			this.UpdateStageActivatedObjects();
			this._audio.GTStop();
			return;
		}
		int num = (int)rawProgress;
		progressing &= (this._activationStageSounds.Length > num);
		if (this._activationState == num || !progressing)
		{
			return;
		}
		if (this._activationRoutine != null)
		{
			base.StopCoroutine(this._activationRoutine);
		}
		this._activationRoutine = base.StartCoroutine(this.ShowActivationEffect());
		this._activationState = num;
		this.UpdateStageActivatedObjects();
		TappableGuardianIdol.IdolActivationSound idolActivationSound = this._activationStageSounds[num];
		this._audio.GTPlayOneShot(idolActivationSound.activation, this._audio.volume);
		this._audio.clip = idolActivationSound.loop;
		this._audio.loop = true;
		this._audio.GTPlay();
	}

	// Token: 0x060026FF RID: 9983 RVA: 0x0004AA3A File Offset: 0x00048C3A
	public void StartLookingAround()
	{
		if (this._lookRoutine != null)
		{
			base.StopCoroutine(this._lookRoutine);
		}
		this._lookRoutine = base.StartCoroutine(this.DoLookingAround());
	}

	// Token: 0x06002700 RID: 9984 RVA: 0x0004AA62 File Offset: 0x00048C62
	public void StopLookingAround()
	{
		if (this._lookRoutine == null)
		{
			return;
		}
		base.StopCoroutine(this._lookRoutine);
		this._lookRoot.localRotation = Quaternion.identity;
		this._lookRoutine = null;
	}

	// Token: 0x06002701 RID: 9985 RVA: 0x0004AA90 File Offset: 0x00048C90
	private IEnumerator DoLookingAround()
	{
		TappableGuardianIdol.<>c__DisplayClass54_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.nextLookTime = Time.time;
		CS$<>8__locals1._lookDirection = this._lookRoot.rotation;
		yield return null;
		for (;;)
		{
			if (Time.time >= CS$<>8__locals1.nextLookTime)
			{
				this.<DoLookingAround>g__PickLookTarget|54_0(ref CS$<>8__locals1);
			}
			this._lookRoot.rotation = Quaternion.Slerp(this._lookRoot.rotation, CS$<>8__locals1._lookDirection, Time.deltaTime * Mathf.Max(1f, (float)this._activationState * this._baseLookRate));
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002702 RID: 9986 RVA: 0x0010A948 File Offset: 0x00108B48
	private void UpdateStageActivatedObjects()
	{
		foreach (TappableGuardianIdol.StageActivatedObject stageActivatedObject in this._stageActivatedObjects)
		{
			stageActivatedObject.UpdateActiveState(this._activationState);
		}
	}

	// Token: 0x06002703 RID: 9987 RVA: 0x0004AA9F File Offset: 0x00048C9F
	private IEnumerator ShowActivationEffect()
	{
		float bulgeDuration = 1f;
		float lerpVal = 0f;
		while (lerpVal < 1f)
		{
			lerpVal += Time.deltaTime / bulgeDuration;
			float num = Mathf.Lerp(1f, this.bulgeScale, this.bulgeCurve.Evaluate(lerpVal));
			this.idolMeshRoot.transform.localScale = Vector3.one * num;
			this.tapCollision.radius = this._colliderBaseRadius * num;
			yield return null;
		}
		this._activationRoutine = null;
		yield break;
	}

	// Token: 0x06002704 RID: 9988 RVA: 0x0004AAAE File Offset: 0x00048CAE
	private IEnumerator TransitionToNextIdol()
	{
		this.isChangingPositions = true;
		this._audio.GTStop();
		if (this.knockbackOnTrigger)
		{
			this.zoneManager.TriggerIdolKnockback();
		}
		if (this.explodeFX)
		{
			ObjectPools.instance.Instantiate(this.explodeFX, base.transform.position);
		}
		this.UpdateActivationProgress(-1f, false);
		this.idolMeshRoot.SetActive(false);
		this.tapCollision.enabled = false;
		base.transform.position = this.transitionPos;
		yield return new WaitForSeconds(this.floatDuration);
		this.idolMeshRoot.SetActive(true);
		this.tapCollision.enabled = true;
		if (this.startFallFX)
		{
			ObjectPools.instance.Instantiate(this.startFallFX, this.transitionPos);
		}
		this._audio.GTPlayOneShot(this._descentSound, 1f);
		this.trailFX.Play();
		float fall = 0f;
		Vector3 startPos = this.transitionPos;
		Vector3 destinationPos = this.finalPos;
		while (fall < this.fallDuration)
		{
			fall += Time.deltaTime;
			base.transform.position = Vector3.Lerp(startPos, destinationPos, fall / this.fallDuration);
			yield return null;
		}
		base.transform.position = destinationPos;
		this.trailFX.Stop();
		if (this.landedFX)
		{
			ObjectPools.instance.Instantiate(this.landedFX, destinationPos);
		}
		if (this.knockbackOnLand)
		{
			this.zoneManager.TriggerIdolKnockback();
		}
		yield return new WaitForSeconds(this.inactiveDuration);
		this._audio.GTPlayOneShot(this._activateSound, this._audio.volume);
		float activateLerp = 0f;
		startPos = this.finalPos;
		destinationPos = this.finalPos + new Vector3(0f, this.activeHeight, 0f);
		AnimationCurve animCurve = AnimationCurves.EaseInOutQuad;
		while (activateLerp < 1f)
		{
			activateLerp = Mathf.Clamp01(activateLerp + Time.deltaTime / this.activationDuration);
			base.transform.position = Vector3.Lerp(startPos, destinationPos, animCurve.Evaluate(activateLerp));
			yield return null;
		}
		if (this.activatedFX)
		{
			ObjectPools.instance.Instantiate(this.activatedFX, base.transform.position);
		}
		if (this.knockbackOnActivate)
		{
			this.zoneManager.TriggerIdolKnockback();
		}
		this.isChangingPositions = false;
		yield break;
	}

	// Token: 0x06002705 RID: 9989 RVA: 0x0004AABD File Offset: 0x00048CBD
	private float EaseInOut(float input)
	{
		if (input >= 0.5f)
		{
			return 1f - Mathf.Pow(-2f * input + 2f, 3f) / 2f;
		}
		return 4f * input * input * input;
	}

	// Token: 0x06002707 RID: 9991 RVA: 0x0004AAF6 File Offset: 0x00048CF6
	[CompilerGenerated]
	private IEnumerator <SetPosition>g__Unshrink|49_0()
	{
		float lerpVal = 0f;
		float growDuration = 0.5f;
		while (lerpVal < 1f)
		{
			lerpVal += Time.deltaTime / growDuration;
			float num = Mathf.Lerp(0f, 1f, AnimationCurves.EaseOutQuad.Evaluate(lerpVal));
			this.idolMeshRoot.transform.localScale = Vector3.one * num;
			this.tapCollision.radius = this._colliderBaseRadius * num;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002708 RID: 9992 RVA: 0x0010AA7C File Offset: 0x00108C7C
	[CompilerGenerated]
	private void <DoLookingAround>g__PickLookTarget|54_0(ref TappableGuardianIdol.<>c__DisplayClass54_0 A_1)
	{
		Transform transform = this.<DoLookingAround>g__GetClosestPlayerPosition|54_2(ref A_1);
		A_1._lookDirection = (transform ? Quaternion.LookRotation(transform.position - this._lookRoot.position) : Quaternion.Euler((float)UnityEngine.Random.Range(-15, 15), this._lookRoot.rotation.eulerAngles.y + (float)UnityEngine.Random.Range(-45, 45), 0f));
		this.<DoLookingAround>g__SetLookTime|54_1(ref A_1);
	}

	// Token: 0x06002709 RID: 9993 RVA: 0x0004AB05 File Offset: 0x00048D05
	[CompilerGenerated]
	private void <DoLookingAround>g__SetLookTime|54_1(ref TappableGuardianIdol.<>c__DisplayClass54_0 A_1)
	{
		A_1.nextLookTime = Time.time + this._lookInterval / (float)this._activationState * 0.5f + UnityEngine.Random.value;
	}

	// Token: 0x0600270A RID: 9994 RVA: 0x0010AAFC File Offset: 0x00108CFC
	[CompilerGenerated]
	private Transform <DoLookingAround>g__GetClosestPlayerPosition|54_2(ref TappableGuardianIdol.<>c__DisplayClass54_0 A_1)
	{
		if (UnityEngine.Random.value < this._randomLookChance)
		{
			return null;
		}
		Vector3 position = base.transform.position;
		float num = float.MaxValue;
		Transform result = null;
		foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
		{
			if (!(vrrig == null))
			{
				bool flag = vrrig.OwningNetPlayer == this.zoneManager.CurrentGuardian;
				float num2 = Vector3.SqrMagnitude(vrrig.transform.position - position) * (float)(flag ? 100 : 1);
				if (num2 < num)
				{
					num = num2;
					result = vrrig.transform;
				}
			}
		}
		return result;
	}

	// Token: 0x04002B05 RID: 11013
	[SerializeField]
	private GorillaGuardianZoneManager zoneManager;

	// Token: 0x04002B06 RID: 11014
	[SerializeField]
	private float floatDuration = 2f;

	// Token: 0x04002B07 RID: 11015
	[SerializeField]
	private float fallDuration = 1.5f;

	// Token: 0x04002B08 RID: 11016
	[SerializeField]
	private float inactiveDuration = 2f;

	// Token: 0x04002B09 RID: 11017
	[SerializeField]
	private float activationDuration = 1f;

	// Token: 0x04002B0A RID: 11018
	[SerializeField]
	private float activeHeight = 1f;

	// Token: 0x04002B0B RID: 11019
	[SerializeField]
	private bool knockbackOnTrigger;

	// Token: 0x04002B0C RID: 11020
	[SerializeField]
	private bool knockbackOnLand = true;

	// Token: 0x04002B0D RID: 11021
	[SerializeField]
	private bool knockbackOnActivate;

	// Token: 0x04002B0E RID: 11022
	[SerializeField]
	private Vector3 fallStartOffset = new Vector3(3f, 20f, 3f);

	// Token: 0x04002B0F RID: 11023
	[SerializeField]
	private ParticleSystem trailFX;

	// Token: 0x04002B10 RID: 11024
	[SerializeField]
	private ParticleSystem tapFX;

	// Token: 0x04002B11 RID: 11025
	[SerializeField]
	private GameObject explodeFX;

	// Token: 0x04002B12 RID: 11026
	[SerializeField]
	private GameObject startFallFX;

	// Token: 0x04002B13 RID: 11027
	[SerializeField]
	private GameObject landedFX;

	// Token: 0x04002B14 RID: 11028
	[SerializeField]
	private GameObject activatedFX;

	// Token: 0x04002B15 RID: 11029
	[SerializeField]
	private SphereCollider tapCollision;

	// Token: 0x04002B16 RID: 11030
	[SerializeField]
	private GameObject idolVisualRoot;

	// Token: 0x04002B17 RID: 11031
	[SerializeField]
	private GameObject idolMeshRoot;

	// Token: 0x04002B18 RID: 11032
	[SerializeField]
	private AnimationCurve bulgeCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.5f, 1f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x04002B19 RID: 11033
	[SerializeField]
	private float bulgeScale = 1.1f;

	// Token: 0x04002B1A RID: 11034
	[SerializeField]
	private AudioSource _audio;

	// Token: 0x04002B1B RID: 11035
	[SerializeField]
	private AudioClip[] _descentSound;

	// Token: 0x04002B1C RID: 11036
	[SerializeField]
	private AudioClip[] _activateSound;

	// Token: 0x04002B1D RID: 11037
	[SerializeField]
	private TappableGuardianIdol.IdolActivationSound[] _activationStageSounds;

	// Token: 0x04002B1E RID: 11038
	[SerializeField]
	private TappableGuardianIdol.StageActivatedObject[] _stageActivatedObjects;

	// Token: 0x04002B1F RID: 11039
	[Header("Look Around")]
	[SerializeField]
	private Transform _lookRoot;

	// Token: 0x04002B20 RID: 11040
	[SerializeField]
	private float _lookInterval = 10f;

	// Token: 0x04002B21 RID: 11041
	[SerializeField]
	private float _baseLookRate = 1f;

	// Token: 0x04002B22 RID: 11042
	[SerializeField]
	private float _randomLookChance = 0.25f;

	// Token: 0x04002B23 RID: 11043
	private Coroutine _lookRoutine;

	// Token: 0x04002B25 RID: 11045
	private Vector3 transitionPos;

	// Token: 0x04002B26 RID: 11046
	private Vector3 finalPos;

	// Token: 0x04002B27 RID: 11047
	private int _activationState;

	// Token: 0x04002B28 RID: 11048
	private Coroutine _activationRoutine;

	// Token: 0x04002B29 RID: 11049
	private float _colliderBaseRadius;

	// Token: 0x04002B2A RID: 11050
	private bool _zoneIsActive = true;

	// Token: 0x04002B2B RID: 11051
	public bool isActivationReady;

	// Token: 0x04002B2C RID: 11052
	private float requiredTapDistance = 3f;

	// Token: 0x02000623 RID: 1571
	[Serializable]
	public struct IdolActivationSound
	{
		// Token: 0x04002B2D RID: 11053
		public AudioClip activation;

		// Token: 0x04002B2E RID: 11054
		public AudioClip loop;
	}

	// Token: 0x02000624 RID: 1572
	[Serializable]
	public struct StageActivatedObject
	{
		// Token: 0x0600270B RID: 9995 RVA: 0x0010ABC8 File Offset: 0x00108DC8
		public void UpdateActiveState(int stage)
		{
			bool active = stage >= this.min && stage <= this.max;
			GameObject[] array = this.objects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(active);
			}
		}

		// Token: 0x04002B2F RID: 11055
		public GameObject[] objects;

		// Token: 0x04002B30 RID: 11056
		public int min;

		// Token: 0x04002B31 RID: 11057
		public int max;
	}
}
