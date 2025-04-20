using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
public class CritterDespawner : MonoBehaviour
{
	// Token: 0x060000A7 RID: 167 RVA: 0x00030B0B File Offset: 0x0002ED0B
	public void DespawnAllCritters()
	{
		CrittersManager.instance.QueueDespawnAllCritters();
	}
}
