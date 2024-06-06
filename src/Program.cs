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
string inputLine = Console.In.ReadToEnd();

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");

if (MatchPattern(inputLine, pattern)) {
    Environment.Exit(0);
}
else {
    Environment.Exit(1);
}