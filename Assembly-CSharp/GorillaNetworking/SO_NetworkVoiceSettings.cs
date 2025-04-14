using System;
using ExitGames.Client.Photon;
using Photon.Voice;
using Photon.Voice.Unity;
using POpusCodec.Enums;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000A83 RID: 2691
	[CreateAssetMenu(fileName = "VoiceSettings", menuName = "Gorilla Tag/VoiceSettings")]
	public class SO_NetworkVoiceSettings : ScriptableObject
	{
		// Token: 0x04004446 RID: 17478
		[Header("Voice settings")]
		public bool AutoConnectAndJoin = true;

		// Token: 0x04004447 RID: 17479
		public bool AutoLeaveAndDisconnect = true;

		// Token: 0x04004448 RID: 17480
		public bool WorkInOfflineMode = true;

		// Token: 0x04004449 RID: 17481
		public DebugLevel LogLevel = DebugLevel.ERROR;

		// Token: 0x0400444A RID: 17482
		public DebugLevel GlobalRecordersLogLevel = DebugLevel.INFO;

		// Token: 0x0400444B RID: 17483
		public DebugLevel GlobalSpeakersLogLevel = DebugLevel.INFO;

		// Token: 0x0400444C RID: 17484
		public bool CreateSpeakerIfNotFound;

		// Token: 0x0400444D RID: 17485
		public int UpdateInterval = 50;

		// Token: 0x0400444E RID: 17486
		public bool SupportLogger;

		// Token: 0x0400444F RID: 17487
		public int BackgroundTimeout = 60000;

		// Token: 0x04004450 RID: 17488
		[Header("Recorder Settings")]
		public bool RecordOnlyWhenEnabled;

		// Token: 0x04004451 RID: 17489
		public bool RecordOnlyWhenJoined = true;

		// Token: 0x04004452 RID: 17490
		public bool StopRecordingWhenPaused;

		// Token: 0x04004453 RID: 17491
		public bool TransmitEnabled = true;

		// Token: 0x04004454 RID: 17492
		public bool AutoStart = true;

		// Token: 0x04004455 RID: 17493
		public bool Encrypt;

		// Token: 0x04004456 RID: 17494
		public byte InterestGroup;

		// Token: 0x04004457 RID: 17495
		public bool DebugEcho;

		// Token: 0x04004458 RID: 17496
		public bool ReliableMode;

		// Token: 0x04004459 RID: 17497
		[Header("Recorder Codec Parameters")]
		public OpusCodec.FrameDuration FrameDuration = OpusCodec.FrameDuration.Frame60ms;

		// Token: 0x0400445A RID: 17498
		public SamplingRate SamplingRate = SamplingRate.Sampling16000;

		// Token: 0x0400445B RID: 17499
		[Range(6000f, 510000f)]
		public int Bitrate = 20000;

		// Token: 0x0400445C RID: 17500
		[Header("Recorder Audio Source Settings")]
		public Recorder.InputSourceType InputSourceType;

		// Token: 0x0400445D RID: 17501
		public Recorder.MicType MicrophoneType;

		// Token: 0x0400445E RID: 17502
		public bool UseFallback = true;

		// Token: 0x0400445F RID: 17503
		public bool Detect = true;

		// Token: 0x04004460 RID: 17504
		[Range(0f, 1f)]
		public float Threshold = 0.07f;

		// Token: 0x04004461 RID: 17505
		public int Delay = 500;
	}
}
