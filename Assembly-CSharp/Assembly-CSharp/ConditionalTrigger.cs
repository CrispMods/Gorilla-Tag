using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004FE RID: 1278
public class ConditionalTrigger : MonoBehaviour, IRigAware
{
	// Token: 0x17000324 RID: 804
	// (get) Token: 0x06001EFD RID: 7933 RVA: 0x0009D14C File Offset: 0x0009B34C
	private int intValue
	{
		get
		{
			return (int)this._tracking;
		}
	}

	// Token: 0x06001EFE RID: 7934 RVA: 0x0009D154 File Offset: 0x0009B354
	public void SetProximityFromRig()
	{
		if (this._rig.AsNull<VRRig>() == null)
		{
			ConditionalTrigger.FindRig(out this._rig);
		}
		if (this._rig)
		{
			this._from = this._rig.transform;
		}
	}

	// Token: 0x06001EFF RID: 7935 RVA: 0x0009D192 File Offset: 0x0009B392
	public void SetProximityToRig()
	{
		if (this._rig.AsNull<VRRig>() == null)
		{
			ConditionalTrigger.FindRig(out this._rig);
		}
		if (this._rig)
		{
			this._to = this._rig.transform;
		}
	}

	// Token: 0x06001F00 RID: 7936 RVA: 0x0009D1D0 File Offset: 0x0009B3D0
	public void SetProximityFrom(Transform from)
	{
		this._from = from;
	}

	// Token: 0x06001F01 RID: 7937 RVA: 0x0009D1D9 File Offset: 0x0009B3D9
	public void SetProxmityTo(Transform to)
	{
		this._to = to;
	}

	// Token: 0x06001F02 RID: 7938 RVA: 0x0009D1E2 File Offset: 0x0009B3E2
	public void TrackedSet(TriggerCondition conditions)
	{
		this._tracking = conditions;
	}

	// Token: 0x06001F03 RID: 7939 RVA: 0x0009D1EB File Offset: 0x0009B3EB
	public void TrackedAdd(TriggerCondition conditions)
	{
		this._tracking |= conditions;
	}

	// Token: 0x06001F04 RID: 7940 RVA: 0x0009D1FB File Offset: 0x0009B3FB
	public void TrackedRemove(TriggerCondition conditions)
	{
		this._tracking &= ~conditions;
	}

	// Token: 0x06001F05 RID: 7941 RVA: 0x0009D1E2 File Offset: 0x0009B3E2
	public void TrackedSet(int conditions)
	{
		this._tracking = (TriggerCondition)conditions;
	}

	// Token: 0x06001F06 RID: 7942 RVA: 0x0009D1EB File Offset: 0x0009B3EB
	public void TrackedAdd(int conditions)
	{
		this._tracking |= (TriggerCondition)conditions;
	}

	// Token: 0x06001F07 RID: 7943 RVA: 0x0009D1FB File Offset: 0x0009B3FB
	public void TrackedRemove(int conditions)
	{
		this._tracking &= (TriggerCondition)(~(TriggerCondition)conditions);
	}

	// Token: 0x06001F08 RID: 7944 RVA: 0x0009D20C File Offset: 0x0009B40C
	public void TrackedClear()
	{
		this._tracking = TriggerCondition.None;
	}

	// Token: 0x06001F09 RID: 7945 RVA: 0x0009D215 File Offset: 0x0009B415
	private void OnEnable()
	{
		this._timeSince = 0f;
	}

	// Token: 0x06001F0A RID: 7946 RVA: 0x0009D227 File Offset: 0x0009B427
	private void Update()
	{
		if (this.IsTracking(TriggerCondition.TimeElapsed))
		{
			this.TrackTimeElapsed();
		}
		if (this.IsTracking(TriggerCondition.Proximity))
		{
			this.TrackProximity();
			return;
		}
		this._distance = 0f;
	}

	// Token: 0x06001F0B RID: 7947 RVA: 0x0009D253 File Offset: 0x0009B453
	private void TrackTimeElapsed()
	{
		if (this._timeSince.HasElapsed(this._interval, true))
		{
			UnityEvent unityEvent = this.onTimeElapsed;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}
	}

	// Token: 0x06001F0C RID: 7948 RVA: 0x0009D27C File Offset: 0x0009B47C
	private void TrackProximity()
	{
		if (!this._from || !this._to)
		{
			this._distance = 0f;
			return;
		}
		this._distance = Vector3.Distance(this._to.position, this._from.position);
		if (this._distance >= this._maxDistance)
		{
			UnityEvent unityEvent = this.onMaxDistance;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}
	}

	// Token: 0x06001F0D RID: 7949 RVA: 0x0009D2EE File Offset: 0x0009B4EE
	private bool IsTracking(TriggerCondition condition)
	{
		return (this._tracking & condition) == condition;
	}

	// Token: 0x06001F0E RID: 7950 RVA: 0x0009D2FB File Offset: 0x0009B4FB
	private static void FindRig(out VRRig rig)
	{
		if (PhotonNetwork.InRoom)
		{
			rig = GorillaGameManager.StaticFindRigForPlayer(NetPlayer.Get(PhotonNetwork.LocalPlayer));
			return;
		}
		rig = VRRig.LocalRig;
	}

	// Token: 0x06001F0F RID: 7951 RVA: 0x0009D31D File Offset: 0x0009B51D
	public void SetRig(VRRig rig)
	{
		this._rig = rig;
	}

	// Token: 0x040022BE RID: 8894
	[Space]
	[SerializeField]
	private TriggerCondition _tracking;

	// Token: 0x040022BF RID: 8895
	[Space]
	[SerializeField]
	private Transform _from;

	// Token: 0x040022C0 RID: 8896
	[SerializeField]
	private Transform _to;

	// Token: 0x040022C1 RID: 8897
	[SerializeField]
	private float _maxDistance;

	// Token: 0x040022C2 RID: 8898
	[NonSerialized]
	private float _distance;

	// Token: 0x040022C3 RID: 8899
	[Space]
	public UnityEvent onMaxDistance;

	// Token: 0x040022C4 RID: 8900
	[SerializeField]
	private float _interval = 1f;

	// Token: 0x040022C5 RID: 8901
	[NonSerialized]
	private TimeSince _timeSince;

	// Token: 0x040022C6 RID: 8902
	[Space]
	public UnityEvent onTimeElapsed;

	// Token: 0x040022C7 RID: 8903
	[Space]
	private VRRig _rig;
}
