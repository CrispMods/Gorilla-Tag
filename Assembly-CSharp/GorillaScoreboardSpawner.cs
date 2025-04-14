using System;
using System.Collections;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200066C RID: 1644
public class GorillaScoreboardSpawner : MonoBehaviour
{
	// Token: 0x060028B3 RID: 10419 RVA: 0x000C814B File Offset: 0x000C634B
	public void Awake()
	{
		base.StartCoroutine(this.UpdateBoard());
	}

	// Token: 0x060028B4 RID: 10420 RVA: 0x000C815C File Offset: 0x000C635C
	private void Start()
	{
		RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.OnJoinedRoom));
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x060028B5 RID: 10421 RVA: 0x000C81A9 File Offset: 0x000C63A9
	public bool IsCurrentScoreboard()
	{
		return base.gameObject.activeInHierarchy;
	}

	// Token: 0x060028B6 RID: 10422 RVA: 0x000C81B8 File Offset: 0x000C63B8
	public void OnJoinedRoom()
	{
		Debug.Log("SCOREBOARD JOIN ROOM");
		if (this.IsCurrentScoreboard())
		{
			this.notInRoomText.SetActive(false);
			this.currentScoreboard = Object.Instantiate<GameObject>(this.scoreboardPrefab, base.transform).GetComponent<GorillaScoreBoard>();
			this.currentScoreboard.transform.rotation = base.transform.rotation;
			if (this.includeMMR)
			{
				this.currentScoreboard.GetComponent<GorillaScoreBoard>().includeMMR = true;
				this.currentScoreboard.GetComponent<Text>().text = "Player                     Color         Level        MMR";
			}
		}
	}

	// Token: 0x060028B7 RID: 10423 RVA: 0x000C8248 File Offset: 0x000C6448
	public bool IsVisible()
	{
		if (!this.forOverlay)
		{
			return this.controllingParentGameObject.activeSelf;
		}
		return GTPlayer.Instance.inOverlay;
	}

	// Token: 0x060028B8 RID: 10424 RVA: 0x000C8268 File Offset: 0x000C6468
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

	// Token: 0x060028B9 RID: 10425 RVA: 0x000C8277 File Offset: 0x000C6477
	public void OnLeftRoom()
	{
		this.Cleanup();
		if (this.notInRoomText)
		{
			this.notInRoomText.SetActive(true);
		}
	}

	// Token: 0x060028BA RID: 10426 RVA: 0x000C8298 File Offset: 0x000C6498
	public void Cleanup()
	{
		if (this.currentScoreboard != null)
		{
			Object.Destroy(this.currentScoreboard.gameObject);
			this.currentScoreboard = null;
		}
	}

	// Token: 0x04002DBF RID: 11711
	public string gameType;

	// Token: 0x04002DC0 RID: 11712
	public bool includeMMR;

	// Token: 0x04002DC1 RID: 11713
	public GameObject scoreboardPrefab;

	// Token: 0x04002DC2 RID: 11714
	public GameObject notInRoomText;

	// Token: 0x04002DC3 RID: 11715
	public GameObject controllingParentGameObject;

	// Token: 0x04002DC4 RID: 11716
	public bool isActive = true;

	// Token: 0x04002DC5 RID: 11717
	public GorillaScoreBoard currentScoreboard;

	// Token: 0x04002DC6 RID: 11718
	public bool lastVisible;

	// Token: 0x04002DC7 RID: 11719
	public bool forOverlay;
}
