using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000197 RID: 407
public class DevConsole : MonoBehaviour, IDebugObject
{
	// Token: 0x17000102 RID: 258
	// (get) Token: 0x06000A17 RID: 2583 RVA: 0x00037152 File Offset: 0x00035352
	public static DevConsole instance
	{
		get
		{
			if (DevConsole._instance == null)
			{
				DevConsole._instance = UnityEngine.Object.FindObjectOfType<DevConsole>();
			}
			return DevConsole._instance;
		}
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06000A18 RID: 2584 RVA: 0x00037170 File Offset: 0x00035370
	public static List<DevConsole.LogEntry> logEntries
	{
		get
		{
			return DevConsole.instance._logEntries;
		}
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x00096D30 File Offset: 0x00094F30
	public void OnDestroyDebugObject()
	{
		Debug.Log("Destroying debug instances now");
		foreach (DevConsoleInstance devConsoleInstance in this.instances)
		{
			UnityEngine.Object.DestroyImmediate(devConsoleInstance.gameObject);
		}
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x00033C0F File Offset: 0x00031E0F
	private void OnEnable()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000C25 RID: 3109
	private static DevConsole _instance;

	// Token: 0x04000C26 RID: 3110
	[SerializeField]
	private AudioClip errorSound;

	// Token: 0x04000C27 RID: 3111
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04000C28 RID: 3112
	[SerializeField]
	private float maxHeight;

	// Token: 0x04000C29 RID: 3113
	public static readonly string[] tracebackScrubbing = new string[]
	{
		"ExitGames.Client.Photon",
		"Photon.Realtime.LoadBalancingClient",
		"Photon.Pun.PhotonHandler"
	};

	// Token: 0x04000C2A RID: 3114
	private const int kLogEntriesCapacityIncrementAmount = 1024;

	// Token: 0x04000C2B RID: 3115
	[SerializeReference]
	[SerializeField]
	private readonly List<DevConsole.LogEntry> _logEntries = new List<DevConsole.LogEntry>(1024);

	// Token: 0x04000C2C RID: 3116
	public int targetLogIndex = -1;

	// Token: 0x04000C2D RID: 3117
	public int currentLogIndex;

	// Token: 0x04000C2E RID: 3118
	public bool isMuted;

	// Token: 0x04000C2F RID: 3119
	public float currentZoomLevel = 1f;

	// Token: 0x04000C30 RID: 3120
	public List<GameObject> disableWhileActive;

	// Token: 0x04000C31 RID: 3121
	public List<GameObject> enableWhileActive;

	// Token: 0x04000C32 RID: 3122
	public int expandAmount = 20;

	// Token: 0x04000C33 RID: 3123
	public int expandedMessageIndex = -1;

	// Token: 0x04000C34 RID: 3124
	public bool canExpand = true;

	// Token: 0x04000C35 RID: 3125
	public List<DevConsole.DisplayedLogLine> logLines = new List<DevConsole.DisplayedLogLine>();

	// Token: 0x04000C36 RID: 3126
	public float lineStartHeight;

	// Token: 0x04000C37 RID: 3127
	public float textStartHeight;

	// Token: 0x04000C38 RID: 3128
	public float lineStartTextWidth;

	// Token: 0x04000C39 RID: 3129
	public double textScale = 0.5;

	// Token: 0x04000C3A RID: 3130
	public List<DevConsoleInstance> instances;

	// Token: 0x02000198 RID: 408
	[Serializable]
	public class LogEntry
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x000371A1 File Offset: 0x000353A1
		public string Message
		{
			get
			{
				if (this.repeatCount > 1)
				{
					return string.Format("({0}) {1}", this.repeatCount, this._Message);
				}
				return this._Message;
			}
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00096DF8 File Offset: 0x00094FF8
		public LogEntry(string message, LogType type, string trace)
		{
			this._Message = message;
			this.Type = type;
			this.Trace = trace;
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = trace.Split("\n".ToCharArray(), StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				string line = array[i];
				if (!DevConsole.tracebackScrubbing.Any((string scrubString) => line.Contains(scrubString)))
				{
					stringBuilder.AppendLine(line);
				}
			}
			this.Trace = stringBuilder.ToString();
			DevConsole.LogEntry.TotalIndex++;
			this.index = DevConsole.LogEntry.TotalIndex;
		}

		// Token: 0x04000C3B RID: 3131
		private static int TotalIndex;

		// Token: 0x04000C3C RID: 3132
		[SerializeReference]
		[SerializeField]
		public readonly string _Message;

		// Token: 0x04000C3D RID: 3133
		[SerializeField]
		[SerializeReference]
		public readonly LogType Type;

		// Token: 0x04000C3E RID: 3134
		public readonly string Trace;

		// Token: 0x04000C3F RID: 3135
		public bool forwarded;

		// Token: 0x04000C40 RID: 3136
		public int repeatCount = 1;

		// Token: 0x04000C41 RID: 3137
		public bool filtered;

		// Token: 0x04000C42 RID: 3138
		public int index;
	}

	// Token: 0x0200019A RID: 410
	[Serializable]
	public class DisplayedLogLine
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x000371DC File Offset: 0x000353DC
		// (set) Token: 0x06000A22 RID: 2594 RVA: 0x000371E4 File Offset: 0x000353E4
		public Type data { get; set; }

		// Token: 0x06000A23 RID: 2595 RVA: 0x00096EA4 File Offset: 0x000950A4
		public DisplayedLogLine(GameObject obj)
		{
			this.lineText = obj.GetComponentInChildren<Text>();
			this.buttons = obj.GetComponentsInChildren<GorillaDevButton>();
			this.transform = obj.GetComponent<RectTransform>();
			this.backdrop = obj.GetComponentInChildren<SpriteRenderer>();
			foreach (GorillaDevButton gorillaDevButton in this.buttons)
			{
				if (gorillaDevButton.Type == DevButtonType.LineExpand)
				{
					this.maximizeButton = gorillaDevButton;
				}
				if (gorillaDevButton.Type == DevButtonType.LineForward)
				{
					this.forwardButton = gorillaDevButton;
				}
			}
		}

		// Token: 0x04000C44 RID: 3140
		public GorillaDevButton[] buttons;

		// Token: 0x04000C45 RID: 3141
		public Text lineText;

		// Token: 0x04000C46 RID: 3142
		public RectTransform transform;

		// Token: 0x04000C47 RID: 3143
		public int targetMessage;

		// Token: 0x04000C48 RID: 3144
		public GorillaDevButton maximizeButton;

		// Token: 0x04000C49 RID: 3145
		public GorillaDevButton forwardButton;

		// Token: 0x04000C4A RID: 3146
		public SpriteRenderer backdrop;

		// Token: 0x04000C4B RID: 3147
		private bool expanded;

		// Token: 0x04000C4C RID: 3148
		public DevInspector inspector;
	}

	// Token: 0x0200019B RID: 411
	[Serializable]
	public class MessagePayload
	{
		// Token: 0x06000A24 RID: 2596 RVA: 0x00096F20 File Offset: 0x00095120
		public static List<DevConsole.MessagePayload> GeneratePayloads(string username, List<DevConsole.LogEntry> entries)
		{
			List<DevConsole.MessagePayload> list = new List<DevConsole.MessagePayload>();
			List<DevConsole.MessagePayload.Block> list2 = new List<DevConsole.MessagePayload.Block>();
			entries.Sort((DevConsole.LogEntry e1, DevConsole.LogEntry e2) => e1.index.CompareTo(e2.index));
			string text = "";
			text += "```";
			list2.Add(new DevConsole.MessagePayload.Block("User `" + username + "` Forwarded some errors"));
			foreach (DevConsole.LogEntry logEntry in entries)
			{
				string[] array = logEntry.Trace.Split("\n".ToCharArray());
				string text2 = "";
				foreach (string str in array)
				{
					text2 = text2 + "    " + str + "\n";
				}
				string text3 = string.Format("({0}) {1}\n{2}\n", logEntry.Type, logEntry.Message, text2);
				if (text.Length + text3.Length > 3000)
				{
					text += "```";
					list2.Add(new DevConsole.MessagePayload.Block(text));
					list.Add(new DevConsole.MessagePayload
					{
						blocks = list2.ToArray()
					});
					list2 = new List<DevConsole.MessagePayload.Block>();
					text = "```";
				}
				text += string.Format("({0}) {1}\n{2}\n", logEntry.Type, logEntry.Message, text2);
			}
			text += "```";
			list2.Add(new DevConsole.MessagePayload.Block(text));
			list.Add(new DevConsole.MessagePayload
			{
				blocks = list2.ToArray()
			});
			return list;
		}

		// Token: 0x04000C4E RID: 3150
		public DevConsole.MessagePayload.Block[] blocks;

		// Token: 0x0200019C RID: 412
		[Serializable]
		public class Block
		{
			// Token: 0x06000A26 RID: 2598 RVA: 0x000371ED File Offset: 0x000353ED
			public Block(string markdownText)
			{
				this.text = new DevConsole.MessagePayload.TextBlock
				{
					text = markdownText,
					type = "mrkdwn"
				};
				this.type = "section";
			}

			// Token: 0x04000C4F RID: 3151
			public string type;

			// Token: 0x04000C50 RID: 3152
			public DevConsole.MessagePayload.TextBlock text;
		}

		// Token: 0x0200019D RID: 413
		[Serializable]
		public class TextBlock
		{
			// Token: 0x04000C51 RID: 3153
			public string type;

			// Token: 0x04000C52 RID: 3154
			public string text;
		}
	}
}
