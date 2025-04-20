using System;
using UnityEngine;

namespace com.AnotherAxiom.MonkeArcade.Joust
{
	// Token: 0x02000B55 RID: 2901
	public class JoustGame : ArcadeGame
	{
		// Token: 0x06004871 RID: 18545 RVA: 0x0005F2D4 File Offset: 0x0005D4D4
		public override byte[] GetNetworkState()
		{
			return new byte[0];
		}

		// Token: 0x06004872 RID: 18546 RVA: 0x00030607 File Offset: 0x0002E807
		public override void SetNetworkState(byte[] obj)
		{
		}

		// Token: 0x06004873 RID: 18547 RVA: 0x0005F2DC File Offset: 0x0005D4DC
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

		// Token: 0x06004874 RID: 18548 RVA: 0x0005F30B File Offset: 0x0005D50B
		protected override void ButtonUp(int player, ArcadeButtons button)
		{
			if (button == ArcadeButtons.GRAB)
			{
				this.joustPlayers[player].gameObject.SetActive(false);
			}
		}

		// Token: 0x06004875 RID: 18549 RVA: 0x0018F4CC File Offset: 0x0018D6CC
		private void Start()
		{
			for (int i = 0; i < this.joustPlayers.Length; i++)
			{
				this.joustPlayers[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06004876 RID: 18550 RVA: 0x0018F500 File Offset: 0x0018D700
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

		// Token: 0x06004877 RID: 18551 RVA: 0x00030607 File Offset: 0x0002E807
		public override void OnTimeout()
		{
		}

		// Token: 0x040049CA RID: 18890
		[SerializeField]
		private JoustPlayer[] joustPlayers;
	}
}
