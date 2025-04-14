using System;
using Unity.Collections;

namespace GorillaTagScripts
{
	// Token: 0x020009A2 RID: 2466
	public class BuilderTableJobs
	{
		// Token: 0x06003CEB RID: 15595 RVA: 0x0011EDE4 File Offset: 0x0011CFE4
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

		// Token: 0x06003CEC RID: 15596 RVA: 0x0011EE6C File Offset: 0x0011D06C
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
