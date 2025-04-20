using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009E7 RID: 2535
	public class GorillaPlayerTimerCountDisplay : MonoBehaviour, ITickSystemTick
	{
		// Token: 0x06003F42 RID: 16194 RVA: 0x000593E5 File Offset: 0x000575E5
		private void Start()
		{
			this.TryInit();
		}

		// Token: 0x06003F43 RID: 16195 RVA: 0x000593E5 File Offset: 0x000575E5
		private void OnEnable()
		{
			this.TryInit();
		}

		// Token: 0x06003F44 RID: 16196 RVA: 0x00168444 File Offset: 0x00166644
		private void TryInit()
		{
			if (this.isInitialized)
			{
				return;
			}
			if (PlayerTimerManager.instance == null)
			{
				return;
			}
			PlayerTimerManager.instance.OnTimerStopped.AddListener(new UnityAction<int, int>(this.OnTimerStopped));
			PlayerTimerManager.instance.OnLocalTimerStarted.AddListener(new UnityAction(this.OnLocalTimerStarted));
			this.displayText.text = "TIME: --.--.-";
			if (PlayerTimerManager.instance.IsLocalTimerStarted() && !this.TickRunning)
			{
				TickSystem<object>.AddTickCallback(this);
			}
			this.isInitialized = true;
		}

		// Token: 0x06003F45 RID: 16197 RVA: 0x001684D0 File Offset: 0x001666D0
		private void OnDisable()
		{
			if (PlayerTimerManager.instance != null)
			{
				PlayerTimerManager.instance.OnTimerStopped.RemoveListener(new UnityAction<int, int>(this.OnTimerStopped));
				PlayerTimerManager.instance.OnLocalTimerStarted.RemoveListener(new UnityAction(this.OnLocalTimerStarted));
			}
			this.isInitialized = false;
			if (this.TickRunning)
			{
				TickSystem<object>.RemoveTickCallback(this);
			}
		}

		// Token: 0x06003F46 RID: 16198 RVA: 0x000593ED File Offset: 0x000575ED
		private void OnLocalTimerStarted()
		{
			if (!this.TickRunning)
			{
				TickSystem<object>.AddTickCallback(this);
			}
		}

		// Token: 0x06003F47 RID: 16199 RVA: 0x00168538 File Offset: 0x00166738
		private void OnTimerStopped(int actorNum, int timeDelta)
		{
			if (actorNum == NetworkSystem.Instance.LocalPlayer.ActorNumber)
			{
				double value = timeDelta / 1000.0;
				this.displayText.text = "TIME: " + TimeSpan.FromSeconds(value).ToString("mm\\:ss\\:f");
				if (this.TickRunning)
				{
					TickSystem<object>.RemoveTickCallback(this);
				}
			}
		}

		// Token: 0x06003F48 RID: 16200 RVA: 0x0016859C File Offset: 0x0016679C
		private void UpdateLatestTime()
		{
			float timeForPlayer = PlayerTimerManager.instance.GetTimeForPlayer(NetworkSystem.Instance.LocalPlayer.ActorNumber);
			this.displayText.text = "TIME: " + TimeSpan.FromSeconds((double)timeForPlayer).ToString("mm\\:ss\\:f");
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06003F49 RID: 16201 RVA: 0x000593FD File Offset: 0x000575FD
		// (set) Token: 0x06003F4A RID: 16202 RVA: 0x00059405 File Offset: 0x00057605
		public bool TickRunning { get; set; }

		// Token: 0x06003F4B RID: 16203 RVA: 0x0005940E File Offset: 0x0005760E
		public void Tick()
		{
			this.UpdateLatestTime();
		}

		// Token: 0x04004047 RID: 16455
		[SerializeField]
		private TMP_Text displayText;

		// Token: 0x04004048 RID: 16456
		private bool isInitialized;
	}
}
