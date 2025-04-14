using System;
using System.Reflection;

namespace Viveport.Core
{
	// Token: 0x02000942 RID: 2370
	public class Logger
	{
		// Token: 0x06003934 RID: 14644 RVA: 0x00108D10 File Offset: 0x00106F10
		public static void Log(string message)
		{
			if (!Logger._hasDetected || Logger._usingUnityLog)
			{
				Logger.UnityLog(message);
				return;
			}
			Logger.ConsoleLog(message);
		}

		// Token: 0x06003935 RID: 14645 RVA: 0x00108D2D File Offset: 0x00106F2D
		private static void ConsoleLog(string message)
		{
			Console.WriteLine(message);
			Logger._hasDetected = true;
		}

		// Token: 0x06003936 RID: 14646 RVA: 0x00108D3C File Offset: 0x00106F3C
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

		// Token: 0x06003937 RID: 14647 RVA: 0x00108DC8 File Offset: 0x00106FC8
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

		// Token: 0x04003B0C RID: 15116
		private const string LoggerTypeNameUnity = "UnityEngine.Debug";

		// Token: 0x04003B0D RID: 15117
		private static bool _hasDetected;

		// Token: 0x04003B0E RID: 15118
		private static bool _usingUnityLog = true;

		// Token: 0x04003B0F RID: 15119
		private static Type _unityLogType;
	}
}
