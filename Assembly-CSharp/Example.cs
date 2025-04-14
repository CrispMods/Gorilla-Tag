using System;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class Example : MonoBehaviour
{
	// Token: 0x06000333 RID: 819 RVA: 0x000148E8 File Offset: 0x00012AE8
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

	// Token: 0x06000334 RID: 820 RVA: 0x00014A74 File Offset: 0x00012C74
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

	// Token: 0x040003A0 RID: 928
	public bool debugPoint;

	// Token: 0x040003A1 RID: 929
	public Vector3 debugPoint_Position;

	// Token: 0x040003A2 RID: 930
	public float debugPoint_Scale;

	// Token: 0x040003A3 RID: 931
	public Color debugPoint_Color;

	// Token: 0x040003A4 RID: 932
	public bool debugBounds;

	// Token: 0x040003A5 RID: 933
	public Vector3 debugBounds_Position;

	// Token: 0x040003A6 RID: 934
	public Vector3 debugBounds_Size;

	// Token: 0x040003A7 RID: 935
	public Color debugBounds_Color;

	// Token: 0x040003A8 RID: 936
	public bool debugCircle;

	// Token: 0x040003A9 RID: 937
	public Vector3 debugCircle_Up;

	// Token: 0x040003AA RID: 938
	public float debugCircle_Radius;

	// Token: 0x040003AB RID: 939
	public Color debugCircle_Color;

	// Token: 0x040003AC RID: 940
	public bool debugWireSphere;

	// Token: 0x040003AD RID: 941
	public float debugWireSphere_Radius;

	// Token: 0x040003AE RID: 942
	public Color debugWireSphere_Color;

	// Token: 0x040003AF RID: 943
	public bool debugCylinder;

	// Token: 0x040003B0 RID: 944
	public Vector3 debugCylinder_End;

	// Token: 0x040003B1 RID: 945
	public float debugCylinder_Radius;

	// Token: 0x040003B2 RID: 946
	public Color debugCylinder_Color;

	// Token: 0x040003B3 RID: 947
	public bool debugCone;

	// Token: 0x040003B4 RID: 948
	public Vector3 debugCone_Direction;

	// Token: 0x040003B5 RID: 949
	public float debugCone_Angle;

	// Token: 0x040003B6 RID: 950
	public Color debugCone_Color;

	// Token: 0x040003B7 RID: 951
	public bool debugArrow;

	// Token: 0x040003B8 RID: 952
	public Vector3 debugArrow_Direction;

	// Token: 0x040003B9 RID: 953
	public Color debugArrow_Color;

	// Token: 0x040003BA RID: 954
	public bool debugCapsule;

	// Token: 0x040003BB RID: 955
	public Vector3 debugCapsule_End;

	// Token: 0x040003BC RID: 956
	public float debugCapsule_Radius;

	// Token: 0x040003BD RID: 957
	public Color debugCapsule_Color;
}
