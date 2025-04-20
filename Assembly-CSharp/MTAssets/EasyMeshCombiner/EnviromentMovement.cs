using System;
using UnityEngine;

namespace MTAssets.EasyMeshCombiner
{
	// Token: 0x02000B43 RID: 2883
	public class EnviromentMovement : MonoBehaviour
	{
		// Token: 0x0600483D RID: 18493 RVA: 0x0005EFEE File Offset: 0x0005D1EE
		private void Start()
		{
			this.thisTransform = base.gameObject.GetComponent<Transform>();
			this.nextPosition = this.pos1;
		}

		// Token: 0x0600483E RID: 18494 RVA: 0x0018D3F0 File Offset: 0x0018B5F0
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

		// Token: 0x04004956 RID: 18774
		private Vector3 nextPosition = Vector3.zero;

		// Token: 0x04004957 RID: 18775
		private Transform thisTransform;

		// Token: 0x04004958 RID: 18776
		public Vector3 pos1;

		// Token: 0x04004959 RID: 18777
		public Vector3 pos2;
	}
}
