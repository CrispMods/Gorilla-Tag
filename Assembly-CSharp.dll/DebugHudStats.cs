using System;
using System.Collections.Generic;
using System.Text;
using GorillaLocomotion;
using GorillaNetworking;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000845 RID: 2117
public class DebugHudStats : MonoBehaviour
{
	// Token: 0x17000558 RID: 1368
	// (get) Token: 0x06003393 RID: 13203 RVA: 0x00051177 File Offset: 0x0004F377
	public static DebugHudStats Instance
	{
		get
		{
			return DebugHudStats._instance;
		}
	}

	// Token: 0x06003394 RID: 13204 RVA: 0x0005117E File Offset: 0x0004F37E
	private void Awake()
	{
		if (DebugHudStats._instance != null && DebugHudStats._instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			DebugHudStats._instance = this;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x06003395 RID: 13205 RVA: 0x000511B9 File Offset: 0x0004F3B9
	private void OnDestroy()
	{
		if (DebugHudStats._instance == this)
		{
			DebugHudStats._instance = null;
		}
	}

	// Token: 0x06003396 RID: 13206 RVA: 0x00138300 File Offset: 0x00136500
	private void Update()
	{
		bool flag = ControllerInputPoller.SecondaryButtonPress(XRNode.LeftHand);
		if (flag != this.buttonDown)
		{
			this.buttonDown = flag;
			if (!this.buttonDown)
			{
				switch (this.currentState)
				{
				case DebugHudStats.State.ShowStats:
					PlayerGameEvents.OnPlayerMoved += this.OnPlayerMoved;
					PlayerGameEvents.OnPlayerSwam += this.OnPlayerSwam;
					break;
				}
				switch (this.currentState)
				{
				case DebugHudStats.State.Inactive:
					this.currentState = DebugHudStats.State.Active;
					this.text.gameObject.SetActive(true);
					break;
				case DebugHudStats.State.Active:
					this.currentState = DebugHudStats.State.ShowLog;
					break;
				case DebugHudStats.State.ShowLog:
					this.currentState = DebugHudStats.State.ShowStats;
					this.distanceMoved = (this.distanceSwam = 0f);
					PlayerGameEvents.OnPlayerMoved += this.OnPlayerMoved;
					PlayerGameEvents.OnPlayerSwam += this.OnPlayerSwam;
					break;
				case DebugHudStats.State.ShowStats:
					this.currentState = DebugHudStats.State.Inactive;
					this.text.gameObject.SetActive(false);
					break;
				}
			}
		}
		if (this.firstAwake == 0f)
		{
			this.firstAwake = Time.time;
		}
		if (this.updateTimer < this.delayUpdateRate)
		{
			this.updateTimer += Time.deltaTime;
			return;
		}
		int num = Mathf.RoundToInt(1f / Time.smoothDeltaTime);
		if (num < 89)
		{
			this.lowFps++;
		}
		else
		{
			this.lowFps = 0;
		}
		this.fpsWarning.gameObject.SetActive(this.lowFps > 5 && this.currentState == DebugHudStats.State.Inactive);
		if (this.currentState != DebugHudStats.State.Inactive)
		{
			this.builder.Clear();
			this.builder.Append("v: ");
			this.builder.Append(GorillaComputer.instance.version);
			this.builder.Append(":");
			this.builder.Append(GorillaComputer.instance.buildCode);
			num = Mathf.Min(num, 90);
			this.builder.Append((num < 89) ? " - <color=\"red\">" : " - <color=\"white\">");
			this.builder.Append(num);
			this.builder.AppendLine(" fps</color>");
			if (GorillaComputer.instance != null)
			{
				this.builder.AppendLine(GorillaComputer.instance.GetServerTime().ToString());
			}
			else
			{
				this.builder.AppendLine("Server Time Unavailable");
			}
			GroupJoinZoneAB groupZone = GorillaTagger.Instance.offlineVRRig.zoneEntity.GroupZone;
			if (groupZone != this.lastGroupJoinZone)
			{
				this.zones = groupZone.ToString();
				this.lastGroupJoinZone = groupZone;
			}
			if (NetworkSystem.Instance.IsMasterClient)
			{
				this.builder.Append("H");
			}
			if (NetworkSystem.Instance.InRoom)
			{
				if (NetworkSystem.Instance.SessionIsPrivate)
				{
					this.builder.Append("Pri ");
				}
				else
				{
					this.builder.Append("Pub ");
				}
			}
			else
			{
				this.builder.Append("DC ");
			}
			this.builder.Append("Z: <color=\"orange\">");
			this.builder.Append(this.zones);
			this.builder.AppendLine("</color>");
			if (this.currentState == DebugHudStats.State.ShowStats)
			{
				this.builder.AppendLine();
				Vector3 vector = GTPlayer.Instance.AveragedVelocity;
				Vector3 headCenterPosition = GTPlayer.Instance.HeadCenterPosition;
				float magnitude = vector.magnitude;
				this.groundVelocity = vector;
				this.groundVelocity.y = 0f;
				this.builder.AppendLine(string.Format("v: {0:F1} m/s", magnitude));
				this.builder.AppendLine(string.Format("ground: {0:F1} m/s", this.groundVelocity.magnitude));
				this.builder.AppendLine(string.Format("head: {0:F2}\n", headCenterPosition));
				this.builder.AppendLine(string.Format("odo: {0:F2}m", this.distanceMoved));
				this.builder.AppendLine(string.Format("swam: {0:F2}m", this.distanceSwam));
			}
			else if (this.currentState == DebugHudStats.State.ShowLog)
			{
				this.builder.AppendLine();
				for (int i = 0; i < this.logMessages.Count; i++)
				{
					this.builder.AppendLine(this.logMessages[i]);
				}
			}
			this.text.text = this.builder.ToString();
		}
		this.updateTimer = 0f;
	}

	// Token: 0x06003397 RID: 13207 RVA: 0x000511CE File Offset: 0x0004F3CE
	private void OnPlayerSwam(float distance, float speed)
	{
		if (distance > 0.005f)
		{
			this.distanceSwam += distance;
		}
	}

	// Token: 0x06003398 RID: 13208 RVA: 0x000511E6 File Offset: 0x0004F3E6
	private void OnPlayerMoved(float distance, float speed)
	{
		if (distance > 0.005f)
		{
			this.distanceMoved += distance;
		}
	}

	// Token: 0x06003399 RID: 13209 RVA: 0x000511FE File Offset: 0x0004F3FE
	private void OnEnable()
	{
		Application.logMessageReceived += this.LogMessageReceived;
	}

	// Token: 0x0600339A RID: 13210 RVA: 0x00051211 File Offset: 0x0004F411
	private void OnDisable()
	{
		Application.logMessageReceived -= this.LogMessageReceived;
	}

	// Token: 0x0600339B RID: 13211 RVA: 0x00051224 File Offset: 0x0004F424
	private void LogMessageReceived(string condition, string stackTrace, LogType type)
	{
		this.logMessages.Add(this.getColorStringFromLogType(type) + condition + "</color>");
		if (this.logMessages.Count > 6)
		{
			this.logMessages.RemoveAt(0);
		}
	}

	// Token: 0x0600339C RID: 13212 RVA: 0x0005125D File Offset: 0x0004F45D
	private string getColorStringFromLogType(LogType type)
	{
		switch (type)
		{
		case LogType.Error:
		case LogType.Assert:
		case LogType.Exception:
			return "<color=\"red\">";
		case LogType.Warning:
			return "<color=\"yellow\">";
		}
		return "<color=\"white\">";
	}

	// Token: 0x0600339D RID: 13213 RVA: 0x001387C8 File Offset: 0x001369C8
	private void OnZoneChanged(ZoneData[] zoneData)
	{
		this.zones = string.Empty;
		for (int i = 0; i < zoneData.Length; i++)
		{
			if (zoneData[i].active)
			{
				this.zones = this.zones + zoneData[i].zone.ToString().ToUpper() + "; ";
			}
		}
	}

	// Token: 0x040036D3 RID: 14035
	private const int FPS_THRESHOLD = 89;

	// Token: 0x040036D4 RID: 14036
	private static DebugHudStats _instance;

	// Token: 0x040036D5 RID: 14037
	[SerializeField]
	private TMP_Text text;

	// Token: 0x040036D6 RID: 14038
	[SerializeField]
	private TMP_Text fpsWarning;

	// Token: 0x040036D7 RID: 14039
	[SerializeField]
	private float delayUpdateRate = 0.25f;

	// Token: 0x040036D8 RID: 14040
	private float updateTimer;

	// Token: 0x040036D9 RID: 14041
	public float sessionAnytrackingLost;

	// Token: 0x040036DA RID: 14042
	public float last30SecondsTrackingLost;

	// Token: 0x040036DB RID: 14043
	private float firstAwake;

	// Token: 0x040036DC RID: 14044
	private bool leftHandTracked;

	// Token: 0x040036DD RID: 14045
	private bool rightHandTracked;

	// Token: 0x040036DE RID: 14046
	private StringBuilder builder;

	// Token: 0x040036DF RID: 14047
	private Vector3 averagedVelocity;

	// Token: 0x040036E0 RID: 14048
	private Vector3 groundVelocity;

	// Token: 0x040036E1 RID: 14049
	private Vector3 centerHeadPos;

	// Token: 0x040036E2 RID: 14050
	private float distanceMoved;

	// Token: 0x040036E3 RID: 14051
	private float distanceSwam;

	// Token: 0x040036E4 RID: 14052
	private List<string> logMessages = new List<string>();

	// Token: 0x040036E5 RID: 14053
	private bool buttonDown;

	// Token: 0x040036E6 RID: 14054
	private bool showLog;

	// Token: 0x040036E7 RID: 14055
	private int lowFps;

	// Token: 0x040036E8 RID: 14056
	private string zones;

	// Token: 0x040036E9 RID: 14057
	private GroupJoinZoneAB lastGroupJoinZone;

	// Token: 0x040036EA RID: 14058
	private DebugHudStats.State currentState = DebugHudStats.State.Active;

	// Token: 0x02000846 RID: 2118
	private enum State
	{
		// Token: 0x040036EC RID: 14060
		Inactive,
		// Token: 0x040036ED RID: 14061
		Active,
		// Token: 0x040036EE RID: 14062
		ShowLog,
		// Token: 0x040036EF RID: 14063
		ShowStats
	}
}
