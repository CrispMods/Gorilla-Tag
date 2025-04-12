using System;
using UnityEngine;
using UnityEngine.UI;

namespace OculusSampleFramework
{
	// Token: 0x02000A66 RID: 2662
	public class DistanceGrabberSample : MonoBehaviour
	{
		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x0600424E RID: 16974 RVA: 0x0005A666 File Offset: 0x00058866
		// (set) Token: 0x0600424F RID: 16975 RVA: 0x00171794 File Offset: 0x0016F994
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

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06004250 RID: 16976 RVA: 0x0005A66E File Offset: 0x0005886E
		// (set) Token: 0x06004251 RID: 16977 RVA: 0x001717D0 File Offset: 0x0016F9D0
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

		// Token: 0x06004252 RID: 16978 RVA: 0x00171810 File Offset: 0x0016FA10
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

		// Token: 0x06004253 RID: 16979 RVA: 0x0005A676 File Offset: 0x00058876
		public void ToggleSphereCasting(Toggle t)
		{
			this.UseSpherecast = !this.UseSpherecast;
		}

		// Token: 0x06004254 RID: 16980 RVA: 0x0005A687 File Offset: 0x00058887
		public void ToggleGrabThroughWalls(Toggle t)
		{
			this.AllowGrabThroughWalls = !this.AllowGrabThroughWalls;
		}

		// Token: 0x04004354 RID: 17236
		private bool useSpherecast;

		// Token: 0x04004355 RID: 17237
		private bool allowGrabThroughWalls;

		// Token: 0x04004356 RID: 17238
		[SerializeField]
		private DistanceGrabber[] m_grabbers;
	}
}
