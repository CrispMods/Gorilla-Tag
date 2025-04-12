using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x020008AA RID: 2218
public static class UnityYaml
{
	// Token: 0x040037E4 RID: 14308
	private static readonly Assembly EngineAssembly = Assembly.GetAssembly(typeof(MonoBehaviour));

	// Token: 0x040037E5 RID: 14309
	private static readonly Assembly TerrainAssembly = Assembly.GetAssembly(typeof(Tree));

	// Token: 0x040037E6 RID: 14310
	public static Dictionary<int, Type> ClassIDToType = new Dictionary<int, Type>();
}
