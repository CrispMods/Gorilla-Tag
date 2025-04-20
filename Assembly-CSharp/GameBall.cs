using System;
using UnityEngine;

// Token: 0x02000499 RID: 1177
public class GameBall : MonoBehaviour
{
	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06001C72 RID: 7282 RVA: 0x000439BA File Offset: 0x00041BBA
	public bool IsLaunched
	{
		get
		{
			return this._launched;
		}
	}

	// Token: 0x06001C73 RID: 7283 RVA: 0x000DC354 File Offset: 0x000DA554
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

	// Token: 0x06001C74 RID: 7284 RVA: 0x000DC3E8 File Offset: 0x000DA5E8
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

	// Token: 0x06001C75 RID: 7285 RVA: 0x000439C2 File Offset: 0x00041BC2
	public void WasLaunched()
	{
		this._launched = true;
		this.collider.isTrigger = true;
		this._launchedTimer = 0f;
	}

	// Token: 0x06001C76 RID: 7286 RVA: 0x000439E2 File Offset: 0x00041BE2
	public Vector3 GetVelocity()
	{
		if (this.rigidBody == null)
		{
			return Vector3.zero;
		}
		return this.rigidBody.velocity;
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x00043A03 File Offset: 0x00041C03
	public void SetVelocity(Vector3 velocity)
	{
		this.rigidBody.velocity = velocity;
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x000DC4A0 File Offset: 0x000DA6A0
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

	// Token: 0x06001C79 RID: 7289 RVA: 0x00043A11 File Offset: 0x00041C11
	public void PlayThrowFx()
	{
		if (this.audioSource != null)
		{
			this.audioSource.clip = this.throwSound;
			this.audioSource.volume = this.throwSoundVolume;
			this.audioSource.Play();
		}
	}

	// Token: 0x06001C7A RID: 7290 RVA: 0x00043A4E File Offset: 0x00041C4E
	public void PlayBounceFX()
	{
		if (this.audioSource != null)
		{
			this.audioSource.clip = this.groundSound;
			this.audioSource.volume = this.groundSoundVolume;
			this.audioSource.Play();
		}
	}

	// Token: 0x06001C7B RID: 7291 RVA: 0x00043A8B File Offset: 0x00041C8B
	public void SetHeldByTeamId(int teamId)
	{
		this.lastHeldByTeamId = teamId;
	}

	// Token: 0x06001C7C RID: 7292 RVA: 0x00043A94 File Offset: 0x00041C94
	private bool IsGamePlayer(Collider collider)
	{
		return GamePlayer.GetGamePlayer(collider, false) != null;
	}

	// Token: 0x06001C7D RID: 7293 RVA: 0x00043AA3 File Offset: 0x00041CA3
	public void SetVisualOffset(bool detach)
	{
		if (this._monkeBall != null)
		{
			this._monkeBall.SetVisualOffset(detach);
		}
	}

	// Token: 0x04001F75 RID: 8053
	public GameBallId id;

	// Token: 0x04001F76 RID: 8054
	public float gravityMult = 1f;

	// Token: 0x04001F77 RID: 8055
	public bool disc;

	// Token: 0x04001F78 RID: 8056
	public Vector3 localDiscUp;

	// Token: 0x04001F79 RID: 8057
	public AudioSource audioSource;

	// Token: 0x04001F7A RID: 8058
	public AudioClip catchSound;

	// Token: 0x04001F7B RID: 8059
	public float catchSoundVolume;

	// Token: 0x04001F7C RID: 8060
	private float _catchSoundDecay;

	// Token: 0x04001F7D RID: 8061
	public AudioClip throwSound;

	// Token: 0x04001F7E RID: 8062
	public float throwSoundVolume;

	// Token: 0x04001F7F RID: 8063
	public AudioClip groundSound;

	// Token: 0x04001F80 RID: 8064
	public float groundSoundVolume;

	// Token: 0x04001F81 RID: 8065
	[SerializeField]
	private Rigidbody rigidBody;

	// Token: 0x04001F82 RID: 8066
	[SerializeField]
	private Collider collider;

	// Token: 0x04001F83 RID: 8067
	public int heldByActorNumber;

	// Token: 0x04001F84 RID: 8068
	public int lastHeldByActorNumber;

	// Token: 0x04001F85 RID: 8069
	public int lastHeldByTeamId;

	// Token: 0x04001F86 RID: 8070
	public int onlyGrabTeamId;

	// Token: 0x04001F87 RID: 8071
	private bool _launched;

	// Token: 0x04001F88 RID: 8072
	private float _launchedTimer;

	// Token: 0x04001F89 RID: 8073
	public MonkeBall _monkeBall;
}
