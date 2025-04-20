using System;
using UnityEngine;

// Token: 0x02000831 RID: 2097
public class BuildTargetManager : MonoBehaviour
{
	// Token: 0x060033A1 RID: 13217 RVA: 0x00051FA5 File Offset: 0x000501A5
	public string GetPath()
	{
		return this.path;
	}

	// Token: 0x04003702 RID: 14082
	public BuildTargetManager.BuildTowards newBuildTarget;

	// Token: 0x04003703 RID: 14083
	public bool isBeta;

	// Token: 0x04003704 RID: 14084
	public bool isQA;

	// Token: 0x04003705 RID: 14085
	public bool spoofIDs;

	// Token: 0x04003706 RID: 14086
	public bool spoofChild;

	// Token: 0x04003707 RID: 14087
	public bool enableAllCosmetics;

	// Token: 0x04003708 RID: 14088
	public OVRManager ovrManager;

	// Token: 0x04003709 RID: 14089
	private string path = "Assets/csc.rsp";

	// Token: 0x0400370A RID: 14090
	public BuildTargetManager.BuildTowards currentBuildTargetDONOTCHANGE;

	// Token: 0x0400370B RID: 14091
	public GorillaTagger gorillaTagger;

	// Token: 0x0400370C RID: 14092
	public GameObject[] betaDisableObjects;

	// Token: 0x0400370D RID: 14093
	public GameObject[] betaEnableObjects;

	// Token: 0x0400370E RID: 14094
	public BuildTargetManager.NetworkBackend networkBackend;

	// Token: 0x02000832 RID: 2098
	public enum BuildTowards
	{
		// Token: 0x04003710 RID: 14096
		Steam,
		// Token: 0x04003711 RID: 14097
		OculusPC,
		// Token: 0x04003712 RID: 14098
		Quest,
		// Token: 0x04003713 RID: 14099
		Viveport
	}

	// Token: 0x02000833 RID: 2099
	public enum NetworkBackend
	{
		// Token: 0x04003715 RID: 14101
		Pun,
		// Token: 0x04003716 RID: 14102
		Fusion
	}
}
