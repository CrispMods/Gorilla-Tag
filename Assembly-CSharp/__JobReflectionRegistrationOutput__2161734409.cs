using System;
using BoingKit;
using GorillaLocomotion.Gameplay;
using GorillaTagScripts;
using GorillaTagScripts.Builder;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

// Token: 0x02000D3B RID: 3387
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__2161734409
{
	// Token: 0x0600543A RID: 21562 RVA: 0x0019C970 File Offset: 0x0019AB70
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

	// Token: 0x0600543B RID: 21563 RVA: 0x0019C9E0 File Offset: 0x0019ABE0
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__2161734409.CreateJobReflectionData();
	}
}
