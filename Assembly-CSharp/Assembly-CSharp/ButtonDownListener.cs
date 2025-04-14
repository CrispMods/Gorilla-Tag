using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020002F1 RID: 753
public class ButtonDownListener : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x1400003E RID: 62
	// (add) Token: 0x0600120E RID: 4622 RVA: 0x0005580C File Offset: 0x00053A0C
	// (remove) Token: 0x0600120F RID: 4623 RVA: 0x00055844 File Offset: 0x00053A44
	public event Action onButtonDown;

	// Token: 0x06001210 RID: 4624 RVA: 0x00055879 File Offset: 0x00053A79
	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.onButtonDown != null)
		{
			this.onButtonDown();
		}
	}
}
