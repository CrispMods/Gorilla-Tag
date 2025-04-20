using System;
using UnityEngine;

namespace com.AnotherAxiom.MonkeArcade.Joust
{
	// Token: 0x02000B56 RID: 2902
	public class JoustPlayer : MonoBehaviour
	{
		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06004879 RID: 18553 RVA: 0x0005F324 File Offset: 0x0005D524
		// (set) Token: 0x0600487A RID: 18554 RVA: 0x0005F32C File Offset: 0x0005D52C
		public float HorizontalSpeed
		{
			get
			{
				return this.HSpeed;
			}
			set
			{
				this.HSpeed = value;
			}
		}

		// Token: 0x0600487B RID: 18555 RVA: 0x0018F588 File Offset: 0x0018D788
		private void LateUpdate()
		{
			this.velocity.x = this.HSpeed * 0.001f;
			if (this.flap)
			{
				this.velocity.y = Mathf.Min(this.velocity.y + 0.0005f, 0.0005f);
				this.flap = false;
			}
			else
			{
				this.velocity.y = Mathf.Max(this.velocity.y - Time.deltaTime * 0.0001f, -0.001f);
				int i = 0;
				while (i < Physics2D.RaycastNonAlloc(base.transform.position, this.velocity.normalized, this.raycastHitResults, this.velocity.magnitude))
				{
					JoustTerrain joustTerrain;
					if (this.raycastHitResults[i].collider.TryGetComponent<JoustTerrain>(out joustTerrain))
					{
						this.velocity.y = 0f;
						if (joustTerrain.transform.localPosition.y < base.transform.localPosition.y)
						{
							base.transform.localPosition = new Vector2(base.transform.localPosition.x, joustTerrain.transform.localPosition.y + this.raycastHitResults[i].collider.bounds.size.y);
							break;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
			base.transform.Translate(this.velocity);
			if ((double)Mathf.Abs(base.transform.localPosition.x) > 4.5)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x * -0.95f, base.transform.localPosition.y);
			}
		}

		// Token: 0x0600487C RID: 18556 RVA: 0x0005F335 File Offset: 0x0005D535
		public void Flap()
		{
			this.flap = true;
		}

		// Token: 0x040049CB RID: 18891
		private Vector2 velocity;

		// Token: 0x040049CC RID: 18892
		private RaycastHit2D[] raycastHitResults = new RaycastHit2D[8];

		// Token: 0x040049CD RID: 18893
		private float HSpeed;

		// Token: 0x040049CE RID: 18894
		private bool flap;
	}
}
