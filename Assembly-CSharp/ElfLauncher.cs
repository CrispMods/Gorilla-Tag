using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200015C RID: 348
public class ElfLauncher : MonoBehaviour
{
	// Token: 0x060008E0 RID: 2272 RVA: 0x0008FED8 File Offset: 0x0008E0D8
	private void OnEnable()
	{
		if (this._events == null)
		{
			this._events = base.gameObject.GetOrAddComponent<RubberDuckEvents>();
			NetPlayer netPlayer = (this.parentHoldable.myOnlineRig != null) ? this.parentHoldable.myOnlineRig.creator : ((this.parentHoldable.myRig != null) ? ((this.parentHoldable.myRig.creator != null) ? this.parentHoldable.myRig.creator : NetworkSystem.Instance.LocalPlayer) : null);
			if (netPlayer != null)
			{
				this.m_player = netPlayer;
				this._events.Init(netPlayer);
			}
			else
			{
				Debug.LogError("Failed to get a reference to the Photon Player needed to hook up the cosmetic event");
			}
		}
		if (this._events != null)
		{
			this._events.Activate += this.ShootShared;
		}
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x0008FFC4 File Offset: 0x0008E1C4
	private void OnDisable()
	{
		if (this._events != null)
		{
			this._events.Activate -= this.ShootShared;
			this._events.Dispose();
			this._events = null;
			this.m_player = null;
		}
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x0009001C File Offset: 0x0008E21C
	private void Awake()
	{
		this._events = base.GetComponent<RubberDuckEvents>();
		this.elfProjectileHash = PoolUtils.GameObjHashCode(this.elfProjectilePrefab);
		TransferrableObjectHoldablePart_Crank[] array = this.cranks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetOnCrankedCallback(new Action<float>(this.OnCranked));
		}
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x00090070 File Offset: 0x0008E270
	private void OnCranked(float deltaAngle)
	{
		this.currentShootCrankAmount += deltaAngle;
		if (Mathf.Abs(this.currentShootCrankAmount) > this.crankShootThreshold)
		{
			this.currentShootCrankAmount = 0f;
			this.Shoot();
		}
		this.currentClickCrankAmount += deltaAngle;
		if (Mathf.Abs(this.currentClickCrankAmount) > this.crankClickThreshold)
		{
			this.currentClickCrankAmount = 0f;
			this.crankClickAudio.Play();
		}
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x000900E8 File Offset: 0x0008E2E8
	private void Shoot()
	{
		if (this.parentHoldable.IsLocalObject())
		{
			GorillaTagger.Instance.StartVibration(true, this.shootHapticStrength, this.shootHapticDuration);
			GorillaTagger.Instance.StartVibration(false, this.shootHapticStrength, this.shootHapticDuration);
			if (PhotonNetwork.InRoom)
			{
				this._events.Activate.RaiseAll(new object[]
				{
					this.muzzle.transform.position,
					this.muzzle.transform.forward
				});
				return;
			}
			this.ShootShared(this.muzzle.transform.position, this.muzzle.transform.forward);
		}
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x000901A8 File Offset: 0x0008E3A8
	private void ShootShared(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
	{
		if (args.Length != 2)
		{
			return;
		}
		if (sender != target)
		{
			return;
		}
		VRRig ownerRig = this.parentHoldable.ownerRig;
		if (info.senderID != ownerRig.creator.ActorNumber)
		{
			return;
		}
		if (args.Length == 2)
		{
			object obj = args[0];
			if (obj is Vector3)
			{
				Vector3 vector = (Vector3)obj;
				obj = args[1];
				if (obj is Vector3)
				{
					Vector3 direction = (Vector3)obj;
					float num = 10000f;
					if (vector.IsValid(num))
					{
						float num2 = 10000f;
						if (direction.IsValid(num2))
						{
							if (!FXSystem.CheckCallSpam(ownerRig.fxSettings, 4, info.SentServerTime) || !ownerRig.IsPositionInRange(vector, 6f))
							{
								return;
							}
							this.ShootShared(vector, direction);
							return;
						}
					}
				}
			}
		}
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x00090260 File Offset: 0x0008E460
	private void ShootShared(Vector3 origin, Vector3 direction)
	{
		this.shootAudio.Play();
		Vector3 lossyScale = base.transform.lossyScale;
		GameObject gameObject = ObjectPools.instance.Instantiate(this.elfProjectileHash);
		gameObject.transform.position = origin;
		gameObject.transform.rotation = Quaternion.LookRotation(direction);
		gameObject.transform.localScale = lossyScale;
		gameObject.GetComponent<Rigidbody>().velocity = direction * this.muzzleVelocity * lossyScale.x;
	}

	// Token: 0x04000A8F RID: 2703
	[SerializeField]
	private TransferrableObject parentHoldable;

	// Token: 0x04000A90 RID: 2704
	[SerializeField]
	private TransferrableObjectHoldablePart_Crank[] cranks;

	// Token: 0x04000A91 RID: 2705
	[SerializeField]
	private float crankShootThreshold = 360f;

	// Token: 0x04000A92 RID: 2706
	[SerializeField]
	private float crankClickThreshold = 30f;

	// Token: 0x04000A93 RID: 2707
	[SerializeField]
	private Transform muzzle;

	// Token: 0x04000A94 RID: 2708
	[SerializeField]
	private GameObject elfProjectilePrefab;

	// Token: 0x04000A95 RID: 2709
	private int elfProjectileHash;

	// Token: 0x04000A96 RID: 2710
	[SerializeField]
	private float muzzleVelocity = 10f;

	// Token: 0x04000A97 RID: 2711
	[SerializeField]
	private SoundBankPlayer crankClickAudio;

	// Token: 0x04000A98 RID: 2712
	[SerializeField]
	private SoundBankPlayer shootAudio;

	// Token: 0x04000A99 RID: 2713
	[SerializeField]
	private float shootHapticStrength;

	// Token: 0x04000A9A RID: 2714
	[SerializeField]
	private float shootHapticDuration;

	// Token: 0x04000A9B RID: 2715
	private RubberDuckEvents _events;

	// Token: 0x04000A9C RID: 2716
	private float currentShootCrankAmount;

	// Token: 0x04000A9D RID: 2717
	private float currentClickCrankAmount;

	// Token: 0x04000A9E RID: 2718
	private NetPlayer m_player;
}
