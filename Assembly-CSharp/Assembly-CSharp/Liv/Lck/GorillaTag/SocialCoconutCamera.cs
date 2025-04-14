using System;
using UnityEngine;

namespace Liv.Lck.GorillaTag
{
	// Token: 0x02000B24 RID: 2852
	public class SocialCoconutCamera : MonoBehaviour
	{
		// Token: 0x06004713 RID: 18195 RVA: 0x001520E7 File Offset: 0x001502E7
		private void Awake()
		{
			if (this._propertyBlock == null)
			{
				this._propertyBlock = new MaterialPropertyBlock();
			}
			this._propertyBlock.SetInt(this.IS_RECORDING, 0);
			this._bodyRenderer.SetPropertyBlock(this._propertyBlock);
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x0015211F File Offset: 0x0015031F
		public void SetVisualsActive(bool active)
		{
			this._isActive = active;
			this._visuals.SetActive(active);
		}

		// Token: 0x06004715 RID: 18197 RVA: 0x00152134 File Offset: 0x00150334
		public void SetRecordingState(bool isRecording)
		{
			if (!this._isActive)
			{
				return;
			}
			this._propertyBlock.SetInt(this.IS_RECORDING, isRecording ? 1 : 0);
			this._bodyRenderer.SetPropertyBlock(this._propertyBlock);
		}

		// Token: 0x0400489D RID: 18589
		[SerializeField]
		private GameObject _visuals;

		// Token: 0x0400489E RID: 18590
		[SerializeField]
		private MeshRenderer _bodyRenderer;

		// Token: 0x0400489F RID: 18591
		private bool _isActive;

		// Token: 0x040048A0 RID: 18592
		private MaterialPropertyBlock _propertyBlock;

		// Token: 0x040048A1 RID: 18593
		private string IS_RECORDING = "_Is_Recording";
	}
}
