using System.Collections.Immutable;
using System.Text.Json;
using NerAnonymizer;
using vforteli.WordPieceTokenizer;

namespace NerAnonymizerTests;

public class UtilsTests
{
    [Test]
    public void TrimOverlapping()
    {
        // [         [         [         [
        // 0123456789012345678901234567890123456789
        // Onko tämä enteellistä Matti Oy? Mene ja tiedä
        //                       [PER]
        //                       [ORG   ]
        var predictions = new List<PredictionResult>
        {
            new() { Start = 22, End = 26, Score = 0.9f, EntityGroup = "PERSON", Word = "" },
            new() { Start = 22, End = 29, Score = 1f, EntityGroup = "ORG", Word = "" },
        };

        var actual = Utils.TrimOverlapping(predictions).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(actual, Has.Count.EqualTo(1));
            Assert.That(actual[0].EntityGroup, Is.EqualTo("ORG"));
        });
    }
    
    [Test]
    public void TrimOverlappingReverse()
    {
        // [         [         [         [
        // 0123456789012345678901234567890123456789
        // Onko tämä enteellistä Oy Matti? Mene ja tiedä
        //                       [ORG   ]
        //                          [PER]
        var predictions = new List<PredictionResult>
        {
            new() { Start = 22, End = 29, Score = 1f, EntityGroup = "ORG", Word = "" },
            new() { Start = 25, End = 29, Score = 0.9f, EntityGroup = "PERSON", Word = "" },
        };

        var actual = Utils.TrimOverlapping(predictions).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(actual, Has.Count.EqualTo(1));
            Assert.That(actual[0].EntityGroup, Is.EqualTo("ORG"));
        });
    }
    
    [Test]
    public void TrimOverlappingShorterIsBetter()
    {
        // [         [         [         [
        // 0123456789012345678901234567890123456789
        // Onko tämä enteellistä Matti Oy? Mene ja tiedä
        //                       [PER]
        //                       [ORG   ]
        var predictions = new List<PredictionResult>
        {
            new() { Start = 22, End = 26, Score = 1f, EntityGroup = "PERSON", Word = "" },
            new() { Start = 22, End = 29, Score = 0.9f, EntityGroup = "ORG", Word = "" },
        };

        var actual = Utils.TrimOverlapping(predictions).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(actual, Has.Count.EqualTo(1));
            Assert.That(actual[0].EntityGroup, Is.EqualTo("PERSON"));
        });
    }
    
    [Test]
    public void TrimOverlappingShorterIsBetterReverse()
    {
        // [         [         [         [
        // 0123456789012345678901234567890123456789
        // Onko tämä enteellistä Oy Matti? Mene ja tiedä
        //                       [ORG   ]
        //                          [PER]
        var predictions = new List<PredictionResult>
        {
            new() { Start = 22, End = 29, Score = 0.9f, EntityGroup = "ORG", Word = "" },
            new() { Start = 25, End = 29, Score = 1f, EntityGroup = "PERSON", Word = "" },
        };

        var actual = Utils.TrimOverlapping(predictions).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(actual, Has.Count.EqualTo(1));
            Assert.That(actual[0].EntityGroup, Is.EqualTo("PERSON"));
        });
    }
}