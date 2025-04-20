using System;
using UnityEngine;

// Token: 0x0200065F RID: 1631
public class CustomMapAccessDoor : MonoBehaviour
{
	// Token: 0x06002862 RID: 10338 RVA: 0x0004B6E6 File Offset: 0x000498E6
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

	// Token: 0x06002863 RID: 10339 RVA: 0x0004B71C File Offset: 0x0004991C
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

	// Token: 0x04002DCB RID: 11723
	public GameObject openDoorObject;

	// Token: 0x04002DCC RID: 11724
	public GameObject closedDoorObject;
}
