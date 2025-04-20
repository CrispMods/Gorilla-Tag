using System;
using Fusion;
using Photon.Pun;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B8C RID: 2956
	[NetworkBehaviourWeaved(1)]
	public class TraverseSpline : NetworkComponent
	{
		// Token: 0x06004A20 RID: 18976 RVA: 0x000603A0 File Offset: 0x0005E5A0
		protected override void Awake()
		{
			base.Awake();
			this.progress = this.SplineProgressOffet % 1f;
		}

		// Token: 0x06004A21 RID: 18977 RVA: 0x0019C634 File Offset: 0x0019A834
		protected virtual void FixedUpdate()
		{
			if (!base.IsMine && this.progressLerpStartTime + 1f > Time.time)
			{
				this.progress = Mathf.Lerp(this.progressLerpStart, this.progressLerpEnd, (Time.time - this.progressLerpStartTime) / 1f);
			}
			else
			{
				if (this.isHeldByLocalPlayer)
				{
					this.currentSpeedMultiplier = Mathf.MoveTowards(this.currentSpeedMultiplier, this.speedMultiplierWhileHeld, this.acceleration * Time.deltaTime);
				}
				else
				{
					this.currentSpeedMultiplier = Mathf.MoveTowards(this.currentSpeedMultiplier, 1f, this.deceleration * Time.deltaTime);
				}
				if (this.goingForward)
				{
					this.progress += Time.deltaTime * this.currentSpeedMultiplier / this.duration;
					if (this.progress > 1f)
					{
						if (this.mode == SplineWalkerMode.Once)
						{
							this.progress = 1f;
						}
						else if (this.mode == SplineWalkerMode.Loop)
						{
							this.progress %= 1f;
						}
						else
						{
							this.progress = 2f - this.progress;
							this.goingForward = false;
						}
					}
				}
				else
				{
					this.progress -= Time.deltaTime * this.currentSpeedMultiplier / this.duration;
					if (this.progress < 0f)
					{
						this.progress = -this.progress;
						this.goingForward = true;
					}
				}
			}
			Vector3 point = this.spline.GetPoint(this.progress, this.constantVelocity);
			base.transform.position = point;
			if (this.lookForward)
			{
				base.transform.LookAt(base.transform.position + this.spline.GetDirection(this.progress, this.constantVelocity));
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06004A22 RID: 18978 RVA: 0x000603BA File Offset: 0x0005E5BA
		// (set) Token: 0x06004A23 RID: 18979 RVA: 0x000603E0 File Offset: 0x0005E5E0
		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe float Data
		{
			get
			{
				if (this.Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing TraverseSpline.Data. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(this.Ptr + 0);
			}
			set
			{
				if (this.Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing TraverseSpline.Data. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(this.Ptr + 0) = value;
			}
		}

		// Token: 0x06004A24 RID: 18980 RVA: 0x00060407 File Offset: 0x0005E607
		public override void WriteDataFusion()
		{
			this.Data = this.progress + this.currentSpeedMultiplier * 1f / this.duration;
		}

		// Token: 0x06004A25 RID: 18981 RVA: 0x00060429 File Offset: 0x0005E629
		public override void ReadDataFusion()
		{
			this.progressLerpEnd = this.Data;
			this.ReadDataShared();
		}

		// Token: 0x06004A26 RID: 18982 RVA: 0x0006043D File Offset: 0x0005E63D
		protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			stream.SendNext(this.progress + this.currentSpeedMultiplier * 1f / this.duration);
		}

		// Token: 0x06004A27 RID: 18983 RVA: 0x00060464 File Offset: 0x0005E664
		protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			this.progressLerpEnd = (float)stream.ReceiveNext();
			this.ReadDataShared();
		}

		// Token: 0x06004A28 RID: 18984 RVA: 0x0019C800 File Offset: 0x0019AA00
		private void ReadDataShared()
		{
			if (float.IsNaN(this.progressLerpEnd) || float.IsInfinity(this.progressLerpEnd))
			{
				this.progressLerpEnd = 1f;
			}
			else
			{
				this.progressLerpEnd = Mathf.Abs(this.progressLerpEnd);
				if (this.progressLerpEnd > 1f)
				{
					this.progressLerpEnd = (float)((double)this.progressLerpEnd % 1.0);
				}
			}
			this.progressLerpStart = ((Mathf.Abs(this.progressLerpEnd - this.progress) > Mathf.Abs(this.progressLerpEnd - (this.progress - 1f))) ? (this.progress - 1f) : this.progress);
			this.progressLerpStartTime = Time.time;
		}

		// Token: 0x06004A29 RID: 18985 RVA: 0x0006047D File Offset: 0x0005E67D
		protected float GetProgress()
		{
			return this.progress;
		}

		// Token: 0x06004A2A RID: 18986 RVA: 0x00060485 File Offset: 0x0005E685
		public float GetCurrentSpeed()
		{
			return this.currentSpeedMultiplier;
		}

		// Token: 0x06004A2C RID: 18988 RVA: 0x0006048D File Offset: 0x0005E68D
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x06004A2D RID: 18989 RVA: 0x000604A5 File Offset: 0x0005E6A5
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x04004C66 RID: 19558
		public BezierSpline spline;

		// Token: 0x04004C67 RID: 19559
		public float duration = 30f;

		// Token: 0x04004C68 RID: 19560
		public float speedMultiplierWhileHeld = 2f;

		// Token: 0x04004C69 RID: 19561
		private float currentSpeedMultiplier;

		// Token: 0x04004C6A RID: 19562
		public float acceleration = 1f;

		// Token: 0x04004C6B RID: 19563
		public float deceleration = 1f;

		// Token: 0x04004C6C RID: 19564
		private bool isHeldByLocalPlayer;

		// Token: 0x04004C6D RID: 19565
		public bool lookForward = true;

		// Token: 0x04004C6E RID: 19566
		public SplineWalkerMode mode;

		// Token: 0x04004C6F RID: 19567
		[SerializeField]
		private float SplineProgressOffet;

		// Token: 0x04004C70 RID: 19568
		private float progress;

		// Token: 0x04004C71 RID: 19569
		private float progressLerpStart;

		// Token: 0x04004C72 RID: 19570
		private float progressLerpEnd;

		// Token: 0x04004C73 RID: 19571
		private const float progressLerpDuration = 1f;

		// Token: 0x04004C74 RID: 19572
		private float progressLerpStartTime;

		// Token: 0x04004C75 RID: 19573
		private bool goingForward = true;

		// Token: 0x04004C76 RID: 19574
		[SerializeField]
		private bool constantVelocity;

		// Token: 0x04004C77 RID: 19575
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Data", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _Data;
	}
}
