using System;
using Fusion;
using Photon.Pun;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B5F RID: 2911
	[NetworkBehaviourWeaved(1)]
	public class TraverseSpline : NetworkComponent
	{
		// Token: 0x060048D5 RID: 18645 RVA: 0x00161115 File Offset: 0x0015F315
		protected override void Awake()
		{
			base.Awake();
			this.progress = this.SplineProgressOffet % 1f;
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x00161130 File Offset: 0x0015F330
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

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x060048D7 RID: 18647 RVA: 0x001612F9 File Offset: 0x0015F4F9
		// (set) Token: 0x060048D8 RID: 18648 RVA: 0x0016131F File Offset: 0x0015F51F
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

		// Token: 0x060048D9 RID: 18649 RVA: 0x00161346 File Offset: 0x0015F546
		public override void WriteDataFusion()
		{
			this.Data = this.progress + this.currentSpeedMultiplier * 1f / this.duration;
		}

		// Token: 0x060048DA RID: 18650 RVA: 0x00161368 File Offset: 0x0015F568
		public override void ReadDataFusion()
		{
			this.progressLerpEnd = this.Data;
			this.ReadDataShared();
		}

		// Token: 0x060048DB RID: 18651 RVA: 0x0016137C File Offset: 0x0015F57C
		protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			stream.SendNext(this.progress + this.currentSpeedMultiplier * 1f / this.duration);
		}

		// Token: 0x060048DC RID: 18652 RVA: 0x001613A3 File Offset: 0x0015F5A3
		protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			this.progressLerpEnd = (float)stream.ReceiveNext();
			this.ReadDataShared();
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x001613BC File Offset: 0x0015F5BC
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

		// Token: 0x060048DE RID: 18654 RVA: 0x00161477 File Offset: 0x0015F677
		protected float GetProgress()
		{
			return this.progress;
		}

		// Token: 0x060048DF RID: 18655 RVA: 0x0016147F File Offset: 0x0015F67F
		public float GetCurrentSpeed()
		{
			return this.currentSpeedMultiplier;
		}

		// Token: 0x060048E1 RID: 18657 RVA: 0x001614D5 File Offset: 0x0015F6D5
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x060048E2 RID: 18658 RVA: 0x001614ED File Offset: 0x0015F6ED
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x04004B70 RID: 19312
		public BezierSpline spline;

		// Token: 0x04004B71 RID: 19313
		public float duration = 30f;

		// Token: 0x04004B72 RID: 19314
		public float speedMultiplierWhileHeld = 2f;

		// Token: 0x04004B73 RID: 19315
		private float currentSpeedMultiplier;

		// Token: 0x04004B74 RID: 19316
		public float acceleration = 1f;

		// Token: 0x04004B75 RID: 19317
		public float deceleration = 1f;

		// Token: 0x04004B76 RID: 19318
		private bool isHeldByLocalPlayer;

		// Token: 0x04004B77 RID: 19319
		public bool lookForward = true;

		// Token: 0x04004B78 RID: 19320
		public SplineWalkerMode mode;

		// Token: 0x04004B79 RID: 19321
		[SerializeField]
		private float SplineProgressOffet;

		// Token: 0x04004B7A RID: 19322
		private float progress;

		// Token: 0x04004B7B RID: 19323
		private float progressLerpStart;

		// Token: 0x04004B7C RID: 19324
		private float progressLerpEnd;

		// Token: 0x04004B7D RID: 19325
		private const float progressLerpDuration = 1f;

		// Token: 0x04004B7E RID: 19326
		private float progressLerpStartTime;

		// Token: 0x04004B7F RID: 19327
		private bool goingForward = true;

		// Token: 0x04004B80 RID: 19328
		[SerializeField]
		private bool constantVelocity;

		// Token: 0x04004B81 RID: 19329
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Data", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _Data;
	}
}
