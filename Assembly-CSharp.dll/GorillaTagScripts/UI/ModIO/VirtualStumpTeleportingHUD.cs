using System;
using TMPro;
using UnityEngine;

namespace GorillaTagScripts.UI.ModIO
{
	// Token: 0x020009DA RID: 2522
	public class VirtualStumpTeleportingHUD : MonoBehaviour
	{
		// Token: 0x06003ED9 RID: 16089 RVA: 0x00164D60 File Offset: 0x00162F60
		public void Initialize(bool isEntering)
		{
			this.isEnteringVirtualStump = isEntering;
			if (isEntering)
			{
				this.teleportingStatusText.text = this.enteringVirtualStumpString;
				this.teleportingStatusText.gameObject.SetActive(true);
				return;
			}
			this.teleportingStatusText.text = this.leavingVirtualStumpString;
			this.teleportingStatusText.gameObject.SetActive(true);
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x00164DBC File Offset: 0x00162FBC
		private void Update()
		{
			if (Time.time - this.lastTextUpdateTime > this.textUpdateInterval)
			{
				this.lastTextUpdateTime = Time.time;
				this.IncrementProgressDots();
				this.teleportingStatusText.text = (this.isEnteringVirtualStump ? this.enteringVirtualStumpString : this.leavingVirtualStumpString);
				for (int i = 0; i < this.numProgressDots; i++)
				{
					TMP_Text tmp_Text = this.teleportingStatusText;
					tmp_Text.text += ".";
				}
			}
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x00058341 File Offset: 0x00056541
		private void IncrementProgressDots()
		{
			this.numProgressDots++;
			if (this.numProgressDots > this.maxNumProgressDots)
			{
				this.numProgressDots = 0;
			}
		}

		// Token: 0x04004023 RID: 16419
		[SerializeField]
		private string enteringVirtualStumpString = "Now Entering the Virtual Stump";

		// Token: 0x04004024 RID: 16420
		[SerializeField]
		private string leavingVirtualStumpString = "Now Leaving the Virtual Stump";

		// Token: 0x04004025 RID: 16421
		[SerializeField]
		private TMP_Text teleportingStatusText;

		// Token: 0x04004026 RID: 16422
		[SerializeField]
		private int maxNumProgressDots = 3;

		// Token: 0x04004027 RID: 16423
		[SerializeField]
		private float textUpdateInterval = 0.5f;

		// Token: 0x04004028 RID: 16424
		private float lastTextUpdateTime;

		// Token: 0x04004029 RID: 16425
		private int numProgressDots;

		// Token: 0x0400402A RID: 16426
		private bool isEnteringVirtualStump;
	}
}
