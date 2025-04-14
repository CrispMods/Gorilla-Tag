using System;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x020009F7 RID: 2551
	public class BuilderMovingPart : MonoBehaviour
	{
		// Token: 0x06003FB3 RID: 16307 RVA: 0x0012DFCC File Offset: 0x0012C1CC
		private void Awake()
		{
			foreach (BuilderAttachGridPlane builderAttachGridPlane in this.myGridPlanes)
			{
				builderAttachGridPlane.movesOnPlace = true;
				builderAttachGridPlane.movingPart = this;
			}
			this.initLocalPos = base.transform.localPosition;
			this.initLocalRotation = base.transform.localRotation;
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x0012E020 File Offset: 0x0012C220
		private long NetworkTimeMs()
		{
			if (PhotonNetwork.InRoom)
			{
				return (long)((ulong)(PhotonNetwork.ServerTimestamp - this.myPiece.activatedTimeStamp + (int)this.startPercentageCycleOffset + int.MinValue));
			}
			return (long)(Time.time * 1000f);
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x0012E055 File Offset: 0x0012C255
		private long CycleLengthMs()
		{
			return (long)(this.cycleDuration * 1000f);
		}

		// Token: 0x06003FB6 RID: 16310 RVA: 0x0012E064 File Offset: 0x0012C264
		public double PlatformTime()
		{
			long num = this.NetworkTimeMs();
			long num2 = this.CycleLengthMs();
			return (double)(num - num / num2 * num2) / 1000.0;
		}

		// Token: 0x06003FB7 RID: 16311 RVA: 0x0012E08F File Offset: 0x0012C28F
		public int CycleCount()
		{
			return (int)(this.NetworkTimeMs() / this.CycleLengthMs());
		}

		// Token: 0x06003FB8 RID: 16312 RVA: 0x0012E09F File Offset: 0x0012C29F
		public float CycleCompletionPercent()
		{
			return Mathf.Clamp((float)(this.PlatformTime() / (double)this.cycleDuration), 0f, 1f);
		}

		// Token: 0x06003FB9 RID: 16313 RVA: 0x0012E0BF File Offset: 0x0012C2BF
		public bool IsEvenCycle()
		{
			return this.CycleCount() % 2 == 0;
		}

		// Token: 0x06003FBA RID: 16314 RVA: 0x0012E0CC File Offset: 0x0012C2CC
		public void ActivateAtNode(byte node, int timestamp)
		{
			float num = (float)node;
			bool flag = (int)node > BuilderMovingPart.NUM_PAUSE_NODES;
			if (flag)
			{
				num -= (float)BuilderMovingPart.NUM_PAUSE_NODES;
			}
			num /= (float)BuilderMovingPart.NUM_PAUSE_NODES;
			num = Mathf.Clamp(num, 0f, 1f);
			if (num >= this.startPercentage)
			{
				int num2 = (int)((num - this.startPercentage) * (float)this.CycleLengthMs());
				int num3 = timestamp - num2;
				if (flag)
				{
					num3 -= (int)this.CycleLengthMs();
				}
				this.myPiece.activatedTimeStamp = num3;
			}
			else
			{
				int num4 = (int)((num + 2f - this.startPercentage) * (float)this.CycleLengthMs());
				if (flag)
				{
					num4 -= (int)this.CycleLengthMs();
				}
				this.myPiece.activatedTimeStamp = timestamp - num4;
			}
			this.SetMoving(true);
		}

		// Token: 0x06003FBB RID: 16315 RVA: 0x0012E184 File Offset: 0x0012C384
		public int GetTimeOffsetMS()
		{
			int num = PhotonNetwork.ServerTimestamp - this.myPiece.activatedTimeStamp;
			uint num2 = (uint)(this.CycleLengthMs() * 2L);
			return num % (int)num2;
		}

		// Token: 0x06003FBC RID: 16316 RVA: 0x0012E1B0 File Offset: 0x0012C3B0
		public byte GetNearestNode()
		{
			int num = Mathf.RoundToInt(this.currT * (float)BuilderMovingPart.NUM_PAUSE_NODES);
			if (!this.IsEvenCycle())
			{
				num += BuilderMovingPart.NUM_PAUSE_NODES;
			}
			return (byte)num;
		}

		// Token: 0x06003FBD RID: 16317 RVA: 0x0012E1E2 File Offset: 0x0012C3E2
		public byte GetStartNode()
		{
			return (byte)Mathf.RoundToInt(this.startPercentage * (float)BuilderMovingPart.NUM_PAUSE_NODES);
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x0012E1F8 File Offset: 0x0012C3F8
		public void PauseMovement(byte node)
		{
			this.SetMoving(false);
			bool flag = (int)node > BuilderMovingPart.NUM_PAUSE_NODES;
			float num = (float)node;
			if (flag)
			{
				num -= (float)BuilderMovingPart.NUM_PAUSE_NODES;
			}
			num /= (float)BuilderMovingPart.NUM_PAUSE_NODES;
			num = Mathf.Clamp(num, 0f, 1f);
			if (this.reverseDirOnCycle)
			{
				num = (flag ? (1f - num) : num);
			}
			if (this.reverseDir)
			{
				num = 1f - num;
			}
			BuilderMovingPart.BuilderMovingPartType builderMovingPartType = this.moveType;
			if (builderMovingPartType == BuilderMovingPart.BuilderMovingPartType.Translation)
			{
				base.transform.localPosition = this.UpdatePointToPoint(num);
				return;
			}
			if (builderMovingPartType != BuilderMovingPart.BuilderMovingPartType.Rotation)
			{
				return;
			}
			this.UpdateRotation(num);
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x0012E290 File Offset: 0x0012C490
		public void SetMoving(bool isMoving)
		{
			this.isMoving = isMoving;
			BuilderAttachGridPlane[] array = this.myGridPlanes;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].isMoving = isMoving;
			}
			if (!isMoving)
			{
				this.ResetMovingGrid();
			}
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x0012E2CC File Offset: 0x0012C4CC
		public void InitMovingGrid()
		{
			if (this.moveType == BuilderMovingPart.BuilderMovingPartType.Translation)
			{
				this.distance = Vector3.Distance(this.endXf.position, this.startXf.position);
				float num = this.distance / this.velocity;
				this.cycleDuration = num + this.cycleDelay;
				float num2 = this.cycleDelay / this.cycleDuration;
				Vector2 vector = new Vector2(num2 / 2f, 0f);
				Vector2 vector2 = new Vector2(1f - num2 / 2f, 1f);
				float num3 = (vector2.y - vector.y) / (vector2.x - vector.x);
				this.lerpAlpha = new AnimationCurve(new Keyframe[]
				{
					new Keyframe(num2 / 2f, 0f, 0f, num3),
					new Keyframe(1f - num2 / 2f, 1f, num3, 0f)
				});
			}
			else
			{
				this.cycleDuration = 1f / this.velocity;
			}
			this.currT = this.startPercentage;
			uint num4 = (uint)(this.cycleDuration * 1000f);
			uint num5 = 2147483648U % num4;
			uint num6 = (uint)(this.startPercentage * num4);
			if (num6 >= num5)
			{
				this.startPercentageCycleOffset = num6 - num5;
				return;
			}
			this.startPercentageCycleOffset = num6 + num4 + num4 - num5;
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x0012E434 File Offset: 0x0012C634
		public void UpdateMovingGrid()
		{
			this.Progress();
			BuilderMovingPart.BuilderMovingPartType builderMovingPartType = this.moveType;
			if (builderMovingPartType == BuilderMovingPart.BuilderMovingPartType.Translation)
			{
				base.transform.localPosition = this.UpdatePointToPoint(this.percent);
				return;
			}
			if (builderMovingPartType != BuilderMovingPart.BuilderMovingPartType.Rotation)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.UpdateRotation(this.percent);
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x0012E484 File Offset: 0x0012C684
		private Vector3 UpdatePointToPoint(float perc)
		{
			float t = this.lerpAlpha.Evaluate(perc);
			return Vector3.Lerp(this.startXf.localPosition, this.endXf.localPosition, t);
		}

		// Token: 0x06003FC3 RID: 16323 RVA: 0x0012E4BC File Offset: 0x0012C6BC
		private void UpdateRotation(float perc)
		{
			Quaternion localRotation = Quaternion.AngleAxis(perc * 360f, Vector3.up);
			base.transform.localRotation = localRotation;
		}

		// Token: 0x06003FC4 RID: 16324 RVA: 0x0012E4E7 File Offset: 0x0012C6E7
		private void ResetMovingGrid()
		{
			base.transform.SetLocalPositionAndRotation(this.initLocalPos, this.initLocalRotation);
		}

		// Token: 0x06003FC5 RID: 16325 RVA: 0x0012E500 File Offset: 0x0012C700
		private void Progress()
		{
			this.currT = this.CycleCompletionPercent();
			this.currForward = this.IsEvenCycle();
			this.percent = this.currT;
			if (this.reverseDirOnCycle)
			{
				this.percent = (this.currForward ? this.currT : (1f - this.currT));
			}
			if (this.reverseDir)
			{
				this.percent = 1f - this.percent;
			}
		}

		// Token: 0x06003FC6 RID: 16326 RVA: 0x0012E578 File Offset: 0x0012C778
		public bool IsAnchoredToTable()
		{
			foreach (BuilderAttachGridPlane builderAttachGridPlane in this.myGridPlanes)
			{
				if (builderAttachGridPlane.attachIndex == builderAttachGridPlane.piece.attachIndex)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003FC7 RID: 16327 RVA: 0x0012E5B4 File Offset: 0x0012C7B4
		public void OnPieceDestroy()
		{
			this.ResetMovingGrid();
		}

		// Token: 0x040040B8 RID: 16568
		public BuilderPiece myPiece;

		// Token: 0x040040B9 RID: 16569
		public BuilderAttachGridPlane[] myGridPlanes;

		// Token: 0x040040BA RID: 16570
		[SerializeField]
		private BuilderMovingPart.BuilderMovingPartType moveType;

		// Token: 0x040040BB RID: 16571
		[SerializeField]
		private float startPercentage = 0.5f;

		// Token: 0x040040BC RID: 16572
		[SerializeField]
		private float velocity;

		// Token: 0x040040BD RID: 16573
		[SerializeField]
		private bool reverseDirOnCycle = true;

		// Token: 0x040040BE RID: 16574
		[SerializeField]
		private bool reverseDir;

		// Token: 0x040040BF RID: 16575
		[SerializeField]
		private float cycleDelay = 0.25f;

		// Token: 0x040040C0 RID: 16576
		[SerializeField]
		protected Transform startXf;

		// Token: 0x040040C1 RID: 16577
		[SerializeField]
		protected Transform endXf;

		// Token: 0x040040C2 RID: 16578
		public static int NUM_PAUSE_NODES = 32;

		// Token: 0x040040C3 RID: 16579
		private AnimationCurve lerpAlpha;

		// Token: 0x040040C4 RID: 16580
		public bool isMoving;

		// Token: 0x040040C5 RID: 16581
		private Quaternion initLocalRotation = Quaternion.identity;

		// Token: 0x040040C6 RID: 16582
		private Vector3 initLocalPos = Vector3.zero;

		// Token: 0x040040C7 RID: 16583
		private float cycleDuration;

		// Token: 0x040040C8 RID: 16584
		private float distance;

		// Token: 0x040040C9 RID: 16585
		private float currT;

		// Token: 0x040040CA RID: 16586
		private float percent;

		// Token: 0x040040CB RID: 16587
		private bool currForward;

		// Token: 0x040040CC RID: 16588
		private float dtSinceServerUpdate;

		// Token: 0x040040CD RID: 16589
		private int lastServerTimeStamp;

		// Token: 0x040040CE RID: 16590
		private float rotateStartAmt;

		// Token: 0x040040CF RID: 16591
		private float rotateAmt;

		// Token: 0x040040D0 RID: 16592
		private uint startPercentageCycleOffset;

		// Token: 0x020009F8 RID: 2552
		public enum BuilderMovingPartType
		{
			// Token: 0x040040D2 RID: 16594
			Translation,
			// Token: 0x040040D3 RID: 16595
			Rotation
		}
	}
}
