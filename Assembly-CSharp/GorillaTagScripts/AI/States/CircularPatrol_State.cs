using System;
using UnityEngine;

namespace GorillaTagScripts.AI.States
{
	// Token: 0x02000A15 RID: 2581
	public class CircularPatrol_State : IState
	{
		// Token: 0x0600409F RID: 16543 RVA: 0x001334DC File Offset: 0x001316DC
		public CircularPatrol_State(AIEntity entity)
		{
			this.entity = entity;
		}

		// Token: 0x060040A0 RID: 16544 RVA: 0x001334EC File Offset: 0x001316EC
		public void Tick()
		{
			Vector3 position = this.entity.circleCenter.position;
			float x = position.x + Mathf.Cos(this.angle) * this.entity.angularSpeed;
			float y = position.y;
			float z = position.z + Mathf.Sin(this.angle) * this.entity.angularSpeed;
			this.entity.transform.position = new Vector3(x, y, z);
			this.angle += this.entity.angularSpeed * Time.deltaTime;
		}

		// Token: 0x060040A1 RID: 16545 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnEnter()
		{
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnExit()
		{
		}

		// Token: 0x040041D7 RID: 16855
		private AIEntity entity;

		// Token: 0x040041D8 RID: 16856
		private float angle;
	}
}
