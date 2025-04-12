using System;
using System.Collections;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200066D RID: 1645
public class GorillaScoreboardSpawner : MonoBehaviour
{
	// Token: 0x060028BB RID: 10427 RVA: 0x0004ABBD File Offset: 0x00048DBD
	public void Awake()
	{
		base.StartCoroutine(this.UpdateBoard());
	}

	// Token: 0x060028BC RID: 10428 RVA: 0x00110AC0 File Offset: 0x0010ECC0
	private void Start()
	{
		RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.OnJoinedRoom));
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x060028BD RID: 10429 RVA: 0x0004ABCC File Offset: 0x00048DCC
	public bool IsCurrentScoreboard()
	{
		return base.gameObject.activeInHierarchy;
	}

	// Token: 0x060028BE RID: 10430 RVA: 0x00110B10 File Offset: 0x0010ED10
	public void OnJoinedRoom()
	{
		Debug.Log("SCOREBOARD JOIN ROOM");
		if (this.IsCurrentScoreboard())
		{
			this.notInRoomText.SetActive(false);
			this.currentScoreboard = UnityEngine.Object.Instantiate<GameObject>(this.scoreboardPrefab, base.transform).GetComponent<GorillaScoreBoard>();
			this.currentScoreboard.transform.rotation = base.transform.rotation;
			if (this.includeMMR)
			{
				this.currentScoreboard.GetComponent<GorillaScoreBoard>().includeMMR = true;
				this.currentScoreboard.GetComponent<Text>().text = "Player                     Color         Level        MMR";
			}
		}
	}

	// Token: 0x060028BF RID: 10431 RVA: 0x0004ABD9 File Offset: 0x00048DD9
	public bool IsVisible()
	{
		if (!this.forOverlay)
		{
			return this.controllingParentGameObject.activeSelf;
		}
		return GTPlayer.Instance.inOverlay;
	}

	// Token: 0x060028C0 RID: 10432 RVA: 0x0004ABF9 File Offset: 0x00048DF9
	private IEnumerator UpdateBoard()
	{
		for (;;)
		{
			try
			{
				if (this.currentScoreboard != null)
				{
					bool flag = this.IsVisible();
					foreach (GorillaPlayerScoreboardLine gorillaPlayerScoreboardLine in this.currentScoreboard.lines)
					{
						if (flag != gorillaPlayerScoreboardLine.lastVisible)
						{
							gorillaPlayerScoreboardLine.lastVisible = flag;
						}
					}
					if (this.currentScoreboard.boardText.enabled != flag)
					{
						this.currentScoreboard.boardText.enabled = flag;
					}
					if (this.currentScoreboard.buttonText.enabled != flag)
					{
						this.currentScoreboard.buttonText.enabled = flag;
					}
				}
			}
			catch
			{
			}
			yield return new WaitForSeconds(1f);
		}
		yield break;
	}

	// Token: 0x060028C1 RID: 10433 RVA: 0x0004AC08 File Offset: 0x00048E08
	public void OnLeftRoom()
	{
		this.Cleanup();
		if (this.notInRoomText)
		{
			this.notInRoomText.SetActive(true);
		}
	}

	// Token: 0x060028C2 RID: 10434 RVA: 0x0004AC29 File Offset: 0x00048E29
	public void Cleanup()
	{
		if (this.currentScoreboard != null)
		{
			UnityEngine.Object.Destroy(this.currentScoreboard.gameObject);
			this.currentScoreboard = null;
		}
	}

	// Token: 0x04002DC5 RID: 11717
	public string gameType;

	// Token: 0x04002DC6 RID: 11718
	public bool includeMMR;

	// Token: 0x04002DC7 RID: 11719
	public GameObject scoreboardPrefab;

	// Token: 0x04002DC8 RID: 11720
	public GameObject notInRoomText;

	// Token: 0x04002DC9 RID: 11721
	public GameObject controllingParentGameObject;

	// Token: 0x04002DCA RID: 11722
	public bool isActive = true;

	// Token: 0x04002DCB RID: 11723
	public GorillaScoreBoard currentScoreboard;

	// Token: 0x04002DCC RID: 11724
	public bool lastVisible;

	// Token: 0x04002DCD RID: 11725
	public bool forOverlay;
}
