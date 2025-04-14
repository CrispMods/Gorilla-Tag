using System;
using System.Collections;
using CjLib;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;

// Token: 0x020000B8 RID: 184
public class Ballista : MonoBehaviourPun
{
	// Token: 0x060004C3 RID: 1219 RVA: 0x0001C869 File Offset: 0x0001AA69
	public void TriggerLoad()
	{
		this.animator.SetTrigger(this.loadTriggerHash);
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x0001C87C File Offset: 0x0001AA7C
	public void TriggerFire()
	{
		this.animator.SetTrigger(this.fireTriggerHash);
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x060004C5 RID: 1221 RVA: 0x0001C88F File Offset: 0x0001AA8F
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

	// Token: 0x060004C6 RID: 1222 RVA: 0x0001C8B0 File Offset: 0x0001AAB0
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

	// Token: 0x060004C7 RID: 1223 RVA: 0x0001C960 File Offset: 0x0001AB60
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

	// Token: 0x060004C8 RID: 1224 RVA: 0x0001CC55 File Offset: 0x0001AE55
	private void FireLocal()
	{
		this.animator.SetTrigger(this.fireTriggerHash);
		this.playerLaunched = false;
		if (this.debugDrawTrajectoryOnLaunch)
		{
			this.DebugDrawTrajectory(8f);
		}
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x0001CC84 File Offset: 0x0001AE84
	private Vector3 GetPlayerBodyCenterPosition(GTPlayer player)
	{
		return player.headCollider.transform.position + Quaternion.Euler(0f, player.headCollider.transform.rotation.eulerAngles.y, 0f) * new Vector3(0f, 0f, -0.15f) + Vector3.down * 0.4f;
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x0001CD00 File Offset: 0x0001AF00
	private void OnTriggerEnter(Collider other)
	{
		GTPlayer instance = GTPlayer.Instance;
		if (instance != null && instance.bodyCollider == other)
		{
			this.playerInTrigger = true;
		}
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x0001CD34 File Offset: 0x0001AF34
	private void OnTriggerExit(Collider other)
	{
		GTPlayer instance = GTPlayer.Instance;
		if (instance != null && instance.bodyCollider == other)
		{
			this.playerInTrigger = false;
		}
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x0001CD65 File Offset: 0x0001AF65
	[PunRPC]
	public void FireBallistaRPC(PhotonMessageInfo info)
	{
		this.FireLocal();
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x0001CD70 File Offset: 0x0001AF70
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

	// Token: 0x060004CE RID: 1230 RVA: 0x0001CE0A File Offset: 0x0001B00A
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

	// Token: 0x060004CF RID: 1231 RVA: 0x0001CE20 File Offset: 0x0001B020
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

	// Token: 0x060004D0 RID: 1232 RVA: 0x0001CE84 File Offset: 0x0001B084
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

	// Token: 0x060004D1 RID: 1233 RVA: 0x0001CF0D File Offset: 0x0001B10D
	public void SetSpeedIndex(int index)
	{
		this.currentSpeedIndex = index;
		this.RefreshButtonColors();
	}

	// Token: 0x04000576 RID: 1398
	public Animator animator;

	// Token: 0x04000577 RID: 1399
	public Transform launchStart;

	// Token: 0x04000578 RID: 1400
	public Transform launchEnd;

	// Token: 0x04000579 RID: 1401
	public Transform launchBone;

	// Token: 0x0400057A RID: 1402
	public float reloadDelay = 1f;

	// Token: 0x0400057B RID: 1403
	public float loadTime = 1.933f;

	// Token: 0x0400057C RID: 1404
	public float playerMagnetismStrength = 3f;

	// Token: 0x0400057D RID: 1405
	public float launchSpeed = 20f;

	// Token: 0x0400057E RID: 1406
	[Range(0f, 1f)]
	public float pitch;

	// Token: 0x0400057F RID: 1407
	private bool useSpeedOptions;

	// Token: 0x04000580 RID: 1408
	public float[] speedOptions = new float[]
	{
		10f,
		15f,
		20f,
		25f
	};

	// Token: 0x04000581 RID: 1409
	public int currentSpeedIndex;

	// Token: 0x04000582 RID: 1410
	public GorillaPressableButton speedZeroButton;

	// Token: 0x04000583 RID: 1411
	public GorillaPressableButton speedOneButton;

	// Token: 0x04000584 RID: 1412
	public GorillaPressableButton speedTwoButton;

	// Token: 0x04000585 RID: 1413
	public GorillaPressableButton speedThreeButton;

	// Token: 0x04000586 RID: 1414
	private bool debugDrawTrajectoryOnLaunch;

	// Token: 0x04000587 RID: 1415
	private int loadTriggerHash = Animator.StringToHash("Load");

	// Token: 0x04000588 RID: 1416
	private int fireTriggerHash = Animator.StringToHash("Fire");

	// Token: 0x04000589 RID: 1417
	private int pitchParamHash = Animator.StringToHash("Pitch");

	// Token: 0x0400058A RID: 1418
	private int idleStateHash = Animator.StringToHash("Idle");

	// Token: 0x0400058B RID: 1419
	private int loadStateHash = Animator.StringToHash("Load");

	// Token: 0x0400058C RID: 1420
	private int fireStateHash = Animator.StringToHash("Fire");

	// Token: 0x0400058D RID: 1421
	private int prevStateHash = Animator.StringToHash("Idle");

	// Token: 0x0400058E RID: 1422
	private float fireCompleteTime;

	// Token: 0x0400058F RID: 1423
	private float loadStartTime;

	// Token: 0x04000590 RID: 1424
	private bool playerInTrigger;

	// Token: 0x04000591 RID: 1425
	private bool playerReadyToFire;

	// Token: 0x04000592 RID: 1426
	private bool playerLaunched;

	// Token: 0x04000593 RID: 1427
	private float playerReadyToFireDist = 0.1f;

	// Token: 0x04000594 RID: 1428
	private Vector3 playerBodyOffsetFromHead = new Vector3(0f, -0.4f, -0.15f);

	// Token: 0x04000595 RID: 1429
	private Vector3 launchDirection;

	// Token: 0x04000596 RID: 1430
	private float launchRampDistance;

	// Token: 0x04000597 RID: 1431
	private int collidingLayer;

	// Token: 0x04000598 RID: 1432
	private int notCollidingLayer;

	// Token: 0x04000599 RID: 1433
	private float playerPullInRate;

	// Token: 0x0400059A RID: 1434
	private float appliedAnimatorPitch;

	// Token: 0x0400059B RID: 1435
	private const int predictionLineSamples = 240;

	// Token: 0x0400059C RID: 1436
	private Vector3[] predictionLinePoints = new Vector3[240];
}
