using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B7C RID: 2940
	public class CustomRopeSimulation : MonoBehaviour
	{
		// Token: 0x060049AF RID: 18863 RVA: 0x0019A778 File Offset: 0x00198978
		private void Start()
		{
			Vector3 position = base.transform.position;
			for (int i = 0; i < this.nodeCount; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ropeNodePrefab);
				gameObject.transform.parent = base.transform;
				gameObject.transform.position = position;
				this.nodes.Add(gameObject.transform);
				position.y -= this.nodeDistance;
			}
			this.nodes[this.nodes.Count - 1].GetComponentInChildren<Renderer>().enabled = false;
			this.burstNodes = new NativeArray<BurstRopeNode>(this.nodes.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x060049B0 RID: 18864 RVA: 0x0005FE9F File Offset: 0x0005E09F
		private void OnDestroy()
		{
			this.burstNodes.Dispose();
		}

		// Token: 0x060049B1 RID: 18865 RVA: 0x0019A828 File Offset: 0x00198A28
		private void Update()
		{
			new SolveRopeJob
			{
				fixedDeltaTime = Time.deltaTime,
				gravity = this.gravity,
				nodes = this.burstNodes,
				nodeDistance = this.nodeDistance,
				rootPos = base.transform.position
			}.Run<SolveRopeJob>();
			for (int i = 0; i < this.burstNodes.Length; i++)
			{
				this.nodes[i].position = this.burstNodes[i].curPos;
				if (i > 0)
				{
					Vector3 a = this.burstNodes[i - 1].curPos - this.burstNodes[i].curPos;
					this.nodes[i].up = -a;
				}
			}
		}

		// Token: 0x04004BF6 RID: 19446
		private List<Transform> nodes = new List<Transform>();

		// Token: 0x04004BF7 RID: 19447
		[SerializeField]
		private GameObject ropeNodePrefab;

		// Token: 0x04004BF8 RID: 19448
		[SerializeField]
		private int nodeCount = 10;

		// Token: 0x04004BF9 RID: 19449
		[SerializeField]
		private float nodeDistance = 0.4f;

		// Token: 0x04004BFA RID: 19450
		[SerializeField]
		private Vector3 gravity = new Vector3(0f, -9.81f, 0f);

		// Token: 0x04004BFB RID: 19451
		private NativeArray<BurstRopeNode> burstNodes;
	}
}
