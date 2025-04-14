using System;
using UnityEngine;

// Token: 0x0200058C RID: 1420
public class GorillaPawn : MonoBehaviour
{
	// Token: 0x1700038C RID: 908
	// (get) Token: 0x06002315 RID: 8981 RVA: 0x000ADE51 File Offset: 0x000AC051
	public VRRig rig
	{
		get
		{
			return this._rig;
		}
	}

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x06002316 RID: 8982 RVA: 0x000ADE59 File Offset: 0x000AC059
	public ZoneEntity zoneEntity
	{
		get
		{
			return this._zoneEntity;
		}
	}

	// Token: 0x1700038E RID: 910
	// (get) Token: 0x06002317 RID: 8983 RVA: 0x000ADE61 File Offset: 0x000AC061
	public new Transform transform
	{
		get
		{
			return this._transform;
		}
	}

	// Token: 0x1700038F RID: 911
	// (get) Token: 0x06002318 RID: 8984 RVA: 0x000ADE69 File Offset: 0x000AC069
	public XformNode handLeft
	{
		get
		{
			return this._handLeftXform;
		}
	}

	// Token: 0x17000390 RID: 912
	// (get) Token: 0x06002319 RID: 8985 RVA: 0x000ADE71 File Offset: 0x000AC071
	public XformNode handRight
	{
		get
		{
			return this._handRightXform;
		}
	}

	// Token: 0x17000391 RID: 913
	// (get) Token: 0x0600231A RID: 8986 RVA: 0x000ADE79 File Offset: 0x000AC079
	public XformNode body
	{
		get
		{
			return this._bodyXform;
		}
	}

	// Token: 0x17000392 RID: 914
	// (get) Token: 0x0600231B RID: 8987 RVA: 0x000ADE81 File Offset: 0x000AC081
	public XformNode head
	{
		get
		{
			return this._headXform;
		}
	}

	// Token: 0x0600231C RID: 8988 RVA: 0x000ADE89 File Offset: 0x000AC089
	private void Awake()
	{
		this.Setup(false);
	}

	// Token: 0x0600231D RID: 8989 RVA: 0x000ADE94 File Offset: 0x000AC094
	private void Setup(bool force)
	{
		this._transform = base.transform;
		this._rig = base.GetComponentInChildren<VRRig>();
		if (!this._rig)
		{
			return;
		}
		this._zoneEntity = this._rig.zoneEntity;
		if (this._zoneEntity)
		{
			if (this._bodyXform == null)
			{
				this._bodyXform = new XformNode();
			}
			this._bodyXform.localPosition = this._zoneEntity.collider.center;
			this._bodyXform.radius = this._zoneEntity.collider.radius;
			this._bodyXform.parent = this._transform;
		}
		bool flag = force || this._handLeft.AsNull<Transform>() == null;
		bool flag2 = force || this._handRight.AsNull<Transform>() == null;
		bool flag3 = force || this._head.AsNull<Transform>() == null;
		if (!flag && !flag2 && !flag3)
		{
			return;
		}
		foreach (Transform transform in this._rig.mainSkin.bones)
		{
			string name = transform.name;
			if (flag3 && name.StartsWith("head", StringComparison.OrdinalIgnoreCase))
			{
				this._head = transform;
				this._headXform = new XformNode();
				this._headXform.localPosition = new Vector3(0f, 0.13f, 0.015f);
				this._headXform.radius = 0.12f;
				this._headXform.parent = transform;
			}
			else if (flag && name.StartsWith("hand.L", StringComparison.OrdinalIgnoreCase))
			{
				this._handLeft = transform;
				this._handLeftXform = new XformNode();
				this._handLeftXform.localPosition = new Vector3(-0.014f, 0.034f, 0f);
				this._handLeftXform.radius = 0.044f;
				this._handLeftXform.parent = transform;
			}
			else if (flag2 && name.StartsWith("hand.R", StringComparison.OrdinalIgnoreCase))
			{
				this._handRight = transform;
				this._handRightXform = new XformNode();
				this._handRightXform.localPosition = new Vector3(0.014f, 0.034f, 0f);
				this._handRightXform.radius = 0.044f;
				this._handRightXform.parent = transform;
			}
		}
	}

	// Token: 0x0600231E RID: 8990 RVA: 0x000AE107 File Offset: 0x000AC307
	private bool CanRun()
	{
		if (GorillaPawn._gPawnActiveCount > 10)
		{
			Debug.LogError(string.Format("Cannot register more than {0} pawns.", 10));
			return false;
		}
		return true;
	}

	// Token: 0x0600231F RID: 8991 RVA: 0x000AE12C File Offset: 0x000AC32C
	private void OnEnable()
	{
		if (!this.CanRun())
		{
			return;
		}
		this._id = -1;
		if (this._rig && this._rig.OwningNetPlayer != null)
		{
			this._id = this._rig.OwningNetPlayer.ActorNumber;
		}
		this._index = GorillaPawn._gPawnActiveCount++;
		GorillaPawn._gPawns[this._index] = this;
	}

