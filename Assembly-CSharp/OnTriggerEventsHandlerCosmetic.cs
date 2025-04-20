using System;
using GorillaTag.Cosmetics;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200044A RID: 1098
[RequireComponent(typeof(OnTriggerEventsCosmetic))]
public class OnTriggerEventsHandlerCosmetic : MonoBehaviour
{
	// Token: 0x06001B0E RID: 6926 RVA: 0x000425D7 File Offset: 0x000407D7
	public void OnTriggerEntered()
	{
		if (this.toggleOnceOnly && this.triggerEntered)
		{
			return;
		}
		this.triggerEntered = true;
		UnityEvent<OnTriggerEventsHandlerCosmetic> unityEvent = this.onTriggerEntered;
		if (unityEvent != null)
		{
			unityEvent.Invoke(this);
		}
		this.ToggleEffects();
	}

	// Token: 0x06001B0F RID: 6927 RVA: 0x000D87C8 File Offset: 0x000D69C8
	public void ToggleEffects()
	{
		if (this.particleToPlay)
		{
			this.particleToPlay.Play();
		}
		if (this.soundBankPlayer)
		{
			this.soundBankPlayer.Play();
		}
		if (this.destroyOnTriggerEnter)
		{
			if (this.destroyDelay > 0f)
			{
				base.Invoke("Destroy", this.destroyDelay);
				return;
			}
			this.Destroy();
		}
	}

	// Token: 0x06001B10 RID: 6928 RVA: 0x00042609 File Offset: 0x00040809
	private void Destroy()
	{
		this.triggerEntered = false;
		if (ObjectPools.instance.DoesPoolExist(base.gameObject))
		{
			ObjectPools.instance.Destroy(base.gameObject);
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04001DD9 RID: 7641
	[SerializeField]
	private ParticleSystem particleToPlay;

	// Token: 0x04001DDA RID: 7642
	[SerializeField]
	private SoundBankPlayer soundBankPlayer;

	// Token: 0x04001DDB RID: 7643
	[SerializeField]
	private bool destroyOnTriggerEnter;

	// Token: 0x04001DDC RID: 7644
	[SerializeField]
	private float destroyDelay = 1f;

	// Token: 0x04001DDD RID: 7645
	[SerializeField]
	private bool toggleOnceOnly;

	// Token: 0x04001DDE RID: 7646
	[HideInInspector]
	public UnityEvent<OnTriggerEventsHandlerCosmetic> onTriggerEntered;

	// Token: 0x04001DDF RID: 7647
	private bool triggerEntered;
}
