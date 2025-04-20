using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GorillaGameModes;
using GT_CustomMapSupportRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x0200078D RID: 1933
public class LuauHud : MonoBehaviour
{
	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x06002F8B RID: 12171 RVA: 0x0004FA84 File Offset: 0x0004DC84
	public static LuauHud Instance
	{
		get
		{
			return LuauHud._instance;
		}
	}

	// Token: 0x06002F8C RID: 12172 RVA: 0x0012C330 File Offset: 0x0012A530
	private void Awake()
	{
		if (LuauHud._instance != null && LuauHud._instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			LuauHud._instance = this;
		}
		this.path = Path.Combine(Application.persistentDataPath, "script.luau");
	}

	// Token: 0x06002F8D RID: 12173 RVA: 0x0004FA8B File Offset: 0x0004DC8B
	private void OnDestroy()
	{
		if (LuauHud._instance == this)
		{
			LuauHud._instance = null;
		}
	}

	// Token: 0x06002F8E RID: 12174 RVA: 0x0012C380 File Offset: 0x0012A580
	private void Start()
	{
		this.useLuauHud = true;
		DebugHudStats instance = DebugHudStats.Instance;
		instance.enabled = false;
		this.debugHud = instance.gameObject;
		this.text = instance.text;
		this.builder = new StringBuilder(50);
	}

	// Token: 0x06002F8F RID: 12175 RVA: 0x0012C3C8 File Offset: 0x0012A5C8
	private void Update()
	{
		MapDescriptor loadedMapDescriptor = CustomMapLoader.LoadedMapDescriptor;
		if (loadedMapDescriptor == null || !loadedMapDescriptor.DevMode)
		{
			if (this.showLog && this.useLuauHud)
			{
				this.showLog = false;
				this.text.gameObject.SetActive(false);
			}
			return;
		}
		GorillaGameManager instance = GorillaGameManager.instance;
		if (instance == null || instance.GameType() != GameModeType.Custom)
		{
			return;
		}
		bool flag = ControllerInputPoller.SecondaryButtonPress(XRNode.LeftHand);
		bool flag2 = ControllerInputPoller.SecondaryButtonPress(XRNode.RightHand);
		if (flag != this.buttonDown && this.useLuauHud)
		{
			this.buttonDown = flag;
			if (!this.buttonDown)
			{
				if (!this.text.gameObject.activeInHierarchy)
				{
					this.text.gameObject.SetActive(true);
					this.showLog = true;
				}
				else
				{
					this.text.gameObject.SetActive(false);
					this.showLog = false;
				}
			}
		}
		if (!flag || !flag2)
		{
			this.resetTimer = Time.time;
		}
		if (Time.time - this.resetTimer > 2f)
		{
			this.LuauLog("Restarting Luau Script");
			if (CustomGameMode.GameModeInitialized)
			{
				CustomGameMode.StopScript();
			}
			if (File.Exists(this.path))
			{
				this.script = File.ReadAllText(this.path);
			}
			if (this.script != "")
			{
				CustomGameMode.LuaScript = this.script;
			}
			CustomGameMode.LuaStart();
			this.resetTimer = Time.time;
		}
		if (this.useLuauHud && this.showLog)
		{
			this.builder.AppendLine();
			for (int i = 0; i < this.luauLogs.Count; i++)
			{
				this.builder.AppendLine(this.luauLogs[i]);
			}
		}
	}

	// Token: 0x06002F90 RID: 12176 RVA: 0x0004FAA0 File Offset: 0x0004DCA0
	public void LuauLog(string log)
	{
		Debug.Log(log);
		this.luauLogs.Add(log);
		if (this.luauLogs.Count > 6)
		{
			this.luauLogs.RemoveAt(0);
		}
	}

	// Token: 0x040033AE RID: 13230
	private bool useLuauHud;

	// Token: 0x040033AF RID: 13231
	private bool buttonDown;

	// Token: 0x040033B0 RID: 13232
	private bool showLog;

	// Token: 0x040033B1 RID: 13233
	private GameObject debugHud;

	// Token: 0x040033B2 RID: 13234
	private TMP_Text text;

	// Token: 0x040033B3 RID: 13235
	private StringBuilder builder;

	// Token: 0x040033B4 RID: 13236
	private float resetTimer;

	// Token: 0x040033B5 RID: 13237
	private string path = "";

	// Token: 0x040033B6 RID: 13238
	private string script = "";

	// Token: 0x040033B7 RID: 13239
	private static LuauHud _instance;

	// Token: 0x040033B8 RID: 13240
	private List<string> luauLogs = new List<string>();
}