	// Token: 0x06002320 RID: 8992 RVA: 0x000AE19C File Offset: 0x000AC39C
	private void OnDisable()
	{
		this._id = -1;
		if (!this.CanRun())
		{
			return;
		}
		if (this._index < 0 || this._index >= GorillaPawn._gPawnActiveCount - 1)
		{
			return;
		}
		int num = --GorillaPawn._gPawnActiveCount;
		GorillaPawn._gPawns.Swap(this._index, num);
		this._index = num;
	}

	// Token: 0x06002321 RID: 8993 RVA: 0x000AE1F8 File Offset: 0x000AC3F8
	private void OnDestroy()
	{
		int num = GorillaPawn._gPawns.IndexOfRef(this);
		GorillaPawn._gPawns[num] = null;
		Array.Sort<GorillaPawn>(GorillaPawn._gPawns, new Comparison<GorillaPawn>(GorillaPawn.ComparePawns));
		int num2 = 0;
		while (num2 < GorillaPawn._gPawns.Length && GorillaPawn._gPawns[num2])
		{
			num2++;
		}
		GorillaPawn._gPawnActiveCount = num2;
	}

	// Token: 0x06002322 RID: 8994 RVA: 0x000AE258 File Offset: 0x000AC458
	private static int ComparePawns(GorillaPawn x, GorillaPawn y)
	{
		bool flag = x.AsNull<GorillaPawn>() == null;
		bool flag2 = y.AsNull<GorillaPawn>() == null;
		if (flag && flag2)
		{
			return 0;
		}
		if (flag)
		{
			return 1;
		}
		if (flag2)
		{
			return -1;
		}
		return x._index.CompareTo(y._index);
	}

	// Token: 0x17000393 RID: 915
	// (get) Token: 0x06002323 RID: 8995 RVA: 0x000AE2A1 File Offset: 0x000AC4A1
	public static GorillaPawn[] AllPawns
	{
		get
		{
			return GorillaPawn._gPawns;
		}
	}

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x06002324 RID: 8996 RVA: 0x000AE2A8 File Offset: 0x000AC4A8
	public static int ActiveCount
	{
		get
		{
			return GorillaPawn._gPawnActiveCount;
		}
	}

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x06002325 RID: 8997 RVA: 0x000AE2AF File Offset: 0x000AC4AF
	public static Matrix4x4[] ShaderData
	{
		get
		{
			return GorillaPawn._gShaderData;
		}
	}

	// Token: 0x06002326 RID: 8998 RVA: 0x000AE2B8 File Offset: 0x000AC4B8
	public static void SyncPawnData()
	{
		Matrix4x4[] gShaderData = GorillaPawn._gShaderData;
		m4x4 m4x = default(m4x4);
		for (int i = 0; i < GorillaPawn._gPawnActiveCount; i++)
		{
			GorillaPawn gorillaPawn = GorillaPawn._gPawns[i];
			Vector4 worldPosition = gorillaPawn._headXform.worldPosition;
			Vector4 worldPosition2 = gorillaPawn._bodyXform.worldPosition;
			Vector4 worldPosition3 = gorillaPawn._handLeftXform.worldPosition;
			Vector4 worldPosition4 = gorillaPawn._handRightXform.worldPosition;
			m4x.SetRow0(ref worldPosition);
			m4x.SetRow1(ref worldPosition2);
			m4x.SetRow2(ref worldPosition3);
			m4x.SetRow3(ref worldPosition4);
			m4x.Push(ref gShaderData[i]);
		}
		for (int j = GorillaPawn._gPawnActiveCount; j < 10; j++)
		{
			MatrixUtils.Clear(ref gShaderData[j]);
		}
	}

	// Token: 0x040026A9 RID: 9897
	[SerializeField]
	private Transform _transform;

	// Token: 0x040026AA RID: 9898
	[SerializeField]
	private Transform _handLeft;

	// Token: 0x040026AB RID: 9899
	[SerializeField]
	private Transform _handRight;

	// Token: 0x040026AC RID: 9900
	[SerializeField]
	private Transform _head;

	// Token: 0x040026AD RID: 9901
	[Space]
	[SerializeField]
	private VRRig _rig;

	// Token: 0x040026AE RID: 9902
	[SerializeField]
	private ZoneEntity _zoneEntity;

	// Token: 0x040026AF RID: 9903
	[Space]
	[SerializeField]
	private XformNode _handLeftXform;

	// Token: 0x040026B0 RID: 9904
	[SerializeField]
	private XformNode _handRightXform;

	// Token: 0x040026B1 RID: 9905
	[SerializeField]
	private XformNode _bodyXform;

	// Token: 0x040026B2 RID: 9906
	[SerializeField]
	private XformNode _headXform;

	// Token: 0x040026B3 RID: 9907
	[Space]
	private int _id;

	// Token: 0x040026B4 RID: 9908
	private int _index;

	// Token: 0x040026B5 RID: 9909
	private bool _invalid;

	// Token: 0x040026B6 RID: 9910
	public const int MAX_PAWNS = 10;

	// Token: 0x040026B7 RID: 9911
	private static GorillaPawn[] _gPawns = new GorillaPawn[10];

	// Token: 0x040026B8 RID: 9912
	private static int _gPawnActiveCount = 0;

	// Token: 0x040026B9 RID: 9913
	private static Matrix4x4[] _gShaderData = new Matrix4x4[10];
}
