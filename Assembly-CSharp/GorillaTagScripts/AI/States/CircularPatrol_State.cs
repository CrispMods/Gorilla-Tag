using System;
using UnityEngine;

namespace GorillaTagScripts.AI.States
{
	// Token: 0x02000A42 RID: 2626
	public class CircularPatrol_State : IState
	{
		// Token: 0x060041E4 RID: 16868 RVA: 0x0005B1DC File Offset: 0x000593DC
		public CircularPatrol_State(AIEntity entity)
		{
			this.entity = entity;
		}

		// Token: 0x060041E5 RID: 16869 RVA: 0x00173B44 File Offset: 0x00171D44
		public void Tick()
		{
			Vector3 position = this.entity.circleCenter.position;
			float x = position.x + Mathf.Cos(this.angle) * this.entity.angularSpeed;
			float y = position.y;
			float z = position.z + Mathf.Sin(this.angle) * this.entity.angularSpeed;
			this.entity.transform.position = new Vector3(x, y, z);
			this.angle += this.entity.angularSpeed * Time.deltaTime;
		}

		// Token: 0x060041E6 RID: 16870 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnEnter()
		{
		}

		// Token: 0x060041E7 RID: 16871 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnExit()
		{
		}

		// Token: 0x040042D1 RID: 17105
		private AIEntity entity;

		// Token: 0x040042D2 RID: 17106
		private float angle;
	}
}
