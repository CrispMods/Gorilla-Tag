using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020007A9 RID: 1961
internal class OwnershipGaurdHandler : IPunOwnershipCallbacks
{
	// Token: 0x06003060 RID: 12384 RVA: 0x000E9819 File Offset: 0x000E7A19
	static OwnershipGaurdHandler()
	{
		PhotonNetwork.AddCallbackTarget(OwnershipGaurdHandler.callbackInstance);
	}

	// Token: 0x06003061 RID: 12385 RVA: 0x000E9839 File Offset: 0x000E7A39
	internal static void RegisterView(PhotonView view)
	{
		if (view == null || OwnershipGaurdHandler.gaurdedViews.Contains(view))
		{
			return;
		}
		OwnershipGaurdHandler.gaurdedViews.Add(view);
	}

	// Token: 0x06003062 RID: 12386 RVA: 0x000E9860 File Offset: 0x000E7A60
	internal static void RegisterViews(PhotonView[] photonViews)
	{
		for (int i = 0; i < photonViews.Length; i++)
		{
			OwnershipGaurdHandler.RegisterView(photonViews[i]);
		}
	}

	// Token: 0x06003063 RID: 12387 RVA: 0x000E9885 File Offset: 0x000E7A85
	internal static void RemoveView(PhotonView view)
	{
		if (view == null)
		{
			return;
		}
		OwnershipGaurdHandler.gaurdedViews.Remove(view);
	}

	// Token: 0x06003064 RID: 12388 RVA: 0x000E98A0 File Offset: 0x000E7AA0
	internal static void RemoveViews(PhotonView[] photonViews)
	{
		for (int i = 0; i < photonViews.Length; i++)
		{
			OwnershipGaurdHandler.RemoveView(photonViews[i]);
		}
	}

	// Token: 0x06003065 RID: 12389 RVA: 0x000E98C8 File Offset: 0x000E7AC8
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

	// Token: 0x06003066 RID: 12390 RVA: 0x000023F4 File Offset: 0x000005F4
	void IPunOwnershipCallbacks.OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
	{
	}

	// Token: 0x06003067 RID: 12391 RVA: 0x000023F4 File Offset: 0x000005F4
	void IPunOwnershipCallbacks.OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
	{
	}

	// Token: 0x0400345E RID: 13406
	private static HashSet<PhotonView> gaurdedViews = new HashSet<PhotonView>();

	// Token: 0x0400345F RID: 13407
	private static readonly OwnershipGaurdHandler callbackInstance = new OwnershipGaurdHandler();
}
