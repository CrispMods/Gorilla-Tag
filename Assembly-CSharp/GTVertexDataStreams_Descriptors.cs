using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020001F3 RID: 499
public static class GTVertexDataStreams_Descriptors
{
	// Token: 0x06000BAD RID: 2989 RVA: 0x0009BD30 File Offset: 0x00099F30
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

	// Token: 0x04000E3D RID: 3645
	public static readonly VertexAttributeDescriptor position = new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, 0);

	// Token: 0x04000E3E RID: 3646
	public static readonly VertexAttributeDescriptor color = new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.UNorm8, 4, 0);

	// Token: 0x04000E3F RID: 3647
	public static readonly VertexAttributeDescriptor uv1 = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float16, 4, 0);

	// Token: 0x04000E40 RID: 3648
	public static readonly VertexAttributeDescriptor lightmapUv = new VertexAttributeDescriptor(VertexAttribute.TexCoord1, VertexAttributeFormat.Float16, 2, 0);

	// Token: 0x04000E41 RID: 3649
	public static readonly VertexAttributeDescriptor normal = new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3, 1);

	// Token: 0x04000E42 RID: 3650
	public static readonly VertexAttributeDescriptor tangent = new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.SNorm8, 4, 1);
}
