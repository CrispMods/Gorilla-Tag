using System;
using BoingKit;
using GorillaLocomotion.Gameplay;
using GorillaTagScripts;
using GorillaTagScripts.Builder;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

// Token: 0x02000D6C RID: 3436
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__2483391320
{
	// Token: 0x0600559C RID: 21916 RVA: 0x001D153C File Offset: 0x001CF73C
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobParallelForTransformExtensions.EarlyJobInit<BuilderRenderer.SetupInstanceDataForMesh>();
			IJobParallelForTransformExtensions.EarlyJobInit<BuilderRenderer.SetupInstanceDataForMeshStatic>();
			IJobParallelForExtensions.EarlyJobInit<GorillaIKMgr.IKJob>();
			IJobParallelForTransformExtensions.EarlyJobInit<GorillaIKMgr.IKTransformJob>();
			IJobExtensions.EarlyJobInit<DayNightCycle.LerpBakedLightingJob>();
			IJobParallelForTransformExtensions.EarlyJobInit<VRRigJobManager.VRRigTransformJob>();
			IJobParallelForExtensions.EarlyJobInit<BuilderFindPotentialSnaps>();
			IJobParallelForTransformExtensions.EarlyJobInit<FindNearbyPiecesJob>();
			IJobParallelForTransformExtensions.EarlyJobInit<BuilderConveyorManager.EvaluateSplineJob>();
			IJobExtensions.EarlyJobInit<SolveRopeJob>();
			IJobExtensions.EarlyJobInit<VectorizedSolveRopeJob>();
			IJobParallelForExtensions.EarlyJobInit<BoingWorkAsynchronous.BehaviorJob>();
			IJobParallelForExtensions.EarlyJobInit<BoingWorkAsynchronous.ReactorJob>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x0600559D RID: 21917 RVA: 0x000677FC File Offset: 0x000659FC
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__2483391320.CreateJobReflectionData();
	}
}
