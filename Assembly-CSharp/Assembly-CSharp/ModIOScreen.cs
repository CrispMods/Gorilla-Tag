using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000684 RID: 1668
public abstract class ModIOScreen : MonoBehaviour
{
	// Token: 0x06002987 RID: 10631
	public abstract void Initialize();

	// Token: 0x06002988 RID: 10632 RVA: 0x000CDC4E File Offset: 0x000CBE4E
	public virtual void Show()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
			GameEvents.OnModIOKeyboardButtonPressedEvent.AddListener(new UnityAction<ModIOKeyboardButton.ModIOKeyboardBindings>(this.PressButton));
		}
	}

	// Token: 0x06002989 RID: 10633 RVA: 0x000CDC80 File Offset: 0x000CBE80
	public virtual void Hide()
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
			GameEvents.OnModIOKeyboardButtonPressedEvent.RemoveListener(new UnityAction<ModIOKeyboardButton.ModIOKeyboardBindings>(this.PressButton));
		}
	}

	// Token: 0x0600298A RID: 10634 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void PressButton(ModIOKeyboardButton.ModIOKeyboardBindings button)
	{
	}
}
