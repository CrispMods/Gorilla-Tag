using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace LitJson
{
	// Token: 0x02000972 RID: 2418
	public class JsonWriter
	{
		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06003AEE RID: 15086 RVA: 0x00056620 File Offset: 0x00054820
		// (set) Token: 0x06003AEF RID: 15087 RVA: 0x00056628 File Offset: 0x00054828
		public int IndentValue
		{
			get
			{
				return this.indent_value;
			}
			set
			{
				this.indentation = this.indentation / this.indent_value * value;
				this.indent_value = value;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06003AF0 RID: 15088 RVA: 0x00056646 File Offset: 0x00054846
		// (set) Token: 0x06003AF1 RID: 15089 RVA: 0x0005664E File Offset: 0x0005484E
		public bool PrettyPrint
		{
			get
			{
				return this.pretty_print;
			}
			set
			{
				this.pretty_print = value;
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06003AF2 RID: 15090 RVA: 0x00056657 File Offset: 0x00054857
		public TextWriter TextWriter
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06003AF3 RID: 15091 RVA: 0x0005665F File Offset: 0x0005485F
		// (set) Token: 0x06003AF4 RID: 15092 RVA: 0x00056667 File Offset: 0x00054867
		public bool Validate
		{
			get
			{
				return this.validate;
			}
			set
			{
				this.validate = value;
			}
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x0005667C File Offset: 0x0005487C
		public JsonWriter()
		{
			this.inst_string_builder = new StringBuilder();
			this.writer = new StringWriter(this.inst_string_builder);
			this.Init();
		}

		// Token: 0x06003AF7 RID: 15095 RVA: 0x000566A6 File Offset: 0x000548A6
		public JsonWriter(StringBuilder sb) : this(new StringWriter(sb))
		{
		}

		// Token: 0x06003AF8 RID: 15096 RVA: 0x000566B4 File Offset: 0x000548B4
		public JsonWriter(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
			this.Init();
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x0014EF30 File Offset: 0x0014D130
		private void DoValidation(Condition cond)
		{
			if (!this.context.ExpectingValue)
			{
				this.context.Count++;
			}
			if (!this.validate)
			{
				return;
			}
			if (this.has_reached_end)
			{
				throw new JsonException("A complete JSON symbol has already been written");
			}
			switch (cond)
			{
			case Condition.InArray:
				if (!this.context.InArray)
				{
					throw new JsonException("Can't close an array here");
				}
				break;
			case Condition.InObject:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't close an object here");
				}
				break;
			case Condition.NotAProperty:
				if (this.context.InObject && !this.context.ExpectingValue)
				{
					throw new JsonException("Expected a property");
				}
				break;
			case Condition.Property:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't add a property here");
				}
				break;
			case Condition.Value:
				if (!this.context.InArray && (!this.context.InObject || !this.context.ExpectingValue))
				{
					throw new JsonException("Can't add a value here");
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x0014F054 File Offset: 0x0014D254
		private void Init()
		{
			this.has_reached_end = false;
			this.hex_seq = new char[4];
			this.indentation = 0;
			this.indent_value = 4;
			this.pretty_print = false;
			this.validate = true;
			this.ctx_stack = new Stack<WriterContext>();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x0014F0B8 File Offset: 0x0014D2B8
		private static void IntToHex(int n, char[] hex)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = n % 16;
				if (num < 10)
				{
					hex[3 - i] = (char)(48 + num);
				}
				else
				{
					hex[3 - i] = (char)(65 + (num - 10));
				}
				n >>= 4;
			}
		}

		// Token: 0x06003AFC RID: 15100 RVA: 0x000566D7 File Offset: 0x000548D7
		private void Indent()
		{
			if (this.pretty_print)
			{
				this.indentation += this.indent_value;
			}
		}

		// Token: 0x06003AFD RID: 15101 RVA: 0x0014F0FC File Offset: 0x0014D2FC
		private void Put(string str)
		{
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				for (int i = 0; i < this.indentation; i++)
				{
					this.writer.Write(' ');
				}
			}
			this.writer.Write(str);
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x000566F4 File Offset: 0x000548F4
		private void PutNewline()
		{
			this.PutNewline(true);
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x0014F148 File Offset: 0x0014D348
		private void PutNewline(bool add_comma)
		{
			if (add_comma && !this.context.ExpectingValue && this.context.Count > 1)
			{
				this.writer.Write(',');
			}
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				this.writer.Write('\n');
			}
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x0014F1A4 File Offset: 0x0014D3A4
		private void PutString(string str)
		{
			this.Put(string.Empty);
			this.writer.Write('"');
			int length = str.Length;
			int i = 0;
			while (i < length)
			{
				char c = str[i];
				switch (c)
				{
				case '\b':
					this.writer.Write("\\b");
					break;
				case '\t':
					this.writer.Write("\\t");
					break;
				case '\n':
					this.writer.Write("\\n");
					break;
				case '\v':
					goto IL_E4;
				case '\f':
					this.writer.Write("\\f");
					break;
				case '\r':
					this.writer.Write("\\r");
					break;
				default:
					if (c != '"' && c != '\\')
					{
						goto IL_E4;
					}
					this.writer.Write('\\');
					this.writer.Write(str[i]);
					break;
				}
				IL_141:
				i++;
				continue;
				IL_E4:
				if (str[i] >= ' ' && str[i] <= '~')
				{
					this.writer.Write(str[i]);
					goto IL_141;
				}
				JsonWriter.IntToHex((int)str[i], this.hex_seq);
				this.writer.Write("\\u");
				this.writer.Write(this.hex_seq);
				goto IL_141;
			}
			this.writer.Write('"');
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x000566FD File Offset: 0x000548FD
		private void Unindent()
		{
			if (this.pretty_print)
			{
				this.indentation -= this.indent_value;
			}
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x0005671A File Offset: 0x0005491A
		public override string ToString()
		{
			if (this.inst_string_builder == null)
			{
				return string.Empty;
			}
			return this.inst_string_builder.ToString();
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x0014F30C File Offset: 0x0014D50C
		public void Reset()
		{
			this.has_reached_end = false;
			this.ctx_stack.Clear();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
			if (this.inst_string_builder != null)
			{
				this.inst_string_builder.Remove(0, this.inst_string_builder.Length);
			}
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x00056735 File Offset: 0x00054935
		public void Write(bool boolean)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(boolean ? "true" : "false");
			this.context.ExpectingValue = false;
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x00056765 File Offset: 0x00054965
		public void Write(decimal number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x0014F368 File Offset: 0x0014D568
		public void Write(double number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			string text = Convert.ToString(number, JsonWriter.number_format);
			this.Put(text);
			if (text.IndexOf('.') == -1 && text.IndexOf('E') == -1)
			{
				this.writer.Write(".0");
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x00056791 File Offset: 0x00054991
		public void Write(int number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x000567BD File Offset: 0x000549BD
		public void Write(long number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x000567E9 File Offset: 0x000549E9
		public void Write(string str)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			if (str == null)
			{
				this.Put("null");
			}
			else
			{
				this.PutString(str);
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x0005681B File Offset: 0x00054A1B
		public void Write(ulong number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06003B0B RID: 15115 RVA: 0x0014F3C8 File Offset: 0x0014D5C8
		public void WriteArrayEnd()
		{
			this.DoValidation(Condition.InArray);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("]");
		}

		// Token: 0x06003B0C RID: 15116 RVA: 0x0014F434 File Offset: 0x0014D634
		public void WriteArrayStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("[");
			this.context = new WriterContext();
			this.context.InArray = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x06003B0D RID: 15117 RVA: 0x0014F488 File Offset: 0x0014D688
		public void WriteObjectEnd()
		{
			this.DoValidation(Condition.InObject);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("}");
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x0014F4F4 File Offset: 0x0014D6F4
		public void WriteObjectStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("{");
			this.context = new WriterContext();
			this.context.InObject = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x06003B0F RID: 15119 RVA: 0x0014F548 File Offset: 0x0014D748
		public void WritePropertyName(string property_name)
		{
			this.DoValidation(Condition.Property);
			this.PutNewline();
			this.PutString(property_name);
			if (this.pretty_print)
			{
				if (property_name.Length > this.context.Padding)
				{
					this.context.Padding = property_name.Length;
				}
				for (int i = this.context.Padding - property_name.Length; i >= 0; i--)
				{
					this.writer.Write(' ');
				}
				this.writer.Write(": ");
			}
			else
			{
				this.writer.Write(':');
			}
			this.context.ExpectingValue = true;
		}

		// Token: 0x04003C33 RID: 15411
		private static NumberFormatInfo number_format = NumberFormatInfo.InvariantInfo;

		// Token: 0x04003C34 RID: 15412
		private WriterContext context;

		// Token: 0x04003C35 RID: 15413
		private Stack<WriterContext> ctx_stack;

		// Token: 0x04003C36 RID: 15414
		private bool has_reached_end;

		// Token: 0x04003C37 RID: 15415
		private char[] hex_seq;

		// Token: 0x04003C38 RID: 15416
		private int indentation;

		// Token: 0x04003C39 RID: 15417
		private int indent_value;

		// Token: 0x04003C3A RID: 15418
		private StringBuilder inst_string_builder;

		// Token: 0x04003C3B RID: 15419
		private bool pretty_print;

		// Token: 0x04003C3C RID: 15420
		private bool validate;

		// Token: 0x04003C3D RID: 15421
		private TextWriter writer;
	}
}
