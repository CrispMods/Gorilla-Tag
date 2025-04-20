using System;
using UnityEngine;

// Token: 0x02000080 RID: 128
public class Example : MonoBehaviour
{
	// Token: 0x06000365 RID: 869 RVA: 0x00078D90 File Offset: 0x00076F90
	private void OnDrawGizmos()
	{
		if (this.debugPoint)
		{
			DebugExtension.DrawPoint(this.debugPoint_Position, this.debugPoint_Color, this.debugPoint_Scale);
		}
		if (this.debugBounds)
		{
			DebugExtension.DrawBounds(new Bounds(new Vector3(10f, 0f, 0f), this.debugBounds_Size), this.debugBounds_Color);
		}
		if (this.debugCircle)
		{
			DebugExtension.DrawCircle(new Vector3(20f, 0f, 0f), this.debugCircle_Up, this.debugCircle_Color, this.debugCircle_Radius);
		}
		if (this.debugWireSphere)
		{
			Gizmos.color = this.debugWireSphere_Color;
			Gizmos.DrawWireSphere(new Vector3(30f, 0f, 0f), this.debugWireSphere_Radius);
		}
		if (this.debugCylinder)
		{
			DebugExtension.DrawCylinder(new Vector3(40f, 0f, 0f), this.debugCylinder_End, this.debugCylinder_Color, this.debugCylinder_Radius);
		}
		if (this.debugCone)
		{
			DebugExtension.DrawCone(new Vector3(50f, 0f, 0f), this.debugCone_Direction, this.debugCone_Color, this.debugCone_Angle);
		}
		if (this.debugArrow)
		{
			DebugExtension.DrawArrow(new Vector3(60f, 0f, 0f), this.debugArrow_Direction, this.debugArrow_Color);
		}
		if (this.debugCapsule)
		{
			DebugExtension.DrawCapsule(new Vector3(70f, 0f, 0f), this.debugCapsule_End, this.debugCapsule_Color, this.debugCapsule_Radius);
		}
	}

	// Token: 0x06000366 RID: 870 RVA: 0x00078F1C File Offset: 0x0007711C
	private void Update()
	{
		DebugExtension.DebugPoint(this.debugPoint_Position, this.debugPoint_Color, this.debugPoint_Scale, 0f, true);
		DebugExtension.DebugBounds(new Bounds(new Vector3(10f, 0f, 0f), this.debugBounds_Size), this.debugBounds_Color, 0f, true);
		DebugExtension.DebugCircle(new Vector3(20f, 0f, 0f), this.debugCircle_Up, this.debugCircle_Color, this.debugCircle_Radius, 0f, true);
		DebugExtension.DebugWireSphere(new Vector3(30f, 0f, 0f), this.debugWireSphere_Color, this.debugWireSphere_Radius, 0f, true);
		DebugExtension.DebugCylinder(new Vector3(40f, 0f, 0f), this.debugCylinder_End, this.debugCylinder_Color, this.debugCylinder_Radius, 0f, true);
		DebugExtension.DebugCone(new Vector3(50f, 0f, 0f), this.debugCone_Direction, this.debugCone_Color, this.debugCone_Angle, 0f, true);
		DebugExtension.DebugArrow(new Vector3(60f, 0f, 0f), this.debugArrow_Direction, this.debugArrow_Color, 0f, true);
		DebugExtension.DebugCapsule(new Vector3(70f, 0f, 0f), this.debugCapsule_End, this.debugCapsule_Color, this.debugCapsule_Radius, 0f, true);
	}

	// Token: 0x040003D4 RID: 980
	public bool debugPoint;

	// Token: 0x040003D5 RID: 981
	public Vector3 debugPoint_Position;

	// Token: 0x040003D6 RID: 982
	public float debugPoint_Scale;

	// Token: 0x040003D7 RID: 983
	public Color debugPoint_Color;

	// Token: 0x040003D8 RID: 984
	public bool debugBounds;

	// Token: 0x040003D9 RID: 985
	public Vector3 debugBounds_Position;

	// Token: 0x040003DA RID: 986
	public Vector3 debugBounds_Size;

	// Token: 0x040003DB RID: 987
	public Color debugBounds_Color;

	// Token: 0x040003DC RID: 988
	public bool debugCircle;

	// Token: 0x040003DD RID: 989
	public Vector3 debugCircle_Up;

	// Token: 0x040003DE RID: 990
	public float debugCircle_Radius;

	// Token: 0x040003DF RID: 991
	public Color debugCircle_Color;

	// Token: 0x040003E0 RID: 992
	public bool debugWireSphere;

	// Token: 0x040003E1 RID: 993
	public float debugWireSphere_Radius;

	// Token: 0x040003E2 RID: 994
	public Color debugWireSphere_Color;

	// Token: 0x040003E3 RID: 995
	public bool debugCylinder;

	// Token: 0x040003E4 RID: 996
	public Vector3 debugCylinder_End;

	// Token: 0x040003E5 RID: 997
	public float debugCylinder_Radius;

	// Token: 0x040003E6 RID: 998
	public Color debugCylinder_Color;

	// Token: 0x040003E7 RID: 999
	public bool debugCone;

	// Token: 0x040003E8 RID: 1000
	public Vector3 debugCone_Direction;

	// Token: 0x040003E9 RID: 1001
	public float debugCone_Angle;

	// Token: 0x040003EA RID: 1002
	public Color debugCone_Color;

	// Token: 0x040003EB RID: 1003
	public bool debugArrow;

	// Token: 0x040003EC RID: 1004
	public Vector3 debugArrow_Direction;

	// Token: 0x040003ED RID: 1005
	public Color debugArrow_Color;

	// Token: 0x040003EE RID: 1006
	public bool debugCapsule;

	// Token: 0x040003EF RID: 1007
	public Vector3 debugCapsule_End;

	// Token: 0x040003F0 RID: 1008
	public float debugCapsule_Radius;

	// Token: 0x040003F1 RID: 1009
	public Color debugCapsule_Color;
}
