using System;
using UnityEngine;

// Token: 0x02000084 RID: 132
[CreateAssetMenu(fileName = "New Game Mode Selector Button Layout Data", menuName = "Game Settings/Game Mode Selector Button Layout Data", order = 1)]
public class GameModeSelectorButtonLayoutData : ScriptableObject
{
	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000371 RID: 881 RVA: 0x00032A8E File Offset: 0x00030C8E
	public ModeSelectButtonInfoData[] Info
	{
		get
		{
			return this.info;
		}
	}

	// Token: 0x040003FD RID: 1021
	[SerializeField]
	private ModeSelectButtonInfoData[] info;
}
