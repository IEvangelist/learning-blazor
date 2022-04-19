// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Humanizer;

namespace Learning.Blazor.Extensions;

internal static class AreYouHumanMathExtensions
{
    internal static string HumanizeQuestion(this AreYouHumanMath math)
    {
        var (leftOperand, rightOperand, mathOperator) = math;
        var operatorStr = mathOperator switch
        {
            MathOperator.Addition => "plus (➕)",
            MathOperator.Subtraction => "minus (➖)",
            MathOperator.Multiplication => "times (❌)",

            _ => throw new ArgumentException(
                $"The operator is not supported: {mathOperator}")
        };

        static string ToQtyWords(int number) =>
            "".ToQuantity(number, ShowQuantityAs.Words);

        var (leftOperandPhrase, rightOperandPhrase) =
            (ToQtyWords(leftOperand), ToQtyWords(rightOperand));

        // Given:  "7 + 3 = ?"
        // Expect: "What does seven plus (➕) three equal?"
        return $"What does {leftOperandPhrase} {operatorStr} {rightOperandPhrase} equal?";
    }
}
