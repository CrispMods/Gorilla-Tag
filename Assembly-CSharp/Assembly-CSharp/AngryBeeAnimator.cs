using System;
using UnityEngine;

// Token: 0x020000DC RID: 220
public class AngryBeeAnimator : MonoBehaviour
{
	// Token: 0x060005A4 RID: 1444 RVA: 0x000213A0 File Offset: 0x0001F5A0
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
			Vector2 vector = Random.insideUnitCircle * this.orbitMaxCenterDisplacement;
			gameObject.transform.localPosition = new Vector3(vector.x, Random.Range(-this.orbitMaxHeightDisplacement, this.orbitMaxHeightDisplacement), vector.y);
			gameObject.transform.localRotation = Quaternion.Euler(Random.Range(-this.orbitMaxTilt, this.orbitMaxTilt), (float)Random.Range(0, 360), 0f);
			this.beeOrbitalAxes[i] = gameObject.transform.up;
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.beePrefab, gameObject.transform);
			float num = Random.Range(this.orbitMinRadius, this.orbitMaxRadius);
			this.beeOrbitalRadii[i] = num;
			gameObject2.transform.localPosition = Vector3.forward * num;
			gameObject2.transform.localRotation = Quaternion.Euler(-90f, 90f, 0f);
			gameObject2.transform.localScale = Vector3.one * this.beeScale;
			this.bees[i] = gameObject2;
			this.beeOrbits[i] = gameObject;
		}
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x0002153C File Offset: 0x0001F73C
	private void Update()
	{
		float angle = this.orbitSpeed * Time.deltaTime;
		for (int i = 0; i < this.numBees; i++)
		{
			this.beeOrbits[i].transform.Rotate(this.beeOrbitalAxes[i], angle);
		}
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x00021588 File Offset: 0x0001F788
	public void SetEmergeFraction(float fraction)
	{
		for (int i = 0; i < this.numBees; i++)
		{
			this.bees[i].transform.localPosition = Vector3.forward * fraction * this.beeOrbitalRadii[i];
		}
	}

	// Token: 0x040006A8 RID: 1704
	[SerializeField]
	private GameObject beePrefab;

	// Token: 0x040006A9 RID: 1705
	[SerializeField]
	private int numBees;

	// Token: 0x040006AA RID: 1706
	[SerializeField]
	private float orbitMinRadius;

	// Token: 0x040006AB RID: 1707
	[SerializeField]
	private float orbitMaxRadius;

	// Token: 0x040006AC RID: 1708
	[SerializeField]
	private float orbitMaxHeightDisplacement;

	// Token: 0x040006AD RID: 1709
	[SerializeField]
	private float orbitMaxCenterDisplacement;

	// Token: 0x040006AE RID: 1710
	[SerializeField]
	private float orbitMaxTilt;

	// Token: 0x040006AF RID: 1711
	[SerializeField]
	private float orbitSpeed;

	// Token: 0x040006B0 RID: 1712
	[SerializeField]
	private float beeScale;

	// Token: 0x040006B1 RID: 1713
	private GameObject[] beeOrbits;

	// Token: 0x040006B2 RID: 1714
	private GameObject[] bees;

	// Token: 0x040006B3 RID: 1715
	private Vector3[] beeOrbitalAxes;

	// Token: 0x040006B4 RID: 1716
	private float[] beeOrbitalRadii;
}
