using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000499 RID: 1177
public class MonkeBall : MonoBehaviour
{
	// Token: 0x06001C7E RID: 7294 RVA: 0x00042952 File Offset: 0x00040B52
	private void Start()
	{
		this.Refresh();
	}

	// Token: 0x06001C7F RID: 7295 RVA: 0x000DB444 File Offset: 0x000D9644
	private void Update()
	{
		this.UpdateVisualOffset();
		if (!PhotonNetwork.IsMasterClient)
		{
			if (this._resyncPosition)
			{
				this._resyncDelay -= Time.deltaTime;
				if (this._resyncDelay <= 0f)
				{
					this._resyncPosition = false;
					GameBallManager.Instance.RequestSetBallPosition(this.gameBall.id);
				}
			}
			if (this._positionFailsafe)
			{
				if (base.transform.position.y < -500f || (GameBallManager.Instance.transform.position - base.transform.position).sqrMagnitude > 6400f)
				{
					if (PhotonNetwork.IsConnected)
					{
						GameBallManager.Instance.RequestSetBallPosition(this.gameBall.id);
					}
					else
					{
						base.transform.position = GameBallManager.Instance.transform.position;
					}
					this._positionFailsafe = false;
					this._positionFailsafeTimer = 3f;
					return;
				}
			}
			else
			{
				this._positionFailsafeTimer -= Time.deltaTime;
				if (this._positionFailsafeTimer <= 0f)
				{
					this._positionFailsafe = true;
				}
			}
			return;
		}
		if (this.gameBall.onlyGrabTeamId != -1 && Time.timeAsDouble >= this.restrictTeamGrabEndTime)
		{
			MonkeBallGame.Instance.RequestRestrictBallToTeam(this.gameBall.id, -1);
		}
		if (this.AlreadyDropped())
		{
			this._droppedTimer += Time.deltaTime;
			if (this._droppedTimer >= 7.5f)
			{
				this._droppedTimer = 0f;
				GameBallManager.Instance.RequestTeleportBall(this.gameBall.id, base.transform.position, base.transform.rotation, this._rigidBody.velocity, this._rigidBody.angularVelocity);
			}
		}
		if (this._resyncPosition)
		{
			this._resyncDelay -= Time.deltaTime;
			if (this._resyncDelay <= 0f)
			{
				this._resyncPosition = false;
				GameBallManager.Instance.RequestTeleportBall(this.gameBall.id, base.transform.position, base.transform.rotation, this._rigidBody.velocity, this._rigidBody.angularVelocity);
			}
		}
		if (this._positionFailsafe)
		{
			if (base.transform.position.y < -250f || (GameBallManager.Instance.transform.position - base.transform.position).sqrMagnitude > 6400f)
			{
				MonkeBallGame.Instance.LaunchBallNeutral(this.gameBall.id);
				this._positionFailsafe = false;
				this._positionFailsafeTimer = 3f;
				return;
			}
		}
		else
		{
			this._positionFailsafeTimer -= Time.deltaTime;
			if (this._positionFailsafeTimer <= 0f)
			{
				this._positionFailsafe = true;
			}
		}
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x000DB724 File Offset: 0x000D9924
	public void OnCollisionEnter(Collision collision)
	{
		if (this.AlreadyDropped())
		{
			return;
		}
		if (MonkeBall.IsGamePlayer(collision.collider))
		{
			return;
		}
		this.alreadyDropped = true;
		this._droppedTimer = 0f;
		this.gameBall.PlayBounceFX();
		if (!PhotonNetwork.IsMasterClient)
		{
			if (this._rigidBody.velocity.sqrMagnitude > 1f)
			{
				this._resyncPosition = true;
				this._resyncDelay = 1.5f;
			}
			int lastHeldByActorNumber = this.gameBall.lastHeldByActorNumber;
			int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
			return;
		}
		if (this._rigidBody.velocity.sqrMagnitude > 1f)
		{
			this._resyncPosition = true;
			this._resyncDelay = 0.5f;
		}
		if (this._launchAfterScore)
		{
			this._launchAfterScore = false;
			MonkeBallGame.Instance.RequestRestrictBallToTeamOnScore(this.gameBall.id, MonkeBallGame.Instance.GetOtherTeam(this.gameBall.lastHeldByTeamId));
			return;
		}
		MonkeBallGame.Instance.RequestRestrictBallToTeam(this.gameBall.id, MonkeBallGame.Instance.GetOtherTeam(this.gameBall.lastHeldByTeamId));
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x0004295A File Offset: 0x00040B5A
	public void TriggerDelayedResync()
	{
		this._resyncPosition = true;
		if (PhotonNetwork.IsMasterClient)
		{
			this._resyncDelay = 0.5f;
			return;
		}
		this._resyncDelay = 1.5f;
	}

	// Token: 0x06001C82 RID: 7298 RVA: 0x00042981 File Offset: 0x00040B81
	public void SetRigidbodyDiscrete()
	{
		this._rigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x0004298F File Offset: 0x00040B8F
	public void SetRigidbodyContinuous()
	{
		this._rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x0004299D File Offset: 0x00040B9D
	public static MonkeBall Get(GameBall ball)
	{
		if (ball == null)
		{
			return null;
		}
		return ball.GetComponent<MonkeBall>();
	}

	// Token: 0x06001C85 RID: 7301 RVA: 0x000429B0 File Offset: 0x00040BB0
	public bool AlreadyDropped()
	{
		return this.alreadyDropped;
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x000429B8 File Offset: 0x00040BB8
	public void OnGrabbed()
	{
		this.alreadyDropped = false;
		this._resyncPosition = false;
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x000429C8 File Offset: 0x00040BC8
	public void OnSwitchHeldByTeam(int teamId)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			MonkeBallGame.Instance.RequestRestrictBallToTeam(this.gameBall.id, teamId);
		}
	}

	// Token: 0x06001C88 RID: 7304 RVA: 0x000429E7 File Offset: 0x00040BE7
	public void ClearCannotGrabTeamId()
	{
		this.gameBall.onlyGrabTeamId = -1;
		this.restrictTeamGrabEndTime = -1.0;
		this.Refresh();
	}

	// Token: 0x06001C89 RID: 7305 RVA: 0x000DB844 File Offset: 0x000D9A44
	public bool RestrictBallToTeam(int teamId, float duration)
	{
		if (teamId == this.gameBall.onlyGrabTeamId && Time.timeAsDouble + (double)duration < this.restrictTeamGrabEndTime)
		{
			return false;
		}
		this.gameBall.onlyGrabTeamId = teamId;
		this.restrictTeamGrabEndTime = Time.timeAsDouble + (double)duration;
		this.Refresh();
		return true;
	}

	// Token: 0x06001C8A RID: 7306 RVA: 0x00042A0A File Offset: 0x00040C0A
	private void Refresh()
	{
		if (this.gameBall.onlyGrabTeamId == -1)
		{
			this.mainRenderer.material = this.defaultMaterial;
			return;
		}
		this.mainRenderer.material = this.teamMaterial[this.gameBall.onlyGrabTeamId];
	}

	// Token: 0x06001C8B RID: 7307 RVA: 0x00042A49 File Offset: 0x00040C49
	private static bool IsGamePlayer(Collider collider)
	{
		return GamePlayer.GetGamePlayer(collider, false) != null;
	}

	// Token: 0x06001C8C RID: 7308 RVA: 0x000DB894 File Offset: 0x000D9A94
	public void SetVisualOffset(bool detach)
	{
		if (detach)
		{
			this.lastVisiblePosition = this.mainRenderer.transform.position;
			this._visualOffset = true;
			this._timeOffset = Time.time;
			this.mainRenderer.transform.SetParent(null, true);
			return;
		}
		this.ReattachVisuals();
	}

	// Token: 0x06001C8D RID: 7309 RVA: 0x000DB8E8 File Offset: 0x000D9AE8
	private void ReattachVisuals()
	{
		if (!this._visualOffset)
		{
			return;
		}
		this.mainRenderer.transform.SetParent(base.transform);
		this.mainRenderer.transform.localPosition = Vector3.zero;
		this.mainRenderer.transform.localRotation = Quaternion.identity;
		this._visualOffset = false;
	}

	// Token: 0x06001C8E RID: 7310 RVA: 0x000DB948 File Offset: 0x000D9B48
	private void UpdateVisualOffset()
	{
		if (this._visualOffset)
		{
			this.mainRenderer.transform.position = Vector3.Lerp(this.mainRenderer.transform.position, this._rigidBody.position, Mathf.Clamp((Time.time - this._timeOffset) / this.maxLerpTime, this.offsetLerp, 1f));
			if ((this.mainRenderer.transform.position - this._rigidBody.position).sqrMagnitude < this._offsetThreshold)
			{
				this.ReattachVisuals();
			}
		}
	}

	// Token: 0x04001F72 RID: 8050
	public GameBall gameBall;

	// Token: 0x04001F73 RID: 8051
	public MeshRenderer mainRenderer;

	// Token: 0x04001F74 RID: 8052
	public Material defaultMaterial;

	// Token: 0x04001F75 RID: 8053
	public Material[] teamMaterial;

	// Token: 0x04001F76 RID: 8054
	public double restrictTeamGrabEndTime;

	// Token: 0x04001F77 RID: 8055
	public bool alreadyDropped;

	// Token: 0x04001F78 RID: 8056
	private bool _launchAfterScore;

	// Token: 0x04001F79 RID: 8057
	private float _droppedTimer;

	// Token: 0x04001F7A RID: 8058
	private bool _resyncPosition;

	// Token: 0x04001F7B RID: 8059
	private float _resyncDelay;

	// Token: 0x04001F7C RID: 8060
	private bool _visualOffset;

	// Token: 0x04001F7D RID: 8061
	private float _offsetThreshold = 0.05f;

	// Token: 0x04001F7E RID: 8062
	private float _timeOffset;

	// Token: 0x04001F7F RID: 8063
	public float maxLerpTime = 0.5f;

	// Token: 0x04001F80 RID: 8064
	public float offsetLerp = 0.2f;

	// Token: 0x04001F81 RID: 8065
	private bool _positionFailsafe = true;

	// Token: 0x04001F82 RID: 8066
	private float _positionFailsafeTimer;

	// Token: 0x04001F83 RID: 8067
	public Vector3 lastVisiblePosition;

	// Token: 0x04001F84 RID: 8068
	[SerializeField]
	private Rigidbody _rigidBody;
}
