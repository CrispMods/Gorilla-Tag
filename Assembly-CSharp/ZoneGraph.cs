using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200090D RID: 2317
[DefaultExecutionOrder(5555)]
public class ZoneGraph : MonoBehaviour
{
	// Token: 0x060037C0 RID: 14272 RVA: 0x00054D3B File Offset: 0x00052F3B
	public static ZoneGraph Instance()
	{
		return ZoneGraph.gGraph;
	}

	// Token: 0x060037C1 RID: 14273 RVA: 0x00054D42 File Offset: 0x00052F42
	public static ZoneDef ColliderToZoneDef(BoxCollider collider)
	{
		if (!(collider == null))
		{
			return ZoneGraph.gGraph._colliderToZoneDef[collider];
		}
		return null;
	}

	// Token: 0x060037C2 RID: 14274 RVA: 0x00054D5F File Offset: 0x00052F5F
	public static ZoneNode ColliderToNode(BoxCollider collider)
	{
		if (!(collider == null))
		{
			return ZoneGraph.gGraph._colliderToNode[collider];
		}
		return ZoneNode.Null;
	}

	// Token: 0x060037C3 RID: 14275 RVA: 0x00054D80 File Offset: 0x00052F80
	private void Awake()
	{
		if (ZoneGraph.gGraph != null && ZoneGraph.gGraph != this)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			ZoneGraph.gGraph = this;
		}
		this.CompileColliderMaps(this._zoneDefs);
	}

	// Token: 0x060037C4 RID: 14276 RVA: 0x001497C8 File Offset: 0x001479C8
	private void CompileColliderMaps(ZoneDef[] zones)
	{
		foreach (ZoneDef zoneDef in zones)
		{
			for (int j = 0; j < zoneDef.colliders.Length; j++)
			{
				BoxCollider key = zoneDef.colliders[j];
				this._colliderToZoneDef[key] = zoneDef;
			}
		}
		for (int k = 0; k < this._colliders.Length; k++)
		{
			BoxCollider key2 = this._colliders[k];
			this._colliderToNode[key2] = this._nodes[k];
		}
	}

	// Token: 0x060037C5 RID: 14277 RVA: 0x0014984C File Offset: 0x00147A4C
	public static int Compare(ZoneDef x, ZoneDef y)
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
		int num = (int)x.zoneId;
		int num2 = num.CompareTo((int)y.zoneId);
		if (num2 == 0)
		{
			num = (int)x.subZoneId;
			num2 = num.CompareTo((int)y.subZoneId);
		}
		return num2;
	}

	// Token: 0x060037C6 RID: 14278 RVA: 0x00054DB6 File Offset: 0x00052FB6
	public static void Register(ZoneEntity entity)
	{
		if (ZoneGraph.gGraph == null)
		{
			ZoneGraph.gGraph = UnityEngine.Object.FindFirstObjectByType<ZoneGraph>();
		}
		if (!ZoneGraph.gGraph._entityList.Contains(entity))
		{
			ZoneGraph.gGraph._entityList.Add(entity);
		}
	}

	// Token: 0x060037C7 RID: 14279 RVA: 0x00054DF1 File Offset: 0x00052FF1
	public static void Unregister(ZoneEntity entity)
	{
		ZoneGraph.gGraph._entityList.Remove(entity);
	}

	// Token: 0x04003ACC RID: 15052
	[SerializeField]
	private ZoneDef[] _zoneDefs = new ZoneDef[0];

	// Token: 0x04003ACD RID: 15053
	[SerializeField]
	private BoxCollider[] _colliders = new BoxCollider[0];

	// Token: 0x04003ACE RID: 15054
	[SerializeField]
	private ZoneNode[] _nodes = new ZoneNode[0];

	// Token: 0x04003ACF RID: 15055
	[Space]
	[NonSerialized]
	private Dictionary<BoxCollider, ZoneDef> _colliderToZoneDef = new Dictionary<BoxCollider, ZoneDef>(64);

	// Token: 0x04003AD0 RID: 15056
	[Space]
	[NonSerialized]
	private Dictionary<BoxCollider, ZoneNode> _colliderToNode = new Dictionary<BoxCollider, ZoneNode>(64);

	// Token: 0x04003AD1 RID: 15057
	[Space]
	[NonSerialized]
	private List<ZoneEntity> _entityList = new List<ZoneEntity>(16);

	// Token: 0x04003AD2 RID: 15058
	private static ZoneGraph gGraph;
}
