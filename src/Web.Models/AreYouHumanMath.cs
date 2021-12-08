// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Runtime.Versioning;

namespace Learning.Blazor.Models;

[
    RequiresPreviewFeatures(
        "Generic Math is in preview.",
        Url = "https://aka.ms/dotnet-warnings/generic-math-preview")
]
public readonly record struct AreYouHumanMath<T>(
    T LeftOperand,
    T RightOperand,
    MathOperator Operator = MathOperator.Addition)
    where T : INumber<T>
{
    private static readonly Lazy<Random> s_random = new(() => new());

    public bool IsCorrect(T result) => result == Operator switch
    {
        MathOperator.Addition => LeftOperand + RightOperand,
        MathOperator.Subtraction => LeftOperand - RightOperand,
        MathOperator.Multiplication => LeftOperand * RightOperand,
        MathOperator.Division => LeftOperand / RightOperand,

        _ => throw new ArgumentException(
            $"The operator is not supported: {Operator}")
    };

    public override string ToString() =>
        $"{LeftOperand} {(Operator == MathOperator.Addition ? "+" : "-")} {RightOperand} =";

    public string GetQuestion() => $"{this} ?";

    public static AreYouHumanMath<int> RandomIntegerFactory(
        MathOperator mathOperator = MathOperator.Addition) =>
        new(s_random.Value.Next(1, 500),
            s_random.Value.Next(1, 500),
            mathOperator);
}

public enum MathOperator
{
    Addition,
    Subtraction,
    Multiplication,
    Division
};
