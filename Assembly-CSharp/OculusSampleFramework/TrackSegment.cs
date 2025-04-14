using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A6A RID: 2666
	public class TrackSegment : MonoBehaviour
	{
		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x0600426A RID: 17002 RVA: 0x0013947B File Offset: 0x0013767B
		// (set) Token: 0x0600426B RID: 17003 RVA: 0x00139483 File Offset: 0x00137683
		public float StartDistance { get; set; }

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x0600426C RID: 17004 RVA: 0x0013948C File Offset: 0x0013768C
		// (set) Token: 0x0600426D RID: 17005 RVA: 0x00139494 File Offset: 0x00137694
		public float GridSize
		{
			get
			{
				return this._gridSize;
			}
			private set
			{
				this._gridSize = value;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x0600426E RID: 17006 RVA: 0x0013949D File Offset: 0x0013769D
		// (set) Token: 0x0600426F RID: 17007 RVA: 0x001394A5 File Offset: 0x001376A5
		public int SubDivCount
		{
			get
			{
				return this._subDivCount;
			}
			set
			{
				this._subDivCount = value;
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06004270 RID: 17008 RVA: 0x001394AE File Offset: 0x001376AE
		public TrackSegment.SegmentType Type
		{
			get
			{
				return this._segmentType;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06004271 RID: 17009 RVA: 0x001394B6 File Offset: 0x001376B6
		public Pose EndPose
		{
			get
			{
				this.UpdatePose(this.SegmentLength, this._endPose);
				return this._endPose;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06004272 RID: 17010 RVA: 0x001394D0 File Offset: 0x001376D0
		public float Radius
		{
			get
			{
				return 0.5f * this.GridSize;
			}
		}

		// Token: 0x06004273 RID: 17011 RVA: 0x001394DE File Offset: 0x001376DE
		public float setGridSize(float size)
		{
			this.GridSize = size;
			return this.GridSize / 0.8f;
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06004274 RID: 17012 RVA: 0x001394F4 File Offset: 0x001376F4
		public float SegmentLength
		{
			get
			{
				TrackSegment.SegmentType type = this.Type;
				if (type == TrackSegment.SegmentType.Straight)
				{
					return this.GridSize;
				}
				if (type - TrackSegment.SegmentType.LeftTurn > 1)
				{
					return 1f;
				}
				return 1.5707964f * this.Radius;
			}
		}

		// Token: 0x06004275 RID: 17013 RVA: 0x000023F4 File Offset: 0x000005F4
		private void Awake()
		{
		}

		// Token: 0x06004276 RID: 17014 RVA: 0x0013952C File Offset: 0x0013772C
		public void UpdatePose(float distanceIntoSegment, Pose pose)
		{
			if (this.Type == TrackSegment.SegmentType.Straight)
			{
				pose.Position = base.transform.position + distanceIntoSegment * base.transform.forward;
				pose.Rotation = base.transform.rotation;
				return;
			}
			if (this.Type == TrackSegment.SegmentType.LeftTurn)
			{
				float num = distanceIntoSegment / this.SegmentLength;
				float num2 = 1.5707964f * num;
				Vector3 position = new Vector3(this.Radius * Mathf.Cos(num2) - this.Radius, 0f, this.Radius * Mathf.Sin(num2));
				Quaternion rhs = Quaternion.Euler(0f, -num2 * 57.29578f, 0f);
				pose.Position = base.transform.TransformPoint(position);
				pose.Rotation = base.transform.rotation * rhs;
				return;
			}
			if (this.Type == TrackSegment.SegmentType.RightTurn)
			{
				float num3 = 3.1415927f - 1.5707964f * distanceIntoSegment / this.SegmentLength;
				Vector3 position2 = new Vector3(this.Radius * Mathf.Cos(num3) + this.Radius, 0f, this.Radius * Mathf.Sin(num3));
				Quaternion rhs2 = Quaternion.Euler(0f, (3.1415927f - num3) * 57.29578f, 0f);
				pose.Position = base.transform.TransformPoint(position2);
				pose.Rotation = base.transform.rotation * rhs2;
				return;
			}
			pose.Position = Vector3.zero;
			pose.Rotation = Quaternion.identity;
		}

		// Token: 0x06004277 RID: 17015 RVA: 0x000023F4 File Offset: 0x000005F4
		private void Update()
		{
		}

		// Token: 0x06004278 RID: 17016 RVA: 0x001396B8 File Offset: 0x001378B8
		private void OnDisable()
		{
			Object.Destroy(this._mesh);
		}

		// Token: 0x06004279 RID: 17017 RVA: 0x001396C8 File Offset: 0x001378C8
		private void DrawDebugLines()
		{
			for (int i = 1; i < this.SubDivCount + 1; i++)
			{
				float num = this.SegmentLength / (float)this.SubDivCount;
				this.UpdatePose((float)(i - 1) * num, this._p1);
				this.UpdatePose((float)i * num, this._p2);
				float d = 0.075f;
				Debug.DrawLine(this._p1.Position + d * (this._p1.Rotation * Vector3.right), this._p2.Position + d * (this._p2.Rotation * Vector3.right));
				Debug.DrawLine(this._p1.Position - d * (this._p1.Rotation * Vector3.right), this._p2.Position - d * (this._p2.Rotation * Vector3.right));
			}
			Debug.DrawLine(base.transform.position - 0.5f * this.GridSize * base.transform.right, base.transform.position + 0.5f * this.GridSize * base.transform.right, Color.yellow);
			Debug.DrawLine(base.transform.position - 0.5f * this.GridSize * base.transform.right, base.transform.position - 0.5f * this.GridSize * base.transform.right + this.GridSize * base.transform.forward, Color.yellow);
			Debug.DrawLine(base.transform.position + 0.5f * this.GridSize * base.transform.right, base.transform.position + 0.5f * this.GridSize * base.transform.right + this.GridSize * base.transform.forward, Color.yellow);
			Debug.DrawLine(base.transform.position - 0.5f * this.GridSize * base.transform.right + this.GridSize * base.transform.forward, base.transform.position + 0.5f * this.GridSize * base.transform.right + this.GridSize * base.transform.forward, Color.yellow);
		}

		// Token: 0x0600427A RID: 17018 RVA: 0x001399D0 File Offset: 0x00137BD0
		public void RegenerateTrackAndMesh()
		{
			if (base.transform.childCount > 0 && !this._mesh)
			{
				this._mesh = base.transform.GetChild(0).gameObject;
			}
			if (this._mesh)
			{
				Object.DestroyImmediate(this._mesh);
			}
			if (this._segmentType == TrackSegment.SegmentType.LeftTurn)
			{
				this._mesh = Object.Instantiate<GameObject>(this._leftTurn.gameObject);
			}
			else if (this._segmentType == TrackSegment.SegmentType.RightTurn)
			{
				this._mesh = Object.Instantiate<GameObject>(this._rightTurn.gameObject);
			}
			else
			{
				this._mesh = Object.Instantiate<GameObject>(this._straight.gameObject);
			}
			this._mesh.transform.SetParent(base.transform, false);
			this._mesh.transform.position += this.GridSize / 2f * base.transform.forward;
			this._mesh.transform.localScale = new Vector3(this.GridSize / 0.8f, this.GridSize / 0.8f, this.GridSize / 0.8f);
		}

		// Token: 0x04004363 RID: 17251
		[SerializeField]
		private TrackSegment.SegmentType _segmentType;

		// Token: 0x04004364 RID: 17252
		[SerializeField]
		private MeshFilter _straight;

		// Token: 0x04004365 RID: 17253
		[SerializeField]
		private MeshFilter _leftTurn;

		// Token: 0x04004366 RID: 17254
		[SerializeField]
		private MeshFilter _rightTurn;

		// Token: 0x04004367 RID: 17255
		private float _gridSize = 0.8f;

		// Token: 0x04004368 RID: 17256
		private int _subDivCount = 20;

		// Token: 0x04004369 RID: 17257
		private const float _originalGridSize = 0.8f;

		// Token: 0x0400436A RID: 17258
		private const float _trackWidth = 0.15f;

		// Token: 0x0400436B RID: 17259
		private GameObject _mesh;

		// Token: 0x0400436D RID: 17261
		private Pose _p1 = new Pose();

		// Token: 0x0400436E RID: 17262
		private Pose _p2 = new Pose();

		// Token: 0x0400436F RID: 17263
		private Pose _endPose = new Pose();

		// Token: 0x02000A6B RID: 2667
		public enum SegmentType
		{
			// Token: 0x04004371 RID: 17265
			Straight,
			// Token: 0x04004372 RID: 17266
			LeftTurn,
			// Token: 0x04004373 RID: 17267
			RightTurn,
			// Token: 0x04004374 RID: 17268
			Switch
		}
	}
}
