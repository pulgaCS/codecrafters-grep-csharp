using System;
using System.IO;
using System.Text;

class MyGrep
{
    const string digitClass = "\\d";
    const string alphaNumericClass = "\\w";

    static void Main(string[] args)
    {
        if (args.Length < 2 || args[0] != "-E")
        {
            Console.Error.WriteLine("usage: mygrep -E <pattern>");
            Environment.Exit(2); // 1 means no lines were selected, >1 means error
        }

        string pattern = args[1];
        string input = Console.In.ReadToEnd();

        bool ok = Match(Encoding.UTF8.GetBytes(input), pattern);

        if (!ok)
        {
            Environment.Exit(1);
        }

        // default exit code is 0 which means success
    }

    static bool Match(byte[] line, string pattern)
    {
        //try to match first char
        if (pattern[0] == '^')
        {
            return MatchUtil(line, pattern.Substring(1));
        }
        else if (pattern[0] == '\\')
        {
            if (pattern[1] == 'd')
            {
                return ContainsDigitAndMatch(line, pattern.Substring(2));
            }
            else if (pattern[1] == 'w')
            {
                return ContainsAlphaNumericAndMatch(line, pattern.Substring(2));
            }
            return false;
        }
        else if (pattern[0] == '[')
        {
            return MatchCharacterClass(line, pattern);
        }
        return MatchUtil(line, pattern);
    }

    static bool MatchUtil(byte[] line, string pattern)
    {
        int lineIndx = 0;
        int i = 0;
        int lastPatternIndexStart = 0;
        for (i = 0; i < pattern.Length && lineIndx < line.Length; i++)
        {
            char r = pattern[i];
            int classStartIndex = i;
            if (r == '\\')
            {
                if (pattern[i + 1] == 'd')
                {
                    if (!IsDigit((char)line[lineIndx]))
                    {
                        return false;
                    }
                }
                else if (pattern[i + 1] == 'w')
                {
                    if (!IsAlphaNumeric((char)line[lineIndx]))
                    {
                        return false;
                    }
                }
                lineIndx++;
                i += 1;
            }
            else if (r == '[')
            {
                int classEndIndx = pattern.IndexOf(']', i + 1);
                string allChars = pattern.Substring(i + 1, classEndIndx - i - 1);
                bool notCheck = allChars[0] == '^';
                if (notCheck)
                {
                    allChars = allChars.Substring(1);
                }
                bool result = allChars.Contains((char)line[lineIndx]) != notCheck;
                if (!result)
                {
                    return false;
                }
                lineIndx++;
                i = classEndIndx;
            }
            else if (r == '$')
            {
                return line.Length == lineIndx;
            }
            else if (r == (char)line[lineIndx])
            {
                lineIndx++;
            }
            else if (r == '+')
            {
                if (i != 0 && MatchUtil(SubArray(line, lineIndx), pattern.Substring(lastPatternIndexStart)))
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
            lastPatternIndexStart = classStartIndex;
        }
        return i == pattern.Length || (pattern[i] == '$'); // for loop breaking conditions simplifies the logic
    }

    static bool ContainsDigitAndMatch(byte[] line, string pattern)
    {
        while (true)
        {
            int indx = IndexOfDigit(line);
            if (indx == -1)
            {
                return false;
            }
            if (MatchUtil(SubArray(line, indx + 1), pattern))
            {
                return true;
            }
            line = SubArray(line, indx + 1);
        }
    }

    static bool ContainsAlphaNumericAndMatch(byte[] line, string pattern)
    {
        while (true)
        {
            int indx = IndexOfAlphaNumeric(line);
            if (indx == -1)
            {
                return false;
            }
            if (MatchUtil(SubArray(line, indx + 1), pattern))
            {
                return true;
            }
            line = SubArray(line, indx + 1);
        }
    }

    static bool MatchCharacterClass(byte[] line, string pattern)
    {
        int classEndIndx = pattern.IndexOf(']');
        string allChars = pattern.Substring(1, classEndIndx - 1);
        bool notCheck = allChars[0] == '^';
        if (notCheck)
        {
            allChars = allChars.Substring(1);
        }
        while (true)
        {
            int indx = IndexOfAny(line, allChars.ToCharArray());
            if ((indx == -1) == notCheck)
            {
                if (MatchUtil(SubArray(line, indx + 1), pattern.Substring(classEndIndx + 1)))
                {
                    return true;
                }
            }
            else if (!notCheck)
            {
                return false;
            }
            line = SubArray(line, indx + 1);
        }
    }

    static int IndexOfDigit(byte[] line)
    {
        for (int i = 0; i < line.Length; i++)
        {
            if (IsDigit((char)line[i]))
            {
                return i;
            }
        }
        return -1;
    }

    static int IndexOfAlphaNumeric(byte[] line)
    {
        for (int i = 0; i < line.Length; i++)
        {
            if (IsAlphaNumeric((char)line[i]))
            {
                return i;
            }
        }
        return -1;
    }

    static int IndexOfAny(byte[] line, char[] anyOf)
    {
        for (int i = 0; i < line.Length; i++)
        {
            if (Array.Exists(anyOf, element => element == (char)line[i]))
            {
                return i;
            }
        }
        return -1;
    }

    static bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    static bool IsAlphaNumeric(char c)
    {
        return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || IsDigit(c);
    }

    static byte[] SubArray(byte[] data, int index)
    {
        int length = data.Length - index;
        byte[] result = new byte[length];
        Array.Copy(data, index, result, 0, length);
        return result;
    }
}
