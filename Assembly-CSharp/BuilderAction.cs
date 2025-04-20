using System;
using UnityEngine;

// Token: 0x020004C3 RID: 1219
public struct BuilderAction
{
	// Token: 0x0400208C RID: 8332
	public BuilderActionType type;

	// Token: 0x0400208D RID: 8333
	public int pieceId;

	// Token: 0x0400208E RID: 8334
	public int parentPieceId;

	// Token: 0x0400208F RID: 8335
	public Vector3 localPosition;

	// Token: 0x04002090 RID: 8336
	public Quaternion localRotation;

	// Token: 0x04002091 RID: 8337
	public byte twist;

	// Token: 0x04002092 RID: 8338
	public sbyte bumpOffsetx;

	// Token: 0x04002093 RID: 8339
	public sbyte bumpOffsetz;

	// Token: 0x04002094 RID: 8340
	public bool isLeftHand;

	// Token: 0x04002095 RID: 8341
	public int playerActorNumber;

	// Token: 0x04002096 RID: 8342
	public int parentAttachIndex;

	// Token: 0x04002097 RID: 8343
	public int attachIndex;

	// Token: 0x04002098 RID: 8344
	public SnapBounds attachBounds;

	// Token: 0x04002099 RID: 8345
	public SnapBounds parentAttachBounds;

	// Token: 0x0400209A RID: 8346
	public Vector3 velocity;

	// Token: 0x0400209B RID: 8347
	public Vector3 angVelocity;

	// Token: 0x0400209C RID: 8348
	public int localCommandId;

	// Token: 0x0400209D RID: 8349
	public int timeStamp;
}
