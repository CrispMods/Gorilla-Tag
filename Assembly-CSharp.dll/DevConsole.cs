using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200018C RID: 396
public class DevConsole : MonoBehaviour, IDebugObject
{
	// Token: 0x170000FB RID: 251
	// (get) Token: 0x060009CD RID: 2509 RVA: 0x00035E92 File Offset: 0x00034092
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

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x060009CE RID: 2510 RVA: 0x00035EB0 File Offset: 0x000340B0
	public static List<DevConsole.LogEntry> logEntries
	{
		get
		{
			return DevConsole.instance._logEntries;
		}
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x0009443C File Offset: 0x0009263C
	public void OnDestroyDebugObject()
	{
		Debug.Log("Destroying debug instances now");
		foreach (DevConsoleInstance devConsoleInstance in this.instances)
		{
			UnityEngine.Object.DestroyImmediate(devConsoleInstance.gameObject);
		}
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x00032A08 File Offset: 0x00030C08
	private void OnEnable()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000BE0 RID: 3040
	private static DevConsole _instance;

	// Token: 0x04000BE1 RID: 3041
	[SerializeField]
	private AudioClip errorSound;

	// Token: 0x04000BE2 RID: 3042
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04000BE3 RID: 3043
	[SerializeField]
	private float maxHeight;

	// Token: 0x04000BE4 RID: 3044
	public static readonly string[] tracebackScrubbing = new string[]
	{
		"ExitGames.Client.Photon",
		"Photon.Realtime.LoadBalancingClient",
		"Photon.Pun.PhotonHandler"
	};

	// Token: 0x04000BE5 RID: 3045
	private const int kLogEntriesCapacityIncrementAmount = 1024;

	// Token: 0x04000BE6 RID: 3046
	[SerializeReference]
	[SerializeField]
	private readonly List<DevConsole.LogEntry> _logEntries = new List<DevConsole.LogEntry>(1024);

	// Token: 0x04000BE7 RID: 3047
	public int targetLogIndex = -1;

	// Token: 0x04000BE8 RID: 3048
	public int currentLogIndex;

	// Token: 0x04000BE9 RID: 3049
	public bool isMuted;

	// Token: 0x04000BEA RID: 3050
	public float currentZoomLevel = 1f;

	// Token: 0x04000BEB RID: 3051
	public List<GameObject> disableWhileActive;

	// Token: 0x04000BEC RID: 3052
	public List<GameObject> enableWhileActive;

	// Token: 0x04000BED RID: 3053
	public int expandAmount = 20;

	// Token: 0x04000BEE RID: 3054
	public int expandedMessageIndex = -1;

	// Token: 0x04000BEF RID: 3055
	public bool canExpand = true;

	// Token: 0x04000BF0 RID: 3056
	public List<DevConsole.DisplayedLogLine> logLines = new List<DevConsole.DisplayedLogLine>();

	// Token: 0x04000BF1 RID: 3057
	public float lineStartHeight;

	// Token: 0x04000BF2 RID: 3058
	public float textStartHeight;

	// Token: 0x04000BF3 RID: 3059
	public float lineStartTextWidth;

	// Token: 0x04000BF4 RID: 3060
	public double textScale = 0.5;

	// Token: 0x04000BF5 RID: 3061
	public List<DevConsoleInstance> instances;

	// Token: 0x0200018D RID: 397
	[Serializable]
	public class LogEntry
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060009D3 RID: 2515 RVA: 0x00035EE1 File Offset: 0x000340E1
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

		// Token: 0x060009D4 RID: 2516 RVA: 0x00094504 File Offset: 0x00092704
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

		// Token: 0x04000BF6 RID: 3062
		private static int TotalIndex;

		// Token: 0x04000BF7 RID: 3063
		[SerializeReference]
		[SerializeField]
		public readonly string _Message;

		// Token: 0x04000BF8 RID: 3064
		[SerializeField]
		[SerializeReference]
		public readonly LogType Type;

		// Token: 0x04000BF9 RID: 3065
		public readonly string Trace;

		// Token: 0x04000BFA RID: 3066
		public bool forwarded;

		// Token: 0x04000BFB RID: 3067
		public int repeatCount = 1;

		// Token: 0x04000BFC RID: 3068
		public bool filtered;

		// Token: 0x04000BFD RID: 3069
		public int index;
	}

	// Token: 0x0200018F RID: 399
	[Serializable]
	public class DisplayedLogLine
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x00035F1C File Offset: 0x0003411C
		// (set) Token: 0x060009D8 RID: 2520 RVA: 0x00035F24 File Offset: 0x00034124
		public Type data { get; set; }

		// Token: 0x060009D9 RID: 2521 RVA: 0x000945B0 File Offset: 0x000927B0
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

		// Token: 0x04000BFF RID: 3071
		public GorillaDevButton[] buttons;

		// Token: 0x04000C00 RID: 3072
		public Text lineText;

		// Token: 0x04000C01 RID: 3073
		public RectTransform transform;

		// Token: 0x04000C02 RID: 3074
		public int targetMessage;

		// Token: 0x04000C03 RID: 3075
		public GorillaDevButton maximizeButton;

		// Token: 0x04000C04 RID: 3076
		public GorillaDevButton forwardButton;

		// Token: 0x04000C05 RID: 3077
		public SpriteRenderer backdrop;

		// Token: 0x04000C06 RID: 3078
		private bool expanded;

		// Token: 0x04000C07 RID: 3079
		public DevInspector inspector;
	}

	// Token: 0x02000190 RID: 400
	[Serializable]
	public class MessagePayload
	{
		// Token: 0x060009DA RID: 2522 RVA: 0x0009462C File Offset: 0x0009282C
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

		// Token: 0x04000C09 RID: 3081
		public DevConsole.MessagePayload.Block[] blocks;

		// Token: 0x02000191 RID: 401
		[Serializable]
		public class Block
		{
			// Token: 0x060009DC RID: 2524 RVA: 0x00035F2D File Offset: 0x0003412D
			public Block(string markdownText)
			{
				this.text = new DevConsole.MessagePayload.TextBlock
				{
					text = markdownText,
					type = "mrkdwn"
				};
				this.type = "section";
			}

			// Token: 0x04000C0A RID: 3082
			public string type;

			// Token: 0x04000C0B RID: 3083
			public DevConsole.MessagePayload.TextBlock text;
		}

		// Token: 0x02000192 RID: 402
		[Serializable]
		public class TextBlock
		{
			// Token: 0x04000C0C RID: 3084
			public string type;

			// Token: 0x04000C0D RID: 3085
			public string text;
		}
	}
}
