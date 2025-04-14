using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GorillaNetworking;
using KID.Api;
using KID.Client;
using KID.Model;
using UnityEngine;

// Token: 0x020006FD RID: 1789
internal static class KIDIntegrationRouter
{
	// Token: 0x06002C55 RID: 11349 RVA: 0x000DB0DA File Offset: 0x000D92DA
	public static AgeGateAPIsApi GetKIDAgeGateAPI()
	{
		if (KIDIntegrationRouter._kIDAgeGateAPI == null || KIDIntegrationRouter._kIDAgeGateAPI.Configuration.AccessToken != KIDIntegrationRouter._kIDConfiguration.AccessToken)
		{
			KIDIntegrationRouter._kIDAgeGateAPI = new AgeGateAPIsApi(KIDIntegrationRouter.GetKIDConfig());
		}
		return KIDIntegrationRouter._kIDAgeGateAPI;
	}

	// Token: 0x06002C56 RID: 11350 RVA: 0x000DB117 File Offset: 0x000D9317
	public static ChallengeAPIsApi GetKIDChallengeAPI()
	{
		if (KIDIntegrationRouter._kIDChallengeAPI == null || KIDIntegrationRouter._kIDChallengeAPI.Configuration.AccessToken != KIDIntegrationRouter._kIDConfiguration.AccessToken)
		{
			KIDIntegrationRouter._kIDChallengeAPI = new ChallengeAPIsApi(KIDIntegrationRouter.GetKIDConfig());
		}
		return KIDIntegrationRouter._kIDChallengeAPI;
	}

	// Token: 0x06002C57 RID: 11351 RVA: 0x000DB154 File Offset: 0x000D9354
	public static SessionAPIsApi GetKIDSessionAPI()
	{
		if (KIDIntegrationRouter._kIDSessionAPI == null || KIDIntegrationRouter._kIDSessionAPI.Configuration.AccessToken != KIDIntegrationRouter._kIDConfiguration.AccessToken)
		{
			KIDIntegrationRouter._kIDSessionAPI = new SessionAPIsApi(KIDIntegrationRouter.GetKIDConfig());
		}
		return KIDIntegrationRouter._kIDSessionAPI;
	}

	// Token: 0x06002C58 RID: 11352 RVA: 0x000DB194 File Offset: 0x000D9394
	public static Task<bool> RefreshToken()
	{
		KIDIntegrationRouter.<RefreshToken>d__9 <RefreshToken>d__;
		<RefreshToken>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<RefreshToken>d__.<>1__state = -1;
		<RefreshToken>d__.<>t__builder.Start<KIDIntegrationRouter.<RefreshToken>d__9>(ref <RefreshToken>d__);
		return <RefreshToken>d__.<>t__builder.Task;
	}

	// Token: 0x06002C59 RID: 11353 RVA: 0x000DB1D0 File Offset: 0x000D93D0
	public static Task<bool> AuthenticateKIDClient()
	{
		KIDIntegrationRouter.<AuthenticateKIDClient>d__10 <AuthenticateKIDClient>d__;
		<AuthenticateKIDClient>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<AuthenticateKIDClient>d__.<>1__state = -1;
		<AuthenticateKIDClient>d__.<>t__builder.Start<KIDIntegrationRouter.<AuthenticateKIDClient>d__10>(ref <AuthenticateKIDClient>d__);
		return <AuthenticateKIDClient>d__.<>t__builder.Task;
	}

	// Token: 0x06002C5A RID: 11354 RVA: 0x000DB20C File Offset: 0x000D940C
	public static CheckAgeGateRequest CreateAgeGateRequest(DateTime dateOfBirth)
	{
		if (dateOfBirth == default(DateTime))
		{
			dateOfBirth = DateTime.Today;
		}
		return new CheckAgeGateRequest(KIDManager.Location, dateOfBirth);
	}

	// Token: 0x06002C5B RID: 11355 RVA: 0x000DB23C File Offset: 0x000D943C
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

	// Token: 0x06002C5C RID: 11356 RVA: 0x000DB30E File Offset: 0x000D950E
	private static AuthAPIsApi GetKIDAuthAPI()
	{
		if (KIDIntegrationRouter._kIDAuthAPI == null || KIDIntegrationRouter._kIDAuthAPI.Configuration.AccessToken != KIDIntegrationRouter._kIDConfiguration.AccessToken)
		{
			KIDIntegrationRouter._kIDAuthAPI = new AuthAPIsApi(KIDIntegrationRouter.GetKIDConfig());
		}
		return KIDIntegrationRouter._kIDAuthAPI;
	}

	// Token: 0x06002C5D RID: 11357 RVA: 0x000DB34B File Offset: 0x000D954B
	private static CreateClientAuthTokenRequest GetClientAuthToken()
	{
		return new CreateClientAuthTokenRequest(PlayFabAuthenticator.instance.GetPlayFabPlayerId());
	}

	// Token: 0x040031B1 RID: 12721
	private const string DATE_FORMAT = "yyyy-MM-dd";

	// Token: 0x040031B2 RID: 12722
	private static Configuration _kIDConfiguration;

	// Token: 0x040031B3 RID: 12723
	private static AuthAPIsApi _kIDAuthAPI;

	// Token: 0x040031B4 RID: 12724
	private static AgeGateAPIsApi _kIDAgeGateAPI;

	// Token: 0x040031B5 RID: 12725
	private static ChallengeAPIsApi _kIDChallengeAPI;

	// Token: 0x040031B6 RID: 12726
	private static SessionAPIsApi _kIDSessionAPI;
}
