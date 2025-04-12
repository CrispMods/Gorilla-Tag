using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020001E8 RID: 488
public static class GTVertexDataStreams_Descriptors
{
	// Token: 0x06000B63 RID: 2915 RVA: 0x0009943C File Offset: 0x0009763C
	public static void DoSetVertexBufferParams(ref Mesh.MeshData writeData, int totalVertexCount)
	{
		NativeArray<VertexAttributeDescriptor> attributes = new NativeArray<VertexAttributeDescriptor>(6, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
		int num = 0;
		attributes[num++] = GTVertexDataStreams_Descriptors.position;
		attributes[num++] = GTVertexDataStreams_Descriptors.color;
		attributes[num++] = GTVertexDataStreams_Descriptors.uv1;
		attributes[num++] = GTVertexDataStreams_Descriptors.lightmapUv;
		attributes[num++] = GTVertexDataStreams_Descriptors.normal;
		attributes[num++] = GTVertexDataStreams_Descriptors.tangent;
		writeData.SetVertexBufferParams(totalVertexCount, attributes);
		attributes.Dispose();
	}

	// Token: 0x04000DF8 RID: 3576
	public static readonly VertexAttributeDescriptor position = new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, 0);

	// Token: 0x04000DF9 RID: 3577
	public static readonly VertexAttributeDescriptor color = new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.UNorm8, 4, 0);

	// Token: 0x04000DFA RID: 3578
	public static readonly VertexAttributeDescriptor uv1 = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float16, 4, 0);

	// Token: 0x04000DFB RID: 3579
	public static readonly VertexAttributeDescriptor lightmapUv = new VertexAttributeDescriptor(VertexAttribute.TexCoord1, VertexAttributeFormat.Float16, 2, 0);

	// Token: 0x04000DFC RID: 3580
	public static readonly VertexAttributeDescriptor normal = new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3, 1);

	// Token: 0x04000DFD RID: 3581
	public static readonly VertexAttributeDescriptor tangent = new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.SNorm8, 4, 1);
}
