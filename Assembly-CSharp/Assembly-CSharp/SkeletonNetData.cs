using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

// Token: 0x020000B5 RID: 181
[NetworkStructWeaved(11)]
[StructLayout(LayoutKind.Explicit, Size = 44)]
public struct SkeletonNetData : INetworkStruct
{
	// Token: 0x17000052 RID: 82
	// (get) Token: 0x060004A3 RID: 1187 RVA: 0x0001C1E5 File Offset: 0x0001A3E5
	// (set) Token: 0x060004A4 RID: 1188 RVA: 0x0001C1ED File Offset: 0x0001A3ED
	public int CurrentState { readonly get; set; }

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x060004A5 RID: 1189 RVA: 0x0001C1F6 File Offset: 0x0001A3F6
	// (set) Token: 0x060004A6 RID: 1190 RVA: 0x0001C208 File Offset: 0x0001A408
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

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x060004A7 RID: 1191 RVA: 0x0001C21B File Offset: 0x0001A41B
	// (set) Token: 0x060004A8 RID: 1192 RVA: 0x0001C22D File Offset: 0x0001A42D
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

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x060004A9 RID: 1193 RVA: 0x0001C240 File Offset: 0x0001A440
	// (set) Token: 0x060004AA RID: 1194 RVA: 0x0001C248 File Offset: 0x0001A448
	public int CurrentNode { readonly get; set; }

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x060004AB RID: 1195 RVA: 0x0001C251 File Offset: 0x0001A451
	// (set) Token: 0x060004AC RID: 1196 RVA: 0x0001C259 File Offset: 0x0001A459
	public int NextNode { readonly get; set; }

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x060004AD RID: 1197 RVA: 0x0001C262 File Offset: 0x0001A462
	// (set) Token: 0x060004AE RID: 1198 RVA: 0x0001C26A File Offset: 0x0001A46A
	public int AngerPoint { readonly get; set; }

	// Token: 0x060004AF RID: 1199 RVA: 0x0001C273 File Offset: 0x0001A473
	public SkeletonNetData(int state, Vector3 pos, Quaternion rot, int cNode, int nNode, int angerPoint)
	{
		this.CurrentState = state;
		this.Position = pos;
		this.Rotation = rot;
		this.CurrentNode = cNode;
		this.NextNode = nNode;
		this.AngerPoint = angerPoint;
	}

	// Token: 0x04000567 RID: 1383
	[FixedBufferProperty(typeof(Vector3), typeof(UnityValueSurrogate@ReaderWriter@UnityEngine_Vector3), 0, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(4)]
	private FixedStorage@3 _Position;

	// Token: 0x04000568 RID: 1384
	[FixedBufferProperty(typeof(Quaternion), typeof(UnityValueSurrogate@ReaderWriter@UnityEngine_Quaternion), 0, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(16)]
	private FixedStorage@4 _Rotation;
}
