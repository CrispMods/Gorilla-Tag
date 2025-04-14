using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009C3 RID: 2499
	public class GorillaPlayerTimerButton : MonoBehaviour
	{
		// Token: 0x06003E2C RID: 15916 RVA: 0x0012710A File Offset: 0x0012530A
		private void Awake()
		{
			this.materialProps = new MaterialPropertyBlock();
		}

		// Token: 0x06003E2D RID: 15917 RVA: 0x00127117 File Offset: 0x00125317
		private void Start()
		{
			this.TryInit();
		}

		// Token: 0x06003E2E RID: 15918 RVA: 0x00127117 File Offset: 0x00125317
		private void OnEnable()
		{
			this.TryInit();
		}

		// Token: 0x06003E2F RID: 15919 RVA: 0x00127120 File Offset: 0x00125320
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

		// Token: 0x06003E30 RID: 15920 RVA: 0x0012719C File Offset: 0x0012539C
		private void OnDisable()
		{
			if (PlayerTimerManager.instance != null)
			{
				PlayerTimerManager.instance.OnTimerStopped.RemoveListener(new UnityAction<int, int>(this.OnTimerStopped));
				PlayerTimerManager.instance.OnLocalTimerStarted.RemoveListener(new UnityAction(this.OnLocalTimerStarted));
			}
			this.isInitialized = false;
		}

		// Token: 0x06003E31 RID: 15921 RVA: 0x001271F3 File Offset: 0x001253F3
		private void OnLocalTimerStarted()
		{
			if (this.isBothStartAndStop)
			{
				this.isStartButton = false;
			}
		}

		// Token: 0x06003E32 RID: 15922 RVA: 0x00127204 File Offset: 0x00125404
		private void OnTimerStopped(int actorNum, int timeDelta)
		{
			if (this.isBothStartAndStop && actorNum == NetworkSystem.Instance.LocalPlayer.ActorNumber)
			{
				this.isStartButton = true;
			}
		}

		// Token: 0x06003E33 RID: 15923 RVA: 0x00127228 File Offset: 0x00125428
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

		// Token: 0x06003E34 RID: 15924 RVA: 0x001272E8 File Offset: 0x001254E8
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

		// Token: 0x04003F76 RID: 16246
		private float lastTriggeredTime;

		// Token: 0x04003F77 RID: 16247
		[SerializeField]
		private bool isStartButton;

		// Token: 0x04003F78 RID: 16248
		[SerializeField]
		private bool isBothStartAndStop;

		// Token: 0x04003F79 RID: 16249
		[SerializeField]
		private float debounceTime = 0.5f;

		// Token: 0x04003F7A RID: 16250
		[SerializeField]
		private MeshRenderer mesh;

		// Token: 0x04003F7B RID: 16251
		[SerializeField]
		private Color pressColor;

		// Token: 0x04003F7C RID: 16252
		[SerializeField]
		private Color notPressedColor;

		// Token: 0x04003F7D RID: 16253
		private MaterialPropertyBlock materialProps;

		// Token: 0x04003F7E RID: 16254
		private bool isInitialized;
	}
}
