using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004FE RID: 1278
public class ConditionalTrigger : MonoBehaviour, IRigAware
{
	// Token: 0x17000324 RID: 804
	// (get) Token: 0x06001EFD RID: 7933 RVA: 0x00043E5B File Offset: 0x0004205B
	private int intValue
	{
		get
		{
			return (int)this._tracking;
		}
	}

	// Token: 0x06001EFE RID: 7934 RVA: 0x00043E63 File Offset: 0x00042063
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

	// Token: 0x06001EFF RID: 7935 RVA: 0x00043EA1 File Offset: 0x000420A1
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

	// Token: 0x06001F00 RID: 7936 RVA: 0x00043EDF File Offset: 0x000420DF
	public void SetProximityFrom(Transform from)
	{
		this._from = from;
	}

	// Token: 0x06001F01 RID: 7937 RVA: 0x00043EE8 File Offset: 0x000420E8
	public void SetProxmityTo(Transform to)
	{
		this._to = to;
	}

	// Token: 0x06001F02 RID: 7938 RVA: 0x00043EF1 File Offset: 0x000420F1
	public void TrackedSet(TriggerCondition conditions)
	{
		this._tracking = conditions;
	}

	// Token: 0x06001F03 RID: 7939 RVA: 0x00043EFA File Offset: 0x000420FA
	public void TrackedAdd(TriggerCondition conditions)
	{
		this._tracking |= conditions;
	}

	// Token: 0x06001F04 RID: 7940 RVA: 0x00043F0A File Offset: 0x0004210A
	public void TrackedRemove(TriggerCondition conditions)
	{
		this._tracking &= ~conditions;
	}

	// Token: 0x06001F05 RID: 7941 RVA: 0x00043EF1 File Offset: 0x000420F1
	public void TrackedSet(int conditions)
	{
		this._tracking = (TriggerCondition)conditions;
	}

	// Token: 0x06001F06 RID: 7942 RVA: 0x00043EFA File Offset: 0x000420FA
	public void TrackedAdd(int conditions)
	{
		this._tracking |= (TriggerCondition)conditions;
	}

	// Token: 0x06001F07 RID: 7943 RVA: 0x00043F0A File Offset: 0x0004210A
	public void TrackedRemove(int conditions)
	{
		this._tracking &= (TriggerCondition)(~(TriggerCondition)conditions);
	}

	// Token: 0x06001F08 RID: 7944 RVA: 0x00043F1B File Offset: 0x0004211B
	public void TrackedClear()
	{
		this._tracking = TriggerCondition.None;
	}

	// Token: 0x06001F09 RID: 7945 RVA: 0x00043F24 File Offset: 0x00042124
	private void OnEnable()
	{
		this._timeSince = 0f;
	}

	// Token: 0x06001F0A RID: 7946 RVA: 0x00043F36 File Offset: 0x00042136
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

	// Token: 0x06001F0B RID: 7947 RVA: 0x00043F62 File Offset: 0x00042162
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

	// Token: 0x06001F0C RID: 7948 RVA: 0x000EC3F4 File Offset: 0x000EA5F4
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

	// Token: 0x06001F0D RID: 7949 RVA: 0x00043F88 File Offset: 0x00042188
	private bool IsTracking(TriggerCondition condition)
	{
		return (this._tracking & condition) == condition;
	}

	// Token: 0x06001F0E RID: 7950 RVA: 0x00043F95 File Offset: 0x00042195
	private static void FindRig(out VRRig rig)
	{
		if (PhotonNetwork.InRoom)
		{
			rig = GorillaGameManager.StaticFindRigForPlayer(NetPlayer.Get(PhotonNetwork.LocalPlayer));
			return;
		}
		rig = VRRig.LocalRig;
	}

	// Token: 0x06001F0F RID: 7951 RVA: 0x00043FB7 File Offset: 0x000421B7
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
