﻿using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A71 RID: 2673
	public class TrainCar : TrainCarBase
	{
		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06004299 RID: 17049 RVA: 0x0013A599 File Offset: 0x00138799
		public float DistanceBehindParentScaled
		{
			get
			{
				return this.scale * this._distanceBehindParent;
			}
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x0013A5A8 File Offset: 0x001387A8
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x0600429B RID: 17051 RVA: 0x0013A5B0 File Offset: 0x001387B0
		public override void UpdatePosition()
		{
			base.Distance = this._parentLocomotive.Distance - this.DistanceBehindParentScaled;
			base.UpdateCarPosition();
			base.RotateCarWheels();
		}

		// Token: 0x0400439D RID: 17309
		[SerializeField]
		private TrainCarBase _parentLocomotive;

		// Token: 0x0400439E RID: 17310
		[SerializeField]
		protected float _distanceBehindParent = 0.1f;
	}
}
