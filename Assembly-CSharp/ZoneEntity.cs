using System;
using System.Collections.Generic;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020008EF RID: 2287
public class ZoneEntity : MonoBehaviour
{
	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x060036E1 RID: 14049 RVA: 0x001041E4 File Offset: 0x001023E4
	public string entityTag
	{
		get
		{
			return this._entityTag;
		}
	}

	// Token: 0x1700059B RID: 1435
	// (get) Token: 0x060036E2 RID: 14050 RVA: 0x001041EC File Offset: 0x001023EC
	public int entityID
	{
		get
		{
			int value = this._entityID.GetValueOrDefault();
			if (this._entityID == null)
			{
				value = base.GetInstanceID();
				this._entityID = new int?(value);
			}
			return this._entityID.Value;
		}
	}

	// Token: 0x1700059C RID: 1436
	// (get) Token: 0x060036E3 RID: 14051 RVA: 0x00104230 File Offset: 0x00102430
	public VRRig entityRig
	{
		get
		{
			return this._entityRig;
		}
	}

	// Token: 0x1700059D RID: 1437
	// (get) Token: 0x060036E4 RID: 14052 RVA: 0x00104238 File Offset: 0x00102438
	public SphereCollider collider
	{
		get
		{
			return this._collider;
		}
	}

	// Token: 0x1700059E RID: 1438
	// (get) Token: 0x060036E5 RID: 14053 RVA: 0x00104240 File Offset: 0x00102440
	public GroupJoinZoneAB GroupZone
	{
		get
		{
			return (this.currentGroupZone & ~this.currentExcludeGroupZone) | this.previousGroupZone;
		}
	}

	// Token: 0x060036E6 RID: 14054 RVA: 0x00104263 File Offset: 0x00102463
	protected virtual void OnEnable()
	{
		this.insideBoxes.Clear();
		ZoneGraph.Register(this);
	}

	// Token: 0x060036E7 RID: 14055 RVA: 0x00104276 File Offset: 0x00102476
	protected virtual void OnDisable()
	{
		this.insideBoxes.Clear();
		ZoneGraph.Unregister(this);
	}

	// Token: 0x060036E8 RID: 14056 RVA: 0x00104289 File Offset: 0x00102489
	protected virtual void OnTriggerEnter(Collider c)
	{
		this.OnZoneTrigger(GTZoneEventType.zone_enter, c);
	}

	// Token: 0x060036E9 RID: 14057 RVA: 0x00104293 File Offset: 0x00102493
	protected virtual void OnTriggerExit(Collider c)
	{
		this.OnZoneTrigger(GTZoneEventType.zone_exit, c);
	}

