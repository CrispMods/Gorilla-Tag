using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A6E RID: 2670
	public class TrainCar : TrainCarBase
	{
		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x0600428D RID: 17037 RVA: 0x00139FD1 File Offset: 0x001381D1
		public float DistanceBehindParentScaled
		{
			get
			{
				return this.scale * this._distanceBehindParent;
			}
		}

		// Token: 0x0600428E RID: 17038 RVA: 0x00139FE0 File Offset: 0x001381E0
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x0600428F RID: 17039 RVA: 0x00139FE8 File Offset: 0x001381E8
		public override void UpdatePosition()
		{
			base.Distance = this._parentLocomotive.Distance - this.DistanceBehindParentScaled;
			base.UpdateCarPosition();
			base.RotateCarWheels();
		}

		// Token: 0x0400438B RID: 17291
		[SerializeField]
		private TrainCarBase _parentLocomotive;

		// Token: 0x0400438C RID: 17292
		[SerializeField]
		protected float _distanceBehindParent = 0.1f;
	}
}
