using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009C0 RID: 2496
	public class GorillaPlayerTimerButton : MonoBehaviour
	{
		// Token: 0x06003E20 RID: 15904 RVA: 0x00126B42 File Offset: 0x00124D42
		private void Awake()
		{
			this.materialProps = new MaterialPropertyBlock();
		}

		// Token: 0x06003E21 RID: 15905 RVA: 0x00126B4F File Offset: 0x00124D4F
		private void Start()
		{
			this.TryInit();
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x00126B4F File Offset: 0x00124D4F
		private void OnEnable()
		{
			this.TryInit();
		}

		// Token: 0x06003E23 RID: 15907 RVA: 0x00126B58 File Offset: 0x00124D58
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

		// Token: 0x06003E24 RID: 15908 RVA: 0x00126BD4 File Offset: 0x00124DD4
		private void OnDisable()
		{
			if (PlayerTimerManager.instance != null)
			{
				PlayerTimerManager.instance.OnTimerStopped.RemoveListener(new UnityAction<int, int>(this.OnTimerStopped));
				PlayerTimerManager.instance.OnLocalTimerStarted.RemoveListener(new UnityAction(this.OnLocalTimerStarted));
			}
			this.isInitialized = false;
		}

		// Token: 0x06003E25 RID: 15909 RVA: 0x00126C2B File Offset: 0x00124E2B
		private void OnLocalTimerStarted()
		{
			if (this.isBothStartAndStop)
			{
				this.isStartButton = false;
			}
		}

		// Token: 0x06003E26 RID: 15910 RVA: 0x00126C3C File Offset: 0x00124E3C
		private void OnTimerStopped(int actorNum, int timeDelta)
		{
			if (this.isBothStartAndStop && actorNum == NetworkSystem.Instance.LocalPlayer.ActorNumber)
			{
				this.isStartButton = true;
			}
		}

		// Token: 0x06003E27 RID: 15911 RVA: 0x00126C60 File Offset: 0x00124E60
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

		// Token: 0x06003E28 RID: 15912 RVA: 0x00126D20 File Offset: 0x00124F20
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

		// Token: 0x04003F64 RID: 16228
		private float lastTriggeredTime;

		// Token: 0x04003F65 RID: 16229
		[SerializeField]
		private bool isStartButton;

		// Token: 0x04003F66 RID: 16230
		[SerializeField]
		private bool isBothStartAndStop;

		// Token: 0x04003F67 RID: 16231
		[SerializeField]
		private float debounceTime = 0.5f;

		// Token: 0x04003F68 RID: 16232
		[SerializeField]
		private MeshRenderer mesh;

		// Token: 0x04003F69 RID: 16233
		[SerializeField]
		private Color pressColor;

		// Token: 0x04003F6A RID: 16234
		[SerializeField]
		private Color notPressedColor;

		// Token: 0x04003F6B RID: 16235
		private MaterialPropertyBlock materialProps;

		// Token: 0x04003F6C RID: 16236
		private bool isInitialized;
	}
}
