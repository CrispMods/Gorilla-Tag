using System;
using UnityEngine;

// Token: 0x020005A8 RID: 1448
public class GorillaVRConstraint : MonoBehaviour
{
	// Token: 0x060023EF RID: 9199 RVA: 0x0004744C File Offset: 0x0004564C
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

	// Token: 0x040027EC RID: 10220
	public bool isConstrained;

	// Token: 0x040027ED RID: 10221
	public float angle = 3600f;
}
