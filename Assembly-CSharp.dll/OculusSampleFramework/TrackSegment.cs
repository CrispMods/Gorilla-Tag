using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A6D RID: 2669
	public class TrackSegment : MonoBehaviour
	{
		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06004276 RID: 17014 RVA: 0x0005A7F3 File Offset: 0x000589F3
		// (set) Token: 0x06004277 RID: 17015 RVA: 0x0005A7FB File Offset: 0x000589FB
		public float StartDistance { get; set; }

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06004278 RID: 17016 RVA: 0x0005A804 File Offset: 0x00058A04
		// (set) Token: 0x06004279 RID: 17017 RVA: 0x0005A80C File Offset: 0x00058A0C
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

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x0600427A RID: 17018 RVA: 0x0005A815 File Offset: 0x00058A15
		// (set) Token: 0x0600427B RID: 17019 RVA: 0x0005A81D File Offset: 0x00058A1D
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

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x0600427C RID: 17020 RVA: 0x0005A826 File Offset: 0x00058A26
		public TrackSegment.SegmentType Type
		{
			get
			{
				return this._segmentType;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x0600427D RID: 17021 RVA: 0x0005A82E File Offset: 0x00058A2E
		public Pose EndPose
		{
			get
			{
				this.UpdatePose(this.SegmentLength, this._endPose);
				return this._endPose;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x0600427E RID: 17022 RVA: 0x0005A848 File Offset: 0x00058A48
		public float Radius
		{
			get
			{
				return 0.5f * this.GridSize;
			}
		}

		// Token: 0x0600427F RID: 17023 RVA: 0x0005A856 File Offset: 0x00058A56
		public float setGridSize(float size)
		{
			this.GridSize = size;
			return this.GridSize / 0.8f;
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06004280 RID: 17024 RVA: 0x00171C50 File Offset: 0x0016FE50
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

		// Token: 0x06004281 RID: 17025 RVA: 0x0002F75F File Offset: 0x0002D95F
		private void Awake()
		{
		}

		// Token: 0x06004282 RID: 17026 RVA: 0x00171C88 File Offset: 0x0016FE88
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

		// Token: 0x06004283 RID: 17027 RVA: 0x0002F75F File Offset: 0x0002D95F
		private void Update()
		{
		}

		// Token: 0x06004284 RID: 17028 RVA: 0x0005A86B File Offset: 0x00058A6B
		private void OnDisable()
		{
			UnityEngine.Object.Destroy(this._mesh);
		}

		// Token: 0x06004285 RID: 17029 RVA: 0x00171E14 File Offset: 0x00170014
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

		// Token: 0x06004286 RID: 17030 RVA: 0x0017211C File Offset: 0x0017031C
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

		// Token: 0x04004375 RID: 17269
		[SerializeField]
		private TrackSegment.SegmentType _segmentType;

		// Token: 0x04004376 RID: 17270
		[SerializeField]
		private MeshFilter _straight;

		// Token: 0x04004377 RID: 17271
		[SerializeField]
		private MeshFilter _leftTurn;

		// Token: 0x04004378 RID: 17272
		[SerializeField]
		private MeshFilter _rightTurn;

		// Token: 0x04004379 RID: 17273
		private float _gridSize = 0.8f;

		// Token: 0x0400437A RID: 17274
		private int _subDivCount = 20;

		// Token: 0x0400437B RID: 17275
		private const float _originalGridSize = 0.8f;

		// Token: 0x0400437C RID: 17276
		private const float _trackWidth = 0.15f;

		// Token: 0x0400437D RID: 17277
		private GameObject _mesh;

		// Token: 0x0400437F RID: 17279
		private Pose _p1 = new Pose();

		// Token: 0x04004380 RID: 17280
		private Pose _p2 = new Pose();

		// Token: 0x04004381 RID: 17281
		private Pose _endPose = new Pose();

		// Token: 0x02000A6E RID: 2670
		public enum SegmentType
		{
			// Token: 0x04004383 RID: 17283
			Straight,
			// Token: 0x04004384 RID: 17284
			LeftTurn,
			// Token: 0x04004385 RID: 17285
			RightTurn,
			// Token: 0x04004386 RID: 17286
			Switch
		}
	}
}
