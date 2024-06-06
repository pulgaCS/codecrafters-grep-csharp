using System;

class Program
{
    // Helper function to find the index of the first match of the given pattern in the input line
    static int FindFirstMatchIndex(string inputLine, string pattern, bool startFlag = false, bool endFlag = false)
    {
        // Check for end of line
        if (pattern == "$" && inputLine == "")
            return 0;

        // Check for patterns like \d and \w
        if (pattern == "\\d" || pattern == "\\w")
        {
            for (int idx = 0; idx < inputLine.Length; idx++)
            {
                if ((pattern == "\\d" && Char.IsDigit(inputLine[idx])) ||
                    (pattern == "\\w" && (Char.IsLetterOrDigit(inputLine[idx]))))
                {
                    if (!startFlag || (startFlag && idx == 0))
                        return idx + 1;
                }
            }
        }
        // Check for patterns like [abc] or [^abc]
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
        // Check for regular characters
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

    // Recursive function to check if inputLine matches pattern
    static bool MatchPatternSequence(string inputLine, string pattern)
    {
        bool startFlag = false;
        bool endFlag = false;

        while (!string.IsNullOrEmpty(pattern))
        {
            // Check for start flag
            if (pattern[0] == '^')
            {
                startFlag = true;
                pattern = pattern.Substring(1);
            }

            // Check for end flag
            if (pattern[pattern.Length - 1] == '$')
            {
                endFlag = true;
                pattern = pattern.Substring(0, pattern.Length - 1);
            }

            // Get the current pattern to match
            string currentPattern;
            if (pattern.StartsWith("\\"))
            {
                currentPattern = pattern.Substring(0, 2);
                pattern = pattern.Substring(2);
            }
            else if (pattern.StartsWith("["))
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

            // Handling special characters like "."
            if (!string.IsNullOrEmpty(pattern) && pattern[0] == '.')
            {
                pattern = "^" + currentPattern + pattern;
            }

            // Handling repetition with "+"
            if (!string.IsNullOrEmpty(pattern) && pattern[0] == '+')
            {
                pattern = pattern.Substring(1);
                int matchLen = 0;
                while (true)
                {
                    int inputStartPos = FindFirstMatchIndex(inputLine, currentPattern, startFlag, endFlag);
                    Console.WriteLine(inputStartPos); // For debugging
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

        // Return true once the whole pattern is checked without returning false
        return true;
    }

    static void Main(string[] args)
    {
        // Check if the program is invoked with the correct arguments
        if (args.Length < 2 || args[0] != "-E")
        {
            Console.WriteLine("Expected first argument to be '-E'");
            Environment.Exit(2); // Exit with status 2 indicating incorrect usage
        }

        string pattern = args[1];
        string inputLine = Console.In.ReadToEnd().Trim();

        Console.WriteLine("Logs from your program will appear here!");

        if (MatchPatternSequence(inputLine, pattern))
            Environment.Exit(0); // Exit with status 0 indicating successful match
        else
            Environment.Exit(1); // Exit with status 1 indicating no match
    }
}
