using System;
using Unity.Collections;

namespace GorillaTagScripts
{
	// Token: 0x020009A5 RID: 2469
	public class BuilderTableJobs
	{
		// Token: 0x06003CF7 RID: 15607 RVA: 0x0011F3AC File Offset: 0x0011D5AC
		public static void BuildTestPieceListForJob(BuilderPiece testPiece, NativeList<BuilderPieceData> testPieceList, NativeList<BuilderGridPlaneData> testGridPlaneList)
		{
			if (testPiece == null)
			{
				return;
			}
			int length = testPieceList.Length;
			BuilderPieceData builderPieceData = new BuilderPieceData(testPiece);
			testPieceList.Add(builderPieceData);
			for (int i = 0; i < testPiece.gridPlanes.Count; i++)
			{
				BuilderGridPlaneData builderGridPlaneData = new BuilderGridPlaneData(testPiece.gridPlanes[i], length);
				testGridPlaneList.Add(builderGridPlaneData);
			}
			BuilderPiece builderPiece = testPiece.firstChildPiece;
			while (builderPiece != null)
			{
				BuilderTableJobs.BuildTestPieceListForJob(builderPiece, testPieceList, testGridPlaneList);
				builderPiece = builderPiece.nextSiblingPiece;
			}
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x0011F434 File Offset: 0x0011D634
		public static void BuildTestPieceListForJob(BuilderPiece testPiece, NativeList<BuilderGridPlaneData> testGridPlaneList)
		{
			if (testPiece == null)
			{
				return;
			}
			for (int i = 0; i < testPiece.gridPlanes.Count; i++)
			{
				BuilderGridPlaneData builderGridPlaneData = new BuilderGridPlaneData(testPiece.gridPlanes[i], -1);
				testGridPlaneList.Add(builderGridPlaneData);
			}
			BuilderPiece builderPiece = testPiece.firstChildPiece;
			while (builderPiece != null)
			{
				BuilderTableJobs.BuildTestPieceListForJob(builderPiece, testGridPlaneList);
				builderPiece = builderPiece.nextSiblingPiece;
			}
		}
	}
}
