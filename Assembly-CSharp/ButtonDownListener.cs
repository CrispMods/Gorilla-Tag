using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020002FC RID: 764
public class ButtonDownListener : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x1400003F RID: 63
	// (add) Token: 0x06001257 RID: 4695 RVA: 0x000AFC38 File Offset: 0x000ADE38
	// (remove) Token: 0x06001258 RID: 4696 RVA: 0x000AFC70 File Offset: 0x000ADE70
	public event Action onButtonDown;

	// Token: 0x06001259 RID: 4697 RVA: 0x0003C975 File Offset: 0x0003AB75
	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.onButtonDown != null)
		{
			this.onButtonDown();
		}
	}
}
