using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020002F1 RID: 753
public class ButtonDownListener : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x1400003E RID: 62
	// (add) Token: 0x0600120E RID: 4622 RVA: 0x000AD3A0 File Offset: 0x000AB5A0
	// (remove) Token: 0x0600120F RID: 4623 RVA: 0x000AD3D8 File Offset: 0x000AB5D8
	public event Action onButtonDown;

	// Token: 0x06001210 RID: 4624 RVA: 0x0003B6B5 File Offset: 0x000398B5
	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.onButtonDown != null)
		{
			this.onButtonDown();
		}
	}
}
