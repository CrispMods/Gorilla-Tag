using System;
using System.Collections;
using GorillaTagScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x020005B7 RID: 1463
public class HauntedObject : MonoBehaviour
{
	// Token: 0x06002456 RID: 9302 RVA: 0x000B508C File Offset: 0x000B328C
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

	// Token: 0x06002457 RID: 9303 RVA: 0x000B5148 File Offset: 0x000B3348
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

	// Token: 0x06002458 RID: 9304 RVA: 0x000B51D3 File Offset: 0x000B33D3
	private void Start()
	{
		this.initialPos = base.transform.position;
		this.passedTime = 0f;
		this.lightPassedTime = 0f;
	}

	// Token: 0x06002459 RID: 9305 RVA: 0x000B51FC File Offset: 0x000B33FC
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

	// Token: 0x0600245A RID: 9306 RVA: 0x000B52CB File Offset: 0x000B34CB
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

	// Token: 0x0600245B RID: 9307 RVA: 0x000B52DA File Offset: 0x000B34DA
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

	// Token: 0x0400286F RID: 10351
	[Tooltip("If this box is checked, then object will rattle when hunted")]
	public bool rattle;

	// Token: 0x04002870 RID: 10352
	public float speed = 60f;

	// Token: 0x04002871 RID: 10353
	public float amount = 0.01f;

	// Token: 0x04002872 RID: 10354
	public float duration = 1f;

	// Token: 0x04002873 RID: 10355
	[FormerlySerializedAs("FBX")]
	public GameObject FBXprefab;

	// Token: 0x04002874 RID: 10356
	[Tooltip("Use to turn off a game object like candle flames when hunted")]
	public GameObject TurnOffLight;

	// Token: 0x04002875 RID: 10357
	public float TurnOffDuration = 2f;

	// Token: 0x04002876 RID: 10358
	private Vector3 initialPos;

	// Token: 0x04002877 RID: 10359
	private float passedTime;

	// Token: 0x04002878 RID: 10360
	private float lightPassedTime;

	// Token: 0x04002879 RID: 10361
	private GameObject lurkerGhost;

	// Token: 0x0400287A RID: 10362
	private GameObject wanderingGhost;

	// Token: 0x0400287B RID: 10363
	private Animator[] animators;

	// Token: 0x0400287C RID: 10364
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x0400287D RID: 10365
	[FormerlySerializedAs("rattlingSound")]
	public AudioClip hauntedSound;
}
