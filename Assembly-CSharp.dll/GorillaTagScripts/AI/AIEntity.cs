using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GorillaTagScripts.AI
{
	// Token: 0x02000A12 RID: 2578
	public class AIEntity : MonoBehaviour
	{
		// Token: 0x06004091 RID: 16529 RVA: 0x0016C8C0 File Offset: 0x0016AAC0
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

		// Token: 0x06004092 RID: 16530 RVA: 0x0016C928 File Offset: 0x0016AB28
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

		// Token: 0x06004093 RID: 16531 RVA: 0x0016CA18 File Offset: 0x0016AC18
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

		// Token: 0x040041CB RID: 16843
		public GameObject waypointsContainer;

		// Token: 0x040041CC RID: 16844
		public Transform circleCenter;

		// Token: 0x040041CD RID: 16845
		public float circleRadius;

		// Token: 0x040041CE RID: 16846
		public float angularSpeed;

		// Token: 0x040041CF RID: 16847
		public float patrolSpeed;

		// Token: 0x040041D0 RID: 16848
		public float fleeSpeed;

		// Token: 0x040041D1 RID: 16849
		public NavMeshAgent navMeshAgent;

		// Token: 0x040041D2 RID: 16850
		public Animator animator;

		// Token: 0x040041D3 RID: 16851
		public float fleeRang;

		// Token: 0x040041D4 RID: 16852
		public float fleeSpeedMult;

		// Token: 0x040041D5 RID: 16853
		public float minChaseRange;

		// Token: 0x040041D6 RID: 16854
		public float attackDistance;

		// Token: 0x040041D7 RID: 16855
		public float navMeshSampleRange = 5f;

		// Token: 0x040041D8 RID: 16856
		internal readonly List<Transform> waypoints = new List<Transform>();

		// Token: 0x040041D9 RID: 16857
		internal float defaultSpeed;

		// Token: 0x040041DA RID: 16858
		public Transform followTarget;

		// Token: 0x040041DB RID: 16859
		public NetPlayer targetPlayer;

		// Token: 0x040041DC RID: 16860
		public bool targetIsOnNavMesh;
	}
}
