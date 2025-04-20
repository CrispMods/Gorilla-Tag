using System;
using System.Collections.Generic;

// Token: 0x02000226 RID: 550
public static class XSceneRefGlobalHub
{
	// Token: 0x06000CC7 RID: 3271 RVA: 0x000A072C File Offset: 0x0009E92C
	public static void Register(int ID, XSceneRefTarget obj)
	{
		if (ID > 0)
		{
			int sceneIndex = (int)obj.GetSceneIndex();
			if (sceneIndex >= 0)
			{
				XSceneRefGlobalHub.registry[sceneIndex][ID] = obj;
			}
		}
	}

	// Token: 0x06000CC8 RID: 3272 RVA: 0x000A075C File Offset: 0x0009E95C
	public static void Unregister(int ID, XSceneRefTarget obj)
	{
		int sceneIndex = (int)obj.GetSceneIndex();
		if (ID > 0 && sceneIndex >= 0)
		{
			XSceneRefGlobalHub.registry[sceneIndex].Remove(ID);
		}
	}

	// Token: 0x06000CC9 RID: 3273 RVA: 0x00038EEC File Offset: 0x000370EC
	public static bool TryResolve(SceneIndex sceneIndex, int ID, out XSceneRefTarget result)
	{
		return XSceneRefGlobalHub.registry[(int)sceneIndex].TryGetValue(ID, out result);
	}

	// Token: 0x04001028 RID: 4136
	private static List<Dictionary<int, XSceneRefTarget>> registry = new List<Dictionary<int, XSceneRefTarget>>
	{
		new Dictionary<int, XSceneRefTarget>
		{
			{
				0,
				null
			}
		},
		new Dictionary<int, XSceneRefTarget>
		{
			{
				0,
				null
			}
		},
		new Dictionary<int, XSceneRefTarget>
		{
			{
				0,
				null
			}
		},
		new Dictionary<int, XSceneRefTarget>
		{
			{
				0,
				null
			}
		},
		new Dictionary<int, XSceneRefTarget>
		{
			{
				0,
				null
			}
		},
		new Dictionary<int, XSceneRefTarget>
		{
			{
				0,
				null
			}
		},
		new Dictionary<int, XSceneRefTarget>
		{
			{
				0,
				null
			}
		},
		new Dictionary<int, XSceneRefTarget>
		{
			{
				0,
				null
			}
		},
		new Dictionary<int, XSceneRefTarget>
		{
			{
				0,
				null
			}
		},
		new Dictionary<int, XSceneRefTarget>
		{
			{
				0,
				null
			}
		},
		new Dictionary<int, XSceneRefTarget>
		{
			{
				0,
				null
			}
		},
		new Dictionary<int, XSceneRefTarget>
		{
			{
				0,
				null
			}
		},
		new Dictionary<int, XSceneRefTarget>
		{
			{
				0,
				null
			}
		},
		new Dictionary<int, XSceneRefTarget>
		{
			{
				0,
				null
			}
		},
		new Dictionary<int, XSceneRefTarget>
		{
			{
				0,
				null
			}
		}
	};
}
