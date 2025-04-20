using System;
using UnityEngine;

namespace Liv.Lck.GorillaTag
{
	// Token: 0x02000B4E RID: 2894
	public class SocialCoconutCamera : MonoBehaviour
	{
		// Token: 0x06004850 RID: 18512 RVA: 0x0005F10F File Offset: 0x0005D30F
		private void Awake()
		{
			if (this._propertyBlock == null)
			{
				this._propertyBlock = new MaterialPropertyBlock();
			}
			this._propertyBlock.SetInt(this.IS_RECORDING, 0);
			this._bodyRenderer.SetPropertyBlock(this._propertyBlock);
		}

		// Token: 0x06004851 RID: 18513 RVA: 0x0005F147 File Offset: 0x0005D347
		public void SetVisualsActive(bool active)
		{
			this._isActive = active;
			this._visuals.SetActive(active);
		}

		// Token: 0x06004852 RID: 18514 RVA: 0x0005F15C File Offset: 0x0005D35C
		public void SetRecordingState(bool isRecording)
		{
			if (!this._isActive)
			{
				return;
			}
			this._propertyBlock.SetInt(this.IS_RECORDING, isRecording ? 1 : 0);
			this._bodyRenderer.SetPropertyBlock(this._propertyBlock);
		}

		// Token: 0x04004980 RID: 18816
		[SerializeField]
		private GameObject _visuals;

		// Token: 0x04004981 RID: 18817
		[SerializeField]
		private MeshRenderer _bodyRenderer;

		// Token: 0x04004982 RID: 18818
		private bool _isActive;

		// Token: 0x04004983 RID: 18819
		private MaterialPropertyBlock _propertyBlock;

		// Token: 0x04004984 RID: 18820
		private string IS_RECORDING = "_Is_Recording";
	}
}
