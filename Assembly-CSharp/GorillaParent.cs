using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

// Token: 0x0200058B RID: 1419
public class GorillaParent : MonoBehaviour
{
	// Token: 0x0600230F RID: 8975 RVA: 0x000ADD84 File Offset: 0x000ABF84
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
			Object.Destroy(base.gameObject);
			return;
		}
	}

	// Token: 0x06002310 RID: 8976 RVA: 0x000ADDBF File Offset: 0x000ABFBF
	protected void OnDestroy()
	{
		if (GorillaParent.instance == this)
		{
			GorillaParent.hasInstance = false;
			GorillaParent.instance = null;
		}
	}

	// Token: 0x06002311 RID: 8977 RVA: 0x000ADDDE File Offset: 0x000ABFDE
	public void LateUpdate()
	{
		if (RoomSystem.JoinedRoom && GorillaTagger.Instance.myVRRig.IsNull())
		{
			VRRigCache.Instance.InstantiateNetworkObject();
		}
	}

	// Token: 0x06002312 RID: 8978 RVA: 0x000ADE02 File Offset: 0x000AC002
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

	// Token: 0x06002313 RID: 8979 RVA: 0x000ADE19 File Offset: 0x000AC019
	public static void OnReplicatedClientReady(Action action)
	{
		if (GorillaParent.replicatedClientReady)
		{
			action();
			return;
		}
		GorillaParent.onReplicatedClientReady = (Action)Delegate.Combine(GorillaParent.onReplicatedClientReady, action);
	}

	// Token: 0x0400269F RID: 9887
	public GameObject tagUI;

	// Token: 0x040026A0 RID: 9888
	public GameObject playerParent;

	// Token: 0x040026A1 RID: 9889
	public GameObject vrrigParent;

	// Token: 0x040026A2 RID: 9890
	[OnEnterPlay_SetNull]
	public static volatile GorillaParent instance;

	// Token: 0x040026A3 RID: 9891
	[OnEnterPlay_Set(false)]
	public static bool hasInstance;

	// Token: 0x040026A4 RID: 9892
	public List<VRRig> vrrigs;

	// Token: 0x040026A5 RID: 9893
	public Dictionary<NetPlayer, VRRig> vrrigDict = new Dictionary<NetPlayer, VRRig>();

	// Token: 0x040026A6 RID: 9894
	private int i;

	// Token: 0x040026A7 RID: 9895
	private static bool replicatedClientReady;

	// Token: 0x040026A8 RID: 9896
	private static Action onReplicatedClientReady;
}
