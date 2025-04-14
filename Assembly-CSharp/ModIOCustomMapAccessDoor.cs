using System;
using UnityEngine;

// Token: 0x020005EE RID: 1518
public class ModIOCustomMapAccessDoor : MonoBehaviour
{
	// Token: 0x060025C8 RID: 9672 RVA: 0x000BA755 File Offset: 0x000B8955
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

	// Token: 0x060025C9 RID: 9673 RVA: 0x000BA78B File Offset: 0x000B898B
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

	// Token: 0x040029E1 RID: 10721
	public GameObject openDoorObject;

	// Token: 0x040029E2 RID: 10722
	public GameObject closedDoorObject;
}
