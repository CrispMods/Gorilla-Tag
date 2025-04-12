using System;
using UnityEngine;

namespace com.AnotherAxiom.MonkeArcade.Joust
{
	// Token: 0x02000B2B RID: 2859
	public class JoustGame : ArcadeGame
	{
		// Token: 0x06004734 RID: 18228 RVA: 0x0005D8BD File Offset: 0x0005BABD
		public override byte[] GetNetworkState()
		{
			return new byte[0];
		}

		// Token: 0x06004735 RID: 18229 RVA: 0x0002F75F File Offset: 0x0002D95F
		public override void SetNetworkState(byte[] obj)
		{
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x0005D8C5 File Offset: 0x0005BAC5
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

		// Token: 0x06004737 RID: 18231 RVA: 0x0005D8F4 File Offset: 0x0005BAF4
		protected override void ButtonUp(int player, ArcadeButtons button)
		{
			if (button == ArcadeButtons.GRAB)
			{
				this.joustPlayers[player].gameObject.SetActive(false);
			}
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x00188558 File Offset: 0x00186758
		private void Start()
		{
			for (int i = 0; i < this.joustPlayers.Length; i++)
			{
				this.joustPlayers[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06004739 RID: 18233 RVA: 0x0018858C File Offset: 0x0018678C
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

		// Token: 0x0600473A RID: 18234 RVA: 0x0002F75F File Offset: 0x0002D95F
		public override void OnTimeout()
		{
		}

		// Token: 0x040048E7 RID: 18663
		[SerializeField]
		private JoustPlayer[] joustPlayers;
	}
}
