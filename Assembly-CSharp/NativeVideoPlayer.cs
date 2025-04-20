using System;
using UnityEngine;

// Token: 0x020002F9 RID: 761
public static class NativeVideoPlayer
{
	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06001244 RID: 4676 RVA: 0x000AF86C File Offset: 0x000ADA6C
	private static IntPtr VideoPlayerClass
	{
		get
		{
			if (NativeVideoPlayer._VideoPlayerClass == null)
			{
				try
				{
					IntPtr intPtr = AndroidJNI.FindClass("com/oculus/videoplayer/NativeVideoPlayer");
					if (intPtr != IntPtr.Zero)
					{
						NativeVideoPlayer._VideoPlayerClass = new IntPtr?(AndroidJNI.NewGlobalRef(intPtr));
						AndroidJNI.DeleteLocalRef(intPtr);
					}
					else
					{
						Debug.LogError("Failed to find NativeVideoPlayer class");
						NativeVideoPlayer._VideoPlayerClass = new IntPtr?(IntPtr.Zero);
					}
				}
				catch (Exception exception)
				{
					Debug.LogError("Failed to find NativeVideoPlayer class");
					Debug.LogException(exception);
					NativeVideoPlayer._VideoPlayerClass = new IntPtr?(IntPtr.Zero);
				}
			}
			return NativeVideoPlayer._VideoPlayerClass.GetValueOrDefault();
		}
	}

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06001245 RID: 4677 RVA: 0x000AF90C File Offset: 0x000ADB0C
	private static IntPtr Activity
	{
		get
		{
			if (NativeVideoPlayer._Activity == null)
			{
				try
				{
					IntPtr intPtr = AndroidJNI.FindClass("com/unity3d/player/UnityPlayer");
					IntPtr staticFieldID = AndroidJNI.GetStaticFieldID(intPtr, "currentActivity", "Landroid/app/Activity;");
					IntPtr staticObjectField = AndroidJNI.GetStaticObjectField(intPtr, staticFieldID);
					NativeVideoPlayer._Activity = new IntPtr?(AndroidJNI.NewGlobalRef(staticObjectField));
					AndroidJNI.DeleteLocalRef(staticObjectField);
					AndroidJNI.DeleteLocalRef(intPtr);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					NativeVideoPlayer._Activity = new IntPtr?(IntPtr.Zero);
				}
			}
			return NativeVideoPlayer._Activity.GetValueOrDefault();
		}
	}

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06001246 RID: 4678 RVA: 0x00030498 File Offset: 0x0002E698
	public static bool IsAvailable
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06001247 RID: 4679 RVA: 0x0003C6E8 File Offset: 0x0003A8E8
	public static bool IsPlaying
	{
		get
		{
			if (NativeVideoPlayer.getIsPlayingMethodId == IntPtr.Zero)
			{
				NativeVideoPlayer.getIsPlayingMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "getIsPlaying", "()Z");
			}
			return AndroidJNI.CallStaticBooleanMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.getIsPlayingMethodId, NativeVideoPlayer.EmptyParams);
		}
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06001248 RID: 4680 RVA: 0x0003C728 File Offset: 0x0003A928
	public static NativeVideoPlayer.PlabackState CurrentPlaybackState
	{
		get
		{
			if (NativeVideoPlayer.getCurrentPlaybackStateMethodId == IntPtr.Zero)
			{
				NativeVideoPlayer.getCurrentPlaybackStateMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "getCurrentPlaybackState", "()I");
			}
			return (NativeVideoPlayer.PlabackState)AndroidJNI.CallStaticIntMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.getCurrentPlaybackStateMethodId, NativeVideoPlayer.EmptyParams);
		}
	}

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x06001249 RID: 4681 RVA: 0x0003C768 File Offset: 0x0003A968
	public static long Duration
	{
		get
		{
			if (NativeVideoPlayer.getDurationMethodId == IntPtr.Zero)
			{
				NativeVideoPlayer.getDurationMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "getDuration", "()J");
			}
			return AndroidJNI.CallStaticLongMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.getDurationMethodId, NativeVideoPlayer.EmptyParams);
		}
	}

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x0600124A RID: 4682 RVA: 0x0003C7A8 File Offset: 0x0003A9A8
	public static NativeVideoPlayer.StereoMode VideoStereoMode
	{
		get
		{
			if (NativeVideoPlayer.getStereoModeMethodId == IntPtr.Zero)
			{
				NativeVideoPlayer.getStereoModeMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "getStereoMode", "()I");
			}
			return (NativeVideoPlayer.StereoMode)AndroidJNI.CallStaticIntMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.getStereoModeMethodId, NativeVideoPlayer.EmptyParams);
		}
	}

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x0600124B RID: 4683 RVA: 0x0003C7E8 File Offset: 0x0003A9E8
	public static int VideoWidth
	{
		get
		{
			if (NativeVideoPlayer.getWidthMethodId == IntPtr.Zero)
			{
				NativeVideoPlayer.getWidthMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "getWidth", "()I");
			}
			return AndroidJNI.CallStaticIntMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.getWidthMethodId, NativeVideoPlayer.EmptyParams);
		}
	}

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x0600124C RID: 4684 RVA: 0x0003C828 File Offset: 0x0003AA28
	public static int VideoHeight
	{
		get
		{
			if (NativeVideoPlayer.getHeightMethodId == IntPtr.Zero)
			{
				NativeVideoPlayer.getHeightMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "getHeight", "()I");
			}
			return AndroidJNI.CallStaticIntMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.getHeightMethodId, NativeVideoPlayer.EmptyParams);
		}
	}

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x0600124D RID: 4685 RVA: 0x0003C868 File Offset: 0x0003AA68
	// (set) Token: 0x0600124E RID: 4686 RVA: 0x000AF998 File Offset: 0x000ADB98
	public static long PlaybackPosition
	{
		get
		{
			if (NativeVideoPlayer.getPlaybackPositionMethodId == IntPtr.Zero)
			{
				NativeVideoPlayer.getPlaybackPositionMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "getPlaybackPosition", "()J");
			}
			return AndroidJNI.CallStaticLongMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.getPlaybackPositionMethodId, NativeVideoPlayer.EmptyParams);
		}
		set
		{
			if (NativeVideoPlayer.setPlaybackPositionMethodId == IntPtr.Zero)
			{
				NativeVideoPlayer.setPlaybackPositionMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "setPlaybackPosition", "(J)V");
				NativeVideoPlayer.setPlaybackPositionParams = new jvalue[1];
			}
			NativeVideoPlayer.setPlaybackPositionParams[0].j = value;
			AndroidJNI.CallStaticVoidMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.setPlaybackPositionMethodId, NativeVideoPlayer.setPlaybackPositionParams);
		}
	}

	// Token: 0x0600124F RID: 4687 RVA: 0x000AFA00 File Offset: 0x000ADC00
	public static void PlayVideo(string path, string drmLicenseUrl, IntPtr surfaceObj)
	{
		if (NativeVideoPlayer.playVideoMethodId == IntPtr.Zero)
		{
			NativeVideoPlayer.playVideoMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "playVideo", "(Landroid/content/Context;Ljava/lang/String;Ljava/lang/String;Landroid/view/Surface;)V");
			NativeVideoPlayer.playVideoParams = new jvalue[4];
		}
		IntPtr intPtr = AndroidJNI.NewStringUTF(path);
		IntPtr intPtr2 = AndroidJNI.NewStringUTF(drmLicenseUrl);
		NativeVideoPlayer.playVideoParams[0].l = NativeVideoPlayer.Activity;
		NativeVideoPlayer.playVideoParams[1].l = intPtr;
		NativeVideoPlayer.playVideoParams[2].l = intPtr2;
		NativeVideoPlayer.playVideoParams[3].l = surfaceObj;
		AndroidJNI.CallStaticVoidMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.playVideoMethodId, NativeVideoPlayer.playVideoParams);
		AndroidJNI.DeleteLocalRef(intPtr);
		AndroidJNI.DeleteLocalRef(intPtr2);
	}

	// Token: 0x06001250 RID: 4688 RVA: 0x0003C8A8 File Offset: 0x0003AAA8
	public static void Stop()
	{
		if (NativeVideoPlayer.stopMethodId == IntPtr.Zero)
		{
			NativeVideoPlayer.stopMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "stop", "()V");
		}
		AndroidJNI.CallStaticVoidMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.stopMethodId, NativeVideoPlayer.EmptyParams);
	}

	// Token: 0x06001251 RID: 4689 RVA: 0x0003C8E8 File Offset: 0x0003AAE8
	public static void Play()
	{
		if (NativeVideoPlayer.resumeMethodId == IntPtr.Zero)
		{
			NativeVideoPlayer.resumeMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "resume", "()V");
		}
		AndroidJNI.CallStaticVoidMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.resumeMethodId, NativeVideoPlayer.EmptyParams);
	}

	// Token: 0x06001252 RID: 4690 RVA: 0x0003C928 File Offset: 0x0003AB28
	public static void Pause()
	{
		if (NativeVideoPlayer.pauseMethodId == IntPtr.Zero)
		{
			NativeVideoPlayer.pauseMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "pause", "()V");
		}
		AndroidJNI.CallStaticVoidMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.pauseMethodId, NativeVideoPlayer.EmptyParams);
	}

	// Token: 0x06001253 RID: 4691 RVA: 0x000AFAB8 File Offset: 0x000ADCB8
	public static void SetPlaybackSpeed(float speed)
	{
		if (NativeVideoPlayer.setPlaybackSpeedMethodId == IntPtr.Zero)
		{
			NativeVideoPlayer.setPlaybackSpeedMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "setPlaybackSpeed", "(F)V");
			NativeVideoPlayer.setPlaybackSpeedParams = new jvalue[1];
		}
		NativeVideoPlayer.setPlaybackSpeedParams[0].f = speed;
		AndroidJNI.CallStaticVoidMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.setPlaybackSpeedMethodId, NativeVideoPlayer.setPlaybackSpeedParams);
	}

	// Token: 0x06001254 RID: 4692 RVA: 0x000AFB20 File Offset: 0x000ADD20
	public static void SetLooping(bool looping)
	{
		if (NativeVideoPlayer.setLoopingMethodId == IntPtr.Zero)
		{
			NativeVideoPlayer.setLoopingMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "setLooping", "(Z)V");
			NativeVideoPlayer.setLoopingParams = new jvalue[1];
		}
		NativeVideoPlayer.setLoopingParams[0].z = looping;
		AndroidJNI.CallStaticVoidMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.setLoopingMethodId, NativeVideoPlayer.setLoopingParams);
	}

	// Token: 0x06001255 RID: 4693 RVA: 0x000AFB88 File Offset: 0x000ADD88
	public static void SetListenerRotation(Quaternion rotation)
	{
		if (NativeVideoPlayer.setListenerRotationQuaternionMethodId == IntPtr.Zero)
		{
			NativeVideoPlayer.setListenerRotationQuaternionMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "setListenerRotationQuaternion", "(FFFF)V");
			NativeVideoPlayer.setListenerRotationQuaternionParams = new jvalue[4];
		}
		NativeVideoPlayer.setListenerRotationQuaternionParams[0].f = rotation.x;
		NativeVideoPlayer.setListenerRotationQuaternionParams[1].f = rotation.y;
		NativeVideoPlayer.setListenerRotationQuaternionParams[2].f = rotation.z;
		NativeVideoPlayer.setListenerRotationQuaternionParams[3].f = rotation.w;
		AndroidJNI.CallStaticVoidMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.setListenerRotationQuaternionMethodId, NativeVideoPlayer.setListenerRotationQuaternionParams);
	}

	// Token: 0x04001411 RID: 5137
	private static IntPtr? _Activity;

	// Token: 0x04001412 RID: 5138
	private static IntPtr? _VideoPlayerClass;

	// Token: 0x04001413 RID: 5139
	private static readonly jvalue[] EmptyParams = new jvalue[0];

	// Token: 0x04001414 RID: 5140
	private static IntPtr getIsPlayingMethodId;

	// Token: 0x04001415 RID: 5141
	private static IntPtr getCurrentPlaybackStateMethodId;

	// Token: 0x04001416 RID: 5142
	private static IntPtr getDurationMethodId;

	// Token: 0x04001417 RID: 5143
	private static IntPtr getStereoModeMethodId;

	// Token: 0x04001418 RID: 5144
	private static IntPtr getWidthMethodId;

	// Token: 0x04001419 RID: 5145
	private static IntPtr getHeightMethodId;

	// Token: 0x0400141A RID: 5146
	private static IntPtr getPlaybackPositionMethodId;

	// Token: 0x0400141B RID: 5147
	private static IntPtr setPlaybackPositionMethodId;

	// Token: 0x0400141C RID: 5148
	private static jvalue[] setPlaybackPositionParams;

	// Token: 0x0400141D RID: 5149
	private static IntPtr playVideoMethodId;

	// Token: 0x0400141E RID: 5150
	private static jvalue[] playVideoParams;

	// Token: 0x0400141F RID: 5151
	private static IntPtr stopMethodId;

	// Token: 0x04001420 RID: 5152
	private static IntPtr resumeMethodId;

	// Token: 0x04001421 RID: 5153
	private static IntPtr pauseMethodId;

	// Token: 0x04001422 RID: 5154
	private static IntPtr setPlaybackSpeedMethodId;

	// Token: 0x04001423 RID: 5155
	private static jvalue[] setPlaybackSpeedParams;

	// Token: 0x04001424 RID: 5156
	private static IntPtr setLoopingMethodId;

	// Token: 0x04001425 RID: 5157
	private static jvalue[] setLoopingParams;

	// Token: 0x04001426 RID: 5158
	private static IntPtr setListenerRotationQuaternionMethodId;

	// Token: 0x04001427 RID: 5159
	private static jvalue[] setListenerRotationQuaternionParams;

	// Token: 0x020002FA RID: 762
	public enum PlabackState
	{
		// Token: 0x04001429 RID: 5161
		Idle = 1,
		// Token: 0x0400142A RID: 5162
		Preparing,
		// Token: 0x0400142B RID: 5163
		Buffering,
		// Token: 0x0400142C RID: 5164
		Ready,
		// Token: 0x0400142D RID: 5165
		Ended
	}

	// Token: 0x020002FB RID: 763
	public enum StereoMode
	{
		// Token: 0x0400142F RID: 5167
		Unknown = -1,
		// Token: 0x04001430 RID: 5168
		Mono,
		// Token: 0x04001431 RID: 5169
		TopBottom,
		// Token: 0x04001432 RID: 5170
		LeftRight,
		// Token: 0x04001433 RID: 5171
		Mesh
	}
}
