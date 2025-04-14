using System;
using System.Collections.Generic;
using System.Text;
using GorillaLocomotion;
using GorillaNetworking;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000842 RID: 2114
public class DebugHudStats : MonoBehaviour
{
	// Token: 0x17000557 RID: 1367
	// (get) Token: 0x06003387 RID: 13191 RVA: 0x000F5E93 File Offset: 0x000F4093
	public static DebugHudStats Instance
	{
		get
		{
			return DebugHudStats._instance;
		}
	}

	// Token: 0x06003388 RID: 13192 RVA: 0x000F5E9A File Offset: 0x000F409A
	private void Awake()
	{
		if (DebugHudStats._instance != null && DebugHudStats._instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			DebugHudStats._instance = this;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x06003389 RID: 13193 RVA: 0x000F5ED5 File Offset: 0x000F40D5
	private void OnDestroy()
	{
		if (DebugHudStats._instance == this)
		{
			DebugHudStats._instance = null;
		}
	}

	// Token: 0x0600338A RID: 13194 RVA: 0x000F5EEC File Offset: 0x000F40EC
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

	// Token: 0x0600338B RID: 13195 RVA: 0x000F63B1 File Offset: 0x000F45B1
	private void OnPlayerSwam(float distance, float speed)
	{
		if (distance > 0.005f)
		{
			this.distanceSwam += distance;
		}
	}

	// Token: 0x0600338C RID: 13196 RVA: 0x000F63C9 File Offset: 0x000F45C9
	private void OnPlayerMoved(float distance, float speed)
	{
		if (distance > 0.005f)
		{
			this.distanceMoved += distance;
		}
	}

	// Token: 0x0600338D RID: 13197 RVA: 0x000F63E1 File Offset: 0x000F45E1
	private void OnEnable()
	{
		Application.logMessageReceived += this.LogMessageReceived;
	}

	// Token: 0x0600338E RID: 13198 RVA: 0x000F63F4 File Offset: 0x000F45F4
	private void OnDisable()
	{
		Application.logMessageReceived -= this.LogMessageReceived;
	}

	// Token: 0x0600338F RID: 13199 RVA: 0x000F6407 File Offset: 0x000F4607
	private void LogMessageReceived(string condition, string stackTrace, LogType type)
	{
		this.logMessages.Add(this.getColorStringFromLogType(type) + condition + "</color>");
		if (this.logMessages.Count > 6)
		{
			this.logMessages.RemoveAt(0);
		}
	}

	// Token: 0x06003390 RID: 13200 RVA: 0x000F6440 File Offset: 0x000F4640
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

	// Token: 0x06003391 RID: 13201 RVA: 0x000F6470 File Offset: 0x000F4670
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

	// Token: 0x040036C1 RID: 14017
	private const int FPS_THRESHOLD = 89;

	// Token: 0x040036C2 RID: 14018
	private static DebugHudStats _instance;

	// Token: 0x040036C3 RID: 14019
	[SerializeField]
	private TMP_Text text;

	// Token: 0x040036C4 RID: 14020
	[SerializeField]
	private TMP_Text fpsWarning;

	// Token: 0x040036C5 RID: 14021
	[SerializeField]
	private float delayUpdateRate = 0.25f;

	// Token: 0x040036C6 RID: 14022
	private float updateTimer;

	// Token: 0x040036C7 RID: 14023
	public float sessionAnytrackingLost;

	// Token: 0x040036C8 RID: 14024
	public float last30SecondsTrackingLost;

	// Token: 0x040036C9 RID: 14025
	private float firstAwake;

	// Token: 0x040036CA RID: 14026
	private bool leftHandTracked;

	// Token: 0x040036CB RID: 14027
	private bool rightHandTracked;

	// Token: 0x040036CC RID: 14028
	private StringBuilder builder;

	// Token: 0x040036CD RID: 14029
	private Vector3 averagedVelocity;

	// Token: 0x040036CE RID: 14030
	private Vector3 groundVelocity;

	// Token: 0x040036CF RID: 14031
	private Vector3 centerHeadPos;

	// Token: 0x040036D0 RID: 14032
	private float distanceMoved;

	// Token: 0x040036D1 RID: 14033
	private float distanceSwam;

	// Token: 0x040036D2 RID: 14034
	private List<string> logMessages = new List<string>();

	// Token: 0x040036D3 RID: 14035
	private bool buttonDown;

	// Token: 0x040036D4 RID: 14036
	private bool showLog;

	// Token: 0x040036D5 RID: 14037
	private int lowFps;

	// Token: 0x040036D6 RID: 14038
	private string zones;

	// Token: 0x040036D7 RID: 14039
	private GroupJoinZoneAB lastGroupJoinZone;

	// Token: 0x040036D8 RID: 14040
	private DebugHudStats.State currentState = DebugHudStats.State.Active;

	// Token: 0x02000843 RID: 2115
	private enum State
	{
		// Token: 0x040036DA RID: 14042
		Inactive,
		// Token: 0x040036DB RID: 14043
		Active,
		// Token: 0x040036DC RID: 14044
		ShowLog,
		// Token: 0x040036DD RID: 14045
		ShowStats
	}
}
