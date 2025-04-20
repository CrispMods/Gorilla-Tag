using System;
using System.Collections.Generic;
using Fusion;
using GorillaGameModes;
using UnityEngine;

// Token: 0x02000260 RID: 608
public class CustomObjectProvider : NetworkObjectProviderDefault
{
	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000E16 RID: 3606 RVA: 0x0003A148 File Offset: 0x00038348
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

	// Token: 0x06000E17 RID: 3607 RVA: 0x0003A15E File Offset: 0x0003835E
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

	// Token: 0x06000E18 RID: 3608 RVA: 0x0003A178 File Offset: 0x00038378
	private void IsGameMode(NetworkObject instance)
	{
		if (instance.gameObject.GetComponent<GameModeSerializer>() != null)
		{
			GorillaGameModes.GameMode.GetGameModeInstance(GorillaGameModes.GameMode.GetGameModeKeyFromRoomProp()).AddFusionDataBehaviour(instance);
			CustomObjectProvider.Baker.Bake(instance.gameObject);
		}
	}

	// Token: 0x06000E19 RID: 3609 RVA: 0x0003A1AE File Offset: 0x000383AE
	protected override void DestroySceneObject(NetworkRunner runner, NetworkSceneObjectId sceneObjectId, NetworkObject instance)
	{
		if (this.SceneObjects != null && this.SceneObjects.Contains(instance.gameObject))
		{
			return;
		}
		base.DestroySceneObject(runner, sceneObjectId, instance);
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x0003A1D5 File Offset: 0x000383D5
	protected override void DestroyPrefabInstance(NetworkRunner runner, NetworkPrefabId prefabId, NetworkObject instance)
	{
		base.DestroyPrefabInstance(runner, prefabId, instance);
	}

	// Token: 0x04001120 RID: 4384
	public const int GameModeFlag = 1;

	// Token: 0x04001121 RID: 4385
	public const int PlayerFlag = 2;

	// Token: 0x04001122 RID: 4386
	private static NetworkObjectBaker baker;

	// Token: 0x04001123 RID: 4387
	internal List<GameObject> SceneObjects;
}
