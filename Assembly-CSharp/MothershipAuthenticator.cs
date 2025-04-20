using System;
using GorillaExtensions;
using Steamworks;
using UnityEngine;

// Token: 0x020007BF RID: 1983
public class MothershipAuthenticator : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x060030FF RID: 12543 RVA: 0x0013294C File Offset: 0x00130B4C
	public void Awake()
	{
		if (MothershipAuthenticator.Instance == null)
		{
			MothershipAuthenticator.Instance = this;
		}
		else if (MothershipAuthenticator.Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
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

	// Token: 0x06003100 RID: 12544 RVA: 0x0005067F File Offset: 0x0004E87F
	public void BeginLoginFlow()
	{
		Debug.Log("making login call");
		this.LogInWithSteam();
	}

	// Token: 0x06003101 RID: 12545 RVA: 0x00050691 File Offset: 0x0004E891
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

	// Token: 0x06003102 RID: 12546 RVA: 0x000506BD File Offset: 0x0004E8BD
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

	// Token: 0x06003103 RID: 12547 RVA: 0x00032C89 File Offset: 0x00030E89
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06003104 RID: 12548 RVA: 0x00032C92 File Offset: 0x00030E92
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06003105 RID: 12549 RVA: 0x000506DD File Offset: 0x0004E8DD
	public void SliceUpdate()
	{
		if (MothershipClientApiUnity.IsEnabled())
		{
			MothershipClientApiUnity.Tick(Time.deltaTime);
		}
	}

	// Token: 0x06003107 RID: 12551 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x040034FA RID: 13562
	public static volatile MothershipAuthenticator Instance;

	// Token: 0x040034FB RID: 13563
	public MetaAuthenticator MetaAuthenticator;

	// Token: 0x040034FC RID: 13564
	public SteamAuthenticator SteamAuthenticator;

	// Token: 0x040034FD RID: 13565
	public string TestNickname = "Foo";

	// Token: 0x040034FE RID: 13566
	public string TestAccountId = "Bar";

	// Token: 0x040034FF RID: 13567
	public int MaxMetaLoginAttempts = 5;

	// Token: 0x04003500 RID: 13568
	public Action OnLoginSuccess;

	// Token: 0x04003501 RID: 13569
	public Action OnLoginFailure;

	// Token: 0x04003502 RID: 13570
	public Action<int> OnLoginAttemptFailure;
}
