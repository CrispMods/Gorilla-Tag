using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200085F RID: 2143
internal class NetworkVector3
{
	// Token: 0x1700055F RID: 1375
	// (get) Token: 0x06003404 RID: 13316 RVA: 0x00051724 File Offset: 0x0004F924
	public Vector3 CurrentSyncTarget
	{
		get
		{
			return this._currentSyncTarget;
		}
	}

	// Token: 0x06003405 RID: 13317 RVA: 0x00139ED0 File Offset: 0x001380D0
	public void SetNewSyncTarget(Vector3 newTarget)
	{
		Vector3 currentSyncTarget = this.CurrentSyncTarget;
		ref currentSyncTarget.SetValueSafe(newTarget);
		this.distanceTraveled = currentSyncTarget - this._currentSyncTarget;
		this._currentSyncTarget = currentSyncTarget;
		this.lastSetNetTime = PhotonNetwork.Time;
	}

	// Token: 0x06003406 RID: 13318 RVA: 0x00139F14 File Offset: 0x00138114
	public Vector3 GetPredictedFuture()
	{
		float d = (float)(PhotonNetwork.Time - this.lastSetNetTime) * (float)PhotonNetwork.SerializationRate;
		Vector3 b = this.distanceTraveled * d;
		return this._currentSyncTarget + b;
	}

	// Token: 0x06003407 RID: 13319 RVA: 0x0005172C File Offset: 0x0004F92C
	public void Reset()
	{
		this._currentSyncTarget = Vector3.zero;
		this.distanceTraveled = Vector3.zero;
		this.lastSetNetTime = 0.0;
	}

	// Token: 0x0400371E RID: 14110
	private double lastSetNetTime;

	// Token: 0x0400371F RID: 14111
	private Vector3 _currentSyncTarget = Vector3.zero;

	// Token: 0x04003720 RID: 14112
	private Vector3 distanceTraveled = Vector3.zero;
}
