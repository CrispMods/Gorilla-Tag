using System;
using GorillaExtensions;
using Steamworks;
using UnityEngine;

// Token: 0x020007A7 RID: 1959
public class MothershipAuthenticator : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x0600304D RID: 12365 RVA: 0x000E9510 File Offset: 0x000E7710
	public void Awake()
	{
		if (MothershipAuthenticator.Instance == null)
		{
			MothershipAuthenticator.Instance = this;
		}
		else if (MothershipAuthenticator.Instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		if (!MothershipClientApiUnity.IsEnabled())
		{
			Debug.Log("Mothership is not enabled.");
			return;
		}
		if (MothershipAuthenticator.Instance.SteamAuthenticator == null)
		{
			MothershipAuthenticator.Instance.SteamAuthenticator = MothershipAuthenticator.Instance.gameObject.GetOrAddComponent<SteamAuthenticator>();
		}
		MothershipClientApiUnity.SetAuthRefreshedCallback(delegate(string id)
		{
			this.BeginLoginFlow();
		});
	}

	// Token: 0x0600304E RID: 12366 RVA: 0x000E95A4 File Offset: 0x000E77A4
	public void BeginLoginFlow()
	{
		Debug.Log("making login call");
		this.LogInWithSteam();
	}

	// Token: 0x0600304F RID: 12367 RVA: 0x000E95B6 File Offset: 0x000E77B6
	private void LogInWithInsecure()
	{
		MothershipClientApiUnity.LogInWithInsecure1(this.TestNickname, this.TestAccountId, delegate(LoginResponse LoginResponse)
		{
			Debug.Log("Logged in with Mothership Id " + LoginResponse.MothershipPlayerId);
			Action onLoginSuccess = this.OnLoginSuccess;
			if (onLoginSuccess == null)
			{
				return;
			}
			onLoginSuccess();
		}, delegate(MothershipError MothershipError, int errorCode)
		{
			Debug.Log(string.Format("Failed to log in, error {0} {1}", MothershipError.Details, errorCode));
			Action onLoginFailure = this.OnLoginFailure;
			if (onLoginFailure == null)
			{
				return;
			}
			onLoginFailure();
		});
	}

	// Token: 0x06003050 RID: 12368 RVA: 0x000E95E2 File Offset: 0x000E77E2
	private void LogInWithSteam()
	{
		MothershipClientApiUnity.StartLoginWithSteam(delegate(PlayerSteamBeginLoginResponse resp)
		{
			string nonce = resp.Nonce;
			SteamAuthTicket ticketHandle = HAuthTicket.Invalid;
			Action<LoginResponse> <>9__4;
			Action<MothershipError, int> <>9__5;
			ticketHandle = this.SteamAuthenticator.GetAuthTicketForWebApi(nonce, delegate(string ticket)
			{
				string nonce = nonce;
				Action<LoginResponse> successAction;
				if ((successAction = <>9__4) == null)
				{
					successAction = (<>9__4 = delegate(LoginResponse successResp)
					{
						ticketHandle.Dispose();
						Debug.Log("Logged in to Mothership with Steam");
						Action onLoginSuccess = this.OnLoginSuccess;
						if (onLoginSuccess == null)
						{
							return;
						}
						onLoginSuccess();
					});
				}
				Action<MothershipError, int> errorAction;
				if ((errorAction = <>9__5) == null)
				{
					errorAction = (<>9__5 = delegate(MothershipError MothershipError, int errorCode)
					{
						ticketHandle.Dispose();
						Debug.Log(string.Format("Couldn't log into Mothership with Steam {0} {1}", MothershipError.Details, errorCode));
						Action onLoginFailure = this.OnLoginFailure;
						if (onLoginFailure == null)
						{
							return;
						}
						onLoginFailure();
					});
				}
				MothershipClientApiUnity.CompleteLoginWithSteam(nonce, ticket, successAction, errorAction);
			}, delegate(EResult error)
			{
				Debug.Log(string.Format("Couldn't get an auth ticket for logging into Mothership with Steam {0}", error));
				Action onLoginFailure = this.OnLoginFailure;
				if (onLoginFailure == null)
				{
					return;
				}
				onLoginFailure();
			});
		}, delegate(MothershipError MothershipError, int errorCode)
		{
			Debug.Log(string.Format("Couldn't start Mothership auth for Steam {0} {1}", MothershipError.Details, errorCode));
			Action onLoginFailure = this.OnLoginFailure;
			if (onLoginFailure == null)
			{
				return;
			}
			onLoginFailure();
		});
	}

	// Token: 0x06003051 RID: 12369 RVA: 0x000158F9 File Offset: 0x00013AF9
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06003052 RID: 12370 RVA: 0x00015902 File Offset: 0x00013B02
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06003053 RID: 12371 RVA: 0x000E9602 File Offset: 0x000E7802
	public void SliceUpdate()
	{
		if (MothershipClientApiUnity.IsEnabled())
		{
			MothershipClientApiUnity.Tick(Time.deltaTime);
		}
	}

	// Token: 0x06003055 RID: 12373 RVA: 0x0000F974 File Offset: 0x0000DB74
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04003450 RID: 13392
	public static volatile MothershipAuthenticator Instance;

	// Token: 0x04003451 RID: 13393
	public MetaAuthenticator MetaAuthenticator;

	// Token: 0x04003452 RID: 13394
	public SteamAuthenticator SteamAuthenticator;

	// Token: 0x04003453 RID: 13395
	public string TestNickname = "Foo";

	// Token: 0x04003454 RID: 13396
	public string TestAccountId = "Bar";

	// Token: 0x04003455 RID: 13397
	public int MaxMetaLoginAttempts = 5;

	// Token: 0x04003456 RID: 13398
	public Action OnLoginSuccess;

	// Token: 0x04003457 RID: 13399
	public Action OnLoginFailure;

	// Token: 0x04003458 RID: 13400
	public Action<int> OnLoginAttemptFailure;
}
