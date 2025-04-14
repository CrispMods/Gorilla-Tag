using System;
using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;

namespace GorillaTag.Dev.Benchmarks
{
	// Token: 0x02000BE7 RID: 3047
	public class VisualBenchmark : MonoBehaviour
	{
		// Token: 0x06004CAA RID: 19626 RVA: 0x00174BE8 File Offset: 0x00172DE8
		protected void Awake()
		{
			Application.quitting += delegate()
			{
				VisualBenchmark.isQuitting = true;
			};
			List<ProfilerRecorderHandle> list = new List<ProfilerRecorderHandle>(5500);
			ProfilerRecorderHandle.GetAvailable(list);
			Debug.Log(string.Format("poop Available stats: {0}", list.Count), this);
			List<VisualBenchmark.StatInfo> list2 = new List<VisualBenchmark.StatInfo>(600);
			foreach (ProfilerRecorderHandle handle in list)
			{
				ProfilerRecorderDescription description = ProfilerRecorderHandle.GetDescription(handle);
				if (description.Category == ProfilerCategory.Render)
				{
					list2.Add(new VisualBenchmark.StatInfo
					{
						name = description.Name,
						unit = description.UnitType
					});
				}
			}
			this.availableRenderStats = list2.ToArray();
			Debug.Log(string.Format("poop availableRenderStats: {0}", list2.Count), this);
			List<Transform> list3 = new List<Transform>(this.benchmarkLocations.Length);
			foreach (Transform transform in this.benchmarkLocations)
			{
				if (transform != null)
				{
					list3.Add(transform);
				}
			}
			this.benchmarkLocations = list3.ToArray();
		}

		// Token: 0x06004CAB RID: 19627 RVA: 0x00174D4C File Offset: 0x00172F4C
		protected void OnEnable()
		{
			this.renderStatsRecorders = new ProfilerRecorder[this.availableRenderStats.Length];
			for (int i = 0; i < this.availableRenderStats.Length; i++)
			{
				this.renderStatsRecorders[i] = ProfilerRecorder.StartNew(ProfilerCategory.Render, this.availableRenderStats[i].name, 1, ProfilerRecorderOptions.Default);
			}
			this.state = VisualBenchmark.EState.Setup;
		}

		// Token: 0x06004CAC RID: 19628 RVA: 0x00174DB0 File Offset: 0x00172FB0
		protected void OnDisable()
		{
			foreach (ProfilerRecorder profilerRecorder in this.renderStatsRecorders)
			{
				profilerRecorder.Dispose();
			}
		}

