using System;
using UnityEngine;

// Token: 0x02000817 RID: 2071
public class BuildTargetManager : MonoBehaviour
{
	// Token: 0x060032E6 RID: 13030 RVA: 0x000F3E5B File Offset: 0x000F205B
	public string GetPath()
	{
		return this.path;
	}

	// Token: 0x04003646 RID: 13894
	public BuildTargetManager.BuildTowards newBuildTarget;

	// Token: 0x04003647 RID: 13895
	public bool isBeta;

	// Token: 0x04003648 RID: 13896
	public bool isQA;

	// Token: 0x04003649 RID: 13897
	public bool spoofIDs;

	// Token: 0x0400364A RID: 13898
	public bool spoofChild;

	// Token: 0x0400364B RID: 13899
	public bool enableAllCosmetics;

	// Token: 0x0400364C RID: 13900
	public OVRManager ovrManager;

	// Token: 0x0400364D RID: 13901
	private string path = "Assets/csc.rsp";

	// Token: 0x0400364E RID: 13902
	public BuildTargetManager.BuildTowards currentBuildTargetDONOTCHANGE;

	// Token: 0x0400364F RID: 13903
	public GorillaTagger gorillaTagger;

	// Token: 0x04003650 RID: 13904
	public GameObject[] betaDisableObjects;

	// Token: 0x04003651 RID: 13905
	public GameObject[] betaEnableObjects;

	// Token: 0x04003652 RID: 13906
	public BuildTargetManager.NetworkBackend networkBackend;

	// Token: 0x02000818 RID: 2072
	public enum BuildTowards
	{
		// Token: 0x04003654 RID: 13908
		Steam,
		// Token: 0x04003655 RID: 13909
		OculusPC,
		// Token: 0x04003656 RID: 13910
		Quest,
		// Token: 0x04003657 RID: 13911
		Viveport
	}

	// Token: 0x02000819 RID: 2073
	public enum NetworkBackend
	{
		// Token: 0x04003659 RID: 13913
		Pun,
		// Token: 0x0400365A RID: 13914
		Fusion
	}
}
