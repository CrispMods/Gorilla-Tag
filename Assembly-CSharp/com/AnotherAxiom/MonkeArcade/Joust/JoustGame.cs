using System;
using UnityEngine;

namespace com.AnotherAxiom.MonkeArcade.Joust
{
	// Token: 0x02000B28 RID: 2856
	public class JoustGame : ArcadeGame
	{
		// Token: 0x06004728 RID: 18216 RVA: 0x00152F9E File Offset: 0x0015119E
		public override byte[] GetNetworkState()
		{
			return new byte[0];
		}

		// Token: 0x06004729 RID: 18217 RVA: 0x000023F4 File Offset: 0x000005F4
		public override void SetNetworkState(byte[] obj)
		{
		}

		// Token: 0x0600472A RID: 18218 RVA: 0x00152FA6 File Offset: 0x001511A6
		protected override void ButtonDown(int player, ArcadeButtons button)
		{
			if (button != ArcadeButtons.GRAB)
			{
				if (button == ArcadeButtons.TRIGGER)
				{
					this.joustPlayers[player].Flap();
					return;
				}
			}
			else
			{
				this.joustPlayers[player].gameObject.SetActive(true);
			}
		}

		// Token: 0x0600472B RID: 18219 RVA: 0x00152FD5 File Offset: 0x001511D5
		protected override void ButtonUp(int player, ArcadeButtons button)
		{
			if (button == ArcadeButtons.GRAB)
			{
				this.joustPlayers[player].gameObject.SetActive(false);
			}
		}

		// Token: 0x0600472C RID: 18220 RVA: 0x00152FF0 File Offset: 0x001511F0
		private void Start()
		{
			for (int i = 0; i < this.joustPlayers.Length; i++)
			{
				this.joustPlayers[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x00153024 File Offset: 0x00151224
		private void Update()
		{
			for (int i = 0; i < this.joustPlayers.Length; i++)
			{
				if (this.joustPlayers[i].gameObject.activeInHierarchy)
				{
					int num = (base.getButtonState(i, ArcadeButtons.LEFT) ? -1 : 0) + (base.getButtonState(i, ArcadeButtons.RIGHT) ? 1 : 0);
					this.joustPlayers[i].HorizontalSpeed = Mathf.Clamp(this.joustPlayers[i].HorizontalSpeed + (float)num * Time.deltaTime, -1f, 1f);
				}
			}
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x000023F4 File Offset: 0x000005F4
		public override void OnTimeout()
		{
		}

		// Token: 0x040048D5 RID: 18645
		[SerializeField]
		private JoustPlayer[] joustPlayers;
	}
}
