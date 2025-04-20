using System;
using System.Collections;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200064B RID: 1611
public class GorillaScoreboardSpawner : MonoBehaviour
{
	// Token: 0x060027DE RID: 10206 RVA: 0x0004B15A File Offset: 0x0004935A
	public void Awake()
	{
		base.StartCoroutine(this.UpdateBoard());
	}

	// Token: 0x060027DF RID: 10207 RVA: 0x0010EEE8 File Offset: 0x0010D0E8
	private void Start()
	{
		RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.OnJoinedRoom));
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x060027E0 RID: 10208 RVA: 0x0004B169 File Offset: 0x00049369
	public bool IsCurrentScoreboard()
	{
		return base.gameObject.activeInHierarchy;
	}

	// Token: 0x060027E1 RID: 10209 RVA: 0x0010EF38 File Offset: 0x0010D138
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

	// Token: 0x060027E2 RID: 10210 RVA: 0x0004B176 File Offset: 0x00049376
	public bool IsVisible()
	{
		if (!this.forOverlay)
		{
			return this.controllingParentGameObject.activeSelf;
		}
		return GTPlayer.Instance.inOverlay;
	}

	// Token: 0x060027E3 RID: 10211 RVA: 0x0004B196 File Offset: 0x00049396
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

	// Token: 0x060027E4 RID: 10212 RVA: 0x0004B1A5 File Offset: 0x000493A5
	public void OnLeftRoom()
	{
		this.Cleanup();
		if (this.notInRoomText)
		{
			this.notInRoomText.SetActive(true);
		}
	}

	// Token: 0x060027E5 RID: 10213 RVA: 0x0004B1C6 File Offset: 0x000493C6
	public void Cleanup()
	{
		if (this.currentScoreboard != null)
		{
			UnityEngine.Object.Destroy(this.currentScoreboard.gameObject);
			this.currentScoreboard = null;
		}
	}

	// Token: 0x04002D25 RID: 11557
	public string gameType;

	// Token: 0x04002D26 RID: 11558
	public bool includeMMR;

	// Token: 0x04002D27 RID: 11559
	public GameObject scoreboardPrefab;

	// Token: 0x04002D28 RID: 11560
	public GameObject notInRoomText;

	// Token: 0x04002D29 RID: 11561
	public GameObject controllingParentGameObject;

	// Token: 0x04002D2A RID: 11562
	public bool isActive = true;

	// Token: 0x04002D2B RID: 11563
	public GorillaScoreBoard currentScoreboard;

	// Token: 0x04002D2C RID: 11564
	public bool lastVisible;

	// Token: 0x04002D2D RID: 11565
	public bool forOverlay;
}
