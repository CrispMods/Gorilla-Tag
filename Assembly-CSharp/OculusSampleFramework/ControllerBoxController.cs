using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A91 RID: 2705
	public class ControllerBoxController : MonoBehaviour
	{
		// Token: 0x0600438F RID: 17295 RVA: 0x00030607 File Offset: 0x0002E807
		private void Awake()
		{
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x0005C09A File Offset: 0x0005A29A
		public void StartStopStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.StartStopStateChanged();
			}
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x0005C0B0 File Offset: 0x0005A2B0
		public void DecreaseSpeedStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.DecreaseSpeedStateChanged();
			}
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x0005C0C6 File Offset: 0x0005A2C6
		public void IncreaseSpeedStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.IncreaseSpeedStateChanged();
			}
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x0005C0DC File Offset: 0x0005A2DC
		public void SmokeButtonStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.SmokeButtonStateChanged();
			}
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x0005C0F2 File Offset: 0x0005A2F2
		public void WhistleButtonStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.WhistleButtonStateChanged();
			}
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x0005C108 File Offset: 0x0005A308
		public void ReverseButtonStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.ReverseButtonStateChanged();
			}
		}

		// Token: 0x06004396 RID: 17302 RVA: 0x0005C11E File Offset: 0x0005A31E
		public void SwitchVisualization(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				HandsManager.Instance.SwitchVisualization();
			}
		}

		// Token: 0x06004397 RID: 17303 RVA: 0x0005C133 File Offset: 0x0005A333
		public void GoMoo(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._cowController.GoMooCowGo();
			}
		}

		// Token: 0x0400443F RID: 17471
		[SerializeField]
		private TrainLocomotive _locomotive;

		// Token: 0x04004440 RID: 17472
		[SerializeField]
		private CowController _cowController;
	}
}
