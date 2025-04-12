using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008F3 RID: 2291
[DefaultExecutionOrder(5555)]
public class ZoneGraph : MonoBehaviour
{
	// Token: 0x060036FB RID: 14075 RVA: 0x00053799 File Offset: 0x00051999
	public static ZoneGraph Instance()
	{
		return ZoneGraph.gGraph;
	}

	// Token: 0x060036FC RID: 14076 RVA: 0x000537A0 File Offset: 0x000519A0
	public static ZoneDef ColliderToZoneDef(BoxCollider collider)
	{
		if (!(collider == null))
		{
			return ZoneGraph.gGraph._colliderToZoneDef[collider];
		}
		return null;
	}

	// Token: 0x060036FD RID: 14077 RVA: 0x000537BD File Offset: 0x000519BD
	public static ZoneNode ColliderToNode(BoxCollider collider)
	{
		if (!(collider == null))
		{
			return ZoneGraph.gGraph._colliderToNode[collider];
		}
		return ZoneNode.Null;
	}

	// Token: 0x060036FE RID: 14078 RVA: 0x000537DE File Offset: 0x000519DE
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

	// Token: 0x060036FF RID: 14079 RVA: 0x0014417C File Offset: 0x0014237C
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

	// Token: 0x06003700 RID: 14080 RVA: 0x00144200 File Offset: 0x00142400
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

	// Token: 0x06003701 RID: 14081 RVA: 0x00053814 File Offset: 0x00051A14
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

	// Token: 0x06003702 RID: 14082 RVA: 0x0005384F File Offset: 0x00051A4F
	public static void Unregister(ZoneEntity entity)
	{
		ZoneGraph.gGraph._entityList.Remove(entity);
	}

	// Token: 0x04003A19 RID: 14873
	[SerializeField]
	private ZoneDef[] _zoneDefs = new ZoneDef[0];

	// Token: 0x04003A1A RID: 14874
	[SerializeField]
	private BoxCollider[] _colliders = new BoxCollider[0];

	// Token: 0x04003A1B RID: 14875
	[SerializeField]
	private ZoneNode[] _nodes = new ZoneNode[0];

	// Token: 0x04003A1C RID: 14876
	[Space]
	[NonSerialized]
	private Dictionary<BoxCollider, ZoneDef> _colliderToZoneDef = new Dictionary<BoxCollider, ZoneDef>(64);

	// Token: 0x04003A1D RID: 14877
	[Space]
	[NonSerialized]
	private Dictionary<BoxCollider, ZoneNode> _colliderToNode = new Dictionary<BoxCollider, ZoneNode>(64);

	// Token: 0x04003A1E RID: 14878
	[Space]
	[NonSerialized]
	private List<ZoneEntity> _entityList = new List<ZoneEntity>(16);

	// Token: 0x04003A1F RID: 14879
	private static ZoneGraph gGraph;
}
