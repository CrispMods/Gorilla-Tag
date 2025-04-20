using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GorillaNetworking;
using KID.Api;
using KID.Client;
using KID.Model;
using UnityEngine;

// Token: 0x02000712 RID: 1810
internal static class KIDIntegrationRouter
{
	// Token: 0x06002CEB RID: 11499 RVA: 0x0004E7CA File Offset: 0x0004C9CA
	public static AgeGateAPIsApi GetKIDAgeGateAPI()
	{
		if (KIDIntegrationRouter._kIDAgeGateAPI == null || KIDIntegrationRouter._kIDAgeGateAPI.Configuration.AccessToken != KIDIntegrationRouter._kIDConfiguration.AccessToken)
		{
			KIDIntegrationRouter._kIDAgeGateAPI = new AgeGateAPIsApi(KIDIntegrationRouter.GetKIDConfig());
		}
		return KIDIntegrationRouter._kIDAgeGateAPI;
	}

	// Token: 0x06002CEC RID: 11500 RVA: 0x0004E807 File Offset: 0x0004CA07
	public static ChallengeAPIsApi GetKIDChallengeAPI()
	{
		if (KIDIntegrationRouter._kIDChallengeAPI == null || KIDIntegrationRouter._kIDChallengeAPI.Configuration.AccessToken != KIDIntegrationRouter._kIDConfiguration.AccessToken)
		{
			KIDIntegrationRouter._kIDChallengeAPI = new ChallengeAPIsApi(KIDIntegrationRouter.GetKIDConfig());
		}
		return KIDIntegrationRouter._kIDChallengeAPI;
	}

	// Token: 0x06002CED RID: 11501 RVA: 0x0004E844 File Offset: 0x0004CA44
	public static SessionAPIsApi GetKIDSessionAPI()
	{
		if (KIDIntegrationRouter._kIDSessionAPI == null || KIDIntegrationRouter._kIDSessionAPI.Configuration.AccessToken != KIDIntegrationRouter._kIDConfiguration.AccessToken)
		{
			KIDIntegrationRouter._kIDSessionAPI = new SessionAPIsApi(KIDIntegrationRouter.GetKIDConfig());
		}
		return KIDIntegrationRouter._kIDSessionAPI;
	}

	// Token: 0x06002CEE RID: 11502 RVA: 0x00125CC8 File Offset: 0x00123EC8
	public static Task<bool> RefreshToken()
	{
		KIDIntegrationRouter.<RefreshToken>d__9 <RefreshToken>d__;
		<RefreshToken>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<RefreshToken>d__.<>1__state = -1;
		<RefreshToken>d__.<>t__builder.Start<KIDIntegrationRouter.<RefreshToken>d__9>(ref <RefreshToken>d__);
		return <RefreshToken>d__.<>t__builder.Task;
	}

	// Token: 0x06002CEF RID: 11503 RVA: 0x00125D04 File Offset: 0x00123F04
	public static Task<bool> AuthenticateKIDClient()
	{
		KIDIntegrationRouter.<AuthenticateKIDClient>d__10 <AuthenticateKIDClient>d__;
		<AuthenticateKIDClient>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<AuthenticateKIDClient>d__.<>1__state = -1;
		<AuthenticateKIDClient>d__.<>t__builder.Start<KIDIntegrationRouter.<AuthenticateKIDClient>d__10>(ref <AuthenticateKIDClient>d__);
		return <AuthenticateKIDClient>d__.<>t__builder.Task;
	}

	// Token: 0x06002CF0 RID: 11504 RVA: 0x00125D40 File Offset: 0x00123F40
	public static CheckAgeGateRequest CreateAgeGateRequest(DateTime dateOfBirth)
	{
		if (dateOfBirth == default(DateTime))
		{
			dateOfBirth = DateTime.Today;
		}
		return new CheckAgeGateRequest(KIDManager.Location, dateOfBirth);
	}

	// Token: 0x06002CF1 RID: 11505 RVA: 0x00125D70 File Offset: 0x00123F70
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

	// Token: 0x06002CF2 RID: 11506 RVA: 0x0004E881 File Offset: 0x0004CA81
	private static AuthAPIsApi GetKIDAuthAPI()
	{
		if (KIDIntegrationRouter._kIDAuthAPI == null || KIDIntegrationRouter._kIDAuthAPI.Configuration.AccessToken != KIDIntegrationRouter._kIDConfiguration.AccessToken)
		{
			KIDIntegrationRouter._kIDAuthAPI = new AuthAPIsApi(KIDIntegrationRouter.GetKIDConfig());
		}
		return KIDIntegrationRouter._kIDAuthAPI;
	}

	// Token: 0x06002CF3 RID: 11507 RVA: 0x0004E8BE File Offset: 0x0004CABE
	private static CreateClientAuthTokenRequest GetClientAuthToken()
	{
		return new CreateClientAuthTokenRequest(PlayFabAuthenticator.instance.GetPlayFabPlayerId());
	}

	// Token: 0x0400324E RID: 12878
	private const string DATE_FORMAT = "yyyy-MM-dd";

	// Token: 0x0400324F RID: 12879
	private static Configuration _kIDConfiguration;

	// Token: 0x04003250 RID: 12880
	private static AuthAPIsApi _kIDAuthAPI;

	// Token: 0x04003251 RID: 12881
	private static AgeGateAPIsApi _kIDAgeGateAPI;

	// Token: 0x04003252 RID: 12882
	private static ChallengeAPIsApi _kIDChallengeAPI;

	// Token: 0x04003253 RID: 12883
	private static SessionAPIsApi _kIDSessionAPI;
}
