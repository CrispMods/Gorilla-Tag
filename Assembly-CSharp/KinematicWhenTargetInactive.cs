using System;
using UnityEngine;

// Token: 0x02000449 RID: 1097
public class KinematicWhenTargetInactive : MonoBehaviour
{
	// Token: 0x06001B03 RID: 6915 RVA: 0x00085104 File Offset: 0x00083304
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

	// Token: 0x04001DEE RID: 7662
	public Rigidbody[] rigidBodies;

	// Token: 0x04001DEF RID: 7663
	public GameObject target;
}
