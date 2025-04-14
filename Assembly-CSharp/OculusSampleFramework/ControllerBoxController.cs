using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A64 RID: 2660
	public class ControllerBoxController : MonoBehaviour
	{
		// Token: 0x0600424A RID: 16970 RVA: 0x000023F4 File Offset: 0x000005F4
		private void Awake()
		{
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x00138F91 File Offset: 0x00137191
		public void StartStopStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.StartStopStateChanged();
			}
		}

		// Token: 0x0600424C RID: 16972 RVA: 0x00138FA7 File Offset: 0x001371A7
		public void DecreaseSpeedStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.DecreaseSpeedStateChanged();
			}
		}

		// Token: 0x0600424D RID: 16973 RVA: 0x00138FBD File Offset: 0x001371BD
		public void IncreaseSpeedStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.IncreaseSpeedStateChanged();
			}
		}

		// Token: 0x0600424E RID: 16974 RVA: 0x00138FD3 File Offset: 0x001371D3
		public void SmokeButtonStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.SmokeButtonStateChanged();
			}
		}

		// Token: 0x0600424F RID: 16975 RVA: 0x00138FE9 File Offset: 0x001371E9
		public void WhistleButtonStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.WhistleButtonStateChanged();
			}
		}

		// Token: 0x06004250 RID: 16976 RVA: 0x00138FFF File Offset: 0x001371FF
		public void ReverseButtonStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.ReverseButtonStateChanged();
			}
		}

		// Token: 0x06004251 RID: 16977 RVA: 0x00139015 File Offset: 0x00137215
		public void SwitchVisualization(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				HandsManager.Instance.SwitchVisualization();
			}
		}

		// Token: 0x06004252 RID: 16978 RVA: 0x0013902A File Offset: 0x0013722A
		public void GoMoo(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._cowController.GoMooCowGo();
			}
		}

		// Token: 0x04004345 RID: 17221
		[SerializeField]
		private TrainLocomotive _locomotive;

		// Token: 0x04004346 RID: 17222
		[SerializeField]
		private CowController _cowController;
	}
}
