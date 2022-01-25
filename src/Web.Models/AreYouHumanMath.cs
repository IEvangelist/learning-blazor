// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Models;

public readonly record struct AreYouHumanMath(
    byte LeftOperand,
    byte RightOperand,
    MathOperator Operator = MathOperator.Addition)
{
    private static readonly Random s_random = Random.Shared;

    /// <summary>
    /// Determines if the given <paramref name="guess"/> value is correct.
    /// </summary>
    /// <param name="guess">The value being evaluated for correctness.</param>
    /// <returns>
    /// <c>true</c> when the given <paramref name="guess"/> is correct,
    /// otherwise <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// An <see cref="ArgumentException"/> is thrown when
    /// the current <see cref="Operator"/> value is not defined.
    /// </exception>
    public bool IsCorrect(int guess) => guess == Operator switch
    {
        MathOperator.Addition => LeftOperand + RightOperand,
        MathOperator.Subtraction => LeftOperand - RightOperand,
        MathOperator.Multiplication => LeftOperand * RightOperand,

        _ => throw new ArgumentException(
            $"The operator is not supported: {Operator}")
    };

    /// <summary>
    /// The string representation of the <see cref="AreYouHumanMath"/> instance.
    /// <code language="cs">
    /// <![CDATA[
    /// var math = new AreYouHumanMath(7, 3);
    /// math.ToString(); // "7 + 3 ="
    /// ]]>
    /// </code>
    /// </summary>
    /// <exception cref="ArgumentException">
    /// An <see cref="ArgumentException"/> is thrown when
    /// the current <see cref="Operator"/> value is not defined.
    /// </exception>
    public override string ToString()
    {
        var operatorStr = Operator switch
        {
            MathOperator.Addition => "+",
            MathOperator.Subtraction => "-",
            MathOperator.Multiplication => "*",

            _ => throw new ArgumentException(
                $"The operator is not supported: {Operator}")
        };

        return $"{LeftOperand} {operatorStr} {RightOperand} =";
    }

    /// <summary>
    /// The string representation of the <see cref="AreYouHumanMath"/> instance.
    /// <code language="cs">
    /// <![CDATA[
    /// var math = new AreYouHumanMath(7, 3);
    /// math.GetQuestion(); // "7 + 3 = ?"
    /// ]]>
    /// </code>
    /// </summary>
    public string GetQuestion() => $"{this} ?";

    /// <summary>
    /// Factory method that creates a new randomized instance of
    /// <see cref="AreYouHumanMath"/> given the <paramref name="mathOperator"/> 
    /// </summary>
    public static AreYouHumanMath CreateNew(
        MathOperator? mathOperator = null)
    {
        var mathOp =
            mathOperator.GetValueOrDefault(RandomOperator());

        var (left, right) = mathOp switch
        {
            MathOperator.Addition => (Next(), Next()),
            MathOperator.Subtraction => (Next(120), Next(120)),
            _ => (Next(30), Next(30)),
        };

        (left, right) = (Math.Max(left, right), Math.Min(left, right));

        return new AreYouHumanMath(
            (byte)left,
            (byte)right,
            mathOp);

        static MathOperator RandomOperator()
        {
            var values = Enum.GetValues<MathOperator>();
            return values[s_random.Next(values.Length)];
        };

        static int Next(byte? maxValue = null) =>
            s_random.Next(1, maxValue ?? byte.MaxValue);
    }
}

public enum MathOperator { Addition, Subtraction, Multiplication };
