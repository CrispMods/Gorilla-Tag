using System;
using System.Globalization;
using System.Reflection;

// Token: 0x020008A5 RID: 2213
public class ProxyType : Type
{
	// Token: 0x06003584 RID: 13700 RVA: 0x000FE584 File Offset: 0x000FC784
	public ProxyType()
	{
	}

	// Token: 0x06003585 RID: 13701 RVA: 0x000FE59C File Offset: 0x000FC79C
	public ProxyType(string typeName)
	{
		this._typeName = typeName;
	}

	// Token: 0x17000576 RID: 1398
	// (get) Token: 0x06003586 RID: 13702 RVA: 0x000FE5BB File Offset: 0x000FC7BB
	public override string Name
	{
		get
		{
			return this._typeName;
		}
	}

	// Token: 0x17000577 RID: 1399
	// (get) Token: 0x06003587 RID: 13703 RVA: 0x000FE5C3 File Offset: 0x000FC7C3
	public override string FullName
	{
		get
		{
			return ProxyType.kPrefix + this._typeName;
		}
	}

	// Token: 0x06003588 RID: 13704 RVA: 0x000FE5D8 File Offset: 0x000FC7D8
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

	// Token: 0x06003589 RID: 13705 RVA: 0x000FE664 File Offset: 0x000FC864
	public override string ToString()
	{
		return base.ToString() + "." + this._typeName;
	}

	// Token: 0x0600358A RID: 13706 RVA: 0x000FE67C File Offset: 0x000FC87C
	public override object[] GetCustomAttributes(bool inherit)
	{
		return this._self.GetCustomAttributes(inherit);
	}

