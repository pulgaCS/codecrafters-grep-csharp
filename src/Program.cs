using System;
using System.Collections.Generic;

public class Token
{
    public TokenType Type { get; set; }
    public string? Value { get; set; } // Make Value nullable

    public Token(TokenType type, string? value)
    {
        Type = type;
        Value = value;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Sample usage of the DFA class
        var dfa = new DFA();
        var tokens = new List<Token>
        {
            new Token(TokenType.LITERAL, "abc"),
            new Token(TokenType.LITERAL, "def"),
            new Token(TokenType.END_ANCHOR, null)
        };
        dfa.BuildDFA(tokens);
        Console.WriteLine(dfa.Match("abcdef")); // Should print True
        Console.WriteLine(dfa.Match("xyz"));    // Should print False
    }
}

public class DFA
{
    private bool hasStartAnchor;
    private bool hasEndAnchor;
    private int startState;
    private List<Dictionary<char, int>> transitions;
    private HashSet<int> acceptStates;

    public DFA()
    {
        hasStartAnchor = false;
        hasEndAnchor = false;
        startState = 0;
        transitions = new List<Dictionary<char, int>>();
        acceptStates = new HashSet<int>();
    }

    public bool Match(string inputLine)
    {
        if (hasStartAnchor)
        {
            return MatchFromStart(inputLine);
        }
        else
        {
            return MatchFromAnyPosition(inputLine);
        }
    }

    private bool MatchFromStart(string inputLine)
    {
        int currentState = startState;
        int index = 0;
        while (index < inputLine.Length)
        {
            char ch = inputLine[index];
            Console.WriteLine($"Processing character '{ch}' at index {index} from state {currentState}");

            if (!transitions[currentState].TryGetValue(ch, out currentState))
            {
                break;
            }

            if (index == inputLine.Length - 1 && acceptStates.Contains(currentState))
            {
                return true;
            }

            index++;
        }
        return false;
    }

    private bool MatchFromAnyPosition(string inputLine)
    {
        for (int startIndex = 0; startIndex < inputLine.Length; startIndex++)
        {
            int currentState = startState;
            for (int index = startIndex; index < inputLine.Length; index++)
            {
                char ch = inputLine[index];
                Console.WriteLine($"Processing character '{ch}' at index {index} from state {currentState}");

                if (!transitions[currentState].TryGetValue(ch, out currentState))
                {
                    break;
                }
            }
            if (acceptStates.Contains(currentState) && (!hasEndAnchor || startIndex + currentState == inputLine.Length))
            {
                return true;
            }
        }
        return false;
    }

    public void BuildDFA(List<Token> tokens)
    {
        int state = 0;

        transitions.Clear();

        Console.WriteLine("Building DFA from tokens...");
        for (int i = 0; i < tokens.Count; i++)
        {
            Token token = tokens[i];
            int nextState = state + 1;
            Console.WriteLine($"Previous state: {state} Next state: {nextState}");

            if (token.Type == TokenType.DIGIT)
            {
                Console.WriteLine("Adding transitions for digits");
                AddRangeTransition(state, nextState, '0', '9');
                transitions.Add(new Dictionary<char, int>(transitions[state]));
            }
            else if (token.Type == TokenType.ALPHANUM)
            {
                Console.WriteLine("Adding transitions for alphanumerics");
                AddRangeTransition(state, nextState, 'a', 'z');
                AddRangeTransition(state, nextState, 'A', 'Z');
                AddRangeTransition(state, nextState, '0', '9');
                transitions.Add(new Dictionary<char, int>(transitions[state]));
            }
            else if (token.Type == TokenType.CHARACTER_GROUP)
            {
                Console.WriteLine($"Adding transitions for character group: {token.Value}");
                foreach (char ch in token.Value)
                {
                    transitions[state][ch] = nextState;
                }
                transitions.Add(new Dictionary<char, int>(transitions[state]));
            }
            else if (token.Type == TokenType.NEGATIVE_CHARACTER_GROUP)
            {
                Console.WriteLine($"Adding transitions for negative character group: {token.Value}");
                for (char ch = (char)0; ch < 128; ch++)
                {
                    if (token.Value.IndexOf(ch) == -1)
                    {
                        transitions[state][ch] = nextState;
                    }
                }
                transitions.Add(new Dictionary<char, int>(transitions[state]));
            }
            else if (token.Type == TokenType.START_ANCHOR)
            {
                hasStartAnchor = true;
                Console.WriteLine("Adding start anchor");
                continue;
            }
            else if (token.Type == TokenType.END_ANCHOR)
            {
                hasEndAnchor = true;
                Console.WriteLine("Adding end anchor");
                acceptStates.Add(state);
                Console.WriteLine($"DFA constructed. Accepting state: {state}");
                return;
            }
            else if (token.Type == TokenType.ONE_OR_MORE)
            {
                Console.WriteLine("Adding one or more quantifier");
                foreach (KeyValuePair<char, int> entry in transitions[state - 1])
                {
                    Console.WriteLine($"Adding transition for '{entry.Key}'");
                    transitions[state][entry.Key] = state;
                }
                Console.WriteLine($"Adding transition for literal one or more '{tokens[i - 1].Value[0]}'");
                transitions[state][tokens[i - 1].Value[0]] = nextState;
                continue;
            }
            else
            {
                transitions[state][token.Value[0]] = nextState;
                Console.WriteLine($"Adding transition for literal '{token.Value[0]}'");
                for (char ch = (char)0; ch < 128; ch++)
                {
                    transitions[nextState][ch] = nextState;
                }
            }
            state = nextState;
        }
        if (!hasEndAnchor)
        {
            acceptStates.Add(state);
        }
        Console.WriteLine($"DFA constructed. Accepting state: {state}");
    }

    private void AddRangeTransition(int fromState, int toState, char startChar, char endChar)
    {
        for (char ch = startChar; ch <= endChar; ch++)
        {
            transitions[fromState][ch] = toState;
        }
    }
}
