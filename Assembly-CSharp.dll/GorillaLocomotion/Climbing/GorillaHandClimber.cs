﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace GorillaLocomotion.Climbing
{
	// Token: 0x02000B69 RID: 2921
	public class GorillaHandClimber : MonoBehaviour
	{
		// Token: 0x06004909 RID: 18697 RVA: 0x0005EAEA File Offset: 0x0005CCEA
		private void Awake()
		{
			this.col = base.GetComponent<Collider>();
		}

		// Token: 0x0600490A RID: 18698 RVA: 0x00196F88 File Offset: 0x00195188
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

		// Token: 0x0600490B RID: 18699 RVA: 0x0005EAF8 File Offset: 0x0005CCF8
		public void SetCanRelease(bool canRelease)
		{
			this.canRelease = canRelease;
		}

		// Token: 0x0600490C RID: 18700 RVA: 0x001970F8 File Offset: 0x001952F8
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

		// Token: 0x0600490D RID: 18701 RVA: 0x001971F8 File Offset: 0x001953F8
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

		// Token: 0x0600490E RID: 18702 RVA: 0x00197234 File Offset: 0x00195434
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

		// Token: 0x0600490F RID: 18703 RVA: 0x0005EB01 File Offset: 0x0005CD01
		public void ForceStopClimbing(bool startingNewClimb = false, bool doDontReclimb = false)
		{
			this.player.EndClimbing(this, startingNewClimb, doDontReclimb);
		}

		// Token: 0x04004BBF RID: 19391
		[SerializeField]
		private GTPlayer player;

		// Token: 0x04004BC0 RID: 19392
		[SerializeField]
		private EquipmentInteractor equipmentInteractor;

		// Token: 0x04004BC1 RID: 19393
		private List<GorillaClimbable> potentialClimbables = new List<GorillaClimbable>();

		// Token: 0x04004BC2 RID: 19394
		[Header("Non-hand input should have the component disabled")]
		public XRNode xrNode = XRNode.LeftHand;

		// Token: 0x04004BC3 RID: 19395
		[NonSerialized]
		public bool isClimbing;

		// Token: 0x04004BC4 RID: 19396
		[NonSerialized]
		public bool queuedToBecomeValidToGrabAgain;

		// Token: 0x04004BC5 RID: 19397
		[NonSerialized]
		public GorillaClimbable dontReclimbLast;

		// Token: 0x04004BC6 RID: 19398
		[NonSerialized]
		public Vector3 lastAutoReleasePos = Vector3.zero;

		// Token: 0x04004BC7 RID: 19399
		public Transform handRoot;

		// Token: 0x04004BC8 RID: 19400
		private const float DIST_FOR_CLEAR_RELEASE = 0.35f;

		// Token: 0x04004BC9 RID: 19401
		private const float DIST_FOR_GRAB = 0.15f;

		// Token: 0x04004BCA RID: 19402
		private Collider col;

		// Token: 0x04004BCB RID: 19403
		private bool canRelease = true;
	}
}
