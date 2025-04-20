using System;
using System.Collections;
using System.Collections.Generic;
using GorillaLocomotion;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020007B7 RID: 1975
public class GorillaFriendCollider : MonoBehaviour
{
	// Token: 0x060030DC RID: 12508 RVA: 0x00132014 File Offset: 0x00130214
	public void Awake()
	{
		this.thisCapsule = base.GetComponent<CapsuleCollider>();
		this.thisBox = base.GetComponent<BoxCollider>();
		this.jiggleAmount = UnityEngine.Random.Range(0f, 1f);
		this.tagAndBodyLayerMask = (LayerMask.GetMask(new string[]
		{
			"Gorilla Tag Collider"
		}) | LayerMask.GetMask(new string[]
		{
			"Gorilla Body Collider"
		}));
	}

	// Token: 0x060030DD RID: 12509 RVA: 0x00050572 File Offset: 0x0004E772
	public void OnEnable()
	{
		base.StartCoroutine(this.UpdatePlayersInSphere());
	}

	// Token: 0x060030DE RID: 12510 RVA: 0x00050581 File Offset: 0x0004E781
	public void OnDisable()
	{
		base.StopCoroutine(this.UpdatePlayersInSphere());
	}

	// Token: 0x060030DF RID: 12511 RVA: 0x0005058F File Offset: 0x0004E78F
	private void AddUserID(in string userID)
	{
		if (this.playerIDsCurrentlyTouching.Contains(userID))
		{
			return;
		}
		this.playerIDsCurrentlyTouching.Add(userID);
	}

	// Token: 0x060030E0 RID: 12512 RVA: 0x000505AE File Offset: 0x0004E7AE
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

	// Token: 0x040034C5 RID: 13509
	public List<string> playerIDsCurrentlyTouching = new List<string>();

	// Token: 0x040034C6 RID: 13510
	private CapsuleCollider thisCapsule;

	// Token: 0x040034C7 RID: 13511
	private BoxCollider thisBox;

	// Token: 0x040034C8 RID: 13512
	public bool runCheckWhileNotInRoom;

	// Token: 0x040034C9 RID: 13513
	public string[] myAllowedMapsToJoin;

	// Token: 0x040034CA RID: 13514
	private readonly Collider[] overlapColliders = new Collider[20];

	// Token: 0x040034CB RID: 13515
	private int tagAndBodyLayerMask;

	// Token: 0x040034CC RID: 13516
	private float jiggleAmount;

	// Token: 0x040034CD RID: 13517
	private Collider otherCollider;

	// Token: 0x040034CE RID: 13518
	private GameObject otherColliderGO;

	// Token: 0x040034CF RID: 13519
	private VRRig collidingRig;

	// Token: 0x040034D0 RID: 13520
	private int collisions;

	// Token: 0x040034D1 RID: 13521
	private WaitForSeconds wait1Sec = new WaitForSeconds(1f);
}
