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

            StringBuilder charClass = new StringBuilder();
            while (patternIndex < pattern.Length && pattern[patternIndex] != ']')
            {
                charClass.Append(pattern[patternIndex]);
                patternIndex++;
            }

            if (patternIndex >= pattern.Length || pattern[patternIndex] != ']')
            {
                return false;
            }

            bool matchFound = false;
            foreach (char c in charClass.ToString())
            {
                if (negate)
                {
                    if (c != inputLine[inputIndex])
                    {
                        matchFound = true;
                        break;
                    }
                }
                else
                {
                    if (c == inputLine[inputIndex])
                    {
                        matchFound = true;
                        break;
                    }
                }
            }

            if (!matchFound)
            {
                return false;
            }
        }
        else
        {
            if (inputLine[inputIndex] != patternChar)
            {
                return false;
            }
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