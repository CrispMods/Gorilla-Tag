using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000488 RID: 1160
public class AtticHider : MonoBehaviour
{
	// Token: 0x06001C07 RID: 7175 RVA: 0x00088496 File Offset: 0x00086696
	private void Start()
	{
		this.OnZoneChanged();
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06001C08 RID: 7176 RVA: 0x000884C4 File Offset: 0x000866C4
	private void OnDestroy()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06001C09 RID: 7177 RVA: 0x000884EC File Offset: 0x000866EC
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

	// Token: 0x06001C0A RID: 7178 RVA: 0x0008856B File Offset: 0x0008676B
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

	// Token: 0x04001F12 RID: 7954
	[SerializeField]
	private MeshRenderer AtticRenderer;

	// Token: 0x04001F13 RID: 7955
	private Coroutine _coroutine;
}
