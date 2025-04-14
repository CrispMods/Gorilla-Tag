using System;
using TMPro;
using UnityEngine;

namespace GorillaTagScripts.UI.ModIO
{
	// Token: 0x020009D7 RID: 2519
	public class VirtualStumpTeleportingHUD : MonoBehaviour
	{
		// Token: 0x06003ECD RID: 16077 RVA: 0x00129EC8 File Offset: 0x001280C8
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

		// Token: 0x06003ECE RID: 16078 RVA: 0x00129F24 File Offset: 0x00128124
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

		// Token: 0x06003ECF RID: 16079 RVA: 0x00129FA3 File Offset: 0x001281A3
		private void IncrementProgressDots()
		{
			this.numProgressDots++;
			if (this.numProgressDots > this.maxNumProgressDots)
			{
				this.numProgressDots = 0;
			}
		}

		// Token: 0x04004011 RID: 16401
		[SerializeField]
		private string enteringVirtualStumpString = "Now Entering the Virtual Stump";

		// Token: 0x04004012 RID: 16402
		[SerializeField]
		private string leavingVirtualStumpString = "Now Leaving the Virtual Stump";

		// Token: 0x04004013 RID: 16403
		[SerializeField]
		private TMP_Text teleportingStatusText;

		// Token: 0x04004014 RID: 16404
		[SerializeField]
		private int maxNumProgressDots = 3;

		// Token: 0x04004015 RID: 16405
		[SerializeField]
		private float textUpdateInterval = 0.5f;

		// Token: 0x04004016 RID: 16406
		private float lastTextUpdateTime;

		// Token: 0x04004017 RID: 16407
		private int numProgressDots;

		// Token: 0x04004018 RID: 16408
		private bool isEnteringVirtualStump;
	}
}
