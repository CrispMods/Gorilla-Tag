using System;
using System.Text;
using AOT;
using Steamworks;
using UnityEngine;

// Token: 0x020007E1 RID: 2017
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x060031EE RID: 12782 RVA: 0x000F025E File Offset: 0x000EE45E
	protected static SteamManager Instance
	{
		get
		{
			if (SteamManager.s_instance == null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return SteamManager.s_instance;
		}
	}

	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x060031EF RID: 12783 RVA: 0x000F0282 File Offset: 0x000EE482
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x060031F0 RID: 12784 RVA: 0x000F028E File Offset: 0x000EE48E
	[MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
	protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x060031F1 RID: 12785 RVA: 0x000F0296 File Offset: 0x000EE496
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void InitOnPlayMode()
	{
		SteamManager.s_EverInitialized = false;
		SteamManager.s_instance = null;
	}

	// Token: 0x060031F2 RID: 12786 RVA: 0x000F02A4 File Offset: 0x000EE4A4
	protected virtual void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
			{
				Debug.Log("[Steamworks.NET] Shutting down because RestartAppIfNecessary returned true. Steam will restart the application.");
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			string str = "[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n";
			DllNotFoundException ex2 = ex;
			Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null), this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInitialized = true;
	}

	// Token: 0x060031F3 RID: 12787 RVA: 0x000F038C File Offset: 0x000EE58C
	protected virtual void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x060031F4 RID: 12788 RVA: 0x000F03DA File Offset: 0x000EE5DA
	protected virtual void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x060031F5 RID: 12789 RVA: 0x000F03FE File Offset: 0x000EE5FE
	protected virtual void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x04003580 RID: 13696
	protected static bool s_EverInitialized;

	// Token: 0x04003581 RID: 13697
	protected static SteamManager s_instance;

	// Token: 0x04003582 RID: 13698
	protected bool m_bInitialized;

	// Token: 0x04003583 RID: 13699
	protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
