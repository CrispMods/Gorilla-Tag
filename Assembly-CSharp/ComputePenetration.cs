using System;
using Drawing;
using UnityEngine;

// Token: 0x020006B9 RID: 1721
public class ComputePenetration : MonoBehaviour
{
	// Token: 0x06002ADF RID: 10975 RVA: 0x0004CE67 File Offset: 0x0004B067
	public void Compute()
	{
		if (this.colliderA == null)
		{
			return;
		}
		this.colliderB == null;
	}

	// Token: 0x06002AE0 RID: 10976 RVA: 0x0011F614 File Offset: 0x0011D814
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

	// Token: 0x06002AE1 RID: 10977 RVA: 0x0011F714 File Offset: 0x0011D914
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

	// Token: 0x04003066 RID: 12390
	public Collider colliderA;

	// Token: 0x04003067 RID: 12391
	public Collider colliderB;

	// Token: 0x04003068 RID: 12392
	public bool overlapped;

	// Token: 0x04003069 RID: 12393
	public Vector3 direction;

	// Token: 0x0400306A RID: 12394
	public float distance;

	// Token: 0x0400306B RID: 12395
	private TimeSince lastUpdate = TimeSince.Now();
}
