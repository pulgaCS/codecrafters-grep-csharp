using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static List<string> Tokenize(string pattern)
    {
        string specialChars = "\\[";
        List<string> tokens = new List<string>();

        string token = "";
        string charNeeded = "";
        int lenNeeded = -1;

        void Reset()
        {
            token = "";
            charNeeded = "";
            lenNeeded = -1;
        }

        void Flush(string extra = null)
        {
            if (!string.IsNullOrEmpty(token))
                tokens.Add(token);
            if (!string.IsNullOrEmpty(extra))
                tokens.Add(extra);
            Reset();
        }

        foreach (char c in pattern)
        {
            if (specialChars.Contains(c))
                Flush();
            if (c == '\\')
                lenNeeded = 2;
            else if (c == '[')
                charNeeded = "]";

            token += c;
            if (token.Length == lenNeeded || c == charNeeded.FirstOrDefault())
                Flush();

            if (new char[] { '+' }.Contains(c))
            {
                if (token.Length > 2)
                {
                    string extra = token.Substring(token.Length - 2);
                    token = token.Substring(0, token.Length - 2);
                    Flush(extra);
                }
                else
                {
                    Flush();
                }
            }
        }

        Flush();
        return tokens;
    }

    static bool MatchPattern(string inputLine, string pattern)
    {
        bool matchStart = false;
        bool matchEnd = false;
        bool matchFailed = false;

        if (pattern.StartsWith("^"))
        {
            matchStart = true;
            pattern = pattern.Substring(1);
        }

        if (pattern.EndsWith("$"))
        {
            matchEnd = true;
            pattern = pattern.Substring(0, pattern.Length - 1);
        }

        List<string> tokens = Tokenize(pattern);
        int i = 0, j = 0;

        void ProcessMatch(bool success, int matchLength = 1)
        {
            if (success)
                j++;
            else
            {
                j = 0;
                matchFailed = true;
            }

            i += matchLength;
        }

        while (i < inputLine.Length && j < tokens.Count)
        {
            string token = tokens[j];
            bool repeatPlus = false;

            if (token.EndsWith("+"))
            {
                repeatPlus = true;
                token = token.Substring(0, token.Length - 1);
            }

            if (token == "\\d")
                ProcessMatch(char.IsDigit(inputLine[i]));
            else if (token == "\\w")
                ProcessMatch(char.IsLetterOrDigit(inputLine[i]));
            else if (token.StartsWith("[") && token.EndsWith("]"))
            {
                token = token.Trim('[', ']');
                if (token.StartsWith("^"))
                    ProcessMatch(!token.Contains(inputLine[i]));
                else
                    ProcessMatch(token.Contains(inputLine[i]));
            }
            else
            {
                if (repeatPlus)
                {
                    int temp = i;
                    while (inputLine.Substring(temp).StartsWith(token))
                        temp += 1;
                    ProcessMatch(temp > i, (temp - i) != 0 ? (temp - i) : 1);
                }
                else
                {
                    string text = inputLine.Substring(i, token.Length);
                    ProcessMatch(text == token, token.Length);
                }
            }

            if (matchStart && matchFailed)
                return false;
        }

        if (matchEnd)
            return i == inputLine.Length;
        else
            return j == tokens.Count;
    }

    static void Main(string[] args)
    {
        if (args.Length < 3 || args[0] != "-E")
        {
            Console.WriteLine("Expected first argument to be '-E'");
            Environment.Exit(1);
        }

        string pattern = args[2];
        string inputLine = Console.In.ReadToEnd().Trim(); // Read input from stdin

        if (MatchPattern(inputLine, pattern))
            Environment.Exit(0);
        else
            Environment.Exit(1);
    }

}
