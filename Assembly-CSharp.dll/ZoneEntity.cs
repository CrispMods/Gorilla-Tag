using System;
using System.Collections.Generic;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020008F2 RID: 2290
public class ZoneEntity : MonoBehaviour
{
	// Token: 0x1700059B RID: 1435
	// (get) Token: 0x060036ED RID: 14061 RVA: 0x00053724 File Offset: 0x00051924
	public string entityTag
	{
		get
		{
			return this._entityTag;
		}
	}

	// Token: 0x1700059C RID: 1436
	// (get) Token: 0x060036EE RID: 14062 RVA: 0x00143D04 File Offset: 0x00141F04
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

	// Token: 0x1700059D RID: 1437
	// (get) Token: 0x060036EF RID: 14063 RVA: 0x0005372C File Offset: 0x0005192C
	public VRRig entityRig
	{
		get
		{
			return this._entityRig;
		}
	}

	// Token: 0x1700059E RID: 1438
	// (get) Token: 0x060036F0 RID: 14064 RVA: 0x00053734 File Offset: 0x00051934
	public SphereCollider collider
	{
		get
		{
			return this._collider;
		}
	}

	// Token: 0x1700059F RID: 1439
	// (get) Token: 0x060036F1 RID: 14065 RVA: 0x0005373C File Offset: 0x0005193C
	public GroupJoinZoneAB GroupZone
	{
		get
		{
			return (this.currentGroupZone & ~this.currentExcludeGroupZone) | this.previousGroupZone;
		}
	}

	// Token: 0x060036F2 RID: 14066 RVA: 0x0005375F File Offset: 0x0005195F
	protected virtual void OnEnable()
	{
		this.insideBoxes.Clear();
		ZoneGraph.Register(this);
	}

	// Token: 0x060036F3 RID: 14067 RVA: 0x00053772 File Offset: 0x00051972
	protected virtual void OnDisable()
	{
		this.insideBoxes.Clear();
		ZoneGraph.Unregister(this);
	}

	// Token: 0x060036F4 RID: 14068 RVA: 0x00053785 File Offset: 0x00051985
	protected virtual void OnTriggerEnter(Collider c)
	{
		this.OnZoneTrigger(GTZoneEventType.zone_enter, c);
	}

	// Token: 0x060036F5 RID: 14069 RVA: 0x0005378F File Offset: 0x0005198F
	protected virtual void OnTriggerExit(Collider c)
	{
		this.OnZoneTrigger(GTZoneEventType.zone_exit, c);
	}

	// Token: 0x060036F6 RID: 14070 RVA: 0x00143D48 File Offset: 0x00141F48
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

	// Token: 0x060036F7 RID: 14071 RVA: 0x00143E10 File Offset: 0x00142010
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

	// Token: 0x060036F8 RID: 14072 RVA: 0x00143E40 File Offset: 0x00142040
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

	// Token: 0x060036F9 RID: 14073 RVA: 0x00144070 File Offset: 0x00142270
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

	// Token: 0x04003A04 RID: 14852
	[Space]
	[NonSerialized]
	private int? _entityID;

	// Token: 0x04003A05 RID: 14853
	[SerializeField]
	private string _entityTag;

	// Token: 0x04003A06 RID: 14854
	[Space]
	[SerializeField]
	private bool _emitTelemetry = true;

	// Token: 0x04003A07 RID: 14855
	[SerializeField]
	private int _zoneStayEventInterval = 300;

	// Token: 0x04003A08 RID: 14856
	[Space]
	[SerializeField]
	private VRRig _entityRig;

	// Token: 0x04003A09 RID: 14857
	[SerializeField]
	private SphereCollider _collider;

	// Token: 0x04003A0A RID: 14858
	[SerializeField]
	private Rigidbody _rigidbody;

	// Token: 0x04003A0B RID: 14859
	[Space]
	[NonSerialized]
	public GTZone currentZone = GTZone.none;

	// Token: 0x04003A0C RID: 14860
	[NonSerialized]
	public GTSubZone currentSubZone;

	// Token: 0x04003A0D RID: 14861
	[NonSerialized]
	private GroupJoinZoneAB currentGroupZone = 0;

	// Token: 0x04003A0E RID: 14862
	[NonSerialized]
	private GroupJoinZoneAB previousGroupZone = 0;

	// Token: 0x04003A0F RID: 14863
	[NonSerialized]
	private GroupJoinZoneAB currentExcludeGroupZone = 0;

	// Token: 0x04003A10 RID: 14864
	private HashSet<BoxCollider> insideBoxes = new HashSet<BoxCollider>();

	// Token: 0x04003A11 RID: 14865
	private int currentZonePriority;

	// Token: 0x04003A12 RID: 14866
	private float groupZoneClearAtTimestamp;

	// Token: 0x04003A13 RID: 14867
	private float groupZoneClearInterval = 0.1f;

	// Token: 0x04003A14 RID: 14868
	[Space]
	[NonSerialized]
	public ZoneNode currentNode = ZoneNode.Null;

	// Token: 0x04003A15 RID: 14869
	[NonSerialized]
	public ZoneNode lastEnteredNode = ZoneNode.Null;

	// Token: 0x04003A16 RID: 14870
	[NonSerialized]
	public ZoneNode lastExitedNode = ZoneNode.Null;

	// Token: 0x04003A17 RID: 14871
	[Space]
	[NonSerialized]
	private TimeSince sinceZoneEntered = 0;

	// Token: 0x04003A18 RID: 14872
	private TimeSince gLastStayPoll = 0;
}
