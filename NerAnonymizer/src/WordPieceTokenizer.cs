using System.Collections.Frozen;

namespace NerAnonymizer;

/// <summary>
/// Raw token, this is the output from the model
/// </summary>
public record struct Token(long Id, int Start, int End);


/// <summary>
/// Internal raw prediction with only score and index
/// </summary>
public record Prediction(string EntityGroup, double Score, int Start, int End);


/// <summary>
/// Word piece model for tokenizing using existing vocabularies
/// </summary>
public class WordPieceTokenizer
{
    private readonly FrozenDictionary<string, int> _tokensToIds;

    private readonly string[] _idsToTokens;


    /// <summary>
    /// Create a new WordPiece model with a vocabulary
    /// </summary>
    /// <param name="vocabulary"></param>
    public WordPieceTokenizer(string vocabulary)
    {
        _idsToTokens = vocabulary.Split(["\n", "\r\n"], StringSplitOptions.None);
        _tokensToIds = _idsToTokens.Select((o, i) => new KeyValuePair<string, int>(o, i)).ToFrozenDictionary();
    }


    public IReadOnlyDictionary<string, int> GetVocab() => _tokensToIds;

    public int GetVocabSize() => _idsToTokens.Length;

    public string? IdToToken(int id) => _idsToTokens[id];

    public int? TokenToId(string token) => _tokensToIds[token];


    /// <summary>
    /// Tokenize text into word pieces
    /// </summary>
    public IReadOnlyList<Token> Tokenize(ReadOnlySpan<char> text) => new PreTokenizer().Split(text).SelectMany(TokenizeWord).ToList();


    /// <summary>
    /// Split word into.. eh word pieces
    /// </summary>
    public IEnumerable<Token> TokenizeWord(Word word)
    {
        if (word.Start == word.End)
        {
            throw new ArgumentException("uh, should probably not feed this with empty words?");
        }

        // perfect match
        if (_tokensToIds.TryGetValue(word.Text, out var id))
        {
            return [new Token(id, word.Start, word.End)];
        }

        var tokens = new List<Token>();
        var currentWord = word.Text;
        var start = word.Start;


        // this is probably more efficient the other way around... but lets bang my head into the wall trying to solve already solved questions
        for (int i = currentWord.Length; i > 0; i--)
        {
            if (_tokensToIds.TryGetValue(currentWord[..i], out var pieceId))
            {
                tokens.Add(new Token(pieceId, start, start + i));
                currentWord = currentWord[i..];
                start += i;
                break;
            }
        }

        if (tokens.Count == 0)
        {
            return [new Token(_tokensToIds["[UNK]"], word.Start, word.End)];
        }

        while (currentWord.Length > 0)
        {
            var unk = true;
            for (int i = currentWord.Length; i > 0; i--)
            {
                if (_tokensToIds.TryGetValue("##" + currentWord[..i], out var pieceId))
                {
                    unk = false;
                    tokens.Add(new Token(pieceId, start, start + i));
                    currentWord = currentWord[i..];
                    start += i;
                    break;
                }
            }

            if (unk)
            {
                return [new Token(_tokensToIds["[UNK]"], word.Start, word.End)];
            }
        }

        if (tokens.Count == 0)
        {
            return [new Token(_tokensToIds["[UNK]"], word.Start, word.End)];
        }

        return tokens;
    }
}