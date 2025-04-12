using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200010B RID: 267
public class VotingCard : MonoBehaviour
{
	// Token: 0x06000728 RID: 1832 RVA: 0x00034226 File Offset: 0x00032426
	private void MoveToOffPosition()
	{
		this._card.transform.position = this._offPosition.position;
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x00034243 File Offset: 0x00032443
	private void MoveToOnPosition()
	{
		this._card.transform.position = this._onPosition.position;
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x00087ECC File Offset: 0x000860CC
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

	// Token: 0x0600072B RID: 1835 RVA: 0x00034260 File Offset: 0x00032460
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

	// Token: 0x04000882 RID: 2178
	[SerializeField]
	private GameObject _card;

	// Token: 0x04000883 RID: 2179
	[SerializeField]
	private Transform _offPosition;

	// Token: 0x04000884 RID: 2180
	[SerializeField]
	private Transform _onPosition;

	// Token: 0x04000885 RID: 2181
	[SerializeField]
	private float activationTime = 0.5f;

	// Token: 0x04000886 RID: 2182
	private bool _isVisible;
}
