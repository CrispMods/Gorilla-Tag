using System;
using System.Collections;
using CjLib;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;

// Token: 0x020000C2 RID: 194
public class Ballista : MonoBehaviourPun
{
	// Token: 0x060004FF RID: 1279 RVA: 0x00033C1D File Offset: 0x00031E1D
	public void TriggerLoad()
	{
		this.animator.SetTrigger(this.loadTriggerHash);
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x00033C30 File Offset: 0x00031E30
	public void TriggerFire()
	{
		this.animator.SetTrigger(this.fireTriggerHash);
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000501 RID: 1281 RVA: 0x00033C43 File Offset: 0x00031E43
	private float LaunchSpeed
	{
		get
		{
			if (!this.useSpeedOptions)
			{
				return this.launchSpeed;
			}
			return this.speedOptions[this.currentSpeedIndex];
		}
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x0007FCE0 File Offset: 0x0007DEE0
	private void Awake()
	{
		this.launchDirection = this.launchEnd.position - this.launchStart.position;
		this.launchRampDistance = this.launchDirection.magnitude;
		this.launchDirection /= this.launchRampDistance;
		this.collidingLayer = LayerMask.NameToLayer("Default");
		this.notCollidingLayer = LayerMask.NameToLayer("Prop");
		this.playerPullInRate = Mathf.Exp(this.playerMagnetismStrength);
		this.animator.SetFloat(this.pitchParamHash, this.pitch);
		this.appliedAnimatorPitch = this.pitch;
		this.RefreshButtonColors();
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x0007FD90 File Offset: 0x0007DF90
	private void Update()
	{
		float deltaTime = Time.deltaTime;
		AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
		if (currentAnimatorStateInfo.shortNameHash == this.idleStateHash)
		{
			if (this.prevStateHash == this.fireStateHash)
			{
				this.fireCompleteTime = Time.time;
			}
			if (Time.time - this.fireCompleteTime > this.reloadDelay)
			{
				this.animator.SetTrigger(this.loadTriggerHash);
				this.loadStartTime = Time.time;
			}
		}
		else if (currentAnimatorStateInfo.shortNameHash == this.loadStateHash)
		{
			if (Time.time - this.loadStartTime > this.loadTime)
			{
				if (this.playerInTrigger)
				{
					GTPlayer instance = GTPlayer.Instance;
					Vector3 playerBodyCenterPosition = this.GetPlayerBodyCenterPosition(instance);
					Vector3 b = Vector3.Dot(playerBodyCenterPosition - this.launchStart.position, this.launchDirection) * this.launchDirection + this.launchStart.position;
					Vector3 b2 = playerBodyCenterPosition - b;
					Vector3 a = Vector3.Lerp(Vector3.zero, b2, Mathf.Exp(-this.playerPullInRate * deltaTime));
					instance.transform.position = instance.transform.position + (a - b2);
					this.playerReadyToFire = (a.sqrMagnitude < this.playerReadyToFireDist * this.playerReadyToFireDist);
				}
				else
				{
					this.playerReadyToFire = false;
				}
				if (this.playerReadyToFire)
				{
					if (PhotonNetwork.InRoom)
					{
						base.photonView.RPC("FireBallistaRPC", RpcTarget.Others, Array.Empty<object>());
					}
					this.FireLocal();
				}
			}
		}
		else if (currentAnimatorStateInfo.shortNameHash == this.fireStateHash && !this.playerLaunched && (this.playerReadyToFire || this.playerInTrigger))
		{
			float num = Vector3.Dot(this.launchBone.position - this.launchStart.position, this.launchDirection) / this.launchRampDistance;
			GTPlayer instance2 = GTPlayer.Instance;
			Vector3 playerBodyCenterPosition2 = this.GetPlayerBodyCenterPosition(instance2);
			float b3 = Vector3.Dot(playerBodyCenterPosition2 - this.launchStart.position, this.launchDirection) / this.launchRampDistance;
			float num2 = 0.25f / this.launchRampDistance;
			float num3 = Mathf.Max(num + num2, b3);
			float d = num3 * this.launchRampDistance;
			Vector3 a2 = this.launchDirection * d + this.launchStart.position;
			instance2.transform.position + (a2 - playerBodyCenterPosition2);
			instance2.transform.position = instance2.transform.position + (a2 - playerBodyCenterPosition2);
			instance2.SetPlayerVelocity(Vector3.zero);
			if (num3 >= 1f)
			{
				this.playerLaunched = true;
				instance2.SetPlayerVelocity(this.LaunchSpeed * this.launchDirection);
				instance2.SetMaximumSlipThisFrame();
			}
		}
		this.prevStateHash = currentAnimatorStateInfo.shortNameHash;
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x00033C61 File Offset: 0x00031E61
	private void FireLocal()
	{
		this.animator.SetTrigger(this.fireTriggerHash);
		this.playerLaunched = false;
		if (this.debugDrawTrajectoryOnLaunch)
		{
			this.DebugDrawTrajectory(8f);
		}
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x00080088 File Offset: 0x0007E288
	private Vector3 GetPlayerBodyCenterPosition(GTPlayer player)
	{
		return player.headCollider.transform.position + Quaternion.Euler(0f, player.headCollider.transform.rotation.eulerAngles.y, 0f) * new Vector3(0f, 0f, -0.15f) + Vector3.down * 0.4f;
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x00080104 File Offset: 0x0007E304
	private void OnTriggerEnter(Collider other)
	{
		GTPlayer instance = GTPlayer.Instance;
		if (instance != null && instance.bodyCollider == other)
		{
			this.playerInTrigger = true;
		}
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x00080138 File Offset: 0x0007E338
	private void OnTriggerExit(Collider other)
	{
		GTPlayer instance = GTPlayer.Instance;
		if (instance != null && instance.bodyCollider == other)
		{
			this.playerInTrigger = false;
		}
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x00033C8F File Offset: 0x00031E8F
	[PunRPC]
	public void FireBallistaRPC(PhotonMessageInfo info)
	{
		this.FireLocal();
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x0008016C File Offset: 0x0007E36C
	private void UpdatePredictionLine()
	{
		float d = 0.033333335f;
		Vector3 vector = this.launchEnd.position;
		Vector3 a = (this.launchEnd.position - this.launchStart.position).normalized * this.LaunchSpeed;
		for (int i = 0; i < 240; i++)
		{
			this.predictionLinePoints[i] = vector;
			vector += a * d;
			a += Vector3.down * 9.8f * d;
		}
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x00033C97 File Offset: 0x00031E97
	private IEnumerator DebugDrawTrajectory(float duration)
	{
		this.UpdatePredictionLine();
		float startTime = Time.time;
		while (Time.time < startTime + duration)
		{
			DebugUtil.DrawLine(this.launchStart.position, this.launchEnd.position, Color.yellow, true);
			DebugUtil.DrawLines(this.predictionLinePoints, Color.yellow, true);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x00080208 File Offset: 0x0007E408
	private void OnDrawGizmosSelected()
	{
		if (this.launchStart != null && this.launchEnd != null)
		{
			this.UpdatePredictionLine();
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(this.launchStart.position, this.launchEnd.position);
			Gizmos.DrawLineList(this.predictionLinePoints);
		}
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x0008026C File Offset: 0x0007E46C
	public void RefreshButtonColors()
	{
		this.speedZeroButton.isOn = (this.currentSpeedIndex == 0);
		this.speedZeroButton.UpdateColor();
		this.speedOneButton.isOn = (this.currentSpeedIndex == 1);
		this.speedOneButton.UpdateColor();
		this.speedTwoButton.isOn = (this.currentSpeedIndex == 2);
		this.speedTwoButton.UpdateColor();
		this.speedThreeButton.isOn = (this.currentSpeedIndex == 3);
		this.speedThreeButton.UpdateColor();
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x00033CAD File Offset: 0x00031EAD
	public void SetSpeedIndex(int index)
	{
		this.currentSpeedIndex = index;
		this.RefreshButtonColors();
	}

	// Token: 0x040005B6 RID: 1462
	public Animator animator;

	// Token: 0x040005B7 RID: 1463
	public Transform launchStart;

	// Token: 0x040005B8 RID: 1464
	public Transform launchEnd;

	// Token: 0x040005B9 RID: 1465
	public Transform launchBone;

	// Token: 0x040005BA RID: 1466
	public float reloadDelay = 1f;

	// Token: 0x040005BB RID: 1467
	public float loadTime = 1.933f;

	// Token: 0x040005BC RID: 1468
	public float playerMagnetismStrength = 3f;

	// Token: 0x040005BD RID: 1469
	public float launchSpeed = 20f;

	// Token: 0x040005BE RID: 1470
	[Range(0f, 1f)]
	public float pitch;

	// Token: 0x040005BF RID: 1471
	private bool useSpeedOptions;

	// Token: 0x040005C0 RID: 1472
	public float[] speedOptions = new float[]
	{
		10f,
		15f,
		20f,
		25f
	};

	// Token: 0x040005C1 RID: 1473
	public int currentSpeedIndex;

	// Token: 0x040005C2 RID: 1474
	public GorillaPressableButton speedZeroButton;

	// Token: 0x040005C3 RID: 1475
	public GorillaPressableButton speedOneButton;

	// Token: 0x040005C4 RID: 1476
	public GorillaPressableButton speedTwoButton;

	// Token: 0x040005C5 RID: 1477
	public GorillaPressableButton speedThreeButton;

	// Token: 0x040005C6 RID: 1478
	private bool debugDrawTrajectoryOnLaunch;

	// Token: 0x040005C7 RID: 1479
	private int loadTriggerHash = Animator.StringToHash("Load");

	// Token: 0x040005C8 RID: 1480
	private int fireTriggerHash = Animator.StringToHash("Fire");

	// Token: 0x040005C9 RID: 1481
	private int pitchParamHash = Animator.StringToHash("Pitch");

	// Token: 0x040005CA RID: 1482
	private int idleStateHash = Animator.StringToHash("Idle");

	// Token: 0x040005CB RID: 1483
	private int loadStateHash = Animator.StringToHash("Load");

	// Token: 0x040005CC RID: 1484
	private int fireStateHash = Animator.StringToHash("Fire");

	// Token: 0x040005CD RID: 1485
	private int prevStateHash = Animator.StringToHash("Idle");

	// Token: 0x040005CE RID: 1486
	private float fireCompleteTime;

	// Token: 0x040005CF RID: 1487
	private float loadStartTime;

	// Token: 0x040005D0 RID: 1488
	private bool playerInTrigger;

	// Token: 0x040005D1 RID: 1489
	private bool playerReadyToFire;

	// Token: 0x040005D2 RID: 1490
	private bool playerLaunched;

	// Token: 0x040005D3 RID: 1491
	private float playerReadyToFireDist = 0.1f;

	// Token: 0x040005D4 RID: 1492
	private Vector3 playerBodyOffsetFromHead = new Vector3(0f, -0.4f, -0.15f);

	// Token: 0x040005D5 RID: 1493
	private Vector3 launchDirection;

	// Token: 0x040005D6 RID: 1494
	private float launchRampDistance;

	// Token: 0x040005D7 RID: 1495
	private int collidingLayer;

	// Token: 0x040005D8 RID: 1496
	private int notCollidingLayer;

	// Token: 0x040005D9 RID: 1497
	private float playerPullInRate;

	// Token: 0x040005DA RID: 1498
	private float appliedAnimatorPitch;

	// Token: 0x040005DB RID: 1499
	private const int predictionLineSamples = 240;

	// Token: 0x040005DC RID: 1500
	private Vector3[] predictionLinePoints = new Vector3[240];
}
