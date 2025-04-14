using System;
using UnityEngine;

// Token: 0x02000492 RID: 1170
public class GamePlayer : MonoBehaviour
{
	// Token: 0x06001C51 RID: 7249 RVA: 0x00089EB0 File Offset: 0x000880B0
	private void Awake()
	{
		this.hands = new GamePlayer.HandData[2];
		for (int i = 0; i < 2; i++)
		{
			this.ClearGrabbed(i);
		}
		this.teamId = -1;
	}

	// Token: 0x06001C52 RID: 7250 RVA: 0x00089EE4 File Offset: 0x000880E4
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

	// Token: 0x06001C53 RID: 7251 RVA: 0x00089F38 File Offset: 0x00088138
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

	// Token: 0x06001C54 RID: 7252 RVA: 0x00089F78 File Offset: 0x00088178
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

	// Token: 0x06001C55 RID: 7253 RVA: 0x00089FB1 File Offset: 0x000881B1
	public void ClearGrabbed(int handIndex)
	{
		this.SetGrabbed(GameBallId.Invalid, handIndex);
	}

	// Token: 0x06001C56 RID: 7254 RVA: 0x00089FC0 File Offset: 0x000881C0
	public void ClearAllGrabbed()
	{
		for (int i = 0; i < this.hands.Length; i++)
		{
			this.ClearGrabbed(i);
		}
	}

	// Token: 0x06001C57 RID: 7255 RVA: 0x00089FE7 File Offset: 0x000881E7
	public void SetInGoalZone(bool inZone)
	{
		if (inZone)
		{
			this.inGoalZone++;
			return;
		}
		this.inGoalZone--;
	}

	// Token: 0x06001C58 RID: 7256 RVA: 0x0008A00C File Offset: 0x0008820C
	public bool IsHoldingBall()
	{
		return this.GetGameBallId().IsValid();
	}

	// Token: 0x06001C59 RID: 7257 RVA: 0x0008A027 File Offset: 0x00088227
	public GameBallId GetGameBallId(int handIndex)
	{
		return this.hands[handIndex].grabbedGameBallId;
	}

	// Token: 0x06001C5A RID: 7258 RVA: 0x0008A03C File Offset: 0x0008823C
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

	// Token: 0x06001C5B RID: 7259 RVA: 0x0008A078 File Offset: 0x00088278
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

	// Token: 0x06001C5C RID: 7260 RVA: 0x0008A0C7 File Offset: 0x000882C7
	public bool IsLocalPlayer()
	{
		return VRRigCache.Instance.localRig.Creator.ActorNumber == this.rig.OwningNetPlayer.ActorNumber;
	}

	// Token: 0x06001C5D RID: 7261 RVA: 0x0008A0EF File Offset: 0x000882EF
	public static bool IsLeftHand(int handIndex)
	{
		return handIndex == 0;
	}

	// Token: 0x06001C5E RID: 7262 RVA: 0x0008A0F5 File Offset: 0x000882F5
	public static int GetHandIndex(bool leftHand)
	{
		if (!leftHand)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06001C5F RID: 7263 RVA: 0x0008A100 File Offset: 0x00088300
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

	// Token: 0x06001C60 RID: 7264 RVA: 0x0008A13C File Offset: 0x0008833C
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

	// Token: 0x06001C61 RID: 7265 RVA: 0x0008A168 File Offset: 0x00088368
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

	// Token: 0x04001F56 RID: 8022
	public VRRig rig;

	// Token: 0x04001F57 RID: 8023
	public int teamId;

	// Token: 0x04001F58 RID: 8024
	private GamePlayer.HandData[] hands;

	// Token: 0x04001F59 RID: 8025
	public const int MAX_HANDS = 2;

	// Token: 0x04001F5A RID: 8026
	public const int LEFT_HAND = 0;

	// Token: 0x04001F5B RID: 8027
	public const int RIGHT_HAND = 1;

	// Token: 0x04001F5C RID: 8028
	private int inGoalZone;

	// Token: 0x02000493 RID: 1171
	private struct HandData
	{
		// Token: 0x04001F5D RID: 8029
		public GameBallId grabbedGameBallId;
	}
}
