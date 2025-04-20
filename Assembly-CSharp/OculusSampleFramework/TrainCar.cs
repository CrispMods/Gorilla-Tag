using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A9B RID: 2715
	public class TrainCar : TrainCarBase
	{
		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x060043D2 RID: 17362 RVA: 0x0005C332 File Offset: 0x0005A532
		public float DistanceBehindParentScaled
		{
			get
			{
				return this.scale * this._distanceBehindParent;
			}
		}

		// Token: 0x060043D3 RID: 17363 RVA: 0x0005C341 File Offset: 0x0005A541
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060043D4 RID: 17364 RVA: 0x0005C349 File Offset: 0x0005A549
		public override void UpdatePosition()
		{
			base.Distance = this._parentLocomotive.Distance - this.DistanceBehindParentScaled;
			base.UpdateCarPosition();
			base.RotateCarWheels();
		}

		// Token: 0x04004485 RID: 17541
		[SerializeField]
		private TrainCarBase _parentLocomotive;

		// Token: 0x04004486 RID: 17542
		[SerializeField]
		protected float _distanceBehindParent = 0.1f;
	}
}
