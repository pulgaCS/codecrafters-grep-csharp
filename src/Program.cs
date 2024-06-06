using System;
using System.IO;
using System.Text;

class PulgaGrep
{
    // Regular expression classes for digit and alphanumeric characters
    const string digitClass = "\\d";
    const string alphaNumericClass = "\\w";

    static void Main(string[] args)
    {
        // Check if the arguments are valid
        if (args.Length < 2 || args[0] != "-E")
        {
            Console.Error.WriteLine("usage: mygrep -E <pattern>");
            Environment.Exit(2); // 1 means no lines were selected, >1 means error
        }

        string pattern = args[1]; // Get the pattern from the arguments
        string input = Console.In.ReadToEnd(); // Read the input from standard input

        // Match the input against the pattern
        bool ok = Match(Encoding.UTF8.GetBytes(input), pattern);

        if (!ok)
        {
            Environment.Exit(1); // No match found, exit with code 1
        }

        // Default exit code is 0 which means success
    }

    // Main matching function
    static bool Match(byte[] line, string pattern)
    {
        // Handle special cases for the first character in the pattern
        if (pattern[0] == '^')
        {
            return MatchUtil(line, pattern.Substring(1)); // Match start of line
        }
        else if (pattern[0] == '\\')
        {
            if (pattern[1] == 'd')
            {
                return ContainsDigitAndMatch(line, pattern.Substring(2)); // Match digit class
            }
            else if (pattern[1] == 'w')
            {
                return ContainsAlphaNumericAndMatch(line, pattern.Substring(2)); // Match alphanumeric class
            }
            return false;
        }
        else if (pattern[0] == '[')
        {
            return MatchCharacterClass(line, pattern); // Match character class
        }
        return MatchUtil(line, pattern); // General case
    }

    // Utility function for general matching
    static bool MatchUtil(byte[] line, string pattern)
    {
        int lineIndx = 0;
        int i = 0;
        int lastPatternIndexStart = 0;

        for (i = 0; i < pattern.Length && lineIndx < line.Length; i++)
        {
            char r = pattern[i];
            int classStartIndex = i;

            // Handle special pattern characters
            if (r == '\\')
            {
                if (pattern[i + 1] == 'd')
                {
                    if (!IsDigit((char)line[lineIndx]))
                    {
                        return false; // Digit expected, but not found
                    }
                }
                else if (pattern[i + 1] == 'w')
                {
                    if (!IsAlphaNumeric((char)line[lineIndx]))
                    {
                        return false; // Alphanumeric expected, but not found
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
                    return false; // Character class mismatch
                }
                lineIndx++;
                i = classEndIndx;
            }
            else if (r == '$')
            {
                return line.Length == lineIndx; // Match end of line
            }
            else if (r == (char)line[lineIndx])
            {
                lineIndx++;
            }
            else if (r == '+')
            {
                if (i != 0 && MatchUtil(SubArray(line, lineIndx), pattern.Substring(lastPatternIndexStart)))
                {
                    return true; // Match one or more previous character
                }
            }
            else
            {
                return false; // Character mismatch
            }
            lastPatternIndexStart = classStartIndex;
        }
        return i == pattern.Length || (pattern[i] == '$'); // for loop breaking conditions simplifies the logic
    }

    // Function to match lines containing a digit
    static bool ContainsDigitAndMatch(byte[] line, string pattern)
    {
        while (true)
        {
            int indx = IndexOfDigit(line);
            if (indx == -1)
            {
                return false; // No digit found
            }
            if (MatchUtil(SubArray(line, indx + 1), pattern))
            {
                return true; // Match found after digit
            }
            line = SubArray(line, indx + 1);
        }
    }

    // Function to match lines containing an alphanumeric character
    static bool ContainsAlphaNumericAndMatch(byte[] line, string pattern)
    {
        while (true)
        {
            int indx = IndexOfAlphaNumeric(line);
            if (indx == -1)
            {
                return false; // No alphanumeric character found
            }
            if (MatchUtil(SubArray(line, indx + 1), pattern))
            {
                return true; // Match found after alphanumeric character
            }
            line = SubArray(line, indx + 1);
        }
    }

    // Function to match lines with a character class
    static bool MatchCharacterClass(byte[] line, string pattern)
    {
        int classEndIndx = pattern.IndexOf(']');
        string allChars = pattern.Substring(1, classEndIndx - 1);
        bool notCheck = allChars[0] == '^';

        if (notCheck)
        {
            allChars = allChars.Substring(1);
        }

        while (line.Length > 0)
        {
            bool charInClass = allChars.Contains((char)line[0]);
            if ((notCheck && !charInClass) || (!notCheck && charInClass))
            {
                if (MatchUtil(SubArray(line, 1), pattern.Substring(classEndIndx + 1)))
                {
                    return true; // Match found with character class
                }
            }
            else if (!notCheck)
            {
                return false; // Character class mismatch
            }
            line = SubArray(line, 1);
        }
        return false;
    }

    // Function to find the index of the first digit in the line
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

    // Function to find the index of the first alphanumeric character in the line
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

    // Check if a character is a digit
    static bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    // Check if a character is alphanumeric
    static bool IsAlphaNumeric(char c)
    {
        return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || IsDigit(c);
    }

    // Utility function to create a subarray from the given byte array starting at the specified index
    static byte[] SubArray(byte[] data, int index)
    {
        int length = data.Length - index;
        byte[] result = new byte[length];
        Array.Copy(data, index, result, 0, length);
        return result;
    }
}