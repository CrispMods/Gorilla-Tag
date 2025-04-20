using System;
using TMPro;
using UnityEngine;

namespace GorillaTagScripts.UI.ModIO
{
	// Token: 0x02000A00 RID: 2560
	public class VirtualStumpTeleportingHUD : MonoBehaviour
	{
		// Token: 0x06003FF9 RID: 16377 RVA: 0x0016B1BC File Offset: 0x001693BC
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

		// Token: 0x06003FFA RID: 16378 RVA: 0x0016B218 File Offset: 0x00169418
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

		// Token: 0x06003FFB RID: 16379 RVA: 0x00059CC4 File Offset: 0x00057EC4
		private void IncrementProgressDots()
		{
			this.numProgressDots++;
			if (this.numProgressDots > this.maxNumProgressDots)
			{
				this.numProgressDots = 0;
			}
		}

		// Token: 0x040040F5 RID: 16629
		[SerializeField]
		private string enteringVirtualStumpString = "Now Entering the Virtual Stump";

		// Token: 0x040040F6 RID: 16630
		[SerializeField]
		private string leavingVirtualStumpString = "Now Leaving the Virtual Stump";

		// Token: 0x040040F7 RID: 16631
		[SerializeField]
		private TMP_Text teleportingStatusText;

		// Token: 0x040040F8 RID: 16632
		[SerializeField]
		private int maxNumProgressDots = 3;

		// Token: 0x040040F9 RID: 16633
		[SerializeField]
		private float textUpdateInterval = 0.5f;

		// Token: 0x040040FA RID: 16634
		private float lastTextUpdateTime;

		// Token: 0x040040FB RID: 16635
		private int numProgressDots;

		// Token: 0x040040FC RID: 16636
		private bool isEnteringVirtualStump;
	}
}
