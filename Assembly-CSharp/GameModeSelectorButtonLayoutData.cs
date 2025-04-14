using System;
using UnityEngine;

// Token: 0x0200007D RID: 125
[CreateAssetMenu(fileName = "New Game Mode Selector Button Layout Data", menuName = "Game Settings/Game Mode Selector Button Layout Data", order = 1)]
public class GameModeSelectorButtonLayoutData : ScriptableObject
{
	// Token: 0x1700002E RID: 46
	// (get) Token: 0x0600033F RID: 831 RVA: 0x00014F6A File Offset: 0x0001316A
	public ModeSelectButtonInfoData[] Info
	{
		get
		{
			return this.info;
		}
	}

	// Token: 0x040003C9 RID: 969
	[SerializeField]
	private ModeSelectButtonInfoData[] info;
}
