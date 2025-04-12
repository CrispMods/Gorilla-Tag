using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000488 RID: 1160
public class AtticHider : MonoBehaviour
{
	// Token: 0x06001C0A RID: 7178 RVA: 0x00042595 File Offset: 0x00040795
	private void Start()
	{
		this.OnZoneChanged();
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06001C0B RID: 7179 RVA: 0x000425C3 File Offset: 0x000407C3
	private void OnDestroy()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06001C0C RID: 7180 RVA: 0x000D93B0 File Offset: 0x000D75B0
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

	// Token: 0x06001C0D RID: 7181 RVA: 0x000425EB File Offset: 0x000407EB
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

	// Token: 0x04001F13 RID: 7955
	[SerializeField]
	private MeshRenderer AtticRenderer;

	// Token: 0x04001F14 RID: 7956
	private Coroutine _coroutine;
}
