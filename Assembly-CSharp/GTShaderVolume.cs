using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000235 RID: 565
[ExecuteAlways]
public class GTShaderVolume : MonoBehaviour
{
	// Token: 0x06000CEF RID: 3311 RVA: 0x00039127 File Offset: 0x00037327
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

	// Token: 0x06000CF0 RID: 3312 RVA: 0x00039150 File Offset: 0x00037350
	private void OnDisable()
	{
		GTShaderVolume.gVolumes.Remove(this);
	}

	// Token: 0x06000CF1 RID: 3313 RVA: 0x000A0C6C File Offset: 0x0009EE6C
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

	// Token: 0x04001055 RID: 4181
	public const int MAX_VOLUMES = 16;

	// Token: 0x04001056 RID: 4182
	private static Matrix4x4[] ShaderData = new Matrix4x4[16];

	// Token: 0x04001057 RID: 4183
	[Space]
	private static List<GTShaderVolume> gVolumes = new List<GTShaderVolume>(16);

	// Token: 0x04001058 RID: 4184
	private static ShaderHashId _GT_ShaderVolumes = "_GT_ShaderVolumes";

	// Token: 0x04001059 RID: 4185
	private static ShaderHashId _GT_ShaderVolumesActive = "_GT_ShaderVolumesActive";
}
