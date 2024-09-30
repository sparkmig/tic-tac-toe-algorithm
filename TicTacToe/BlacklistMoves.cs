using System.Text.Json;

namespace Tickie_tickie_tow;

public class BlacklistMoves
{
    private Dictionary<string, List<char>> _blacklist = new Dictionary<string, List<char>>();
    private Random _random = new();
    private (string, char)? _lastmove = null;

    public char NextMove(Char[] board)
    {
        var b = new string(board);
        List<char> moves = board.Where(x => x != 'X' && x != 'O' && x != '0').ToList();
        if (_blacklist.ContainsKey(b))
            moves = (moves.Where(x => !_blacklist[b]?.Contains(x) ?? true)).ToList();
        if(moves.Count == 0)
            return board.Where(x => x != 'X' && x != 'O' && x != '0').First();
        char move = moves[_random.Next(moves.Count - 1)];
        _lastmove = new ValueTuple<string, char>(b, move);
        return move;
    }

    public void Lose()
    {
        (string board, char move) = _lastmove.Value;
        if (!_blacklist.ContainsKey(board))
            _blacklist.Add(board, new List<char>());

        _blacklist[board].Add(move);

        Console.WriteLine(JsonSerializer.Serialize(_blacklist));
    }
}