	// Token: 0x060036EA RID: 14058 RVA: 0x001042A0 File Offset: 0x001024A0
	protected virtual void OnTriggerStay(Collider c)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		BoxCollider boxCollider = c as BoxCollider;
		if (boxCollider == null)
		{
			return;
		}
		ZoneDef zoneDef = ZoneGraph.ColliderToZoneDef(boxCollider);
		if (Time.time >= this.groupZoneClearAtTimestamp)
		{
			this.previousGroupZone = (this.currentGroupZone & ~this.currentExcludeGroupZone);
			this.currentGroupZone = zoneDef.groupZoneAB;
			this.currentExcludeGroupZone = zoneDef.excludeGroupZoneAB;
			this.groupZoneClearAtTimestamp = Time.time + this.groupZoneClearInterval;
		}
		else
		{
			this.currentGroupZone |= zoneDef.groupZoneAB;
			this.currentExcludeGroupZone |= zoneDef.excludeGroupZoneAB;
		}
		if (!this.gLastStayPoll.HasElapsed(1f, true))
		{
			return;
		}
		this.OnZoneTrigger(GTZoneEventType.zone_stay, boxCollider);
	}

	// Token: 0x060036EB RID: 14059 RVA: 0x00104368 File Offset: 0x00102568
	protected virtual void OnZoneTrigger(GTZoneEventType zoneEvent, Collider c)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		BoxCollider boxCollider = c as BoxCollider;
		if (boxCollider == null)
		{
			return;
		}
		ZoneDef zone = ZoneGraph.ColliderToZoneDef(boxCollider);
		this.OnZoneTrigger(zoneEvent, zone, boxCollider);
	}

	// Token: 0x060036EC RID: 14060 RVA: 0x00104398 File Offset: 0x00102598
	private void OnZoneTrigger(GTZoneEventType zoneEvent, ZoneDef zone, BoxCollider box)
	{
		bool flag = false;
		switch (zoneEvent)
		{
		case GTZoneEventType.zone_enter:
		{
			if (zone.zoneId != this.lastEnteredNode.zoneId)
			{
				this.sinceZoneEntered = 0;
			}
			this.lastEnteredNode = ZoneGraph.ColliderToNode(box);
			ZoneDef zoneDef = ZoneGraph.ColliderToZoneDef(box);
			this.insideBoxes.Add(box);
			if (zoneDef.priority > this.currentZonePriority)
			{
				this.currentZone = zone.zoneId;
				this.currentSubZone = zone.subZoneId;
				this.currentZonePriority = zoneDef.priority;
			}
			if (zone.subZoneId == GTSubZone.store_register)
			{
				GorillaTelemetry.PostShopEvent(this._entityRig, GTShopEventType.register_visit, CosmeticsController.instance.currentCart);
			}
			flag = zone.trackEnter;
			break;
		}
		case GTZoneEventType.zone_exit:
			this.lastExitedNode = ZoneGraph.ColliderToNode(box);
			this.insideBoxes.Remove(box);
			if (this.currentZone == this.lastExitedNode.zoneId)
			{
				int num = 0;
				ZoneDef zoneDef2 = null;
				foreach (BoxCollider collider in this.insideBoxes)
				{
					ZoneDef zoneDef3 = ZoneGraph.ColliderToZoneDef(collider);
					if (zoneDef3.priority > num)
					{
						zoneDef2 = zoneDef3;
						num = zoneDef3.priority;
					}
				}
				if (zoneDef2 != null)
				{
					this.currentZone = zoneDef2.zoneId;
					this.currentSubZone = zoneDef2.subZoneId;
					this.currentZonePriority = zoneDef2.priority;
				}
				else
				{
					this.currentZone = GTZone.none;
					this.currentSubZone = GTSubZone.none;
					this.currentZonePriority = 0;
				}
			}
			if (this.currentZone == GTZone.forest && this.currentSubZone == GTSubZone.tree_room)
			{
				zone.subZoneId = GTSubZone.none;
			}
			flag = zone.trackExit;
			break;
		case GTZoneEventType.zone_stay:
		{
			bool flag2 = this.sinceZoneEntered.secondsElapsedInt >= this._zoneStayEventInterval;
			if (flag2)
			{
				this.sinceZoneEntered = 0;
			}
			flag = (zone.trackStay && flag2);
			break;
		}
		}
		GorillaTelemetry.CurrentZone = zone.zoneId;
		GorillaTelemetry.CurrentSubZone = zone.subZoneId;
		if (!this._emitTelemetry)
		{
			return;
		}
		if (!flag)
		{
			return;
		}
		if (!this._entityRig.isOfflineVRRig)
		{
			return;
		}
		GorillaTelemetry.PostZoneEvent(zone.zoneId, zone.subZoneId, zoneEvent);
	}

	// Token: 0x060036ED RID: 14061 RVA: 0x001045C8 File Offset: 0x001027C8
	public static int Compare<T>(T x, T y) where T : ZoneEntity
	{
		if (x == null && y == null)
		{
			return 0;
		}
		if (x == null)
		{
			return 1;
		}
		if (y == null)
		{
			return -1;
		}
		return x.entityID.CompareTo(y.entityID);
	}

	// Token: 0x040039F2 RID: 14834
	[Space]
	[NonSerialized]
	private int? _entityID;

	// Token: 0x040039F3 RID: 14835
	[SerializeField]
	private string _entityTag;

	// Token: 0x040039F4 RID: 14836
	[Space]
	[SerializeField]
	private bool _emitTelemetry = true;

	// Token: 0x040039F5 RID: 14837
	[SerializeField]
	private int _zoneStayEventInterval = 300;

	// Token: 0x040039F6 RID: 14838
	[Space]
	[SerializeField]
	private VRRig _entityRig;

	// Token: 0x040039F7 RID: 14839
	[SerializeField]
	private SphereCollider _collider;

	// Token: 0x040039F8 RID: 14840
	[SerializeField]
	private Rigidbody _rigidbody;

	// Token: 0x040039F9 RID: 14841
	[Space]
	[NonSerialized]
	public GTZone currentZone = GTZone.none;

	// Token: 0x040039FA RID: 14842
	[NonSerialized]
	public GTSubZone currentSubZone;

	// Token: 0x040039FB RID: 14843
	[NonSerialized]
	private GroupJoinZoneAB currentGroupZone = 0;

	// Token: 0x040039FC RID: 14844
	[NonSerialized]
	private GroupJoinZoneAB previousGroupZone = 0;

	// Token: 0x040039FD RID: 14845
	[NonSerialized]
	private GroupJoinZoneAB currentExcludeGroupZone = 0;

	// Token: 0x040039FE RID: 14846
	private HashSet<BoxCollider> insideBoxes = new HashSet<BoxCollider>();

	// Token: 0x040039FF RID: 14847
	private int currentZonePriority;

	// Token: 0x04003A00 RID: 14848
	private float groupZoneClearAtTimestamp;

	// Token: 0x04003A01 RID: 14849
	private float groupZoneClearInterval = 0.1f;

	// Token: 0x04003A02 RID: 14850
	[Space]
	[NonSerialized]
	public ZoneNode currentNode = ZoneNode.Null;

	// Token: 0x04003A03 RID: 14851
	[NonSerialized]
	public ZoneNode lastEnteredNode = ZoneNode.Null;

	// Token: 0x04003A04 RID: 14852
	[NonSerialized]
	public ZoneNode lastExitedNode = ZoneNode.Null;

	// Token: 0x04003A05 RID: 14853
	[Space]
	[NonSerialized]
	private TimeSince sinceZoneEntered = 0;

	// Token: 0x04003A06 RID: 14854
	private TimeSince gLastStayPoll = 0;
}
