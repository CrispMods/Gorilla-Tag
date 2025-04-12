using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

// Token: 0x0200058C RID: 1420
public class GorillaParent : MonoBehaviour
{
	// Token: 0x06002317 RID: 8983 RVA: 0x00046A74 File Offset: 0x00044C74
	public void Awake()
	{
		if (GorillaParent.instance == null)
		{
			GorillaParent.instance = this;
			GorillaParent.hasInstance = true;
			return;
		}
		if (GorillaParent.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
	}

	// Token: 0x06002318 RID: 8984 RVA: 0x00046AAF File Offset: 0x00044CAF
	protected void OnDestroy()
	{
		if (GorillaParent.instance == this)
		{
			GorillaParent.hasInstance = false;
			GorillaParent.instance = null;
		}
	}

	// Token: 0x06002319 RID: 8985 RVA: 0x00046ACE File Offset: 0x00044CCE
	public void LateUpdate()
	{
		if (RoomSystem.JoinedRoom && GorillaTagger.Instance.myVRRig.IsNull())
		{
			Debug.Log("noi");
		}
	}

	// Token: 0x0600231A RID: 8986 RVA: 0x00046AF2 File Offset: 0x00044CF2
	public static void ReplicatedClientReady()
	{
		GorillaParent.replicatedClientReady = true;
		Action action = GorillaParent.onReplicatedClientReady;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x0600231B RID: 8987 RVA: 0x00046B09 File Offset: 0x00044D09
	public static void OnReplicatedClientReady(Action action)
	{
		if (GorillaParent.replicatedClientReady)
		{
			action();
			return;
		}
		GorillaParent.onReplicatedClientReady = (Action)Delegate.Combine(GorillaParent.onReplicatedClientReady, action);
	}

	// Token: 0x040026A5 RID: 9893
	public GameObject tagUI;

	// Token: 0x040026A6 RID: 9894
	public GameObject playerParent;

	// Token: 0x040026A7 RID: 9895
	public GameObject vrrigParent;

	// Token: 0x040026A8 RID: 9896
	[OnEnterPlay_SetNull]
	public static volatile GorillaParent instance;

	// Token: 0x040026A9 RID: 9897
	[OnEnterPlay_Set(false)]
	public static bool hasInstance;

	// Token: 0x040026AA RID: 9898
	public List<VRRig> vrrigs;

	// Token: 0x040026AB RID: 9899
	public Dictionary<NetPlayer, VRRig> vrrigDict = new Dictionary<NetPlayer, VRRig>();

	// Token: 0x040026AC RID: 9900
	private int i;

	// Token: 0x040026AD RID: 9901
	private static bool replicatedClientReady;

	// Token: 0x040026AE RID: 9902
	private static Action onReplicatedClientReady;
}
