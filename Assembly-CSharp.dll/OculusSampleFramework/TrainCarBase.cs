using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A72 RID: 2674
	public abstract class TrainCarBase : MonoBehaviour
	{
		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x0600429D RID: 17053 RVA: 0x0005A980 File Offset: 0x00058B80
		// (set) Token: 0x0600429E RID: 17054 RVA: 0x0005A988 File Offset: 0x00058B88
		public float Distance { get; protected set; }

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x0600429F RID: 17055 RVA: 0x0005A991 File Offset: 0x00058B91
		// (set) Token: 0x060042A0 RID: 17056 RVA: 0x0005A999 File Offset: 0x00058B99
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

		// Token: 0x060042A1 RID: 17057 RVA: 0x0002F75F File Offset: 0x0002D95F
		protected virtual void Awake()
		{
		}

		// Token: 0x060042A2 RID: 17058 RVA: 0x00172664 File Offset: 0x00170864
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

		// Token: 0x060042A3 RID: 17059 RVA: 0x001726C4 File Offset: 0x001708C4
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

		// Token: 0x060042A4 RID: 17060 RVA: 0x001727DC File Offset: 0x001709DC
		protected void RotateCarWheels()
		{
			float num = this.Distance / 0.027f % 6.2831855f;
			Transform[] individualWheels = this._individualWheels;
			for (int i = 0; i < individualWheels.Length; i++)
			{
				individualWheels[i].localRotation = Quaternion.AngleAxis(57.29578f * num, Vector3.right);
			}
		}

		// Token: 0x060042A5 RID: 17061
		public abstract void UpdatePosition();

		// Token: 0x0400439F RID: 17311
		private static Vector3 OFFSET = new Vector3(0f, 0.0195f, 0f);

		// Token: 0x040043A0 RID: 17312
		private const float WHEEL_RADIUS = 0.027f;

		// Token: 0x040043A1 RID: 17313
		private const float TWO_PI = 6.2831855f;

		// Token: 0x040043A2 RID: 17314
		[SerializeField]
		protected Transform _frontWheels;

		// Token: 0x040043A3 RID: 17315
		[SerializeField]
		protected Transform _rearWheels;

		// Token: 0x040043A4 RID: 17316
		[SerializeField]
		protected TrainTrack _trainTrack;

		// Token: 0x040043A5 RID: 17317
		[SerializeField]
		protected Transform[] _individualWheels;

		// Token: 0x040043A7 RID: 17319
		protected float scale = 1f;

		// Token: 0x040043A8 RID: 17320
		private Pose _frontPose = new Pose();

		// Token: 0x040043A9 RID: 17321
		private Pose _rearPose = new Pose();
	}
}
