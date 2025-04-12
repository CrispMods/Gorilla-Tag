using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000644 RID: 1604
[DisallowMultipleComponent]
public class TappableGuardianIdol : Tappable
{
	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x060027D3 RID: 10195 RVA: 0x0004A3E0 File Offset: 0x000485E0
	// (set) Token: 0x060027D4 RID: 10196 RVA: 0x0004A3E8 File Offset: 0x000485E8
	public bool isChangingPositions { get; private set; }

	// Token: 0x060027D5 RID: 10197 RVA: 0x0004A3F1 File Offset: 0x000485F1
	protected override void OnEnable()
	{
		base.OnEnable();
		this._colliderBaseRadius = this.tapCollision.radius;
	}

	// Token: 0x060027D6 RID: 10198 RVA: 0x0004A40A File Offset: 0x0004860A
	protected override void OnDisable()
	{
		base.OnDisable();
		this.isChangingPositions = false;
		this._activationState = -1;
		this.isActivationReady = true;
		this.tapCollision.radius = this._colliderBaseRadius;
	}

	// Token: 0x060027D7 RID: 10199 RVA: 0x0004A438 File Offset: 0x00048638
	public void OnZoneActiveStateChanged(bool zoneActive)
	{
		GTDev.Log<string>(string.Format("OnZoneActiveStateChanged({0}->{1})", this._zoneIsActive, zoneActive), this, null);
		this._zoneIsActive = zoneActive;
		this.idolVisualRoot.SetActive(this._zoneIsActive);
	}

	// Token: 0x060027D8 RID: 10200 RVA: 0x0010C2D8 File Offset: 0x0010A4D8
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

	// Token: 0x060027D9 RID: 10201 RVA: 0x0010C3A4 File Offset: 0x0010A5A4
	public void SetPosition(Vector3 position)
	{
		base.transform.position = position + new Vector3(0f, this.activeHeight, 0f);
		this.UpdateStageActivatedObjects();
		this._audio.GTPlayOneShot(this._activateSound, this._audio.volume);
		base.StartCoroutine(this.<SetPosition>g__Unshrink|49_0());
	}

	// Token: 0x060027DA RID: 10202 RVA: 0x0004A474 File Offset: 0x00048674
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

	// Token: 0x060027DB RID: 10203 RVA: 0x0010C408 File Offset: 0x0010A608
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

	// Token: 0x060027DC RID: 10204 RVA: 0x0004A4A5 File Offset: 0x000486A5
	public void StartLookingAround()
	{
		if (this._lookRoutine != null)
		{
			base.StopCoroutine(this._lookRoutine);
		}
		this._lookRoutine = base.StartCoroutine(this.DoLookingAround());
	}

	// Token: 0x060027DD RID: 10205 RVA: 0x0004A4CD File Offset: 0x000486CD
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

	// Token: 0x060027DE RID: 10206 RVA: 0x0004A4FB File Offset: 0x000486FB
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

	// Token: 0x060027DF RID: 10207 RVA: 0x0010C520 File Offset: 0x0010A720
	private void UpdateStageActivatedObjects()
	{
		foreach (TappableGuardianIdol.StageActivatedObject stageActivatedObject in this._stageActivatedObjects)
		{
			stageActivatedObject.UpdateActiveState(this._activationState);
		}
	}

	// Token: 0x060027E0 RID: 10208 RVA: 0x0004A50A File Offset: 0x0004870A
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

	// Token: 0x060027E1 RID: 10209 RVA: 0x0004A519 File Offset: 0x00048719
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

	// Token: 0x060027E2 RID: 10210 RVA: 0x0004A528 File Offset: 0x00048728
	private float EaseInOut(float input)
	{
		if (input >= 0.5f)
		{
			return 1f - Mathf.Pow(-2f * input + 2f, 3f) / 2f;
		}
		return 4f * input * input * input;
	}

	// Token: 0x060027E4 RID: 10212 RVA: 0x0004A561 File Offset: 0x00048761
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

	// Token: 0x060027E5 RID: 10213 RVA: 0x0010C654 File Offset: 0x0010A854
	[CompilerGenerated]
	private void <DoLookingAround>g__PickLookTarget|54_0(ref TappableGuardianIdol.<>c__DisplayClass54_0 A_1)
	{
		Transform transform = this.<DoLookingAround>g__GetClosestPlayerPosition|54_2(ref A_1);
		A_1._lookDirection = (transform ? Quaternion.LookRotation(transform.position - this._lookRoot.position) : Quaternion.Euler((float)UnityEngine.Random.Range(-15, 15), this._lookRoot.rotation.eulerAngles.y + (float)UnityEngine.Random.Range(-45, 45), 0f));
		this.<DoLookingAround>g__SetLookTime|54_1(ref A_1);
	}

	// Token: 0x060027E6 RID: 10214 RVA: 0x0004A570 File Offset: 0x00048770
	[CompilerGenerated]
	private void <DoLookingAround>g__SetLookTime|54_1(ref TappableGuardianIdol.<>c__DisplayClass54_0 A_1)
	{
		A_1.nextLookTime = Time.time + this._lookInterval / (float)this._activationState * 0.5f + UnityEngine.Random.value;
	}

	// Token: 0x060027E7 RID: 10215 RVA: 0x0010C6D4 File Offset: 0x0010A8D4
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