	// Token: 0x0600358B RID: 13707 RVA: 0x000FE68A File Offset: 0x000FC88A
	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		return this._self.GetCustomAttributes(attributeType, inherit);
	}

	// Token: 0x0600358C RID: 13708 RVA: 0x000FE699 File Offset: 0x000FC899
	public override bool IsDefined(Type attributeType, bool inherit)
	{
		return this._self.IsDefined(attributeType, inherit);
	}

	// Token: 0x17000578 RID: 1400
	// (get) Token: 0x0600358D RID: 13709 RVA: 0x000FE6A8 File Offset: 0x000FC8A8
	public override Module Module
	{
		get
		{
			return this._self.Module;
		}
	}

	// Token: 0x17000579 RID: 1401
	// (get) Token: 0x0600358E RID: 13710 RVA: 0x000FE6B5 File Offset: 0x000FC8B5
	public override string Namespace
	{
		get
		{
			return this._self.Namespace;
		}
	}

	// Token: 0x0600358F RID: 13711 RVA: 0x00002076 File Offset: 0x00000276
	protected override TypeAttributes GetAttributeFlagsImpl()
	{
		return TypeAttributes.NotPublic;
	}

	// Token: 0x06003590 RID: 13712 RVA: 0x00042E31 File Offset: 0x00041031
	protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		return null;
	}

	// Token: 0x06003591 RID: 13713 RVA: 0x000FE6C2 File Offset: 0x000FC8C2
	public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
	{
		return this._self.GetConstructors(bindingAttr);
	}

	// Token: 0x06003592 RID: 13714 RVA: 0x000FE6D0 File Offset: 0x000FC8D0
	public override Type GetElementType()
	{
		return this._self.GetElementType();
	}

	// Token: 0x06003593 RID: 13715 RVA: 0x000FE6DD File Offset: 0x000FC8DD
	public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
	{
		return this._self.GetEvent(name, bindingAttr);
	}

	// Token: 0x06003594 RID: 13716 RVA: 0x000FE6EC File Offset: 0x000FC8EC
	public override EventInfo[] GetEvents(BindingFlags bindingAttr)
	{
		return this._self.GetEvents(bindingAttr);
	}

	// Token: 0x06003595 RID: 13717 RVA: 0x000FE6FA File Offset: 0x000FC8FA
	public override FieldInfo GetField(string name, BindingFlags bindingAttr)
	{
		return this._self.GetField(name, bindingAttr);
	}

	// Token: 0x06003596 RID: 13718 RVA: 0x000FE709 File Offset: 0x000FC909
	public override FieldInfo[] GetFields(BindingFlags bindingAttr)
	{
		return this._self.GetFields(bindingAttr);
	}

	// Token: 0x06003597 RID: 13719 RVA: 0x000FE717 File Offset: 0x000FC917
	public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
	{
		return this._self.GetMembers(bindingAttr);
	}

	// Token: 0x06003598 RID: 13720 RVA: 0x00042E31 File Offset: 0x00041031
	protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		return null;
	}

	// Token: 0x06003599 RID: 13721 RVA: 0x000FE725 File Offset: 0x000FC925
	public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
	{
		return this._self.GetMethods(bindingAttr);
	}

	// Token: 0x0600359A RID: 13722 RVA: 0x000FE733 File Offset: 0x000FC933
	public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
	{
		return this._self.GetProperties(bindingAttr);
	}

	// Token: 0x0600359B RID: 13723 RVA: 0x000FE744 File Offset: 0x000FC944
	public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
	{
		return this._self.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
	}

	// Token: 0x1700057A RID: 1402
	// (get) Token: 0x0600359C RID: 13724 RVA: 0x000FE769 File Offset: 0x000FC969
	public override Type UnderlyingSystemType
	{
		get
		{
			return this._self.UnderlyingSystemType;
		}
	}

	// Token: 0x0600359D RID: 13725 RVA: 0x00002076 File Offset: 0x00000276
	protected override bool IsArrayImpl()
	{
		return false;
	}

	// Token: 0x0600359E RID: 13726 RVA: 0x00002076 File Offset: 0x00000276
	protected override bool IsByRefImpl()
	{
		return false;
	}

	// Token: 0x0600359F RID: 13727 RVA: 0x00002076 File Offset: 0x00000276
	protected override bool IsCOMObjectImpl()
	{
		return false;
	}

	// Token: 0x060035A0 RID: 13728 RVA: 0x00002076 File Offset: 0x00000276
	protected override bool IsPointerImpl()
	{
		return false;
	}

	// Token: 0x060035A1 RID: 13729 RVA: 0x00002076 File Offset: 0x00000276
	protected override bool IsPrimitiveImpl()
	{
		return false;
	}

	// Token: 0x1700057B RID: 1403
	// (get) Token: 0x060035A2 RID: 13730 RVA: 0x000FE776 File Offset: 0x000FC976
	public override Assembly Assembly
	{
		get
		{
			return this._self.Assembly;
		}
	}

	// Token: 0x1700057C RID: 1404
	// (get) Token: 0x060035A3 RID: 13731 RVA: 0x000FE783 File Offset: 0x000FC983
	public override string AssemblyQualifiedName
	{
		get
		{
			return this._self.AssemblyQualifiedName.Replace("ProxyType", this.FullName);
		}
	}

	// Token: 0x1700057D RID: 1405
	// (get) Token: 0x060035A4 RID: 13732 RVA: 0x000FE7A0 File Offset: 0x000FC9A0
	public override Type BaseType
	{
		get
		{
			return this._self.BaseType;
		}
	}

	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x060035A5 RID: 13733 RVA: 0x000FE7AD File Offset: 0x000FC9AD
	public override Guid GUID
	{
		get
		{
			return this._self.GUID;
		}
	}

	// Token: 0x060035A6 RID: 13734 RVA: 0x00042E31 File Offset: 0x00041031
	protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
	{
		return null;
	}

	// Token: 0x060035A7 RID: 13735 RVA: 0x00002076 File Offset: 0x00000276
	protected override bool HasElementTypeImpl()
	{
		return false;
	}

	// Token: 0x060035A8 RID: 13736 RVA: 0x000FE7BA File Offset: 0x000FC9BA
	public override Type GetNestedType(string name, BindingFlags bindingAttr)
	{
		return this._self.GetNestedType(name, bindingAttr);
	}

	// Token: 0x060035A9 RID: 13737 RVA: 0x000FE7C9 File Offset: 0x000FC9C9
	public override Type[] GetNestedTypes(BindingFlags bindingAttr)
	{
		return this._self.GetNestedTypes(bindingAttr);
	}

	// Token: 0x060035AA RID: 13738 RVA: 0x000FE7D7 File Offset: 0x000FC9D7
	public override Type GetInterface(string name, bool ignoreCase)
	{
		return this._self.GetInterface(name, ignoreCase);
	}

	// Token: 0x060035AB RID: 13739 RVA: 0x000FE7E6 File Offset: 0x000FC9E6
	public override Type[] GetInterfaces()
	{
		return this._self.GetInterfaces();
	}

	// Token: 0x040037CA RID: 14282
	private Type _self = typeof(ProxyType);

	// Token: 0x040037CB RID: 14283
	private readonly string _typeName;

	// Token: 0x040037CC RID: 14284
	private static readonly string kPrefix = "ProxyType.";

	// Token: 0x040037CD RID: 14285
	private static InvalidType kInvalidType = new InvalidType();
}
