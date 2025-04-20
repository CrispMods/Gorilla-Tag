using System;
using GorillaExtensions;
using GorillaTag.CosmeticSystem;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C69 RID: 3177
	public class DistanceCheckerCosmetic : MonoBehaviour, ISpawnable
	{
		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06004F7D RID: 20349 RVA: 0x00063EA1 File Offset: 0x000620A1
		// (set) Token: 0x06004F7E RID: 20350 RVA: 0x00063EA9 File Offset: 0x000620A9
		public bool IsSpawned { get; set; }

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06004F7F RID: 20351 RVA: 0x00063EB2 File Offset: 0x000620B2
		// (set) Token: 0x06004F80 RID: 20352 RVA: 0x00063EBA File Offset: 0x000620BA
		public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x06004F81 RID: 20353 RVA: 0x00063EC3 File Offset: 0x000620C3
		public void OnSpawn(VRRig rig)
		{
			this.myRig = rig;
		}

		// Token: 0x06004F82 RID: 20354 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnDespawn()
		{
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x00063ECC File Offset: 0x000620CC
		private void OnEnable()
		{
			this.currentState = DistanceCheckerCosmetic.State.None;
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x00063ED5 File Offset: 0x000620D5
		private void Update()
		{
			this.UpdateDistance();
		}

		// Token: 0x06004F85 RID: 20357 RVA: 0x00063EDD File Offset: 0x000620DD
		private bool IsBelowThreshold(Vector3 distance)
		{
			return distance.IsShorterThan(this.distanceThreshold);
		}

		// Token: 0x06004F86 RID: 20358 RVA: 0x00063EF0 File Offset: 0x000620F0
		private bool IsAboveThreshold(Vector3 distance)
		{
			return distance.IsLongerThan(this.distanceThreshold);
		}

		// Token: 0x06004F87 RID: 20359 RVA: 0x001B7DD4 File Offset: 0x001B5FD4
		private void UpdateDistance()
		{
			bool flag = true;
			bool flag2 = false;
			switch (this.distanceTo)
			{
			case DistanceCheckerCosmetic.DistanceCondition.Owner:
			{
				Vector3 distance = this.myRig.transform.position - this.distanceFrom.position;
				if (this.IsBelowThreshold(distance))
				{
					this.UpdateState(DistanceCheckerCosmetic.State.BelowThreshold);
					return;
				}
				if (this.IsAboveThreshold(distance))
				{
					this.UpdateState(DistanceCheckerCosmetic.State.AboveThreshold);
				}
				break;
			}
			case DistanceCheckerCosmetic.DistanceCondition.Others:
				foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
				{
					if (!(vrrig == this.myRig))
					{
						Vector3 distance2 = vrrig.transform.position - this.distanceFrom.position;
						flag2 = this.IsBelowThreshold(distance2);
						if (flag2)
						{
							this.UpdateState(DistanceCheckerCosmetic.State.BelowThreshold);
							break;
						}
						flag = this.IsAboveThreshold(distance2);
					}
				}
				if (flag && !flag2)
				{
					this.UpdateState(DistanceCheckerCosmetic.State.AboveThreshold);
					return;
				}
				break;
			case DistanceCheckerCosmetic.DistanceCondition.Everyone:
				foreach (VRRig vrrig2 in GorillaParent.instance.vrrigs)
				{
					Vector3 distance3 = vrrig2.transform.position - this.distanceFrom.position;
					flag2 = this.IsBelowThreshold(distance3);
					if (flag2)
					{
						this.UpdateState(DistanceCheckerCosmetic.State.BelowThreshold);
						break;
					}
					flag = this.IsAboveThreshold(distance3);
				}
				if (flag && !flag2)
				{
					this.UpdateState(DistanceCheckerCosmetic.State.AboveThreshold);
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06004F88 RID: 20360 RVA: 0x001B7F74 File Offset: 0x001B6174
		private void UpdateState(DistanceCheckerCosmetic.State newState)
		{
			if (this.currentState == newState)
			{
				return;
			}
			this.currentState = newState;
			if (this.currentState != DistanceCheckerCosmetic.State.AboveThreshold)
			{
				if (this.currentState == DistanceCheckerCosmetic.State.BelowThreshold)
				{
					UnityEvent unityEvent = this.onOneIsBelowThreshold;
					if (unityEvent == null)
					{
						return;
					}
					unityEvent.Invoke();
				}
				return;
			}
			UnityEvent unityEvent2 = this.onAllAreAboveThreshold;
			if (unityEvent2 == null)
			{
				return;
			}
			unityEvent2.Invoke();
		}

		// Token: 0x0400525D RID: 21085
		[SerializeField]
		private Transform distanceFrom;

		// Token: 0x0400525E RID: 21086
		[SerializeField]
		private DistanceCheckerCosmetic.DistanceCondition distanceTo;

		// Token: 0x0400525F RID: 21087
		[Tooltip("Receive events when above or below this distance")]
		[SerializeField]
		private float distanceThreshold;

		// Token: 0x04005260 RID: 21088
		public UnityEvent onOneIsBelowThreshold;

		// Token: 0x04005261 RID: 21089
		public UnityEvent onAllAreAboveThreshold;

		// Token: 0x04005262 RID: 21090
		private VRRig myRig;

		// Token: 0x04005263 RID: 21091
		private DistanceCheckerCosmetic.State currentState;

		// Token: 0x02000C6A RID: 3178
		private enum State
		{
			// Token: 0x04005267 RID: 21095
			AboveThreshold,
			// Token: 0x04005268 RID: 21096
			BelowThreshold,
			// Token: 0x04005269 RID: 21097
			None
		}

		// Token: 0x02000C6B RID: 3179
		private enum DistanceCondition
		{
			// Token: 0x0400526B RID: 21099
			None,
			// Token: 0x0400526C RID: 21100
			Owner,
			// Token: 0x0400526D RID: 21101
			Others,
			// Token: 0x0400526E RID: 21102
			Everyone
		}
	}
}
