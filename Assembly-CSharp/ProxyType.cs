using System;
using System.Globalization;
using System.Reflection;

// Token: 0x020008C1 RID: 2241
public class ProxyType : Type
{
	// Token: 0x0600364C RID: 13900 RVA: 0x00053C20 File Offset: 0x00051E20
	public ProxyType()
	{
	}

	// Token: 0x0600364D RID: 13901 RVA: 0x00053C38 File Offset: 0x00051E38
	public ProxyType(string typeName)
	{
		this._typeName = typeName;
	}

	// Token: 0x17000587 RID: 1415
	// (get) Token: 0x0600364E RID: 13902 RVA: 0x00053C57 File Offset: 0x00051E57
	public override string Name
	{
		get
		{
			return this._typeName;
		}
	}

	// Token: 0x17000588 RID: 1416
	// (get) Token: 0x0600364F RID: 13903 RVA: 0x00053C5F File Offset: 0x00051E5F
	public override string FullName
	{
		get
		{
			return ProxyType.kPrefix + this._typeName;
		}
	}

	// Token: 0x06003650 RID: 13904 RVA: 0x001446B4 File Offset: 0x001428B4
	public static ProxyType Parse(string input)
	{
		if (string.IsNullOrWhiteSpace(input))
		{
			throw new ArgumentNullException("input");
		}
		input = input.Trim();
		if (!input.Contains(ProxyType.kPrefix, StringComparison.InvariantCultureIgnoreCase))
		{
			return ProxyType.kInvalidType;
		}
		if (!input.StartsWith(ProxyType.kPrefix, StringComparison.InvariantCultureIgnoreCase))
		{
			return ProxyType.kInvalidType;
		}
		if (input.Contains(','))
		{
			input = input.Split(',', StringSplitOptions.None)[0];
		}
		string text = input.Split('.', StringSplitOptions.None)[1].Trim();
		if (string.IsNullOrWhiteSpace(text))
		{
			return ProxyType.kInvalidType;
		}
		return new ProxyType(text);
	}

	// Token: 0x06003651 RID: 13905 RVA: 0x00053C71 File Offset: 0x00051E71
	public override string ToString()
	{
		return base.ToString() + "." + this._typeName;
	}

	// Token: 0x06003652 RID: 13906 RVA: 0x00053C89 File Offset: 0x00051E89
	public override object[] GetCustomAttributes(bool inherit)
	{
		return this._self.GetCustomAttributes(inherit);
	}

