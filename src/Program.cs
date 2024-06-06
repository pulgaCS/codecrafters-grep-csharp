using System;
using System.IO;

static bool MatchPattern(string inputLine, string pattern) {
    string placeholder = Guid.NewGuid().ToString(); // Unique placeholder
    string replacedPattern = pattern.Replace(@"\\", placeholder);
    string[] patterns = replacedPattern.Split(' ');

    foreach (string p in patterns) {
        string currentPattern = p.Replace(placeholder, @"\"); // Replace placeholder back with '\\'

        if (currentPattern.Length == 1) {
            if (!inputLine.Contains(currentPattern)) {
                return false;
            }
        }
        else if (currentPattern.StartsWith(@"\d")) {
            int digitCount = 1;
            if (currentPattern.Length > 2 && char.IsDigit(currentPattern[2])) {
                digitCount = int.Parse(currentPattern.Substring(2));
            }

            int consecutiveDigits = 0;
            foreach (char c in inputLine) {
                if (char.IsDigit(c)) {
                    consecutiveDigits++;
                    if (consecutiveDigits == digitCount) {
                        break;
                    }
                }
                else {
                    consecutiveDigits = 0;
                }
            }

            if (consecutiveDigits != digitCount) {
                return false;
            }
        }
        else if (currentPattern.Length > 2 && currentPattern[0] == '[' && currentPattern[currentPattern.Length - 1] == ']') {
            bool foundChar = false;
            if (currentPattern[1] == '^') {
                foreach (char c in inputLine) {
                    if (!currentPattern.Substring(2, currentPattern.Length - 3).Contains(c)) {
                        foundChar = true;
                        break;
                    }
                }
            }
            else {
                foreach (char c in inputLine) {
                    if (currentPattern.Substring(1, currentPattern.Length - 2).Contains(c)) {
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
            if (!inputLine.Contains(currentPattern)) {
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