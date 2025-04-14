using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020002F1 RID: 753
public class ButtonDownListener : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x1400003E RID: 62
	// (add) Token: 0x0600120B RID: 4619 RVA: 0x00055488 File Offset: 0x00053688
	// (remove) Token: 0x0600120C RID: 4620 RVA: 0x000554C0 File Offset: 0x000536C0
	public event Action onButtonDown;

	// Token: 0x0600120D RID: 4621 RVA: 0x000554F5 File Offset: 0x000536F5
	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.onButtonDown != null)
		{
			this.onButtonDown();
		}
	}
}
