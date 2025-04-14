using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A78 RID: 2680
	public class TrainTrack : MonoBehaviour
	{
		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x060042CC RID: 17100 RVA: 0x0013B154 File Offset: 0x00139354
		// (set) Token: 0x060042CD RID: 17101 RVA: 0x0013B15C File Offset: 0x0013935C
		public float TrackLength
		{
			get
			{
				return this._trainLength;
			}
			private set
			{
				this._trainLength = value;
			}
		}

		// Token: 0x060042CE RID: 17102 RVA: 0x0013B165 File Offset: 0x00139365
		private void Awake()
		{
			this.Regenerate();
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x0013B170 File Offset: 0x00139370
		public TrackSegment GetSegment(float distance)
		{
			int childCount = this._segmentParent.childCount;
			for (int i = 0; i < childCount; i++)
			{
				TrackSegment trackSegment = this._trackSegments[i];
				TrackSegment trackSegment2 = this._trackSegments[(i + 1) % childCount];
				if (distance >= trackSegment.StartDistance && (distance < trackSegment2.StartDistance || i == childCount - 1))
				{
					return trackSegment;
				}
			}
			return null;
		}

		// Token: 0x060042D0 RID: 17104 RVA: 0x0013B1C8 File Offset: 0x001393C8
		public void Regenerate()
		{
			this._trackSegments = this._segmentParent.GetComponentsInChildren<TrackSegment>();
			this.TrackLength = 0f;
			int childCount = this._segmentParent.childCount;
			TrackSegment trackSegment = null;
			float scale = 0f;
			for (int i = 0; i < childCount; i++)
			{
				TrackSegment trackSegment2 = this._trackSegments[i];
				trackSegment2.SubDivCount = this._subDivCount;
				scale = trackSegment2.setGridSize(this._gridSize);
				if (trackSegment != null)
				{
					Pose endPose = trackSegment.EndPose;
					trackSegment2.transform.position = endPose.Position;
					trackSegment2.transform.rotation = endPose.Rotation;
					trackSegment2.StartDistance = this.TrackLength;
				}
				if (this._regnerateTrackMeshOnAwake)
				{
					trackSegment2.RegenerateTrackAndMesh();
				}
				this.TrackLength += trackSegment2.SegmentLength;
				trackSegment = trackSegment2;
			}
			this.SetScale(scale);
		}

		// Token: 0x060042D1 RID: 17105 RVA: 0x0013B2B0 File Offset: 0x001394B0
		private void SetScale(float ratio)
		{
			this._trainParent.localScale = new Vector3(ratio, ratio, ratio);
			TrainCar[] componentsInChildren = this._trainParent.GetComponentsInChildren<TrainCar>();
			this._trainParent.GetComponentInChildren<TrainLocomotive>().Scale = ratio;
			TrainCar[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Scale = ratio;
			}
		}

		// Token: 0x040043EA RID: 17386
		[SerializeField]
		private float _gridSize = 0.5f;

		// Token: 0x040043EB RID: 17387
		[SerializeField]
		private int _subDivCount = 20;

		// Token: 0x040043EC RID: 17388
		[SerializeField]
		private Transform _segmentParent;

		// Token: 0x040043ED RID: 17389
		[SerializeField]
		private Transform _trainParent;

		// Token: 0x040043EE RID: 17390
		[SerializeField]
		private bool _regnerateTrackMeshOnAwake;

		// Token: 0x040043EF RID: 17391
		private float _trainLength = -1f;

		// Token: 0x040043F0 RID: 17392
		private TrackSegment[] _trackSegments;
	}
}
