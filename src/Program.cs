using System;
using System.IO;

static bool MatchPattern(string inputLine, string pattern) {
    return MatchPatternRecursive(inputLine, 0, pattern, 0);
}

static bool MatchPatternRecursive(string inputLine, int inputIndex, string pattern, int patternIndex) {
    // If we have reached the end of the pattern, check if we have also reached the end of the input
    if (patternIndex == pattern.Length) {
        return inputIndex == inputLine.Length;
    }

    // If the next pattern character is an escaped character class
    if (pattern[patternIndex] == '\\' && patternIndex + 1 < pattern.Length) {
        char patternClass = pattern[patternIndex + 1];
        if (inputIndex < inputLine.Length && MatchCharacterClass(inputLine[inputIndex], patternClass)) {
            return MatchPatternRecursive(inputLine, inputIndex + 1, pattern, patternIndex + 2);
        }
        return false;
    }

    // Otherwise, check for direct character match
    if (inputIndex < inputLine.Length && inputLine[inputIndex] == pattern[patternIndex]) {
        return MatchPatternRecursive(inputLine, inputIndex + 1, pattern, patternIndex + 1);
    }

    // If current characters do not match and we are not at the end of pattern/input, return false
    return false;
}

static bool MatchCharacterClass(char c, char patternClass) {
    return patternClass switch {
        'd' => char.IsDigit(c),
        'w' => char.IsLetterOrDigit(c),
        _ => throw new ArgumentException($"Unknown character class: \\{patternClass}")
    };
}

if (args.Length < 2 || args[0] != "-E") {
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}

string pattern = args[1];
string inputLine = Console.In.ReadToEnd().Trim(); // Trim to remove any extra newline from input

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");

if (MatchPattern(inputLine, pattern)) {
    Environment.Exit(0);
}
else {
    Environment.Exit(1);
}
