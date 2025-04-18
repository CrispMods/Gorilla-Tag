﻿using System;
using UnityEngine;

// Token: 0x0200024E RID: 590
public class TestScreen : ArcadeGame
{
	// Token: 0x06000DB4 RID: 3508 RVA: 0x00037F8B File Offset: 0x0003618B
	public override byte[] GetNetworkState()
	{
		return null;
	}

	// Token: 0x06000DB5 RID: 3509 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void SetNetworkState(byte[] b)
	{
	}

	// Token: 0x06000DB6 RID: 3510 RVA: 0x000A088C File Offset: 0x0009EA8C
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

	// Token: 0x06000DB7 RID: 3511 RVA: 0x00038D28 File Offset: 0x00036F28
	protected override void ButtonUp(int player, ArcadeButtons button)
	{
		this.lights[this.buttonToLightIndex(player, button)].color = Color.red;
	}

	// Token: 0x06000DB8 RID: 3512 RVA: 0x00038D43 File Offset: 0x00036F43
	protected override void ButtonDown(int player, ArcadeButtons button)
	{
		this.lights[this.buttonToLightIndex(player, button)].color = Color.green;
	}

	// Token: 0x06000DB9 RID: 3513 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void OnTimeout()
	{
	}

	// Token: 0x040010C4 RID: 4292
	[SerializeField]
	private SpriteRenderer[] lights;

	// Token: 0x040010C5 RID: 4293
	[SerializeField]
	private Transform dot;
}
