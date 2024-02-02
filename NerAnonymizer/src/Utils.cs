using System.Text;

namespace NerAnonymizer;

public class Utils
{
    /// <summary>
    /// Anonymize text
    /// Categories.. PERSON
    /// </summary>
    public static string Anonymize(string input, IEnumerable<PredictionResult> groupedResults)
    {
        var sb = new StringBuilder(input);
        foreach (var entity in groupedResults.Where(o => o.EntityGroup == "PERSON").OrderByDescending(o => o.Start))
        {
            sb.Remove(entity.Start, entity.End - entity.Start).Insert(entity.Start, "*****");
        }

        return sb.ToString();
    }
}
