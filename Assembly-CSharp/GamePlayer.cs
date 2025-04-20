using System;
using UnityEngine;

// Token: 0x0200049E RID: 1182
public class GamePlayer : MonoBehaviour
{
	// Token: 0x06001CA5 RID: 7333 RVA: 0x000DD7EC File Offset: 0x000DB9EC
	private void Awake()
	{
		this.hands = new GamePlayer.HandData[2];
		for (int i = 0; i < 2; i++)
		{
			this.ClearGrabbed(i);
		}
		this.teamId = -1;
	}

	// Token: 0x06001CA6 RID: 7334 RVA: 0x000DD820 File Offset: 0x000DBA20
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

	// Token: 0x06001CA7 RID: 7335 RVA: 0x000DD874 File Offset: 0x000DBA74
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

	// Token: 0x06001CA8 RID: 7336 RVA: 0x000DD8B4 File Offset: 0x000DBAB4
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

	// Token: 0x06001CA9 RID: 7337 RVA: 0x00043B5B File Offset: 0x00041D5B
	public void ClearGrabbed(int handIndex)
	{
		this.SetGrabbed(GameBallId.Invalid, handIndex);
	}

	// Token: 0x06001CAA RID: 7338 RVA: 0x000DD8F0 File Offset: 0x000DBAF0
	public void ClearAllGrabbed()
	{
		for (int i = 0; i < this.hands.Length; i++)
		{
			this.ClearGrabbed(i);
		}
	}

	// Token: 0x06001CAB RID: 7339 RVA: 0x00043B69 File Offset: 0x00041D69
	public void SetInGoalZone(bool inZone)
	{
		if (inZone)
		{
			this.inGoalZone++;
			return;
		}
		this.inGoalZone--;
	}

	// Token: 0x06001CAC RID: 7340 RVA: 0x000DD918 File Offset: 0x000DBB18
	public bool IsHoldingBall()
	{
		return this.GetGameBallId().IsValid();
	}

	// Token: 0x06001CAD RID: 7341 RVA: 0x00043B8B File Offset: 0x00041D8B
	public GameBallId GetGameBallId(int handIndex)
	{
		return this.hands[handIndex].grabbedGameBallId;
	}

	// Token: 0x06001CAE RID: 7342 RVA: 0x000DD934 File Offset: 0x000DBB34
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

	// Token: 0x06001CAF RID: 7343 RVA: 0x000DD970 File Offset: 0x000DBB70
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

	// Token: 0x06001CB0 RID: 7344 RVA: 0x00043B9E File Offset: 0x00041D9E
	public bool IsLocalPlayer()
	{
		return VRRigCache.Instance.localRig.Creator.ActorNumber == this.rig.OwningNetPlayer.ActorNumber;
	}

	// Token: 0x06001CB1 RID: 7345 RVA: 0x00043BC6 File Offset: 0x00041DC6
	public static bool IsLeftHand(int handIndex)
	{
		return handIndex == 0;
	}

	// Token: 0x06001CB2 RID: 7346 RVA: 0x00043BCC File Offset: 0x00041DCC
	public static int GetHandIndex(bool leftHand)
	{
		if (!leftHand)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06001CB3 RID: 7347 RVA: 0x000DD9C0 File Offset: 0x000DBBC0
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

	// Token: 0x06001CB4 RID: 7348 RVA: 0x000DD9FC File Offset: 0x000DBBFC
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

	// Token: 0x06001CB5 RID: 7349 RVA: 0x000DDA28 File Offset: 0x000DBC28
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

	// Token: 0x04001FA5 RID: 8101
	public VRRig rig;

	// Token: 0x04001FA6 RID: 8102
	public int teamId;

	// Token: 0x04001FA7 RID: 8103
	private GamePlayer.HandData[] hands;

	// Token: 0x04001FA8 RID: 8104
	public const int MAX_HANDS = 2;

	// Token: 0x04001FA9 RID: 8105
	public const int LEFT_HAND = 0;

	// Token: 0x04001FAA RID: 8106
	public const int RIGHT_HAND = 1;

	// Token: 0x04001FAB RID: 8107
	private int inGoalZone;

	// Token: 0x0200049F RID: 1183
	private struct HandData
	{
		// Token: 0x04001FAC RID: 8108
		public GameBallId grabbedGameBallId;
	}
}
