using System;
using GorillaTag.Cosmetics;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200043E RID: 1086
[RequireComponent(typeof(OnTriggerEventsCosmetic))]
public class OnTriggerEventsHandlerCosmetic : MonoBehaviour
{
	// Token: 0x06001ABD RID: 6845 RVA: 0x00083C4E File Offset: 0x00081E4E
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

	// Token: 0x06001ABE RID: 6846 RVA: 0x00083C80 File Offset: 0x00081E80
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

	// Token: 0x06001ABF RID: 6847 RVA: 0x00083CEA File Offset: 0x00081EEA
	private void Destroy()
	{
		this.triggerEntered = false;
		if (ObjectPools.instance.DoesPoolExist(base.gameObject))
		{
			ObjectPools.instance.Destroy(base.gameObject);
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001D8B RID: 7563
	[SerializeField]
	private ParticleSystem particleToPlay;

	// Token: 0x04001D8C RID: 7564
	[SerializeField]
	private SoundBankPlayer soundBankPlayer;

	// Token: 0x04001D8D RID: 7565
	[SerializeField]
	private bool destroyOnTriggerEnter;

	// Token: 0x04001D8E RID: 7566
	[SerializeField]
	private float destroyDelay = 1f;

	// Token: 0x04001D8F RID: 7567
	[SerializeField]
	private bool toggleOnceOnly;

	// Token: 0x04001D90 RID: 7568
	[HideInInspector]
	public UnityEvent<OnTriggerEventsHandlerCosmetic> onTriggerEntered;

	// Token: 0x04001D91 RID: 7569
	private bool triggerEntered;
}
