using System;
using System.Collections.Generic;
using System.IO;

namespace LitJson
{
	// Token: 0x0200096F RID: 2415
	public class JsonReader
	{
		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06003AD9 RID: 15065 RVA: 0x00056512 File Offset: 0x00054712
		// (set) Token: 0x06003ADA RID: 15066 RVA: 0x0005651F File Offset: 0x0005471F
		public bool AllowComments
		{
			get
			{
				return this.lexer.AllowComments;
			}
			set
			{
				this.lexer.AllowComments = value;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06003ADB RID: 15067 RVA: 0x0005652D File Offset: 0x0005472D
		// (set) Token: 0x06003ADC RID: 15068 RVA: 0x0005653A File Offset: 0x0005473A
		public bool AllowSingleQuotedStrings
		{
			get
			{
				return this.lexer.AllowSingleQuotedStrings;
			}
			set
			{
				this.lexer.AllowSingleQuotedStrings = value;
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06003ADD RID: 15069 RVA: 0x00056548 File Offset: 0x00054748
		public bool EndOfInput
		{
			get
			{
				return this.end_of_input;
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06003ADE RID: 15070 RVA: 0x00056550 File Offset: 0x00054750
		public bool EndOfJson
		{
			get
			{
				return this.end_of_json;
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06003ADF RID: 15071 RVA: 0x00056558 File Offset: 0x00054758
		public JsonToken Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06003AE0 RID: 15072 RVA: 0x00056560 File Offset: 0x00054760
		public object Value
		{
			get
			{
				return this.token_value;
			}
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x00056568 File Offset: 0x00054768
		static JsonReader()
		{
			JsonReader.PopulateParseTable();
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x0005656F File Offset: 0x0005476F
		public JsonReader(string json_text) : this(new StringReader(json_text), true)
		{
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x0005657E File Offset: 0x0005477E
		public JsonReader(TextReader reader) : this(reader, false)
		{
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x0014E7AC File Offset: 0x0014C9AC
		private JsonReader(TextReader reader, bool owned)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.read_started = false;
			this.automaton_stack = new Stack<int>();
			this.automaton_stack.Push(65553);
			this.automaton_stack.Push(65543);
			this.lexer = new Lexer(reader);
			this.end_of_input = false;
			this.end_of_json = false;
			this.reader = reader;
			this.reader_is_owned = owned;
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x0014E838 File Offset: 0x0014CA38
		private static void PopulateParseTable()
		{
			JsonReader.parse_table = new Dictionary<int, IDictionary<int, int[]>>();
			JsonReader.TableAddRow(ParserToken.Array);
			JsonReader.TableAddCol(ParserToken.Array, 91, new int[]
			{
				91,
				65549
			});
			JsonReader.TableAddRow(ParserToken.ArrayPrime);
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 34, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 91, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 93, new int[]
			{
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 123, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65537, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65538, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65539, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65540, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddRow(ParserToken.Object);
			JsonReader.TableAddCol(ParserToken.Object, 123, new int[]
			{
				123,
				65545
			});
			JsonReader.TableAddRow(ParserToken.ObjectPrime);
			JsonReader.TableAddCol(ParserToken.ObjectPrime, 34, new int[]
			{
				65546,
				65547,
				125
			});
			JsonReader.TableAddCol(ParserToken.ObjectPrime, 125, new int[]
			{
				125
			});
			JsonReader.TableAddRow(ParserToken.Pair);
			JsonReader.TableAddCol(ParserToken.Pair, 34, new int[]
			{
				65552,
				58,
				65550
			});
			JsonReader.TableAddRow(ParserToken.PairRest);
			JsonReader.TableAddCol(ParserToken.PairRest, 44, new int[]
			{
				44,
				65546,
				65547
			});
			JsonReader.TableAddCol(ParserToken.PairRest, 125, new int[]
			{
				65554
			});
			JsonReader.TableAddRow(ParserToken.String);
			JsonReader.TableAddCol(ParserToken.String, 34, new int[]
			{
				34,
				65541,
				34
			});
			JsonReader.TableAddRow(ParserToken.Text);
			JsonReader.TableAddCol(ParserToken.Text, 91, new int[]
			{
				65548
			});
			JsonReader.TableAddCol(ParserToken.Text, 123, new int[]
			{
				65544
			});
			JsonReader.TableAddRow(ParserToken.Value);
			JsonReader.TableAddCol(ParserToken.Value, 34, new int[]
			{
				65552
			});
			JsonReader.TableAddCol(ParserToken.Value, 91, new int[]
			{
				65548
			});
			JsonReader.TableAddCol(ParserToken.Value, 123, new int[]
			{
				65544
			});
			JsonReader.TableAddCol(ParserToken.Value, 65537, new int[]
			{
				65537
			});
			JsonReader.TableAddCol(ParserToken.Value, 65538, new int[]
			{
				65538
			});
			JsonReader.TableAddCol(ParserToken.Value, 65539, new int[]
			{
				65539
			});
			JsonReader.TableAddCol(ParserToken.Value, 65540, new int[]
			{
				65540
			});
			JsonReader.TableAddRow(ParserToken.ValueRest);
			JsonReader.TableAddCol(ParserToken.ValueRest, 44, new int[]
			{
				44,
				65550,
				65551
			});
			JsonReader.TableAddCol(ParserToken.ValueRest, 93, new int[]
			{
				65554
			});
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x00056588 File Offset: 0x00054788
		private static void TableAddCol(ParserToken row, int col, params int[] symbols)
		{
			JsonReader.parse_table[(int)row].Add(col, symbols);
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x0005659C File Offset: 0x0005479C
		private static void TableAddRow(ParserToken rule)
		{
			JsonReader.parse_table.Add((int)rule, new Dictionary<int, int[]>());
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x0014EBB4 File Offset: 0x0014CDB4
		private void ProcessNumber(string number)
		{
			double num;
			if ((number.IndexOf('.') != -1 || number.IndexOf('e') != -1 || number.IndexOf('E') != -1) && double.TryParse(number, out num))
			{
				this.token = JsonToken.Double;
				this.token_value = num;
				return;
			}
			int num2;
			if (int.TryParse(number, out num2))
			{
				this.token = JsonToken.Int;
				this.token_value = num2;
				return;
			}
			long num3;
			if (long.TryParse(number, out num3))
			{
				this.token = JsonToken.Long;
				this.token_value = num3;
				return;
			}
			this.token = JsonToken.Int;
			this.token_value = 0;
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x0014EC50 File Offset: 0x0014CE50
		private void ProcessSymbol()
		{
			if (this.current_symbol == 91)
			{
				this.token = JsonToken.ArrayStart;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 93)
			{
				this.token = JsonToken.ArrayEnd;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 123)
			{
				this.token = JsonToken.ObjectStart;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 125)
			{
				this.token = JsonToken.ObjectEnd;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 34)
			{
				if (this.parser_in_string)
				{
					this.parser_in_string = false;
					this.parser_return = true;
					return;
				}
				if (this.token == JsonToken.None)
				{
					this.token = JsonToken.String;
				}
				this.parser_in_string = true;
				return;
			}
			else
			{
				if (this.current_symbol == 65541)
				{
					this.token_value = this.lexer.StringValue;
					return;
				}
				if (this.current_symbol == 65539)
				{
					this.token = JsonToken.Boolean;
					this.token_value = false;
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65540)
				{
					this.token = JsonToken.Null;
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65537)
				{
					this.ProcessNumber(this.lexer.StringValue);
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65546)
				{
					this.token = JsonToken.PropertyName;
					return;
				}
				if (this.current_symbol == 65538)
				{
					this.token = JsonToken.Boolean;
					this.token_value = true;
					this.parser_return = true;
				}
				return;
			}
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x000565AE File Offset: 0x000547AE
		private bool ReadToken()
		{
			if (this.end_of_input)
			{
				return false;
			}
			this.lexer.NextToken();
			if (this.lexer.EndOfInput)
			{
				this.Close();
				return false;
			}
			this.current_input = this.lexer.Token;
			return true;
		}

		// Token: 0x06003AEB RID: 15083 RVA: 0x000565ED File Offset: 0x000547ED
		public void Close()
		{
			if (this.end_of_input)
			{
				return;
			}
			this.end_of_input = true;
			this.end_of_json = true;
			if (this.reader_is_owned)
			{
				this.reader.Close();
			}
			this.reader = null;
		}

		// Token: 0x06003AEC RID: 15084 RVA: 0x0014EDC4 File Offset: 0x0014CFC4
		public bool Read()
		{
			if (this.end_of_input)
			{
				return false;
			}
			if (this.end_of_json)
			{
				this.end_of_json = false;
				this.automaton_stack.Clear();
				this.automaton_stack.Push(65553);
				this.automaton_stack.Push(65543);
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.token = JsonToken.None;
			this.token_value = null;
			if (!this.read_started)
			{
				this.read_started = true;
				if (!this.ReadToken())
				{
					return false;
				}
			}
			while (!this.parser_return)
			{
				this.current_symbol = this.automaton_stack.Pop();
				this.ProcessSymbol();
				if (this.current_symbol == this.current_input)
				{
					if (!this.ReadToken())
					{
						if (this.automaton_stack.Peek() != 65553)
						{
							throw new JsonException("Input doesn't evaluate to proper JSON text");
						}
						return this.parser_return;
					}
				}
				else
				{
					int[] array;
					try
					{
						array = JsonReader.parse_table[this.current_symbol][this.current_input];
					}
					catch (KeyNotFoundException inner_exception)
					{
						throw new JsonException((ParserToken)this.current_input, inner_exception);
					}
					if (array[0] != 65554)
					{
						for (int i = array.Length - 1; i >= 0; i--)
						{
							this.automaton_stack.Push(array[i]);
						}
					}
				}
			}
			if (this.automaton_stack.Peek() == 65553)
			{
				this.end_of_json = true;
			}
			return true;
		}

		// Token: 0x04003C1A RID: 15386
		private static IDictionary<int, IDictionary<int, int[]>> parse_table;

		// Token: 0x04003C1B RID: 15387
		private Stack<int> automaton_stack;

		// Token: 0x04003C1C RID: 15388
		private int current_input;

		// Token: 0x04003C1D RID: 15389
		private int current_symbol;

		// Token: 0x04003C1E RID: 15390
		private bool end_of_json;

		// Token: 0x04003C1F RID: 15391
		private bool end_of_input;

		// Token: 0x04003C20 RID: 15392
		private Lexer lexer;

		// Token: 0x04003C21 RID: 15393
		private bool parser_in_string;

		// Token: 0x04003C22 RID: 15394
		private bool parser_return;

		// Token: 0x04003C23 RID: 15395
		private bool read_started;

		// Token: 0x04003C24 RID: 15396
		private TextReader reader;

		// Token: 0x04003C25 RID: 15397
		private bool reader_is_owned;

		// Token: 0x04003C26 RID: 15398
		private object token_value;

		// Token: 0x04003C27 RID: 15399
		private JsonToken token;
	}
}
