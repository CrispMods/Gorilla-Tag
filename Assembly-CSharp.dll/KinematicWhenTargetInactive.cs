using System;
using UnityEngine;

// Token: 0x02000449 RID: 1097
public class KinematicWhenTargetInactive : MonoBehaviour
{
	// Token: 0x06001B06 RID: 6918 RVA: 0x000D6FEC File Offset: 0x000D51EC
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

	// Token: 0x04001DEF RID: 7663
	public Rigidbody[] rigidBodies;

	// Token: 0x04001DF0 RID: 7664
	public GameObject target;
}
