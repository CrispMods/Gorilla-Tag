using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000152 RID: 338
public class ElfLauncher : MonoBehaviour
{
	// Token: 0x0600089C RID: 2204 RVA: 0x0002F1EC File Offset: 0x0002D3EC
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

	// Token: 0x0600089D RID: 2205 RVA: 0x0002F2D8 File Offset: 0x0002D4D8
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

	// Token: 0x0600089E RID: 2206 RVA: 0x0002F330 File Offset: 0x0002D530
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

	// Token: 0x0600089F RID: 2207 RVA: 0x0002F384 File Offset: 0x0002D584
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

	// Token: 0x060008A0 RID: 2208 RVA: 0x0002F3FC File Offset: 0x0002D5FC
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

	// Token: 0x060008A1 RID: 2209 RVA: 0x0002F4BC File Offset: 0x0002D6BC
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

	// Token: 0x060008A2 RID: 2210 RVA: 0x0002F574 File Offset: 0x0002D774
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

	// Token: 0x04000A4C RID: 2636
	[SerializeField]
	private TransferrableObject parentHoldable;

	// Token: 0x04000A4D RID: 2637
	[SerializeField]
	private TransferrableObjectHoldablePart_Crank[] cranks;

	// Token: 0x04000A4E RID: 2638
	[SerializeField]
	private float crankShootThreshold = 360f;

	// Token: 0x04000A4F RID: 2639
	[SerializeField]
	private float crankClickThreshold = 30f;

	// Token: 0x04000A50 RID: 2640
	[SerializeField]
	private Transform muzzle;

	// Token: 0x04000A51 RID: 2641
	[SerializeField]
	private GameObject elfProjectilePrefab;

	// Token: 0x04000A52 RID: 2642
	private int elfProjectileHash;

	// Token: 0x04000A53 RID: 2643
	[SerializeField]
	private float muzzleVelocity = 10f;

	// Token: 0x04000A54 RID: 2644
	[SerializeField]
	private SoundBankPlayer crankClickAudio;

	// Token: 0x04000A55 RID: 2645
	[SerializeField]
	private SoundBankPlayer shootAudio;

	// Token: 0x04000A56 RID: 2646
	[SerializeField]
	private float shootHapticStrength;

	// Token: 0x04000A57 RID: 2647
	[SerializeField]
	private float shootHapticDuration;

	// Token: 0x04000A58 RID: 2648
	private RubberDuckEvents _events;

	// Token: 0x04000A59 RID: 2649
	private float currentShootCrankAmount;

	// Token: 0x04000A5A RID: 2650
	private float currentClickCrankAmount;

	// Token: 0x04000A5B RID: 2651
	private NetPlayer m_player;
}
