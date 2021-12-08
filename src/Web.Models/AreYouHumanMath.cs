// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Models;

public readonly record struct AreYouHumanMath(
    byte LeftOperand,
    byte RightOperand,
    MathOperator Operator = MathOperator.Addition)
{
    private static readonly Lazy<Random> s_random = new(() => new());

    public bool IsCorrect(int result) => result == Operator switch
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

    public static AreYouHumanMath RandomFactory(
        MathOperator mathOperator = MathOperator.Addition) =>
        new((byte)s_random.Value.Next(1, byte.MaxValue),
            (byte)s_random.Value.Next(1, byte.MaxValue),
            mathOperator);
}

public enum MathOperator
{
    Addition,
    Subtraction,
    Multiplication,
    Division
};
