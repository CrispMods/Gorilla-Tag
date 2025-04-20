using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x020008C3 RID: 2243
public static class UnityYaml
{
	// Token: 0x04003893 RID: 14483
	private static readonly Assembly EngineAssembly = Assembly.GetAssembly(typeof(MonoBehaviour));

	// Token: 0x04003894 RID: 14484
	private static readonly Assembly TerrainAssembly = Assembly.GetAssembly(typeof(Tree));

	// Token: 0x04003895 RID: 14485
	public static Dictionary<int, Type> ClassIDToType = new Dictionary<int, Type>();
}
