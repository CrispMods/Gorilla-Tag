using System;
using GorillaExtensions;
using GorillaTag.CosmeticSystem;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C3B RID: 3131
	public class DistanceCheckerCosmetic : MonoBehaviour, ISpawnable
	{
		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06004E29 RID: 20009 RVA: 0x0017FB08 File Offset: 0x0017DD08
		// (set) Token: 0x06004E2A RID: 20010 RVA: 0x0017FB10 File Offset: 0x0017DD10
		public bool IsSpawned { get; set; }

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06004E2B RID: 20011 RVA: 0x0017FB19 File Offset: 0x0017DD19
		// (set) Token: 0x06004E2C RID: 20012 RVA: 0x0017FB21 File Offset: 0x0017DD21
		public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x06004E2D RID: 20013 RVA: 0x0017FB2A File Offset: 0x0017DD2A
		public void OnSpawn(VRRig rig)
		{
			this.myRig = rig;
		}

		// Token: 0x06004E2E RID: 20014 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnDespawn()
		{
		}

		// Token: 0x06004E2F RID: 20015 RVA: 0x0017FB33 File Offset: 0x0017DD33
		private void OnEnable()
		{
			this.currentState = DistanceCheckerCosmetic.State.None;
		}

		// Token: 0x06004E30 RID: 20016 RVA: 0x0017FB3C File Offset: 0x0017DD3C
		private void Update()
		{
			this.UpdateDistance();
		}

		// Token: 0x06004E31 RID: 20017 RVA: 0x0017FB44 File Offset: 0x0017DD44
		private bool IsBelowThreshold(Vector3 distance)
		{
			return distance.IsShorterThan(this.distanceThreshold);
		}

		// Token: 0x06004E32 RID: 20018 RVA: 0x0017FB57 File Offset: 0x0017DD57
		private bool IsAboveThreshold(Vector3 distance)
		{
			return distance.IsLongerThan(this.distanceThreshold);
		}

		// Token: 0x06004E33 RID: 20019 RVA: 0x0017FB6C File Offset: 0x0017DD6C
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

		// Token: 0x06004E34 RID: 20020 RVA: 0x0017FD0C File Offset: 0x0017DF0C
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

		// Token: 0x04005163 RID: 20835
		[SerializeField]
		private Transform distanceFrom;

		// Token: 0x04005164 RID: 20836
		[SerializeField]
		private DistanceCheckerCosmetic.DistanceCondition distanceTo;

		// Token: 0x04005165 RID: 20837
		[Tooltip("Receive events when above or below this distance")]
		[SerializeField]
		private float distanceThreshold;

		// Token: 0x04005166 RID: 20838
		public UnityEvent onOneIsBelowThreshold;

		// Token: 0x04005167 RID: 20839
		public UnityEvent onAllAreAboveThreshold;

		// Token: 0x04005168 RID: 20840
		private VRRig myRig;

		// Token: 0x04005169 RID: 20841
		private DistanceCheckerCosmetic.State currentState;

		// Token: 0x02000C3C RID: 3132
		private enum State
		{
			// Token: 0x0400516D RID: 20845
			AboveThreshold,
			// Token: 0x0400516E RID: 20846
			BelowThreshold,
			// Token: 0x0400516F RID: 20847
			None
		}

		// Token: 0x02000C3D RID: 3133
		private enum DistanceCondition
		{
			// Token: 0x04005171 RID: 20849
			None,
			// Token: 0x04005172 RID: 20850
			Owner,
			// Token: 0x04005173 RID: 20851
			Others,
			// Token: 0x04005174 RID: 20852
			Everyone
		}
	}
}
