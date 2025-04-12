using System;
using UnityEngine;

namespace com.AnotherAxiom.MonkeArcade.Joust
{
	// Token: 0x02000B2C RID: 2860
	public class JoustPlayer : MonoBehaviour
	{
		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x0600473C RID: 18236 RVA: 0x0005D90D File Offset: 0x0005BB0D
		// (set) Token: 0x0600473D RID: 18237 RVA: 0x0005D915 File Offset: 0x0005BB15
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

		// Token: 0x0600473E RID: 18238 RVA: 0x00188614 File Offset: 0x00186814
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

		// Token: 0x0600473F RID: 18239 RVA: 0x0005D91E File Offset: 0x0005BB1E
		public void Flap()
		{
			this.flap = true;
		}

		// Token: 0x040048E8 RID: 18664
		private Vector2 velocity;

		// Token: 0x040048E9 RID: 18665
		private RaycastHit2D[] raycastHitResults = new RaycastHit2D[8];

		// Token: 0x040048EA RID: 18666
		private float HSpeed;

		// Token: 0x040048EB RID: 18667
		private bool flap;
	}
}
