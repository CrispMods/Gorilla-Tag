using System;
using UnityEngine;

// Token: 0x02000455 RID: 1109
public class KinematicWhenTargetInactive : MonoBehaviour
{
	// Token: 0x06001B57 RID: 6999 RVA: 0x000D9C8C File Offset: 0x000D7E8C
	private void LateUpdate()
	{
		if (!this.target.activeSelf)
		{
			foreach (Rigidbody rigidbody in this.rigidBodies)
			{
				if (!rigidbody.isKinematic)
				{
					rigidbody.isKinematic = true;
				}
			}
			return;
		}
		foreach (Rigidbody rigidbody2 in this.rigidBodies)
		{
			if (rigidbody2.isKinematic)
			{
				rigidbody2.isKinematic = false;
			}
		}
	}

	// Token: 0x04001E3D RID: 7741
	public Rigidbody[] rigidBodies;

	// Token: 0x04001E3E RID: 7742
	public GameObject target;
}
