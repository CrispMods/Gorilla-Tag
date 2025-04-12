using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009C4 RID: 2500
	public class GorillaPlayerTimerCountDisplay : MonoBehaviour, ITickSystemTick
	{
		// Token: 0x06003E36 RID: 15926 RVA: 0x00057B4E File Offset: 0x00055D4E
		private void Start()
		{
			this.TryInit();
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x00057B4E File Offset: 0x00055D4E
		private void OnEnable()
		{
			this.TryInit();
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x00162420 File Offset: 0x00160620
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

		// Token: 0x06003E39 RID: 15929 RVA: 0x001624AC File Offset: 0x001606AC
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

		// Token: 0x06003E3A RID: 15930 RVA: 0x00057B56 File Offset: 0x00055D56
		private void OnLocalTimerStarted()
		{
			if (!this.TickRunning)
			{
				TickSystem<object>.AddTickCallback(this);
			}
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x00162514 File Offset: 0x00160714
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

		// Token: 0x06003E3C RID: 15932 RVA: 0x00162578 File Offset: 0x00160778
		private void UpdateLatestTime()
		{
			float timeForPlayer = PlayerTimerManager.instance.GetTimeForPlayer(NetworkSystem.Instance.LocalPlayer.ActorNumber);
			this.displayText.text = "TIME: " + TimeSpan.FromSeconds((double)timeForPlayer).ToString("mm\\:ss\\:f");
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06003E3D RID: 15933 RVA: 0x00057B66 File Offset: 0x00055D66
		// (set) Token: 0x06003E3E RID: 15934 RVA: 0x00057B6E File Offset: 0x00055D6E
		public bool TickRunning { get; set; }

		// Token: 0x06003E3F RID: 15935 RVA: 0x00057B77 File Offset: 0x00055D77
		public void Tick()
		{
			this.UpdateLatestTime();
		}

		// Token: 0x04003F7F RID: 16255
		[SerializeField]
		private TMP_Text displayText;

		// Token: 0x04003F80 RID: 16256
		private bool isInitialized;
	}
}
