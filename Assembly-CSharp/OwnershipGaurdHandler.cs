using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020007C1 RID: 1985
internal class OwnershipGaurdHandler : IPunOwnershipCallbacks
{
	// Token: 0x06003112 RID: 12562 RVA: 0x0005082E File Offset: 0x0004EA2E
	static OwnershipGaurdHandler()
	{
		PhotonNetwork.AddCallbackTarget(OwnershipGaurdHandler.callbackInstance);
	}

	// Token: 0x06003113 RID: 12563 RVA: 0x0005084E File Offset: 0x0004EA4E
	internal static void RegisterView(PhotonView view)
	{
		if (view == null || OwnershipGaurdHandler.gaurdedViews.Contains(view))
		{
			return;
		}
		OwnershipGaurdHandler.gaurdedViews.Add(view);
	}

	// Token: 0x06003114 RID: 12564 RVA: 0x00132AA4 File Offset: 0x00130CA4
	internal static void RegisterViews(PhotonView[] photonViews)
	{
		for (int i = 0; i < photonViews.Length; i++)
		{
			OwnershipGaurdHandler.RegisterView(photonViews[i]);
		}
	}

	// Token: 0x06003115 RID: 12565 RVA: 0x00050873 File Offset: 0x0004EA73
	internal static void RemoveView(PhotonView view)
	{
		if (view == null)
		{
			return;
		}
		OwnershipGaurdHandler.gaurdedViews.Remove(view);
	}

	// Token: 0x06003116 RID: 12566 RVA: 0x00132ACC File Offset: 0x00130CCC
	internal static void RemoveViews(PhotonView[] photonViews)
	{
		for (int i = 0; i < photonViews.Length; i++)
		{
			OwnershipGaurdHandler.RemoveView(photonViews[i]);
		}
	}

	// Token: 0x06003117 RID: 12567 RVA: 0x00132AF4 File Offset: 0x00130CF4
	void IPunOwnershipCallbacks.OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
	{
		if (!OwnershipGaurdHandler.gaurdedViews.Contains(targetView))
		{
			return;
		}
		if (targetView.IsRoomView)
		{
			if (targetView.Owner != PhotonNetwork.MasterClient)
			{
				targetView.OwnerActorNr = 0;
				targetView.ControllerActorNr = 0;
				return;
			}
		}
		else if (targetView.OwnerActorNr != targetView.CreatorActorNr || targetView.ControllerActorNr != targetView.CreatorActorNr)
		{
			targetView.OwnerActorNr = targetView.CreatorActorNr;
			targetView.ControllerActorNr = targetView.CreatorActorNr;
		}
	}

	// Token: 0x06003118 RID: 12568 RVA: 0x00030607 File Offset: 0x0002E807
	void IPunOwnershipCallbacks.OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
	{
	}

	// Token: 0x06003119 RID: 12569 RVA: 0x00030607 File Offset: 0x0002E807
	void IPunOwnershipCallbacks.OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
	{
	}

	// Token: 0x04003508 RID: 13576
	private static HashSet<PhotonView> gaurdedViews = new HashSet<PhotonView>();

	// Token: 0x04003509 RID: 13577
	private static readonly OwnershipGaurdHandler callbackInstance = new OwnershipGaurdHandler();
}
