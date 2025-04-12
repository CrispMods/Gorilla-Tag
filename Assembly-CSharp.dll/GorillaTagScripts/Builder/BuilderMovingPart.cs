using System;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x020009FA RID: 2554
	public class BuilderMovingPart : MonoBehaviour
	{
		// Token: 0x06003FBF RID: 16319 RVA: 0x001683C8 File Offset: 0x001665C8
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

		// Token: 0x06003FC0 RID: 16320 RVA: 0x00058DBE File Offset: 0x00056FBE
		private long NetworkTimeMs()
		{
			if (PhotonNetwork.InRoom)
			{
				return (long)((ulong)(PhotonNetwork.ServerTimestamp - this.myPiece.activatedTimeStamp + (int)this.startPercentageCycleOffset + int.MinValue));
			}
			return (long)(Time.time * 1000f);
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x00058DF3 File Offset: 0x00056FF3
		private long CycleLengthMs()
		{
			return (long)(this.cycleDuration * 1000f);
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x0016841C File Offset: 0x0016661C
		public double PlatformTime()
		{
			long num = this.NetworkTimeMs();
			long num2 = this.CycleLengthMs();
			return (double)(num - num / num2 * num2) / 1000.0;
		}

		// Token: 0x06003FC3 RID: 16323 RVA: 0x00058E02 File Offset: 0x00057002
		public int CycleCount()
		{
			return (int)(this.NetworkTimeMs() / this.CycleLengthMs());
		}

		// Token: 0x06003FC4 RID: 16324 RVA: 0x00058E12 File Offset: 0x00057012
		public float CycleCompletionPercent()
		{
			return Mathf.Clamp((float)(this.PlatformTime() / (double)this.cycleDuration), 0f, 1f);
		}

		// Token: 0x06003FC5 RID: 16325 RVA: 0x00058E32 File Offset: 0x00057032
		public bool IsEvenCycle()
		{
			return this.CycleCount() % 2 == 0;
		}

		// Token: 0x06003FC6 RID: 16326 RVA: 0x00168448 File Offset: 0x00166648
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

		// Token: 0x06003FC7 RID: 16327 RVA: 0x00168500 File Offset: 0x00166700
		public int GetTimeOffsetMS()
		{
			int num = PhotonNetwork.ServerTimestamp - this.myPiece.activatedTimeStamp;
			uint num2 = (uint)(this.CycleLengthMs() * 2L);
			return num % (int)num2;
		}

		// Token: 0x06003FC8 RID: 16328 RVA: 0x0016852C File Offset: 0x0016672C
		public byte GetNearestNode()
		{
			int num = Mathf.RoundToInt(this.currT * (float)BuilderMovingPart.NUM_PAUSE_NODES);
			if (!this.IsEvenCycle())
			{
				num += BuilderMovingPart.NUM_PAUSE_NODES;
			}
			return (byte)num;
		}

		// Token: 0x06003FC9 RID: 16329 RVA: 0x00058E3F File Offset: 0x0005703F
		public byte GetStartNode()
		{
			return (byte)Mathf.RoundToInt(this.startPercentage * (float)BuilderMovingPart.NUM_PAUSE_NODES);
		}

		// Token: 0x06003FCA RID: 16330 RVA: 0x00168560 File Offset: 0x00166760
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

		// Token: 0x06003FCB RID: 16331 RVA: 0x001685F8 File Offset: 0x001667F8
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

		// Token: 0x06003FCC RID: 16332 RVA: 0x00168634 File Offset: 0x00166834
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

		// Token: 0x06003FCD RID: 16333 RVA: 0x0016879C File Offset: 0x0016699C
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

		// Token: 0x06003FCE RID: 16334 RVA: 0x001687EC File Offset: 0x001669EC
		private Vector3 UpdatePointToPoint(float perc)
		{
			float t = this.lerpAlpha.Evaluate(perc);
			return Vector3.Lerp(this.startXf.localPosition, this.endXf.localPosition, t);
		}

		// Token: 0x06003FCF RID: 16335 RVA: 0x0014F104 File Offset: 0x0014D304
		private void UpdateRotation(float perc)
		{
			Quaternion localRotation = Quaternion.AngleAxis(perc * 360f, Vector3.up);
			base.transform.localRotation = localRotation;
		}

		// Token: 0x06003FD0 RID: 16336 RVA: 0x00058E54 File Offset: 0x00057054
		private void ResetMovingGrid()
		{
			base.transform.SetLocalPositionAndRotation(this.initLocalPos, this.initLocalRotation);
		}

		// Token: 0x06003FD1 RID: 16337 RVA: 0x00168824 File Offset: 0x00166A24
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

		// Token: 0x06003FD2 RID: 16338 RVA: 0x0016889C File Offset: 0x00166A9C
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

		// Token: 0x06003FD3 RID: 16339 RVA: 0x00058E6D File Offset: 0x0005706D
		public void OnPieceDestroy()
		{
			this.ResetMovingGrid();
		}

		// Token: 0x040040CA RID: 16586
		public BuilderPiece myPiece;

		// Token: 0x040040CB RID: 16587
		public BuilderAttachGridPlane[] myGridPlanes;

		// Token: 0x040040CC RID: 16588
		[SerializeField]
		private BuilderMovingPart.BuilderMovingPartType moveType;

		// Token: 0x040040CD RID: 16589
		[SerializeField]
		private float startPercentage = 0.5f;

		// Token: 0x040040CE RID: 16590
		[SerializeField]
		private float velocity;

		// Token: 0x040040CF RID: 16591
		[SerializeField]
		private bool reverseDirOnCycle = true;

		// Token: 0x040040D0 RID: 16592
		[SerializeField]
		private bool reverseDir;

		// Token: 0x040040D1 RID: 16593
		[SerializeField]
		private float cycleDelay = 0.25f;

		// Token: 0x040040D2 RID: 16594
		[SerializeField]
		protected Transform startXf;

		// Token: 0x040040D3 RID: 16595
		[SerializeField]
		protected Transform endXf;

		// Token: 0x040040D4 RID: 16596
		public static int NUM_PAUSE_NODES = 32;

		// Token: 0x040040D5 RID: 16597
		private AnimationCurve lerpAlpha;

		// Token: 0x040040D6 RID: 16598
		public bool isMoving;

		// Token: 0x040040D7 RID: 16599
		private Quaternion initLocalRotation = Quaternion.identity;

		// Token: 0x040040D8 RID: 16600
		private Vector3 initLocalPos = Vector3.zero;

		// Token: 0x040040D9 RID: 16601
		private float cycleDuration;

		// Token: 0x040040DA RID: 16602
		private float distance;

		// Token: 0x040040DB RID: 16603
		private float currT;

		// Token: 0x040040DC RID: 16604
		private float percent;

		// Token: 0x040040DD RID: 16605
		private bool currForward;

		// Token: 0x040040DE RID: 16606
		private float dtSinceServerUpdate;

		// Token: 0x040040DF RID: 16607
		private int lastServerTimeStamp;

		// Token: 0x040040E0 RID: 16608
		private float rotateStartAmt;

		// Token: 0x040040E1 RID: 16609
		private float rotateAmt;

		// Token: 0x040040E2 RID: 16610
		private uint startPercentageCycleOffset;

		// Token: 0x020009FB RID: 2555
		public enum BuilderMovingPartType
		{
			// Token: 0x040040E4 RID: 16612
			Translation,
			// Token: 0x040040E5 RID: 16613
			Rotation
		}
	}
}
