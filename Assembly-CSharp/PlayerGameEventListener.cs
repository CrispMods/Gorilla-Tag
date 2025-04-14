using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000110 RID: 272
public class PlayerGameEventListener : MonoBehaviour
{
	// Token: 0x0600074D RID: 1869 RVA: 0x00029462 File Offset: 0x00027662
	private void OnEnable()
	{
		this.SubscribeToEvents();
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x0002946A File Offset: 0x0002766A
	private void OnDisable()
	{
		this.UnsubscribeFromEvents();
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x00029474 File Offset: 0x00027674
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

	// Token: 0x06000750 RID: 1872 RVA: 0x000295A8 File Offset: 0x000277A8
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

	// Token: 0x06000751 RID: 1873 RVA: 0x000296D9 File Offset: 0x000278D9
	private void OnGameMoveEventTriggered(float distance, float speed)
	{
		Debug.LogError("Movement events not supported - please implement");
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x000296E8 File Offset: 0x000278E8
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

	// Token: 0x040008AF RID: 2223
	[SerializeField]
	private PlayerGameEvents.EventType eventType;

	// Token: 0x040008B0 RID: 2224
	[Tooltip("Cooldown in seconds")]
	[SerializeField]
	private string filter;

	// Token: 0x040008B1 RID: 2225
	[SerializeField]
	private float cooldown = 1f;

	// Token: 0x040008B2 RID: 2226
	[SerializeField]
	private UnityEvent onGameEvent;

	// Token: 0x040008B3 RID: 2227
	private float _cooldownEnd;
}
