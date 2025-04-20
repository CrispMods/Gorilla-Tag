using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006A3 RID: 1699
public abstract class CustomMapsTerminalScreen : MonoBehaviour
{
	// Token: 0x06002A42 RID: 10818
	public abstract void Initialize();

	// Token: 0x06002A43 RID: 10819 RVA: 0x0004C80D File Offset: 0x0004AA0D
	public virtual void Show()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
			GameEvents.OnModIOKeyboardButtonPressedEvent.AddListener(new UnityAction<CustomMapsTerminalButton.ModIOKeyboardBindings>(this.PressButton));
		}
	}

	// Token: 0x06002A44 RID: 10820 RVA: 0x0004C83F File Offset: 0x0004AA3F
	public virtual void Hide()
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
			GameEvents.OnModIOKeyboardButtonPressedEvent.RemoveListener(new UnityAction<CustomMapsTerminalButton.ModIOKeyboardBindings>(this.PressButton));
		}
	}

	// Token: 0x06002A45 RID: 10821 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void PressButton(CustomMapsTerminalButton.ModIOKeyboardBindings button)
	{
	}
}
