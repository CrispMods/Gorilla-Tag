using System;
using System.Collections.Generic;
using GorillaGameModes;
using UnityEngine;

// Token: 0x0200008A RID: 138
public class GameModeSpecificObjectRegistry : MonoBehaviour
{
	// Token: 0x06000384 RID: 900 RVA: 0x00032AEF File Offset: 0x00030CEF
	private void OnEnable()
	{
		GameModeSpecificObject.OnAwake += this.GameModeSpecificObject_OnAwake;
		GameModeSpecificObject.OnDestroyed += this.GameModeSpecificObject_OnDestroyed;
		GameMode.OnStartGameMode += this.GameMode_OnStartGameMode;
	}

	// Token: 0x06000385 RID: 901 RVA: 0x00032B24 File Offset: 0x00030D24
	private void OnDisable()
	{
		GameModeSpecificObject.OnAwake -= this.GameModeSpecificObject_OnAwake;
		GameModeSpecificObject.OnDestroyed -= this.GameModeSpecificObject_OnDestroyed;
		GameMode.OnStartGameMode -= this.GameMode_OnStartGameMode;
	}

	// Token: 0x06000386 RID: 902 RVA: 0x0007957C File Offset: 0x0007777C
	private void GameModeSpecificObject_OnAwake(GameModeSpecificObject obj)
	{
		foreach (GameModeType key in obj.GameModes)
		{
			if (!this.gameModeSpecificObjects.ContainsKey(key))
			{
				this.gameModeSpecificObjects.Add(key, new List<GameModeSpecificObject>());
			}
			this.gameModeSpecificObjects[key].Add(obj);
		}
		if (GameMode.ActiveGameMode == null)
		{
			obj.gameObject.SetActive(obj.Validation == GameModeSpecificObject.ValidationMethod.Exclusion);
			return;
		}
		obj.gameObject.SetActive(obj.CheckValid(GameMode.ActiveGameMode.GameType()));
	}

	// Token: 0x06000387 RID: 903 RVA: 0x00079638 File Offset: 0x00077838
	private void GameModeSpecificObject_OnDestroyed(GameModeSpecificObject obj)
	{
		foreach (GameModeType key in obj.GameModes)
		{
			if (this.gameModeSpecificObjects.ContainsKey(key))
			{
				this.gameModeSpecificObjects[key].Remove(obj);
			}
		}
	}

	// Token: 0x06000388 RID: 904 RVA: 0x000796A8 File Offset: 0x000778A8
	private void GameMode_OnStartGameMode(GameModeType newGameModeType)
	{
		if (this.currentGameType == newGameModeType)
		{
			return;
		}
		if (this.gameModeSpecificObjects.ContainsKey(this.currentGameType))
		{
			foreach (GameModeSpecificObject gameModeSpecificObject in this.gameModeSpecificObjects[this.currentGameType])
			{
				gameModeSpecificObject.gameObject.SetActive(gameModeSpecificObject.CheckValid(newGameModeType));
			}
		}
		if (this.gameModeSpecificObjects.ContainsKey(newGameModeType))
		{
			foreach (GameModeSpecificObject gameModeSpecificObject2 in this.gameModeSpecificObjects[newGameModeType])
			{
				gameModeSpecificObject2.gameObject.SetActive(gameModeSpecificObject2.CheckValid(newGameModeType));
			}
		}
		this.currentGameType = newGameModeType;
	}

	// Token: 0x0400040E RID: 1038
	private Dictionary<GameModeType, List<GameModeSpecificObject>> gameModeSpecificObjects = new Dictionary<GameModeType, List<GameModeSpecificObject>>();

	// Token: 0x0400040F RID: 1039
	private GameModeType currentGameType = GameModeType.Count;
}
