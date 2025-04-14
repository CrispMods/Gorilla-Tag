using System;
using UnityEngine;

// Token: 0x0200081A RID: 2074
public class BuildTargetManager : MonoBehaviour
{
	// Token: 0x060032F2 RID: 13042 RVA: 0x000F4423 File Offset: 0x000F2623
	public string GetPath()
	{
		return this.path;
	}

	// Token: 0x04003658 RID: 13912
	public BuildTargetManager.BuildTowards newBuildTarget;

	// Token: 0x04003659 RID: 13913
	public bool isBeta;

	// Token: 0x0400365A RID: 13914
	public bool isQA;

	// Token: 0x0400365B RID: 13915
	public bool spoofIDs;

	// Token: 0x0400365C RID: 13916
	public bool spoofChild;

	// Token: 0x0400365D RID: 13917
	public bool enableAllCosmetics;

	// Token: 0x0400365E RID: 13918
	public OVRManager ovrManager;

	// Token: 0x0400365F RID: 13919
	private string path = "Assets/csc.rsp";

	// Token: 0x04003660 RID: 13920
	public BuildTargetManager.BuildTowards currentBuildTargetDONOTCHANGE;

	// Token: 0x04003661 RID: 13921
	public GorillaTagger gorillaTagger;

	// Token: 0x04003662 RID: 13922
	public GameObject[] betaDisableObjects;

	// Token: 0x04003663 RID: 13923
	public GameObject[] betaEnableObjects;

	// Token: 0x04003664 RID: 13924
	public BuildTargetManager.NetworkBackend networkBackend;

	// Token: 0x0200081B RID: 2075
	public enum BuildTowards
	{
		// Token: 0x04003666 RID: 13926
		Steam,
		// Token: 0x04003667 RID: 13927
		OculusPC,
		// Token: 0x04003668 RID: 13928
		Quest,
		// Token: 0x04003669 RID: 13929
		Viveport
	}

	// Token: 0x0200081C RID: 2076
	public enum NetworkBackend
	{
		// Token: 0x0400366B RID: 13931
		Pun,
		// Token: 0x0400366C RID: 13932
		Fusion
	}
}
