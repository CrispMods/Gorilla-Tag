using System;
using UnityEngine;

namespace MTAssets.EasyMeshCombiner
{
	// Token: 0x02000B19 RID: 2841
	public class EnviromentMovement : MonoBehaviour
	{
		// Token: 0x06004700 RID: 18176 RVA: 0x001511A3 File Offset: 0x0014F3A3
		private void Start()
		{
			this.thisTransform = base.gameObject.GetComponent<Transform>();
			this.nextPosition = this.pos1;
		}

		// Token: 0x06004701 RID: 18177 RVA: 0x001511C4 File Offset: 0x0014F3C4
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

		// Token: 0x04004873 RID: 18547
		private Vector3 nextPosition = Vector3.zero;

		// Token: 0x04004874 RID: 18548
		private Transform thisTransform;

		// Token: 0x04004875 RID: 18549
		public Vector3 pos1;

		// Token: 0x04004876 RID: 18550
		public Vector3 pos2;
	}
}
