using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class CrittersRigActorSetup : MonoBehaviour
{
	// Token: 0x0600027B RID: 635 RVA: 0x00031ED3 File Offset: 0x000300D3
	public void OnEnable()
	{
		CrittersManager.RegisterRigActorSetup(this);
	}

	// Token: 0x0600027C RID: 636 RVA: 0x00073FC4 File Offset: 0x000721C4
	public void OnDisable()
	{
		for (int i = 0; i < this.rigActors.Length; i++)
		{
			this.rigActors[i].actorSet = null;
		}
	}

	// Token: 0x0600027D RID: 637 RVA: 0x00073FF8 File Offset: 0x000721F8
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

	// Token: 0x0600027E RID: 638 RVA: 0x000740A0 File Offset: 0x000722A0
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

	// Token: 0x040002FE RID: 766
	public CrittersRigActorSetup.RigActor[] rigActors;

	// Token: 0x040002FF RID: 767
	public List<object> rigActorData = new List<object>();

	// Token: 0x04000300 RID: 768
	public VRRig myRig;

	// Token: 0x02000063 RID: 99
	[Serializable]
	public struct RigActor
	{
		// Token: 0x04000301 RID: 769
		public Transform location;

		// Token: 0x04000302 RID: 770
		public CrittersActor.CrittersActorType type;

		// Token: 0x04000303 RID: 771
		public int subIndex;

		// Token: 0x04000304 RID: 772
		public CrittersActor actorSet;
	}
}
