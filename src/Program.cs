using System;

class Program
{
    // Helper function to recursively match the pattern
    static bool MatchHere(string remainingInput, string pattern, string inputLine)
    {
        // Base case: empty pattern matches any input
        if (pattern == "")
            return true;

        // Special case: pattern is "$", meaning it should match the end of input
        if (pattern == "$")
            return remainingInput == "";

        // Base case: if there's no input remaining, the match failed
        if (remainingInput == "")
            return false;

        // Handling escape sequences like \d and \w
        if (pattern.StartsWith("\\d"))
        {
            if (Char.IsDigit(remainingInput[0]))
                return MatchHere(remainingInput.Substring(1), pattern.Substring(2), inputLine);
            else
                return MatchHere(remainingInput.Substring(1), pattern, inputLine);
        }
        else if (pattern.StartsWith("\\w"))
        {
            if (Char.IsLetterOrDigit(remainingInput[0]))
                return MatchHere(remainingInput.Substring(1), pattern.Substring(2), inputLine);
            else
                return MatchHere(remainingInput.Substring(1), pattern, inputLine);
        }
        // Handling negative character groups like [^...]
        else if (pattern.StartsWith("[^"))
        {
            string charactersInNegativeCharacterGroup = pattern.Split(']')[0].Substring(2);
            return !charactersInNegativeCharacterGroup.Contains(remainingInput[0]) && MatchHere(remainingInput.Substring(1), pattern.Substring(pattern.IndexOf(']') + 1), inputLine);
        }
        // Handling positive character groups like [...]
        else if (pattern.StartsWith("["))
        {
            string charactersInPositiveCharacterGroup = pattern.Split(']')[0].Substring(1);
            return charactersInPositiveCharacterGroup.Contains(remainingInput[0]) && MatchHere(remainingInput.Substring(1), pattern.Substring(pattern.IndexOf(']') + 1), inputLine);
        }
        // Handling single characters and characters not requiring special handling
        else if (pattern.Length == 1)
        {
            return pattern[0] == remainingInput[0] && MatchHere(remainingInput.Substring(1), pattern.Substring(1), inputLine);
        }
        // Handling any other case
        else
        {
            return pattern[0] == remainingInput[0] && MatchHere(remainingInput.Substring(1), pattern.Substring(1), inputLine);
        }
    }

    // Main function to check if the pattern matches the input line
    static bool MatchPattern(string inputLine, string pattern)
    {
        // If the pattern starts with "^", it must match from the beginning of the input
        if (pattern[0] == '^')
        {
            return MatchHere(inputLine, pattern.Substring(1), inputLine);
        }

        // If the pattern doesn't start with "^", it can match anywhere in the input
        // Base case: if there's no input, the match fails
        if (inputLine == "")
            return false;

        // Otherwise, try to match the pattern
        return MatchHere(inputLine, pattern, inputLine);
    }

    static void Main(string[] args)
    {
        // Check if the program is invoked with the correct arguments
        if (args.Length < 2 || args[0] != "-E")
        {
            Console.WriteLine("Expected first argument to be '-E'");
            Environment.Exit(2); // Exit with status 2 indicating incorrect usage
        }

        // Extract the pattern and input line from command-line arguments and standard input
        string pattern = args[1];
        string inputLine = Console.In.ReadToEnd().Trim();

        // Output log message
        Console.WriteLine("Logs from your program will appear here!");

        // Check if the input line matches the pattern
        if (MatchPattern(inputLine, pattern))
            Environment.Exit(0); // Exit with status 0 indicating successful match
        else
            Environment.Exit(1); // Exit with status 1 indicating no match
    }
}
