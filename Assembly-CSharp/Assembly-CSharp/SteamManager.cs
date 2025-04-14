using System;
using System.Text;
using AOT;
using Steamworks;
using UnityEngine;

// Token: 0x020007E4 RID: 2020
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x060031FA RID: 12794 RVA: 0x000F0826 File Offset: 0x000EEA26
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

	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x060031FB RID: 12795 RVA: 0x000F084A File Offset: 0x000EEA4A
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x060031FC RID: 12796 RVA: 0x000F0856 File Offset: 0x000EEA56
	[MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
	protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x060031FD RID: 12797 RVA: 0x000F085E File Offset: 0x000EEA5E
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void InitOnPlayMode()
	{
		SteamManager.s_EverInitialized = false;
		SteamManager.s_instance = null;
	}

	// Token: 0x060031FE RID: 12798 RVA: 0x000F086C File Offset: 0x000EEA6C
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

	// Token: 0x060031FF RID: 12799 RVA: 0x000F0954 File Offset: 0x000EEB54
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

	// Token: 0x06003200 RID: 12800 RVA: 0x000F09A2 File Offset: 0x000EEBA2
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

	// Token: 0x06003201 RID: 12801 RVA: 0x000F09C6 File Offset: 0x000EEBC6
	protected virtual void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x04003592 RID: 13714
	protected static bool s_EverInitialized;

	// Token: 0x04003593 RID: 13715
	protected static SteamManager s_instance;

	// Token: 0x04003594 RID: 13716
	protected bool m_bInitialized;

	// Token: 0x04003595 RID: 13717
	protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
