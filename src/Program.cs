using System;

class Program
{
    static bool MatchPattern(string inputLine, string pattern)
    {
        // If the pattern starts with "^"
        if (pattern.StartsWith("^"))
        {
            // Check if the input line starts with the remaining pattern
            return MatchHere(inputLine, pattern.Substring(1), inputLine);
        }
        else
        {
            // Call the helper function MatchHere to perform the matching
            return MatchHere(inputLine, pattern, inputLine);
        }
    }

    static bool MatchHere(string remainingInput, string pattern, string inputLine)
    {
        // Base case: empty pattern matches any input
        if (pattern == "")
            return true;

        // Base case: if there's no input remaining, the match failed
        if (remainingInput == "")
            return false;

        // Handling escape sequences like \d and \w
        if (pattern.StartsWith("\\d"))
        {
            // If the next character in the input is a digit
            if (Char.IsDigit(remainingInput[0]))
                // Continue matching with the rest of the pattern and input
                return MatchHere(remainingInput.Substring(1), pattern.Substring(2), inputLine);
            else
                // Otherwise, continue matching with the same input and pattern
                return MatchHere(remainingInput.Substring(1), pattern, inputLine);
        }
        else if (pattern.StartsWith("\\w"))
        {
            // If the next character in the input is a letter or digit
            if (Char.IsLetterOrDigit(remainingInput[0]))
                // Continue matching with the rest of the pattern and input
                return MatchHere(remainingInput.Substring(1), pattern.Substring(2), inputLine);
            else
                // Otherwise, continue matching with the same input and pattern
                return MatchHere(remainingInput.Substring(1), pattern, inputLine);
        }
        // Handling negative character groups like [^...]
        else if (pattern.StartsWith("[^"))
        {
            // Extracting characters inside the negative character group
            string charactersInNegativeCharacterGroup = pattern.Substring(2, pattern.IndexOf(']') - 2);
            // If the next character in the input is not in the negative character group
            if (!charactersInNegativeCharacterGroup.Contains(remainingInput[0]))
                // Continue matching with the rest of the pattern and input
                return MatchHere(remainingInput.Substring(1), pattern.Substring(pattern.IndexOf(']') + 1), inputLine);
            else
                // Otherwise, the match fails
                return false;
        }
        // Handling positive character groups like [...]
        else if (pattern.StartsWith("["))
        {
            // Extracting characters inside the positive character group
            string charactersInPositiveCharacterGroup = pattern.Substring(1, pattern.IndexOf(']') - 1);
            // If the next character in the input is in the positive character group
            if (charactersInPositiveCharacterGroup.Contains(remainingInput[0]))
                // Continue matching with the rest of the pattern and input
                return MatchHere(remainingInput.Substring(1), pattern.Substring(pattern.IndexOf(']') + 1), inputLine);
            else
                // Otherwise, the match fails
                return false;
        }
        // Handling single characters and characters not requiring special handling
        else
        {
            // If the next character in the input matches the pattern character
            if (remainingInput[0] == pattern[0])
                // Continue matching with the rest of the pattern and input
                return MatchHere(remainingInput.Substring(1), pattern.Substring(1), inputLine);
            else
                // Otherwise, the match fails
                return false;
        }
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

        Console.WriteLine("Logs from your program will appear here!");

        // Check if the input line matches the pattern
        if (MatchPattern(inputLine, pattern))
            Environment.Exit(0); // Exit with status 0 indicating successful match
        else
            Environment.Exit(1); // Exit with status 1 indicating no match
    }
}
