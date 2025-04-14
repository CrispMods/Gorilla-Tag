using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200022A RID: 554
[ExecuteAlways]
public class GTShaderVolume : MonoBehaviour
{
	// Token: 0x06000CA4 RID: 3236 RVA: 0x00042C20 File Offset: 0x00040E20
	private void OnEnable()
	{
		if (GTShaderVolume.gVolumes.Count > 16)
		{
			return;
		}
		if (!GTShaderVolume.gVolumes.Contains(this))
		{
			GTShaderVolume.gVolumes.Add(this);
		}
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x00042C49 File Offset: 0x00040E49
	private void OnDisable()
	{
		GTShaderVolume.gVolumes.Remove(this);
	}

	// Token: 0x06000CA6 RID: 3238 RVA: 0x00042C58 File Offset: 0x00040E58
	public static void SyncVolumeData()
	{
		m4x4 m4x = default(m4x4);
		int count = GTShaderVolume.gVolumes.Count;
		for (int i = 0; i < 16; i++)
		{
			if (i >= count)
			{
				MatrixUtils.Clear(ref GTShaderVolume.ShaderData[i]);
			}
			else
			{
				GTShaderVolume gtshaderVolume = GTShaderVolume.gVolumes[i];
				if (!gtshaderVolume)
				{
					MatrixUtils.Clear(ref GTShaderVolume.ShaderData[i]);
				}
				else
				{
					Transform transform = gtshaderVolume.transform;
					Vector4 vector = transform.position;
					Vector4 vector2 = transform.rotation.ToVector();
					Vector4 vector3 = transform.localScale;
					m4x.SetRow0(ref vector);
					m4x.SetRow1(ref vector2);
					m4x.SetRow2(ref vector3);
					m4x.Push(ref GTShaderVolume.ShaderData[i]);
				}
			}
		}
		Shader.SetGlobalInteger(GTShaderVolume._GT_ShaderVolumesActive, count);
		Shader.SetGlobalMatrixArray(GTShaderVolume._GT_ShaderVolumes, GTShaderVolume.ShaderData);
	}

	// Token: 0x0400100F RID: 4111
	public const int MAX_VOLUMES = 16;

	// Token: 0x04001010 RID: 4112
	private static Matrix4x4[] ShaderData = new Matrix4x4[16];

	// Token: 0x04001011 RID: 4113
	[Space]
	private static List<GTShaderVolume> gVolumes = new List<GTShaderVolume>(16);

	// Token: 0x04001012 RID: 4114
	private static ShaderHashId _GT_ShaderVolumes = "_GT_ShaderVolumes";

	// Token: 0x04001013 RID: 4115
	private static ShaderHashId _GT_ShaderVolumesActive = "_GT_ShaderVolumesActive";
}
