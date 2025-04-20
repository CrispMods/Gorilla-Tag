using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

// Token: 0x020000BF RID: 191
[NetworkStructWeaved(11)]
[StructLayout(LayoutKind.Explicit, Size = 44)]
public struct SkeletonNetData : INetworkStruct
{
	// Token: 0x17000057 RID: 87
	// (get) Token: 0x060004DD RID: 1245 RVA: 0x00033A3F File Offset: 0x00031C3F
	// (set) Token: 0x060004DE RID: 1246 RVA: 0x00033A47 File Offset: 0x00031C47
	public int CurrentState { readonly get; set; }

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x060004DF RID: 1247 RVA: 0x00033A50 File Offset: 0x00031C50
	// (set) Token: 0x060004E0 RID: 1248 RVA: 0x00033A62 File Offset: 0x00031C62
	[Networked]
	public unsafe Vector3 Position
	{
		readonly get
		{
			return *(Vector3*)Native.ReferenceToPointer<FixedStorage@3>(ref this._Position);
		}
		set
		{
			*(Vector3*)Native.ReferenceToPointer<FixedStorage@3>(ref this._Position) = value;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00033A75 File Offset: 0x00031C75
	// (set) Token: 0x060004E2 RID: 1250 RVA: 0x00033A87 File Offset: 0x00031C87
	[Networked]
	public unsafe Quaternion Rotation
	{
		readonly get
		{
			return *(Quaternion*)Native.ReferenceToPointer<FixedStorage@4>(ref this._Rotation);
		}
		set
		{
			*(Quaternion*)Native.ReferenceToPointer<FixedStorage@4>(ref this._Rotation) = value;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x060004E3 RID: 1251 RVA: 0x00033A9A File Offset: 0x00031C9A
	// (set) Token: 0x060004E4 RID: 1252 RVA: 0x00033AA2 File Offset: 0x00031CA2
	public int CurrentNode { readonly get; set; }

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x060004E5 RID: 1253 RVA: 0x00033AAB File Offset: 0x00031CAB
	// (set) Token: 0x060004E6 RID: 1254 RVA: 0x00033AB3 File Offset: 0x00031CB3
	public int NextNode { readonly get; set; }

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x060004E7 RID: 1255 RVA: 0x00033ABC File Offset: 0x00031CBC
	// (set) Token: 0x060004E8 RID: 1256 RVA: 0x00033AC4 File Offset: 0x00031CC4
	public int AngerPoint { readonly get; set; }

	// Token: 0x060004E9 RID: 1257 RVA: 0x00033ACD File Offset: 0x00031CCD
	public SkeletonNetData(int state, Vector3 pos, Quaternion rot, int cNode, int nNode, int angerPoint)
	{
		this.CurrentState = state;
		this.Position = pos;
		this.Rotation = rot;
		this.CurrentNode = cNode;
		this.NextNode = nNode;
		this.AngerPoint = angerPoint;
	}

	// Token: 0x040005A6 RID: 1446
	[FixedBufferProperty(typeof(Vector3), typeof(UnityValueSurrogate@ReaderWriter@UnityEngine_Vector3), 0, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(4)]
	private FixedStorage@3 _Position;

	// Token: 0x040005A7 RID: 1447
	[FixedBufferProperty(typeof(Quaternion), typeof(UnityValueSurrogate@ReaderWriter@UnityEngine_Quaternion), 0, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(16)]
	private FixedStorage@4 _Rotation;
}
