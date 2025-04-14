using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200085C RID: 2140
internal class NetworkVector3
{
	// Token: 0x1700055E RID: 1374
	// (get) Token: 0x060033F8 RID: 13304 RVA: 0x000F80EA File Offset: 0x000F62EA
	public Vector3 CurrentSyncTarget
	{
		get
		{
			return this._currentSyncTarget;
		}
	}

	// Token: 0x060033F9 RID: 13305 RVA: 0x000F80F4 File Offset: 0x000F62F4
	public void SetNewSyncTarget(Vector3 newTarget)
	{
		Vector3 currentSyncTarget = this.CurrentSyncTarget;
		ref currentSyncTarget.SetValueSafe(newTarget);
		this.distanceTraveled = currentSyncTarget - this._currentSyncTarget;
		this._currentSyncTarget = currentSyncTarget;
		this.lastSetNetTime = PhotonNetwork.Time;
	}

	// Token: 0x060033FA RID: 13306 RVA: 0x000F8138 File Offset: 0x000F6338
	public Vector3 GetPredictedFuture()
	{
		float d = (float)(PhotonNetwork.Time - this.lastSetNetTime) * (float)PhotonNetwork.SerializationRate;
		Vector3 b = this.distanceTraveled * d;
		return this._currentSyncTarget + b;
	}

	// Token: 0x060033FB RID: 13307 RVA: 0x000F8173 File Offset: 0x000F6373
	public void Reset()
	{
		this._currentSyncTarget = Vector3.zero;
		this.distanceTraveled = Vector3.zero;
		this.lastSetNetTime = 0.0;
	}

	// Token: 0x0400370C RID: 14092
	private double lastSetNetTime;

	// Token: 0x0400370D RID: 14093
	private Vector3 _currentSyncTarget = Vector3.zero;

	// Token: 0x0400370E RID: 14094
	private Vector3 distanceTraveled = Vector3.zero;
}
