using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000038 RID: 56
public static class CrittersGrabberSharedData
{
	// Token: 0x06000118 RID: 280 RVA: 0x00030FAF File Offset: 0x0002F1AF
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

	// Token: 0x06000119 RID: 281 RVA: 0x00030FE7 File Offset: 0x0002F1E7
	public static void AddEnteredActor(CrittersActor actor)
	{
		CrittersGrabberSharedData.Initialize();
		if (CrittersGrabberSharedData.enteredCritterActor.Contains(actor))
		{
			return;
		}
		CrittersGrabberSharedData.enteredCritterActor.Add(actor);
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00031007 File Offset: 0x0002F207
	public static void RemoveEnteredActor(CrittersActor actor)
	{
		CrittersGrabberSharedData.Initialize();
		if (!CrittersGrabberSharedData.enteredCritterActor.Contains(actor))
		{
			return;
		}
		CrittersGrabberSharedData.enteredCritterActor.Remove(actor);
	}

	// Token: 0x0600011B RID: 283 RVA: 0x00031028 File Offset: 0x0002F228
	public static void AddTrigger(CapsuleCollider trigger)
	{
		CrittersGrabberSharedData.Initialize();
		if (CrittersGrabberSharedData.triggerCollidersToCheck.Contains(trigger))
		{
			return;
		}
		CrittersGrabberSharedData.triggerCollidersToCheck.Add(trigger);
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00031048 File Offset: 0x0002F248
	public static void RemoveTrigger(CapsuleCollider trigger)
	{
		CrittersGrabberSharedData.Initialize();
		if (!CrittersGrabberSharedData.triggerCollidersToCheck.Contains(trigger))
		{
			return;
		}
		CrittersGrabberSharedData.triggerCollidersToCheck.Remove(trigger);
	}

	// Token: 0x0600011D RID: 285 RVA: 0x00031069 File Offset: 0x0002F269
	public static void AddActorGrabber(CrittersActorGrabber grabber)
	{
		CrittersGrabberSharedData.Initialize();
		if (CrittersGrabberSharedData.actorGrabbers.Contains(grabber))
		{
			return;
		}
		CrittersGrabberSharedData.actorGrabbers.Add(grabber);
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00031089 File Offset: 0x0002F289
	public static void RemoveActorGrabber(CrittersActorGrabber grabber)
	{
		CrittersGrabberSharedData.Initialize();
		if (!CrittersGrabberSharedData.actorGrabbers.Contains(grabber))
		{
			return;
		}
		CrittersGrabberSharedData.actorGrabbers.Remove(grabber);
	}

	// Token: 0x0600011F RID: 287 RVA: 0x0006D4BC File Offset: 0x0006B6BC
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

	// Token: 0x0400014F RID: 335
	public static List<CrittersActor> enteredCritterActor;

	// Token: 0x04000150 RID: 336
	public static List<CapsuleCollider> triggerCollidersToCheck;

	// Token: 0x04000151 RID: 337
	public static List<CrittersActor> heldActor;

	// Token: 0x04000152 RID: 338
	public static List<CrittersActorGrabber> actorGrabbers;

	// Token: 0x04000153 RID: 339
	private static bool initialized;
}
