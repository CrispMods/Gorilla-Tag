using System;
using UnityEngine;

// Token: 0x02000259 RID: 601
public class TestScreen : ArcadeGame
{
	// Token: 0x06000DFD RID: 3581 RVA: 0x0003924B File Offset: 0x0003744B
	public override byte[] GetNetworkState()
	{
		return null;
	}

	// Token: 0x06000DFE RID: 3582 RVA: 0x00030607 File Offset: 0x0002E807
	public override void SetNetworkState(byte[] b)
	{
	}

	// Token: 0x06000DFF RID: 3583 RVA: 0x000A3118 File Offset: 0x000A1318
	private int buttonToLightIndex(int player, ArcadeButtons button)
	{
		int num = 0;
		if (button <= ArcadeButtons.RIGHT)
		{
			switch (button)
			{
			case ArcadeButtons.GRAB:
				num = 0;
				break;
			case ArcadeButtons.UP:
				num = 1;
				break;
			case ArcadeButtons.GRAB | ArcadeButtons.UP:
				break;
			case ArcadeButtons.DOWN:
				num = 2;
				break;
			default:
				if (button != ArcadeButtons.LEFT)
				{
					if (button == ArcadeButtons.RIGHT)
					{
						num = 4;
					}
				}
				else
				{
					num = 3;
				}
				break;
			}
		}
		else if (button != ArcadeButtons.B0)
		{
			if (button != ArcadeButtons.B1)
			{
				if (button == ArcadeButtons.TRIGGER)
				{
					num = 7;
				}
			}
			else
			{
				num = 6;
			}
		}
		else
		{
			num = 5;
		}
		return (player * 8 + num) % this.lights.Length;
	}

	// Token: 0x06000E00 RID: 3584 RVA: 0x00039FE8 File Offset: 0x000381E8
	protected override void ButtonUp(int player, ArcadeButtons button)
	{
		this.lights[this.buttonToLightIndex(player, button)].color = Color.red;
	}

	// Token: 0x06000E01 RID: 3585 RVA: 0x0003A003 File Offset: 0x00038203
	protected override void ButtonDown(int player, ArcadeButtons button)
	{
		this.lights[this.buttonToLightIndex(player, button)].color = Color.green;
	}

	// Token: 0x06000E02 RID: 3586 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnTimeout()
	{
	}

	// Token: 0x04001109 RID: 4361
	[SerializeField]
	private SpriteRenderer[] lights;

	// Token: 0x0400110A RID: 4362
	[SerializeField]
	private Transform dot;
}
