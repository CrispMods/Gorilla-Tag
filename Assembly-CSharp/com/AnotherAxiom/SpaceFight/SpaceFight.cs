using System;
using UnityEngine;

namespace com.AnotherAxiom.SpaceFight
{
	// Token: 0x02000B4F RID: 2895
	public class SpaceFight : ArcadeGame
	{
		// Token: 0x06004854 RID: 18516 RVA: 0x0018E214 File Offset: 0x0018C414
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

		// Token: 0x06004855 RID: 18517 RVA: 0x0018E3A4 File Offset: 0x0018C5A4
		private void clamp(Transform tr)
		{
			tr.localPosition = new Vector2(Mathf.Clamp(tr.localPosition.x, -this.tableSize.x, this.tableSize.x), Mathf.Clamp(tr.localPosition.y, -this.tableSize.y, this.tableSize.y));
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x0005F1A3 File Offset: 0x0005D3A3
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

		// Token: 0x06004857 RID: 18519 RVA: 0x0005F1C8 File Offset: 0x0005D3C8
		private void move(Transform p, float speed)
		{
			p.Translate(p.up * Time.deltaTime * speed, Space.World);
		}

		// Token: 0x06004858 RID: 18520 RVA: 0x0005F1E7 File Offset: 0x0005D3E7
		private void turn(Transform p, bool cw)
		{
			p.Rotate(0f, 0f, (float)(cw ? 180 : -180) * Time.deltaTime);
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x0018E410 File Offset: 0x0018C610
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

		// Token: 0x0600485A RID: 18522 RVA: 0x0018E590 File Offset: 0x0018C790
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

		// Token: 0x0600485B RID: 18523 RVA: 0x00030607 File Offset: 0x0002E807
		protected override void ButtonUp(int player, ArcadeButtons button)
		{
		}

		// Token: 0x0600485C RID: 18524 RVA: 0x00030607 File Offset: 0x0002E807
		public override void OnTimeout()
		{
		}

		// Token: 0x04004985 RID: 18821
		[SerializeField]
		private Transform[] player;

		// Token: 0x04004986 RID: 18822
		[SerializeField]
		private Transform[] projectile;

		// Token: 0x04004987 RID: 18823
		[SerializeField]
		private Vector2 tableSize;

		// Token: 0x04004988 RID: 18824
		private bool[] projectilesFired = new bool[2];

		// Token: 0x04004989 RID: 18825
		private SpaceFight.SpaceFlightNetState netStateLast;

		// Token: 0x0400498A RID: 18826
		private SpaceFight.SpaceFlightNetState netStateCur;

		// Token: 0x02000B50 RID: 2896
		[Serializable]
		private struct SpaceFlightNetState : IEquatable<SpaceFight.SpaceFlightNetState>
		{
			// Token: 0x0600485E RID: 18526 RVA: 0x0018E67C File Offset: 0x0018C87C
			public bool Equals(SpaceFight.SpaceFlightNetState other)
			{
				return this.P1LocX.Approx(other.P1LocX, 1E-06f) && this.P1LocY.Approx(other.P1LocY, 1E-06f) && this.P1Rot.Approx(other.P1Rot, 1E-06f) && this.P2LocX.Approx(other.P2LocX, 1E-06f) && this.P2LocY.Approx(other.P2LocY, 1E-06f) && this.P1Rot.Approx(other.P1Rot, 1E-06f) && this.P1PrLocX.Approx(other.P1PrLocX, 1E-06f) && this.P1PrLocY.Approx(other.P1PrLocY, 1E-06f) && this.P2PrLocX.Approx(other.P2PrLocX, 1E-06f) && this.P2PrLocY.Approx(other.P2PrLocY, 1E-06f);
			}

			// Token: 0x0400498B RID: 18827
			public float P1LocX;

			// Token: 0x0400498C RID: 18828
			public float P1LocY;

			// Token: 0x0400498D RID: 18829
			public float P1Rot;

			// Token: 0x0400498E RID: 18830
			public float P2LocX;

			// Token: 0x0400498F RID: 18831
			public float P2LocY;

			// Token: 0x04004990 RID: 18832
			public float P2Rot;

			// Token: 0x04004991 RID: 18833
			public float P1PrLocX;

			// Token: 0x04004992 RID: 18834
			public float P1PrLocY;

			// Token: 0x04004993 RID: 18835
			public float P2PrLocX;

			// Token: 0x04004994 RID: 18836
			public float P2PrLocY;
		}
	}
}
