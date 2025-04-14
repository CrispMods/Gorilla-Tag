using System;
using UnityEngine;

// Token: 0x020004B7 RID: 1207
public class BuilderActions
{
	// Token: 0x06001D2A RID: 7466 RVA: 0x0008E228 File Offset: 0x0008C428
	public static BuilderAction CreateAttachToPlayer(int cmdId, int pieceId, Vector3 localPosition, Quaternion localRotation, int actorNumber, bool leftHand)
	{
		return new BuilderAction
		{
			type = BuilderActionType.AttachToPlayer,
			localCommandId = cmdId,
			pieceId = pieceId,
			playerActorNumber = actorNumber,
			localPosition = localPosition,
			localRotation = localRotation,
			isLeftHand = leftHand
		};
	}

	// Token: 0x06001D2B RID: 7467 RVA: 0x0008E278 File Offset: 0x0008C478
	public static BuilderAction CreateAttachToPlayerRollback(int cmdId, BuilderPiece piece)
	{
		return BuilderActions.CreateAttachToPlayer(cmdId, piece.pieceId, piece.transform.localPosition, piece.transform.localRotation, piece.heldByPlayerActorNumber, piece.heldInLeftHand);
	}

	// Token: 0x06001D2C RID: 7468 RVA: 0x0008E2A8 File Offset: 0x0008C4A8
	public static BuilderAction CreateDetachFromPlayer(int cmdId, int pieceId, int actorNumber)
	{
		return new BuilderAction
		{
			type = BuilderActionType.DetachFromPlayer,
			localCommandId = cmdId,
			pieceId = pieceId,
			playerActorNumber = actorNumber
		};
	}

	// Token: 0x06001D2D RID: 7469 RVA: 0x0008E2E0 File Offset: 0x0008C4E0
	public static BuilderAction CreateAttachToPiece(int cmdId, int pieceId, int parentPieceId, int attachIndex, int parentAttachIndex, sbyte bumpOffsetX, sbyte bumpOffsetZ, byte twist, int actorNumber, int timeStamp)
	{
		return new BuilderAction
		{
			type = BuilderActionType.AttachToPiece,
			localCommandId = cmdId,
			pieceId = pieceId,
			parentPieceId = parentPieceId,
			attachIndex = attachIndex,
			parentAttachIndex = parentAttachIndex,
			bumpOffsetx = bumpOffsetX,
			bumpOffsetz = bumpOffsetZ,
			twist = twist,
			playerActorNumber = actorNumber,
			timeStamp = timeStamp
		};
	}

	// Token: 0x06001D2E RID: 7470 RVA: 0x0008E354 File Offset: 0x0008C554
	public static BuilderAction CreateAttachToPieceRollback(int cmdId, BuilderPiece piece, int actorNumber)
	{
		byte pieceTwist = piece.GetPieceTwist();
		sbyte bumpOffsetX;
		sbyte bumpOffsetZ;
		piece.GetPieceBumpOffset(pieceTwist, out bumpOffsetX, out bumpOffsetZ);
		return BuilderActions.CreateAttachToPiece(cmdId, piece.pieceId, piece.parentPiece.pieceId, piece.attachIndex, piece.parentAttachIndex, bumpOffsetX, bumpOffsetZ, pieceTwist, actorNumber, piece.activatedTimeStamp);
	}

	// Token: 0x06001D2F RID: 7471 RVA: 0x0008E3A0 File Offset: 0x0008C5A0
	public static BuilderAction CreateDetachFromPiece(int cmdId, int pieceId, int actorNumber)
	{
		return new BuilderAction
		{
			type = BuilderActionType.DetachFromPiece,
			localCommandId = cmdId,
			pieceId = pieceId,
			playerActorNumber = actorNumber
		};
	}

	// Token: 0x06001D30 RID: 7472 RVA: 0x0008E3D8 File Offset: 0x0008C5D8
	public static BuilderAction CreateMakeRoot(int cmdId, int pieceId)
	{
		return new BuilderAction
		{
			type = BuilderActionType.MakePieceRoot,
			localCommandId = cmdId,
			pieceId = pieceId
		};
	}

	// Token: 0x06001D31 RID: 7473 RVA: 0x0008E408 File Offset: 0x0008C608
	public static BuilderAction CreateDropPiece(int cmdId, int pieceId, Vector3 localPosition, Quaternion localRotation, Vector3 velocity, Vector3 angVelocity, int actorNumber)
	{
		return new BuilderAction
		{
			type = BuilderActionType.DropPiece,
			localCommandId = cmdId,
			pieceId = pieceId,
			localPosition = localPosition,
			localRotation = localRotation,
			velocity = velocity,
			angVelocity = angVelocity,
			playerActorNumber = actorNumber
		};
	}

	// Token: 0x06001D32 RID: 7474 RVA: 0x0008E464 File Offset: 0x0008C664
	public static BuilderAction CreateDropPieceRollback(int cmdId, BuilderPiece rootPiece, int actorNumber)
	{
		Vector3 position = rootPiece.transform.position;
		Quaternion rotation = rootPiece.transform.rotation;
		Vector3 velocity = Vector3.zero;
		Vector3 angVelocity = Vector3.zero;
		Rigidbody component = rootPiece.GetComponent<Rigidbody>();
		if (component != null)
		{
			position = component.position;
			rotation = component.rotation;
			velocity = component.velocity;
			angVelocity = component.angularVelocity;
		}
		return BuilderActions.CreateDropPiece(cmdId, rootPiece.pieceId, position, rotation, velocity, angVelocity, actorNumber);
	}

	// Token: 0x06001D33 RID: 7475 RVA: 0x0008E4D8 File Offset: 0x0008C6D8
	public static BuilderAction CreateAttachToShelfRollback(int cmdId, BuilderPiece piece, int shelfID, bool isConveyor, int timestamp = 0, float splineTime = 0f)
	{
		return new BuilderAction
		{
			type = BuilderActionType.AttachToShelf,
			localCommandId = cmdId,
			pieceId = piece.pieceId,
			attachIndex = shelfID,
			parentAttachIndex = timestamp,
			isLeftHand = isConveyor,
			velocity = new Vector3(splineTime, 0f, 0f),
			localPosition = piece.transform.localPosition,
			localRotation = piece.transform.localRotation
		};
	}
}
