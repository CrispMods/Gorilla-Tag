using System;
using Fusion;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000283 RID: 643
[RequireComponent(typeof(PhotonView))]
public class NetworkSceneObject : SimulationBehaviour
{
	// Token: 0x17000196 RID: 406
	// (get) Token: 0x06000F2D RID: 3885 RVA: 0x0004C154 File Offset: 0x0004A354
	public bool IsMine
	{
		get
		{
			return this.photonView.IsMine;
		}
	}

	// Token: 0x06000F2E RID: 3886 RVA: 0x0004C161 File Offset: 0x0004A361
	protected virtual void Start()
	{
		if (this.photonView == null)
		{
			this.photonView = base.GetComponent<PhotonView>();
		}
	}

	// Token: 0x06000F2F RID: 3887 RVA: 0x0004C17D File Offset: 0x0004A37D
	protected virtual void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x0004C185 File Offset: 0x0004A385
	protected virtual void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x0004C190 File Offset: 0x0004A390
	private void RegisterOnRunner()
	{
		NetworkRunner runner = (NetworkSystem.Instance as NetworkSystemFusion).runner;
		if (runner != null && runner.IsRunning)
		{
			runner.AddGlobal(this);
		}
	}

	// Token: 0x06000F32 RID: 3890 RVA: 0x0004C1C8 File Offset: 0x0004A3C8
	private void RemoveFromRunner()
	{
		NetworkRunner runner = (NetworkSystem.Instance as NetworkSystemFusion).runner;
		if (runner != null && runner.IsRunning)
		{
			runner.RemoveGlobal(this);
		}
	}

	// Token: 0x040011BE RID: 4542
	public PhotonView photonView;
}
