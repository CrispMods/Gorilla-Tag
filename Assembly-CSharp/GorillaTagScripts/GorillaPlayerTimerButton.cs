using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009E6 RID: 2534
	public class GorillaPlayerTimerButton : MonoBehaviour
	{
		// Token: 0x06003F38 RID: 16184 RVA: 0x00059389 File Offset: 0x00057589
		private void Awake()
		{
			this.materialProps = new MaterialPropertyBlock();
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x00059396 File Offset: 0x00057596
		private void Start()
		{
			this.TryInit();
		}

		// Token: 0x06003F3A RID: 16186 RVA: 0x00059396 File Offset: 0x00057596
		private void OnEnable()
		{
			this.TryInit();
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x00168250 File Offset: 0x00166450
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
			if (this.isBothStartAndStop)
			{
				this.isStartButton = !PlayerTimerManager.instance.IsLocalTimerStarted();
			}
			this.isInitialized = true;
		}

		// Token: 0x06003F3C RID: 16188 RVA: 0x001682CC File Offset: 0x001664CC
		private void OnDisable()
		{
			if (PlayerTimerManager.instance != null)
			{
				PlayerTimerManager.instance.OnTimerStopped.RemoveListener(new UnityAction<int, int>(this.OnTimerStopped));
				PlayerTimerManager.instance.OnLocalTimerStarted.RemoveListener(new UnityAction(this.OnLocalTimerStarted));
			}
			this.isInitialized = false;
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x0005939E File Offset: 0x0005759E
		private void OnLocalTimerStarted()
		{
			if (this.isBothStartAndStop)
			{
				this.isStartButton = false;
			}
		}

		// Token: 0x06003F3E RID: 16190 RVA: 0x000593AF File Offset: 0x000575AF
		private void OnTimerStopped(int actorNum, int timeDelta)
		{
			if (this.isBothStartAndStop && actorNum == NetworkSystem.Instance.LocalPlayer.ActorNumber)
			{
				this.isStartButton = true;
			}
		}

		// Token: 0x06003F3F RID: 16191 RVA: 0x00168324 File Offset: 0x00166524
		private void OnTriggerEnter(Collider other)
		{
			if (!base.enabled)
			{
				return;
			}
			GorillaTriggerColliderHandIndicator componentInParent = other.GetComponentInParent<GorillaTriggerColliderHandIndicator>();
			if (componentInParent == null)
			{
				return;
			}
			if (Time.time < this.lastTriggeredTime + this.debounceTime)
			{
				return;
			}
			if (!NetworkSystem.Instance.InRoom)
			{
				return;
			}
			GorillaTagger.Instance.StartVibration(componentInParent.isLeftHand, GorillaTagger.Instance.tapHapticStrength, GorillaTagger.Instance.tapHapticDuration);
			this.mesh.GetPropertyBlock(this.materialProps);
			this.materialProps.SetColor("_BaseColor", this.pressColor);
			this.mesh.SetPropertyBlock(this.materialProps);
			PlayerTimerManager.instance.RequestTimerToggle(this.isStartButton);
			this.lastTriggeredTime = Time.time;
		}

		// Token: 0x06003F40 RID: 16192 RVA: 0x001683E4 File Offset: 0x001665E4
		private void OnTriggerExit(Collider other)
		{
			if (!base.enabled)
			{
				return;
			}
			if (other.GetComponentInParent<GorillaTriggerColliderHandIndicator>() == null)
			{
				return;
			}
			this.mesh.GetPropertyBlock(this.materialProps);
			this.materialProps.SetColor("_BaseColor", this.notPressedColor);
			this.mesh.SetPropertyBlock(this.materialProps);
		}

		// Token: 0x0400403E RID: 16446
		private float lastTriggeredTime;

		// Token: 0x0400403F RID: 16447
		[SerializeField]
		private bool isStartButton;

		// Token: 0x04004040 RID: 16448
		[SerializeField]
		private bool isBothStartAndStop;

		// Token: 0x04004041 RID: 16449
		[SerializeField]
		private float debounceTime = 0.5f;

		// Token: 0x04004042 RID: 16450
		[SerializeField]
		private MeshRenderer mesh;

		// Token: 0x04004043 RID: 16451
		[SerializeField]
		private Color pressColor;

		// Token: 0x04004044 RID: 16452
		[SerializeField]
		private Color notPressedColor;

		// Token: 0x04004045 RID: 16453
		private MaterialPropertyBlock materialProps;

		// Token: 0x04004046 RID: 16454
		private bool isInitialized;
	}
}
