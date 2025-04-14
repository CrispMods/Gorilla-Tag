using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000035 RID: 53
public static class CrittersGrabberSharedData
{
	// Token: 0x06000106 RID: 262 RVA: 0x00007E41 File Offset: 0x00006041
	public static void Initialize()
	{
		if (CrittersGrabberSharedData.initialized)
		{
			return;
		}
		CrittersGrabberSharedData.initialized = true;
		CrittersGrabberSharedData.enteredCritterActor = new List<CrittersActor>();
		CrittersGrabberSharedData.triggerCollidersToCheck = new List<CapsuleCollider>();
		CrittersGrabberSharedData.heldActor = new List<CrittersActor>();
		CrittersGrabberSharedData.actorGrabbers = new List<CrittersActorGrabber>();
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00007E79 File Offset: 0x00006079
	public static void AddEnteredActor(CrittersActor actor)
	{
		CrittersGrabberSharedData.Initialize();
		if (CrittersGrabberSharedData.enteredCritterActor.Contains(actor))
		{
			return;
		}
		CrittersGrabberSharedData.enteredCritterActor.Add(actor);
	}

	// Token: 0x06000108 RID: 264 RVA: 0x00007E99 File Offset: 0x00006099
	public static void RemoveEnteredActor(CrittersActor actor)
	{
		CrittersGrabberSharedData.Initialize();
		if (!CrittersGrabberSharedData.enteredCritterActor.Contains(actor))
		{
			return;
		}
		CrittersGrabberSharedData.enteredCritterActor.Remove(actor);
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00007EBA File Offset: 0x000060BA
	public static void AddTrigger(CapsuleCollider trigger)
	{
		CrittersGrabberSharedData.Initialize();
		if (CrittersGrabberSharedData.triggerCollidersToCheck.Contains(trigger))
		{
			return;
		}
		CrittersGrabberSharedData.triggerCollidersToCheck.Add(trigger);
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00007EDA File Offset: 0x000060DA
	public static void RemoveTrigger(CapsuleCollider trigger)
	{
		CrittersGrabberSharedData.Initialize();
		if (!CrittersGrabberSharedData.triggerCollidersToCheck.Contains(trigger))
		{
			return;
		}
		CrittersGrabberSharedData.triggerCollidersToCheck.Remove(trigger);
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00007EFB File Offset: 0x000060FB
	public static void AddActorGrabber(CrittersActorGrabber grabber)
	{
		CrittersGrabberSharedData.Initialize();
		if (CrittersGrabberSharedData.actorGrabbers.Contains(grabber))
		{
			return;
		}
		CrittersGrabberSharedData.actorGrabbers.Add(grabber);
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00007F1B File Offset: 0x0000611B
	public static void RemoveActorGrabber(CrittersActorGrabber grabber)
	{
		CrittersGrabberSharedData.Initialize();
		if (!CrittersGrabberSharedData.actorGrabbers.Contains(grabber))
		{
			return;
		}
		CrittersGrabberSharedData.actorGrabbers.Remove(grabber);
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00007F3C File Offset: 0x0000613C
	public static void DisableEmptyGrabberJoints()
	{
		CrittersGrabberSharedData.Initialize();
		for (int i = 0; i < CrittersGrabberSharedData.actorGrabbers.Count; i++)
		{
			if (CrittersGrabberSharedData.actorGrabbers[i].grabber != null && CrittersGrabberSharedData.actorGrabbers[i].actorsStillPresent.Count == 0)
			{
				for (int j = 0; j < CrittersGrabberSharedData.actorGrabbers[i].grabber.grabbedActors.Count; j++)
				{
					CrittersGrabberSharedData.actorGrabbers[i].grabber.grabbedActors[j].DisconnectJoint();
				}
			}
		}
	}

	// Token: 0x04000146 RID: 326
	public static List<CrittersActor> enteredCritterActor;

	// Token: 0x04000147 RID: 327
	public static List<CapsuleCollider> triggerCollidersToCheck;

	// Token: 0x04000148 RID: 328
	public static List<CrittersActor> heldActor;

	// Token: 0x04000149 RID: 329
	public static List<CrittersActorGrabber> actorGrabbers;

	// Token: 0x0400014A RID: 330
	private static bool initialized;
}
