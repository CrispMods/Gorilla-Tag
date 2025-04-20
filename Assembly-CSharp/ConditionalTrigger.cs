using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200050B RID: 1291
public class ConditionalTrigger : MonoBehaviour, IRigAware
{
	// Token: 0x1700032B RID: 811
	// (get) Token: 0x06001F53 RID: 8019 RVA: 0x000451FA File Offset: 0x000433FA
	private int intValue
	{
		get
		{
			return (int)this._tracking;
		}
	}

	// Token: 0x06001F54 RID: 8020 RVA: 0x00045202 File Offset: 0x00043402
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

	// Token: 0x06001F55 RID: 8021 RVA: 0x00045240 File Offset: 0x00043440
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

	// Token: 0x06001F56 RID: 8022 RVA: 0x0004527E File Offset: 0x0004347E
	public void SetProximityFrom(Transform from)
	{
		this._from = from;
	}

	// Token: 0x06001F57 RID: 8023 RVA: 0x00045287 File Offset: 0x00043487
	public void SetProxmityTo(Transform to)
	{
		this._to = to;
	}

	// Token: 0x06001F58 RID: 8024 RVA: 0x00045290 File Offset: 0x00043490
	public void TrackedSet(TriggerCondition conditions)
	{
		this._tracking = conditions;
	}

	// Token: 0x06001F59 RID: 8025 RVA: 0x00045299 File Offset: 0x00043499
	public void TrackedAdd(TriggerCondition conditions)
	{
		this._tracking |= conditions;
	}

	// Token: 0x06001F5A RID: 8026 RVA: 0x000452A9 File Offset: 0x000434A9
	public void TrackedRemove(TriggerCondition conditions)
	{
		this._tracking &= ~conditions;
	}

	// Token: 0x06001F5B RID: 8027 RVA: 0x00045290 File Offset: 0x00043490
	public void TrackedSet(int conditions)
	{
		this._tracking = (TriggerCondition)conditions;
	}

	// Token: 0x06001F5C RID: 8028 RVA: 0x00045299 File Offset: 0x00043499
	public void TrackedAdd(int conditions)
	{
		this._tracking |= (TriggerCondition)conditions;
	}

	// Token: 0x06001F5D RID: 8029 RVA: 0x000452A9 File Offset: 0x000434A9
	public void TrackedRemove(int conditions)
	{
		this._tracking &= (TriggerCondition)(~(TriggerCondition)conditions);
	}

	// Token: 0x06001F5E RID: 8030 RVA: 0x000452BA File Offset: 0x000434BA
	public void TrackedClear()
	{
		this._tracking = TriggerCondition.None;
	}

	// Token: 0x06001F5F RID: 8031 RVA: 0x000452C3 File Offset: 0x000434C3
	private void OnEnable()
	{
		this._timeSince = 0f;
	}

	// Token: 0x06001F60 RID: 8032 RVA: 0x000452D5 File Offset: 0x000434D5
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

	// Token: 0x06001F61 RID: 8033 RVA: 0x00045301 File Offset: 0x00043501
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

	// Token: 0x06001F62 RID: 8034 RVA: 0x000EF130 File Offset: 0x000ED330
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

	// Token: 0x06001F63 RID: 8035 RVA: 0x00045327 File Offset: 0x00043527
	private bool IsTracking(TriggerCondition condition)
	{
		return (this._tracking & condition) == condition;
	}

	// Token: 0x06001F64 RID: 8036 RVA: 0x00045334 File Offset: 0x00043534
	private static void FindRig(out VRRig rig)
	{
		if (PhotonNetwork.InRoom)
		{
			rig = GorillaGameManager.StaticFindRigForPlayer(NetPlayer.Get(PhotonNetwork.LocalPlayer));
			return;
		}
		rig = VRRig.LocalRig;
	}

	// Token: 0x06001F65 RID: 8037 RVA: 0x00045356 File Offset: 0x00043556
	public void SetRig(VRRig rig)
	{
		this._rig = rig;
	}

	// Token: 0x04002310 RID: 8976
	[Space]
	[SerializeField]
	private TriggerCondition _tracking;

	// Token: 0x04002311 RID: 8977
	[Space]
	[SerializeField]
	private Transform _from;

	// Token: 0x04002312 RID: 8978
	[SerializeField]
	private Transform _to;

	// Token: 0x04002313 RID: 8979
	[SerializeField]
	private float _maxDistance;

	// Token: 0x04002314 RID: 8980
	[NonSerialized]
	private float _distance;

	// Token: 0x04002315 RID: 8981
	[Space]
	public UnityEvent onMaxDistance;

	// Token: 0x04002316 RID: 8982
	[SerializeField]
	private float _interval = 1f;

	// Token: 0x04002317 RID: 8983
	[NonSerialized]
	private TimeSince _timeSince;

	// Token: 0x04002318 RID: 8984
	[Space]
	public UnityEvent onTimeElapsed;

	// Token: 0x04002319 RID: 8985
	[Space]
	private VRRig _rig;
}
