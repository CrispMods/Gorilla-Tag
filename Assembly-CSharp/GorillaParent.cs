using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000599 RID: 1433
public class GorillaParent : MonoBehaviour
{
	// Token: 0x0600236F RID: 9071 RVA: 0x00047E72 File Offset: 0x00046072
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

	// Token: 0x06002370 RID: 9072 RVA: 0x00047EAD File Offset: 0x000460AD
	protected void OnDestroy()
	{
		if (GorillaParent.instance == this)
		{
			GorillaParent.hasInstance = false;
			GorillaParent.instance = null;
		}
	}

	// Token: 0x06002371 RID: 9073 RVA: 0x00047ECC File Offset: 0x000460CC
	public void LateUpdate()
	{
		if (RoomSystem.JoinedRoom && GorillaTagger.Instance.myVRRig.IsNull())
		{
			VRRigCache.Instance.InstantiateNetworkObject();
		}
	}

	// Token: 0x06002372 RID: 9074 RVA: 0x00047EF0 File Offset: 0x000460F0
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

	// Token: 0x06002373 RID: 9075 RVA: 0x00047F07 File Offset: 0x00046107
	public static void OnReplicatedClientReady(Action action)
	{
		if (GorillaParent.replicatedClientReady)
		{
			action();
			return;
		}
		GorillaParent.onReplicatedClientReady = (Action)Delegate.Combine(GorillaParent.onReplicatedClientReady, action);
	}

	// Token: 0x040026FA RID: 9978
	public GameObject tagUI;

	// Token: 0x040026FB RID: 9979
	public GameObject playerParent;

	// Token: 0x040026FC RID: 9980
	public GameObject vrrigParent;

	// Token: 0x040026FD RID: 9981
	[OnEnterPlay_SetNull]
	public static volatile GorillaParent instance;

	// Token: 0x040026FE RID: 9982
	[OnEnterPlay_Set(false)]
	public static bool hasInstance;

	// Token: 0x040026FF RID: 9983
	public List<VRRig> vrrigs;

	// Token: 0x04002700 RID: 9984
	public Dictionary<NetPlayer, VRRig> vrrigDict = new Dictionary<NetPlayer, VRRig>();

	// Token: 0x04002701 RID: 9985
	private int i;

	// Token: 0x04002702 RID: 9986
	private static bool replicatedClientReady;

	// Token: 0x04002703 RID: 9987
	private static Action onReplicatedClientReady;
}
