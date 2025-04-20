using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace GorillaLocomotion.Climbing
{
	// Token: 0x02000B93 RID: 2963
	public class GorillaHandClimber : MonoBehaviour
	{
		// Token: 0x06004A48 RID: 19016 RVA: 0x00060522 File Offset: 0x0005E722
		private void Awake()
		{
			this.col = base.GetComponent<Collider>();
		}

		// Token: 0x06004A49 RID: 19017 RVA: 0x0019DFA0 File Offset: 0x0019C1A0
		private void Update()
		{
			for (int i = this.potentialClimbables.Count - 1; i >= 0; i--)
			{
				GorillaClimbable gorillaClimbable = this.potentialClimbables[i];
				if (gorillaClimbable == null || !gorillaClimbable.isActiveAndEnabled)
				{
					this.potentialClimbables.RemoveAt(i);
				}
				else if (gorillaClimbable.climbOnlyWhileSmall && this.player.scale > 0.99f)
				{
					this.potentialClimbables.RemoveAt(i);
				}
			}
			bool grab = ControllerInputPoller.GetGrab(this.xrNode);
			bool grabRelease = ControllerInputPoller.GetGrabRelease(this.xrNode);
			if (!this.isClimbing)
			{
				if (this.queuedToBecomeValidToGrabAgain && Vector3.Distance(this.lastAutoReleasePos, this.handRoot.localPosition) >= 0.35f)
				{
					this.queuedToBecomeValidToGrabAgain = false;
				}
				if (grabRelease)
				{
					this.queuedToBecomeValidToGrabAgain = false;
					this.dontReclimbLast = null;
				}
				GorillaClimbable closestClimbable = this.GetClosestClimbable();
				if (!this.queuedToBecomeValidToGrabAgain && closestClimbable && grab && !this.equipmentInteractor.GetIsHolding(this.xrNode) && closestClimbable != this.dontReclimbLast && !this.player.inOverlay)
				{
					GorillaClimbableRef gorillaClimbableRef = closestClimbable as GorillaClimbableRef;
					if (gorillaClimbableRef != null)
					{
						this.player.BeginClimbing(gorillaClimbableRef.climb, this, gorillaClimbableRef);
						return;
					}
					this.player.BeginClimbing(closestClimbable, this, null);
					return;
				}
			}
			else if (grabRelease && this.canRelease)
			{
				this.player.EndClimbing(this, false, false);
			}
		}

		// Token: 0x06004A4A RID: 19018 RVA: 0x00060530 File Offset: 0x0005E730
		public void SetCanRelease(bool canRelease)
		{
			this.canRelease = canRelease;
		}

		// Token: 0x06004A4B RID: 19019 RVA: 0x0019E110 File Offset: 0x0019C310
		public GorillaClimbable GetClosestClimbable()
		{
			if (this.potentialClimbables.Count == 0)
			{
				return null;
			}
			if (this.potentialClimbables.Count == 1)
			{
				return this.potentialClimbables[0];
			}
			Vector3 position = base.transform.position;
			Bounds bounds = this.col.bounds;
			float num = 0.15f;
			GorillaClimbable result = null;
			foreach (GorillaClimbable gorillaClimbable in this.potentialClimbables)
			{
				float num2;
				if (gorillaClimbable.colliderCache)
				{
					if (!bounds.Intersects(gorillaClimbable.colliderCache.bounds))
					{
						continue;
					}
					Vector3 b = gorillaClimbable.colliderCache.ClosestPoint(position);
					num2 = Vector3.Distance(position, b);
				}
				else
				{
					num2 = Vector3.Distance(position, gorillaClimbable.transform.position);
				}
				if (num2 < num)
				{
					result = gorillaClimbable;
					num = num2;
				}
			}
			return result;
		}

		// Token: 0x06004A4C RID: 19020 RVA: 0x0019E210 File Offset: 0x0019C410
		private void OnTriggerEnter(Collider other)
		{
			GorillaClimbable item;
			if (other.TryGetComponent<GorillaClimbable>(out item))
			{
				this.potentialClimbables.Add(item);
				return;
			}
			GorillaClimbableRef item2;
			if (other.TryGetComponent<GorillaClimbableRef>(out item2))
			{
				this.potentialClimbables.Add(item2);
			}
		}

		// Token: 0x06004A4D RID: 19021 RVA: 0x0019E24C File Offset: 0x0019C44C
		private void OnTriggerExit(Collider other)
		{
			GorillaClimbable item;
			if (other.TryGetComponent<GorillaClimbable>(out item))
			{
				this.potentialClimbables.Remove(item);
				return;
			}
			GorillaClimbableRef item2;
			if (other.TryGetComponent<GorillaClimbableRef>(out item2))
			{
				this.potentialClimbables.Remove(item2);
			}
		}

		// Token: 0x06004A4E RID: 19022 RVA: 0x00060539 File Offset: 0x0005E739
		public void ForceStopClimbing(bool startingNewClimb = false, bool doDontReclimb = false)
		{
			this.player.EndClimbing(this, startingNewClimb, doDontReclimb);
		}

		// Token: 0x04004CA3 RID: 19619
		[SerializeField]
		private GTPlayer player;

		// Token: 0x04004CA4 RID: 19620
		[SerializeField]
		private EquipmentInteractor equipmentInteractor;

		// Token: 0x04004CA5 RID: 19621
		private List<GorillaClimbable> potentialClimbables = new List<GorillaClimbable>();

		// Token: 0x04004CA6 RID: 19622
		[Header("Non-hand input should have the component disabled")]
		public XRNode xrNode = XRNode.LeftHand;

		// Token: 0x04004CA7 RID: 19623
		[NonSerialized]
		public bool isClimbing;

		// Token: 0x04004CA8 RID: 19624
		[NonSerialized]
		public bool queuedToBecomeValidToGrabAgain;

		// Token: 0x04004CA9 RID: 19625
		[NonSerialized]
		public GorillaClimbable dontReclimbLast;

		// Token: 0x04004CAA RID: 19626
		[NonSerialized]
		public Vector3 lastAutoReleasePos = Vector3.zero;

		// Token: 0x04004CAB RID: 19627
		public Transform handRoot;

		// Token: 0x04004CAC RID: 19628
		private const float DIST_FOR_CLEAR_RELEASE = 0.35f;

		// Token: 0x04004CAD RID: 19629
		private const float DIST_FOR_GRAB = 0.15f;

		// Token: 0x04004CAE RID: 19630
		private Collider col;

		// Token: 0x04004CAF RID: 19631
		private bool canRelease = true;
	}
}
