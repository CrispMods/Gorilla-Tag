using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A67 RID: 2663
	public class ControllerBoxController : MonoBehaviour
	{
		// Token: 0x06004256 RID: 16982 RVA: 0x000023F4 File Offset: 0x000005F4
		private void Awake()
		{
		}

		// Token: 0x06004257 RID: 16983 RVA: 0x00139559 File Offset: 0x00137759
		public void StartStopStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.StartStopStateChanged();
			}
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x0013956F File Offset: 0x0013776F
		public void DecreaseSpeedStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.DecreaseSpeedStateChanged();
			}
		}

		// Token: 0x06004259 RID: 16985 RVA: 0x00139585 File Offset: 0x00137785
		public void IncreaseSpeedStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.IncreaseSpeedStateChanged();
			}
		}

		// Token: 0x0600425A RID: 16986 RVA: 0x0013959B File Offset: 0x0013779B
		public void SmokeButtonStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.SmokeButtonStateChanged();
			}
		}

		// Token: 0x0600425B RID: 16987 RVA: 0x001395B1 File Offset: 0x001377B1
		public void WhistleButtonStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.WhistleButtonStateChanged();
			}
		}

		// Token: 0x0600425C RID: 16988 RVA: 0x001395C7 File Offset: 0x001377C7
		public void ReverseButtonStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.ReverseButtonStateChanged();
			}
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x001395DD File Offset: 0x001377DD
		public void SwitchVisualization(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				HandsManager.Instance.SwitchVisualization();
			}
		}

		// Token: 0x0600425E RID: 16990 RVA: 0x001395F2 File Offset: 0x001377F2
		public void GoMoo(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._cowController.GoMooCowGo();
			}
		}

		// Token: 0x04004357 RID: 17239
		[SerializeField]
		private TrainLocomotive _locomotive;

		// Token: 0x04004358 RID: 17240
		[SerializeField]
		private CowController _cowController;
	}
}
