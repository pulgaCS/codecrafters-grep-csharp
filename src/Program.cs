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
    // Initialize variables to keep track of the current index in the input line and the pattern.
    int inputIndex = index;
    int patternIndex = 0;

    // Loop through the pattern and input line to check for matches.
    while (patternIndex < pattern.Length && inputIndex < inputLine.Length)
    {
        // Get the current character in the pattern.
        char patternChar = pattern[patternIndex];

        // Check for escape characters.
        if (patternChar == '\\')
        {
            patternIndex++; // Move to the next character after the escape character.
            if (patternIndex >= pattern.Length)
            {
                return false; // If there's no character after the escape character, return false.
            }

            // Get the escaped character.
            char escChar = pattern[patternIndex];
            switch (escChar)
            {
                case 'd':
                // Check if the current character in the input line is a digit.
                if (!char.IsDigit(inputLine[inputIndex]))
                {
                    return false; // If it's not a digit, return false.
                }
                break;
                case 'w':
                // Check if the current character in the input line is a letter or digit.
                if (!char.IsLetterOrDigit(inputLine[inputIndex]))
                {
                    return false; // If it's neither a letter nor a digit, return false.
                }
                break;
                default:
                return false; // Return false for unknown escape characters.
            }
        }
        else if (patternChar == '[')
        {
            patternIndex++; // Move to the next character after '['.
            // Check if the character class is negated.
            bool negate = pattern[patternIndex] == '^';
            if (negate)
            {
                patternIndex++; // Move to the next character after '^'.
            }

            // Find the closing bracket ']' for the character class.
            int closingBracketIndex = pattern.IndexOf(']', patternIndex);
            if (closingBracketIndex == -1)
            {
                return false; // If ']' is not found, return false.
            }

            // Extract the character class between '[' and ']'.
            string characterClass = pattern.Substring(patternIndex, closingBracketIndex - patternIndex);
            patternIndex = closingBracketIndex; // Move pattern index to ']' position.

            // Check if the current character in the input line matches the character class.
            bool matchFound = characterClass.Contains(inputLine[inputIndex]);
            if (negate)
            {
                matchFound = !matchFound; // If negated, invert the match result.
            }

            if (!matchFound)
            {
                return false; // If no match is found, return false.
            }
        }
        else
        {
            // Check if the current character in the input line matches the pattern character.
            if (inputLine[inputIndex] != patternChar)
            {
                return false; // If there's no match, return false.
            }
        }

        // Move to the next character in both the pattern and the input line.
        patternIndex++;
        inputIndex++;
    }

    // Return true if the pattern has been fully matched.
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