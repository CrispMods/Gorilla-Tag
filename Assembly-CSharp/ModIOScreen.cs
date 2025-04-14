using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000683 RID: 1667
public abstract class ModIOScreen : MonoBehaviour
{
	// Token: 0x0600297F RID: 10623
	public abstract void Initialize();

	// Token: 0x06002980 RID: 10624 RVA: 0x000CD7CE File Offset: 0x000CB9CE
	public virtual void Show()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
			GameEvents.OnModIOKeyboardButtonPressedEvent.AddListener(new UnityAction<ModIOKeyboardButton.ModIOKeyboardBindings>(this.PressButton));
		}
	}

	// Token: 0x06002981 RID: 10625 RVA: 0x000CD800 File Offset: 0x000CBA00
	public virtual void Hide()
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
			GameEvents.OnModIOKeyboardButtonPressedEvent.RemoveListener(new UnityAction<ModIOKeyboardButton.ModIOKeyboardBindings>(this.PressButton));
		}
	}

	// Token: 0x06002982 RID: 10626 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void PressButton(ModIOKeyboardButton.ModIOKeyboardBindings button)
	{
	}
}
