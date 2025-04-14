using System;
using UnityEngine;

namespace MTAssets.EasyMeshCombiner
{
	// Token: 0x02000B16 RID: 2838
	public class EnviromentMovement : MonoBehaviour
	{
		// Token: 0x060046F4 RID: 18164 RVA: 0x00150BDB File Offset: 0x0014EDDB
		private void Start()
		{
			this.thisTransform = base.gameObject.GetComponent<Transform>();
			this.nextPosition = this.pos1;
		}

		// Token: 0x060046F5 RID: 18165 RVA: 0x00150BFC File Offset: 0x0014EDFC
		private void Update()
		{
			if (Vector3.Distance(this.thisTransform.position, this.nextPosition) > 0.5f)
			{
				base.transform.position = Vector3.Lerp(this.thisTransform.position, this.nextPosition, 2f * Time.deltaTime);
				return;
			}
			if (this.nextPosition == this.pos1)
			{
				this.nextPosition = this.pos2;
				return;
			}
			if (this.nextPosition == this.pos2)
			{
				this.nextPosition = this.pos1;
				return;
			}
		}

		// Token: 0x04004861 RID: 18529
		private Vector3 nextPosition = Vector3.zero;

		// Token: 0x04004862 RID: 18530
		private Transform thisTransform;

		// Token: 0x04004863 RID: 18531
		public Vector3 pos1;

		// Token: 0x04004864 RID: 18532
		public Vector3 pos2;
	}
}
