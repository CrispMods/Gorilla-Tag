using System;
using System.Collections.Generic;
using System.Text;
using KID.Model;
using TMPro;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009CC RID: 2508
	public class PlayerTimerBoard : MonoBehaviour
	{
		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06003E8A RID: 16010 RVA: 0x00128589 File Offset: 0x00126789
		// (set) Token: 0x06003E8B RID: 16011 RVA: 0x00128591 File Offset: 0x00126791
		public bool IsDirty { get; set; } = true;

		// Token: 0x06003E8C RID: 16012 RVA: 0x0012859A File Offset: 0x0012679A
		private void Start()
		{
			this.TryInit();
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x0012859A File Offset: 0x0012679A
		private void OnEnable()
		{
			this.TryInit();
		}

		// Token: 0x06003E8E RID: 16014 RVA: 0x001285A2 File Offset: 0x001267A2
		private void TryInit()
		{
			if (this.isInitialized)
			{
				return;
			}
			if (PlayerTimerManager.instance == null)
			{
				return;
			}
			PlayerTimerManager.instance.RegisterTimerBoard(this);
			this.isInitialized = true;
		}

		// Token: 0x06003E8F RID: 16015 RVA: 0x001285CD File Offset: 0x001267CD
		private void OnDisable()
		{
			if (PlayerTimerManager.instance != null)
			{
				PlayerTimerManager.instance.UnregisterTimerBoard(this);
			}
			this.isInitialized = false;
		}

		// Token: 0x06003E90 RID: 16016 RVA: 0x001285EE File Offset: 0x001267EE
		public void SetSleepState(bool awake)
		{
			this.playerColumn.enabled = awake;
			this.timeColumn.enabled = awake;
			if (this.linesParent != null)
			{
				this.linesParent.SetActive(awake);
			}
		}

		// Token: 0x06003E91 RID: 16017 RVA: 0x00128622 File Offset: 0x00126822
		public void SortLines()
		{
			this.lines.Sort(new Comparison<PlayerTimerBoardLine>(PlayerTimerBoardLine.CompareByTotalTime));
		}

		// Token: 0x06003E92 RID: 16018 RVA: 0x0012863C File Offset: 0x0012683C
		public void RedrawPlayerLines()
		{
			this.stringBuilder.Clear();
			this.stringBuilderTime.Clear();
			this.stringBuilder.Append("<b><color=yellow>PLAYER</color></b>");
			this.stringBuilderTime.Append("<b><color=yellow>LATEST TIME</color></b>");
			this.SortLines();
			ValueTuple<bool, Permission.ManagedByEnum> customNicknamePermissionStatus = KIDManager.Instance.GetCustomNicknamePermissionStatus();
			bool flag = (customNicknamePermissionStatus.Item1 || customNicknamePermissionStatus.Item2 == Permission.ManagedByEnum.PLAYER) && customNicknamePermissionStatus.Item2 != Permission.ManagedByEnum.PROHIBITED;
			for (int i = 0; i < this.lines.Count; i++)
			{
				try
				{
					if (this.lines[i].gameObject.activeInHierarchy)
					{
						this.lines[i].gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, (float)(this.startingYValue - this.lineHeight * i), 0f);
						if (this.lines[i].linePlayer != null && this.lines[i].linePlayer.InRoom)
						{
							this.stringBuilder.Append("\n ");
							this.stringBuilder.Append(flag ? this.lines[i].playerNameVisible : this.lines[i].linePlayer.DefaultName);
							this.stringBuilderTime.Append("\n ");
							this.stringBuilderTime.Append(this.lines[i].playerTimeStr);
						}
					}
				}
				catch
				{
				}
			}
			this.playerColumn.text = this.stringBuilder.ToString();
			this.timeColumn.text = this.stringBuilderTime.ToString();
			this.IsDirty = false;
		}

		// Token: 0x04003FD0 RID: 16336
		[SerializeField]
		private GameObject linesParent;

		// Token: 0x04003FD1 RID: 16337
		public List<PlayerTimerBoardLine> lines;

		// Token: 0x04003FD2 RID: 16338
		public TextMeshPro notInRoomText;

		// Token: 0x04003FD3 RID: 16339
		public TextMeshPro playerColumn;

		// Token: 0x04003FD4 RID: 16340
		public TextMeshPro timeColumn;

		// Token: 0x04003FD5 RID: 16341
		[SerializeField]
		private int startingYValue;

		// Token: 0x04003FD6 RID: 16342
		[SerializeField]
		private int lineHeight;

		// Token: 0x04003FD7 RID: 16343
		private StringBuilder stringBuilder = new StringBuilder(220);

		// Token: 0x04003FD8 RID: 16344
		private StringBuilder stringBuilderTime = new StringBuilder(220);

		// Token: 0x04003FD9 RID: 16345
		private bool isInitialized;
	}
}
