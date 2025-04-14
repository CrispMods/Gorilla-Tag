using System;
using GorillaExtensions;
using GorillaTag.CosmeticSystem;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C38 RID: 3128
	public class DistanceCheckerCosmetic : MonoBehaviour, ISpawnable
	{
		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06004E1D RID: 19997 RVA: 0x0017F540 File Offset: 0x0017D740
		// (set) Token: 0x06004E1E RID: 19998 RVA: 0x0017F548 File Offset: 0x0017D748
		public bool IsSpawned { get; set; }

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06004E1F RID: 19999 RVA: 0x0017F551 File Offset: 0x0017D751
		// (set) Token: 0x06004E20 RID: 20000 RVA: 0x0017F559 File Offset: 0x0017D759
		public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x06004E21 RID: 20001 RVA: 0x0017F562 File Offset: 0x0017D762
		public void OnSpawn(VRRig rig)
		{
			this.myRig = rig;
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnDespawn()
		{
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x0017F56B File Offset: 0x0017D76B
		private void OnEnable()
		{
			this.currentState = DistanceCheckerCosmetic.State.None;
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x0017F574 File Offset: 0x0017D774
		private void Update()
		{
			this.UpdateDistance();
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x0017F57C File Offset: 0x0017D77C
		private bool IsBelowThreshold(Vector3 distance)
		{
			return distance.IsShorterThan(this.distanceThreshold);
		}

		// Token: 0x06004E26 RID: 20006 RVA: 0x0017F58F File Offset: 0x0017D78F
		private bool IsAboveThreshold(Vector3 distance)
		{
			return distance.IsLongerThan(this.distanceThreshold);
		}

		// Token: 0x06004E27 RID: 20007 RVA: 0x0017F5A4 File Offset: 0x0017D7A4
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

		// Token: 0x06004E28 RID: 20008 RVA: 0x0017F744 File Offset: 0x0017D944
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

		// Token: 0x04005151 RID: 20817
		[SerializeField]
		private Transform distanceFrom;

		// Token: 0x04005152 RID: 20818
		[SerializeField]
		private DistanceCheckerCosmetic.DistanceCondition distanceTo;

		// Token: 0x04005153 RID: 20819
		[Tooltip("Receive events when above or below this distance")]
		[SerializeField]
		private float distanceThreshold;

		// Token: 0x04005154 RID: 20820
		public UnityEvent onOneIsBelowThreshold;

		// Token: 0x04005155 RID: 20821
		public UnityEvent onAllAreAboveThreshold;

		// Token: 0x04005156 RID: 20822
		private VRRig myRig;

		// Token: 0x04005157 RID: 20823
		private DistanceCheckerCosmetic.State currentState;

		// Token: 0x02000C39 RID: 3129
		private enum State
		{
			// Token: 0x0400515B RID: 20827
			AboveThreshold,
			// Token: 0x0400515C RID: 20828
			BelowThreshold,
			// Token: 0x0400515D RID: 20829
			None
		}

		// Token: 0x02000C3A RID: 3130
		private enum DistanceCondition
		{
			// Token: 0x0400515F RID: 20831
			None,
			// Token: 0x04005160 RID: 20832
			Owner,
			// Token: 0x04005161 RID: 20833
			Others,
			// Token: 0x04005162 RID: 20834
			Everyone
		}
	}
}
