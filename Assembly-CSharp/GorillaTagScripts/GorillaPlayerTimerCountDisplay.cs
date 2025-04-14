using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009C1 RID: 2497
	public class GorillaPlayerTimerCountDisplay : MonoBehaviour, ITickSystemTick
	{
		// Token: 0x06003E2A RID: 15914 RVA: 0x00126D90 File Offset: 0x00124F90
		private void Start()
		{
			this.TryInit();
		}

		// Token: 0x06003E2B RID: 15915 RVA: 0x00126D90 File Offset: 0x00124F90
		private void OnEnable()
		{
			this.TryInit();
		}

		// Token: 0x06003E2C RID: 15916 RVA: 0x00126D98 File Offset: 0x00124F98
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

		// Token: 0x06003E2D RID: 15917 RVA: 0x00126E24 File Offset: 0x00125024
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

		// Token: 0x06003E2E RID: 15918 RVA: 0x00126E89 File Offset: 0x00125089
		private void OnLocalTimerStarted()
		{
			if (!this.TickRunning)
			{
				TickSystem<object>.AddTickCallback(this);
			}
		}

		// Token: 0x06003E2F RID: 15919 RVA: 0x00126E9C File Offset: 0x0012509C
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

		// Token: 0x06003E30 RID: 15920 RVA: 0x00126F00 File Offset: 0x00125100
		private void UpdateLatestTime()
		{
			float timeForPlayer = PlayerTimerManager.instance.GetTimeForPlayer(NetworkSystem.Instance.LocalPlayer.ActorNumber);
			this.displayText.text = "TIME: " + TimeSpan.FromSeconds((double)timeForPlayer).ToString("mm\\:ss\\:f");
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06003E31 RID: 15921 RVA: 0x00126F50 File Offset: 0x00125150
		// (set) Token: 0x06003E32 RID: 15922 RVA: 0x00126F58 File Offset: 0x00125158
		public bool TickRunning { get; set; }

		// Token: 0x06003E33 RID: 15923 RVA: 0x00126F61 File Offset: 0x00125161
		public void Tick()
		{
			this.UpdateLatestTime();
		}

		// Token: 0x04003F6D RID: 16237
		[SerializeField]
		private TMP_Text displayText;

		// Token: 0x04003F6E RID: 16238
		private bool isInitialized;
	}
}
