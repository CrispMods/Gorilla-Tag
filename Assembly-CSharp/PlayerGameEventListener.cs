using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200011A RID: 282
public class PlayerGameEventListener : MonoBehaviour
{
	// Token: 0x0600078E RID: 1934 RVA: 0x00035669 File Offset: 0x00033869
	private void OnEnable()
	{
		this.SubscribeToEvents();
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x00035671 File Offset: 0x00033871
	private void OnDisable()
	{
		this.UnsubscribeFromEvents();
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x0008AF9C File Offset: 0x0008919C
	private void SubscribeToEvents()
	{
		switch (this.eventType)
		{
		case PlayerGameEvents.EventType.NONE:
			return;
		case PlayerGameEvents.EventType.GameModeObjective:
			PlayerGameEvents.OnGameModeObjectiveTrigger += this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.GameModeCompleteRound:
			PlayerGameEvents.OnGameModeCompleteRound += this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.GrabbedObject:
			PlayerGameEvents.OnGrabbedObject += this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.DroppedObject:
			PlayerGameEvents.OnDroppedObject += this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.EatObject:
			PlayerGameEvents.OnEatObject += this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.TapObject:
			PlayerGameEvents.OnTapObject += this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.LaunchedProjectile:
			PlayerGameEvents.OnLaunchedProjectile += this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.PlayerMoved:
			PlayerGameEvents.OnPlayerMoved += this.OnGameMoveEventTriggered;
			return;
		case PlayerGameEvents.EventType.PlayerSwam:
			PlayerGameEvents.OnPlayerSwam += this.OnGameMoveEventTriggered;
			return;
		case PlayerGameEvents.EventType.TriggerHandEfffect:
			PlayerGameEvents.OnTriggerHandEffect += this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.EnterLocation:
			PlayerGameEvents.OnEnterLocation += this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.MiscEvent:
			PlayerGameEvents.OnMiscEvent += this.OnGameEventTriggered;
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x0008B0D0 File Offset: 0x000892D0
	private void UnsubscribeFromEvents()
	{
		switch (this.eventType)
		{
		case PlayerGameEvents.EventType.NONE:
			return;
		case PlayerGameEvents.EventType.GameModeObjective:
			PlayerGameEvents.OnGameModeObjectiveTrigger -= this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.GameModeCompleteRound:
			PlayerGameEvents.OnGameModeCompleteRound -= this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.GrabbedObject:
			PlayerGameEvents.OnGrabbedObject -= this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.DroppedObject:
			PlayerGameEvents.OnDroppedObject -= this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.EatObject:
			PlayerGameEvents.OnEatObject -= this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.TapObject:
			PlayerGameEvents.OnTapObject -= this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.LaunchedProjectile:
			PlayerGameEvents.OnLaunchedProjectile -= this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.PlayerMoved:
			PlayerGameEvents.OnPlayerMoved -= this.OnGameMoveEventTriggered;
			return;
		case PlayerGameEvents.EventType.PlayerSwam:
			PlayerGameEvents.OnPlayerSwam -= this.OnGameMoveEventTriggered;
			return;
		case PlayerGameEvents.EventType.TriggerHandEfffect:
			PlayerGameEvents.OnTriggerHandEffect -= this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.EnterLocation:
			PlayerGameEvents.OnEnterLocation -= this.OnGameEventTriggered;
			return;
		case PlayerGameEvents.EventType.MiscEvent:
			PlayerGameEvents.OnMiscEvent -= this.OnGameEventTriggered;
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x00035679 File Offset: 0x00033879
	private void OnGameMoveEventTriggered(float distance, float speed)
	{
		Debug.LogError("Movement events not supported - please implement");
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x0008B204 File Offset: 0x00089404
	public void OnGameEventTriggered(string eventName)
	{
		if (!string.IsNullOrEmpty(this.filter) && !eventName.StartsWith(this.filter))
		{
			return;
		}
		if (this._cooldownEnd > Time.time)
		{
			return;
		}
		this._cooldownEnd = Time.time + this.cooldown;
		UnityEvent unityEvent = this.onGameEvent;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x040008F0 RID: 2288
	[SerializeField]
	private PlayerGameEvents.EventType eventType;

	// Token: 0x040008F1 RID: 2289
	[Tooltip("Cooldown in seconds")]
	[SerializeField]
	private string filter;

	// Token: 0x040008F2 RID: 2290
	[SerializeField]
	private float cooldown = 1f;

	// Token: 0x040008F3 RID: 2291
	[SerializeField]
	private UnityEvent onGameEvent;

	// Token: 0x040008F4 RID: 2292
	private float _cooldownEnd;
}
