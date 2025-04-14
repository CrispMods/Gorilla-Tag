using System;
using System.Collections.Generic;
using Fusion;
using GorillaGameModes;
using UnityEngine;

// Token: 0x02000255 RID: 597
public class CustomObjectProvider : NetworkObjectProviderDefault
{
	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06000DCD RID: 3533 RVA: 0x0004654C File Offset: 0x0004474C
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

	// Token: 0x06000DCE RID: 3534 RVA: 0x00046562 File Offset: 0x00044762
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

	// Token: 0x06000DCF RID: 3535 RVA: 0x0004657C File Offset: 0x0004477C
	private void IsGameMode(NetworkObject instance)
	{
		if (instance.gameObject.GetComponent<GameModeSerializer>() != null)
		{
			GorillaGameModes.GameMode.GetGameModeInstance(GorillaGameModes.GameMode.GetGameModeKeyFromRoomProp()).AddFusionDataBehaviour(instance);
			CustomObjectProvider.Baker.Bake(instance.gameObject);
		}
	}

	// Token: 0x06000DD0 RID: 3536 RVA: 0x000465B2 File Offset: 0x000447B2
	protected override void DestroySceneObject(NetworkRunner runner, NetworkSceneObjectId sceneObjectId, NetworkObject instance)
	{
		if (this.SceneObjects != null && this.SceneObjects.Contains(instance.gameObject))
		{
			return;
		}
		base.DestroySceneObject(runner, sceneObjectId, instance);
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x000465D9 File Offset: 0x000447D9
	protected override void DestroyPrefabInstance(NetworkRunner runner, NetworkPrefabId prefabId, NetworkObject instance)
	{
		base.DestroyPrefabInstance(runner, prefabId, instance);
	}

	// Token: 0x040010DB RID: 4315
	public const int GameModeFlag = 1;

	// Token: 0x040010DC RID: 4316
	public const int PlayerFlag = 2;

	// Token: 0x040010DD RID: 4317
	private static NetworkObjectBaker baker;

	// Token: 0x040010DE RID: 4318
	internal List<GameObject> SceneObjects;
}
