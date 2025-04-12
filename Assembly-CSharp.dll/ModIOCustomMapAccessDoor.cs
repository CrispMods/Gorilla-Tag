using System;
using UnityEngine;

// Token: 0x020005EF RID: 1519
public class ModIOCustomMapAccessDoor : MonoBehaviour
{
	// Token: 0x060025D0 RID: 9680 RVA: 0x00048B8F File Offset: 0x00046D8F
	public void OpenDoor()
	{
		if (this.openDoorObject != null)
		{
			this.openDoorObject.SetActive(true);
		}
		if (this.closedDoorObject != null)
		{
			this.closedDoorObject.SetActive(false);
		}
	}

	// Token: 0x060025D1 RID: 9681 RVA: 0x00048BC5 File Offset: 0x00046DC5
	public void CloseDoor()
	{
		if (this.openDoorObject != null)
		{
			this.openDoorObject.SetActive(false);
		}
		if (this.closedDoorObject != null)
		{
			this.closedDoorObject.SetActive(true);
		}
	}

	// Token: 0x040029E7 RID: 10727
	public GameObject openDoorObject;

	// Token: 0x040029E8 RID: 10728
	public GameObject closedDoorObject;
}
