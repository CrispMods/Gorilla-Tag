using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000AA2 RID: 2722
	public class TrainTrack : MonoBehaviour
	{
		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06004405 RID: 17413 RVA: 0x0005C5AB File Offset: 0x0005A7AB
		// (set) Token: 0x06004406 RID: 17414 RVA: 0x0005C5B3 File Offset: 0x0005A7B3
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

		// Token: 0x06004407 RID: 17415 RVA: 0x0005C5BC File Offset: 0x0005A7BC
		private void Awake()
		{
			this.Regenerate();
		}

		// Token: 0x06004408 RID: 17416 RVA: 0x00179E28 File Offset: 0x00178028
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

		// Token: 0x06004409 RID: 17417 RVA: 0x00179E80 File Offset: 0x00178080
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

		// Token: 0x0600440A RID: 17418 RVA: 0x00179F68 File Offset: 0x00178168
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

		// Token: 0x040044D2 RID: 17618
		[SerializeField]
		private float _gridSize = 0.5f;

		// Token: 0x040044D3 RID: 17619
		[SerializeField]
		private int _subDivCount = 20;

		// Token: 0x040044D4 RID: 17620
		[SerializeField]
		private Transform _segmentParent;

		// Token: 0x040044D5 RID: 17621
		[SerializeField]
		private Transform _trainParent;

		// Token: 0x040044D6 RID: 17622
		[SerializeField]
		private bool _regnerateTrackMeshOnAwake;

		// Token: 0x040044D7 RID: 17623
		private float _trainLength = -1f;

		// Token: 0x040044D8 RID: 17624
		private TrackSegment[] _trackSegments;
	}
}
