using System;
using Fusion;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000283 RID: 643
[RequireComponent(typeof(PhotonView))]
public class NetworkSceneObject : SimulationBehaviour
{
	// Token: 0x17000196 RID: 406
	// (get) Token: 0x06000F2F RID: 3887 RVA: 0x00039ACA File Offset: 0x00037CCA
	public bool IsMine
	{
		get
		{
			return this.photonView.IsMine;
		}
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x00039AD7 File Offset: 0x00037CD7
	protected virtual void Start()
	{
		if (this.photonView == null)
		{
			this.photonView = base.GetComponent<PhotonView>();
		}
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x00039AF3 File Offset: 0x00037CF3
	protected virtual void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
	}

	// Token: 0x06000F32 RID: 3890 RVA: 0x00039AFB File Offset: 0x00037CFB
	protected virtual void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x000A5C90 File Offset: 0x000A3E90
	private void RegisterOnRunner()
	{
		NetworkRunner runner = (NetworkSystem.Instance as NetworkSystemFusion).runner;
		if (runner != null && runner.IsRunning)
		{
			runner.AddGlobal(this);
		}
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x000A5CC8 File Offset: 0x000A3EC8
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
