using System;
using UnityEngine;

namespace Liv.Lck.GorillaTag
{
	// Token: 0x02000B21 RID: 2849
	public class SocialCoconutCamera : MonoBehaviour
	{
		// Token: 0x06004707 RID: 18183 RVA: 0x00151B1F File Offset: 0x0014FD1F
		private void Awake()
		{
			if (this._propertyBlock == null)
			{
				this._propertyBlock = new MaterialPropertyBlock();
			}
			this._propertyBlock.SetInt(this.IS_RECORDING, 0);
			this._bodyRenderer.SetPropertyBlock(this._propertyBlock);
		}

		// Token: 0x06004708 RID: 18184 RVA: 0x00151B57 File Offset: 0x0014FD57
		public void SetVisualsActive(bool active)
		{
			this._isActive = active;
			this._visuals.SetActive(active);
		}

		// Token: 0x06004709 RID: 18185 RVA: 0x00151B6C File Offset: 0x0014FD6C
		public void SetRecordingState(bool isRecording)
		{
			if (!this._isActive)
			{
				return;
			}
			this._propertyBlock.SetInt(this.IS_RECORDING, isRecording ? 1 : 0);
			this._bodyRenderer.SetPropertyBlock(this._propertyBlock);
		}

		// Token: 0x0400488B RID: 18571
		[SerializeField]
		private GameObject _visuals;

		// Token: 0x0400488C RID: 18572
		[SerializeField]
		private MeshRenderer _bodyRenderer;

		// Token: 0x0400488D RID: 18573
		private bool _isActive;

		// Token: 0x0400488E RID: 18574
		private MaterialPropertyBlock _propertyBlock;

		// Token: 0x0400488F RID: 18575
		private string IS_RECORDING = "_Is_Recording";
	}
}
