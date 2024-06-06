using System;

class Program
{
    static bool MatchPattern(string inputLine, string pattern)
    {
        return MatchHere(inputLine, pattern, inputLine);
    }

    static bool MatchHere(string remainingInput, string pattern, string inputLine)
    {
        if (pattern == "")
            return true;

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
            string charactersInNegativeCharacterGroup = pattern.Substring(2, pattern.IndexOf(']') - 2);
            if (!charactersInNegativeCharacterGroup.Contains(remainingInput[0]))
                return MatchHere(remainingInput.Substring(1), pattern.Substring(pattern.IndexOf(']') + 1), inputLine);
            else
                return false;
        }
        else if (pattern.StartsWith("["))
        {
            string charactersInPositiveCharacterGroup = pattern.Substring(1, pattern.IndexOf(']') - 1);
            if (charactersInPositiveCharacterGroup.Contains(remainingInput[0]))
                return MatchHere(remainingInput.Substring(1), pattern.Substring(pattern.IndexOf(']') + 1), inputLine);
            else
                return false;
        }
        else
        {
            if (remainingInput[0] == pattern[0])
                return MatchHere(remainingInput.Substring(1), pattern.Substring(1), inputLine);
            else
                return false;
        }
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

        if (MatchPattern(inputLine, pattern))
            Environment.Exit(0);
        else
            Environment.Exit(1);
    }
}
