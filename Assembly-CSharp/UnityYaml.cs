using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x020008A7 RID: 2215
public static class UnityYaml
{
	// Token: 0x040037D2 RID: 14290
	private static readonly Assembly EngineAssembly = Assembly.GetAssembly(typeof(MonoBehaviour));

	// Token: 0x040037D3 RID: 14291
	private static readonly Assembly TerrainAssembly = Assembly.GetAssembly(typeof(Tree));

	// Token: 0x040037D4 RID: 14292
	public static Dictionary<int, Type> ClassIDToType = new Dictionary<int, Type>();
}
