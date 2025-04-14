using System;
using UnityEngine;

namespace com.AnotherAxiom.MonkeArcade.Joust
{
	// Token: 0x02000B29 RID: 2857
	public class JoustPlayer : MonoBehaviour
	{
		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06004730 RID: 18224 RVA: 0x001530A9 File Offset: 0x001512A9
		// (set) Token: 0x06004731 RID: 18225 RVA: 0x001530B1 File Offset: 0x001512B1
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

		// Token: 0x06004732 RID: 18226 RVA: 0x001530BC File Offset: 0x001512BC
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

		// Token: 0x06004733 RID: 18227 RVA: 0x001532A2 File Offset: 0x001514A2
		public void Flap()
		{
			this.flap = true;
		}

		// Token: 0x040048D6 RID: 18646
		private Vector2 velocity;

		// Token: 0x040048D7 RID: 18647
		private RaycastHit2D[] raycastHitResults = new RaycastHit2D[8];

		// Token: 0x040048D8 RID: 18648
		private float HSpeed;

		// Token: 0x040048D9 RID: 18649
		private bool flap;
	}
}
