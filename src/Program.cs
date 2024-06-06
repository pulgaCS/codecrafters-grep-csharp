using System;
using System.Text.RegularExpressions;

public class PatternMatcher
{
    public static int FindFirstMatchIndex(string inputLine, string pattern, bool startFlag = false, bool endFlag = false)
    {
        if (pattern == "$" && inputLine == "")
            return 0;

        if (pattern == "\\d" || pattern == "\\w")
        {
            for (int idx = 0; idx < inputLine.Length; idx++)
            {
                char c = inputLine[idx];
                if ((pattern == "\\d" && char.IsDigit(c)) || (pattern == "\\w" && (char.IsLetterOrDigit(c))))
                {
                    if (!startFlag || (startFlag && idx == 0))
                        return idx + 1;
                }
            }
        }
        else if (pattern[0] == '[' && pattern[pattern.Length - 1] == ']')
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

    public static bool MatchPatternSequence(string inputLine, string pattern)
    {
        bool startFlag = false;
        bool endFlag = false;

        while (pattern.Length > 0)
        {
            if (pattern[0] == '^')
            {
                startFlag = true;
                pattern = pattern.Substring(1);
            }

            if (pattern[pattern.Length - 1] == '$')
            {
                endFlag = true;
                pattern = pattern.Substring(0, pattern.Length - 1);
            }

            string currentPattern;
            if (pattern[0] == '\\')
            {
                currentPattern = pattern.Substring(0, 2);
                pattern = pattern.Substring(2);
            }
            else if (pattern[0] == '[')
            {
                int closingIndex = pattern.IndexOf(']') + 1;
                if (closingIndex == 0)
                    throw new ArgumentException("Closing not found");
                currentPattern = pattern.Substring(0, closingIndex);
                pattern = pattern.Substring(closingIndex);
            }
            else
            {
                currentPattern = pattern.Substring(0, 1);
                pattern = pattern.Substring(1);
            }

            if (pattern.Length > 0 && pattern[0] == '.')
                pattern = "^" + currentPattern + pattern;

            if (pattern.Length > 0 && pattern[0] == '+')
            {
                pattern = pattern.Substring(1);
                int matchLen = 0;
                while (true)
                {
                    int inputStartPos = FindFirstMatchIndex(inputLine, currentPattern, startFlag, endFlag);
                    Console.WriteLine(inputStartPos);
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

    public static void Main(string[] args)
    {
        if (args.Length < 3 || args[0] != "-E")
        {
            Console.WriteLine("Expected first argument to be '-E'");
            Environment.Exit(1);
        }

        string pattern = args[2];
        string inputLine = Console.ReadLine();

        if (MatchPatternSequence(inputLine, pattern))
            Environment.Exit(0);
        else
            Environment.Exit(1);
    }
}
