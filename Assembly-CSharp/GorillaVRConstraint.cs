using System;
using UnityEngine;

// Token: 0x020005A7 RID: 1447
public class GorillaVRConstraint : MonoBehaviour
{
	// Token: 0x060023E7 RID: 9191 RVA: 0x000B30E4 File Offset: 0x000B12E4
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

	// Token: 0x040027E6 RID: 10214
	public bool isConstrained;

	// Token: 0x040027E7 RID: 10215
	public float angle = 3600f;
}
