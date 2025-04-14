using System;
using UnityEngine;

// Token: 0x020004B6 RID: 1206
public struct BuilderAction
{
	// Token: 0x04002039 RID: 8249
	public BuilderActionType type;

	// Token: 0x0400203A RID: 8250
	public int pieceId;

	// Token: 0x0400203B RID: 8251
	public int parentPieceId;

	// Token: 0x0400203C RID: 8252
	public Vector3 localPosition;

	// Token: 0x0400203D RID: 8253
	public Quaternion localRotation;

	// Token: 0x0400203E RID: 8254
	public byte twist;

	// Token: 0x0400203F RID: 8255
	public sbyte bumpOffsetx;

	// Token: 0x04002040 RID: 8256
	public sbyte bumpOffsetz;

	// Token: 0x04002041 RID: 8257
	public bool isLeftHand;

	// Token: 0x04002042 RID: 8258
	public int playerActorNumber;

	// Token: 0x04002043 RID: 8259
	public int parentAttachIndex;

	// Token: 0x04002044 RID: 8260
	public int attachIndex;

	// Token: 0x04002045 RID: 8261
	public SnapBounds attachBounds;

	// Token: 0x04002046 RID: 8262
	public SnapBounds parentAttachBounds;

	// Token: 0x04002047 RID: 8263
	public Vector3 velocity;

	// Token: 0x04002048 RID: 8264
	public Vector3 angVelocity;

	// Token: 0x04002049 RID: 8265
	public int localCommandId;

	// Token: 0x0400204A RID: 8266
	public int timeStamp;
}
