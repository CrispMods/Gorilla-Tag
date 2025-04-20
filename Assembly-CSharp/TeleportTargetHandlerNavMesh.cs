using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020002F0 RID: 752
public class TeleportTargetHandlerNavMesh : TeleportTargetHandler
{
	// Token: 0x06001223 RID: 4643 RVA: 0x0003C5C0 File Offset: 0x0003A7C0
	private void Awake()
	{
		this._path = new NavMeshPath();
	}

	// Token: 0x06001224 RID: 4644 RVA: 0x000AF49C File Offset: 0x000AD69C
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

	// Token: 0x06001225 RID: 4645 RVA: 0x000AF508 File Offset: 0x000AD708
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

	// Token: 0x06001226 RID: 4646 RVA: 0x00030607 File Offset: 0x0002E807
	[Conditional("SHOW_PATH_RESULT")]
	private void OnDrawGizmos()
	{
	}

	// Token: 0x040013FD RID: 5117
	public int NavMeshAreaMask = -1;

	// Token: 0x040013FE RID: 5118
	private NavMeshPath _path;
}
