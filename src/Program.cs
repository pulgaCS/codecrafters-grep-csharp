using System;
using System.IO;

static bool MatchPattern(string inputLine, string pattern) {
    int i = 0, j = 0;
    while (i < inputLine.Length && j < pattern.Length) {
        if (pattern[j] == '\\') {
            j++;
            if (pattern[j] == 'd' && !char.IsDigit(inputLine[i])) {
                return false;
            }
            if (pattern[j] == 'w' && !char.IsLetterOrDigit(inputLine[i])) {
                return false;
            }
        }
        else if (pattern[j] == '[') {
            bool match = false;
            j++;
            bool negate = pattern[j] == '^';
            if (negate) {
                j++;
            }
            while (pattern[j] != ']') {
                if (negate) {
                    if (!inputLine[i].Equals(pattern[j])) {
                        match = true;
                        break;
                    }
                }
                else {
                    if (inputLine[i].Equals(pattern[j])) {
                        match = true;
                        break;
                    }
                }
                j++;
            }
            if (!match) {
                return false;
            }
            while (pattern[j] != ']') {
                j++;
            }
        }
        else if (!inputLine[i].Equals(pattern[j])) {
            return false;
        }
        i++;
        j++;
    }
    return i == inputLine.Length && j == pattern.Length;
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