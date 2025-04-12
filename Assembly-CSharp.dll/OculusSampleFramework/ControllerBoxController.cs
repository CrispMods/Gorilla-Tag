using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A67 RID: 2663
	public class ControllerBoxController : MonoBehaviour
	{
		// Token: 0x06004256 RID: 16982 RVA: 0x0002F75F File Offset: 0x0002D95F
		private void Awake()
		{
		}

		// Token: 0x06004257 RID: 16983 RVA: 0x0005A698 File Offset: 0x00058898
		public void StartStopStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.StartStopStateChanged();
			}
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x0005A6AE File Offset: 0x000588AE
		public void DecreaseSpeedStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.DecreaseSpeedStateChanged();
			}
		}

		// Token: 0x06004259 RID: 16985 RVA: 0x0005A6C4 File Offset: 0x000588C4
		public void IncreaseSpeedStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.IncreaseSpeedStateChanged();
			}
		}

		// Token: 0x0600425A RID: 16986 RVA: 0x0005A6DA File Offset: 0x000588DA
		public void SmokeButtonStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.SmokeButtonStateChanged();
			}
		}

		// Token: 0x0600425B RID: 16987 RVA: 0x0005A6F0 File Offset: 0x000588F0
		public void WhistleButtonStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.WhistleButtonStateChanged();
			}
		}

		// Token: 0x0600425C RID: 16988 RVA: 0x0005A706 File Offset: 0x00058906
		public void ReverseButtonStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				this._locomotive.ReverseButtonStateChanged();
			}
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x0005A71C File Offset: 0x0005891C
		public void SwitchVisualization(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				HandsManager.Instance.SwitchVisualization();
			}
		}

		// Token: 0x0600425E RID: 16990 RVA: 0x0005A731 File Offset: 0x00058931
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
