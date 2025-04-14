using System;
using System.Collections.Generic;

// Token: 0x0200021B RID: 539
public static class XSceneRefGlobalHub
{
	// Token: 0x06000C7E RID: 3198 RVA: 0x000427E4 File Offset: 0x000409E4
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

	// Token: 0x06000C7F RID: 3199 RVA: 0x00042814 File Offset: 0x00040A14
	public static void Unregister(int ID, XSceneRefTarget obj)
	{
		int sceneIndex = (int)obj.GetSceneIndex();
		if (ID > 0 && sceneIndex >= 0)
		{
			XSceneRefGlobalHub.registry[sceneIndex].Remove(ID);
		}
	}

	// Token: 0x06000C80 RID: 3200 RVA: 0x00042842 File Offset: 0x00040A42
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
