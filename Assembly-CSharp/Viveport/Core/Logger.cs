using System;
using System.Reflection;

namespace Viveport.Core
{
	// Token: 0x0200095C RID: 2396
	public class Logger
	{
		// Token: 0x060039F9 RID: 14841 RVA: 0x00055C46 File Offset: 0x00053E46
		public static void Log(string message)
		{
			if (!Logger._hasDetected || Logger._usingUnityLog)
			{
				Logger.UnityLog(message);
				return;
			}
			Logger.ConsoleLog(message);
		}

		// Token: 0x060039FA RID: 14842 RVA: 0x00055C63 File Offset: 0x00053E63
		private static void ConsoleLog(string message)
		{
			Console.WriteLine(message);
			Logger._hasDetected = true;
		}

		// Token: 0x060039FB RID: 14843 RVA: 0x0014C920 File Offset: 0x0014AB20
		private static void UnityLog(string message)
		{
			try
			{
				if (Logger._unityLogType == null)
				{
					Logger._unityLogType = Logger.GetType("UnityEngine.Debug");
				}
				Logger._unityLogType.GetMethod("Log", new Type[]
				{
					typeof(string)
				}).Invoke(null, new object[]
				{
					message
				});
				Logger._usingUnityLog = true;
			}
			catch (Exception)
			{
				Logger.ConsoleLog(message);
				Logger._usingUnityLog = false;
			}
			Logger._hasDetected = true;
		}

		// Token: 0x060039FC RID: 14844 RVA: 0x0014C9AC File Offset: 0x0014ABAC
		private static Type GetType(string typeName)
		{
			Type type = Type.GetType(typeName);
			if (type != null)
			{
				return type;
			}
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				type = assemblies[i].GetType(typeName);
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}

		// Token: 0x04003BBF RID: 15295
		private const string LoggerTypeNameUnity = "UnityEngine.Debug";

		// Token: 0x04003BC0 RID: 15296
		private static bool _hasDetected;

		// Token: 0x04003BC1 RID: 15297
		private static bool _usingUnityLog = true;

		// Token: 0x04003BC2 RID: 15298
		private static Type _unityLogType;
	}
}