	// Token: 0x04002BA5 RID: 11173
	[SerializeField]
	private GorillaGuardianZoneManager zoneManager;

	// Token: 0x04002BA6 RID: 11174
	[SerializeField]
	private float floatDuration = 2f;

	// Token: 0x04002BA7 RID: 11175
	[SerializeField]
	private float fallDuration = 1.5f;

	// Token: 0x04002BA8 RID: 11176
	[SerializeField]
	private float inactiveDuration = 2f;

	// Token: 0x04002BA9 RID: 11177
	[SerializeField]
	private float activationDuration = 1f;

	// Token: 0x04002BAA RID: 11178
	[SerializeField]
	private float activeHeight = 1f;

	// Token: 0x04002BAB RID: 11179
	[SerializeField]
	private bool knockbackOnTrigger;

	// Token: 0x04002BAC RID: 11180
	[SerializeField]
	private bool knockbackOnLand = true;

	// Token: 0x04002BAD RID: 11181
	[SerializeField]
	private bool knockbackOnActivate;

	// Token: 0x04002BAE RID: 11182
	[SerializeField]
	private Vector3 fallStartOffset = new Vector3(3f, 20f, 3f);

	// Token: 0x04002BAF RID: 11183
	[SerializeField]
	private ParticleSystem trailFX;

	// Token: 0x04002BB0 RID: 11184
	[SerializeField]
	private ParticleSystem tapFX;

	// Token: 0x04002BB1 RID: 11185
	[SerializeField]
	private GameObject explodeFX;

	// Token: 0x04002BB2 RID: 11186
	[SerializeField]
	private GameObject startFallFX;

	// Token: 0x04002BB3 RID: 11187
	[SerializeField]
	private GameObject landedFX;

	// Token: 0x04002BB4 RID: 11188
	[SerializeField]
	private GameObject activatedFX;

	// Token: 0x04002BB5 RID: 11189
	[SerializeField]
	private SphereCollider tapCollision;

	// Token: 0x04002BB6 RID: 11190
	[SerializeField]
	private GameObject idolVisualRoot;

	// Token: 0x04002BB7 RID: 11191
	[SerializeField]
	private GameObject idolMeshRoot;

	// Token: 0x04002BB8 RID: 11192
	[SerializeField]
	private AnimationCurve bulgeCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.5f, 1f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x04002BB9 RID: 11193
	[SerializeField]
	private float bulgeScale = 1.1f;

	// Token: 0x04002BBA RID: 11194
	[SerializeField]
	private AudioSource _audio;

	// Token: 0x04002BBB RID: 11195
	[SerializeField]
	private AudioClip[] _descentSound;

	// Token: 0x04002BBC RID: 11196
	[SerializeField]
	private AudioClip[] _activateSound;

	// Token: 0x04002BBD RID: 11197
	[SerializeField]
	private TappableGuardianIdol.IdolActivationSound[] _activationStageSounds;

	// Token: 0x04002BBE RID: 11198
	[SerializeField]
	private TappableGuardianIdol.StageActivatedObject[] _stageActivatedObjects;

	// Token: 0x04002BBF RID: 11199
	[Header("Look Around")]
	[SerializeField]
	private Transform _lookRoot;

	// Token: 0x04002BC0 RID: 11200
	[SerializeField]
	private float _lookInterval = 10f;

	// Token: 0x04002BC1 RID: 11201
	[SerializeField]
	private float _baseLookRate = 1f;

	// Token: 0x04002BC2 RID: 11202
	[SerializeField]
	private float _randomLookChance = 0.25f;

	// Token: 0x04002BC3 RID: 11203
	private Coroutine _lookRoutine;

	// Token: 0x04002BC5 RID: 11205
	private Vector3 transitionPos;

	// Token: 0x04002BC6 RID: 11206
	private Vector3 finalPos;

	// Token: 0x04002BC7 RID: 11207
	private int _activationState;

	// Token: 0x04002BC8 RID: 11208
	private Coroutine _activationRoutine;

	// Token: 0x04002BC9 RID: 11209
	private float _colliderBaseRadius;

	// Token: 0x04002BCA RID: 11210
	private bool _zoneIsActive = true;

	// Token: 0x04002BCB RID: 11211
	public bool isActivationReady;

	// Token: 0x04002BCC RID: 11212
	private float requiredTapDistance = 3f;

	// Token: 0x02000645 RID: 1605
	[Serializable]
	public struct IdolActivationSound
	{
		// Token: 0x04002BCD RID: 11213
		public AudioClip activation;

		// Token: 0x04002BCE RID: 11214
		public AudioClip loop;
	}

	// Token: 0x02000646 RID: 1606
	[Serializable]
	public struct StageActivatedObject
	{
		// Token: 0x060027E8 RID: 10216 RVA: 0x0010C7A0 File Offset: 0x0010A9A0
		public void UpdateActiveState(int stage)
		{
			bool active = stage >= this.min && stage <= this.max;
			GameObject[] array = this.objects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(active);
			}
		}

		// Token: 0x04002BCF RID: 11215
		public GameObject[] objects;

		// Token: 0x04002BD0 RID: 11216
		public int min;

		// Token: 0x04002BD1 RID: 11217
		public int max;
	}
}
