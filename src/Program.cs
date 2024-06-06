using System;
using System.IO;

static bool IsDigit(char c) {
    return c >= '0' && c <= '9';
}

static bool MatchPattern(string inputLine, string pattern) {
    string[] patterns = pattern.Split(' ');

    foreach (string p in patterns) {
        if (p.Length == 1) {
            if (!inputLine.Contains(p)) {
                return false;
            }
        }
        else if (p == @"\d") {
            bool foundDigit = false;
            foreach (char c in inputLine) {
                if (IsDigit(c)) {
                    foundDigit = true;
                    break;
                }
            }
            if (!foundDigit) {
                return false;
            }
        }
        else if (p == @"\w") {
            bool foundAlphanumeric = false;
            foreach (char c in inputLine) {
                if (char.IsLetterOrDigit(c)) {
                    foundAlphanumeric = true;
                    break;
                }
            }
            if (!foundAlphanumeric) {
                return false;
            }
        }
        else if (p.Length > 2 && p[0] == '[' && p[p.Length - 1] == ']') {
            bool foundChar = false;
            if (p[1] == '^') {
                foreach (char c in inputLine) {
                    if (!p.Substring(2, p.Length - 3).Contains(c)) {
                        foundChar = true;
                        break;
                    }
                }
            }
            else {
                foreach (char c in inputLine) {
                    if (p.Substring(1, p.Length - 2).Contains(c)) {
                        foundChar = true;
                        break;
                    }
                }
            }
            if (!foundChar) {
                return false;
            }
        }
        else {
            if (!inputLine.Contains(p)) {
                return false;
            }
        }
    }

    return true;
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