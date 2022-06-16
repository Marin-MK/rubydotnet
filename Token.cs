using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rubydotnet;

public class Token
{
    public string Type;
    public string Value;
    public int Index;
    public int Length;

    public Token(string Type, string Value, int Index, int Length)
    {
        this.Type = Type;
        this.Value = Value;
        this.Index = Index;
        this.Length = Length;
    }
}
