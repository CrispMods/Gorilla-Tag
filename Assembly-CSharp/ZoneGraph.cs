using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008F0 RID: 2288
[DefaultExecutionOrder(5555)]
public class ZoneGraph : MonoBehaviour
{
	// Token: 0x060036EF RID: 14063 RVA: 0x001046D4 File Offset: 0x001028D4
	public static ZoneGraph Instance()
	{
		return ZoneGraph.gGraph;
	}

	// Token: 0x060036F0 RID: 14064 RVA: 0x001046DB File Offset: 0x001028DB
	public static ZoneDef ColliderToZoneDef(BoxCollider collider)
	{
		if (!(collider == null))
		{
			return ZoneGraph.gGraph._colliderToZoneDef[collider];
		}
		return null;
	}

	// Token: 0x060036F1 RID: 14065 RVA: 0x001046F8 File Offset: 0x001028F8
	public static ZoneNode ColliderToNode(BoxCollider collider)
	{
		if (!(collider == null))
		{
			return ZoneGraph.gGraph._colliderToNode[collider];
		}
		return ZoneNode.Null;
	}

	// Token: 0x060036F2 RID: 14066 RVA: 0x00104719 File Offset: 0x00102919
	private void Awake()
	{
		if (ZoneGraph.gGraph != null && ZoneGraph.gGraph != this)
		{
			Object.Destroy(this);
		}
		else
		{
			ZoneGraph.gGraph = this;
		}
		this.CompileColliderMaps(this._zoneDefs);
	}

	// Token: 0x060036F3 RID: 14067 RVA: 0x00104750 File Offset: 0x00102950
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

	// Token: 0x060036F4 RID: 14068 RVA: 0x001047D4 File Offset: 0x001029D4
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

	// Token: 0x060036F5 RID: 14069 RVA: 0x00104839 File Offset: 0x00102A39
	public static void Register(ZoneEntity entity)
	{
		if (ZoneGraph.gGraph == null)
		{
			ZoneGraph.gGraph = Object.FindFirstObjectByType<ZoneGraph>();
		}
		if (!ZoneGraph.gGraph._entityList.Contains(entity))
		{
			ZoneGraph.gGraph._entityList.Add(entity);
		}
	}

	// Token: 0x060036F6 RID: 14070 RVA: 0x00104874 File Offset: 0x00102A74
	public static void Unregister(ZoneEntity entity)
	{
		ZoneGraph.gGraph._entityList.Remove(entity);
	}

	// Token: 0x04003A07 RID: 14855
	[SerializeField]
	private ZoneDef[] _zoneDefs = new ZoneDef[0];

	// Token: 0x04003A08 RID: 14856
	[SerializeField]
	private BoxCollider[] _colliders = new BoxCollider[0];

	// Token: 0x04003A09 RID: 14857
	[SerializeField]
	private ZoneNode[] _nodes = new ZoneNode[0];

	// Token: 0x04003A0A RID: 14858
	[Space]
	[NonSerialized]
	private Dictionary<BoxCollider, ZoneDef> _colliderToZoneDef = new Dictionary<BoxCollider, ZoneDef>(64);

	// Token: 0x04003A0B RID: 14859
	[Space]
	[NonSerialized]
	private Dictionary<BoxCollider, ZoneNode> _colliderToNode = new Dictionary<BoxCollider, ZoneNode>(64);

	// Token: 0x04003A0C RID: 14860
	[Space]
	[NonSerialized]
	private List<ZoneEntity> _entityList = new List<ZoneEntity>(16);

	// Token: 0x04003A0D RID: 14861
	private static ZoneGraph gGraph;
}
