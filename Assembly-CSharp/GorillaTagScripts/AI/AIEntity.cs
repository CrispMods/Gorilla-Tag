using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GorillaTagScripts.AI
{
	// Token: 0x02000A3C RID: 2620
	public class AIEntity : MonoBehaviour
	{
		// Token: 0x060041CA RID: 16842 RVA: 0x00173744 File Offset: 0x00171944
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

		// Token: 0x060041CB RID: 16843 RVA: 0x001737AC File Offset: 0x001719AC
		protected void ChooseRandomTarget()
		{
			int randomTarget = UnityEngine.Random.Range(0, GorillaParent.instance.vrrigs.Count);
			int num = GorillaParent.instance.vrrigs.FindIndex((VRRig x) => x.creator != null && x.creator == GorillaParent.instance.vrrigs[randomTarget].creator);
			if (num == -1)
			{
				num = UnityEngine.Random.Range(0, GorillaParent.instance.vrrigs.Count);
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

		// Token: 0x060041CC RID: 16844 RVA: 0x0017389C File Offset: 0x00171A9C
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

		// Token: 0x040042B3 RID: 17075
		public GameObject waypointsContainer;

		// Token: 0x040042B4 RID: 17076
		public Transform circleCenter;

		// Token: 0x040042B5 RID: 17077
		public float circleRadius;

		// Token: 0x040042B6 RID: 17078
		public float angularSpeed;

		// Token: 0x040042B7 RID: 17079
		public float patrolSpeed;

		// Token: 0x040042B8 RID: 17080
		public float fleeSpeed;

		// Token: 0x040042B9 RID: 17081
		public NavMeshAgent navMeshAgent;

		// Token: 0x040042BA RID: 17082
		public Animator animator;

		// Token: 0x040042BB RID: 17083
		public float fleeRang;

		// Token: 0x040042BC RID: 17084
		public float fleeSpeedMult;

		// Token: 0x040042BD RID: 17085
		public float minChaseRange;

		// Token: 0x040042BE RID: 17086
		public float attackDistance;

		// Token: 0x040042BF RID: 17087
		public float navMeshSampleRange = 5f;

		// Token: 0x040042C0 RID: 17088
		internal readonly List<Transform> waypoints = new List<Transform>();

		// Token: 0x040042C1 RID: 17089
		internal float defaultSpeed;

		// Token: 0x040042C2 RID: 17090
		public Transform followTarget;

		// Token: 0x040042C3 RID: 17091
		public NetPlayer targetPlayer;

		// Token: 0x040042C4 RID: 17092
		public bool targetIsOnNavMesh;
	}
}
