using System;
using System.IO;
using System.Text;

// This function checks if a given input line matches a given pattern.
static bool MatchPattern(string inputLine, string pattern)
{
    // Get the lengths of the input line and the pattern.
    int inputLength = inputLine.Length;
    int patternLength = pattern.Length;

    // Loop through the input line to find potential matches with the pattern.
    for (int i = 0; i <= inputLength - patternLength; i++)
    {
        // Check if there is a match starting from the current index i.
        if (MatchFromIndex(inputLine, pattern, i))
        {
            return true; // If there's a match, return true.
        }
    }

    return false; // If no match is found, return false.
}

// This function checks for a pattern match starting from a specific index in the input line.
static bool MatchFromIndex(string inputLine, string pattern, int index)
{
    int inputIndex = index;
    int patternIndex = 0;

    while (patternIndex < pattern.Length && inputIndex < inputLine.Length)
    {
        char patternChar = pattern[patternIndex];

        if (patternChar == '\\')
        {
            patternIndex++;
            if (patternIndex >= pattern.Length)
            {
                return false;
            }

            char escChar = pattern[patternIndex];
            switch (escChar)
            {
                case 'd':
                if (!char.IsDigit(inputLine[inputIndex]))
                {
                    return false;
                }
                break;
                case 'w':
                if (!char.IsLetterOrDigit(inputLine[inputIndex]))
                {
                    return false;
                }
                break;
                default:
                return false;
            }
        }
        else if (patternChar == '[')
        {
            patternIndex++;
            bool negate = pattern[patternIndex] == '^';
            if (negate)
            {
                patternIndex++;
            }

            int closingBracketIndex = pattern.IndexOf(']', patternIndex);
            if (closingBracketIndex == -1)
            {
                return false;
            }

            string characterClass = pattern.Substring(patternIndex, closingBracketIndex - patternIndex);
            patternIndex = closingBracketIndex;

            bool matchFound = characterClass.Contains(inputLine[inputIndex]);
            if (negate)
            {
                matchFound = !matchFound;
            }

            if (!matchFound)
            {
                return false;
            }
        }
        else if (char.IsDigit(patternChar))
        {
            // Check if the pattern specifies a digit count followed by a space.
            int count = patternChar - '0'; // Convert the character representing the digit count to an integer.
                                           // Check if there are enough characters in the input line to match the specified count.
            if (inputIndex + count <= inputLine.Length)
            {
                // Get the substring of the input line corresponding to the digit count.
                string digitsSubstring = inputLine.Substring(inputIndex, count);
                // Check if the substring consists of only digits.
                if (digitsSubstring.All(char.IsDigit))
                {
                    inputIndex += count; // Move the input index forward by the digit count.
                    patternIndex++; // Move the pattern index forward.
                    continue; // Continue to the next iteration of the loop.
                }
            }
            // If the specified digit count does not match, return false.
            return false;
        }


        patternIndex++;
        inputIndex++;
    }

    return patternIndex == pattern.Length;
}

// Check if the program is invoked with the correct arguments.
if (args.Length < 2 || args[0] != "-E")
{
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2); // Exit with status 2 indicating incorrect usage.
}

// Extract the pattern and input line from command-line arguments and standard input.
string pattern = args[1];
string inputLine = Console.In.ReadToEnd().Trim();

Console.WriteLine("Logs from your program will appear here!");

// Check if the input line matches the pattern.
if (MatchPattern(inputLine, pattern))
{
    Environment.Exit(0); // Exit with status 0 indicating successful match.
}
else
{
    Environment.Exit(1); // Exit with status 1 indicating no match.
}