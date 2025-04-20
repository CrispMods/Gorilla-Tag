using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A31 RID: 2609
	public class BuilderPieceTimer : MonoBehaviour, IBuilderPieceComponent, ITickSystemTick
	{
		// Token: 0x0600416F RID: 16751 RVA: 0x0005AC33 File Offset: 0x00058E33
		private void Awake()
		{
			this.buttonTrigger.TriggeredEvent.AddListener(new UnityAction(this.OnButtonPressed));
		}

		// Token: 0x06004170 RID: 16752 RVA: 0x0005AC51 File Offset: 0x00058E51
		private void OnDestroy()
		{
			if (this.buttonTrigger != null)
			{
				this.buttonTrigger.TriggeredEvent.RemoveListener(new UnityAction(this.OnButtonPressed));
			}
		}

		// Token: 0x06004171 RID: 16753 RVA: 0x00171944 File Offset: 0x0016FB44
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

		// Token: 0x06004172 RID: 16754 RVA: 0x001719FC File Offset: 0x0016FBFC
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

		// Token: 0x06004173 RID: 16755 RVA: 0x0005AC7D File Offset: 0x00058E7D
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

		// Token: 0x06004174 RID: 16756 RVA: 0x00171AAC File Offset: 0x0016FCAC
		private void OnZoneChanged()
		{
			bool active = ZoneManagement.instance.IsZoneActive(GTZone.monkeBlocks);
			if (this.displayText != null)
			{
				this.displayText.gameObject.SetActive(active);
			}
		}

		// Token: 0x06004175 RID: 16757 RVA: 0x00171AE8 File Offset: 0x0016FCE8
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

		// Token: 0x06004176 RID: 16758 RVA: 0x0005ACA9 File Offset: 0x00058EA9
		public void OnPieceDestroy()
		{
			if (this.displayText != null)
			{
				ZoneManagement instance = ZoneManagement.instance;
				instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
			}
		}

		// Token: 0x06004177 RID: 16759 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x06004178 RID: 16760 RVA: 0x00171B50 File Offset: 0x0016FD50
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

		// Token: 0x06004179 RID: 16761 RVA: 0x00171BFC File Offset: 0x0016FDFC
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

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x0600417A RID: 16762 RVA: 0x0005ACDF File Offset: 0x00058EDF
		// (set) Token: 0x0600417B RID: 16763 RVA: 0x0005ACE7 File Offset: 0x00058EE7
		public bool TickRunning { get; set; }

		// Token: 0x0600417C RID: 16764 RVA: 0x00171C78 File Offset: 0x0016FE78
		public void Tick()
		{
			if (this.displayText != null)
			{
				float num = PlayerTimerManager.instance.GetTimeForPlayer(NetworkSystem.Instance.LocalPlayer.ActorNumber);
				num = Mathf.Clamp(num, 0f, 3599.99f);
				this.displayText.text = "TIME: " + TimeSpan.FromSeconds((double)num).ToString("mm\\:ss\\:f");
			}
		}

		// Token: 0x04004241 RID: 16961
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x04004242 RID: 16962
		[SerializeField]
		private bool isStart;

		// Token: 0x04004243 RID: 16963
		[SerializeField]
		private bool isBoth;

		// Token: 0x04004244 RID: 16964
		[SerializeField]
		private BuilderSmallHandTrigger buttonTrigger;

		// Token: 0x04004245 RID: 16965
		[SerializeField]
		private SoundBankPlayer activateSoundBank;

		// Token: 0x04004246 RID: 16966
		[SerializeField]
		private SoundBankPlayer stopSoundBank;

		// Token: 0x04004247 RID: 16967
		[SerializeField]
		private float debounceTime = 0.5f;

		// Token: 0x04004248 RID: 16968
		private float lastTriggeredTime;

		// Token: 0x04004249 RID: 16969
		private double latestTime = 3.4028234663852886E+38;

		// Token: 0x0400424A RID: 16970
		[SerializeField]
		private TMP_Text displayText;
	}
}
