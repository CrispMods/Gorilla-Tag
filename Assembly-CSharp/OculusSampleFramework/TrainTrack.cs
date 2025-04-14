using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A75 RID: 2677
	public class TrainTrack : MonoBehaviour
	{
		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x060042C0 RID: 17088 RVA: 0x0013AB8C File Offset: 0x00138D8C
		// (set) Token: 0x060042C1 RID: 17089 RVA: 0x0013AB94 File Offset: 0x00138D94
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

		// Token: 0x060042C2 RID: 17090 RVA: 0x0013AB9D File Offset: 0x00138D9D
		private void Awake()
		{
			this.Regenerate();
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x0013ABA8 File Offset: 0x00138DA8
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

		// Token: 0x060042C4 RID: 17092 RVA: 0x0013AC00 File Offset: 0x00138E00
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

		// Token: 0x060042C5 RID: 17093 RVA: 0x0013ACE8 File Offset: 0x00138EE8
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

		// Token: 0x040043D8 RID: 17368
		[SerializeField]
		private float _gridSize = 0.5f;

		// Token: 0x040043D9 RID: 17369
		[SerializeField]
		private int _subDivCount = 20;

		// Token: 0x040043DA RID: 17370
		[SerializeField]
		private Transform _segmentParent;

		// Token: 0x040043DB RID: 17371
		[SerializeField]
		private Transform _trainParent;

		// Token: 0x040043DC RID: 17372
		[SerializeField]
		private bool _regnerateTrackMeshOnAwake;

		// Token: 0x040043DD RID: 17373
		private float _trainLength = -1f;

		// Token: 0x040043DE RID: 17374
		private TrackSegment[] _trackSegments;
	}
}
