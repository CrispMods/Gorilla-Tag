using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GorillaTagScripts.AI
{
	// Token: 0x02000A0F RID: 2575
	public class AIEntity : MonoBehaviour
	{
		// Token: 0x06004085 RID: 16517 RVA: 0x00132F74 File Offset: 0x00131174
		protected void Awake()
		{
			this.navMeshAgent = base.gameObject.GetComponent<NavMeshAgent>();
			this.animator = base.gameObject.GetComponent<Animator>();
			if (this.waypointsContainer != null)
			{
				foreach (Transform item in this.waypointsContainer.GetComponentsInChildren<Transform>())
				{
					this.waypoints.Add(item);
				}
			}
		}

		// Token: 0x06004086 RID: 16518 RVA: 0x00132FDC File Offset: 0x001311DC
		protected void ChooseRandomTarget()
		{
			int randomTarget = Random.Range(0, GorillaParent.instance.vrrigs.Count);
			int num = GorillaParent.instance.vrrigs.FindIndex((VRRig x) => x.creator != null && x.creator == GorillaParent.instance.vrrigs[randomTarget].creator);
			if (num == -1)
			{
				num = Random.Range(0, GorillaParent.instance.vrrigs.Count);
			}
			if (num < GorillaParent.instance.vrrigs.Count)
			{
				this.targetPlayer = GorillaParent.instance.vrrigs[num].creator;
				this.followTarget = GorillaParent.instance.vrrigs[num].head.rigTarget;
				NavMeshHit navMeshHit;
				this.targetIsOnNavMesh = NavMesh.SamplePosition(this.followTarget.position, out navMeshHit, this.navMeshSampleRange, 1);
				return;
			}
			this.targetPlayer = null;
			this.followTarget = null;
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x001330CC File Offset: 0x001312CC
		protected void ChooseClosestTarget()
		{
			VRRig vrrig = null;
			float num = float.MaxValue;
			foreach (VRRig vrrig2 in GorillaParent.instance.vrrigs)
			{
				if (vrrig2.head != null && !(vrrig2.head.rigTarget == null))
				{
					float sqrMagnitude = (base.transform.position - vrrig2.head.rigTarget.transform.position).sqrMagnitude;
					if (sqrMagnitude < this.minChaseRange * this.minChaseRange && sqrMagnitude < num)
					{
						num = sqrMagnitude;
						vrrig = vrrig2;
					}
				}
			}
			if (vrrig != null)
			{
				this.targetPlayer = vrrig.creator;
				this.followTarget = vrrig.head.rigTarget;
				NavMeshHit navMeshHit;
				this.targetIsOnNavMesh = NavMesh.SamplePosition(this.followTarget.position, out navMeshHit, this.navMeshSampleRange, 1);
				return;
			}
			this.targetPlayer = null;
			this.followTarget = null;
		}

		// Token: 0x040041B9 RID: 16825
		public GameObject waypointsContainer;

		// Token: 0x040041BA RID: 16826
		public Transform circleCenter;

		// Token: 0x040041BB RID: 16827
		public float circleRadius;

		// Token: 0x040041BC RID: 16828
		public float angularSpeed;

		// Token: 0x040041BD RID: 16829
		public float patrolSpeed;

		// Token: 0x040041BE RID: 16830
		public float fleeSpeed;

		// Token: 0x040041BF RID: 16831
		public NavMeshAgent navMeshAgent;

		// Token: 0x040041C0 RID: 16832
		public Animator animator;

		// Token: 0x040041C1 RID: 16833
		public float fleeRang;

		// Token: 0x040041C2 RID: 16834
		public float fleeSpeedMult;

		// Token: 0x040041C3 RID: 16835
		public float minChaseRange;

		// Token: 0x040041C4 RID: 16836
		public float attackDistance;

		// Token: 0x040041C5 RID: 16837
		public float navMeshSampleRange = 5f;

		// Token: 0x040041C6 RID: 16838
		internal readonly List<Transform> waypoints = new List<Transform>();

		// Token: 0x040041C7 RID: 16839
		internal float defaultSpeed;

		// Token: 0x040041C8 RID: 16840
		public Transform followTarget;

		// Token: 0x040041C9 RID: 16841
		public NetPlayer targetPlayer;

		// Token: 0x040041CA RID: 16842
		public bool targetIsOnNavMesh;
	}
}
