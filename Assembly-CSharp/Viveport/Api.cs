using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using AOT;
using LitJson;
using PublicKeyConvert;
using Viveport.Core;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x02000905 RID: 2309
	public class Api
	{
		// Token: 0x06003767 RID: 14183 RVA: 0x001053EC File Offset: 0x001035EC
		public static void GetLicense(Api.LicenseChecker checker, string appId, string appKey)
		{
			if (checker == null || string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appKey))
			{
				throw new InvalidOperationException("checker == null || string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appKey)");
			}
			Api._appId = appId;
			Api._appKey = appKey;
			Api.InternalLicenseCheckers.Add(checker);
			if (IntPtr.Size == 8)
			{
				Api.GetLicense_64(new GetLicenseCallback(Api.GetLicenseHandler), Api._appId, Api._appKey);
				return;
			}
			Api.GetLicense(new GetLicenseCallback(Api.GetLicenseHandler), Api._appId, Api._appKey);
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x0010546D File Offset: 0x0010366D
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void InitIl2cppCallback(int errorCode)
		{
			Api.initIl2cppCallback(errorCode);
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x0010547C File Offset: 0x0010367C
		public static int Init(StatusCallback callback, string appId)
		{
			if (callback == null || string.IsNullOrEmpty(appId))
			{
				throw new InvalidOperationException("callback == null || string.IsNullOrEmpty(appId)");
			}
			Api.initIl2cppCallback = new StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(new StatusCallback(Api.InitIl2cppCallback));
			if (IntPtr.Size == 8)
			{
				return Api.Init_64(new StatusCallback(Api.InitIl2cppCallback), appId);
			}
			return Api.Init(new StatusCallback(Api.InitIl2cppCallback), appId);
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x001054F3 File Offset: 0x001036F3
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void ShutdownIl2cppCallback(int errorCode)
		{
			Api.shutdownIl2cppCallback(errorCode);
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x00105500 File Offset: 0x00103700
		public static int Shutdown(StatusCallback callback)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
			Api.shutdownIl2cppCallback = new StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(new StatusCallback(Api.ShutdownIl2cppCallback));
			if (IntPtr.Size == 8)
			{
				return Api.Shutdown_64(new StatusCallback(Api.ShutdownIl2cppCallback));
			}
			return Api.Shutdown(new StatusCallback(Api.ShutdownIl2cppCallback));
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x00105570 File Offset: 0x00103770
		public static string Version()
		{
			string text = "";
			try
			{
				if (IntPtr.Size == 8)
				{
					text += Marshal.PtrToStringAnsi(Api.Version_64());
				}
				else
				{
					text += Marshal.PtrToStringAnsi(Api.Version());
				}
			}
			catch (Exception)
			{
				Logger.Log("Can not load version from native library");
			}
			return "C# version: " + Api.VERSION + ", Native version: " + text;
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x001055E4 File Offset: 0x001037E4
		[MonoPInvokeCallback(typeof(QueryRuntimeModeCallback))]
		private static void QueryRuntimeModeIl2cppCallback(int errorCode, int mode)
		{
			Api.queryRuntimeModeIl2cppCallback(errorCode, mode);
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x001055F4 File Offset: 0x001037F4
		public static void QueryRuntimeMode(QueryRuntimeModeCallback callback)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
			Api.queryRuntimeModeIl2cppCallback = new QueryRuntimeModeCallback(callback.Invoke);
			Api.InternalQueryRunTimeCallbacks.Add(new QueryRuntimeModeCallback(Api.QueryRuntimeModeIl2cppCallback));
			if (IntPtr.Size == 8)
			{
				Api.QueryRuntimeMode_64(new QueryRuntimeModeCallback(Api.QueryRuntimeModeIl2cppCallback));
				return;
			}
			Api.QueryRuntimeMode(new QueryRuntimeModeCallback(Api.QueryRuntimeModeIl2cppCallback));
		}

		// Token: 0x0600376F RID: 14191 RVA: 0x00105664 File Offset: 0x00103864
		[MonoPInvokeCallback(typeof(GetLicenseCallback))]
		private static void GetLicenseHandler([MarshalAs(UnmanagedType.LPStr)] string message, [MarshalAs(UnmanagedType.LPStr)] string signature)
		{
			if (string.IsNullOrEmpty(message))
			{
				for (int i = Api.InternalLicenseCheckers.Count - 1; i >= 0; i--)
				{
					Api.LicenseChecker licenseChecker = Api.InternalLicenseCheckers[i];
					licenseChecker.OnFailure(90003, "License message is empty");
					Api.InternalLicenseCheckers.Remove(licenseChecker);
				}
				return;
			}
			if (string.IsNullOrEmpty(signature))
			{
				JsonData jsonData = JsonMapper.ToObject(message);
				int errorCode = 99999;
				string errorMessage = "";
				try
				{
					errorCode = int.Parse((string)jsonData["code"]);
				}
				catch
				{
				}
				try
				{
					errorMessage = (string)jsonData["message"];
				}
				catch
				{
				}
				for (int j = Api.InternalLicenseCheckers.Count - 1; j >= 0; j--)
				{
					Api.LicenseChecker licenseChecker2 = Api.InternalLicenseCheckers[j];
					licenseChecker2.OnFailure(errorCode, errorMessage);
					Api.InternalLicenseCheckers.Remove(licenseChecker2);
				}
				return;
			}
			if (!Api.VerifyMessage(Api._appId, Api._appKey, message, signature))
			{
				for (int k = Api.InternalLicenseCheckers.Count - 1; k >= 0; k--)
				{
					Api.LicenseChecker licenseChecker3 = Api.InternalLicenseCheckers[k];
					licenseChecker3.OnFailure(90001, "License verification failed");
					Api.InternalLicenseCheckers.Remove(licenseChecker3);
				}
				return;
			}
			string @string = Encoding.UTF8.GetString(Convert.FromBase64String(message.Substring(message.IndexOf("\n", StringComparison.Ordinal) + 1)));
			JsonData jsonData2 = JsonMapper.ToObject(@string);
			Logger.Log("License: " + @string);
			long issueTime = -1L;
			long expirationTime = -1L;
			int latestVersion = -1;
			bool updateRequired = false;
			try
			{
				issueTime = (long)jsonData2["issueTime"];
			}
			catch
			{
			}
			try
			{
				expirationTime = (long)jsonData2["expirationTime"];
			}
			catch
			{
			}
			try
			{
				latestVersion = (int)jsonData2["latestVersion"];
			}
			catch
			{
			}
			try
			{
				updateRequired = (bool)jsonData2["updateRequired"];
			}
			catch
			{
			}
			for (int l = Api.InternalLicenseCheckers.Count - 1; l >= 0; l--)
			{
				Api.LicenseChecker licenseChecker4 = Api.InternalLicenseCheckers[l];
				licenseChecker4.OnSuccess(issueTime, expirationTime, latestVersion, updateRequired);
				Api.InternalLicenseCheckers.Remove(licenseChecker4);
			}
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x001058F0 File Offset: 0x00103AF0
		private static bool VerifyMessage(string appId, string appKey, string message, string signature)
		{
			try
			{
				RSACryptoServiceProvider rsacryptoServiceProvider = PEMKeyLoader.CryptoServiceProviderFromPublicKeyInfo(appKey);
				byte[] signature2 = Convert.FromBase64String(signature);
				SHA1Managed halg = new SHA1Managed();
				byte[] bytes = Encoding.UTF8.GetBytes(appId + "\n" + message);
				return rsacryptoServiceProvider.VerifyData(bytes, halg, signature2);
			}
			catch (Exception ex)
			{
				Logger.Log(ex.ToString());
			}
			return false;
		}

		// Token: 0x04003A52 RID: 14930
		internal static readonly List<GetLicenseCallback> InternalGetLicenseCallbacks = new List<GetLicenseCallback>();

		// Token: 0x04003A53 RID: 14931
		internal static readonly List<StatusCallback> InternalStatusCallbacks = new List<StatusCallback>();

		// Token: 0x04003A54 RID: 14932
		internal static readonly List<QueryRuntimeModeCallback> InternalQueryRunTimeCallbacks = new List<QueryRuntimeModeCallback>();

		// Token: 0x04003A55 RID: 14933
		internal static readonly List<StatusCallback2> InternalStatusCallback2s = new List<StatusCallback2>();

		// Token: 0x04003A56 RID: 14934
		internal static readonly List<Api.LicenseChecker> InternalLicenseCheckers = new List<Api.LicenseChecker>();

		// Token: 0x04003A57 RID: 14935
		private static StatusCallback initIl2cppCallback;

		// Token: 0x04003A58 RID: 14936
		private static StatusCallback shutdownIl2cppCallback;

		// Token: 0x04003A59 RID: 14937
		private static QueryRuntimeModeCallback queryRuntimeModeIl2cppCallback;

		// Token: 0x04003A5A RID: 14938
		private static readonly string VERSION = "1.7.2.30";

		// Token: 0x04003A5B RID: 14939
		private static string _appId = "";

		// Token: 0x04003A5C RID: 14940
		private static string _appKey = "";

		// Token: 0x02000906 RID: 2310
		public abstract class LicenseChecker
		{
			// Token: 0x06003773 RID: 14195
			public abstract void OnSuccess(long issueTime, long expirationTime, int latestVersion, bool updateRequired);

			// Token: 0x06003774 RID: 14196
			public abstract void OnFailure(int errorCode, string errorMessage);
		}
	}
}
