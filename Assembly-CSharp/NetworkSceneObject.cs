using System;
using Fusion;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200028E RID: 654
[RequireComponent(typeof(PhotonView))]
public class NetworkSceneObject : SimulationBehaviour
{
	// Token: 0x1700019D RID: 413
	// (get) Token: 0x06000F78 RID: 3960 RVA: 0x0003AD8A File Offset: 0x00038F8A
	public bool IsMine
	{
		get
		{
			return this.photonView.IsMine;
		}
	}

	// Token: 0x06000F79 RID: 3961 RVA: 0x0003AD97 File Offset: 0x00038F97
	protected virtual void Start()
	{
		if (this.photonView == null)
		{
			this.photonView = base.GetComponent<PhotonView>();
		}
	}

	// Token: 0x06000F7A RID: 3962 RVA: 0x0003ADB3 File Offset: 0x00038FB3
	protected virtual void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
	}

	// Token: 0x06000F7B RID: 3963 RVA: 0x0003ADBB File Offset: 0x00038FBB
	protected virtual void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
	}

	// Token: 0x06000F7C RID: 3964 RVA: 0x000A851C File Offset: 0x000A671C
	private void RegisterOnRunner()
	{
		NetworkRunner runner = (NetworkSystem.Instance as NetworkSystemFusion).runner;
		if (runner != null && runner.IsRunning)
		{
			runner.AddGlobal(this);
		}
	}

	// Token: 0x06000F7D RID: 3965 RVA: 0x000A8554 File Offset: 0x000A6754
	private void RemoveFromRunner()
	{
		NetworkRunner runner = (NetworkSystem.Instance as NetworkSystemFusion).runner;
		if (runner != null && runner.IsRunning)
		{
			runner.RemoveGlobal(this);
		}
	}

	// Token: 0x04001206 RID: 4614
	public PhotonView photonView;
}
