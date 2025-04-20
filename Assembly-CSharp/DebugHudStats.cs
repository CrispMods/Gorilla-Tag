using System;
using System.Collections.Generic;
using System.Text;
using GorillaLocomotion;
using GorillaNetworking;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x0200085C RID: 2140
public class DebugHudStats : MonoBehaviour
{
	// Token: 0x17000565 RID: 1381
	// (get) Token: 0x06003442 RID: 13378 RVA: 0x00052585 File Offset: 0x00050785
	public static DebugHudStats Instance
	{
		get
		{
			return DebugHudStats._instance;
		}
	}

	// Token: 0x06003443 RID: 13379 RVA: 0x0005258C File Offset: 0x0005078C
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

	// Token: 0x06003444 RID: 13380 RVA: 0x000525C7 File Offset: 0x000507C7
	private void OnDestroy()
	{
		if (DebugHudStats._instance == this)
		{
			DebugHudStats._instance = null;
		}
	}

	// Token: 0x06003445 RID: 13381 RVA: 0x0013D858 File Offset: 0x0013BA58
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

	// Token: 0x06003446 RID: 13382 RVA: 0x000525DC File Offset: 0x000507DC
	private void OnPlayerSwam(float distance, float speed)
	{
		if (distance > 0.005f)
		{
			this.distanceSwam += distance;
		}
	}

	// Token: 0x06003447 RID: 13383 RVA: 0x000525F4 File Offset: 0x000507F4
	private void OnPlayerMoved(float distance, float speed)
	{
		if (distance > 0.005f)
		{
			this.distanceMoved += distance;
		}
	}

	// Token: 0x06003448 RID: 13384 RVA: 0x0005260C File Offset: 0x0005080C
	private void OnEnable()
	{
		Application.logMessageReceived += this.LogMessageReceived;
	}

	// Token: 0x06003449 RID: 13385 RVA: 0x0005261F File Offset: 0x0005081F
	private void OnDisable()
	{
		Application.logMessageReceived -= this.LogMessageReceived;
	}

	// Token: 0x0600344A RID: 13386 RVA: 0x00052632 File Offset: 0x00050832
	private void LogMessageReceived(string condition, string stackTrace, LogType type)
	{
		this.logMessages.Add(this.getColorStringFromLogType(type) + condition + "</color>");
		if (this.logMessages.Count > 6)
		{
			this.logMessages.RemoveAt(0);
		}
	}

	// Token: 0x0600344B RID: 13387 RVA: 0x0005266B File Offset: 0x0005086B
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

	// Token: 0x0600344C RID: 13388 RVA: 0x0013DD20 File Offset: 0x0013BF20
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

	// Token: 0x0400377D RID: 14205
	private const int FPS_THRESHOLD = 89;

	// Token: 0x0400377E RID: 14206
	private static DebugHudStats _instance;

	// Token: 0x0400377F RID: 14207
	[SerializeField]
	public TMP_Text text;

	// Token: 0x04003780 RID: 14208
	[SerializeField]
	private TMP_Text fpsWarning;

	// Token: 0x04003781 RID: 14209
	[SerializeField]
	private float delayUpdateRate = 0.25f;

	// Token: 0x04003782 RID: 14210
	private float updateTimer;

	// Token: 0x04003783 RID: 14211
	public float sessionAnytrackingLost;

	// Token: 0x04003784 RID: 14212
	public float last30SecondsTrackingLost;

	// Token: 0x04003785 RID: 14213
	private float firstAwake;

	// Token: 0x04003786 RID: 14214
	private bool leftHandTracked;

	// Token: 0x04003787 RID: 14215
	private bool rightHandTracked;

	// Token: 0x04003788 RID: 14216
	private StringBuilder builder;

	// Token: 0x04003789 RID: 14217
	private Vector3 averagedVelocity;

	// Token: 0x0400378A RID: 14218
	private Vector3 groundVelocity;

	// Token: 0x0400378B RID: 14219
	private Vector3 centerHeadPos;

	// Token: 0x0400378C RID: 14220
	private float distanceMoved;

	// Token: 0x0400378D RID: 14221
	private float distanceSwam;

	// Token: 0x0400378E RID: 14222
	private List<string> logMessages = new List<string>();

	// Token: 0x0400378F RID: 14223
	private bool buttonDown;

	// Token: 0x04003790 RID: 14224
	private bool showLog;

	// Token: 0x04003791 RID: 14225
	private int lowFps;

	// Token: 0x04003792 RID: 14226
	private string zones;

	// Token: 0x04003793 RID: 14227
	private GroupJoinZoneAB lastGroupJoinZone;

	// Token: 0x04003794 RID: 14228
	private DebugHudStats.State currentState = DebugHudStats.State.Active;

	// Token: 0x0200085D RID: 2141
	private enum State
	{
		// Token: 0x04003796 RID: 14230
		Inactive,
		// Token: 0x04003797 RID: 14231
		Active,
		// Token: 0x04003798 RID: 14232
		ShowLog,
		// Token: 0x04003799 RID: 14233
		ShowStats
	}
}
