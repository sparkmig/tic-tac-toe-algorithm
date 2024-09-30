using System.Text.Json;

namespace Tickie_tickie_tow;

public class QLearn
{
    private Dictionary<string, List<State>> _states = new();
    private Random _random = new();
    private List<State> previous = new();
    private int _wins = 0;
    private int _losses = 0;
    private int _draws = 0;

    public char NextMove(Char[] board)
    {
        var b = new string(board);
        List<char> moves = board.Where(x => x != 'X' && x != 'O' && x != '0').ToList();
        if (!_states.ContainsKey(b))
        {
            AddBoardWithStates(b, moves);
        }

        var state = PickNext(_states[b]);
        previous.Add(state);
        return state.Move;
    }

    private State PickNext(List<State> states)
    {
        if (states.Any(x => x.Heroistic > 0))
            return states.OrderByDescending(x => x.Heroistic).First();

        return states[_random.Next(states.Count)];
    }

    private void AddBoardWithStates(string board, IEnumerable<char> moves)
    {
        List<State> states = new();
        foreach (var move in moves)
        {
            states.Add(new State(move));
        }

        _states[board] = states;
    }

    public void Lose()
    {
        _losses++;
        previous.Last().Heroistic -= 100;
        foreach (var state in previous)
        {
            state.Heroistic--;
        }

        previous.Clear();
    }

    public void Win()
    {
        _wins++;
        previous.Last().Heroistic += 10;
        foreach (var state in previous)
        {
            state.Heroistic++;
        }

        previous.Clear();
    }


    public void Draw()
    {
        _draws++;
        previous.Clear();
    }

    public void Stats()
    {
        Console.WriteLine("Losses: " + _losses);
        Console.WriteLine("Draws: " + _draws);
        Console.WriteLine("Wins: " + _wins);
    }
}

public class State(char move)
{
    public char Move { get; set; } = move;
    public int Heroistic { get; set; } = 0;
}