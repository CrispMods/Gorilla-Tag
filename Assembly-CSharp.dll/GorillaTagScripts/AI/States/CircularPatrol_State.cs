﻿using System;
using UnityEngine;

namespace GorillaTagScripts.AI.States
{
	// Token: 0x02000A18 RID: 2584
	public class CircularPatrol_State : IState
	{
		// Token: 0x060040AB RID: 16555 RVA: 0x000597DA File Offset: 0x000579DA
		public CircularPatrol_State(AIEntity entity)
		{
			this.entity = entity;
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x0016CCC0 File Offset: 0x0016AEC0
		public void Tick()
		{
			Vector3 position = this.entity.circleCenter.position;
			float x = position.x + Mathf.Cos(this.angle) * this.entity.angularSpeed;
			float y = position.y;
			float z = position.z + Mathf.Sin(this.angle) * this.entity.angularSpeed;
			this.entity.transform.position = new Vector3(x, y, z);
			this.angle += this.entity.angularSpeed * Time.deltaTime;
		}

		// Token: 0x060040AD RID: 16557 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnEnter()
		{
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnExit()
		{
		}

		// Token: 0x040041E9 RID: 16873
		private AIEntity entity;

		// Token: 0x040041EA RID: 16874
		private float angle;
	}
}
