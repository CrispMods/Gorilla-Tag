using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GorillaNetworking;
using KID.Api;
using KID.Client;
using KID.Model;
using UnityEngine;

// Token: 0x020006FE RID: 1790
internal static class KIDIntegrationRouter
{
	// Token: 0x06002C5D RID: 11357 RVA: 0x0004D485 File Offset: 0x0004B685
	public static AgeGateAPIsApi GetKIDAgeGateAPI()
	{
		if (KIDIntegrationRouter._kIDAgeGateAPI == null || KIDIntegrationRouter._kIDAgeGateAPI.Configuration.AccessToken != KIDIntegrationRouter._kIDConfiguration.AccessToken)
		{
			KIDIntegrationRouter._kIDAgeGateAPI = new AgeGateAPIsApi(KIDIntegrationRouter.GetKIDConfig());
		}
		return KIDIntegrationRouter._kIDAgeGateAPI;
	}

	// Token: 0x06002C5E RID: 11358 RVA: 0x0004D4C2 File Offset: 0x0004B6C2
	public static ChallengeAPIsApi GetKIDChallengeAPI()
	{
		if (KIDIntegrationRouter._kIDChallengeAPI == null || KIDIntegrationRouter._kIDChallengeAPI.Configuration.AccessToken != KIDIntegrationRouter._kIDConfiguration.AccessToken)
		{
			KIDIntegrationRouter._kIDChallengeAPI = new ChallengeAPIsApi(KIDIntegrationRouter.GetKIDConfig());
		}
		return KIDIntegrationRouter._kIDChallengeAPI;
	}

	// Token: 0x06002C5F RID: 11359 RVA: 0x0004D4FF File Offset: 0x0004B6FF
	public static SessionAPIsApi GetKIDSessionAPI()
	{
		if (KIDIntegrationRouter._kIDSessionAPI == null || KIDIntegrationRouter._kIDSessionAPI.Configuration.AccessToken != KIDIntegrationRouter._kIDConfiguration.AccessToken)
		{
			KIDIntegrationRouter._kIDSessionAPI = new SessionAPIsApi(KIDIntegrationRouter.GetKIDConfig());
		}
		return KIDIntegrationRouter._kIDSessionAPI;
	}

	// Token: 0x06002C60 RID: 11360 RVA: 0x00121110 File Offset: 0x0011F310
	public static Task<bool> RefreshToken()
	{
		KIDIntegrationRouter.<RefreshToken>d__9 <RefreshToken>d__;
		<RefreshToken>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<RefreshToken>d__.<>1__state = -1;
		<RefreshToken>d__.<>t__builder.Start<KIDIntegrationRouter.<RefreshToken>d__9>(ref <RefreshToken>d__);
		return <RefreshToken>d__.<>t__builder.Task;
	}

	// Token: 0x06002C61 RID: 11361 RVA: 0x0012114C File Offset: 0x0011F34C
	public static Task<bool> AuthenticateKIDClient()
	{
		KIDIntegrationRouter.<AuthenticateKIDClient>d__10 <AuthenticateKIDClient>d__;
		<AuthenticateKIDClient>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<AuthenticateKIDClient>d__.<>1__state = -1;
		<AuthenticateKIDClient>d__.<>t__builder.Start<KIDIntegrationRouter.<AuthenticateKIDClient>d__10>(ref <AuthenticateKIDClient>d__);
		return <AuthenticateKIDClient>d__.<>t__builder.Task;
	}

	// Token: 0x06002C62 RID: 11362 RVA: 0x00121188 File Offset: 0x0011F388
	public static CheckAgeGateRequest CreateAgeGateRequest(DateTime dateOfBirth)
	{
		if (dateOfBirth == default(DateTime))
		{
			dateOfBirth = DateTime.Today;
		}
		return new CheckAgeGateRequest(KIDManager.Location, dateOfBirth);
	}

	// Token: 0x06002C63 RID: 11363 RVA: 0x001211B8 File Offset: 0x0011F3B8
	private static Configuration GetKIDConfig()
	{
		if (string.IsNullOrEmpty(KIDManager.KidAccessToken) || string.IsNullOrEmpty(KIDManager.KidRefreshToken) || string.IsNullOrEmpty(KIDManager.KidBasePath) || string.IsNullOrEmpty(KIDManager.Location))
		{
			Debug.LogError(string.Concat(new string[]
			{
				"[KID] Some or all config settings are invalid, cannot get k-ID Config. Settings are;\n- KidAccessToken: [",
				KIDManager.KidAccessToken,
				"\n- KidRefreshToken: [",
				KIDManager.KidRefreshToken,
				"\n- KidBasePath: [",
				KIDManager.KidBasePath,
				"\n- Location: [",
				KIDManager.Location
			}));
			return null;
		}
		if (KIDIntegrationRouter._kIDConfiguration == null)
		{
			KIDIntegrationRouter._kIDConfiguration = new Configuration();
			KIDIntegrationRouter._kIDConfiguration.BasePath = KIDManager.KidBasePath;
			KIDIntegrationRouter._kIDConfiguration.DateTimeFormat = "yyyy-MM-dd";
			KIDIntegrationRouter._kIDConfiguration.AccessToken = KIDManager.KidAccessToken;
		}
		return KIDIntegrationRouter._kIDConfiguration;
	}

	// Token: 0x06002C64 RID: 11364 RVA: 0x0004D53C File Offset: 0x0004B73C
	private static AuthAPIsApi GetKIDAuthAPI()
	{
		if (KIDIntegrationRouter._kIDAuthAPI == null || KIDIntegrationRouter._kIDAuthAPI.Configuration.AccessToken != KIDIntegrationRouter._kIDConfiguration.AccessToken)
		{
			KIDIntegrationRouter._kIDAuthAPI = new AuthAPIsApi(KIDIntegrationRouter.GetKIDConfig());
		}
		return KIDIntegrationRouter._kIDAuthAPI;
	}

	// Token: 0x06002C65 RID: 11365 RVA: 0x0004D579 File Offset: 0x0004B779
	private static CreateClientAuthTokenRequest GetClientAuthToken()
	{
		return new CreateClientAuthTokenRequest(PlayFabAuthenticator.instance.GetPlayFabPlayerId());
	}

	// Token: 0x040031B7 RID: 12727
	private const string DATE_FORMAT = "yyyy-MM-dd";

	// Token: 0x040031B8 RID: 12728
	private static Configuration _kIDConfiguration;

	// Token: 0x040031B9 RID: 12729
	private static AuthAPIsApi _kIDAuthAPI;

	// Token: 0x040031BA RID: 12730
	private static AgeGateAPIsApi _kIDAgeGateAPI;

	// Token: 0x040031BB RID: 12731
	private static ChallengeAPIsApi _kIDChallengeAPI;

	// Token: 0x040031BC RID: 12732
	private static SessionAPIsApi _kIDSessionAPI;
}
