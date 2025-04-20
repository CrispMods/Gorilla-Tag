using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000314 RID: 788
public class BrushController : MonoBehaviour
{
	// Token: 0x060012CF RID: 4815 RVA: 0x000B30E0 File Offset: 0x000B12E0
	private void Start()
	{
		this.brush.controllerHand = OVRInput.Controller.None;
		if (!this.brush.lineContainer)
		{
			this.brush.lineContainer = new GameObject("LineContainer");
		}
		this.backgroundSphere.material.renderQueue = 3998;
		this.backgroundSphere.transform.parent = null;
		this.backgroundSphere.enabled = false;
		if (base.GetComponent<GrabObject>())
		{
			GrabObject component = base.GetComponent<GrabObject>();
			component.GrabbedObjectDelegate = (GrabObject.GrabbedObject)Delegate.Combine(component.GrabbedObjectDelegate, new GrabObject.GrabbedObject(this.Grab));
			GrabObject component2 = base.GetComponent<GrabObject>();
			component2.ReleasedObjectDelegate = (GrabObject.ReleasedObject)Delegate.Combine(component2.ReleasedObjectDelegate, new GrabObject.ReleasedObject(this.Release));
		}
	}

	// Token: 0x060012D0 RID: 4816 RVA: 0x0003CE96 File Offset: 0x0003B096
	private void Update()
	{
		this.backgroundSphere.transform.position = Camera.main.transform.position;
	}

	// Token: 0x060012D1 RID: 4817 RVA: 0x000B31B0 File Offset: 0x000B13B0
	public void Grab(OVRInput.Controller grabHand)
	{
		this.brush.controllerHand = grabHand;
		this.brush.lineContainer.SetActive(true);
		this.backgroundSphere.enabled = true;
		if (this.grabRoutine != null)
		{
			base.StopCoroutine(this.grabRoutine);
		}
		if (this.releaseRoutine != null)
		{
			base.StopCoroutine(this.releaseRoutine);
		}
		this.grabRoutine = this.FadeSphere(Color.grey, 0.25f, false);
		base.StartCoroutine(this.grabRoutine);
	}

	// Token: 0x060012D2 RID: 4818 RVA: 0x000B3234 File Offset: 0x000B1434
	public void Release()
	{
		this.brush.controllerHand = OVRInput.Controller.None;
		this.brush.lineContainer.SetActive(false);
		if (this.grabRoutine != null)
		{
			base.StopCoroutine(this.grabRoutine);
		}
		if (this.releaseRoutine != null)
		{
			base.StopCoroutine(this.releaseRoutine);
		}
		this.releaseRoutine = this.FadeSphere(new Color(0.5f, 0.5f, 0.5f, 0f), 0.25f, true);
		base.StartCoroutine(this.releaseRoutine);
	}

	// Token: 0x060012D3 RID: 4819 RVA: 0x0003CEB7 File Offset: 0x0003B0B7
	private IEnumerator FadeCameraClearColor(Color newColor, float fadeTime)
	{
		float timer = 0f;
		Color currentColor = Camera.main.backgroundColor;
		while (timer <= fadeTime)
		{
			timer += Time.deltaTime;
			float t = Mathf.Clamp01(timer / fadeTime);
			Camera.main.backgroundColor = Color.Lerp(currentColor, newColor, t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060012D4 RID: 4820 RVA: 0x0003CECD File Offset: 0x0003B0CD
	private IEnumerator FadeSphere(Color newColor, float fadeTime, bool disableOnFinish = false)
	{
		float timer = 0f;
		Color currentColor = this.backgroundSphere.material.GetColor("_Color");
		while (timer <= fadeTime)
		{
			timer += Time.deltaTime;
			float t = Mathf.Clamp01(timer / fadeTime);
			this.backgroundSphere.material.SetColor("_Color", Color.Lerp(currentColor, newColor, t));
			if (disableOnFinish && timer >= fadeTime)
			{
				this.backgroundSphere.enabled = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x040014BA RID: 5306
	public PassthroughBrush brush;

	// Token: 0x040014BB RID: 5307
	public MeshRenderer backgroundSphere;

	// Token: 0x040014BC RID: 5308
	private IEnumerator grabRoutine;

	// Token: 0x040014BD RID: 5309
	private IEnumerator releaseRoutine;
}
