using System;
using UnityEngine;
using UnityEngine.UI;

namespace OculusSampleFramework
{
	// Token: 0x02000A63 RID: 2659
	public class DistanceGrabberSample : MonoBehaviour
	{
		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06004242 RID: 16962 RVA: 0x00138E37 File Offset: 0x00137037
		// (set) Token: 0x06004243 RID: 16963 RVA: 0x00138E40 File Offset: 0x00137040
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

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06004244 RID: 16964 RVA: 0x00138E7A File Offset: 0x0013707A
		// (set) Token: 0x06004245 RID: 16965 RVA: 0x00138E84 File Offset: 0x00137084
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

		// Token: 0x06004246 RID: 16966 RVA: 0x00138EC4 File Offset: 0x001370C4
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

		// Token: 0x06004247 RID: 16967 RVA: 0x00138F6F File Offset: 0x0013716F
		public void ToggleSphereCasting(Toggle t)
		{
			this.UseSpherecast = !this.UseSpherecast;
		}

		// Token: 0x06004248 RID: 16968 RVA: 0x00138F80 File Offset: 0x00137180
		public void ToggleGrabThroughWalls(Toggle t)
		{
			this.AllowGrabThroughWalls = !this.AllowGrabThroughWalls;
		}

		// Token: 0x04004342 RID: 17218
		private bool useSpherecast;

		// Token: 0x04004343 RID: 17219
		private bool allowGrabThroughWalls;

		// Token: 0x04004344 RID: 17220
		[SerializeField]
		private DistanceGrabber[] m_grabbers;
	}
}
