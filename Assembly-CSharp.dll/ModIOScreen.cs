using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000684 RID: 1668
public abstract class ModIOScreen : MonoBehaviour
{
	// Token: 0x06002987 RID: 10631
	public abstract void Initialize();

	// Token: 0x06002988 RID: 10632 RVA: 0x0004B489 File Offset: 0x00049689
	public virtual void Show()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
			GameEvents.OnModIOKeyboardButtonPressedEvent.AddListener(new UnityAction<ModIOKeyboardButton.ModIOKeyboardBindings>(this.PressButton));
		}
	}

	// Token: 0x06002989 RID: 10633 RVA: 0x0004B4BB File Offset: 0x000496BB
	public virtual void Hide()
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
			GameEvents.OnModIOKeyboardButtonPressedEvent.RemoveListener(new UnityAction<ModIOKeyboardButton.ModIOKeyboardBindings>(this.PressButton));
		}
	}

	// Token: 0x0600298A RID: 10634 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void PressButton(ModIOKeyboardButton.ModIOKeyboardBindings button)
	{
	}
}
