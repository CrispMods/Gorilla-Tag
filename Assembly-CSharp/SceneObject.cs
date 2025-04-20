using System;
using UnityEngine;

// Token: 0x020008C2 RID: 2242
[Serializable]
public class SceneObject : IEquatable<SceneObject>
{
	// Token: 0x06003675 RID: 13941 RVA: 0x00053DEE File Offset: 0x00051FEE
	public Type GetObjectType()
	{
		if (string.IsNullOrWhiteSpace(this.typeString))
		{
			return null;
		}
		if (this.typeString.Contains("ProxyType"))
		{
			return ProxyType.Parse(this.typeString);
		}
		return Type.GetType(this.typeString);
	}

	// Token: 0x06003676 RID: 13942 RVA: 0x00053E28 File Offset: 0x00052028
	public SceneObject(int classID, ulong fileID)
	{
		this.classID = classID;
		this.fileID = fileID;
		this.typeString = UnityYaml.ClassIDToType[classID].AssemblyQualifiedName;
	}

	// Token: 0x06003677 RID: 13943 RVA: 0x00053E54 File Offset: 0x00052054
	public bool Equals(SceneObject other)
	{
		return this.fileID == other.fileID && this.classID == other.classID;
	}

	// Token: 0x06003678 RID: 13944 RVA: 0x00144768 File Offset: 0x00142968
	public override bool Equals(object obj)
	{
		SceneObject sceneObject = obj as SceneObject;
		return sceneObject != null && this.Equals(sceneObject);
	}

	// Token: 0x06003679 RID: 13945 RVA: 0x00144788 File Offset: 0x00142988
	public override int GetHashCode()
	{
		int i = this.classID;
		int i2 = StaticHash.Compute((long)this.fileID);
		return StaticHash.Compute(i, i2);
	}

	// Token: 0x0600367A RID: 13946 RVA: 0x00053E74 File Offset: 0x00052074
	public static bool operator ==(SceneObject x, SceneObject y)
	{
		return x.Equals(y);
	}

	// Token: 0x0600367B RID: 13947 RVA: 0x00053E7D File Offset: 0x0005207D
	public static bool operator !=(SceneObject x, SceneObject y)
	{
		return !x.Equals(y);
	}

	// Token: 0x0400388F RID: 14479
	public int classID;

	// Token: 0x04003890 RID: 14480
	public ulong fileID;

	// Token: 0x04003891 RID: 14481
	[SerializeField]
	public string typeString;

	// Token: 0x04003892 RID: 14482
	public string json;
}
