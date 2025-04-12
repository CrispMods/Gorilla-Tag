using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class CrittersRigActorSetup : MonoBehaviour
{
	// Token: 0x06000250 RID: 592 RVA: 0x00030D91 File Offset: 0x0002EF91
	public void OnEnable()
	{
		CrittersManager.RegisterRigActorSetup(this);
	}

	// Token: 0x06000251 RID: 593 RVA: 0x000719F0 File Offset: 0x0006FBF0
	public void OnDisable()
	{
		for (int i = 0; i < this.rigActors.Length; i++)
		{
			this.rigActors[i].actorSet = null;
		}
	}

	// Token: 0x06000252 RID: 594 RVA: 0x00071A24 File Offset: 0x0006FC24
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

	// Token: 0x06000253 RID: 595 RVA: 0x00071ACC File Offset: 0x0006FCCC
	public void CheckUpdate(ref List<object> refActorData, bool forceCheck = false)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		for (int i = 0; i < this.rigActors.Length; i++)
		{
			CrittersRigActorSetup.RigActor rigActor = this.rigActors[i];
			RigContainer rigContainer;
			if (forceCheck || rigActor.actorSet == null || (rigActor.actorSet.rigPlayerId != this.myRig.Creator.ActorNumber && VRRigCache.Instance.TryGetVrrig(this.myRig.Creator, out rigContainer) && CrittersManager.instance.rigSetupByRig.ContainsKey(this.myRig)))
			{
				CrittersActor crittersActor = this.RefreshActorForIndex(i);
				if (crittersActor != null)
				{
					crittersActor.AddPlayerCrittersActorDataToList(ref refActorData);
				}
			}
		}
	}

	// Token: 0x040002D0 RID: 720
	public CrittersRigActorSetup.RigActor[] rigActors;

	// Token: 0x040002D1 RID: 721
	public List<object> rigActorData = new List<object>();

	// Token: 0x040002D2 RID: 722
	public VRRig myRig;

	// Token: 0x0200005D RID: 93
	[Serializable]
	public struct RigActor
	{
		// Token: 0x040002D3 RID: 723
		public Transform location;

		// Token: 0x040002D4 RID: 724
		public CrittersActor.CrittersActorType type;

		// Token: 0x040002D5 RID: 725
		public int subIndex;

		// Token: 0x040002D6 RID: 726
		public CrittersActor actorSet;
	}
}
