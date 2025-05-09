﻿using System;
using UnityEngine;

// Token: 0x0200007D RID: 125
[CreateAssetMenu(fileName = "New Game Mode Selector Button Layout Data", menuName = "Game Settings/Game Mode Selector Button Layout Data", order = 1)]
public class GameModeSelectorButtonLayoutData : ScriptableObject
{
	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000341 RID: 833 RVA: 0x0003192B File Offset: 0x0002FB2B
	public ModeSelectButtonInfoData[] Info
	{
		get
		{
			return this.info;
		}
	}

	// Token: 0x040003CA RID: 970
	[SerializeField]
	private ModeSelectButtonInfoData[] info;
}
