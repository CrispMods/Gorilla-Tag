using System;
using Fusion;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000283 RID: 643
[RequireComponent(typeof(PhotonView))]
public class NetworkSceneObject : SimulationBehaviour
{
	// Token: 0x17000196 RID: 406
	// (get) Token: 0x06000F2F RID: 3887 RVA: 0x0004C498 File Offset: 0x0004A698
	public bool IsMine
	{
		get
		{
			return this.photonView.IsMine;
		}
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x0004C4A5 File Offset: 0x0004A6A5
	protected virtual void Start()
	{
		if (this.photonView == null)
		{
			this.photonView = base.GetComponent<PhotonView>();
		}
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x0004C4C1 File Offset: 0x0004A6C1
	protected virtual void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
	}

	// Token: 0x06000F32 RID: 3890 RVA: 0x0004C4C9 File Offset: 0x0004A6C9
	protected virtual void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x0004C4D4 File Offset: 0x0004A6D4
	private void RegisterOnRunner()
	{
		NetworkRunner runner = (NetworkSystem.Instance as NetworkSystemFusion).runner;
		if (runner != null && runner.IsRunning)
		{
			runner.AddGlobal(this);
		}
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x0004C50C File Offset: 0x0004A70C
	private void RemoveFromRunner()
	{
		NetworkRunner runner = (NetworkSystem.Instance as NetworkSystemFusion).runner;
		if (runner != null && runner.IsRunning)
		{
			runner.RemoveGlobal(this);
		}
	}

	// Token: 0x040011BF RID: 4543
	public PhotonView photonView;
}
