using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B4F RID: 2895
	public class CustomRopeSimulation : MonoBehaviour
	{
		// Token: 0x06004864 RID: 18532 RVA: 0x0015ED50 File Offset: 0x0015CF50
		private void Start()
		{
			Vector3 position = base.transform.position;
			for (int i = 0; i < this.nodeCount; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.ropeNodePrefab);
				gameObject.transform.parent = base.transform;
				gameObject.transform.position = position;
				this.nodes.Add(gameObject.transform);
				position.y -= this.nodeDistance;
			}
			this.nodes[this.nodes.Count - 1].GetComponentInChildren<Renderer>().enabled = false;
			this.burstNodes = new NativeArray<BurstRopeNode>(this.nodes.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x0015EE00 File Offset: 0x0015D000
		private void OnDestroy()
		{
			this.burstNodes.Dispose();
		}

		// Token: 0x06004866 RID: 18534 RVA: 0x0015EE10 File Offset: 0x0015D010
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

		// Token: 0x04004B00 RID: 19200
		private List<Transform> nodes = new List<Transform>();

		// Token: 0x04004B01 RID: 19201
		[SerializeField]
		private GameObject ropeNodePrefab;

		// Token: 0x04004B02 RID: 19202
		[SerializeField]
		private int nodeCount = 10;

		// Token: 0x04004B03 RID: 19203
		[SerializeField]
		private float nodeDistance = 0.4f;

		// Token: 0x04004B04 RID: 19204
		[SerializeField]
		private Vector3 gravity = new Vector3(0f, -9.81f, 0f);

		// Token: 0x04004B05 RID: 19205
		private NativeArray<BurstRopeNode> burstNodes;
	}
}
