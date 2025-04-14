using System;
using UnityEngine;

// Token: 0x020001AE RID: 430
public class GTShaderGlobals : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x17000105 RID: 261
	// (get) Token: 0x06000A14 RID: 2580 RVA: 0x00037708 File Offset: 0x00035908
	public static Vector3 WorldSpaceCameraPos
	{
		get
		{
			return GTShaderGlobals.gMainCameraWorldPos;
		}
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0003770F File Offset: 0x0003590F
	public static float Time
	{
		get
		{
			return GTShaderGlobals.gTime;
		}
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x06000A16 RID: 2582 RVA: 0x00037716 File Offset: 0x00035916
	public static int Frame
	{
		get
		{
			return GTShaderGlobals.gIFrame;
		}
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x0003771D File Offset: 0x0003591D
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

	// Token: 0x06000A18 RID: 2584 RVA: 0x00037759 File Offset: 0x00035959
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Initialize()
	{
		GTShaderGlobals.InitBlueNoiseTex();
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x000158F9 File Offset: 0x00013AF9
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x00015902 File Offset: 0x00013B02
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x00037760 File Offset: 0x00035960
	public void SliceUpdate()
	{
		GTShaderGlobals.UpdateTime();
		GTShaderGlobals.UpdateFrame();
		GTShaderGlobals.UpdateCamera();
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x00037771 File Offset: 0x00035971
	private static void UpdateFrame()
	{
		GTShaderGlobals.gIFrame = UnityEngine.Time.frameCount;
		Shader.SetGlobalInteger(GTShaderGlobals._GT_iFrame, GTShaderGlobals.gIFrame);
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x00037791 File Offset: 0x00035991
	private static void UpdateCamera()
	{
		if (!GTShaderGlobals.gMainCameraXform)
		{
			return;
		}
		GTShaderGlobals.gMainCameraWorldPos = GTShaderGlobals.gMainCameraXform.position;
		Shader.SetGlobalVector(GTShaderGlobals._GT_WorldSpaceCameraPos, GTShaderGlobals.gMainCameraWorldPos);
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x000377C8 File Offset: 0x000359C8
	private static void UpdateTime()
	{
		GTShaderGlobals.gTime = (float)(DateTime.UtcNow - GTShaderGlobals.gStartTime).TotalSeconds;
		Shader.SetGlobalFloat(GTShaderGlobals._GT_Time, GTShaderGlobals.gTime);
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x00037806 File Offset: 0x00035A06
	private static void UpdatePawns()
	{
		GTShaderGlobals.gActivePawns = GorillaPawn.ActiveCount;
		GorillaPawn.SyncPawnData();
		Shader.SetGlobalMatrixArray(GTShaderGlobals._GT_PawnData, GTShaderGlobals.gPawnData);
		Shader.SetGlobalInteger(GTShaderGlobals._GT_PawnActiveCount, GTShaderGlobals.gActivePawns);
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x00037840 File Offset: 0x00035A40
	private static void InitBlueNoiseTex()
	{
		GTShaderGlobals.gBlueNoiseTex = Resources.Load<Texture2D>("Graphics/Textures/noise_blue_rgba_128");
		GTShaderGlobals.gBlueNoiseTexWH = GTShaderGlobals.gBlueNoiseTex.GetTexelSize();
		Shader.SetGlobalTexture(GTShaderGlobals._GT_BlueNoiseTex, GTShaderGlobals.gBlueNoiseTex);
		Shader.SetGlobalVector(GTShaderGlobals._GT_BlueNoiseTex_WH, GTShaderGlobals.gBlueNoiseTexWH);
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x0000F974 File Offset: 0x0000DB74
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04000C76 RID: 3190
	private static Camera gMainCamera;

	// Token: 0x04000C77 RID: 3191
	private static Transform gMainCameraXform;

	// Token: 0x04000C78 RID: 3192
	private static Vector3 gMainCameraWorldPos;

	// Token: 0x04000C79 RID: 3193
	[Space]
	private static int gIFrame;

	// Token: 0x04000C7A RID: 3194
	private static float gTime;

	// Token: 0x04000C7B RID: 3195
	[Space]
	private static Texture2D gBlueNoiseTex;

	// Token: 0x04000C7C RID: 3196
	private static Vector4 gBlueNoiseTexWH;

	// Token: 0x04000C7D RID: 3197
	[Space]
	private static int gActivePawns;

	// Token: 0x04000C7E RID: 3198
	[Space]
	private static DateTime gStartTime = DateTime.Today.AddDays(-1.0).ToUniversalTime();

	// Token: 0x04000C7F RID: 3199
	private static Matrix4x4[] gPawnData = GorillaPawn.ShaderData;

	// Token: 0x04000C80 RID: 3200
	private static ShaderHashId _GT_WorldSpaceCameraPos = "_GT_WorldSpaceCameraPos";

	// Token: 0x04000C81 RID: 3201
	private static ShaderHashId _GT_BlueNoiseTex = "_GT_BlueNoiseTex";

	// Token: 0x04000C82 RID: 3202
	private static ShaderHashId _GT_BlueNoiseTex_WH = "_GT_BlueNoiseTex_WH";

	// Token: 0x04000C83 RID: 3203
	private static ShaderHashId _GT_iFrame = "_GT_iFrame";

	// Token: 0x04000C84 RID: 3204
	private static ShaderHashId _GT_Time = "_GT_Time";

	// Token: 0x04000C85 RID: 3205
	private static ShaderHashId _GT_PawnData = "_GT_PawnData";

	// Token: 0x04000C86 RID: 3206
	private static ShaderHashId _GT_PawnActiveCount = "_GT_PawnActiveCount";
}
