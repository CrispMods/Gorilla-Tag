using System;
using Drawing;
using UnityEngine;

// Token: 0x020006A4 RID: 1700
public class ComputePenetration : MonoBehaviour
{
	// Token: 0x06002A49 RID: 10825 RVA: 0x000D305B File Offset: 0x000D125B
	public void Compute()
	{
		if (this.colliderA == null)
		{
			return;
		}
		this.colliderB == null;
	}

	// Token: 0x06002A4A RID: 10826 RVA: 0x000D307C File Offset: 0x000D127C
	public void OnDrawGizmos()
	{
		if (this.colliderA.AsNull<Collider>() == null)
		{
			return;
		}
		if (this.colliderB.AsNull<Collider>() == null)
		{
			return;
		}
		Transform transform = this.colliderA.transform;
		Transform transform2 = this.colliderB.transform;
		if (this.lastUpdate.HasElapsed(0.5f, true))
		{
			this.overlapped = Physics.ComputePenetration(this.colliderA, transform.position, transform.rotation, this.colliderB, transform2.position, transform2.rotation, out this.direction, out this.distance);
		}
		Color color = this.overlapped ? Color.red : Color.green;
		this.DrawCollider(this.colliderA, color);
		this.DrawCollider(this.colliderB, color);
		if (this.overlapped)
		{
			Vector3 position = this.colliderB.transform.position;
			Vector3 to = position + this.direction * this.distance;
			Gizmos.DrawLine(position, to);
		}
	}

	// Token: 0x06002A4B RID: 10827 RVA: 0x000D317C File Offset: 0x000D137C
	private unsafe void DrawCollider(Collider c, Color color)
	{
		CommandBuilder commandBuilder = *Draw.ingame;
		using (commandBuilder.WithMatrix(c.transform.localToWorldMatrix))
		{
			commandBuilder.PushColor(color);
			BoxCollider boxCollider = c as BoxCollider;
			if (boxCollider == null)
			{
				SphereCollider sphereCollider = c as SphereCollider;
				if (sphereCollider == null)
				{
					CapsuleCollider capsuleCollider = c as CapsuleCollider;
					if (capsuleCollider != null)
					{
						commandBuilder.WireCapsule(capsuleCollider.center, Vector3.up, capsuleCollider.height, capsuleCollider.radius);
					}
				}
				else
				{
					commandBuilder.WireSphere(sphereCollider.center, sphereCollider.radius);
				}
			}
			else
			{
				commandBuilder.WireBox(boxCollider.center, boxCollider.size);
			}
			commandBuilder.PopColor();
		}
	}

	// Token: 0x04002FC9 RID: 12233
	public Collider colliderA;

	// Token: 0x04002FCA RID: 12234
	public Collider colliderB;

	// Token: 0x04002FCB RID: 12235
	public bool overlapped;

	// Token: 0x04002FCC RID: 12236
	public Vector3 direction;

	// Token: 0x04002FCD RID: 12237
	public float distance;

	// Token: 0x04002FCE RID: 12238
	private TimeSince lastUpdate = TimeSince.Now();
}
