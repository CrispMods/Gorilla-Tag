using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020002E5 RID: 741
public class TeleportTargetHandlerNavMesh : TeleportTargetHandler
{
	// Token: 0x060011DA RID: 4570 RVA: 0x00054C4F File Offset: 0x00052E4F
	private void Awake()
	{
		this._path = new NavMeshPath();
	}

	// Token: 0x060011DB RID: 4571 RVA: 0x00054C5C File Offset: 0x00052E5C
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

	// Token: 0x060011DC RID: 4572 RVA: 0x00054CC8 File Offset: 0x00052EC8
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

	// Token: 0x060011DD RID: 4573 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("SHOW_PATH_RESULT")]
	private void OnDrawGizmos()
	{
	}

	// Token: 0x040013B6 RID: 5046
	public int NavMeshAreaMask = -1;

	// Token: 0x040013B7 RID: 5047
	private NavMeshPath _path;
}
