using System;
using System.Collections;
using GorillaExtensions;
using GorillaGameModes;
using GorillaTagScripts.ModIO;
using GT_CustomMapSupportRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts.VirtualStumpCustomMaps
{
	// Token: 0x020009FB RID: 2555
	public class VirtualStumpReturnWatch : MonoBehaviour
	{
		// Token: 0x06003FD8 RID: 16344 RVA: 0x0016A900 File Offset: 0x00168B00
		private void Start()
		{
			if (this.returnButton != null)
			{
				this.returnButton.onStartPressingButton.AddListener(new UnityAction(this.OnStartedPressingButton));
				this.returnButton.onStopPressingButton.AddListener(new UnityAction(this.OnStoppedPressingButton));
				this.returnButton.onPressButton.AddListener(new UnityAction(this.OnButtonPressed));
			}
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x0016A970 File Offset: 0x00168B70
		private void OnDestroy()
		{
			if (this.returnButton != null)
			{
				this.returnButton.onStartPressingButton.RemoveListener(new UnityAction(this.OnStartedPressingButton));
				this.returnButton.onStopPressingButton.RemoveListener(new UnityAction(this.OnStoppedPressingButton));
				this.returnButton.onPressButton.RemoveListener(new UnityAction(this.OnButtonPressed));
			}
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x0016A9E0 File Offset: 0x00168BE0
		public static void SetWatchProperties(VirtualStumpReturnWatchProps props)
		{
			VirtualStumpReturnWatch.currentCustomMapProps = props;
			VirtualStumpReturnWatch.currentCustomMapProps.holdDuration = Mathf.Clamp(VirtualStumpReturnWatch.currentCustomMapProps.holdDuration, 0.5f, 5f);
			VirtualStumpReturnWatch.currentCustomMapProps.holdDuration_Infection = Mathf.Clamp(VirtualStumpReturnWatch.currentCustomMapProps.holdDuration_Infection, 0.5f, 5f);
			VirtualStumpReturnWatch.currentCustomMapProps.holdDuration_Custom = Mathf.Clamp(VirtualStumpReturnWatch.currentCustomMapProps.holdDuration_Custom, 0.5f, 5f);
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x0016AA5C File Offset: 0x00168C5C
		private float GetCurrentHoldDuration()
		{
			switch (GorillaGameManager.instance.GameType())
			{
			case GameModeType.Infection:
				if (VirtualStumpReturnWatch.currentCustomMapProps.infectionOverride)
				{
					return VirtualStumpReturnWatch.currentCustomMapProps.holdDuration_Infection;
				}
				return VirtualStumpReturnWatch.currentCustomMapProps.holdDuration;
			case GameModeType.Custom:
				if (VirtualStumpReturnWatch.currentCustomMapProps.customModeOverride)
				{
					return VirtualStumpReturnWatch.currentCustomMapProps.holdDuration_Custom;
				}
				return VirtualStumpReturnWatch.currentCustomMapProps.holdDuration;
			}
			return VirtualStumpReturnWatch.currentCustomMapProps.holdDuration;
		}

		// Token: 0x06003FDC RID: 16348 RVA: 0x00059B63 File Offset: 0x00057D63
		private void OnStartedPressingButton()
		{
			this.startPressingButtonTime = Time.time;
			this.currentlyBeingPressed = true;
			this.returnButton.pressDuration = this.GetCurrentHoldDuration();
			this.ShowCountdownText();
			this.updateCountdownCoroutine = base.StartCoroutine(this.UpdateCountdownText());
		}

		// Token: 0x06003FDD RID: 16349 RVA: 0x00059BA0 File Offset: 0x00057DA0
		private void OnStoppedPressingButton()
		{
			this.currentlyBeingPressed = false;
			this.HideCountdownText();
			if (this.updateCountdownCoroutine != null)
			{
				base.StopCoroutine(this.updateCountdownCoroutine);
				this.updateCountdownCoroutine = null;
			}
		}

		// Token: 0x06003FDE RID: 16350 RVA: 0x0016AAF0 File Offset: 0x00168CF0
		private void OnButtonPressed()
		{
			this.currentlyBeingPressed = false;
			if (ZoneManagement.IsInZone(GTZone.customMaps) && !CustomMapManager.IsLocalPlayerInVirtualStump())
			{
				bool flag = VirtualStumpReturnWatch.currentCustomMapProps.shouldTagPlayer;
				bool flag2 = VirtualStumpReturnWatch.currentCustomMapProps.shouldKickPlayer;
				switch (GorillaGameManager.instance.GameType())
				{
				case GameModeType.Infection:
					if (VirtualStumpReturnWatch.currentCustomMapProps.infectionOverride)
					{
						flag = VirtualStumpReturnWatch.currentCustomMapProps.shouldTagPlayer_Infection;
						flag2 = VirtualStumpReturnWatch.currentCustomMapProps.shouldKickPlayer_Infection;
					}
					break;
				case GameModeType.Custom:
					if (VirtualStumpReturnWatch.currentCustomMapProps.customModeOverride)
					{
						flag = VirtualStumpReturnWatch.currentCustomMapProps.shouldTagPlayer_CustomMode;
						flag2 = VirtualStumpReturnWatch.currentCustomMapProps.shouldKickPlayer_CustomMode;
					}
					break;
				}
				if (flag2 && NetworkSystem.Instance.InRoom && !NetworkSystem.Instance.SessionIsPrivate)
				{
					NetworkSystem.Instance.ReturnToSinglePlayer();
				}
				else if (flag)
				{
					GameMode.ReportHit();
				}
				CustomMapManager.ReturnToVirtualStump();
			}
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x0016ABE4 File Offset: 0x00168DE4
		private void ShowCountdownText()
		{
			if (this.countdownText.IsNull())
			{
				return;
			}
			int num = 1 + Mathf.FloorToInt(this.GetCurrentHoldDuration());
			this.countdownText.text = num.ToString();
			this.countdownText.gameObject.SetActive(true);
			if (this.buttonText.IsNotNull())
			{
				this.buttonText.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003FE0 RID: 16352 RVA: 0x0016AC50 File Offset: 0x00168E50
		private void HideCountdownText()
		{
			if (this.countdownText.IsNull())
			{
				return;
			}
			this.countdownText.text = "";
			this.countdownText.gameObject.SetActive(false);
			if (this.buttonText.IsNotNull())
			{
				this.buttonText.gameObject.SetActive(true);
			}
		}

		// Token: 0x06003FE1 RID: 16353 RVA: 0x00059BCA File Offset: 0x00057DCA
		private IEnumerator UpdateCountdownText()
		{
			while (this.currentlyBeingPressed)
			{
				if (this.countdownText.IsNull())
				{
					yield break;
				}
				float f = this.GetCurrentHoldDuration() - (Time.time - this.startPressingButtonTime);
				int num = 1 + Mathf.FloorToInt(f);
				this.countdownText.text = num.ToString();
				yield return null;
			}
			yield break;
		}

		// Token: 0x040040DB RID: 16603
		[SerializeField]
		private HeldButton returnButton;

		// Token: 0x040040DC RID: 16604
		[SerializeField]
		private TMP_Text buttonText;

		// Token: 0x040040DD RID: 16605
		[SerializeField]
		private TMP_Text countdownText;

		// Token: 0x040040DE RID: 16606
		private static VirtualStumpReturnWatchProps currentCustomMapProps;

		// Token: 0x040040DF RID: 16607
		private float startPressingButtonTime = -1f;

		// Token: 0x040040E0 RID: 16608
		private bool currentlyBeingPressed;

		// Token: 0x040040E1 RID: 16609
		private Coroutine updateCountdownCoroutine;
	}
}
