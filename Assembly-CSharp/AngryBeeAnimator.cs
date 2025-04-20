using System;
using UnityEngine;

// Token: 0x020000E6 RID: 230
public class AngryBeeAnimator : MonoBehaviour
{
	// Token: 0x060005E3 RID: 1507 RVA: 0x00083BB0 File Offset: 0x00081DB0
	private void Awake()
	{
		this.bees = new GameObject[this.numBees];
		this.beeOrbits = new GameObject[this.numBees];
		this.beeOrbitalRadii = new float[this.numBees];
		this.beeOrbitalAxes = new Vector3[this.numBees];
		for (int i = 0; i < this.numBees; i++)
		{
			GameObject gameObject = new GameObject();
			gameObject.transform.parent = base.transform;
			Vector2 vector = UnityEngine.Random.insideUnitCircle * this.orbitMaxCenterDisplacement;
			gameObject.transform.localPosition = new Vector3(vector.x, UnityEngine.Random.Range(-this.orbitMaxHeightDisplacement, this.orbitMaxHeightDisplacement), vector.y);
			gameObject.transform.localRotation = Quaternion.Euler(UnityEngine.Random.Range(-this.orbitMaxTilt, this.orbitMaxTilt), (float)UnityEngine.Random.Range(0, 360), 0f);
			this.beeOrbitalAxes[i] = gameObject.transform.up;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.beePrefab, gameObject.transform);
			float num = UnityEngine.Random.Range(this.orbitMinRadius, this.orbitMaxRadius);
			this.beeOrbitalRadii[i] = num;
			gameObject2.transform.localPosition = Vector3.forward * num;
			gameObject2.transform.localRotation = Quaternion.Euler(-90f, 90f, 0f);
			gameObject2.transform.localScale = Vector3.one * this.beeScale;
			this.bees[i] = gameObject2;
			this.beeOrbits[i] = gameObject;
		}
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x00083D4C File Offset: 0x00081F4C
	private void Update()
	{
		float angle = this.orbitSpeed * Time.deltaTime;
		for (int i = 0; i < this.numBees; i++)
		{
			this.beeOrbits[i].transform.Rotate(this.beeOrbitalAxes[i], angle);
		}
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x00083D98 File Offset: 0x00081F98
	public void SetEmergeFraction(float fraction)
	{
		for (int i = 0; i < this.numBees; i++)
		{
			this.bees[i].transform.localPosition = Vector3.forward * fraction * this.beeOrbitalRadii[i];
		}
	}

	// Token: 0x040006E8 RID: 1768
	[SerializeField]
	private GameObject beePrefab;

	// Token: 0x040006E9 RID: 1769
	[SerializeField]
	private int numBees;

	// Token: 0x040006EA RID: 1770
	[SerializeField]
	private float orbitMinRadius;

	// Token: 0x040006EB RID: 1771
	[SerializeField]
	private float orbitMaxRadius;

	// Token: 0x040006EC RID: 1772
	[SerializeField]
	private float orbitMaxHeightDisplacement;

	// Token: 0x040006ED RID: 1773
	[SerializeField]
	private float orbitMaxCenterDisplacement;

	// Token: 0x040006EE RID: 1774
	[SerializeField]
	private float orbitMaxTilt;

	// Token: 0x040006EF RID: 1775
	[SerializeField]
	private float orbitSpeed;

	// Token: 0x040006F0 RID: 1776
	[SerializeField]
	private float beeScale;

	// Token: 0x040006F1 RID: 1777
	private GameObject[] beeOrbits;

	// Token: 0x040006F2 RID: 1778
	private GameObject[] bees;

	// Token: 0x040006F3 RID: 1779
	private Vector3[] beeOrbitalAxes;

	// Token: 0x040006F4 RID: 1780
	private float[] beeOrbitalRadii;
}
