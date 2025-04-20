using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x020004A5 RID: 1189
public class MonkeBall : MonoBehaviour
{
	// Token: 0x06001CCF RID: 7375 RVA: 0x00043C8B File Offset: 0x00041E8B
	private void Start()
	{
		this.Refresh();
	}

	// Token: 0x06001CD0 RID: 7376 RVA: 0x000DE0F4 File Offset: 0x000DC2F4
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

	// Token: 0x06001CD1 RID: 7377 RVA: 0x000DE3D4 File Offset: 0x000DC5D4
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

	// Token: 0x06001CD2 RID: 7378 RVA: 0x00043C93 File Offset: 0x00041E93
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

	// Token: 0x06001CD3 RID: 7379 RVA: 0x00043CBA File Offset: 0x00041EBA
	public void SetRigidbodyDiscrete()
	{
		this._rigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
	}

	// Token: 0x06001CD4 RID: 7380 RVA: 0x00043CC8 File Offset: 0x00041EC8
	public void SetRigidbodyContinuous()
	{
		this._rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
	}

	// Token: 0x06001CD5 RID: 7381 RVA: 0x00043CD6 File Offset: 0x00041ED6
	public static MonkeBall Get(GameBall ball)
	{
		if (ball == null)
		{
			return null;
		}
		return ball.GetComponent<MonkeBall>();
	}

	// Token: 0x06001CD6 RID: 7382 RVA: 0x00043CE9 File Offset: 0x00041EE9
	public bool AlreadyDropped()
	{
		return this.alreadyDropped;
	}

	// Token: 0x06001CD7 RID: 7383 RVA: 0x00043CF1 File Offset: 0x00041EF1
	public void OnGrabbed()
	{
		this.alreadyDropped = false;
		this._resyncPosition = false;
	}

	// Token: 0x06001CD8 RID: 7384 RVA: 0x00043D01 File Offset: 0x00041F01
	public void OnSwitchHeldByTeam(int teamId)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			MonkeBallGame.Instance.RequestRestrictBallToTeam(this.gameBall.id, teamId);
		}
	}

	// Token: 0x06001CD9 RID: 7385 RVA: 0x00043D20 File Offset: 0x00041F20
	public void ClearCannotGrabTeamId()
	{
		this.gameBall.onlyGrabTeamId = -1;
		this.restrictTeamGrabEndTime = -1.0;
		this.Refresh();
	}

	// Token: 0x06001CDA RID: 7386 RVA: 0x000DE4F4 File Offset: 0x000DC6F4
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

	// Token: 0x06001CDB RID: 7387 RVA: 0x00043D43 File Offset: 0x00041F43
	private void Refresh()
	{
		if (this.gameBall.onlyGrabTeamId == -1)
		{
			this.mainRenderer.material = this.defaultMaterial;
			return;
		}
		this.mainRenderer.material = this.teamMaterial[this.gameBall.onlyGrabTeamId];
	}

	// Token: 0x06001CDC RID: 7388 RVA: 0x00043D82 File Offset: 0x00041F82
	private static bool IsGamePlayer(Collider collider)
	{
		return GamePlayer.GetGamePlayer(collider, false) != null;
	}

	// Token: 0x06001CDD RID: 7389 RVA: 0x000DE544 File Offset: 0x000DC744
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

	// Token: 0x06001CDE RID: 7390 RVA: 0x000DE598 File Offset: 0x000DC798
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

	// Token: 0x06001CDF RID: 7391 RVA: 0x000DE5F8 File Offset: 0x000DC7F8
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

	// Token: 0x04001FC0 RID: 8128
	public GameBall gameBall;

	// Token: 0x04001FC1 RID: 8129
	public MeshRenderer mainRenderer;

	// Token: 0x04001FC2 RID: 8130
	public Material defaultMaterial;

	// Token: 0x04001FC3 RID: 8131
	public Material[] teamMaterial;

	// Token: 0x04001FC4 RID: 8132
	public double restrictTeamGrabEndTime;

	// Token: 0x04001FC5 RID: 8133
	public bool alreadyDropped;

	// Token: 0x04001FC6 RID: 8134
	private bool _launchAfterScore;

	// Token: 0x04001FC7 RID: 8135
	private float _droppedTimer;

	// Token: 0x04001FC8 RID: 8136
	private bool _resyncPosition;

	// Token: 0x04001FC9 RID: 8137
	private float _resyncDelay;

	// Token: 0x04001FCA RID: 8138
	private bool _visualOffset;

	// Token: 0x04001FCB RID: 8139
	private float _offsetThreshold = 0.05f;

	// Token: 0x04001FCC RID: 8140
	private float _timeOffset;

	// Token: 0x04001FCD RID: 8141
	public float maxLerpTime = 0.5f;

	// Token: 0x04001FCE RID: 8142
	public float offsetLerp = 0.2f;

	// Token: 0x04001FCF RID: 8143
	private bool _positionFailsafe = true;

	// Token: 0x04001FD0 RID: 8144
	private float _positionFailsafeTimer;

	// Token: 0x04001FD1 RID: 8145
	public Vector3 lastVisiblePosition;

	// Token: 0x04001FD2 RID: 8146
	[SerializeField]
	private Rigidbody _rigidBody;
}
