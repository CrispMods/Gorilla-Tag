﻿using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000987 RID: 2439
	public class BuilderAttachEdge : MonoBehaviour
	{
		// Token: 0x06003BA4 RID: 15268 RVA: 0x0011275E File Offset: 0x0011095E
		private void Awake()
		{
			if (this.center == null)
			{
				this.center = base.transform;
			}
		}

		// Token: 0x06003BA5 RID: 15269 RVA: 0x0011277C File Offset: 0x0011097C
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

		// Token: 0x04003CD0 RID: 15568
		public Transform center;

		// Token: 0x04003CD1 RID: 15569
		public float length;
	}
}
