using System;
using System.Reflection;

namespace Viveport.Core
{
	// Token: 0x0200093F RID: 2367
	public class Logger
	{
		// Token: 0x06003928 RID: 14632 RVA: 0x00108748 File Offset: 0x00106948
		public static void Log(string message)
		{
			if (!Logger._hasDetected || Logger._usingUnityLog)
			{
				Logger.UnityLog(message);
				return;
			}
			Logger.ConsoleLog(message);
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x00108765 File Offset: 0x00106965
		private static void ConsoleLog(string message)
		{
			Console.WriteLine(message);
			Logger._hasDetected = true;
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x00108774 File Offset: 0x00106974
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

		// Token: 0x0600392B RID: 14635 RVA: 0x00108800 File Offset: 0x00106A00
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

		// Token: 0x04003AFA RID: 15098
		private const string LoggerTypeNameUnity = "UnityEngine.Debug";

		// Token: 0x04003AFB RID: 15099
		private static bool _hasDetected;

		// Token: 0x04003AFC RID: 15100
		private static bool _usingUnityLog = true;

		// Token: 0x04003AFD RID: 15101
		private static Type _unityLogType;
	}
}
