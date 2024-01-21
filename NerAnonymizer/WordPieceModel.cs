using Microsoft.ML.Tokenizers;

public class WordPieceModel : Model
{
    private Dictionary<string, int> TokensToIds = new Dictionary<string, int>
    {
        { "Helsingistä", 11671},
        { "tuli", 1059},
        { "Suomen", 663},
        { "Helsinki", 2824},
        { "[CLS]", 102 },
        { "[SEP]", 103 },
    };

    private Dictionary<int, string> IdsToTokens = new Dictionary<int, string>
    {
        { 11671, "Helsingistä" },
        { 1059, "tuli" },
        { 663, "Suomen" },
        { 2824, "Helsinki" },
        { 102, "[CLS]" },
        { 103, "[SEP]" },
    };


    public override Trainer? GetTrainer()
    {
        throw new NotImplementedException();
    }

    public override IReadOnlyDictionary<string, int> GetVocab()
    {
        throw new NotImplementedException();
    }

    public override int GetVocabSize()
    {
        throw new NotImplementedException();
    }

    public override string? IdToString(int id, bool skipSpecialTokens = false)
    {
        throw new NotImplementedException();
    }

    public override string? IdToToken(int id, bool skipSpecialTokens = false)
    {
        return IdsToTokens.GetValueOrDefault(id);
    }

    public override bool IsValidChar(char ch)
    {
        throw new NotImplementedException();
    }

    public override string[] Save(string path, string? prefix = null)
    {
        throw new NotImplementedException();
    }

    public override IReadOnlyList<Token> Tokenize(string sequence)
    {
        var foo = new List<Token>
        {
            new Token(102, "[CLS]", (0, 0)),
            new Token(2824, "Helsinki", (0, 8)),
            new Token(103, "[SEP]", (0, 0)),
            // new Token(1059, "tuli", (13, 17)),
            // new Token(663, "Suomen", (18, 23)),
        };

        return foo;
    }

    public override int? TokenToId(string token)
    {
        throw new NotImplementedException();
    }
}