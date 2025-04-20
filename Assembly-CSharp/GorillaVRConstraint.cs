using System;
using UnityEngine;

// Token: 0x020005B5 RID: 1461
public class GorillaVRConstraint : MonoBehaviour
{
	// Token: 0x06002447 RID: 9287 RVA: 0x00048821 File Offset: 0x00046A21
	private void Update()
	{
		if (NetworkSystem.Instance.WrongVersion)
		{
			this.isConstrained = true;
		}
		if (this.isConstrained && Time.realtimeSinceStartup > this.angle)
		{
			GorillaGameManager.ForceStopGame_DisconnectAndDestroy();
		}
	}

	// Token: 0x04002842 RID: 10306
	public bool isConstrained;

	// Token: 0x04002843 RID: 10307
	public float angle = 3600f;
}
