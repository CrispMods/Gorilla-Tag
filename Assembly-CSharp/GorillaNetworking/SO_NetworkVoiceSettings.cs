using System;
using ExitGames.Client.Photon;
using Photon.Voice;
using Photon.Voice.Unity;
using POpusCodec.Enums;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AB0 RID: 2736
	[CreateAssetMenu(fileName = "VoiceSettings", menuName = "Gorilla Tag/VoiceSettings")]
	public class SO_NetworkVoiceSettings : ScriptableObject
	{
		// Token: 0x04004540 RID: 17728
		[Header("Voice settings")]
		public bool AutoConnectAndJoin = true;

		// Token: 0x04004541 RID: 17729
		public bool AutoLeaveAndDisconnect = true;

		// Token: 0x04004542 RID: 17730
		public bool WorkInOfflineMode = true;

		// Token: 0x04004543 RID: 17731
		public DebugLevel LogLevel = DebugLevel.ERROR;

		// Token: 0x04004544 RID: 17732
		public DebugLevel GlobalRecordersLogLevel = DebugLevel.INFO;

		// Token: 0x04004545 RID: 17733
		public DebugLevel GlobalSpeakersLogLevel = DebugLevel.INFO;

		// Token: 0x04004546 RID: 17734
		public bool CreateSpeakerIfNotFound;

		// Token: 0x04004547 RID: 17735
		public int UpdateInterval = 50;

		// Token: 0x04004548 RID: 17736
		public bool SupportLogger;

		// Token: 0x04004549 RID: 17737
		public int BackgroundTimeout = 60000;

		// Token: 0x0400454A RID: 17738
		[Header("Recorder Settings")]
		public bool RecordOnlyWhenEnabled;

		// Token: 0x0400454B RID: 17739
		public bool RecordOnlyWhenJoined = true;

		// Token: 0x0400454C RID: 17740
		public bool StopRecordingWhenPaused;

		// Token: 0x0400454D RID: 17741
		public bool TransmitEnabled = true;

		// Token: 0x0400454E RID: 17742
		public bool AutoStart = true;

		// Token: 0x0400454F RID: 17743
		public bool Encrypt;

		// Token: 0x04004550 RID: 17744
		public byte InterestGroup;

		// Token: 0x04004551 RID: 17745
		public bool DebugEcho;

		// Token: 0x04004552 RID: 17746
		public bool ReliableMode;

		// Token: 0x04004553 RID: 17747
		[Header("Recorder Codec Parameters")]
		public OpusCodec.FrameDuration FrameDuration = OpusCodec.FrameDuration.Frame60ms;

		// Token: 0x04004554 RID: 17748
		public SamplingRate SamplingRate = SamplingRate.Sampling16000;

		// Token: 0x04004555 RID: 17749
		[Range(6000f, 510000f)]
		public int Bitrate = 20000;

		// Token: 0x04004556 RID: 17750
		[Header("Recorder Audio Source Settings")]
		public Recorder.InputSourceType InputSourceType;

		// Token: 0x04004557 RID: 17751
		public Recorder.MicType MicrophoneType;

		// Token: 0x04004558 RID: 17752
		public bool UseFallback = true;

		// Token: 0x04004559 RID: 17753
		public bool Detect = true;

		// Token: 0x0400455A RID: 17754
		[Range(0f, 1f)]
		public float Threshold = 0.07f;

		// Token: 0x0400455B RID: 17755
		public int Delay = 500;
	}
}
