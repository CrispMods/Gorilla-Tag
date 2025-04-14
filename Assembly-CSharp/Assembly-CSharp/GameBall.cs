using System;
using UnityEngine;

// Token: 0x0200048D RID: 1165
public class GameBall : MonoBehaviour
{
	// Token: 0x17000310 RID: 784
	// (get) Token: 0x06001C21 RID: 7201 RVA: 0x00088BFE File Offset: 0x00086DFE
	public bool IsLaunched
	{
		get
		{
			return this._launched;
		}
	}

	// Token: 0x06001C22 RID: 7202 RVA: 0x00088C08 File Offset: 0x00086E08
	private void Awake()
	{
		this.id = GameBallId.Invalid;
		if (this.rigidBody == null)
		{
			this.rigidBody = base.GetComponent<Rigidbody>();
		}
		if (this.collider == null)
		{
			this.collider = base.GetComponent<Collider>();
		}
		if (this.disc && this.rigidBody != null)
		{
			this.rigidBody.maxAngularVelocity = 28f;
		}
		this.heldByActorNumber = -1;
		this.lastHeldByTeamId = -1;
		this.onlyGrabTeamId = -1;
		this._monkeBall = base.GetComponent<MonkeBall>();
	}

	// Token: 0x06001C23 RID: 7203 RVA: 0x00088C9C File Offset: 0x00086E9C
	private void FixedUpdate()
	{
		if (this.rigidBody == null)
		{
			return;
		}
		if (this._launched)
		{
			this._launchedTimer += Time.fixedDeltaTime;
			if (this.collider.isTrigger && this._launchedTimer > 1f && this.rigidBody.velocity.y <= 0f)
			{
				this._launched = false;
				this.collider.isTrigger = false;
			}
		}
		Vector3 force = -Physics.gravity * (1f - this.gravityMult);
		this.rigidBody.AddForce(force, ForceMode.Acceleration);
		this._catchSoundDecay -= Time.deltaTime;
	}

	// Token: 0x06001C24 RID: 7204 RVA: 0x00088D51 File Offset: 0x00086F51
	public void WasLaunched()
	{
		this._launched = true;
		this.collider.isTrigger = true;
		this._launchedTimer = 0f;
	}

	// Token: 0x06001C25 RID: 7205 RVA: 0x00088D71 File Offset: 0x00086F71
	public Vector3 GetVelocity()
	{
		if (this.rigidBody == null)
		{
			return Vector3.zero;
		}
		return this.rigidBody.velocity;
	}

	// Token: 0x06001C26 RID: 7206 RVA: 0x00088D92 File Offset: 0x00086F92
	public void SetVelocity(Vector3 velocity)
	{
		this.rigidBody.velocity = velocity;
	}

	// Token: 0x06001C27 RID: 7207 RVA: 0x00088DA0 File Offset: 0x00086FA0
	public void PlayCatchFx()
	{
		if (this.audioSource != null && this._catchSoundDecay <= 0f)
		{
			this.audioSource.clip = this.catchSound;
			this.audioSource.volume = this.catchSoundVolume;
			this.audioSource.Play();
			this._catchSoundDecay = 0.1f;
		}
	}

	// Token: 0x06001C28 RID: 7208 RVA: 0x00088E00 File Offset: 0x00087000
	public void PlayThrowFx()
	{
		if (this.audioSource != null)
		{
			this.audioSource.clip = this.throwSound;
			this.audioSource.volume = this.throwSoundVolume;
			this.audioSource.Play();
		}
	}

	// Token: 0x06001C29 RID: 7209 RVA: 0x00088E3D File Offset: 0x0008703D
	public void PlayBounceFX()
	{
		if (this.audioSource != null)
		{
			this.audioSource.clip = this.groundSound;
			this.audioSource.volume = this.groundSoundVolume;
			this.audioSource.Play();
		}
	}

	// Token: 0x06001C2A RID: 7210 RVA: 0x00088E7A File Offset: 0x0008707A
	public void SetHeldByTeamId(int teamId)
	{
		this.lastHeldByTeamId = teamId;
	}

	// Token: 0x06001C2B RID: 7211 RVA: 0x00088E83 File Offset: 0x00087083
	private bool IsGamePlayer(Collider collider)
	{
		return GamePlayer.GetGamePlayer(collider, false) != null;
	}

	// Token: 0x06001C2C RID: 7212 RVA: 0x00088E92 File Offset: 0x00087092
	public void SetVisualOffset(bool detach)
	{
		if (this._monkeBall != null)
		{
			this._monkeBall.SetVisualOffset(detach);
		}
	}

	// Token: 0x04001F27 RID: 7975
	public GameBallId id;

	// Token: 0x04001F28 RID: 7976
	public float gravityMult = 1f;

	// Token: 0x04001F29 RID: 7977
	public bool disc;

	// Token: 0x04001F2A RID: 7978
	public Vector3 localDiscUp;

	// Token: 0x04001F2B RID: 7979
	public AudioSource audioSource;

	// Token: 0x04001F2C RID: 7980
	public AudioClip catchSound;

	// Token: 0x04001F2D RID: 7981
	public float catchSoundVolume;

	// Token: 0x04001F2E RID: 7982
	private float _catchSoundDecay;

	// Token: 0x04001F2F RID: 7983
	public AudioClip throwSound;

	// Token: 0x04001F30 RID: 7984
	public float throwSoundVolume;

	// Token: 0x04001F31 RID: 7985
	public AudioClip groundSound;

	// Token: 0x04001F32 RID: 7986
	public float groundSoundVolume;

	// Token: 0x04001F33 RID: 7987
	[SerializeField]
	private Rigidbody rigidBody;

	// Token: 0x04001F34 RID: 7988
	[SerializeField]
	private Collider collider;

	// Token: 0x04001F35 RID: 7989
	public int heldByActorNumber;

	// Token: 0x04001F36 RID: 7990
	public int lastHeldByActorNumber;

	// Token: 0x04001F37 RID: 7991
	public int lastHeldByTeamId;

	// Token: 0x04001F38 RID: 7992
	public int onlyGrabTeamId;

	// Token: 0x04001F39 RID: 7993
	private bool _launched;

	// Token: 0x04001F3A RID: 7994
	private float _launchedTimer;

	// Token: 0x04001F3B RID: 7995
	public MonkeBall _monkeBall;
}
