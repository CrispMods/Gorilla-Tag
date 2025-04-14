using System;
using UnityEngine;

namespace com.AnotherAxiom.SpaceFight
{
	// Token: 0x02000B25 RID: 2853
	public class SpaceFight : ArcadeGame
	{
		// Token: 0x06004717 RID: 18199 RVA: 0x0015217C File Offset: 0x0015037C
		private void Update()
		{
			for (int i = 0; i < 2; i++)
			{
				if (base.getButtonState(i, ArcadeButtons.UP))
				{
					this.move(this.player[i], 0.15f);
					this.clamp(this.player[i]);
				}
				if (base.getButtonState(i, ArcadeButtons.RIGHT))
				{
					this.turn(this.player[i], true);
				}
				if (base.getButtonState(i, ArcadeButtons.LEFT))
				{
					this.turn(this.player[i], false);
				}
				if (this.projectilesFired[i])
				{
					this.move(this.projectile[i], 0.5f);
					if (Vector2.Distance(this.player[1 - i].localPosition, this.projectile[i].localPosition) < 0.25f)
					{
						base.PlaySound(1, 2);
						this.player[1 - i].Rotate(0f, 0f, 180f);
						this.projectilesFired[i] = false;
					}
					if (Mathf.Abs(this.projectile[i].localPosition.x) > this.tableSize.x || Mathf.Abs(this.projectile[i].localPosition.y) > this.tableSize.y)
					{
						this.projectilesFired[i] = false;
					}
				}
				if (!this.projectilesFired[i])
				{
					this.projectile[i].position = this.player[i].position;
					this.projectile[i].rotation = this.player[i].rotation;
				}
			}
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x0015230C File Offset: 0x0015050C
		private void clamp(Transform tr)
		{
			tr.localPosition = new Vector2(Mathf.Clamp(tr.localPosition.x, -this.tableSize.x, this.tableSize.x), Mathf.Clamp(tr.localPosition.y, -this.tableSize.y, this.tableSize.y));
		}

		// Token: 0x06004719 RID: 18201 RVA: 0x00152377 File Offset: 0x00150577
		protected override void ButtonDown(int player, ArcadeButtons button)
		{
			if (button == ArcadeButtons.TRIGGER)
			{
				if (!this.projectilesFired[player])
				{
					base.PlaySound(0, 3);
				}
				this.projectilesFired[player] = true;
			}
		}

		// Token: 0x0600471A RID: 18202 RVA: 0x0015239C File Offset: 0x0015059C
		private void move(Transform p, float speed)
		{
			p.Translate(p.up * Time.deltaTime * speed, Space.World);
		}

		// Token: 0x0600471B RID: 18203 RVA: 0x001523BB File Offset: 0x001505BB
		private void turn(Transform p, bool cw)
		{
			p.Rotate(0f, 0f, (float)(cw ? 180 : -180) * Time.deltaTime);
		}

		// Token: 0x0600471C RID: 18204 RVA: 0x001523E4 File Offset: 0x001505E4
		public override byte[] GetNetworkState()
		{
			this.netStateCur.P1LocX = this.player[0].localPosition.x;
			this.netStateCur.P1LocY = this.player[0].localPosition.y;
			this.netStateCur.P1Rot = this.player[0].localRotation.eulerAngles.z;
			this.netStateCur.P2LocX = this.player[1].localPosition.x;
			this.netStateCur.P2LocY = this.player[1].localPosition.y;
			this.netStateCur.P2Rot = this.player[1].localRotation.eulerAngles.z;
			this.netStateCur.P1PrLocX = this.projectile[0].localPosition.x;
			this.netStateCur.P1PrLocY = this.projectile[0].localPosition.y;
			this.netStateCur.P2PrLocX = this.projectile[1].localPosition.x;
			this.netStateCur.P2PrLocY = this.projectile[1].localPosition.y;
			if (!this.netStateCur.Equals(this.netStateLast))
			{
				this.netStateLast = this.netStateCur;
				base.SwapNetStateBuffersAndStreams();
				ArcadeGame.WrapNetState(this.netStateLast, this.netStateMemStream);
			}
			return this.netStateBuffer;
		}

		// Token: 0x0600471D RID: 18205 RVA: 0x00152564 File Offset: 0x00150764
		public override void SetNetworkState(byte[] b)
		{
			SpaceFight.SpaceFlightNetState spaceFlightNetState = (SpaceFight.SpaceFlightNetState)ArcadeGame.UnwrapNetState(b);
			this.player[0].localPosition = new Vector2(spaceFlightNetState.P1LocX, spaceFlightNetState.P1LocY);
			this.player[0].localRotation = Quaternion.Euler(0f, 0f, spaceFlightNetState.P1Rot);
			this.player[1].localPosition = new Vector2(spaceFlightNetState.P2LocX, spaceFlightNetState.P2LocY);
			this.player[1].localRotation = Quaternion.Euler(0f, 0f, spaceFlightNetState.P2Rot);
			this.projectile[0].localPosition = new Vector2(spaceFlightNetState.P1PrLocX, spaceFlightNetState.P1PrLocY);
			this.projectile[1].localPosition = new Vector2(spaceFlightNetState.P2PrLocX, spaceFlightNetState.P2PrLocY);
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x000023F4 File Offset: 0x000005F4
		protected override void ButtonUp(int player, ArcadeButtons button)
		{
		}

		// Token: 0x0600471F RID: 18207 RVA: 0x000023F4 File Offset: 0x000005F4
		public override void OnTimeout()
		{
		}

		// Token: 0x040048A2 RID: 18594
		[SerializeField]
		private Transform[] player;

		// Token: 0x040048A3 RID: 18595
		[SerializeField]
		private Transform[] projectile;

		// Token: 0x040048A4 RID: 18596
		[SerializeField]
		private Vector2 tableSize;

		// Token: 0x040048A5 RID: 18597
		private bool[] projectilesFired = new bool[2];

		// Token: 0x040048A6 RID: 18598
		private SpaceFight.SpaceFlightNetState netStateLast;

		// Token: 0x040048A7 RID: 18599
		private SpaceFight.SpaceFlightNetState netStateCur;

		// Token: 0x02000B26 RID: 2854
		[Serializable]
		private struct SpaceFlightNetState : IEquatable<SpaceFight.SpaceFlightNetState>
		{
			// Token: 0x06004721 RID: 18209 RVA: 0x00152664 File Offset: 0x00150864
			public bool Equals(SpaceFight.SpaceFlightNetState other)
			{
				return this.P1LocX.Approx(other.P1LocX, 1E-06f) && this.P1LocY.Approx(other.P1LocY, 1E-06f) && this.P1Rot.Approx(other.P1Rot, 1E-06f) && this.P2LocX.Approx(other.P2LocX, 1E-06f) && this.P2LocY.Approx(other.P2LocY, 1E-06f) && this.P1Rot.Approx(other.P1Rot, 1E-06f) && this.P1PrLocX.Approx(other.P1PrLocX, 1E-06f) && this.P1PrLocY.Approx(other.P1PrLocY, 1E-06f) && this.P2PrLocX.Approx(other.P2PrLocX, 1E-06f) && this.P2PrLocY.Approx(other.P2PrLocY, 1E-06f);
			}

			// Token: 0x040048A8 RID: 18600
			public float P1LocX;

			// Token: 0x040048A9 RID: 18601
			public float P1LocY;

			// Token: 0x040048AA RID: 18602
			public float P1Rot;

			// Token: 0x040048AB RID: 18603
			public float P2LocX;

			// Token: 0x040048AC RID: 18604
			public float P2LocY;

			// Token: 0x040048AD RID: 18605
			public float P2Rot;

			// Token: 0x040048AE RID: 18606
			public float P1PrLocX;

			// Token: 0x040048AF RID: 18607
			public float P1PrLocY;

			// Token: 0x040048B0 RID: 18608
			public float P2PrLocX;

			// Token: 0x040048B1 RID: 18609
			public float P2PrLocY;
		}
	}
}
