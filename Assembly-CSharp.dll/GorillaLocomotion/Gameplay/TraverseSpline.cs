using System;
using Fusion;
using Photon.Pun;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B62 RID: 2914
	[NetworkBehaviourWeaved(1)]
	public class TraverseSpline : NetworkComponent
	{
		// Token: 0x060048E1 RID: 18657 RVA: 0x0005E968 File Offset: 0x0005CB68
		protected override void Awake()
		{
			base.Awake();
			this.progress = this.SplineProgressOffet % 1f;
		}

		// Token: 0x060048E2 RID: 18658 RVA: 0x0019561C File Offset: 0x0019381C
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

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x060048E3 RID: 18659 RVA: 0x0005E982 File Offset: 0x0005CB82
		// (set) Token: 0x060048E4 RID: 18660 RVA: 0x0005E9A8 File Offset: 0x0005CBA8
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

		// Token: 0x060048E5 RID: 18661 RVA: 0x0005E9CF File Offset: 0x0005CBCF
		public override void WriteDataFusion()
		{
			this.Data = this.progress + this.currentSpeedMultiplier * 1f / this.duration;
		}

		// Token: 0x060048E6 RID: 18662 RVA: 0x0005E9F1 File Offset: 0x0005CBF1
		public override void ReadDataFusion()
		{
			this.progressLerpEnd = this.Data;
			this.ReadDataShared();
		}

		// Token: 0x060048E7 RID: 18663 RVA: 0x0005EA05 File Offset: 0x0005CC05
		protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			stream.SendNext(this.progress + this.currentSpeedMultiplier * 1f / this.duration);
		}

		// Token: 0x060048E8 RID: 18664 RVA: 0x0005EA2C File Offset: 0x0005CC2C
		protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			this.progressLerpEnd = (float)stream.ReceiveNext();
			this.ReadDataShared();
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x001957E8 File Offset: 0x001939E8
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

		// Token: 0x060048EA RID: 18666 RVA: 0x0005EA45 File Offset: 0x0005CC45
		protected float GetProgress()
		{
			return this.progress;
		}

		// Token: 0x060048EB RID: 18667 RVA: 0x0005EA4D File Offset: 0x0005CC4D
		public float GetCurrentSpeed()
		{
			return this.currentSpeedMultiplier;
		}

		// Token: 0x060048ED RID: 18669 RVA: 0x0005EA55 File Offset: 0x0005CC55
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x060048EE RID: 18670 RVA: 0x0005EA6D File Offset: 0x0005CC6D
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x04004B82 RID: 19330
		public BezierSpline spline;

		// Token: 0x04004B83 RID: 19331
		public float duration = 30f;

		// Token: 0x04004B84 RID: 19332
		public float speedMultiplierWhileHeld = 2f;

		// Token: 0x04004B85 RID: 19333
		private float currentSpeedMultiplier;

		// Token: 0x04004B86 RID: 19334
		public float acceleration = 1f;

		// Token: 0x04004B87 RID: 19335
		public float deceleration = 1f;

		// Token: 0x04004B88 RID: 19336
		private bool isHeldByLocalPlayer;

		// Token: 0x04004B89 RID: 19337
		public bool lookForward = true;

		// Token: 0x04004B8A RID: 19338
		public SplineWalkerMode mode;

		// Token: 0x04004B8B RID: 19339
		[SerializeField]
		private float SplineProgressOffet;

		// Token: 0x04004B8C RID: 19340
		private float progress;

		// Token: 0x04004B8D RID: 19341
		private float progressLerpStart;

		// Token: 0x04004B8E RID: 19342
		private float progressLerpEnd;

		// Token: 0x04004B8F RID: 19343
		private const float progressLerpDuration = 1f;

		// Token: 0x04004B90 RID: 19344
		private float progressLerpStartTime;

		// Token: 0x04004B91 RID: 19345
		private bool goingForward = true;

		// Token: 0x04004B92 RID: 19346
		[SerializeField]
		private bool constantVelocity;

		// Token: 0x04004B93 RID: 19347
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Data", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _Data;
	}
}
