using System;
using GorillaExtensions;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace com.AnotherAxiom.Paddleball
{
	// Token: 0x02000B24 RID: 2852
	public class Paddleball : ArcadeGame
	{
		// Token: 0x06004716 RID: 18198 RVA: 0x001521AE File Offset: 0x001503AE
		protected override void Awake()
		{
			base.Awake();
			this.yPosToByteFactor = 255f / (2f * this.tableSizeBall.y);
			this.byteToYPosFactor = 1f / this.yPosToByteFactor;
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x001521E8 File Offset: 0x001503E8
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

		// Token: 0x06004718 RID: 18200 RVA: 0x001522AC File Offset: 0x001504AC
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

		// Token: 0x06004719 RID: 18201 RVA: 0x00152914 File Offset: 0x00150B14
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

		// Token: 0x0600471A RID: 18202 RVA: 0x0015297E File Offset: 0x00150B7E
		private float ByteToYPos(byte Y)
		{
			return (float)Y / this.yPosToByteFactor - this.tableSizeBall.y;
		}

		// Token: 0x0600471B RID: 18203 RVA: 0x00152995 File Offset: 0x00150B95
		private byte YPosToByte(float Y)
		{
			return (byte)Mathf.RoundToInt((Y + this.tableSizeBall.y) * this.yPosToByteFactor);
		}

		// Token: 0x0600471C RID: 18204 RVA: 0x001529B4 File Offset: 0x00150BB4
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

		// Token: 0x0600471D RID: 18205 RVA: 0x00152B68 File Offset: 0x00150D68
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

		// Token: 0x0600471E RID: 18206 RVA: 0x000023F4 File Offset: 0x000005F4
		protected override void ButtonUp(int player, ArcadeButtons button)
		{
		}

		// Token: 0x0600471F RID: 18207 RVA: 0x000023F4 File Offset: 0x000005F4
		protected override void ButtonDown(int player, ArcadeButtons button)
		{
		}

		// Token: 0x06004720 RID: 18208 RVA: 0x00152CE4 File Offset: 0x00150EE4
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

		// Token: 0x06004721 RID: 18209 RVA: 0x00152E1B File Offset: 0x0015101B
		public override void OnTimeout()
		{
			this.ChangeScreen(Paddleball.ScreenMode.Title);
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x00152E24 File Offset: 0x00151024
		public override void ReadPlayerDataPUN(int player, PhotonStream stream, PhotonMessageInfo info)
		{
			this.requestedPos[player] = this.ByteToYPos((byte)stream.ReceiveNext());
		}

		// Token: 0x06004723 RID: 18211 RVA: 0x00152E3F File Offset: 0x0015103F
		public override void WritePlayerDataPUN(int player, PhotonStream stream, PhotonMessageInfo info)
		{
			stream.SendNext(this.YPosToByte(this.requestedPos[player]));
		}

		// Token: 0x040048A0 RID: 18592
		[SerializeField]
		private PaddleballPaddle[] p;

		// Token: 0x040048A1 RID: 18593
		private float[] requestedPos = new float[4];

		// Token: 0x040048A2 RID: 18594
		private float[] officialPos = new float[4];

		// Token: 0x040048A3 RID: 18595
		[SerializeField]
		private Transform ball;

		// Token: 0x040048A4 RID: 18596
		[SerializeField]
		private Vector2 ballTrajectory;

		// Token: 0x040048A5 RID: 18597
		[SerializeField]
		private float paddleSpeed = 1f;

		// Token: 0x040048A6 RID: 18598
		[SerializeField]
		private float initialBallSpeed = 1f;

		// Token: 0x040048A7 RID: 18599
		[SerializeField]
		private float ballSpeedBoost = 0.02f;

		// Token: 0x040048A8 RID: 18600
		private float gameBallSpeed = 1f;

		// Token: 0x040048A9 RID: 18601
		[SerializeField]
		private Vector2 tableSizeBall;

		// Token: 0x040048AA RID: 18602
		[SerializeField]
		private Vector2 tableSizePaddle;

		// Token: 0x040048AB RID: 18603
		[SerializeField]
		private GameObject blackWinScreen;

		// Token: 0x040048AC RID: 18604
		[SerializeField]
		private GameObject whiteWinScreen;

		// Token: 0x040048AD RID: 18605
		[SerializeField]
		private GameObject titleScreen;

		// Token: 0x040048AE RID: 18606
		[SerializeField]
		private float winScreenDuration;

		// Token: 0x040048AF RID: 18607
		private float returnToTitleAfterTimestamp;

		// Token: 0x040048B0 RID: 18608
		private int scoreL;

		// Token: 0x040048B1 RID: 18609
		private int scoreR;

		// Token: 0x040048B2 RID: 18610
		private string scoreFormat;

		// Token: 0x040048B3 RID: 18611
		[SerializeField]
		private TMP_Text scoreDisplay;

		// Token: 0x040048B4 RID: 18612
		private float[] paddleIdle;

		// Token: 0x040048B5 RID: 18613
		private Paddleball.ScreenMode currentScreenMode;

		// Token: 0x040048B6 RID: 18614
		private const int AUDIO_WALLBOUNCE = 0;

		// Token: 0x040048B7 RID: 18615
		private const int AUDIO_PADDLEBOUNCE = 1;

		// Token: 0x040048B8 RID: 18616
		private const int AUDIO_SCORE = 2;

		// Token: 0x040048B9 RID: 18617
		private const int AUDIO_WIN = 3;

		// Token: 0x040048BA RID: 18618
		private const int AUDIO_PLAYERJOIN = 4;

		// Token: 0x040048BB RID: 18619
		private const int VAR_REQUESTEDPOS = 0;

		// Token: 0x040048BC RID: 18620
		private const int MAXSCORE = 10;

		// Token: 0x040048BD RID: 18621
		private float yPosToByteFactor;

		// Token: 0x040048BE RID: 18622
		private float byteToYPosFactor;

		// Token: 0x040048BF RID: 18623
		private const float directionToByteFactor = 127.5f;

		// Token: 0x040048C0 RID: 18624
		private const float byteToDirectionFactor = 0.007843138f;

		// Token: 0x040048C1 RID: 18625
		private Paddleball.PaddleballNetState netStateLast;

		// Token: 0x040048C2 RID: 18626
		private Paddleball.PaddleballNetState netStateCur;

		// Token: 0x02000B25 RID: 2853
		private enum ScreenMode
		{
			// Token: 0x040048C4 RID: 18628
			Title,
			// Token: 0x040048C5 RID: 18629
			Gameplay,
			// Token: 0x040048C6 RID: 18630
			WhiteWin,
			// Token: 0x040048C7 RID: 18631
			BlackWin
		}

		// Token: 0x02000B26 RID: 2854
		[Serializable]
		private struct PaddleballNetState : IEquatable<Paddleball.PaddleballNetState>
		{
			// Token: 0x06004725 RID: 18213 RVA: 0x00152EB4 File Offset: 0x001510B4
			public bool Equals(Paddleball.PaddleballNetState other)
			{
				return this.P0LocY == other.P0LocY && this.P1LocY == other.P1LocY && this.P2LocY == other.P2LocY && this.P3LocY == other.P3LocY && this.BallLocX.Approx(other.BallLocX, 1E-06f) && this.BallLocY == other.BallLocY && this.BallTrajectoryX == other.BallTrajectoryX && this.BallTrajectoryY == other.BallTrajectoryY && this.BallSpeed.Approx(other.BallSpeed, 1E-06f) && this.ScoreLeft == other.ScoreLeft && this.ScoreRight == other.ScoreRight && this.ScreenMode == other.ScreenMode;
			}

			// Token: 0x040048C8 RID: 18632
			public byte P0LocY;

			// Token: 0x040048C9 RID: 18633
			public byte P1LocY;

			// Token: 0x040048CA RID: 18634
			public byte P2LocY;

			// Token: 0x040048CB RID: 18635
			public byte P3LocY;

			// Token: 0x040048CC RID: 18636
			public float BallLocX;

			// Token: 0x040048CD RID: 18637
			public byte BallLocY;

			// Token: 0x040048CE RID: 18638
			public byte BallTrajectoryX;

			// Token: 0x040048CF RID: 18639
			public byte BallTrajectoryY;

			// Token: 0x040048D0 RID: 18640
			public float BallSpeed;

			// Token: 0x040048D1 RID: 18641
			public int ScoreLeft;

			// Token: 0x040048D2 RID: 18642
			public int ScoreRight;

			// Token: 0x040048D3 RID: 18643
			public int ScreenMode;
		}
	}
}
