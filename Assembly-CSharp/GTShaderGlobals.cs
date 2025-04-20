using System;
using UnityEngine;

// Token: 0x020001B9 RID: 441
public class GTShaderGlobals : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06000A60 RID: 2656 RVA: 0x00037429 File Offset: 0x00035629
	public static Vector3 WorldSpaceCameraPos
	{
		get
		{
			return GTShaderGlobals.gMainCameraWorldPos;
		}
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x06000A61 RID: 2657 RVA: 0x00037430 File Offset: 0x00035630
	public static float Time
	{
		get
		{
			return GTShaderGlobals.gTime;
		}
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06000A62 RID: 2658 RVA: 0x00037437 File Offset: 0x00035637
	public static int Frame
	{
		get
		{
			return GTShaderGlobals.gIFrame;
		}
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x0003743E File Offset: 0x0003563E
	private void Awake()
	{
		GTShaderGlobals.gMainCamera = Camera.main;
		if (GTShaderGlobals.gMainCamera)
		{
			GTShaderGlobals.gMainCameraXform = GTShaderGlobals.gMainCamera.transform;
			GTShaderGlobals.gMainCameraWorldPos = GTShaderGlobals.gMainCameraXform.position;
		}
		this.SliceUpdate();
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x0003747A File Offset: 0x0003567A
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Initialize()
	{
		GTShaderGlobals.InitBlueNoiseTex();
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x00032C89 File Offset: 0x00030E89
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x00032C92 File Offset: 0x00030E92
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x00037481 File Offset: 0x00035681
	public void SliceUpdate()
	{
		GTShaderGlobals.UpdateTime();
		GTShaderGlobals.UpdateFrame();
		GTShaderGlobals.UpdateCamera();
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x00037492 File Offset: 0x00035692
	private static void UpdateFrame()
	{
		GTShaderGlobals.gIFrame = UnityEngine.Time.frameCount;
		Shader.SetGlobalInteger(GTShaderGlobals._GT_iFrame, GTShaderGlobals.gIFrame);
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x000374B2 File Offset: 0x000356B2
	private static void UpdateCamera()
	{
		if (!GTShaderGlobals.gMainCameraXform)
		{
			return;
		}
		GTShaderGlobals.gMainCameraWorldPos = GTShaderGlobals.gMainCameraXform.position;
		Shader.SetGlobalVector(GTShaderGlobals._GT_WorldSpaceCameraPos, GTShaderGlobals.gMainCameraWorldPos);
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x000974AC File Offset: 0x000956AC
	private static void UpdateTime()
	{
		GTShaderGlobals.gTime = (float)(DateTime.UtcNow - GTShaderGlobals.gStartTime).TotalSeconds;
		Shader.SetGlobalFloat(GTShaderGlobals._GT_Time, GTShaderGlobals.gTime);
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x000374E9 File Offset: 0x000356E9
	private static void UpdatePawns()
	{
		GTShaderGlobals.gActivePawns = GorillaPawn.ActiveCount;
		GorillaPawn.SyncPawnData();
		Shader.SetGlobalMatrixArray(GTShaderGlobals._GT_PawnData, GTShaderGlobals.gPawnData);
		Shader.SetGlobalInteger(GTShaderGlobals._GT_PawnActiveCount, GTShaderGlobals.gActivePawns);
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x000974EC File Offset: 0x000956EC
	private static void InitBlueNoiseTex()
	{
		GTShaderGlobals.gBlueNoiseTex = Resources.Load<Texture2D>("Graphics/Textures/noise_blue_rgba_128");
		GTShaderGlobals.gBlueNoiseTexWH = GTShaderGlobals.gBlueNoiseTex.GetTexelSize();
		Shader.SetGlobalTexture(GTShaderGlobals._GT_BlueNoiseTex, GTShaderGlobals.gBlueNoiseTex);
		Shader.SetGlobalVector(GTShaderGlobals._GT_BlueNoiseTex_WH, GTShaderGlobals.gBlueNoiseTexWH);
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04000CBC RID: 3260
	private static Camera gMainCamera;

	// Token: 0x04000CBD RID: 3261
	private static Transform gMainCameraXform;

	// Token: 0x04000CBE RID: 3262
	private static Vector3 gMainCameraWorldPos;

	// Token: 0x04000CBF RID: 3263
	[Space]
	private static int gIFrame;

	// Token: 0x04000CC0 RID: 3264
	private static float gTime;

	// Token: 0x04000CC1 RID: 3265
	[Space]
	private static Texture2D gBlueNoiseTex;

	// Token: 0x04000CC2 RID: 3266
	private static Vector4 gBlueNoiseTexWH;

	// Token: 0x04000CC3 RID: 3267
	[Space]
	private static int gActivePawns;

	// Token: 0x04000CC4 RID: 3268
	[Space]
	private static DateTime gStartTime = DateTime.Today.AddDays(-1.0).ToUniversalTime();

	// Token: 0x04000CC5 RID: 3269
	private static Matrix4x4[] gPawnData = GorillaPawn.ShaderData;

	// Token: 0x04000CC6 RID: 3270
	private static ShaderHashId _GT_WorldSpaceCameraPos = "_GT_WorldSpaceCameraPos";

	// Token: 0x04000CC7 RID: 3271
	private static ShaderHashId _GT_BlueNoiseTex = "_GT_BlueNoiseTex";

	// Token: 0x04000CC8 RID: 3272
	private static ShaderHashId _GT_BlueNoiseTex_WH = "_GT_BlueNoiseTex_WH";

	// Token: 0x04000CC9 RID: 3273
	private static ShaderHashId _GT_iFrame = "_GT_iFrame";

	// Token: 0x04000CCA RID: 3274
	private static ShaderHashId _GT_Time = "_GT_Time";

	// Token: 0x04000CCB RID: 3275
	private static ShaderHashId _GT_PawnData = "_GT_PawnData";

	// Token: 0x04000CCC RID: 3276
	private static ShaderHashId _GT_PawnActiveCount = "_GT_PawnActiveCount";
}
