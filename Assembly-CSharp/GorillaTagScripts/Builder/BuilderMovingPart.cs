using System;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A24 RID: 2596
	public class BuilderMovingPart : MonoBehaviour
	{
		// Token: 0x060040F8 RID: 16632 RVA: 0x0016F1BC File Offset: 0x0016D3BC
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

		// Token: 0x060040F9 RID: 16633 RVA: 0x0005A7C0 File Offset: 0x000589C0
		private long NetworkTimeMs()
		{
			if (PhotonNetwork.InRoom)
			{
				return (long)((ulong)(PhotonNetwork.ServerTimestamp - this.myPiece.activatedTimeStamp + (int)this.startPercentageCycleOffset + int.MinValue));
			}
			return (long)(Time.time * 1000f);
		}

		// Token: 0x060040FA RID: 16634 RVA: 0x0005A7F5 File Offset: 0x000589F5
		private long CycleLengthMs()
		{
			return (long)(this.cycleDuration * 1000f);
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x0016F210 File Offset: 0x0016D410
		public double PlatformTime()
		{
			long num = this.NetworkTimeMs();
			long num2 = this.CycleLengthMs();
			return (double)(num - num / num2 * num2) / 1000.0;
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x0005A804 File Offset: 0x00058A04
		public int CycleCount()
		{
			return (int)(this.NetworkTimeMs() / this.CycleLengthMs());
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x0005A814 File Offset: 0x00058A14
		public float CycleCompletionPercent()
		{
			return Mathf.Clamp((float)(this.PlatformTime() / (double)this.cycleDuration), 0f, 1f);
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x0005A834 File Offset: 0x00058A34
		public bool IsEvenCycle()
		{
			return this.CycleCount() % 2 == 0;
		}

		// Token: 0x060040FF RID: 16639 RVA: 0x0016F23C File Offset: 0x0016D43C
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

		// Token: 0x06004100 RID: 16640 RVA: 0x0016F2F4 File Offset: 0x0016D4F4
		public int GetTimeOffsetMS()
		{
			int num = PhotonNetwork.ServerTimestamp - this.myPiece.activatedTimeStamp;
			uint num2 = (uint)(this.CycleLengthMs() * 2L);
			return num % (int)num2;
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x0016F320 File Offset: 0x0016D520
		public byte GetNearestNode()
		{
			int num = Mathf.RoundToInt(this.currT * (float)BuilderMovingPart.NUM_PAUSE_NODES);
			if (!this.IsEvenCycle())
			{
				num += BuilderMovingPart.NUM_PAUSE_NODES;
			}
			return (byte)num;
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x0005A841 File Offset: 0x00058A41
		public byte GetStartNode()
		{
			return (byte)Mathf.RoundToInt(this.startPercentage * (float)BuilderMovingPart.NUM_PAUSE_NODES);
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x0016F354 File Offset: 0x0016D554
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

		// Token: 0x06004104 RID: 16644 RVA: 0x0016F3EC File Offset: 0x0016D5EC
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

		// Token: 0x06004105 RID: 16645 RVA: 0x0016F428 File Offset: 0x0016D628
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

		// Token: 0x06004106 RID: 16646 RVA: 0x0016F590 File Offset: 0x0016D790
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

		// Token: 0x06004107 RID: 16647 RVA: 0x0016F5E0 File Offset: 0x0016D7E0
		private Vector3 UpdatePointToPoint(float perc)
		{
			float t = this.lerpAlpha.Evaluate(perc);
			return Vector3.Lerp(this.startXf.localPosition, this.endXf.localPosition, t);
		}

		// Token: 0x06004108 RID: 16648 RVA: 0x001550EC File Offset: 0x001532EC
		private void UpdateRotation(float perc)
		{
			Quaternion localRotation = Quaternion.AngleAxis(perc * 360f, Vector3.up);
			base.transform.localRotation = localRotation;
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x0005A856 File Offset: 0x00058A56
		private void ResetMovingGrid()
		{
			base.transform.SetLocalPositionAndRotation(this.initLocalPos, this.initLocalRotation);
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x0016F618 File Offset: 0x0016D818
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

		// Token: 0x0600410B RID: 16651 RVA: 0x0016F690 File Offset: 0x0016D890
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

		// Token: 0x0600410C RID: 16652 RVA: 0x0005A86F File Offset: 0x00058A6F
		public void OnPieceDestroy()
		{
			this.ResetMovingGrid();
		}

		// Token: 0x040041B2 RID: 16818
		public BuilderPiece myPiece;

		// Token: 0x040041B3 RID: 16819
		public BuilderAttachGridPlane[] myGridPlanes;

		// Token: 0x040041B4 RID: 16820
		[SerializeField]
		private BuilderMovingPart.BuilderMovingPartType moveType;

		// Token: 0x040041B5 RID: 16821
		[SerializeField]
		private float startPercentage = 0.5f;

		// Token: 0x040041B6 RID: 16822
		[SerializeField]
		private float velocity;

		// Token: 0x040041B7 RID: 16823
		[SerializeField]
		private bool reverseDirOnCycle = true;

		// Token: 0x040041B8 RID: 16824
		[SerializeField]
		private bool reverseDir;

		// Token: 0x040041B9 RID: 16825
		[SerializeField]
		private float cycleDelay = 0.25f;

		// Token: 0x040041BA RID: 16826
		[SerializeField]
		protected Transform startXf;

		// Token: 0x040041BB RID: 16827
		[SerializeField]
		protected Transform endXf;

		// Token: 0x040041BC RID: 16828
		public static int NUM_PAUSE_NODES = 32;

		// Token: 0x040041BD RID: 16829
		private AnimationCurve lerpAlpha;

		// Token: 0x040041BE RID: 16830
		public bool isMoving;

		// Token: 0x040041BF RID: 16831
		private Quaternion initLocalRotation = Quaternion.identity;

		// Token: 0x040041C0 RID: 16832
		private Vector3 initLocalPos = Vector3.zero;

		// Token: 0x040041C1 RID: 16833
		private float cycleDuration;

		// Token: 0x040041C2 RID: 16834
		private float distance;

		// Token: 0x040041C3 RID: 16835
		private float currT;

		// Token: 0x040041C4 RID: 16836
		private float percent;

		// Token: 0x040041C5 RID: 16837
		private bool currForward;

		// Token: 0x040041C6 RID: 16838
		private float dtSinceServerUpdate;

		// Token: 0x040041C7 RID: 16839
		private int lastServerTimeStamp;

		// Token: 0x040041C8 RID: 16840
		private float rotateStartAmt;

		// Token: 0x040041C9 RID: 16841
		private float rotateAmt;

		// Token: 0x040041CA RID: 16842
		private uint startPercentageCycleOffset;

		// Token: 0x02000A25 RID: 2597
		public enum BuilderMovingPartType
		{
			// Token: 0x040041CC RID: 16844
			Translation,
			// Token: 0x040041CD RID: 16845
			Rotation
		}
	}
}
