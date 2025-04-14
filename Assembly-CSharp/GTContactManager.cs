using System;
using UnityEngine;

// Token: 0x020001AD RID: 429
public class GTContactManager : MonoBehaviour
{
	// Token: 0x06000A0D RID: 2573 RVA: 0x000023F4 File Offset: 0x000005F4
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void InitializeOnLoad()
	{
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x00037538 File Offset: 0x00035738
	private static GTContactPoint[] InitContactPoints(int count)
	{
		GTContactPoint[] array = new GTContactPoint[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = new GTContactPoint();
		}
		return array;
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x00037564 File Offset: 0x00035764
	public static void RaiseContact(Vector3 point, Vector3 normal)
	{
		if (GTContactManager.gNextFree == -1)
		{
			return;
		}
		float time = GTShaderGlobals.Time;
		GTContactPoint gtcontactPoint = GTContactManager._gContactPoints[GTContactManager.gNextFree];
		gtcontactPoint.contactPoint = point;
		gtcontactPoint.radius = 0.04f;
		gtcontactPoint.counterVelocity = normal;
		gtcontactPoint.timestamp = time;
		gtcontactPoint.lifetime = 2f;
		gtcontactPoint.color = GTContactManager.gRND.NextColor();
		gtcontactPoint.free = 0U;
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x000375CC File Offset: 0x000357CC
	public static void ProcessContacts()
	{
		Matrix4x4[] shaderData = GTContactManager.ShaderData;
		GTContactPoint[] gContactPoints = GTContactManager._gContactPoints;
		int frame = GTShaderGlobals.Frame;
		for (int i = 0; i < 32; i++)
		{
			GTContactManager.Transfer(ref gContactPoints[i].data, ref shaderData[i]);
		}
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x0003760C File Offset: 0x0003580C
	private static void Transfer(ref Matrix4x4 from, ref Matrix4x4 to)
	{
		to.m00 = from.m00;
		to.m01 = from.m01;
		to.m02 = from.m02;
		to.m03 = from.m03;
		to.m10 = from.m10;
		to.m11 = from.m11;
		to.m12 = from.m12;
		to.m13 = from.m13;
		to.m20 = from.m20;
		to.m21 = from.m21;
		to.m22 = from.m22;
		to.m23 = from.m23;
		to.m30 = from.m30;
		to.m31 = from.m31;
		to.m32 = from.m32;
		to.m33 = from.m33;
	}

	// Token: 0x04000C71 RID: 3185
	public const int MAX_CONTACTS = 32;

	// Token: 0x04000C72 RID: 3186
	public static Matrix4x4[] ShaderData = new Matrix4x4[32];

	// Token: 0x04000C73 RID: 3187
	private static GTContactPoint[] _gContactPoints = GTContactManager.InitContactPoints(32);

	// Token: 0x04000C74 RID: 3188
	private static int gNextFree = 0;

	// Token: 0x04000C75 RID: 3189
	private static SRand gRND = new SRand(DateTime.UtcNow);
}
