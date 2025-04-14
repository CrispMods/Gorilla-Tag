using System;
using System.Collections;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000033 RID: 51
[DefaultExecutionOrder(9999)]
public class CrittersActorGrabber : MonoBehaviour
{
	// Token: 0x060000EE RID: 238 RVA: 0x00007284 File Offset: 0x00005484
	private void Awake()
	{
		if (this.grabber == null)
		{
			this.grabber = base.GetComponent<CrittersGrabber>();
		}
		this.vibrationStartDistance *= this.vibrationStartDistance;
		this.vibrationEndDistance *= this.vibrationEndDistance;
		this.rb = base.GetComponent<Rigidbody>();
		CrittersGrabberSharedData.AddActorGrabber(this);
		this.actorsStillPresent = new List<CrittersActor>();
	}

	// Token: 0x060000EF RID: 239 RVA: 0x000072F0 File Offset: 0x000054F0
	private void LateUpdate()
	{
		if (this.isLeft)
		{
			this.NewJointMethod();
		}
		CrittersRigActorSetup crittersRigActorSetup;
		if ((this.grabber == null || !this.grabber.gameObject.activeSelf || this.grabber.rigPlayerId != PhotonNetwork.LocalPlayer.ActorNumber) && CrittersManager.instance.rigSetupByRig.TryGetValue(GorillaTagger.Instance.offlineVRRig, out crittersRigActorSetup))
		{
			int num;
			if (this.isLeft)
			{
				num = 1;
			}
			else
			{
				num = 3;
			}
			this.grabber = crittersRigActorSetup.rigActors[num].location.GetComponentInChildren<CrittersGrabber>();
			if (this.grabber != null)
			{
				this.grabber.isLeft = this.isLeft;
			}
		}
		if (this.grabber != null)
		{
			for (int i = 0; i < this.grabber.grabbedActors.Count; i++)
			{
				if (this.grabber.grabbedActors[i].localCanStore)
				{
					this.grabber.grabbedActors[i].CheckStorable();
				}
			}
		}
		if (this.transformToFollow != null)
		{
			base.transform.position = this.transformToFollow.position;
			base.transform.rotation = this.transformToFollow.rotation;
		}
		if (this.grabber == null)
		{
			return;
		}
		this.VerifyExistingGrab();
		this.validGrabTarget = this.FindGrabTargets();
		bool flag;
		if (this.isLeft)
		{
			flag = ControllerInputPoller.instance.leftGrab;
		}
		else
		{
			flag = ControllerInputPoller.instance.rightGrab;
		}
		bool flag2 = this.isLeft ? (EquipmentInteractor.instance.leftHandHeldEquipment != null) : (EquipmentInteractor.instance.rightHandHeldEquipment != null);
		if (flag2)
		{
			flag = false;
		}
		if (!flag2)
		{
			if (this.validGrabTarget.IsNotNull())
			{
				if (this.validGrabTarget != this.lastHover)
				{
					this.lastHover = this.validGrabTarget;
					this.DoHover();
				}
			}
			else
			{
				this.lastHover = null;
			}
		}
		if (!this.isGrabbing && flag)
		{
			this.isGrabbing = true;
			this.remainingGrabDuration = this.grabDuration;
		}
		else if (this.isGrabbing)
		{
			if (!flag)
			{
				this.isGrabbing = false;
				this.DoRelease();
			}
			else if (this.queuedGrab != null)
			{
				this.CheckApplyQueuedGrab();
			}
		}
		if (this.isGrabbing && this.remainingGrabDuration > 0f)
		{
			this.remainingGrabDuration -= Time.deltaTime;
			this.DoGrab();
		}
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x0000756C File Offset: 0x0000576C
	private CrittersActor FindGrabTargets()
	{
		int num = Physics.OverlapSphereNonAlloc(base.transform.position, this.grabRadius, this.colliders, CrittersManager.instance.objectLayers | CrittersManager.instance.containerLayer);
		float num2 = 10000f;
		Collider collider = null;
		if (num <= 0)
		{
			return null;
		}
		for (int i = 0; i < num; i++)
		{
			Rigidbody attachedRigidbody = this.colliders[i].attachedRigidbody;
			if (!(attachedRigidbody == null))
			{
				CrittersActor component = attachedRigidbody.GetComponent<CrittersActor>();
				CrittersActor crittersActor;
				if (!(component == null) && (!(component is CrittersBag) || !CrittersManager.instance.actorById.TryGetValue(component.parentActorId, out crittersActor) || !(crittersActor is CrittersAttachPoint) || (crittersActor as CrittersAttachPoint).rigPlayerId != PhotonNetwork.LocalPlayer.ActorNumber || (crittersActor as CrittersAttachPoint).anchorLocation != CrittersAttachPoint.AnchoredLocationTypes.Arm || (crittersActor as CrittersAttachPoint).isLeft != this.isLeft) && component.usesRB && component.CanBeGrabbed(this.grabber))
				{
					float sqrMagnitude = (this.colliders[i].attachedRigidbody.position - base.transform.position).sqrMagnitude;
					if (sqrMagnitude < num2)
					{
						num2 = sqrMagnitude;
						collider = this.colliders[i];
					}
				}
			}
		}
		if (collider == null)
		{
			return null;
		}
		return collider.attachedRigidbody.GetComponent<CrittersActor>();
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x000076E5 File Offset: 0x000058E5
	private void DoHover()
	{
		this.validGrabTarget.OnHover(this.isLeft);
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x000076F8 File Offset: 0x000058F8
	private void DoGrab()
	{
		if (this.validGrabTarget.IsNull())
		{
			return;
		}
		this.grabber.grabbing = true;
		if (this.isLeft)
		{
			EquipmentInteractor.instance.disableLeftGrab = true;
		}
		else
		{
			EquipmentInteractor.instance.disableRightGrab = true;
		}
		this.isHandGrabbingDisabled = true;
		this.remainingGrabDuration = 0f;
		Vector3 localOffset = this.grabber.transform.InverseTransformPoint(this.validGrabTarget.transform.position);
		Quaternion localRotation = this.grabber.transform.InverseTransformRotation(this.validGrabTarget.transform.rotation);
		if (this.validGrabTarget.IsCurrentlyAttachedToBag())
		{
			this.queuedGrab = this.validGrabTarget;
			this.queuedRelativeGrabOffset = localOffset;
			this.queuedRelativeGrabRotation = localRotation;
			return;
		}
		if (this.validGrabTarget.AllowGrabbingActor(this.grabber))
		{
			this.ApplyGrab(this.validGrabTarget, localRotation, localOffset);
		}
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x000077E0 File Offset: 0x000059E0
	private void ApplyGrab(CrittersActor grabTarget, Quaternion localRotation, Vector3 localOffset)
	{
		if (grabTarget.AttemptSetEquipmentStorable())
		{
			this.RemoveGrabberPhysicsTrigger();
			this.AddGrabberPhysicsTrigger(grabTarget);
		}
		grabTarget.GrabbedBy(this.grabber, true, localRotation, localOffset, false);
		this.grabber.grabbedActors.Add(grabTarget);
		this.localGrabOffset = localOffset;
		CrittersPawn crittersPawn = grabTarget as CrittersPawn;
		if (crittersPawn.IsNotNull())
		{
			this.PlayHaptics(crittersPawn.grabbedHaptics, crittersPawn.grabbedHapticsStrength);
		}
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x0000784C File Offset: 0x00005A4C
	private void DoRelease()
	{
		this.queuedGrab = null;
		this.grabber.grabbing = false;
		this.StopHaptics();
		for (int i = this.grabber.grabbedActors.Count - 1; i >= 0; i--)
		{
			CrittersActor crittersActor = this.grabber.grabbedActors[i];
			float magnitude = this.estimator.linearVelocity.magnitude;
			float d = magnitude + Mathf.Max(0f, magnitude - CrittersManager.instance.fastThrowThreshold) * CrittersManager.instance.fastThrowMultiplier;
			crittersActor.Released(true, crittersActor.transform.rotation, crittersActor.transform.position, this.estimator.linearVelocity.normalized * d, this.estimator.angularVelocity);
			if (i < this.grabber.grabbedActors.Count)
			{
				this.grabber.grabbedActors.RemoveAt(i);
			}
		}
		this.RemoveGrabberPhysicsTrigger();
		if (this.isHandGrabbingDisabled)
		{
			this.isHandGrabbingDisabled = false;
			if (this.isLeft)
			{
				EquipmentInteractor.instance.disableLeftGrab = false;
				return;
			}
			EquipmentInteractor.instance.disableRightGrab = false;
		}
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00007984 File Offset: 0x00005B84
	private void CheckApplyQueuedGrab()
	{
		if (Vector3.Magnitude(this.grabber.transform.InverseTransformPoint(this.queuedGrab.transform.position) - this.queuedRelativeGrabOffset) > this.grabDetachFromBagDist)
		{
			GorillaTagger.Instance.StartVibration(this.isLeft, GorillaTagger.Instance.tapHapticStrength / 4f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
			if (this.queuedGrab.AllowGrabbingActor(this.grabber))
			{
				this.ApplyGrab(this.queuedGrab, this.queuedRelativeGrabRotation, this.queuedRelativeGrabOffset);
			}
			this.queuedGrab = null;
		}
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00007A2C File Offset: 0x00005C2C
	private void VerifyExistingGrab()
	{
		for (int i = this.grabber.grabbedActors.Count - 1; i >= 0; i--)
		{
			CrittersActor crittersActor = this.grabber.grabbedActors[i];
			if (crittersActor.IsNull() || crittersActor.parentActorId != this.grabber.actorId)
			{
				if (this.grabber.IsNotNull())
				{
					this.grabber.grabbedActors.Remove(crittersActor);
				}
				this.RemoveGrabberPhysicsTrigger();
				this.StopHaptics();
			}
		}
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00007AB0 File Offset: 0x00005CB0
	public void PlayHaptics(AudioClip clip, float strength)
	{
		if (clip == null)
		{
			return;
		}
		this.StopHaptics();
		this.playingHaptics = true;
		this.hapticsClip = clip;
		this.hapticsStrength = strength;
		this.hapticsLength = clip.length;
		this.haptics = base.StartCoroutine(this.PlayHapticsOnLoop());
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00007B00 File Offset: 0x00005D00
	public void StopHaptics()
	{
		if (this.playingHaptics)
		{
			this.playingHaptics = false;
			base.StopCoroutine(this.haptics);
			this.haptics = null;
			GorillaTagger.Instance.StopHapticClip(this.isLeft);
		}
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00007B34 File Offset: 0x00005D34
	private IEnumerator PlayHapticsOnLoop()
	{
		for (;;)
		{
			GorillaTagger.Instance.PlayHapticClip(this.isLeft, this.hapticsClip, this.hapticsStrength);
			yield return new WaitForSeconds(this.hapticsLength);
		}
		yield break;
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00007B44 File Offset: 0x00005D44
	private void OnTriggerEnter(Collider other)
	{
		if (other.attachedRigidbody == null)
		{
			return;
		}
		CrittersActor component = other.attachedRigidbody.GetComponent<CrittersActor>();
		CrittersActor softJoint;
		if (!this.DoesActorActivateJoint(component, out softJoint))
		{
			return;
		}
		this.ActivateJoints(component, softJoint);
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00007B80 File Offset: 0x00005D80
	private void ActivateJoints(CrittersActor rigidJoint, CrittersActor softJoint)
	{
		softJoint.SetJointSoft(this.grabber.rb);
		if (rigidJoint.parentActorId != -1)
		{
			rigidJoint.SetJointRigid(CrittersManager.instance.actorById[rigidJoint.parentActorId].rb);
		}
		CrittersGrabberSharedData.AddEnteredActor(rigidJoint);
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00007BD0 File Offset: 0x00005DD0
	private bool DoesActorActivateJoint(CrittersActor potentialBagActor, out CrittersActor heldStorableActor)
	{
		heldStorableActor = null;
		for (int i = 0; i < this.grabber.grabbedActors.Count; i++)
		{
			if (this.grabber.grabbedActors[i].localCanStore)
			{
				heldStorableActor = this.grabber.grabbedActors[i];
			}
		}
		CrittersActor crittersActor;
		return !(heldStorableActor == null) && potentialBagActor is CrittersBag && (!CrittersManager.instance.actorById.TryGetValue(potentialBagActor.parentActorId, out crittersActor) || !(crittersActor is CrittersAttachPoint) || (crittersActor as CrittersAttachPoint).rigPlayerId != PhotonNetwork.LocalPlayer.ActorNumber || (crittersActor as CrittersAttachPoint).anchorLocation != CrittersAttachPoint.AnchoredLocationTypes.Arm || (crittersActor as CrittersAttachPoint).isLeft != this.isLeft);
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00007C9C File Offset: 0x00005E9C
	private void AddGrabberPhysicsTrigger(CrittersActor actor)
	{
		CapsuleCollider capsuleCollider = CrittersManager.DuplicateCapsuleCollider(base.transform, actor.equipmentStoreTriggerCollider);
		capsuleCollider.isTrigger = true;
		this.triggerCollider = capsuleCollider;
		CrittersGrabberSharedData.AddTrigger(this.triggerCollider);
		this.rb.includeLayers = CrittersManager.instance.containerLayer;
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00007CEC File Offset: 0x00005EEC
	private void RemoveGrabberPhysicsTrigger()
	{
		if (this.triggerCollider != null)
		{
			CrittersGrabberSharedData.RemoveTrigger(this.triggerCollider);
			Object.Destroy(this.triggerCollider.gameObject);
		}
		this.triggerCollider = null;
		this.rb.includeLayers = 0;
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00007D3C File Offset: 0x00005F3C
	private void NewJointMethod()
	{
		if (CrittersGrabberSharedData.triggerCollidersToCheck.Count == 0 && CrittersGrabberSharedData.enteredCritterActor.Count == 0)
		{
			return;
		}
		for (int i = 0; i < CrittersGrabberSharedData.actorGrabbers.Count; i++)
		{
			CrittersGrabberSharedData.actorGrabbers[i].actorsStillPresent.Clear();
			CapsuleCollider capsuleCollider = CrittersGrabberSharedData.actorGrabbers[i].triggerCollider;
			if (!(capsuleCollider == null))
			{
				Vector3 b = capsuleCollider.transform.up * MathF.Max(0f, capsuleCollider.height / 2f - capsuleCollider.radius);
				int num = Physics.OverlapCapsuleNonAlloc(capsuleCollider.transform.position + b, capsuleCollider.transform.position - b, capsuleCollider.radius, this.colliders, CrittersManager.instance.containerLayer, QueryTriggerInteraction.Collide);
				if (num != 0)
				{
					for (int j = 0; j < num; j++)
					{
						Rigidbody attachedRigidbody = this.colliders[j].attachedRigidbody;
						if (!(attachedRigidbody == null))
						{
							CrittersActor component = attachedRigidbody.GetComponent<CrittersActor>();
							if (!(component == null) && !CrittersGrabberSharedData.actorGrabbers[i].actorsStillPresent.Contains(component))
							{
								CrittersGrabberSharedData.actorGrabbers[i].actorsStillPresent.Add(component);
							}
						}
					}
				}
			}
		}
		for (int k = 0; k < CrittersGrabberSharedData.actorGrabbers.Count; k++)
		{
			CrittersActorGrabber crittersActorGrabber = CrittersGrabberSharedData.actorGrabbers[k];
			for (int l = 0; l < CrittersGrabberSharedData.actorGrabbers[k].actorsStillPresent.Count; l++)
			{
				CrittersActor crittersActor = CrittersGrabberSharedData.actorGrabbers[k].actorsStillPresent[l];
				CrittersActor softJoint;
				if (crittersActorGrabber.DoesActorActivateJoint(crittersActor, out softJoint))
				{
					crittersActorGrabber.ActivateJoints(crittersActor, softJoint);
				}
			}
		}
		for (int m = CrittersGrabberSharedData.enteredCritterActor.Count - 1; m >= 0; m--)
		{
			CrittersActor crittersActor2 = CrittersGrabberSharedData.enteredCritterActor[m];
			bool flag = false;
			for (int n = 0; n < CrittersGrabberSharedData.actorGrabbers.Count; n++)
			{
				flag |= CrittersGrabberSharedData.actorGrabbers[n].actorsStillPresent.Contains(crittersActor2);
			}
			if (!flag)
			{
				CrittersGrabberSharedData.RemoveEnteredActor(crittersActor2);
				crittersActor2.DisconnectJoint();
			}
		}
		CrittersGrabberSharedData.DisableEmptyGrabberJoints();
	}

	// Token: 0x04000126 RID: 294
	public bool isGrabbing;

	// Token: 0x04000127 RID: 295
	public Collider[] colliders = new Collider[50];

	// Token: 0x04000128 RID: 296
	public bool isLeft;

	// Token: 0x04000129 RID: 297
	public float grabRadius = 0.05f;

	// Token: 0x0400012A RID: 298
	public float grabBreakRadius = 0.15f;

	// Token: 0x0400012B RID: 299
	private float grabDetachFromBagDist = 0.05f;

	// Token: 0x0400012C RID: 300
	public Transform transformToFollow;

	// Token: 0x0400012D RID: 301
	public GorillaVelocityEstimator estimator;

	// Token: 0x0400012E RID: 302
	public CrittersGrabber grabber;

	// Token: 0x0400012F RID: 303
	public float vibrationStartDistance;

	// Token: 0x04000130 RID: 304
	public float vibrationEndDistance;

	// Token: 0x04000131 RID: 305
	public CrittersActorGrabber otherHand;

	// Token: 0x04000132 RID: 306
	private bool isHandGrabbingDisabled;

	// Token: 0x04000133 RID: 307
	private float grabDuration = 0.3f;

	// Token: 0x04000134 RID: 308
	private float remainingGrabDuration;

	// Token: 0x04000135 RID: 309
	private bool playingHaptics;

	// Token: 0x04000136 RID: 310
	private AudioClip hapticsClip;

	// Token: 0x04000137 RID: 311
	private float hapticsStrength;

	// Token: 0x04000138 RID: 312
	private float hapticsLength;

	// Token: 0x04000139 RID: 313
	private Coroutine haptics;

	// Token: 0x0400013A RID: 314
	public CapsuleCollider triggerCollider;

	// Token: 0x0400013B RID: 315
	private Rigidbody rb;

	// Token: 0x0400013C RID: 316
	private CrittersActor validGrabTarget;

	// Token: 0x0400013D RID: 317
	private CrittersActor lastHover;

	// Token: 0x0400013E RID: 318
	private Vector3 localGrabOffset;

	// Token: 0x0400013F RID: 319
	private CrittersActor queuedGrab;

	// Token: 0x04000140 RID: 320
	private Vector3 queuedRelativeGrabOffset;

	// Token: 0x04000141 RID: 321
	private Quaternion queuedRelativeGrabRotation;

	// Token: 0x04000142 RID: 322
	public List<CrittersActor> actorsStillPresent;
}
