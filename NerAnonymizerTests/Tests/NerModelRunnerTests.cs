using System.Collections.Immutable;
using System.Text.Json;
using NerAnonymizer;
using vforteli.WordPieceTokenizer;

namespace NerAnonymizerTests;

public class NerModelRunnerTests
{
    [Test]
    public void TestPredictionGrouping()
    {
        var predictionsJson =
            """
            [
            {
                "EntityGroup": "O",
                "Score": 0.999986184563546,
                "Start": 0,
                "End": 0
            },
            {
                "EntityGroup": "B-GPE",
                "Score": 0.9995203353993161,
                "Start": 0,
                "End": 11
            },
            {
                "EntityGroup": "O",
                "Score": 0.9999917239122099,
                "Start": 12,
                "End": 16
            },
            {
                "EntityGroup": "B-GPE",
                "Score": 0.9213893810450849,
                "Start": 17,
                "End": 23
            },
            {
                "EntityGroup": "I-GPE",
                "Score": 0.7503862692752757,
                "Start": 24,
                "End": 29
            },
            {
                "EntityGroup": "I-GPE",
                "Score": 0.8744298207460659,
                "Start": 29,
                "End": 31
            },
            {
                "EntityGroup": "I-GPE",
                "Score": 0.8751325364180739,
                "Start": 31,
                "End": 36
            },
            {
                "EntityGroup": "I-GPE",
                "Score": 0.9193591785439892,
                "Start": 36,
                "End": 39
            },
            {
                "EntityGroup": "I-GPE",
                "Score": 0.9496926277495757,
                "Start": 39,
                "End": 42
            },
            {
                "EntityGroup": "I-GPE",
                "Score": 0.9379511655627142,
                "Start": 42,
                "End": 43
            },
            {
                "EntityGroup": "O",
                "Score": 0.9999782305576639,
                "Start": 44,
                "End": 55
            },
            {
                "EntityGroup": "B-DATE",
                "Score": 0.9999094646936848,
                "Start": 56,
                "End": 62
            },
            {
                "EntityGroup": "I-DATE",
                "Score": 0.9998611485439627,
                "Start": 63,
                "End": 65
            },
            {
                "EntityGroup": "I-DATE",
                "Score": 0.9998383801645545,
                "Start": 65,
                "End": 67
            },
            {
                "EntityGroup": "O",
                "Score": 0.9996668204796613,
                "Start": 67,
                "End": 68
            },
            {
                "EntityGroup": "O",
                "Score": 0.9996668949436028,
                "Start": 68,
                "End": 68
            }
            ]
            """;

        var predictions = JsonSerializer.Deserialize<List<Prediction>>(predictionsJson);
        var text = "Helsingistä tuli Suomen suuriruhtinaskunnan pääkaupunki vuonna 1812.";

        var actual = NerModelRunner.GetGroupedPredictions(predictions!, text).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(actual, Has.Count.EqualTo(3));

            Assert.That(actual[0].EntityGroup, Is.EqualTo("GPE"));
            Assert.That(actual[0].Word, Is.EqualTo("Helsingistä"));
            Assert.That(actual[0].Start, Is.EqualTo(0));
            Assert.That(actual[0].End, Is.EqualTo(11));
            Assert.That(actual[0].Score, Is.EqualTo(0.999520361f));

            Assert.That(actual[1].EntityGroup, Is.EqualTo("GPE"));
            Assert.That(actual[1].Word, Is.EqualTo("Suomen suuriruhtinaskunnan"));
            Assert.That(actual[1].Start, Is.EqualTo(17));
            Assert.That(actual[1].End, Is.EqualTo(43));
            Assert.That(actual[1].Score, Is.EqualTo(0.889762998f));

            Assert.That(actual[2].EntityGroup, Is.EqualTo("DATE"));
            Assert.That(actual[2].Word, Is.EqualTo("vuonna 1812"));
            Assert.That(actual[2].Start, Is.EqualTo(56));
            Assert.That(actual[2].End, Is.EqualTo(67));
            Assert.That(actual[2].Score, Is.EqualTo(0.999869645f));
        });
    }


    [Test]
    public void GetWindowIndexesWithStride_Multiple()
    {
        var actual = NerModelRunner.GetWindowIndexesWithStride(1000, 500, 100);

        Assert.Multiple(() =>
        {
            Assert.That(actual, Has.Count.EqualTo(3));
            Assert.That(actual[0], Is.EqualTo(0));
            Assert.That(actual[1], Is.EqualTo(400));
            Assert.That(actual[2], Is.EqualTo(800));
        });
    }

    [Test]
    public void GetWindowIndexesWithStride_Single()
    {
        var actual = NerModelRunner.GetWindowIndexesWithStride(300, 500, 100);

        Assert.Multiple(() =>
        {
            Assert.That(actual, Has.Count.EqualTo(1));
            Assert.That(actual[0], Is.EqualTo(0));
        });
    }

    [Test]
    public void GetWindowIndexesWithStride_Multiple_NoOverflow()
    {
        var actual = NerModelRunner.GetWindowIndexesWithStride(900, 500, 100);

        Assert.Multiple(() =>
        {
            Assert.That(actual, Has.Count.EqualTo(2));
            Assert.That(actual[0], Is.EqualTo(0));
            Assert.That(actual[1], Is.EqualTo(400));
        });
    }

    [Test]
    public void GetWindowIndexesWithStride_Multiple_TestSomething()
    {
        var actual = NerModelRunner.GetWindowIndexesWithStride(30, 10, 2);

        Assert.Multiple(() =>
        {
            Assert.That(actual, Has.Count.EqualTo(4));
            Assert.That(actual[0], Is.EqualTo(0));
            Assert.That(actual[1], Is.EqualTo(8));
            Assert.That(actual[2], Is.EqualTo(16));
            Assert.That(actual[3], Is.EqualTo(24));
        });
    }

    [Test]
    public void GetWindowIndexesWithStride_Single_Max()
    {
        var actual = NerModelRunner.GetWindowIndexesWithStride(500, 500, 100);

        Assert.Multiple(() =>
        {
            Assert.That(actual, Has.Count.EqualTo(1));
            Assert.That(actual[0], Is.EqualTo(0));
        });
    }

    [Test]
    public void GetTokenPredictions()
    {
        var labels = new Dictionary<int, string>
        {
            { 0, "A" },
            { 1, "B" },
            { 2, "C" },
        };

        Token[] tokens =
        [
            new(0, 0, 1),
            new(1, 5, 6),
            new(2, 10, 11),
        ];

        float[] values =
        [
            7.2f,
            4f,
            2f,
            1.0f,
            6f,
            1.23f,
            4f,
            3f,
            10f,
        ];

        var actual = NerModelRunner
            .GetTokenPredictions([..labels.Values], tokens, values).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(actual, Has.Count.EqualTo(3));
            Assert.That(actual[0].Score, Is.EqualTo(0.955768228f));
            Assert.That(actual[1].Score, Is.EqualTo(0.985009789f));
            Assert.That(actual[2].Score, Is.EqualTo(0.996620834f));

            Assert.That(actual[0].EntityGroup, Is.EqualTo("A"));
            Assert.That(actual[1].EntityGroup, Is.EqualTo("B"));
            Assert.That(actual[2].EntityGroup, Is.EqualTo("C"));
        });
    }
}