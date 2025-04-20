using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009AA RID: 2474
	public class BuilderAttachEdge : MonoBehaviour
	{
		// Token: 0x06003CB0 RID: 15536 RVA: 0x00057A9C File Offset: 0x00055C9C
		private void Awake()
		{
			if (this.center == null)
			{
				this.center = base.transform;
			}
		}

		// Token: 0x06003CB1 RID: 15537 RVA: 0x00155190 File Offset: 0x00153390
		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Transform transform = this.center;
			if (transform == null)
			{
				transform = base.transform;
			}
			Vector3 a = transform.rotation * Vector3.right;
			Gizmos.DrawLine(transform.position - a * this.length * 0.5f, transform.position + a * this.length * 0.5f);
		}

		// Token: 0x04003D98 RID: 15768
		public Transform center;

		// Token: 0x04003D99 RID: 15769
		public float length;
	}
}
