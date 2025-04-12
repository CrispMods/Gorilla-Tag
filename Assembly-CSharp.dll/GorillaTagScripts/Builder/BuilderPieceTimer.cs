using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A07 RID: 2567
	public class BuilderPieceTimer : MonoBehaviour, IBuilderPieceComponent, ITickSystemTick
	{
		// Token: 0x06004036 RID: 16438 RVA: 0x00059231 File Offset: 0x00057431
		private void Awake()
		{
			this.buttonTrigger.TriggeredEvent.AddListener(new UnityAction(this.OnButtonPressed));
		}

		// Token: 0x06004037 RID: 16439 RVA: 0x0005924F File Offset: 0x0005744F
		private void OnDestroy()
		{
			if (this.buttonTrigger != null)
			{
				this.buttonTrigger.TriggeredEvent.RemoveListener(new UnityAction(this.OnButtonPressed));
			}
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x0016AAC0 File Offset: 0x00168CC0
		private void OnButtonPressed()
		{
			if (this.myPiece.state != BuilderPiece.State.AttachedAndPlaced)
			{
				return;
			}
			if (Time.time > this.lastTriggeredTime + this.debounceTime)
			{
				this.lastTriggeredTime = Time.time;
				if (!this.isStart && this.stopSoundBank != null)
				{
					this.stopSoundBank.Play();
				}
				else if (this.activateSoundBank != null)
				{
					this.activateSoundBank.Play();
				}
				if (this.isBoth && this.isStart && this.displayText != null)
				{
					this.displayText.text = "TIME: 00:00:0";
				}
				PlayerTimerManager.instance.RequestTimerToggle(this.isStart);
			}
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x0016AB78 File Offset: 0x00168D78
		private void OnTimerStopped(int actorNum, int timeDelta)
		{
			if (this.isStart && !this.isBoth)
			{
				return;
			}
			double num = timeDelta;
			this.latestTime = num / 1000.0;
			if (this.latestTime > 3599.989990234375)
			{
				this.latestTime = 3599.989990234375;
			}
			this.displayText.text = "TIME: " + TimeSpan.FromSeconds(this.latestTime).ToString("mm\\:ss\\:ff");
			if (this.isBoth && actorNum == NetworkSystem.Instance.LocalPlayer.ActorNumber)
			{
				this.isStart = true;
				if (this.TickRunning)
				{
					TickSystem<object>.RemoveTickCallback(this);
				}
			}
		}

		// Token: 0x0600403A RID: 16442 RVA: 0x0005927B File Offset: 0x0005747B
		private void OnLocalTimerStarted()
		{
			if (this.isBoth)
			{
				this.isStart = false;
			}
			if (this.myPiece.state == BuilderPiece.State.AttachedAndPlaced && !this.TickRunning)
			{
				TickSystem<object>.AddTickCallback(this);
			}
		}

		// Token: 0x0600403B RID: 16443 RVA: 0x0016AC28 File Offset: 0x00168E28
		private void OnZoneChanged()
		{
			bool active = ZoneManagement.instance.IsZoneActive(GTZone.monkeBlocks);
			if (this.displayText != null)
			{
				this.displayText.gameObject.SetActive(active);
			}
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x0016AC64 File Offset: 0x00168E64
		public void OnPieceCreate(int pieceType, int pieceId)
		{
			this.latestTime = double.MaxValue;
			if (this.displayText != null)
			{
				ZoneManagement instance = ZoneManagement.instance;
				instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
				this.OnZoneChanged();
				this.displayText.text = "TIME: __:__:_";
			}
		}

		// Token: 0x0600403D RID: 16445 RVA: 0x000592A7 File Offset: 0x000574A7
		public void OnPieceDestroy()
		{
			if (this.displayText != null)
			{
				ZoneManagement instance = ZoneManagement.instance;
				instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
			}
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x0600403F RID: 16447 RVA: 0x0016ACCC File Offset: 0x00168ECC
		public void OnPieceActivate()
		{
			this.lastTriggeredTime = 0f;
			PlayerTimerManager.instance.OnTimerStopped.AddListener(new UnityAction<int, int>(this.OnTimerStopped));
			PlayerTimerManager.instance.OnLocalTimerStarted.AddListener(new UnityAction(this.OnLocalTimerStarted));
			if (this.isBoth)
			{
				this.isStart = !PlayerTimerManager.instance.IsLocalTimerStarted();
				if (!this.isStart && this.displayText != null)
				{
					this.displayText.text = "TIME: __:__:_";
				}
			}
			if (PlayerTimerManager.instance.IsLocalTimerStarted() && !this.TickRunning)
			{
				TickSystem<object>.AddTickCallback(this);
			}
		}

		// Token: 0x06004040 RID: 16448 RVA: 0x0016AD78 File Offset: 0x00168F78
		public void OnPieceDeactivate()
		{
			if (PlayerTimerManager.instance != null)
			{
				PlayerTimerManager.instance.OnTimerStopped.RemoveListener(new UnityAction<int, int>(this.OnTimerStopped));
				PlayerTimerManager.instance.OnLocalTimerStarted.RemoveListener(new UnityAction(this.OnLocalTimerStarted));
			}
			if (this.TickRunning)
			{
				TickSystem<object>.RemoveTickCallback(this);
			}
			if (this.displayText != null)
			{
				this.displayText.text = "TIME: --:--:-";
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06004041 RID: 16449 RVA: 0x000592DD File Offset: 0x000574DD
		// (set) Token: 0x06004042 RID: 16450 RVA: 0x000592E5 File Offset: 0x000574E5
		public bool TickRunning { get; set; }

		// Token: 0x06004043 RID: 16451 RVA: 0x0016ADF4 File Offset: 0x00168FF4
		public void Tick()
		{
			if (this.displayText != null)
			{
				float num = PlayerTimerManager.instance.GetTimeForPlayer(NetworkSystem.Instance.LocalPlayer.ActorNumber);
				num = Mathf.Clamp(num, 0f, 3599.99f);
				this.displayText.text = "TIME: " + TimeSpan.FromSeconds((double)num).ToString("mm\\:ss\\:f");
			}
		}

		// Token: 0x04004159 RID: 16729
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x0400415A RID: 16730
		[SerializeField]
		private bool isStart;

		// Token: 0x0400415B RID: 16731
		[SerializeField]
		private bool isBoth;

		// Token: 0x0400415C RID: 16732
		[SerializeField]
		private BuilderSmallHandTrigger buttonTrigger;

		// Token: 0x0400415D RID: 16733
		[SerializeField]
		private SoundBankPlayer activateSoundBank;

		// Token: 0x0400415E RID: 16734
		[SerializeField]
		private SoundBankPlayer stopSoundBank;

		// Token: 0x0400415F RID: 16735
		[SerializeField]
		private float debounceTime = 0.5f;

		// Token: 0x04004160 RID: 16736
		private float lastTriggeredTime;

		// Token: 0x04004161 RID: 16737
		private double latestTime = 3.4028234663852886E+38;

		// Token: 0x04004162 RID: 16738
		[SerializeField]
		private TMP_Text displayText;
	}
}
