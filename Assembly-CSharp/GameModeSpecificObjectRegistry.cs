using System;
using System.Collections.Generic;
using GorillaGameModes;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class GameModeSpecificObjectRegistry : MonoBehaviour
{
	// Token: 0x06000352 RID: 850 RVA: 0x000151B2 File Offset: 0x000133B2
	private void OnEnable()
	{
		GameModeSpecificObject.OnAwake += this.GameModeSpecificObject_OnAwake;
		GameModeSpecificObject.OnDestroyed += this.GameModeSpecificObject_OnDestroyed;
		GameMode.OnStartGameMode += this.GameMode_OnStartGameMode;
	}

	// Token: 0x06000353 RID: 851 RVA: 0x000151E7 File Offset: 0x000133E7
	private void OnDisable()
	{
		GameModeSpecificObject.OnAwake -= this.GameModeSpecificObject_OnAwake;
		GameModeSpecificObject.OnDestroyed -= this.GameModeSpecificObject_OnDestroyed;
		GameMode.OnStartGameMode -= this.GameMode_OnStartGameMode;
	}

	// Token: 0x06000354 RID: 852 RVA: 0x0001521C File Offset: 0x0001341C
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

	// Token: 0x06000355 RID: 853 RVA: 0x000152D8 File Offset: 0x000134D8
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

	// Token: 0x06000356 RID: 854 RVA: 0x00015348 File Offset: 0x00013548
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

	// Token: 0x040003DA RID: 986
	private Dictionary<GameModeType, List<GameModeSpecificObject>> gameModeSpecificObjects = new Dictionary<GameModeType, List<GameModeSpecificObject>>();

	// Token: 0x040003DB RID: 987
	private GameModeType currentGameType = GameModeType.Count;
}
