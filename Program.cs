using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace rubydotnet;

internal class Program
{
    public static void Main(params string[] Args)
    {
        string text = File.ReadAllText("D:/Desktop/ruby.rb");
        Stopwatch s = new Stopwatch();
        s.Start();
        List<Token> tokens = Tokenizer.Tokenize(text, false, false);
        s.Stop();
        StringBuilder sb = new StringBuilder();
        tokens.ForEach(t =>
        {
            sb.AppendLine($"(pos: {t.Index} len: {t.Length} type: {t.Type}, value: {t.Value})");
        });
        Console.WriteLine(sb.ToString());
        Console.WriteLine(s.ElapsedMilliseconds.ToString() + "ms");
        Console.ReadKey();
    }
}
