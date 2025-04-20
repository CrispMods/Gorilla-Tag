using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000878 RID: 2168
internal class NetworkVector3
{
	// Token: 0x1700056F RID: 1391
	// (get) Token: 0x060034C4 RID: 13508 RVA: 0x00052C31 File Offset: 0x00050E31
	public Vector3 CurrentSyncTarget
	{
		get
		{
			return this._currentSyncTarget;
		}
	}

	// Token: 0x060034C5 RID: 13509 RVA: 0x0013F4B8 File Offset: 0x0013D6B8
	public void SetNewSyncTarget(Vector3 newTarget)
	{
		Vector3 currentSyncTarget = this.CurrentSyncTarget;
		ref currentSyncTarget.SetValueSafe(newTarget);
		this.distanceTraveled = currentSyncTarget - this._currentSyncTarget;
		this._currentSyncTarget = currentSyncTarget;
		this.lastSetNetTime = PhotonNetwork.Time;
	}

	// Token: 0x060034C6 RID: 13510 RVA: 0x0013F4FC File Offset: 0x0013D6FC
	public Vector3 GetPredictedFuture()
	{
		float d = (float)(PhotonNetwork.Time - this.lastSetNetTime) * (float)PhotonNetwork.SerializationRate;
		Vector3 b = this.distanceTraveled * d;
		return this._currentSyncTarget + b;
	}

	// Token: 0x060034C7 RID: 13511 RVA: 0x00052C39 File Offset: 0x00050E39
	public void Reset()
	{
		this._currentSyncTarget = Vector3.zero;
		this.distanceTraveled = Vector3.zero;
		this.lastSetNetTime = 0.0;
	}

	// Token: 0x040037CC RID: 14284
	private double lastSetNetTime;

	// Token: 0x040037CD RID: 14285
	private Vector3 _currentSyncTarget = Vector3.zero;

	// Token: 0x040037CE RID: 14286
	private Vector3 distanceTraveled = Vector3.zero;
}
