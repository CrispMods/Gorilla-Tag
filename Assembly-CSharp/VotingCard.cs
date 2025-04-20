using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class VotingCard : MonoBehaviour
{
	// Token: 0x06000767 RID: 1895 RVA: 0x0003548A File Offset: 0x0003368A
	private void MoveToOffPosition()
	{
		this._card.transform.position = this._offPosition.position;
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x000354A7 File Offset: 0x000336A7
	private void MoveToOnPosition()
	{
		this._card.transform.position = this._onPosition.position;
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x0008A7D4 File Offset: 0x000889D4
	public void SetVisible(bool showVote, bool instant)
	{
		if (this._isVisible != showVote)
		{
			base.StopAllCoroutines();
		}
		if (instant)
		{
			this._card.transform.position = (showVote ? this._onPosition.position : this._offPosition.position);
			this._card.SetActive(showVote);
		}
		else if (showVote)
		{
			if (this._isVisible != showVote)
			{
				base.StartCoroutine(this.DoActivate());
			}
		}
		else
		{
			this._card.SetActive(false);
			this._card.transform.position = this._offPosition.position;
		}
		this._isVisible = showVote;
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x000354C4 File Offset: 0x000336C4
	private IEnumerator DoActivate()
	{
		Vector3 from = this._offPosition.position;
		Vector3 to = this._onPosition.position;
		this._card.transform.position = from;
		this._card.SetActive(true);
		float lerpVal = 0f;
		while (lerpVal < 1f)
		{
			lerpVal += Time.deltaTime / this.activationTime;
			this._card.transform.position = Vector3.Lerp(from, to, lerpVal);
			yield return null;
		}
		yield break;
	}

	// Token: 0x040008C2 RID: 2242
	[SerializeField]
	private GameObject _card;

	// Token: 0x040008C3 RID: 2243
	[SerializeField]
	private Transform _offPosition;

	// Token: 0x040008C4 RID: 2244
	[SerializeField]
	private Transform _onPosition;

	// Token: 0x040008C5 RID: 2245
	[SerializeField]
	private float activationTime = 0.5f;

	// Token: 0x040008C6 RID: 2246
	private bool _isVisible;
}
