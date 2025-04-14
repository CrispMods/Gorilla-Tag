using System;
using Drawing;
using UnityEngine;

// Token: 0x020006A5 RID: 1701
public class ComputePenetration : MonoBehaviour
{
	// Token: 0x06002A51 RID: 10833 RVA: 0x000D34DB File Offset: 0x000D16DB
	public void Compute()
	{
		if (this.colliderA == null)
		{
			return;
		}
		this.colliderB == null;
	}

	// Token: 0x06002A52 RID: 10834 RVA: 0x000D34FC File Offset: 0x000D16FC
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

	// Token: 0x06002A53 RID: 10835 RVA: 0x000D35FC File Offset: 0x000D17FC
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

	// Token: 0x04002FCF RID: 12239
	public Collider colliderA;

	// Token: 0x04002FD0 RID: 12240
	public Collider colliderB;

	// Token: 0x04002FD1 RID: 12241
	public bool overlapped;

	// Token: 0x04002FD2 RID: 12242
	public Vector3 direction;

	// Token: 0x04002FD3 RID: 12243
	public float distance;

	// Token: 0x04002FD4 RID: 12244
	private TimeSince lastUpdate = TimeSince.Now();
}
