using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000984 RID: 2436
	public class BuilderAttachEdge : MonoBehaviour
	{
		// Token: 0x06003B98 RID: 15256 RVA: 0x00112196 File Offset: 0x00110396
		private void Awake()
		{
			if (this.center == null)
			{
				this.center = base.transform;
			}
		}

		// Token: 0x06003B99 RID: 15257 RVA: 0x001121B4 File Offset: 0x001103B4
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

		// Token: 0x04003CBE RID: 15550
		public Transform center;

		// Token: 0x04003CBF RID: 15551
		public float length;
	}
}
