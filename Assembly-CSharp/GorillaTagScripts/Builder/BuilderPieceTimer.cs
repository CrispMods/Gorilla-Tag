using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A04 RID: 2564
	public class BuilderPieceTimer : MonoBehaviour, IBuilderPieceComponent, ITickSystemTick
	{
		// Token: 0x0600402A RID: 16426 RVA: 0x00130BAF File Offset: 0x0012EDAF
		private void Awake()
		{
			this.buttonTrigger.TriggeredEvent.AddListener(new UnityAction(this.OnButtonPressed));
		}

		// Token: 0x0600402B RID: 16427 RVA: 0x00130BCD File Offset: 0x0012EDCD
		private void OnDestroy()
		{
			if (this.buttonTrigger != null)
			{
				this.buttonTrigger.TriggeredEvent.RemoveListener(new UnityAction(this.OnButtonPressed));
			}
		}

		// Token: 0x0600402C RID: 16428 RVA: 0x00130BFC File Offset: 0x0012EDFC
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

		// Token: 0x0600402D RID: 16429 RVA: 0x00130CB4 File Offset: 0x0012EEB4
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

		// Token: 0x0600402E RID: 16430 RVA: 0x00130D63 File Offset: 0x0012EF63
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

		// Token: 0x0600402F RID: 16431 RVA: 0x00130D90 File Offset: 0x0012EF90
		private void OnZoneChanged()
		{
			bool active = ZoneManagement.instance.IsZoneActive(GTZone.monkeBlocks);
			if (this.displayText != null)
			{
				this.displayText.gameObject.SetActive(active);
			}
		}

		// Token: 0x06004030 RID: 16432 RVA: 0x00130DCC File Offset: 0x0012EFCC
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

		// Token: 0x06004031 RID: 16433 RVA: 0x00130E32 File Offset: 0x0012F032
		public void OnPieceDestroy()
		{
			if (this.displayText != null)
			{
				ZoneManagement instance = ZoneManagement.instance;
				instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
			}
		}

		// Token: 0x06004032 RID: 16434 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x06004033 RID: 16435 RVA: 0x00130E68 File Offset: 0x0012F068
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

		// Token: 0x06004034 RID: 16436 RVA: 0x00130F14 File Offset: 0x0012F114
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

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06004035 RID: 16437 RVA: 0x00130F90 File Offset: 0x0012F190
		// (set) Token: 0x06004036 RID: 16438 RVA: 0x00130F98 File Offset: 0x0012F198
		public bool TickRunning { get; set; }

		// Token: 0x06004037 RID: 16439 RVA: 0x00130FA4 File Offset: 0x0012F1A4
		public void Tick()
		{
			if (this.displayText != null)
			{
				float num = PlayerTimerManager.instance.GetTimeForPlayer(NetworkSystem.Instance.LocalPlayer.ActorNumber);
				num = Mathf.Clamp(num, 0f, 3599.99f);
				this.displayText.text = "TIME: " + TimeSpan.FromSeconds((double)num).ToString("mm\\:ss\\:f");
			}
		}

		// Token: 0x04004147 RID: 16711
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x04004148 RID: 16712
		[SerializeField]
		private bool isStart;

		// Token: 0x04004149 RID: 16713
		[SerializeField]
		private bool isBoth;

		// Token: 0x0400414A RID: 16714
		[SerializeField]
		private BuilderSmallHandTrigger buttonTrigger;

		// Token: 0x0400414B RID: 16715
		[SerializeField]
		private SoundBankPlayer activateSoundBank;

		// Token: 0x0400414C RID: 16716
		[SerializeField]
		private SoundBankPlayer stopSoundBank;

		// Token: 0x0400414D RID: 16717
		[SerializeField]
		private float debounceTime = 0.5f;

		// Token: 0x0400414E RID: 16718
		private float lastTriggeredTime;

		// Token: 0x0400414F RID: 16719
		private double latestTime = 3.4028234663852886E+38;

		// Token: 0x04004150 RID: 16720
		[SerializeField]
		private TMP_Text displayText;
	}
}
