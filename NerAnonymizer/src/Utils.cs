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

    /// <summary>
    /// Group overlapping predictions and return best*.
    /// * best here is quite undefined
    /// </summary>    
    public static IEnumerable<PredictionResult> TrimOverlapping(IEnumerable<PredictionResult> predictions)
    {
        using var predictionsEnumerator = predictions.GetEnumerator();

        if (!predictionsEnumerator.MoveNext()) yield break;

        var bestPrediction = predictionsEnumerator.Current;
        var predictionEndIndex = predictionsEnumerator.Current.End;

        while (predictionsEnumerator.MoveNext())
        {
            var match = predictionsEnumerator.Current;

            if (predictionsEnumerator.Current.Start > predictionEndIndex)
            {
                yield return bestPrediction;

                bestPrediction = predictionsEnumerator.Current;
            }

            if (match.Score > bestPrediction.Score)
            {
                bestPrediction = match;
            }

            predictionEndIndex = predictionsEnumerator.Current.Start;
        }

        yield return bestPrediction;
    }
}