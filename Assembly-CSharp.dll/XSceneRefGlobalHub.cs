using System;
using System.Collections.Generic;

// Token: 0x0200021B RID: 539
public static class XSceneRefGlobalHub
{
	// Token: 0x06000C7E RID: 3198 RVA: 0x0009DEA0 File Offset: 0x0009C0A0
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

	// Token: 0x06000C7F RID: 3199 RVA: 0x0009DED0 File Offset: 0x0009C0D0
	public static void Unregister(int ID, XSceneRefTarget obj)
	{
		int sceneIndex = (int)obj.GetSceneIndex();
		if (ID > 0 && sceneIndex >= 0)
		{
			XSceneRefGlobalHub.registry[sceneIndex].Remove(ID);
		}
	}

	// Token: 0x06000C80 RID: 3200 RVA: 0x00037C2C File Offset: 0x00035E2C
	public static bool TryResolve(SceneIndex sceneIndex, int ID, out XSceneRefTarget result)
	{
		return XSceneRefGlobalHub.registry[(int)sceneIndex].TryGetValue(ID, out result);
	}

	// Token: 0x04000FE3 RID: 4067
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
