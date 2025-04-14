using System;
using UnityEngine;

// Token: 0x020001AE RID: 430
public class GTShaderGlobals : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x17000105 RID: 261
	// (get) Token: 0x06000A16 RID: 2582 RVA: 0x00037A2C File Offset: 0x00035C2C
	public static Vector3 WorldSpaceCameraPos
	{
		get
		{
			return GTShaderGlobals.gMainCameraWorldPos;
		}
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06000A17 RID: 2583 RVA: 0x00037A33 File Offset: 0x00035C33
	public static float Time
	{
		get
		{
			return GTShaderGlobals.gTime;
		}
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x06000A18 RID: 2584 RVA: 0x00037A3A File Offset: 0x00035C3A
	public static int Frame
	{
		get
		{
			return GTShaderGlobals.gIFrame;
		}
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x00037A41 File Offset: 0x00035C41
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

	// Token: 0x06000A1A RID: 2586 RVA: 0x00037A7D File Offset: 0x00035C7D
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Initialize()
	{
		GTShaderGlobals.InitBlueNoiseTex();
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x00015C1D File Offset: 0x00013E1D
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x00015C26 File Offset: 0x00013E26
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x00037A84 File Offset: 0x00035C84
	public void SliceUpdate()
	{
		GTShaderGlobals.UpdateTime();
		GTShaderGlobals.UpdateFrame();
		GTShaderGlobals.UpdateCamera();
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x00037A95 File Offset: 0x00035C95
	private static void UpdateFrame()
	{
		GTShaderGlobals.gIFrame = UnityEngine.Time.frameCount;
		Shader.SetGlobalInteger(GTShaderGlobals._GT_iFrame, GTShaderGlobals.gIFrame);
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x00037AB5 File Offset: 0x00035CB5
	private static void UpdateCamera()
	{
		if (!GTShaderGlobals.gMainCameraXform)
		{
			return;
		}
		GTShaderGlobals.gMainCameraWorldPos = GTShaderGlobals.gMainCameraXform.position;
		Shader.SetGlobalVector(GTShaderGlobals._GT_WorldSpaceCameraPos, GTShaderGlobals.gMainCameraWorldPos);
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x00037AEC File Offset: 0x00035CEC
	private static void UpdateTime()
	{
		GTShaderGlobals.gTime = (float)(DateTime.UtcNow - GTShaderGlobals.gStartTime).TotalSeconds;
		Shader.SetGlobalFloat(GTShaderGlobals._GT_Time, GTShaderGlobals.gTime);
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x00037B2A File Offset: 0x00035D2A
	private static void UpdatePawns()
	{
		GTShaderGlobals.gActivePawns = GorillaPawn.ActiveCount;
		GorillaPawn.SyncPawnData();
		Shader.SetGlobalMatrixArray(GTShaderGlobals._GT_PawnData, GTShaderGlobals.gPawnData);
		Shader.SetGlobalInteger(GTShaderGlobals._GT_PawnActiveCount, GTShaderGlobals.gActivePawns);
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x00037B64 File Offset: 0x00035D64
	private static void InitBlueNoiseTex()
	{
		GTShaderGlobals.gBlueNoiseTex = Resources.Load<Texture2D>("Graphics/Textures/noise_blue_rgba_128");
		GTShaderGlobals.gBlueNoiseTexWH = GTShaderGlobals.gBlueNoiseTex.GetTexelSize();
		Shader.SetGlobalTexture(GTShaderGlobals._GT_BlueNoiseTex, GTShaderGlobals.gBlueNoiseTex);
		Shader.SetGlobalVector(GTShaderGlobals._GT_BlueNoiseTex_WH, GTShaderGlobals.gBlueNoiseTexWH);
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x0000FD18 File Offset: 0x0000DF18
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04000C77 RID: 3191
	private static Camera gMainCamera;

	// Token: 0x04000C78 RID: 3192
	private static Transform gMainCameraXform;

	// Token: 0x04000C79 RID: 3193
	private static Vector3 gMainCameraWorldPos;

	// Token: 0x04000C7A RID: 3194
	[Space]
	private static int gIFrame;

	// Token: 0x04000C7B RID: 3195
	private static float gTime;

	// Token: 0x04000C7C RID: 3196
	[Space]
	private static Texture2D gBlueNoiseTex;

	// Token: 0x04000C7D RID: 3197
	private static Vector4 gBlueNoiseTexWH;

	// Token: 0x04000C7E RID: 3198
	[Space]
	private static int gActivePawns;

	// Token: 0x04000C7F RID: 3199
	[Space]
	private static DateTime gStartTime = DateTime.Today.AddDays(-1.0).ToUniversalTime();

	// Token: 0x04000C80 RID: 3200
	private static Matrix4x4[] gPawnData = GorillaPawn.ShaderData;

	// Token: 0x04000C81 RID: 3201
	private static ShaderHashId _GT_WorldSpaceCameraPos = "_GT_WorldSpaceCameraPos";

	// Token: 0x04000C82 RID: 3202
	private static ShaderHashId _GT_BlueNoiseTex = "_GT_BlueNoiseTex";

	// Token: 0x04000C83 RID: 3203
	private static ShaderHashId _GT_BlueNoiseTex_WH = "_GT_BlueNoiseTex_WH";

	// Token: 0x04000C84 RID: 3204
	private static ShaderHashId _GT_iFrame = "_GT_iFrame";

	// Token: 0x04000C85 RID: 3205
	private static ShaderHashId _GT_Time = "_GT_Time";

	// Token: 0x04000C86 RID: 3206
	private static ShaderHashId _GT_PawnData = "_GT_PawnData";

	// Token: 0x04000C87 RID: 3207
	private static ShaderHashId _GT_PawnActiveCount = "_GT_PawnActiveCount";
}
