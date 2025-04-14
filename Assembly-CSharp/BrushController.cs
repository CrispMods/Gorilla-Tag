using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000309 RID: 777
public class BrushController : MonoBehaviour
{
	// Token: 0x06001283 RID: 4739 RVA: 0x00058E48 File Offset: 0x00057048
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

	// Token: 0x06001284 RID: 4740 RVA: 0x00058F15 File Offset: 0x00057115
	private void Update()
	{
		this.backgroundSphere.transform.position = Camera.main.transform.position;
	}

	// Token: 0x06001285 RID: 4741 RVA: 0x00058F38 File Offset: 0x00057138
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

	// Token: 0x06001286 RID: 4742 RVA: 0x00058FBC File Offset: 0x000571BC
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

	// Token: 0x06001287 RID: 4743 RVA: 0x00059046 File Offset: 0x00057246
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

	// Token: 0x06001288 RID: 4744 RVA: 0x0005905C File Offset: 0x0005725C
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

	// Token: 0x04001472 RID: 5234
	public PassthroughBrush brush;

	// Token: 0x04001473 RID: 5235
	public MeshRenderer backgroundSphere;

	// Token: 0x04001474 RID: 5236
	private IEnumerator grabRoutine;

	// Token: 0x04001475 RID: 5237
	private IEnumerator releaseRoutine;
}
