using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A9C RID: 2716
	public abstract class TrainCarBase : MonoBehaviour
	{
		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x060043D6 RID: 17366 RVA: 0x0005C382 File Offset: 0x0005A582
		// (set) Token: 0x060043D7 RID: 17367 RVA: 0x0005C38A File Offset: 0x0005A58A
		public float Distance { get; protected set; }

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x060043D8 RID: 17368 RVA: 0x0005C393 File Offset: 0x0005A593
		// (set) Token: 0x060043D9 RID: 17369 RVA: 0x0005C39B File Offset: 0x0005A59B
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

		// Token: 0x060043DA RID: 17370 RVA: 0x00030607 File Offset: 0x0002E807
		protected virtual void Awake()
		{
		}

		// Token: 0x060043DB RID: 17371 RVA: 0x001794E8 File Offset: 0x001776E8
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

		// Token: 0x060043DC RID: 17372 RVA: 0x00179548 File Offset: 0x00177748
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

		// Token: 0x060043DD RID: 17373 RVA: 0x00179660 File Offset: 0x00177860
		protected void RotateCarWheels()
		{
			float num = this.Distance / 0.027f % 6.2831855f;
			Transform[] individualWheels = this._individualWheels;
			for (int i = 0; i < individualWheels.Length; i++)
			{
				individualWheels[i].localRotation = Quaternion.AngleAxis(57.29578f * num, Vector3.right);
			}
		}

		// Token: 0x060043DE RID: 17374
		public abstract void UpdatePosition();

		// Token: 0x04004487 RID: 17543
		private static Vector3 OFFSET = new Vector3(0f, 0.0195f, 0f);

		// Token: 0x04004488 RID: 17544
		private const float WHEEL_RADIUS = 0.027f;

		// Token: 0x04004489 RID: 17545
		private const float TWO_PI = 6.2831855f;

		// Token: 0x0400448A RID: 17546
		[SerializeField]
		protected Transform _frontWheels;

		// Token: 0x0400448B RID: 17547
		[SerializeField]
		protected Transform _rearWheels;

		// Token: 0x0400448C RID: 17548
		[SerializeField]
		protected TrainTrack _trainTrack;

		// Token: 0x0400448D RID: 17549
		[SerializeField]
		protected Transform[] _individualWheels;

		// Token: 0x0400448F RID: 17551
		protected float scale = 1f;

		// Token: 0x04004490 RID: 17552
		private Pose _frontPose = new Pose();

		// Token: 0x04004491 RID: 17553
		private Pose _rearPose = new Pose();
	}
}
