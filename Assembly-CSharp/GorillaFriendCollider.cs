using System;
using System.Collections;
using System.Collections.Generic;
using GorillaLocomotion;
using GorillaNetworking;
using UnityEngine;

// Token: 0x0200079F RID: 1951
public class GorillaFriendCollider : MonoBehaviour
{
	// Token: 0x0600302A RID: 12330 RVA: 0x000E8AC0 File Offset: 0x000E6CC0
	public void Awake()
	{
		this.thisCapsule = base.GetComponent<CapsuleCollider>();
		this.thisBox = base.GetComponent<BoxCollider>();
		this.jiggleAmount = Random.Range(0f, 1f);
		this.tagAndBodyLayerMask = (LayerMask.GetMask(new string[]
		{
			"Gorilla Tag Collider"
		}) | LayerMask.GetMask(new string[]
		{
			"Gorilla Body Collider"
		}));
	}

	// Token: 0x0600302B RID: 12331 RVA: 0x000E8B27 File Offset: 0x000E6D27
	public void OnEnable()
	{
		base.StartCoroutine(this.UpdatePlayersInSphere());
	}

	// Token: 0x0600302C RID: 12332 RVA: 0x000E8B36 File Offset: 0x000E6D36
	public void OnDisable()
	{
		base.StopCoroutine(this.UpdatePlayersInSphere());
	}

	// Token: 0x0600302D RID: 12333 RVA: 0x000E8B44 File Offset: 0x000E6D44
	private void AddUserID(in string userID)
	{
		if (this.playerIDsCurrentlyTouching.Contains(userID))
		{
			return;
		}
		this.playerIDsCurrentlyTouching.Add(userID);
	}

	// Token: 0x0600302E RID: 12334 RVA: 0x000E8B63 File Offset: 0x000E6D63
	private IEnumerator UpdatePlayersInSphere()
	{
		yield return new WaitForSeconds(1f + this.jiggleAmount);
		for (;;)
		{
			if (!NetworkSystem.Instance.InRoom && !this.runCheckWhileNotInRoom)
			{
				yield return this.wait1Sec;
			}
			else
			{
				this.playerIDsCurrentlyTouching.Clear();
				if (this.thisBox != null)
				{
					this.collisions = Physics.OverlapBoxNonAlloc(this.thisBox.transform.position, this.thisBox.size / 2f, this.overlapColliders, this.thisBox.transform.rotation, this.tagAndBodyLayerMask);
				}
				else
				{
					this.collisions = Physics.OverlapSphereNonAlloc(base.transform.position, this.thisCapsule.radius, this.overlapColliders, this.tagAndBodyLayerMask);
				}
				this.collisions = Mathf.Min(this.collisions, this.overlapColliders.Length);
				if (this.collisions > 0)
				{
					for (int i = 0; i < this.collisions; i++)
					{
						this.otherCollider = this.overlapColliders[i];
						if (!(this.otherCollider == null))
						{
							this.otherColliderGO = this.otherCollider.attachedRigidbody.gameObject;
							this.collidingRig = this.otherColliderGO.GetComponent<VRRig>();
							if (this.collidingRig == null || this.collidingRig.creator == null || this.collidingRig.creator.IsNull || string.IsNullOrEmpty(this.collidingRig.creator.UserId))
							{
								if (this.otherColliderGO.GetComponent<GTPlayer>() == null || NetworkSystem.Instance.LocalPlayer == null)
								{
									goto IL_22B;
								}
								string userId = NetworkSystem.Instance.LocalPlayer.UserId;
								this.AddUserID(userId);
							}
							else
							{
								string userId = this.collidingRig.creator.UserId;
								this.AddUserID(userId);
							}
							this.overlapColliders[i] = null;
						}
						IL_22B:;
					}
					if (NetworkSystem.Instance.InRoom && NetworkSystem.Instance.LocalPlayer != null && this.playerIDsCurrentlyTouching.Contains(NetworkSystem.Instance.LocalPlayer.UserId) && GorillaComputer.instance.friendJoinCollider != this)
					{
						GorillaComputer.instance.allowedMapsToJoin = this.myAllowedMapsToJoin;
						GorillaComputer.instance.friendJoinCollider = this;
						GorillaComputer.instance.UpdateScreen();
					}
					this.otherCollider = null;
					this.otherColliderGO = null;
					this.collidingRig = null;
				}
				yield return this.wait1Sec;
			}
		}
		yield break;
	}

	// Token: 0x0400341B RID: 13339
	public List<string> playerIDsCurrentlyTouching = new List<string>();

	// Token: 0x0400341C RID: 13340
	private CapsuleCollider thisCapsule;

	// Token: 0x0400341D RID: 13341
	private BoxCollider thisBox;

	// Token: 0x0400341E RID: 13342
	public bool runCheckWhileNotInRoom;

	// Token: 0x0400341F RID: 13343
	public string[] myAllowedMapsToJoin;

	// Token: 0x04003420 RID: 13344
	private readonly Collider[] overlapColliders = new Collider[20];

	// Token: 0x04003421 RID: 13345
	private int tagAndBodyLayerMask;

	// Token: 0x04003422 RID: 13346
	private float jiggleAmount;

	// Token: 0x04003423 RID: 13347
	private Collider otherCollider;

	// Token: 0x04003424 RID: 13348
	private GameObject otherColliderGO;

	// Token: 0x04003425 RID: 13349
	private VRRig collidingRig;

	// Token: 0x04003426 RID: 13350
	private int collisions;

	// Token: 0x04003427 RID: 13351
	private WaitForSeconds wait1Sec = new WaitForSeconds(1f);
}
