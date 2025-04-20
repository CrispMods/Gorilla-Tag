using System;
using System.Collections;
using GorillaTagScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x020005C5 RID: 1477
public class HauntedObject : MonoBehaviour
{
	// Token: 0x060024B8 RID: 9400 RVA: 0x00103A00 File Offset: 0x00101C00
	private void Awake()
	{
		this.lurkerGhost = GameObject.FindGameObjectWithTag("LurkerGhost");
		LurkerGhost lurkerGhost;
		if (this.lurkerGhost != null && this.lurkerGhost.TryGetComponent<LurkerGhost>(out lurkerGhost))
		{
			LurkerGhost lurkerGhost2 = lurkerGhost;
			lurkerGhost2.TriggerHauntedObjects = (UnityAction<GameObject>)Delegate.Combine(lurkerGhost2.TriggerHauntedObjects, new UnityAction<GameObject>(this.TriggerEffects));
		}
		this.wanderingGhost = GameObject.FindGameObjectWithTag("WanderingGhost");
		WanderingGhost wanderingGhost;
		if (this.wanderingGhost != null && this.wanderingGhost.TryGetComponent<WanderingGhost>(out wanderingGhost))
		{
			WanderingGhost wanderingGhost2 = wanderingGhost;
			wanderingGhost2.TriggerHauntedObjects = (UnityAction<GameObject>)Delegate.Combine(wanderingGhost2.TriggerHauntedObjects, new UnityAction<GameObject>(this.TriggerEffects));
		}
		this.animators = base.transform.GetComponentsInChildren<Animator>();
	}

	// Token: 0x060024B9 RID: 9401 RVA: 0x00103ABC File Offset: 0x00101CBC
	private void OnDestroy()
	{
		LurkerGhost lurkerGhost;
		if (this.lurkerGhost != null && this.lurkerGhost.TryGetComponent<LurkerGhost>(out lurkerGhost))
		{
			LurkerGhost lurkerGhost2 = lurkerGhost;
			lurkerGhost2.TriggerHauntedObjects = (UnityAction<GameObject>)Delegate.Remove(lurkerGhost2.TriggerHauntedObjects, new UnityAction<GameObject>(this.TriggerEffects));
		}
		WanderingGhost wanderingGhost;
		if (this.wanderingGhost != null && this.wanderingGhost.TryGetComponent<WanderingGhost>(out wanderingGhost))
		{
			WanderingGhost wanderingGhost2 = wanderingGhost;
			wanderingGhost2.TriggerHauntedObjects = (UnityAction<GameObject>)Delegate.Remove(wanderingGhost2.TriggerHauntedObjects, new UnityAction<GameObject>(this.TriggerEffects));
		}
	}

	// Token: 0x060024BA RID: 9402 RVA: 0x00048E87 File Offset: 0x00047087
	private void Start()
	{
		this.initialPos = base.transform.position;
		this.passedTime = 0f;
		this.lightPassedTime = 0f;
	}

	// Token: 0x060024BB RID: 9403 RVA: 0x00103B48 File Offset: 0x00101D48
	private void TriggerEffects(GameObject go)
	{
		if (base.gameObject != go)
		{
			return;
		}
		if (this.rattle)
		{
			base.StartCoroutine("Shake");
		}
		if (this.audioSource && this.hauntedSound)
		{
			this.audioSource.GTPlayOneShot(this.hauntedSound, 1f);
		}
		if (this.FBXprefab)
		{
			ObjectPools.instance.Instantiate(this.FBXprefab, base.transform.position);
		}
		if (this.TurnOffLight != null)
		{
			base.StartCoroutine("TurnOff");
		}
		foreach (Animator animator in this.animators)
		{
			if (animator)
			{
				animator.SetTrigger("Haunted");
			}
		}
	}

	// Token: 0x060024BC RID: 9404 RVA: 0x00048EB0 File Offset: 0x000470B0
	private IEnumerator Shake()
	{
		while (this.passedTime < this.duration)
		{
			this.passedTime += Time.deltaTime;
			base.transform.position = new Vector3(this.initialPos.x + Mathf.Sin(Time.time * this.speed) * this.amount, this.initialPos.y + Mathf.Sin(Time.time * this.speed) * this.amount, this.initialPos.z);
			yield return null;
		}
		this.passedTime = 0f;
		yield break;
	}

	// Token: 0x060024BD RID: 9405 RVA: 0x00048EBF File Offset: 0x000470BF
	private IEnumerator TurnOff()
	{
		this.TurnOffLight.gameObject.SetActive(false);
		while (this.lightPassedTime < this.TurnOffDuration)
		{
			this.lightPassedTime += Time.deltaTime;
			yield return null;
		}
		this.TurnOffLight.SetActive(true);
		this.lightPassedTime = 0f;
		yield break;
	}

	// Token: 0x040028CC RID: 10444
	[Tooltip("If this box is checked, then object will rattle when hunted")]
	public bool rattle;

	// Token: 0x040028CD RID: 10445
	public float speed = 60f;

	// Token: 0x040028CE RID: 10446
	public float amount = 0.01f;

	// Token: 0x040028CF RID: 10447
	public float duration = 1f;

	// Token: 0x040028D0 RID: 10448
	[FormerlySerializedAs("FBX")]
	public GameObject FBXprefab;

	// Token: 0x040028D1 RID: 10449
	[Tooltip("Use to turn off a game object like candle flames when hunted")]
	public GameObject TurnOffLight;

	// Token: 0x040028D2 RID: 10450
	public float TurnOffDuration = 2f;

	// Token: 0x040028D3 RID: 10451
	private Vector3 initialPos;

	// Token: 0x040028D4 RID: 10452
	private float passedTime;

	// Token: 0x040028D5 RID: 10453
	private float lightPassedTime;

	// Token: 0x040028D6 RID: 10454
	private GameObject lurkerGhost;

	// Token: 0x040028D7 RID: 10455
	private GameObject wanderingGhost;

	// Token: 0x040028D8 RID: 10456
	private Animator[] animators;

	// Token: 0x040028D9 RID: 10457
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x040028DA RID: 10458
	[FormerlySerializedAs("rattlingSound")]
	public AudioClip hauntedSound;
}
