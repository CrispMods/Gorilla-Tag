using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200068D RID: 1677
public class GorillaFireball : GorillaThrowable, IPunInstantiateMagicCallback
{
	// Token: 0x060029B8 RID: 10680 RVA: 0x000CF176 File Offset: 0x000CD376
	public override void Start()
	{
		base.Start();
		this.canExplode = false;
		this.explosionStartTime = 0f;
	}

	// Token: 0x060029B9 RID: 10681 RVA: 0x000CF190 File Offset: 0x000CD390
	private void Update()
	{
		if (this.explosionStartTime != 0f)
		{
			float num = (Time.time - this.explosionStartTime) / this.totalExplosionTime * (this.maxExplosionScale - 0.25f) + 0.25f;
			base.gameObject.transform.localScale = new Vector3(num, num, num);
			if (base.photonView.IsMine && Time.time > this.explosionStartTime + this.totalExplosionTime)
			{
				PhotonNetwork.Destroy(PhotonView.Get(this));
			}
		}
	}

	// Token: 0x060029BA RID: 10682 RVA: 0x000CF218 File Offset: 0x000CD418
	public override void LateUpdate()
	{
		base.LateUpdate();
		if (this.rigidbody.useGravity)
		{
			this.rigidbody.AddForce(Physics.gravity * -this.gravityStrength * this.rigidbody.mass);
		}
	}

	// Token: 0x060029BB RID: 10683 RVA: 0x000CF264 File Offset: 0x000CD464
	public override void ThrowThisThingo()
	{
		base.ThrowThisThingo();
		this.canExplode = true;
	}

	// Token: 0x060029BC RID: 10684 RVA: 0x000CF273 File Offset: 0x000CD473
	private new void OnCollisionEnter(Collision collision)
	{
		if (base.photonView.IsMine && this.canExplode)
		{
			base.photonView.RPC("Explode", RpcTarget.All, null);
		}
	}

	// Token: 0x060029BD RID: 10685 RVA: 0x000CF29C File Offset: 0x000CD49C
	public void LocalExplode()
	{
		this.rigidbody.isKinematic = true;
		this.canExplode = false;
		this.explosionStartTime = Time.time;
	}

	// Token: 0x060029BE RID: 10686 RVA: 0x000CF2BC File Offset: 0x000CD4BC
	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		if (base.photonView.IsMine)
		{
			if ((bool)base.photonView.InstantiationData[0])
			{
				base.transform.parent = GorillaPlaySpace.Instance.myVRRig.leftHandTransform;
				return;
			}
			base.transform.parent = GorillaPlaySpace.Instance.myVRRig.rightHandTransform;
		}
	}

	// Token: 0x060029BF RID: 10687 RVA: 0x000CF31F File Offset: 0x000CD51F
	[PunRPC]
	public void Explode()
	{
		this.LocalExplode();
	}

	// Token: 0x04002F05 RID: 12037
	public float maxExplosionScale;

	// Token: 0x04002F06 RID: 12038
	public float totalExplosionTime;

	// Token: 0x04002F07 RID: 12039
	public float gravityStrength;

	// Token: 0x04002F08 RID: 12040
	private bool canExplode;

	// Token: 0x04002F09 RID: 12041
	private float explosionStartTime;
}
