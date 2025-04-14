using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A6F RID: 2671
	public abstract class TrainCarBase : MonoBehaviour
	{
		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06004291 RID: 17041 RVA: 0x0013A021 File Offset: 0x00138221
		// (set) Token: 0x06004292 RID: 17042 RVA: 0x0013A029 File Offset: 0x00138229
		public float Distance { get; protected set; }

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06004293 RID: 17043 RVA: 0x0013A032 File Offset: 0x00138232
		// (set) Token: 0x06004294 RID: 17044 RVA: 0x0013A03A File Offset: 0x0013823A
		public float Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				this.scale = value;
			}
		}

		// Token: 0x06004295 RID: 17045 RVA: 0x000023F4 File Offset: 0x000005F4
		protected virtual void Awake()
		{
		}

		// Token: 0x06004296 RID: 17046 RVA: 0x0013A044 File Offset: 0x00138244
		public void UpdatePose(float distance, TrainCarBase train, Pose pose)
		{
			distance = (train._trainTrack.TrackLength + distance) % train._trainTrack.TrackLength;
			if (distance < 0f)
			{
				distance += train._trainTrack.TrackLength;
			}
			TrackSegment segment = train._trainTrack.GetSegment(distance);
			float distanceIntoSegment = distance - segment.StartDistance;
			segment.UpdatePose(distanceIntoSegment, pose);
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x0013A0A4 File Offset: 0x001382A4
		protected void UpdateCarPosition()
		{
			this.UpdatePose(this.Distance + this._frontWheels.transform.localPosition.z * this.scale, this, this._frontPose);
			this.UpdatePose(this.Distance + this._rearWheels.transform.localPosition.z * this.scale, this, this._rearPose);
			Vector3 a = 0.5f * (this._frontPose.Position + this._rearPose.Position);
			Vector3 forward = this._frontPose.Position - this._rearPose.Position;
			base.transform.position = a + TrainCarBase.OFFSET;
			base.transform.rotation = Quaternion.LookRotation(forward, base.transform.up);
			this._frontWheels.transform.rotation = this._frontPose.Rotation;
			this._rearWheels.transform.rotation = this._rearPose.Rotation;
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x0013A1BC File Offset: 0x001383BC
		protected void RotateCarWheels()
		{
			float num = this.Distance / 0.027f % 6.2831855f;
			Transform[] individualWheels = this._individualWheels;
			for (int i = 0; i < individualWheels.Length; i++)
			{
				individualWheels[i].localRotation = Quaternion.AngleAxis(57.29578f * num, Vector3.right);
			}
		}

		// Token: 0x06004299 RID: 17049
		public abstract void UpdatePosition();

		// Token: 0x0400438D RID: 17293
		private static Vector3 OFFSET = new Vector3(0f, 0.0195f, 0f);

		// Token: 0x0400438E RID: 17294
		private const float WHEEL_RADIUS = 0.027f;

		// Token: 0x0400438F RID: 17295
		private const float TWO_PI = 6.2831855f;

		// Token: 0x04004390 RID: 17296
		[SerializeField]
		protected Transform _frontWheels;

		// Token: 0x04004391 RID: 17297
		[SerializeField]
		protected Transform _rearWheels;

		// Token: 0x04004392 RID: 17298
		[SerializeField]
		protected TrainTrack _trainTrack;

		// Token: 0x04004393 RID: 17299
		[SerializeField]
		protected Transform[] _individualWheels;

		// Token: 0x04004395 RID: 17301
		protected float scale = 1f;

		// Token: 0x04004396 RID: 17302
		private Pose _frontPose = new Pose();

		// Token: 0x04004397 RID: 17303
		private Pose _rearPose = new Pose();
	}
}