	// Token: 0x06003653 RID: 13907 RVA: 0x00053C97 File Offset: 0x00051E97
	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		return this._self.GetCustomAttributes(attributeType, inherit);
	}

	// Token: 0x06003654 RID: 13908 RVA: 0x00053CA6 File Offset: 0x00051EA6
	public override bool IsDefined(Type attributeType, bool inherit)
	{
		return this._self.IsDefined(attributeType, inherit);
	}

	// Token: 0x17000589 RID: 1417
	// (get) Token: 0x06003655 RID: 13909 RVA: 0x00053CB5 File Offset: 0x00051EB5
	public override Module Module
	{
		get
		{
			return this._self.Module;
		}
	}

	// Token: 0x1700058A RID: 1418
	// (get) Token: 0x06003656 RID: 13910 RVA: 0x00053CC2 File Offset: 0x00051EC2
	public override string Namespace
	{
		get
		{
			return this._self.Namespace;
		}
	}

	// Token: 0x06003657 RID: 13911 RVA: 0x00030498 File Offset: 0x0002E698
	protected override TypeAttributes GetAttributeFlagsImpl()
	{
		return TypeAttributes.NotPublic;
	}

	// Token: 0x06003658 RID: 13912 RVA: 0x0003924B File Offset: 0x0003744B
	protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		return null;
	}

	// Token: 0x06003659 RID: 13913 RVA: 0x00053CCF File Offset: 0x00051ECF
	public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
	{
		return this._self.GetConstructors(bindingAttr);
	}

	// Token: 0x0600365A RID: 13914 RVA: 0x00053CDD File Offset: 0x00051EDD
	public override Type GetElementType()
	{
		return this._self.GetElementType();
	}

	// Token: 0x0600365B RID: 13915 RVA: 0x00053CEA File Offset: 0x00051EEA
	public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
	{
		return this._self.GetEvent(name, bindingAttr);
	}

	// Token: 0x0600365C RID: 13916 RVA: 0x00053CF9 File Offset: 0x00051EF9
	public override EventInfo[] GetEvents(BindingFlags bindingAttr)
	{
		return this._self.GetEvents(bindingAttr);
	}

	// Token: 0x0600365D RID: 13917 RVA: 0x00053D07 File Offset: 0x00051F07
	public override FieldInfo GetField(string name, BindingFlags bindingAttr)
	{
		return this._self.GetField(name, bindingAttr);
	}

	// Token: 0x0600365E RID: 13918 RVA: 0x00053D16 File Offset: 0x00051F16
	public override FieldInfo[] GetFields(BindingFlags bindingAttr)
	{
		return this._self.GetFields(bindingAttr);
	}

	// Token: 0x0600365F RID: 13919 RVA: 0x00053D24 File Offset: 0x00051F24
	public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
	{
		return this._self.GetMembers(bindingAttr);
	}

	// Token: 0x06003660 RID: 13920 RVA: 0x0003924B File Offset: 0x0003744B
	protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		return null;
	}

	// Token: 0x06003661 RID: 13921 RVA: 0x00053D32 File Offset: 0x00051F32
	public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
	{
		return this._self.GetMethods(bindingAttr);
	}

	// Token: 0x06003662 RID: 13922 RVA: 0x00053D40 File Offset: 0x00051F40
	public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
	{
		return this._self.GetProperties(bindingAttr);
	}

	// Token: 0x06003663 RID: 13923 RVA: 0x00144740 File Offset: 0x00142940
	public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
	{
		return this._self.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
	}

	// Token: 0x1700058B RID: 1419
	// (get) Token: 0x06003664 RID: 13924 RVA: 0x00053D4E File Offset: 0x00051F4E
	public override Type UnderlyingSystemType
	{
		get
		{
			return this._self.UnderlyingSystemType;
		}
	}

	// Token: 0x06003665 RID: 13925 RVA: 0x00030498 File Offset: 0x0002E698
	protected override bool IsArrayImpl()
	{
		return false;
	}

	// Token: 0x06003666 RID: 13926 RVA: 0x00030498 File Offset: 0x0002E698
	protected override bool IsByRefImpl()
	{
		return false;
	}

	// Token: 0x06003667 RID: 13927 RVA: 0x00030498 File Offset: 0x0002E698
	protected override bool IsCOMObjectImpl()
	{
		return false;
	}

	// Token: 0x06003668 RID: 13928 RVA: 0x00030498 File Offset: 0x0002E698
	protected override bool IsPointerImpl()
	{
		return false;
	}

	// Token: 0x06003669 RID: 13929 RVA: 0x00030498 File Offset: 0x0002E698
	protected override bool IsPrimitiveImpl()
	{
		return false;
	}

	// Token: 0x1700058C RID: 1420
	// (get) Token: 0x0600366A RID: 13930 RVA: 0x00053D5B File Offset: 0x00051F5B
	public override Assembly Assembly
	{
		get
		{
			return this._self.Assembly;
		}
	}

	// Token: 0x1700058D RID: 1421
	// (get) Token: 0x0600366B RID: 13931 RVA: 0x00053D68 File Offset: 0x00051F68
	public override string AssemblyQualifiedName
	{
		get
		{
			return this._self.AssemblyQualifiedName.Replace("ProxyType", this.FullName);
		}
	}

	// Token: 0x1700058E RID: 1422
	// (get) Token: 0x0600366C RID: 13932 RVA: 0x00053D85 File Offset: 0x00051F85
	public override Type BaseType
	{
		get
		{
			return this._self.BaseType;
		}
	}

	// Token: 0x1700058F RID: 1423
	// (get) Token: 0x0600366D RID: 13933 RVA: 0x00053D92 File Offset: 0x00051F92
	public override Guid GUID
	{
		get
		{
			return this._self.GUID;
		}
	}

	// Token: 0x0600366E RID: 13934 RVA: 0x0003924B File Offset: 0x0003744B
	protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
	{
		return null;
	}

	// Token: 0x0600366F RID: 13935 RVA: 0x00030498 File Offset: 0x0002E698
	protected override bool HasElementTypeImpl()
	{
		return false;
	}

	// Token: 0x06003670 RID: 13936 RVA: 0x00053D9F File Offset: 0x00051F9F
	public override Type GetNestedType(string name, BindingFlags bindingAttr)
	{
		return this._self.GetNestedType(name, bindingAttr);
	}

	// Token: 0x06003671 RID: 13937 RVA: 0x00053DAE File Offset: 0x00051FAE
	public override Type[] GetNestedTypes(BindingFlags bindingAttr)
	{
		return this._self.GetNestedTypes(bindingAttr);
	}

	// Token: 0x06003672 RID: 13938 RVA: 0x00053DBC File Offset: 0x00051FBC
	public override Type GetInterface(string name, bool ignoreCase)
	{
		return this._self.GetInterface(name, ignoreCase);
	}

	// Token: 0x06003673 RID: 13939 RVA: 0x00053DCB File Offset: 0x00051FCB
	public override Type[] GetInterfaces()
	{
		return this._self.GetInterfaces();
	}

	// Token: 0x0400388B RID: 14475
	private Type _self = typeof(ProxyType);

	// Token: 0x0400388C RID: 14476
	private readonly string _typeName;

	// Token: 0x0400388D RID: 14477
	private static readonly string kPrefix = "ProxyType.";

	// Token: 0x0400388E RID: 14478
	private static InvalidType kInvalidType = new InvalidType();
}
