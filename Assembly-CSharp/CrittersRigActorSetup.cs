using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class CrittersRigActorSetup : MonoBehaviour
{
	// Token: 0x0600024F RID: 591 RVA: 0x0000F0FD File Offset: 0x0000D2FD
	public void OnEnable()
	{
		CrittersManager.RegisterRigActorSetup(this);
	}

	// Token: 0x06000250 RID: 592 RVA: 0x0000F108 File Offset: 0x0000D308
	private CrittersActor RefreshActorForIndex(int index)
	{
		CrittersRigActorSetup.RigActor rigActor = this.rigActors[index];
		if (rigActor.actorSet.IsNotNull())
		{
			rigActor.actorSet.gameObject.SetActive(false);
		}
		CrittersActor crittersActor = CrittersManager.instance.SpawnActor(rigActor.type, rigActor.subIndex);
		if (crittersActor.IsNull())
		{
			return null;
		}
		crittersActor.isOnPlayer = true;
		crittersActor.rigIndex = index;
		crittersActor.rigPlayerId = this.myRig.Creator.ActorNumber;
		if (crittersActor.rigPlayerId == -1 && PhotonNetwork.InRoom)
		{
			crittersActor.rigPlayerId = PhotonNetwork.LocalPlayer.ActorNumber;
		}
		crittersActor.PlacePlayerCrittersActor();
		return crittersActor;
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0000F1B0 File Offset: 0x0000D3B0
	public void CheckUpdate(ref List<object> refActorData, bool forceCheck = false)
	{
		for (int i = 0; i < this.rigActors.Length; i++)
		{
			CrittersRigActorSetup.RigActor rigActor = this.rigActors[i];
			RigContainer rigContainer;
			if (base.gameObject.activeInHierarchy && (forceCheck || rigActor.actorSet == null || (rigActor.actorSet.rigPlayerId != this.myRig.Creator.ActorNumber && VRRigCache.Instance.TryGetVrrig(this.myRig.Creator, out rigContainer) && CrittersManager.instance.rigSetupByRig.ContainsKey(this.myRig))))
			{
				CrittersActor crittersActor = this.RefreshActorForIndex(i);
				if (crittersActor != null)
				{
					crittersActor.AddPlayerCrittersActorDataToList(ref refActorData);
				}
			}
		}
	}

	// Token: 0x040002CF RID: 719
	public CrittersRigActorSetup.RigActor[] rigActors;

	// Token: 0x040002D0 RID: 720
	public List<object> rigActorData = new List<object>();

	// Token: 0x040002D1 RID: 721
	public VRRig myRig;

	// Token: 0x0200005D RID: 93
	[Serializable]
	public struct RigActor
	{
		// Token: 0x040002D2 RID: 722
		public Transform location;

		// Token: 0x040002D3 RID: 723
		public CrittersActor.CrittersActorType type;

		// Token: 0x040002D4 RID: 724
		public int subIndex;

		// Token: 0x040002D5 RID: 725
		public CrittersActor actorSet;
	}
}
