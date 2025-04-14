using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace GorillaLocomotion.Climbing
{
	// Token: 0x02000B66 RID: 2918
	public class GorillaHandClimber : MonoBehaviour
	{
		// Token: 0x060048FD RID: 18685 RVA: 0x00162BFC File Offset: 0x00160DFC
		private void Awake()
		{
			this.col = base.GetComponent<Collider>();
		}

		// Token: 0x060048FE RID: 18686 RVA: 0x00162C0C File Offset: 0x00160E0C
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

		// Token: 0x060048FF RID: 18687 RVA: 0x00162D7C File Offset: 0x00160F7C
		public void SetCanRelease(bool canRelease)
		{
			this.canRelease = canRelease;
		}

		// Token: 0x06004900 RID: 18688 RVA: 0x00162D88 File Offset: 0x00160F88
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

		// Token: 0x06004901 RID: 18689 RVA: 0x00162E88 File Offset: 0x00161088
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

		// Token: 0x06004902 RID: 18690 RVA: 0x00162EC4 File Offset: 0x001610C4
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

		// Token: 0x06004903 RID: 18691 RVA: 0x00162F00 File Offset: 0x00161100
		public void ForceStopClimbing(bool startingNewClimb = false, bool doDontReclimb = false)
		{
			this.player.EndClimbing(this, startingNewClimb, doDontReclimb);
		}

		// Token: 0x04004BAD RID: 19373
		[SerializeField]
		private GTPlayer player;

		// Token: 0x04004BAE RID: 19374
		[SerializeField]
		private EquipmentInteractor equipmentInteractor;

		// Token: 0x04004BAF RID: 19375
		private List<GorillaClimbable> potentialClimbables = new List<GorillaClimbable>();

		// Token: 0x04004BB0 RID: 19376
		[Header("Non-hand input should have the component disabled")]
		public XRNode xrNode = XRNode.LeftHand;

		// Token: 0x04004BB1 RID: 19377
		[NonSerialized]
		public bool isClimbing;

		// Token: 0x04004BB2 RID: 19378
		[NonSerialized]
		public bool queuedToBecomeValidToGrabAgain;

		// Token: 0x04004BB3 RID: 19379
		[NonSerialized]
		public GorillaClimbable dontReclimbLast;

		// Token: 0x04004BB4 RID: 19380
		[NonSerialized]
		public Vector3 lastAutoReleasePos = Vector3.zero;

		// Token: 0x04004BB5 RID: 19381
		public Transform handRoot;

		// Token: 0x04004BB6 RID: 19382
		private const float DIST_FOR_CLEAR_RELEASE = 0.35f;

		// Token: 0x04004BB7 RID: 19383
		private const float DIST_FOR_GRAB = 0.15f;

		// Token: 0x04004BB8 RID: 19384
		private Collider col;

		// Token: 0x04004BB9 RID: 19385
		private bool canRelease = true;
	}
}
