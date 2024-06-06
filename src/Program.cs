using System;

class Program
{
    static int FindFirstMatchIndex(string inputLine, string pattern, bool startFlag = false, bool endFlag = false)
    {
        if (pattern == "$" && inputLine == "")
            return 0;

        if (pattern == "\\d" || pattern == "\\w")
        {
            for (int i = 0; i < inputLine.Length; i++)
            {
                char currentChar = inputLine[i];
                if ((pattern == "\\d" && char.IsDigit(currentChar)) ||
                    (pattern == "\\w" && (char.IsLetterOrDigit(currentChar))))
                {
                    if (!startFlag || (startFlag && i == 0))
                        return i + 1;
                }
            }
        }
        else if (pattern.StartsWith("[") && pattern.EndsWith("]"))
        {
            if (pattern[1] == '^')
            {
                string negativePattern = pattern.Substring(2, pattern.Length - 3);
                foreach (char c in negativePattern)
                {
                    if (inputLine.Contains(c))
                    {
                        if (!startFlag || (startFlag && negativePattern.IndexOf(c) == 0))
                            return -1;
                    }
                }
                return 0;
            }
            else
            {
                string positivePattern = pattern.Substring(1, pattern.Length - 2);
                foreach (char c in positivePattern)
                {
                    int idx = inputLine.IndexOf(c);
                    if (idx != -1)
                    {
                        if (!startFlag || (startFlag && idx == 0))
                            return idx + 1;
                    }
                }
            }
        }
        else
        {
            int idx = inputLine.IndexOf(pattern);
            if (idx >= 0)
            {
                if (!startFlag || (startFlag && idx == 0))
                    return idx + 1;
            }
        }

        return -1;
    }

    static bool MatchPatternSequence(string inputLine, string pattern)
    {
        bool startFlag = false;
        bool endFlag = false;

        while (!string.IsNullOrEmpty(pattern))
        {
            if (pattern.StartsWith("^"))
            {
                startFlag = true;
                pattern = pattern.Substring(1);
            }

            if (pattern.EndsWith("$"))
            {
                endFlag = true;
                pattern = pattern.Substring(0, pattern.Length - 1);
            }

            string currentPattern;
            if (pattern.StartsWith("\\"))
            {
                currentPattern = pattern.Substring(0, 2);
                pattern = pattern.Substring(2);
            }
            else if (pattern.StartsWith("[") && pattern.EndsWith("]"))
            {
                int closingIndex = pattern.IndexOf("]") + 1;
                currentPattern = pattern.Substring(0, closingIndex);
                pattern = pattern.Substring(closingIndex);
            }
            else
            {
                currentPattern = pattern.Substring(0, 1);
                pattern = pattern.Substring(1);
            }

            if (!string.IsNullOrEmpty(pattern) && pattern[0] == '.')
            {
                pattern = "^" + currentPattern + pattern;
            }

            if (!string.IsNullOrEmpty(pattern) && pattern[0] == '+')
            {
                pattern = pattern.Substring(1);
                int matchLen = 0;
                while (true)
                {
                    int inputStartPos = FindFirstMatchIndex(inputLine, currentPattern, startFlag, endFlag);
                    if (inputStartPos < 0)
                    {
                        if (matchLen > 0)
                            break;
                        else
                            return false;
                    }
                    else
                    {
                        matchLen++;
                        inputLine = inputLine.Substring(inputStartPos);
                    }
                }
            }
            else
            {
                int inputStartPos = FindFirstMatchIndex(inputLine, currentPattern, startFlag, endFlag);
                if (inputStartPos < 0)
                    return false;
                inputLine = inputLine.Substring(inputStartPos);
            }
        }

        return true;
    }

    static void Main(string[] args)
    {
        if (args.Length < 2 || args[0] != "-E")
        {
            Console.WriteLine("Expected first argument to be '-E'");
            Environment.Exit(2);
        }

        string pattern = args[1];
        string inputLine = Console.In.ReadToEnd().Trim();

        Console.WriteLine("Logs from your program will appear here!");

        if (MatchPatternSequence(inputLine, pattern))
            Environment.Exit(0);
        else
            Environment.Exit(1);
    }
}
