using System;
using ExitGames.Client.Photon;
using Photon.Voice;
using Photon.Voice.Unity;
using POpusCodec.Enums;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000A86 RID: 2694
	[CreateAssetMenu(fileName = "VoiceSettings", menuName = "Gorilla Tag/VoiceSettings")]
	public class SO_NetworkVoiceSettings : ScriptableObject
	{
		// Token: 0x04004458 RID: 17496
		[Header("Voice settings")]
		public bool AutoConnectAndJoin = true;

		// Token: 0x04004459 RID: 17497
		public bool AutoLeaveAndDisconnect = true;

		// Token: 0x0400445A RID: 17498
		public bool WorkInOfflineMode = true;

		// Token: 0x0400445B RID: 17499
		public DebugLevel LogLevel = DebugLevel.ERROR;

		// Token: 0x0400445C RID: 17500
		public DebugLevel GlobalRecordersLogLevel = DebugLevel.INFO;

		// Token: 0x0400445D RID: 17501
		public DebugLevel GlobalSpeakersLogLevel = DebugLevel.INFO;

		// Token: 0x0400445E RID: 17502
		public bool CreateSpeakerIfNotFound;

		// Token: 0x0400445F RID: 17503
		public int UpdateInterval = 50;

		// Token: 0x04004460 RID: 17504
		public bool SupportLogger;

		// Token: 0x04004461 RID: 17505
		public int BackgroundTimeout = 60000;

		// Token: 0x04004462 RID: 17506
		[Header("Recorder Settings")]
		public bool RecordOnlyWhenEnabled;

		// Token: 0x04004463 RID: 17507
		public bool RecordOnlyWhenJoined = true;

		// Token: 0x04004464 RID: 17508
		public bool StopRecordingWhenPaused;

		// Token: 0x04004465 RID: 17509
		public bool TransmitEnabled = true;

		// Token: 0x04004466 RID: 17510
		public bool AutoStart = true;

		// Token: 0x04004467 RID: 17511
		public bool Encrypt;

		// Token: 0x04004468 RID: 17512
		public byte InterestGroup;

		// Token: 0x04004469 RID: 17513
		public bool DebugEcho;

		// Token: 0x0400446A RID: 17514
		public bool ReliableMode;

		// Token: 0x0400446B RID: 17515
		[Header("Recorder Codec Parameters")]
		public OpusCodec.FrameDuration FrameDuration = OpusCodec.FrameDuration.Frame60ms;

		// Token: 0x0400446C RID: 17516
		public SamplingRate SamplingRate = SamplingRate.Sampling16000;

		// Token: 0x0400446D RID: 17517
		[Range(6000f, 510000f)]
		public int Bitrate = 20000;

		// Token: 0x0400446E RID: 17518
		[Header("Recorder Audio Source Settings")]
		public Recorder.InputSourceType InputSourceType;

		// Token: 0x0400446F RID: 17519
		public Recorder.MicType MicrophoneType;

		// Token: 0x04004470 RID: 17520
		public bool UseFallback = true;

		// Token: 0x04004471 RID: 17521
		public bool Detect = true;

		// Token: 0x04004472 RID: 17522
		[Range(0f, 1f)]
		public float Threshold = 0.07f;

		// Token: 0x04004473 RID: 17523
		public int Delay = 500;
	}
}
