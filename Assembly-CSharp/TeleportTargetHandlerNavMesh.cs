using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020002E5 RID: 741
public class TeleportTargetHandlerNavMesh : TeleportTargetHandler
{
	// Token: 0x060011D7 RID: 4567 RVA: 0x000548CB File Offset: 0x00052ACB
	private void Awake()
	{
		this._path = new NavMeshPath();
	}

	// Token: 0x060011D8 RID: 4568 RVA: 0x000548D8 File Offset: 0x00052AD8
	protected override bool ConsiderTeleport(Vector3 start, ref Vector3 end)
	{
		if (base.LocomotionTeleport.AimCollisionTest(start, end, this.AimCollisionLayerMask, out this.AimData.TargetHitInfo))
		{
			Vector3 normalized = (end - start).normalized;
			end = start + normalized * this.AimData.TargetHitInfo.distance;
			return true;
		}
		return false;
	}

	// Token: 0x060011D9 RID: 4569 RVA: 0x00054944 File Offset: 0x00052B44
	public override Vector3? ConsiderDestination(Vector3 location)
	{
		Vector3? result = base.ConsiderDestination(location);
		if (result != null)
		{
			Vector3 characterPosition = base.LocomotionTeleport.GetCharacterPosition();
			Vector3 valueOrDefault = result.GetValueOrDefault();
			NavMesh.CalculatePath(characterPosition, valueOrDefault, this.NavMeshAreaMask, this._path);
			if (this._path.status == NavMeshPathStatus.PathComplete)
			{
				return result;
			}
		}
		return null;
	}

	// Token: 0x060011DA RID: 4570 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("SHOW_PATH_RESULT")]
	private void OnDrawGizmos()
	{
	}

	// Token: 0x040013B5 RID: 5045
	public int NavMeshAreaMask = -1;

	// Token: 0x040013B6 RID: 5046
	private NavMeshPath _path;
}
