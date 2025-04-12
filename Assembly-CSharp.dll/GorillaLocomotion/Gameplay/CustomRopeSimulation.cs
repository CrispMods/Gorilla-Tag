using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B52 RID: 2898
	public class CustomRopeSimulation : MonoBehaviour
	{
		// Token: 0x06004870 RID: 18544 RVA: 0x00193760 File Offset: 0x00191960
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

		// Token: 0x06004871 RID: 18545 RVA: 0x0005E467 File Offset: 0x0005C667
		private void OnDestroy()
		{
			this.burstNodes.Dispose();
		}

		// Token: 0x06004872 RID: 18546 RVA: 0x00193810 File Offset: 0x00191A10
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

		// Token: 0x04004B12 RID: 19218
		private List<Transform> nodes = new List<Transform>();

		// Token: 0x04004B13 RID: 19219
		[SerializeField]
		private GameObject ropeNodePrefab;

		// Token: 0x04004B14 RID: 19220
		[SerializeField]
		private int nodeCount = 10;

		// Token: 0x04004B15 RID: 19221
		[SerializeField]
		private float nodeDistance = 0.4f;

		// Token: 0x04004B16 RID: 19222
		[SerializeField]
		private Vector3 gravity = new Vector3(0f, -9.81f, 0f);

		// Token: 0x04004B17 RID: 19223
		private NativeArray<BurstRopeNode> burstNodes;
	}
}
