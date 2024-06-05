using System;
using System.IO;

static bool MatchPattern(string inputLine, string pattern) {
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