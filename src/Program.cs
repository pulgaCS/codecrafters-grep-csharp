using System;

class Program
{
    static bool MatchHere(string remainingInput, string pattern, string inputLine)
    {
        // Base case: empty pattern matches any input
        if (pattern == "")
            return true;

        if (pattern == "$")
            return remainingInput == "";

        // Base case: if there's no input remaining, the match failed
        if (remainingInput == "")
            return false;

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
        else if (pattern.StartsWith("[^"))
        {
            string charactersInNegativeCharacterGroup = pattern.Split(']')[0].Substring(2);
            return !charactersInNegativeCharacterGroup.Contains(remainingInput[0]) && MatchHere(remainingInput.Substring(1), pattern.Substring(pattern.IndexOf(']') + 1), inputLine);
        }
        else if (pattern.StartsWith("["))
        {
            string charactersInPositiveCharacterGroup = pattern.Split(']')[0].Substring(1);
            return charactersInPositiveCharacterGroup.Contains(remainingInput[0]) && MatchHere(remainingInput.Substring(1), pattern.Substring(pattern.IndexOf(']') + 1), inputLine);
        }
        else if (pattern.Length == 1)
        {
            return pattern[0] == remainingInput[0] && MatchHere(remainingInput.Substring(1), pattern.Substring(1), inputLine);
        }
        else
        {
            return pattern[0] == remainingInput[0] && MatchHere(remainingInput.Substring(1), pattern.Substring(1), inputLine);
        }
    }

    static bool MatchPattern(string inputLine, string pattern)
    {
        if (pattern[0] == '^')
        {
            return MatchHere(inputLine, pattern.Substring(1), inputLine);
        }

        if (inputLine == "")
            return false;

        return MatchHere(inputLine, pattern, inputLine);
    }

    static void Main(string[] args)
    {
        if (args.Length < 2 || args[0] != "-E")
        {
            Console.WriteLine("Expected first argument to be '-E'");
            Environment.Exit(2); // Exit with status 2 indicating incorrect usage
        }

        string pattern = args[1];
        string inputLine = Console.In.ReadToEnd().Trim();

        Console.WriteLine("Logs from your program will appear here!");

        if (MatchPattern(inputLine, pattern))
            Environment.Exit(0); // Exit with status 0 indicating successful match
        else
            Environment.Exit(1); // Exit with status 1 indicating no match
    }
}
