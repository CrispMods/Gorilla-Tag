using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A97 RID: 2711
	public class TrackSegment : MonoBehaviour
	{
		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x060043AF RID: 17327 RVA: 0x0005C1F5 File Offset: 0x0005A3F5
		// (set) Token: 0x060043B0 RID: 17328 RVA: 0x0005C1FD File Offset: 0x0005A3FD
		public float StartDistance { get; set; }

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x060043B1 RID: 17329 RVA: 0x0005C206 File Offset: 0x0005A406
		// (set) Token: 0x060043B2 RID: 17330 RVA: 0x0005C20E File Offset: 0x0005A40E
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

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x060043B3 RID: 17331 RVA: 0x0005C217 File Offset: 0x0005A417
		// (set) Token: 0x060043B4 RID: 17332 RVA: 0x0005C21F File Offset: 0x0005A41F
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

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x060043B5 RID: 17333 RVA: 0x0005C228 File Offset: 0x0005A428
		public TrackSegment.SegmentType Type
		{
			get
			{
				return this._segmentType;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x060043B6 RID: 17334 RVA: 0x0005C230 File Offset: 0x0005A430
		public Pose EndPose
		{
			get
			{
				this.UpdatePose(this.SegmentLength, this._endPose);
				return this._endPose;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x060043B7 RID: 17335 RVA: 0x0005C24A File Offset: 0x0005A44A
		public float Radius
		{
			get
			{
				return 0.5f * this.GridSize;
			}
		}

		// Token: 0x060043B8 RID: 17336 RVA: 0x0005C258 File Offset: 0x0005A458
		public float setGridSize(float size)
		{
			this.GridSize = size;
			return this.GridSize / 0.8f;
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x060043B9 RID: 17337 RVA: 0x00178AD4 File Offset: 0x00176CD4
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

		// Token: 0x060043BA RID: 17338 RVA: 0x00030607 File Offset: 0x0002E807
		private void Awake()
		{
		}

		// Token: 0x060043BB RID: 17339 RVA: 0x00178B0C File Offset: 0x00176D0C
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

		// Token: 0x060043BC RID: 17340 RVA: 0x00030607 File Offset: 0x0002E807
		private void Update()
		{
		}

		// Token: 0x060043BD RID: 17341 RVA: 0x0005C26D File Offset: 0x0005A46D
		private void OnDisable()
		{
			UnityEngine.Object.Destroy(this._mesh);
		}

		// Token: 0x060043BE RID: 17342 RVA: 0x00178C98 File Offset: 0x00176E98
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

		// Token: 0x060043BF RID: 17343 RVA: 0x00178FA0 File Offset: 0x001771A0
		public void RegenerateTrackAndMesh()
		{
			if (base.transform.childCount > 0 && !this._mesh)
			{
				this._mesh = base.transform.GetChild(0).gameObject;
			}
			if (this._mesh)
			{
				UnityEngine.Object.DestroyImmediate(this._mesh);
			}
			if (this._segmentType == TrackSegment.SegmentType.LeftTurn)
			{
				this._mesh = UnityEngine.Object.Instantiate<GameObject>(this._leftTurn.gameObject);
			}
			else if (this._segmentType == TrackSegment.SegmentType.RightTurn)
			{
				this._mesh = UnityEngine.Object.Instantiate<GameObject>(this._rightTurn.gameObject);
			}
			else
			{
				this._mesh = UnityEngine.Object.Instantiate<GameObject>(this._straight.gameObject);
			}
			this._mesh.transform.SetParent(base.transform, false);
			this._mesh.transform.position += this.GridSize / 2f * base.transform.forward;
			this._mesh.transform.localScale = new Vector3(this.GridSize / 0.8f, this.GridSize / 0.8f, this.GridSize / 0.8f);
		}

		// Token: 0x0400445D RID: 17501
		[SerializeField]
		private TrackSegment.SegmentType _segmentType;

		// Token: 0x0400445E RID: 17502
		[SerializeField]
		private MeshFilter _straight;

		// Token: 0x0400445F RID: 17503
		[SerializeField]
		private MeshFilter _leftTurn;

		// Token: 0x04004460 RID: 17504
		[SerializeField]
		private MeshFilter _rightTurn;

		// Token: 0x04004461 RID: 17505
		private float _gridSize = 0.8f;

		// Token: 0x04004462 RID: 17506
		private int _subDivCount = 20;

		// Token: 0x04004463 RID: 17507
		private const float _originalGridSize = 0.8f;

		// Token: 0x04004464 RID: 17508
		private const float _trackWidth = 0.15f;

		// Token: 0x04004465 RID: 17509
		private GameObject _mesh;

		// Token: 0x04004467 RID: 17511
		private Pose _p1 = new Pose();

		// Token: 0x04004468 RID: 17512
		private Pose _p2 = new Pose();

		// Token: 0x04004469 RID: 17513
		private Pose _endPose = new Pose();

		// Token: 0x02000A98 RID: 2712
		public enum SegmentType
		{
			// Token: 0x0400446B RID: 17515
			Straight,
			// Token: 0x0400446C RID: 17516
			LeftTurn,
			// Token: 0x0400446D RID: 17517
			RightTurn,
			// Token: 0x0400446E RID: 17518
			Switch
		}
	}
}
