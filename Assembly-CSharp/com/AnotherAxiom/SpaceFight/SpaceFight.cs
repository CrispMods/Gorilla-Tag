using System;
using UnityEngine;

namespace com.AnotherAxiom.SpaceFight
{
	// Token: 0x02000B22 RID: 2850
	public class SpaceFight : ArcadeGame
	{
		// Token: 0x0600470B RID: 18187 RVA: 0x00151BB4 File Offset: 0x0014FDB4
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

		// Token: 0x0600470C RID: 18188 RVA: 0x00151D44 File Offset: 0x0014FF44
		private void clamp(Transform tr)
		{
			tr.localPosition = new Vector2(Mathf.Clamp(tr.localPosition.x, -this.tableSize.x, this.tableSize.x), Mathf.Clamp(tr.localPosition.y, -this.tableSize.y, this.tableSize.y));
		}

		// Token: 0x0600470D RID: 18189 RVA: 0x00151DAF File Offset: 0x0014FFAF
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

		// Token: 0x0600470E RID: 18190 RVA: 0x00151DD4 File Offset: 0x0014FFD4
		private void move(Transform p, float speed)
		{
			p.Translate(p.up * Time.deltaTime * speed, Space.World);
		}

		// Token: 0x0600470F RID: 18191 RVA: 0x00151DF3 File Offset: 0x0014FFF3
		private void turn(Transform p, bool cw)
		{
			p.Rotate(0f, 0f, (float)(cw ? 180 : -180) * Time.deltaTime);
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x00151E1C File Offset: 0x0015001C
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

		// Token: 0x06004711 RID: 18193 RVA: 0x00151F9C File Offset: 0x0015019C
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

		// Token: 0x06004712 RID: 18194 RVA: 0x000023F4 File Offset: 0x000005F4
		protected override void ButtonUp(int player, ArcadeButtons button)
		{
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x000023F4 File Offset: 0x000005F4
		public override void OnTimeout()
		{
		}

		// Token: 0x04004890 RID: 18576
		[SerializeField]
		private Transform[] player;

		// Token: 0x04004891 RID: 18577
		[SerializeField]
		private Transform[] projectile;

		// Token: 0x04004892 RID: 18578
		[SerializeField]
		private Vector2 tableSize;

		// Token: 0x04004893 RID: 18579
		private bool[] projectilesFired = new bool[2];

		// Token: 0x04004894 RID: 18580
		private SpaceFight.SpaceFlightNetState netStateLast;

		// Token: 0x04004895 RID: 18581
		private SpaceFight.SpaceFlightNetState netStateCur;

		// Token: 0x02000B23 RID: 2851
		[Serializable]
		private struct SpaceFlightNetState : IEquatable<SpaceFight.SpaceFlightNetState>
		{
			// Token: 0x06004715 RID: 18197 RVA: 0x0015209C File Offset: 0x0015029C
			public bool Equals(SpaceFight.SpaceFlightNetState other)
			{
				return this.P1LocX.Approx(other.P1LocX, 1E-06f) && this.P1LocY.Approx(other.P1LocY, 1E-06f) && this.P1Rot.Approx(other.P1Rot, 1E-06f) && this.P2LocX.Approx(other.P2LocX, 1E-06f) && this.P2LocY.Approx(other.P2LocY, 1E-06f) && this.P1Rot.Approx(other.P1Rot, 1E-06f) && this.P1PrLocX.Approx(other.P1PrLocX, 1E-06f) && this.P1PrLocY.Approx(other.P1PrLocY, 1E-06f) && this.P2PrLocX.Approx(other.P2PrLocX, 1E-06f) && this.P2PrLocY.Approx(other.P2PrLocY, 1E-06f);
			}

			// Token: 0x04004896 RID: 18582
			public float P1LocX;

			// Token: 0x04004897 RID: 18583
			public float P1LocY;

			// Token: 0x04004898 RID: 18584
			public float P1Rot;

			// Token: 0x04004899 RID: 18585
			public float P2LocX;

			// Token: 0x0400489A RID: 18586
			public float P2LocY;

			// Token: 0x0400489B RID: 18587
			public float P2Rot;

			// Token: 0x0400489C RID: 18588
			public float P1PrLocX;

			// Token: 0x0400489D RID: 18589
			public float P1PrLocY;

			// Token: 0x0400489E RID: 18590
			public float P2PrLocX;

			// Token: 0x0400489F RID: 18591
			public float P2PrLocY;
		}
	}
}
