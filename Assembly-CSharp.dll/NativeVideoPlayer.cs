using System;
using UnityEngine;

// Token: 0x020002EE RID: 750
public static class NativeVideoPlayer
{
	// Token: 0x17000207 RID: 519
	// (get) Token: 0x060011FB RID: 4603 RVA: 0x000ACFD4 File Offset: 0x000AB1D4
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

	// Token: 0x17000208 RID: 520
	// (get) Token: 0x060011FC RID: 4604 RVA: 0x000AD074 File Offset: 0x000AB274
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

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x060011FD RID: 4605 RVA: 0x0002F5F0 File Offset: 0x0002D7F0
	public static bool IsAvailable
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x060011FE RID: 4606 RVA: 0x0003B428 File Offset: 0x00039628
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

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x060011FF RID: 4607 RVA: 0x0003B468 File Offset: 0x00039668
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

	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06001200 RID: 4608 RVA: 0x0003B4A8 File Offset: 0x000396A8
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

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06001201 RID: 4609 RVA: 0x0003B4E8 File Offset: 0x000396E8
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

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06001202 RID: 4610 RVA: 0x0003B528 File Offset: 0x00039728
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

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06001203 RID: 4611 RVA: 0x0003B568 File Offset: 0x00039768
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

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06001204 RID: 4612 RVA: 0x0003B5A8 File Offset: 0x000397A8
	// (set) Token: 0x06001205 RID: 4613 RVA: 0x000AD100 File Offset: 0x000AB300
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

	// Token: 0x06001206 RID: 4614 RVA: 0x000AD168 File Offset: 0x000AB368
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

	// Token: 0x06001207 RID: 4615 RVA: 0x0003B5E8 File Offset: 0x000397E8
	public static void Stop()
	{
		if (NativeVideoPlayer.stopMethodId == IntPtr.Zero)
		{
			NativeVideoPlayer.stopMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "stop", "()V");
		}
		AndroidJNI.CallStaticVoidMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.stopMethodId, NativeVideoPlayer.EmptyParams);
	}

	// Token: 0x06001208 RID: 4616 RVA: 0x0003B628 File Offset: 0x00039828
	public static void Play()
	{
		if (NativeVideoPlayer.resumeMethodId == IntPtr.Zero)
		{
			NativeVideoPlayer.resumeMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "resume", "()V");
		}
		AndroidJNI.CallStaticVoidMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.resumeMethodId, NativeVideoPlayer.EmptyParams);
	}

	// Token: 0x06001209 RID: 4617 RVA: 0x0003B668 File Offset: 0x00039868
	public static void Pause()
	{
		if (NativeVideoPlayer.pauseMethodId == IntPtr.Zero)
		{
			NativeVideoPlayer.pauseMethodId = AndroidJNI.GetStaticMethodID(NativeVideoPlayer.VideoPlayerClass, "pause", "()V");
		}
		AndroidJNI.CallStaticVoidMethod(NativeVideoPlayer.VideoPlayerClass, NativeVideoPlayer.pauseMethodId, NativeVideoPlayer.EmptyParams);
	}

	// Token: 0x0600120A RID: 4618 RVA: 0x000AD220 File Offset: 0x000AB420
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

	// Token: 0x0600120B RID: 4619 RVA: 0x000AD288 File Offset: 0x000AB488
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

	// Token: 0x0600120C RID: 4620 RVA: 0x000AD2F0 File Offset: 0x000AB4F0
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

	// Token: 0x040013CA RID: 5066
	private static IntPtr? _Activity;

	// Token: 0x040013CB RID: 5067
	private static IntPtr? _VideoPlayerClass;

	// Token: 0x040013CC RID: 5068
	private static readonly jvalue[] EmptyParams = new jvalue[0];

	// Token: 0x040013CD RID: 5069
	private static IntPtr getIsPlayingMethodId;

	// Token: 0x040013CE RID: 5070
	private static IntPtr getCurrentPlaybackStateMethodId;

	// Token: 0x040013CF RID: 5071
	private static IntPtr getDurationMethodId;

	// Token: 0x040013D0 RID: 5072
	private static IntPtr getStereoModeMethodId;

	// Token: 0x040013D1 RID: 5073
	private static IntPtr getWidthMethodId;

	// Token: 0x040013D2 RID: 5074
	private static IntPtr getHeightMethodId;

	// Token: 0x040013D3 RID: 5075
	private static IntPtr getPlaybackPositionMethodId;

	// Token: 0x040013D4 RID: 5076
	private static IntPtr setPlaybackPositionMethodId;

	// Token: 0x040013D5 RID: 5077
	private static jvalue[] setPlaybackPositionParams;

	// Token: 0x040013D6 RID: 5078
	private static IntPtr playVideoMethodId;

	// Token: 0x040013D7 RID: 5079
	private static jvalue[] playVideoParams;

	// Token: 0x040013D8 RID: 5080
	private static IntPtr stopMethodId;

	// Token: 0x040013D9 RID: 5081
	private static IntPtr resumeMethodId;

	// Token: 0x040013DA RID: 5082
	private static IntPtr pauseMethodId;

	// Token: 0x040013DB RID: 5083
	private static IntPtr setPlaybackSpeedMethodId;

	// Token: 0x040013DC RID: 5084
	private static jvalue[] setPlaybackSpeedParams;

	// Token: 0x040013DD RID: 5085
	private static IntPtr setLoopingMethodId;

	// Token: 0x040013DE RID: 5086
	private static jvalue[] setLoopingParams;

	// Token: 0x040013DF RID: 5087
	private static IntPtr setListenerRotationQuaternionMethodId;

	// Token: 0x040013E0 RID: 5088
	private static jvalue[] setListenerRotationQuaternionParams;

	// Token: 0x020002EF RID: 751
	public enum PlabackState
	{
		// Token: 0x040013E2 RID: 5090
		Idle = 1,
		// Token: 0x040013E3 RID: 5091
		Preparing,
		// Token: 0x040013E4 RID: 5092
		Buffering,
		// Token: 0x040013E5 RID: 5093
		Ready,
		// Token: 0x040013E6 RID: 5094
		Ended
	}

	// Token: 0x020002F0 RID: 752
	public enum StereoMode
	{
		// Token: 0x040013E8 RID: 5096
		Unknown = -1,
		// Token: 0x040013E9 RID: 5097
		Mono,
		// Token: 0x040013EA RID: 5098
		TopBottom,
		// Token: 0x040013EB RID: 5099
		LeftRight,
		// Token: 0x040013EC RID: 5100
		Mesh
	}
}
