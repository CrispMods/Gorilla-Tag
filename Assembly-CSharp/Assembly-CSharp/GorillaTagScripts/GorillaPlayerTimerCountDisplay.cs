using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009C4 RID: 2500
	public class GorillaPlayerTimerCountDisplay : MonoBehaviour, ITickSystemTick
	{
		// Token: 0x06003E36 RID: 15926 RVA: 0x00127358 File Offset: 0x00125558
		private void Start()
		{
			this.TryInit();
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x00127358 File Offset: 0x00125558
		private void OnEnable()
		{
			this.TryInit();
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x00127360 File Offset: 0x00125560
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

		// Token: 0x06003E39 RID: 15929 RVA: 0x001273EC File Offset: 0x001255EC
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

		// Token: 0x06003E3A RID: 15930 RVA: 0x00127451 File Offset: 0x00125651
		private void OnLocalTimerStarted()
		{
			if (!this.TickRunning)
			{
				TickSystem<object>.AddTickCallback(this);
			}
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x00127464 File Offset: 0x00125664
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

		// Token: 0x06003E3C RID: 15932 RVA: 0x001274C8 File Offset: 0x001256C8
		private void UpdateLatestTime()
		{
			float timeForPlayer = PlayerTimerManager.instance.GetTimeForPlayer(NetworkSystem.Instance.LocalPlayer.ActorNumber);
			this.displayText.text = "TIME: " + TimeSpan.FromSeconds((double)timeForPlayer).ToString("mm\\:ss\\:f");
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06003E3D RID: 15933 RVA: 0x00127518 File Offset: 0x00125718
		// (set) Token: 0x06003E3E RID: 15934 RVA: 0x00127520 File Offset: 0x00125720
		public bool TickRunning { get; set; }

		// Token: 0x06003E3F RID: 15935 RVA: 0x00127529 File Offset: 0x00125729
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
