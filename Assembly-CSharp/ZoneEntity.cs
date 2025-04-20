using System;
using System.Collections;
using System.Collections.Generic;
using GorillaNetworking;
using UnityEngine;

// Token: 0x0200090B RID: 2315
public class ZoneEntity : MonoBehaviour
{
	// Token: 0x170005AB RID: 1451
	// (get) Token: 0x060037A9 RID: 14249 RVA: 0x00054C41 File Offset: 0x00052E41
	public string entityTag
	{
		get
		{
			return this._entityTag;
		}
	}

	// Token: 0x170005AC RID: 1452
	// (get) Token: 0x060037AA RID: 14250 RVA: 0x001492C4 File Offset: 0x001474C4
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

	// Token: 0x170005AD RID: 1453
	// (get) Token: 0x060037AB RID: 14251 RVA: 0x00054C49 File Offset: 0x00052E49
	public VRRig entityRig
	{
		get
		{
			return this._entityRig;
		}
	}

	// Token: 0x170005AE RID: 1454
	// (get) Token: 0x060037AC RID: 14252 RVA: 0x00054C51 File Offset: 0x00052E51
	public SphereCollider collider
	{
		get
		{
			return this._collider;
		}
	}

	// Token: 0x170005AF RID: 1455
	// (get) Token: 0x060037AD RID: 14253 RVA: 0x00054C59 File Offset: 0x00052E59
	public GroupJoinZoneAB GroupZone
	{
		get
		{
			return (this.currentGroupZone & ~this.currentExcludeGroupZone) | this.previousGroupZone;
		}
	}

	// Token: 0x060037AE RID: 14254 RVA: 0x00054C7C File Offset: 0x00052E7C
	protected virtual void OnEnable()
	{
		this.insideBoxes.Clear();
		ZoneGraph.Register(this);
	}

	// Token: 0x060037AF RID: 14255 RVA: 0x00054C8F File Offset: 0x00052E8F
	protected virtual void OnDisable()
	{
		this.insideBoxes.Clear();
		ZoneGraph.Unregister(this);
	}

	// Token: 0x060037B0 RID: 14256 RVA: 0x00054CA2 File Offset: 0x00052EA2
	public void EnableZoneChanges()
	{
		this._collider.enabled = true;
		if (this.disabledZoneChangesOnTriggerStayCoroutine != null)
		{
			base.StopCoroutine(this.disabledZoneChangesOnTriggerStayCoroutine);
			this.disabledZoneChangesOnTriggerStayCoroutine = null;
		}
	}

	// Token: 0x060037B1 RID: 14257 RVA: 0x00054CCB File Offset: 0x00052ECB
	public void DisableZoneChanges()
	{
		this._collider.enabled = false;
		if (this.insideBoxes.Count > 0 && this.disabledZoneChangesOnTriggerStayCoroutine == null)
		{
			this.disabledZoneChangesOnTriggerStayCoroutine = base.StartCoroutine(this.DisabledZoneCollider_OnTriggerStay());
		}
	}

	// Token: 0x060037B2 RID: 14258 RVA: 0x00054D01 File Offset: 0x00052F01
	private IEnumerator DisabledZoneCollider_OnTriggerStay()
	{
		for (;;)
		{
			foreach (BoxCollider c in this.insideBoxes)
			{
				this.OnTriggerStay(c);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060037B3 RID: 14259 RVA: 0x00054D10 File Offset: 0x00052F10
	protected virtual void OnTriggerEnter(Collider c)
	{
		this.OnZoneTrigger(GTZoneEventType.zone_enter, c);
	}

	// Token: 0x060037B4 RID: 14260 RVA: 0x00054D1A File Offset: 0x00052F1A
	protected virtual void OnTriggerExit(Collider c)
	{
		this.OnZoneTrigger(GTZoneEventType.zone_exit, c);
	}

	// Token: 0x060037B5 RID: 14261 RVA: 0x00149308 File Offset: 0x00147508
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

	// Token: 0x060037B6 RID: 14262 RVA: 0x001493D0 File Offset: 0x001475D0
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

	// Token: 0x060037B7 RID: 14263 RVA: 0x00149400 File Offset: 0x00147600
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

	// Token: 0x060037B8 RID: 14264 RVA: 0x00149630 File Offset: 0x00147830
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

	// Token: 0x04003AB3 RID: 15027
	[Space]
	[NonSerialized]
	private int? _entityID;

	// Token: 0x04003AB4 RID: 15028
	[SerializeField]
	private string _entityTag;

	// Token: 0x04003AB5 RID: 15029
	[Space]
	[SerializeField]
	private bool _emitTelemetry = true;

	// Token: 0x04003AB6 RID: 15030
	[SerializeField]
	private int _zoneStayEventInterval = 300;

	// Token: 0x04003AB7 RID: 15031
	[Space]
	[SerializeField]
	private VRRig _entityRig;

	// Token: 0x04003AB8 RID: 15032
	[SerializeField]
	private SphereCollider _collider;

	// Token: 0x04003AB9 RID: 15033
	[SerializeField]
	private Rigidbody _rigidbody;

	// Token: 0x04003ABA RID: 15034
	[Space]
	[NonSerialized]
	public GTZone currentZone = GTZone.none;

	// Token: 0x04003ABB RID: 15035
	[NonSerialized]
	public GTSubZone currentSubZone;

	// Token: 0x04003ABC RID: 15036
	[NonSerialized]
	private GroupJoinZoneAB currentGroupZone = 0;

	// Token: 0x04003ABD RID: 15037
	[NonSerialized]
	private GroupJoinZoneAB previousGroupZone = 0;

	// Token: 0x04003ABE RID: 15038
	[NonSerialized]
	private GroupJoinZoneAB currentExcludeGroupZone = 0;

	// Token: 0x04003ABF RID: 15039
	private HashSet<BoxCollider> insideBoxes = new HashSet<BoxCollider>();

	// Token: 0x04003AC0 RID: 15040
	private int currentZonePriority;

	// Token: 0x04003AC1 RID: 15041
	private float groupZoneClearAtTimestamp;

	// Token: 0x04003AC2 RID: 15042
	private float groupZoneClearInterval = 0.1f;

	// Token: 0x04003AC3 RID: 15043
	private Coroutine disabledZoneChangesOnTriggerStayCoroutine;

	// Token: 0x04003AC4 RID: 15044
	[Space]
	[NonSerialized]
	public ZoneNode currentNode = ZoneNode.Null;

	// Token: 0x04003AC5 RID: 15045
	[NonSerialized]
	public ZoneNode lastEnteredNode = ZoneNode.Null;

	// Token: 0x04003AC6 RID: 15046
	[NonSerialized]
	public ZoneNode lastExitedNode = ZoneNode.Null;

	// Token: 0x04003AC7 RID: 15047
	[Space]
	[NonSerialized]
	private TimeSince sinceZoneEntered = 0;

	// Token: 0x04003AC8 RID: 15048
	private TimeSince gLastStayPoll = 0;
}
