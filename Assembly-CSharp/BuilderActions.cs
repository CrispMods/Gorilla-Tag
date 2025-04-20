using System;
using UnityEngine;

// Token: 0x020004C4 RID: 1220
public class BuilderActions
{
	// Token: 0x06001D83 RID: 7555 RVA: 0x000E147C File Offset: 0x000DF67C
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

	// Token: 0x06001D84 RID: 7556 RVA: 0x0004432A File Offset: 0x0004252A
	public static BuilderAction CreateAttachToPlayerRollback(int cmdId, BuilderPiece piece)
	{
		return BuilderActions.CreateAttachToPlayer(cmdId, piece.pieceId, piece.transform.localPosition, piece.transform.localRotation, piece.heldByPlayerActorNumber, piece.heldInLeftHand);
	}

	// Token: 0x06001D85 RID: 7557 RVA: 0x000E14CC File Offset: 0x000DF6CC
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

	// Token: 0x06001D86 RID: 7558 RVA: 0x000E1504 File Offset: 0x000DF704
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

	// Token: 0x06001D87 RID: 7559 RVA: 0x000E1578 File Offset: 0x000DF778
	public static BuilderAction CreateAttachToPieceRollback(int cmdId, BuilderPiece piece, int actorNumber)
	{
		byte pieceTwist = piece.GetPieceTwist();
		sbyte bumpOffsetX;
		sbyte bumpOffsetZ;
		piece.GetPieceBumpOffset(pieceTwist, out bumpOffsetX, out bumpOffsetZ);
		return BuilderActions.CreateAttachToPiece(cmdId, piece.pieceId, piece.parentPiece.pieceId, piece.attachIndex, piece.parentAttachIndex, bumpOffsetX, bumpOffsetZ, pieceTwist, actorNumber, piece.activatedTimeStamp);
	}

	// Token: 0x06001D88 RID: 7560 RVA: 0x000E15C4 File Offset: 0x000DF7C4
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

	// Token: 0x06001D89 RID: 7561 RVA: 0x000E15FC File Offset: 0x000DF7FC
	public static BuilderAction CreateMakeRoot(int cmdId, int pieceId)
	{
		return new BuilderAction
		{
			type = BuilderActionType.MakePieceRoot,
			localCommandId = cmdId,
			pieceId = pieceId
		};
	}

	// Token: 0x06001D8A RID: 7562 RVA: 0x000E162C File Offset: 0x000DF82C
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

	// Token: 0x06001D8B RID: 7563 RVA: 0x000E1688 File Offset: 0x000DF888
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

	// Token: 0x06001D8C RID: 7564 RVA: 0x000E16FC File Offset: 0x000DF8FC
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
