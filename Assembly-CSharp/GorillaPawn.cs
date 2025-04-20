using System;
using UnityEngine;

// Token: 0x0200059A RID: 1434
public class GorillaPawn : MonoBehaviour
{
	// Token: 0x17000394 RID: 916
	// (get) Token: 0x06002375 RID: 9077 RVA: 0x00047F3F File Offset: 0x0004613F
	public VRRig rig
	{
		get
		{
			return this._rig;
		}
	}

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x06002376 RID: 9078 RVA: 0x00047F47 File Offset: 0x00046147
	public ZoneEntity zoneEntity
	{
		get
		{
			return this._zoneEntity;
		}
	}

	// Token: 0x17000396 RID: 918
	// (get) Token: 0x06002377 RID: 9079 RVA: 0x00047F4F File Offset: 0x0004614F
	public new Transform transform
	{
		get
		{
			return this._transform;
		}
	}

	// Token: 0x17000397 RID: 919
	// (get) Token: 0x06002378 RID: 9080 RVA: 0x00047F57 File Offset: 0x00046157
	public XformNode handLeft
	{
		get
		{
			return this._handLeftXform;
		}
	}

	// Token: 0x17000398 RID: 920
	// (get) Token: 0x06002379 RID: 9081 RVA: 0x00047F5F File Offset: 0x0004615F
	public XformNode handRight
	{
		get
		{
			return this._handRightXform;
		}
	}

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x0600237A RID: 9082 RVA: 0x00047F67 File Offset: 0x00046167
	public XformNode body
	{
		get
		{
			return this._bodyXform;
		}
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x0600237B RID: 9083 RVA: 0x00047F6F File Offset: 0x0004616F
	public XformNode head
	{
		get
		{
			return this._headXform;
		}
	}

	// Token: 0x0600237C RID: 9084 RVA: 0x00047F77 File Offset: 0x00046177
	private void Awake()
	{
		this.Setup(false);
	}

	// Token: 0x0600237D RID: 9085 RVA: 0x000FD5D8 File Offset: 0x000FB7D8
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

	// Token: 0x0600237E RID: 9086 RVA: 0x00047F80 File Offset: 0x00046180
	private bool CanRun()
	{
		if (GorillaPawn._gPawnActiveCount > 10)
		{
			Debug.LogError(string.Format("Cannot register more than {0} pawns.", 10));
			return false;
		}
		return true;
	}

	// Token: 0x0600237F RID: 9087 RVA: 0x000FD84C File Offset: 0x000FBA4C
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

	// Token: 0x06002380 RID: 9088 RVA: 0x000FD8BC File Offset: 0x000FBABC
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

	// Token: 0x06002381 RID: 9089 RVA: 0x000FD918 File Offset: 0x000FBB18
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

	// Token: 0x06002382 RID: 9090 RVA: 0x000FD978 File Offset: 0x000FBB78
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

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x06002383 RID: 9091 RVA: 0x00047FA4 File Offset: 0x000461A4
	public static GorillaPawn[] AllPawns
	{
		get
		{
			return GorillaPawn._gPawns;
		}
	}

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x06002384 RID: 9092 RVA: 0x00047FAB File Offset: 0x000461AB
	public static int ActiveCount
	{
		get
		{
			return GorillaPawn._gPawnActiveCount;
		}
	}

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x06002385 RID: 9093 RVA: 0x00047FB2 File Offset: 0x000461B2
	public static Matrix4x4[] ShaderData
	{
		get
		{
			return GorillaPawn._gShaderData;
		}
	}

	// Token: 0x06002386 RID: 9094 RVA: 0x000FD9C4 File Offset: 0x000FBBC4
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

	// Token: 0x04002704 RID: 9988
	[SerializeField]
	private Transform _transform;

	// Token: 0x04002705 RID: 9989
	[SerializeField]
	private Transform _handLeft;

	// Token: 0x04002706 RID: 9990
	[SerializeField]
	private Transform _handRight;

	// Token: 0x04002707 RID: 9991
	[SerializeField]
	private Transform _head;

	// Token: 0x04002708 RID: 9992
	[Space]
	[SerializeField]
	private VRRig _rig;

	// Token: 0x04002709 RID: 9993
	[SerializeField]
	private ZoneEntity _zoneEntity;

	// Token: 0x0400270A RID: 9994
	[Space]
	[SerializeField]
	private XformNode _handLeftXform;

	// Token: 0x0400270B RID: 9995
	[SerializeField]
	private XformNode _handRightXform;

	// Token: 0x0400270C RID: 9996
	[SerializeField]
	private XformNode _bodyXform;

	// Token: 0x0400270D RID: 9997
	[SerializeField]
	private XformNode _headXform;

	// Token: 0x0400270E RID: 9998
	[Space]
	private int _id;

	// Token: 0x0400270F RID: 9999
	private int _index;

	// Token: 0x04002710 RID: 10000
	private bool _invalid;

	// Token: 0x04002711 RID: 10001
	public const int MAX_PAWNS = 10;

	// Token: 0x04002712 RID: 10002
	private static GorillaPawn[] _gPawns = new GorillaPawn[10];

	// Token: 0x04002713 RID: 10003
	private static int _gPawnActiveCount = 0;

	// Token: 0x04002714 RID: 10004
	private static Matrix4x4[] _gShaderData = new Matrix4x4[10];
}
