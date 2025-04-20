using System;
using GorillaExtensions;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace com.AnotherAxiom.Paddleball
{
	// Token: 0x02000B51 RID: 2897
	public class Paddleball : ArcadeGame
	{
		// Token: 0x0600485F RID: 18527 RVA: 0x0005F223 File Offset: 0x0005D423
		protected override void Awake()
		{
			base.Awake();
			this.yPosToByteFactor = 255f / (2f * this.tableSizeBall.y);
			this.byteToYPosFactor = 1f / this.yPosToByteFactor;
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x0018E790 File Offset: 0x0018C990
		private void Start()
		{
			this.whiteWinScreen.SetActive(false);
			this.blackWinScreen.SetActive(false);
			this.titleScreen.SetActive(true);
			this.ball.gameObject.SetActive(false);
			this.currentScreenMode = Paddleball.ScreenMode.Title;
			this.paddleIdle = new float[this.p.Length];
			for (int i = 0; i < this.p.Length; i++)
			{
				this.p[i].gameObject.SetActive(false);
				this.paddleIdle[i] = 30f;
			}
			this.gameBallSpeed = this.initialBallSpeed;
			this.scoreR = (this.scoreL = 0);
			this.scoreFormat = this.scoreDisplay.text;
			this.UpdateScore();
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x0018E854 File Offset: 0x0018CA54
		private void Update()
		{
			if (this.currentScreenMode == Paddleball.ScreenMode.Gameplay)
			{
				this.ball.Translate(this.ballTrajectory.normalized * Time.deltaTime * this.gameBallSpeed);
				if (this.ball.localPosition.y > this.tableSizeBall.y)
				{
					this.ball.localPosition = new Vector3(this.ball.localPosition.x, this.tableSizeBall.y, this.ball.localPosition.z);
					this.ballTrajectory.y = -this.ballTrajectory.y;
					base.PlaySound(0, 3);
				}
				if (this.ball.localPosition.y < -this.tableSizeBall.y)
				{
					this.ball.localPosition = new Vector3(this.ball.localPosition.x, -this.tableSizeBall.y, this.ball.localPosition.z);
					this.ballTrajectory.y = -this.ballTrajectory.y;
					base.PlaySound(0, 3);
				}
				if (this.ball.localPosition.x > this.tableSizeBall.x)
				{
					this.ball.localPosition = new Vector3(this.tableSizeBall.x, this.ball.localPosition.y, this.ball.localPosition.z);
					this.ballTrajectory.x = -this.ballTrajectory.x;
					this.gameBallSpeed = this.initialBallSpeed;
					this.scoreL++;
					this.UpdateScore();
					base.PlaySound(2, 3);
					if (this.scoreL >= 10)
					{
						this.ChangeScreen(Paddleball.ScreenMode.WhiteWin);
					}
				}
				if (this.ball.localPosition.x < -this.tableSizeBall.x)
				{
					this.ball.localPosition = new Vector3(-this.tableSizeBall.x, this.ball.localPosition.y, this.ball.localPosition.z);
					this.ballTrajectory.x = -this.ballTrajectory.x;
					this.gameBallSpeed = this.initialBallSpeed;
					this.scoreR++;
					this.UpdateScore();
					base.PlaySound(2, 3);
					if (this.scoreR >= 10)
					{
						this.ChangeScreen(Paddleball.ScreenMode.BlackWin);
					}
				}
			}
			if (this.returnToTitleAfterTimestamp != 0f && Time.time > this.returnToTitleAfterTimestamp)
			{
				this.ChangeScreen(Paddleball.ScreenMode.Title);
			}
			for (int i = 0; i < this.p.Length; i++)
			{
				if (base.IsPlayerLocallyControlled(i))
				{
					float num = this.requestedPos[i];
					if (base.getButtonState(i, ArcadeButtons.UP))
					{
						this.requestedPos[i] += Time.deltaTime * this.paddleSpeed;
					}
					else if (base.getButtonState(i, ArcadeButtons.DOWN))
					{
						this.requestedPos[i] -= Time.deltaTime * this.paddleSpeed;
					}
					this.requestedPos[i] = Mathf.Clamp(this.requestedPos[i], -this.tableSizePaddle.y, this.tableSizePaddle.y);
				}
				float value;
				if (!NetworkSystem.Instance.InRoom || NetworkSystem.Instance.IsMasterClient)
				{
					value = Mathf.MoveTowards(this.p[i].transform.localPosition.y, this.requestedPos[i], Time.deltaTime * this.paddleSpeed);
				}
				else
				{
					value = Mathf.MoveTowards(this.p[i].transform.localPosition.y, this.officialPos[i], Time.deltaTime * this.paddleSpeed);
				}
				this.p[i].transform.localPosition = this.p[i].transform.localPosition.WithY(Mathf.Clamp(value, -this.tableSizePaddle.y, this.tableSizePaddle.y));
				if (base.getButtonState(i, ArcadeButtons.GRAB))
				{
					this.paddleIdle[i] = 0f;
					Paddleball.ScreenMode screenMode = this.currentScreenMode;
					if (screenMode != Paddleball.ScreenMode.Title)
					{
						if (screenMode == Paddleball.ScreenMode.Gameplay)
						{
							this.returnToTitleAfterTimestamp = Time.time + 30f;
						}
					}
					else
					{
						this.ChangeScreen(Paddleball.ScreenMode.Gameplay);
					}
				}
				else
				{
					this.paddleIdle[i] += Time.deltaTime;
				}
				bool flag = this.paddleIdle[i] < 30f;
				if (this.p[i].gameObject.activeSelf != flag)
				{
					if (flag)
					{
						base.PlaySound(4, 3);
						Vector3 localPosition = this.p[i].transform.localPosition;
						localPosition.y = 0f;
						this.requestedPos[i] = localPosition.y;
						this.p[i].transform.localPosition = localPosition;
					}
					this.p[i].gameObject.SetActive(this.paddleIdle[i] < 30f);
				}
				if (this.p[i].gameObject.activeInHierarchy && Mathf.Abs(this.ball.localPosition.x - this.p[i].transform.localPosition.x) < 0.1f && Mathf.Abs(this.ball.localPosition.y - this.p[i].transform.localPosition.y) < 0.5f)
				{
					this.ballTrajectory.y = (this.ball.localPosition.y - this.p[i].transform.localPosition.y) * 1.25f;
					float x = this.ballTrajectory.x;
					if (this.p[i].Right)
					{
						this.ballTrajectory.x = Mathf.Abs(this.ballTrajectory.y) - 1f;
					}
					else
					{
						this.ballTrajectory.x = 1f - Mathf.Abs(this.ballTrajectory.y);
					}
					if (x > 0f != this.ballTrajectory.x > 0f)
					{
						base.PlaySound(1, 3);
					}
					this.ballTrajectory.Normalize();
					this.gameBallSpeed += this.ballSpeedBoost;
				}
			}
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x0018EEBC File Offset: 0x0018D0BC
		private void UpdateScore()
		{
			if (this.scoreFormat == null)
			{
				return;
			}
			this.scoreL = Mathf.Clamp(this.scoreL, 0, 10);
			this.scoreR = Mathf.Clamp(this.scoreR, 0, 10);
			this.scoreDisplay.text = string.Format(this.scoreFormat, this.scoreL, this.scoreR);
		}

		// Token: 0x06004863 RID: 18531 RVA: 0x0005F25A File Offset: 0x0005D45A
		private float ByteToYPos(byte Y)
		{
			return (float)Y / this.yPosToByteFactor - this.tableSizeBall.y;
		}

		// Token: 0x06004864 RID: 18532 RVA: 0x0005F271 File Offset: 0x0005D471
		private byte YPosToByte(float Y)
		{
			return (byte)Mathf.RoundToInt((Y + this.tableSizeBall.y) * this.yPosToByteFactor);
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x0018EF28 File Offset: 0x0018D128
		public override byte[] GetNetworkState()
		{
			this.netStateCur.P0LocY = this.YPosToByte(this.p[0].transform.localPosition.y);
			this.netStateCur.P1LocY = this.YPosToByte(this.p[1].transform.localPosition.y);
			this.netStateCur.P2LocY = this.YPosToByte(this.p[2].transform.localPosition.y);
			this.netStateCur.P3LocY = this.YPosToByte(this.p[3].transform.localPosition.y);
			this.netStateCur.BallLocX = this.ball.localPosition.x;
			this.netStateCur.BallLocY = this.YPosToByte(this.ball.localPosition.y);
			this.netStateCur.BallTrajectoryX = (byte)((this.ballTrajectory.x + 1f) * 127.5f);
			this.netStateCur.BallTrajectoryY = (byte)((this.ballTrajectory.y + 1f) * 127.5f);
			this.netStateCur.BallSpeed = this.gameBallSpeed;
			this.netStateCur.ScoreLeft = this.scoreL;
			this.netStateCur.ScoreRight = this.scoreR;
			this.netStateCur.ScreenMode = (int)this.currentScreenMode;
			if (!this.netStateCur.Equals(this.netStateLast))
			{
				this.netStateLast = this.netStateCur;
				base.SwapNetStateBuffersAndStreams();
				ArcadeGame.WrapNetState(this.netStateLast, this.netStateMemStream);
			}
			return this.netStateBuffer;
		}

		// Token: 0x06004866 RID: 18534 RVA: 0x0018F0DC File Offset: 0x0018D2DC
		public override void SetNetworkState(byte[] b)
		{
			Paddleball.PaddleballNetState paddleballNetState = (Paddleball.PaddleballNetState)ArcadeGame.UnwrapNetState(b);
			this.officialPos[0] = this.ByteToYPos(paddleballNetState.P0LocY);
			this.officialPos[1] = this.ByteToYPos(paddleballNetState.P1LocY);
			this.officialPos[2] = this.ByteToYPos(paddleballNetState.P2LocY);
			this.officialPos[3] = this.ByteToYPos(paddleballNetState.P3LocY);
			Vector2 vector = new Vector2(paddleballNetState.BallLocX, this.ByteToYPos(paddleballNetState.BallLocY));
			Vector2 normalized = new Vector2((float)paddleballNetState.BallTrajectoryX * 0.007843138f - 1f, (float)paddleballNetState.BallTrajectoryY * 0.007843138f - 1f).normalized;
			Vector2 a = vector - normalized * Vector2.Dot(vector, normalized);
			Vector2 vector2 = this.ball.localPosition.xy();
			Vector2 b2 = vector2 - this.ballTrajectory * Vector2.Dot(vector2, this.ballTrajectory);
			if ((a - b2).IsLongerThan(0.1f))
			{
				this.ball.localPosition = vector;
				this.ballTrajectory = normalized.xy();
			}
			this.gameBallSpeed = paddleballNetState.BallSpeed;
			this.ChangeScreen((Paddleball.ScreenMode)paddleballNetState.ScreenMode);
			if (this.scoreL != paddleballNetState.ScoreLeft || this.scoreR != paddleballNetState.ScoreRight)
			{
				this.scoreL = paddleballNetState.ScoreLeft;
				this.scoreR = paddleballNetState.ScoreRight;
				this.UpdateScore();
			}
		}

		// Token: 0x06004867 RID: 18535 RVA: 0x00030607 File Offset: 0x0002E807
		protected override void ButtonUp(int player, ArcadeButtons button)
		{
		}

		// Token: 0x06004868 RID: 18536 RVA: 0x00030607 File Offset: 0x0002E807
		protected override void ButtonDown(int player, ArcadeButtons button)
		{
		}

		// Token: 0x06004869 RID: 18537 RVA: 0x0018F258 File Offset: 0x0018D458
		private void ChangeScreen(Paddleball.ScreenMode mode)
		{
			if (this.currentScreenMode == mode)
			{
				return;
			}
			switch (this.currentScreenMode)
			{
			case Paddleball.ScreenMode.Title:
				this.titleScreen.SetActive(false);
				break;
			case Paddleball.ScreenMode.Gameplay:
				this.ball.gameObject.SetActive(false);
				break;
			case Paddleball.ScreenMode.WhiteWin:
				this.whiteWinScreen.SetActive(false);
				break;
			case Paddleball.ScreenMode.BlackWin:
				this.blackWinScreen.SetActive(false);
				break;
			}
			this.currentScreenMode = mode;
			switch (mode)
			{
			case Paddleball.ScreenMode.Title:
				this.gameBallSpeed = this.initialBallSpeed;
				this.scoreL = 0;
				this.scoreR = 0;
				this.UpdateScore();
				this.returnToTitleAfterTimestamp = 0f;
				this.titleScreen.SetActive(true);
				return;
			case Paddleball.ScreenMode.Gameplay:
				this.ball.gameObject.SetActive(true);
				this.returnToTitleAfterTimestamp = Time.time + 30f;
				return;
			case Paddleball.ScreenMode.WhiteWin:
				this.whiteWinScreen.SetActive(true);
				this.returnToTitleAfterTimestamp = Time.time + this.winScreenDuration;
				base.PlaySound(3, 3);
				return;
			case Paddleball.ScreenMode.BlackWin:
				this.blackWinScreen.SetActive(true);
				this.returnToTitleAfterTimestamp = Time.time + this.winScreenDuration;
				base.PlaySound(3, 3);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600486A RID: 18538 RVA: 0x0005F28D File Offset: 0x0005D48D
		public override void OnTimeout()
		{
			this.ChangeScreen(Paddleball.ScreenMode.Title);
		}

		// Token: 0x0600486B RID: 18539 RVA: 0x0005F296 File Offset: 0x0005D496
		public override void ReadPlayerDataPUN(int player, PhotonStream stream, PhotonMessageInfo info)
		{
			this.requestedPos[player] = this.ByteToYPos((byte)stream.ReceiveNext());
		}

		// Token: 0x0600486C RID: 18540 RVA: 0x0005F2B1 File Offset: 0x0005D4B1
		public override void WritePlayerDataPUN(int player, PhotonStream stream, PhotonMessageInfo info)
		{
			stream.SendNext(this.YPosToByte(this.requestedPos[player]));
		}

		// Token: 0x04004995 RID: 18837
		[SerializeField]
		private PaddleballPaddle[] p;

		// Token: 0x04004996 RID: 18838
		private float[] requestedPos = new float[4];

		// Token: 0x04004997 RID: 18839
		private float[] officialPos = new float[4];

		// Token: 0x04004998 RID: 18840
		[SerializeField]
		private Transform ball;

		// Token: 0x04004999 RID: 18841
		[SerializeField]
		private Vector2 ballTrajectory;

		// Token: 0x0400499A RID: 18842
		[SerializeField]
		private float paddleSpeed = 1f;

		// Token: 0x0400499B RID: 18843
		[SerializeField]
		private float initialBallSpeed = 1f;

		// Token: 0x0400499C RID: 18844
		[SerializeField]
		private float ballSpeedBoost = 0.02f;

		// Token: 0x0400499D RID: 18845
		private float gameBallSpeed = 1f;

		// Token: 0x0400499E RID: 18846
		[SerializeField]
		private Vector2 tableSizeBall;

		// Token: 0x0400499F RID: 18847
		[SerializeField]
		private Vector2 tableSizePaddle;

		// Token: 0x040049A0 RID: 18848
		[SerializeField]
		private GameObject blackWinScreen;

		// Token: 0x040049A1 RID: 18849
		[SerializeField]
		private GameObject whiteWinScreen;

		// Token: 0x040049A2 RID: 18850
		[SerializeField]
		private GameObject titleScreen;

		// Token: 0x040049A3 RID: 18851
		[SerializeField]
		private float winScreenDuration;

		// Token: 0x040049A4 RID: 18852
		private float returnToTitleAfterTimestamp;

		// Token: 0x040049A5 RID: 18853
		private int scoreL;

		// Token: 0x040049A6 RID: 18854
		private int scoreR;

		// Token: 0x040049A7 RID: 18855
		private string scoreFormat;

		// Token: 0x040049A8 RID: 18856
		[SerializeField]
		private TMP_Text scoreDisplay;

		// Token: 0x040049A9 RID: 18857
		private float[] paddleIdle;

		// Token: 0x040049AA RID: 18858
		private Paddleball.ScreenMode currentScreenMode;

		// Token: 0x040049AB RID: 18859
		private const int AUDIO_WALLBOUNCE = 0;

		// Token: 0x040049AC RID: 18860
		private const int AUDIO_PADDLEBOUNCE = 1;

		// Token: 0x040049AD RID: 18861
		private const int AUDIO_SCORE = 2;

		// Token: 0x040049AE RID: 18862
		private const int AUDIO_WIN = 3;

		// Token: 0x040049AF RID: 18863
		private const int AUDIO_PLAYERJOIN = 4;

		// Token: 0x040049B0 RID: 18864
		private const int VAR_REQUESTEDPOS = 0;

		// Token: 0x040049B1 RID: 18865
		private const int MAXSCORE = 10;

		// Token: 0x040049B2 RID: 18866
		private float yPosToByteFactor;

		// Token: 0x040049B3 RID: 18867
		private float byteToYPosFactor;

		// Token: 0x040049B4 RID: 18868
		private const float directionToByteFactor = 127.5f;

		// Token: 0x040049B5 RID: 18869
		private const float byteToDirectionFactor = 0.007843138f;

		// Token: 0x040049B6 RID: 18870
		private Paddleball.PaddleballNetState netStateLast;

		// Token: 0x040049B7 RID: 18871
		private Paddleball.PaddleballNetState netStateCur;

		// Token: 0x02000B52 RID: 2898
		private enum ScreenMode
		{
			// Token: 0x040049B9 RID: 18873
			Title,
			// Token: 0x040049BA RID: 18874
			Gameplay,
			// Token: 0x040049BB RID: 18875
			WhiteWin,
			// Token: 0x040049BC RID: 18876
			BlackWin
		}

		// Token: 0x02000B53 RID: 2899
		[Serializable]
		private struct PaddleballNetState : IEquatable<Paddleball.PaddleballNetState>
		{
			// Token: 0x0600486E RID: 18542 RVA: 0x0018F3E8 File Offset: 0x0018D5E8
			public bool Equals(Paddleball.PaddleballNetState other)
			{
				return this.P0LocY == other.P0LocY && this.P1LocY == other.P1LocY && this.P2LocY == other.P2LocY && this.P3LocY == other.P3LocY && this.BallLocX.Approx(other.BallLocX, 1E-06f) && this.BallLocY == other.BallLocY && this.BallTrajectoryX == other.BallTrajectoryX && this.BallTrajectoryY == other.BallTrajectoryY && this.BallSpeed.Approx(other.BallSpeed, 1E-06f) && this.ScoreLeft == other.ScoreLeft && this.ScoreRight == other.ScoreRight && this.ScreenMode == other.ScreenMode;
			}

			// Token: 0x040049BD RID: 18877
			public byte P0LocY;

			// Token: 0x040049BE RID: 18878
			public byte P1LocY;

			// Token: 0x040049BF RID: 18879
			public byte P2LocY;

			// Token: 0x040049C0 RID: 18880
			public byte P3LocY;

			// Token: 0x040049C1 RID: 18881
			public float BallLocX;

			// Token: 0x040049C2 RID: 18882
			public byte BallLocY;

			// Token: 0x040049C3 RID: 18883
			public byte BallTrajectoryX;

			// Token: 0x040049C4 RID: 18884
			public byte BallTrajectoryY;

			// Token: 0x040049C5 RID: 18885
			public float BallSpeed;

			// Token: 0x040049C6 RID: 18886
			public int ScoreLeft;

			// Token: 0x040049C7 RID: 18887
			public int ScoreRight;

			// Token: 0x040049C8 RID: 18888
			public int ScreenMode;
		}
	}
}
