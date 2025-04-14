using System;
using System.Collections.Generic;
using Fusion;
using GorillaGameModes;
using UnityEngine;

// Token: 0x02000255 RID: 597
public class CustomObjectProvider : NetworkObjectProviderDefault
{
	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06000DCB RID: 3531 RVA: 0x00046208 File Offset: 0x00044408
	private static NetworkObjectBaker Baker
	{
		get
		{
			NetworkObjectBaker result;
			if ((result = CustomObjectProvider.baker) == null)
			{
				result = (CustomObjectProvider.baker = new NetworkObjectBaker());
			}
			return result;
		}
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x0004621E File Offset: 0x0004441E
	public override NetworkObjectAcquireResult AcquirePrefabInstance(NetworkRunner runner, in NetworkPrefabAcquireContext context, out NetworkObject instance)
	{
		NetworkObjectAcquireResult networkObjectAcquireResult = base.AcquirePrefabInstance(runner, context, out instance);
		if (networkObjectAcquireResult == NetworkObjectAcquireResult.Success)
		{
			this.IsGameMode(instance);
			return networkObjectAcquireResult;
		}
		instance = null;
		return networkObjectAcquireResult;
	}

	// Token: 0x06000DCD RID: 3533 RVA: 0x00046238 File Offset: 0x00044438
	private void IsGameMode(NetworkObject instance)
	{
		if (instance.gameObject.GetComponent<GameModeSerializer>() != null)
		{
			GorillaGameModes.GameMode.GetGameModeInstance(GorillaGameModes.GameMode.GetGameModeKeyFromRoomProp()).AddFusionDataBehaviour(instance);
			CustomObjectProvider.Baker.Bake(instance.gameObject);
		}
	}

	// Token: 0x06000DCE RID: 3534 RVA: 0x0004626E File Offset: 0x0004446E
	protected override void DestroySceneObject(NetworkRunner runner, NetworkSceneObjectId sceneObjectId, NetworkObject instance)
	{
		if (this.SceneObjects != null && this.SceneObjects.Contains(instance.gameObject))
		{
			return;
		}
		base.DestroySceneObject(runner, sceneObjectId, instance);
	}

	// Token: 0x06000DCF RID: 3535 RVA: 0x00046295 File Offset: 0x00044495
	protected override void DestroyPrefabInstance(NetworkRunner runner, NetworkPrefabId prefabId, NetworkObject instance)
	{
		base.DestroyPrefabInstance(runner, prefabId, instance);
	}

	// Token: 0x040010DA RID: 4314
	public const int GameModeFlag = 1;

	// Token: 0x040010DB RID: 4315
	public const int PlayerFlag = 2;

	// Token: 0x040010DC RID: 4316
	private static NetworkObjectBaker baker;

	// Token: 0x040010DD RID: 4317
	internal List<GameObject> SceneObjects;
}
