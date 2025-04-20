using System;
using UnityEngine;
using UnityEngine.UI;

namespace OculusSampleFramework
{
	// Token: 0x02000A90 RID: 2704
	public class DistanceGrabberSample : MonoBehaviour
	{
		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06004387 RID: 17287 RVA: 0x0005C068 File Offset: 0x0005A268
		// (set) Token: 0x06004388 RID: 17288 RVA: 0x00178618 File Offset: 0x00176818
		public bool UseSpherecast
		{
			get
			{
				return this.useSpherecast;
			}
			set
			{
				this.useSpherecast = value;
				for (int i = 0; i < this.m_grabbers.Length; i++)
				{
					this.m_grabbers[i].UseSpherecast = this.useSpherecast;
				}
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06004389 RID: 17289 RVA: 0x0005C070 File Offset: 0x0005A270
		// (set) Token: 0x0600438A RID: 17290 RVA: 0x00178654 File Offset: 0x00176854
		public bool AllowGrabThroughWalls
		{
			get
			{
				return this.allowGrabThroughWalls;
			}
			set
			{
				this.allowGrabThroughWalls = value;
				for (int i = 0; i < this.m_grabbers.Length; i++)
				{
					this.m_grabbers[i].m_preventGrabThroughWalls = !this.allowGrabThroughWalls;
				}
			}
		}

		// Token: 0x0600438B RID: 17291 RVA: 0x00178694 File Offset: 0x00176894
		private void Start()
		{
			DebugUIBuilder.instance.AddLabel("Distance Grab Sample", 0);
			DebugUIBuilder.instance.AddToggle("Use Spherecasting", new DebugUIBuilder.OnToggleValueChange(this.ToggleSphereCasting), this.useSpherecast, 0);
			DebugUIBuilder.instance.AddToggle("Grab Through Walls", new DebugUIBuilder.OnToggleValueChange(this.ToggleGrabThroughWalls), this.allowGrabThroughWalls, 0);
			DebugUIBuilder.instance.Show();
			float displayFrequency = OVRManager.display.displayFrequency;
			if (displayFrequency > 0.1f)
			{
				Debug.Log("Setting Time.fixedDeltaTime to: " + (1f / displayFrequency).ToString());
				Time.fixedDeltaTime = 1f / displayFrequency;
			}
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x0005C078 File Offset: 0x0005A278
		public void ToggleSphereCasting(Toggle t)
		{
			this.UseSpherecast = !this.UseSpherecast;
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x0005C089 File Offset: 0x0005A289
		public void ToggleGrabThroughWalls(Toggle t)
		{
			this.AllowGrabThroughWalls = !this.AllowGrabThroughWalls;
		}

		// Token: 0x0400443C RID: 17468
		private bool useSpherecast;

		// Token: 0x0400443D RID: 17469
		private bool allowGrabThroughWalls;

		// Token: 0x0400443E RID: 17470
		[SerializeField]
		private DistanceGrabber[] m_grabbers;
	}
}
