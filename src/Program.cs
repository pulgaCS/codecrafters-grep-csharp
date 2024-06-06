using System;
using System.IO;

static bool MatchPattern(string inputLine, string pattern) {
    // Check if the pattern contains whitespace
    if (pattern.Contains(" ")) {
        // Split the pattern into parts
        string[] patternParts = pattern.Split(' ');

        // Match the first part of the pattern
        if (!MatchPattern(inputLine, patternParts[0]))
            return false;

        // Match the rest of the pattern
        if (inputLine.Length >= patternParts[0].Length) {
            return MatchPattern(inputLine.Substring(patternParts[0].Length), string.Join(" ", patternParts.Skip(1)));
        }
        else {
            return false;
        }
    }

    // Handle other patterns like \d, \w, [^abc], etc.
    if (pattern.Length == 1) {
        return inputLine.Contains(pattern);
    }
    else if (pattern == @"\d") {
        foreach (char c in inputLine) {
            if (char.IsDigit(c)) {
                return true;
            }
        }
        return false;
    }
    else if (pattern == @"\w") {
        foreach (char c in inputLine) {
            if (char.IsLetterOrDigit(c)) {
                return true;
            }
        }
        return false;
    }
    else if (pattern.Length > 2 && pattern[0] == '[' && pattern[pattern.Length - 1] == ']') {
        // Handle character classes like [^abc]
        if (pattern[1] == '^') {
            foreach (char c in inputLine) {
                if (!pattern.Substring(2, pattern.Length - 3).Contains(c)) {
                    return true;
                }
            }
            return false;
        }
        else {
            foreach (char c in inputLine) {
                if (pattern.Substring(1, pattern.Length - 2).Contains(c)) {
                    return true;
                }
            }
            return false;
        }
    }
    else {
        throw new ArgumentException($"Unhandled pattern: {pattern}");
    }
}

if (args.Length < 2 || args[0] != "-E") {
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}

string pattern = args[1];
bool matchFound = false;
int character;

while ((character = Console.In.Read()) != -1) {
    if (character == '\n') {
        // End of line, break the loop
        break;
    }
    char c = (char)character;
    if (!matchFound && MatchPattern(c.ToString(), pattern)) {
        matchFound = true;
        break;
    }
}

if (matchFound) {
    Environment.Exit(0);
}
else {
    Environment.Exit(1);
}