using System;
using System.Collections;
using GorillaTagScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x020005B8 RID: 1464
public class HauntedObject : MonoBehaviour
{
	// Token: 0x0600245E RID: 9310 RVA: 0x00100B4C File Offset: 0x000FED4C
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

	// Token: 0x0600245F RID: 9311 RVA: 0x00100C08 File Offset: 0x000FEE08
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

	// Token: 0x06002460 RID: 9312 RVA: 0x00047A7E File Offset: 0x00045C7E
	private void Start()
	{
		this.initialPos = base.transform.position;
		this.passedTime = 0f;
		this.lightPassedTime = 0f;
	}

	// Token: 0x06002461 RID: 9313 RVA: 0x00100C94 File Offset: 0x000FEE94
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

	// Token: 0x06002462 RID: 9314 RVA: 0x00047AA7 File Offset: 0x00045CA7
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

	// Token: 0x06002463 RID: 9315 RVA: 0x00047AB6 File Offset: 0x00045CB6
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

	// Token: 0x04002875 RID: 10357
	[Tooltip("If this box is checked, then object will rattle when hunted")]
	public bool rattle;

	// Token: 0x04002876 RID: 10358
	public float speed = 60f;

	// Token: 0x04002877 RID: 10359
	public float amount = 0.01f;

	// Token: 0x04002878 RID: 10360
	public float duration = 1f;

	// Token: 0x04002879 RID: 10361
	[FormerlySerializedAs("FBX")]
	public GameObject FBXprefab;

	// Token: 0x0400287A RID: 10362
	[Tooltip("Use to turn off a game object like candle flames when hunted")]
	public GameObject TurnOffLight;

	// Token: 0x0400287B RID: 10363
	public float TurnOffDuration = 2f;

	// Token: 0x0400287C RID: 10364
	private Vector3 initialPos;

	// Token: 0x0400287D RID: 10365
	private float passedTime;

	// Token: 0x0400287E RID: 10366
	private float lightPassedTime;

	// Token: 0x0400287F RID: 10367
	private GameObject lurkerGhost;

	// Token: 0x04002880 RID: 10368
	private GameObject wanderingGhost;

	// Token: 0x04002881 RID: 10369
	private Animator[] animators;

	// Token: 0x04002882 RID: 10370
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04002883 RID: 10371
	[FormerlySerializedAs("rattlingSound")]
	public AudioClip hauntedSound;
}
