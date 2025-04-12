using System;
using UnityEngine;

// Token: 0x02000492 RID: 1170
public class GamePlayer : MonoBehaviour
{
	// Token: 0x06001C54 RID: 7252 RVA: 0x000DAB3C File Offset: 0x000D8D3C
	private void Awake()
	{
		this.hands = new GamePlayer.HandData[2];
		for (int i = 0; i < 2; i++)
		{
			this.ClearGrabbed(i);
		}
		this.teamId = -1;
	}

	// Token: 0x06001C55 RID: 7253 RVA: 0x000DAB70 File Offset: 0x000D8D70
	public void CleanupPlayer()
	{
		MonkeBallPlayer component = base.GetComponent<MonkeBallPlayer>();
		if (component != null)
		{
			component.currGoalZone = null;
			for (int i = 0; i < MonkeBallGame.Instance.goalZones.Count; i++)
			{
				MonkeBallGame.Instance.goalZones[i].CleanupPlayer(component);
			}
		}
	}

	// Token: 0x06001C56 RID: 7254 RVA: 0x000DABC4 File Offset: 0x000D8DC4
	public void SetGrabbed(GameBallId gameBallId, int handIndex)
	{
		if (gameBallId.IsValid())
		{
			this.ClearGrabbedIfHeld(gameBallId);
		}
		GamePlayer.HandData handData = this.hands[handIndex];
		handData.grabbedGameBallId = gameBallId;
		this.hands[handIndex] = handData;
	}

	// Token: 0x06001C57 RID: 7255 RVA: 0x000DAC04 File Offset: 0x000D8E04
	public void ClearGrabbedIfHeld(GameBallId gameBallId)
	{
		for (int i = 0; i < 2; i++)
		{
			if (this.hands[i].grabbedGameBallId == gameBallId)
			{
				this.ClearGrabbed(i);
			}
		}
	}

	// Token: 0x06001C58 RID: 7256 RVA: 0x00042822 File Offset: 0x00040A22
	public void ClearGrabbed(int handIndex)
	{
		this.SetGrabbed(GameBallId.Invalid, handIndex);
	}

	// Token: 0x06001C59 RID: 7257 RVA: 0x000DAC40 File Offset: 0x000D8E40
	public void ClearAllGrabbed()
	{
		for (int i = 0; i < this.hands.Length; i++)
		{
			this.ClearGrabbed(i);
		}
	}

	// Token: 0x06001C5A RID: 7258 RVA: 0x00042830 File Offset: 0x00040A30
	public void SetInGoalZone(bool inZone)
	{
		if (inZone)
		{
			this.inGoalZone++;
			return;
		}
		this.inGoalZone--;
	}

	// Token: 0x06001C5B RID: 7259 RVA: 0x000DAC68 File Offset: 0x000D8E68
	public bool IsHoldingBall()
	{
		return this.GetGameBallId().IsValid();
	}

	// Token: 0x06001C5C RID: 7260 RVA: 0x00042852 File Offset: 0x00040A52
	public GameBallId GetGameBallId(int handIndex)
	{
		return this.hands[handIndex].grabbedGameBallId;
	}

	// Token: 0x06001C5D RID: 7261 RVA: 0x000DAC84 File Offset: 0x000D8E84
	public int FindHandIndex(GameBallId gameBallId)
	{
		for (int i = 0; i < this.hands.Length; i++)
		{
			if (this.hands[i].grabbedGameBallId == gameBallId)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06001C5E RID: 7262 RVA: 0x000DACC0 File Offset: 0x000D8EC0
	public GameBallId GetGameBallId()
	{
		for (int i = 0; i < this.hands.Length; i++)
		{
			if (this.hands[i].grabbedGameBallId.IsValid())
			{
				return this.hands[i].grabbedGameBallId;
			}
		}
		return GameBallId.Invalid;
	}

	// Token: 0x06001C5F RID: 7263 RVA: 0x00042865 File Offset: 0x00040A65
	public bool IsLocalPlayer()
	{
		return VRRigCache.Instance.localRig.Creator.ActorNumber == this.rig.OwningNetPlayer.ActorNumber;
	}

	// Token: 0x06001C60 RID: 7264 RVA: 0x0004288D File Offset: 0x00040A8D
	public static bool IsLeftHand(int handIndex)
	{
		return handIndex == 0;
	}

	// Token: 0x06001C61 RID: 7265 RVA: 0x00042893 File Offset: 0x00040A93
	public static int GetHandIndex(bool leftHand)
	{
		if (!leftHand)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06001C62 RID: 7266 RVA: 0x000DAD10 File Offset: 0x000D8F10
	public static VRRig GetRig(int actorNumber)
	{
		NetPlayer player = NetworkSystem.Instance.GetPlayer(actorNumber);
		RigContainer rigContainer;
		if (player == null || player.IsNull || !VRRigCache.Instance.TryGetVrrig(player, out rigContainer))
		{
			return null;
		}
		return rigContainer.Rig;
	}

	// Token: 0x06001C63 RID: 7267 RVA: 0x000DAD4C File Offset: 0x000D8F4C
	public static GamePlayer GetGamePlayer(int actorNumber)
	{
		if (actorNumber < 0)
		{
			return null;
		}
		VRRig vrrig = GamePlayer.GetRig(actorNumber);
		if (vrrig == null)
		{
			return null;
		}
		return vrrig.GetComponent<GamePlayer>();
	}

	// Token: 0x06001C64 RID: 7268 RVA: 0x000DAD78 File Offset: 0x000D8F78
	public static GamePlayer GetGamePlayer(Collider collider, bool bodyOnly = false)
	{
		Transform transform = collider.transform;
		while (transform != null)
		{
			GamePlayer component = transform.GetComponent<GamePlayer>();
			if (component != null)
			{
				return component;
			}
			if (bodyOnly)
			{
				break;
			}
			transform = transform.parent;
		}
		return null;
	}

	// Token: 0x04001F57 RID: 8023
	public VRRig rig;

	// Token: 0x04001F58 RID: 8024
	public int teamId;

	// Token: 0x04001F59 RID: 8025
	private GamePlayer.HandData[] hands;

	// Token: 0x04001F5A RID: 8026
	public const int MAX_HANDS = 2;

	// Token: 0x04001F5B RID: 8027
	public const int LEFT_HAND = 0;

	// Token: 0x04001F5C RID: 8028
	public const int RIGHT_HAND = 1;

	// Token: 0x04001F5D RID: 8029
	private int inGoalZone;

	// Token: 0x02000493 RID: 1171
	private struct HandData
	{
		// Token: 0x04001F5E RID: 8030
		public GameBallId grabbedGameBallId;
	}
}