		// Token: 0x06004CAD RID: 19629 RVA: 0x00174DE4 File Offset: 0x00172FE4
		protected void LateUpdate()
		{
			if (VisualBenchmark.isQuitting)
			{
				return;
			}
			switch (this.state)
			{
			case VisualBenchmark.EState.Setup:
				Debug.Log("poop start");
				this.sb.Clear();
				this.currentLocationIndex = 0;
				this.lastTime = Time.realtimeSinceStartup;
				this.state = VisualBenchmark.EState.WaitingBeforeCollectingGarbage;
				return;
			case VisualBenchmark.EState.WaitingBeforeCollectingGarbage:
				Debug.Log("poop wait 1");
				if (Time.realtimeSinceStartup - this.lastTime >= this.collectGarbageDelay)
				{
					this.lastTime = Time.time;
					GC.Collect();
					this.state = VisualBenchmark.EState.WaitingBeforeRecordingStats;
					return;
				}
				break;
			case VisualBenchmark.EState.WaitingBeforeRecordingStats:
				Debug.Log("poop wait 2");
				if (Time.time - this.lastTime >= this.recordStatsDelay)
				{
					this.lastTime = Time.time;
					this.RecordLocationStats(this.benchmarkLocations[this.currentLocationIndex]);
					if (this.currentLocationIndex < this.benchmarkLocations.Length - 1)
					{
						this.currentLocationIndex++;
						this.state = VisualBenchmark.EState.WaitingBeforeCollectingGarbage;
						return;
					}
					this.state = VisualBenchmark.EState.TearDown;
					return;
				}
				break;
			case VisualBenchmark.EState.TearDown:
				Debug.Log("poop teardown");
				Debug.Log(this.sb.ToString());
				this.state = VisualBenchmark.EState.Setup;
				if (this.sb.Length > this.sb.Capacity)
				{
					Debug.Log("Capacity exceeded on string builder, increase string builder's capacity. " + string.Format("capacity={0}, length={1}", this.sb.Capacity, this.sb.Length), this);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06004CAE RID: 19630 RVA: 0x00174F64 File Offset: 0x00173164
		private void RecordLocationStats(Transform xform)
		{
			this.sb.Append("Location: ");
			this.sb.Append(xform.name);
			this.sb.Append("\n");
			this.sb.Append("pos=");
			this.sb.Append(xform.position.ToString("F3"));
			this.sb.Append(" rot=");
			this.sb.Append(xform.rotation.ToString("F3"));
			this.sb.Append(" scale=");
			this.sb.Append(xform.lossyScale.ToString("F3"));
			this.sb.Append("\n");
			for (int i = 0; i < this.renderStatsRecorders.Length; i++)
			{
				this.sb.Append(this.availableRenderStats[i].name);
				this.sb.Append(": ");
				ProfilerMarkerDataUnit unit = this.availableRenderStats[i].unit;
				if (unit != ProfilerMarkerDataUnit.TimeNanoseconds)
				{
					if (unit == ProfilerMarkerDataUnit.Bytes)
					{
						this.sb.Append((double)this.renderStatsRecorders[i].LastValue / 1024.0);
						this.sb.Append("kb");
					}
					else
					{
						this.sb.Append(this.renderStatsRecorders[i].LastValue);
						this.sb.Append(' ');
						this.sb.Append(this.availableRenderStats[i].unit.ToString());
					}
				}
				else
				{
					this.sb.Append((double)this.renderStatsRecorders[i].LastValue / 1000000.0);
					this.sb.Append("ms");
				}
				this.sb.Append('\n');
			}
		}

		// Token: 0x04004E6F RID: 20079
		[Tooltip("the camera will be moved and rotated to these spots and record stats.")]
		public Transform[] benchmarkLocations;

		// Token: 0x04004E70 RID: 20080
		[Tooltip("How long to wait before calling GC.Collect() to clean up memory.")]
		public float collectGarbageDelay = 2f;

		// Token: 0x04004E71 RID: 20081
		[Tooltip("How long to wait before recording stats after the camera was moved to a new location.\nThis + collectGarbageDelay is the total time spent at each location.")]
		private float recordStatsDelay = 2f;

		// Token: 0x04004E72 RID: 20082
		[Tooltip("The camera to use for profiling. If null, a new camera will be created.")]
		private Camera cam;

		// Token: 0x04004E73 RID: 20083
		private VisualBenchmark.StatInfo[] availableRenderStats;

		// Token: 0x04004E74 RID: 20084
		private ProfilerRecorder[] renderStatsRecorders;

		// Token: 0x04004E75 RID: 20085
		private static bool isQuitting = true;

		// Token: 0x04004E76 RID: 20086
		private int currentLocationIndex;

		// Token: 0x04004E77 RID: 20087
		private VisualBenchmark.EState state = VisualBenchmark.EState.WaitingBeforeCollectingGarbage;

		// Token: 0x04004E78 RID: 20088
		private float lastTime;

		// Token: 0x04004E79 RID: 20089
		private readonly StringBuilder sb = new StringBuilder(1024);

		// Token: 0x02000BE8 RID: 3048
		private struct StatInfo
		{
			// Token: 0x04004E7A RID: 20090
			public string name;

			// Token: 0x04004E7B RID: 20091
			public ProfilerMarkerDataUnit unit;
		}

		// Token: 0x02000BE9 RID: 3049
		private enum EState
		{
			// Token: 0x04004E7D RID: 20093
			Setup,
			// Token: 0x04004E7E RID: 20094
			WaitingBeforeCollectingGarbage,
			// Token: 0x04004E7F RID: 20095
			WaitingBeforeRecordingStats,
			// Token: 0x04004E80 RID: 20096
			TearDown
		}
	}
}
