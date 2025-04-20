using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000494 RID: 1172
public class AtticHider : MonoBehaviour
{
	// Token: 0x06001C5B RID: 7259 RVA: 0x000438CE File Offset: 0x00041ACE
	private void Start()
	{
		this.OnZoneChanged();
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06001C5C RID: 7260 RVA: 0x000438FC File Offset: 0x00041AFC
	private void OnDestroy()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06001C5D RID: 7261 RVA: 0x000DC060 File Offset: 0x000DA260
	private void OnZoneChanged()
	{
		if (this.AtticRenderer == null)
		{
			return;
		}
		if (ZoneManagement.instance.IsZoneActive(GTZone.attic))
		{
			if (this._coroutine != null)
			{
				base.StopCoroutine(this._coroutine);
				this._coroutine = null;
			}
			this._coroutine = base.StartCoroutine(this.WaitForAtticLoad());
			return;
		}
		if (this._coroutine != null)
		{
			base.StopCoroutine(this._coroutine);
			this._coroutine = null;
		}
		this.AtticRenderer.enabled = true;
	}

	// Token: 0x06001C5E RID: 7262 RVA: 0x00043924 File Offset: 0x00041B24
	private IEnumerator WaitForAtticLoad()
	{
		while (!ZoneManagement.instance.IsSceneLoaded(GTZone.attic))
		{
			yield return new WaitForSeconds(0.2f);
		}
		yield return null;
		this.AtticRenderer.enabled = false;
		this._coroutine = null;
		yield break;
	}

	// Token: 0x04001F61 RID: 8033
	[SerializeField]
	private MeshRenderer AtticRenderer;

	// Token: 0x04001F62 RID: 8034
	private Coroutine _coroutine;
}
