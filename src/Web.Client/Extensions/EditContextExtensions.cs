// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions;

public static class EditContextExtensions
{
    /// <summary>
    /// Maps the given <paramref name="accessor"/> expression to the resulting
    /// CSS returned from calling
    /// <see cref="EditContextFieldClassExtensions.FieldCssClass"/> as follows:
    /// <list type="bullet">
    /// <item><c>"modified valid"</c>: <c>"fa-check-circle has-text-success"</c></item>
    /// <item><c>"modified invalid"</c>: <c>"fa-times-circle has-text-danger"</c></item>
    /// <item><c>""</c>: <c>"fa-question-circle"</c></item>
    /// </list>
    /// </summary>
    public static string GetValidityCss<T>(
        this EditContext context,
        Expression<Func<T?>> accessor)
    {
        var css = context?.FieldCssClass(accessor);
        return GetValidityCss(
            IsValid(css),
            IsInvalid(css),
            IsModified(css));
    }

    /// <summary>
    /// Maps the given <paramref name="accessorOne"/> and
    /// <paramref name="accessorTwo"/> expressions to the resulting
    /// CSS returned from calling
    /// <see cref="EditContextFieldClassExtensions.FieldCssClass"/> as follows:
    /// <list type="bullet">
    /// <item><c>"modified valid"</c>: <c>"fa-check-circle has-text-success"</c></item>
    /// <item><c>"modified invalid"</c>: <c>"fa-times-circle has-text-danger"</c></item>
    /// <item><c>""</c>: <c>"fa-question-circle"</c></item>
    /// </list>
    /// </summary>
    public static string GetValidityCss<TOne, TTwo>(
        this EditContext context,
        Expression<Func<TOne?>> accessorOne,
        Expression<Func<TTwo?>> accessorTwo)
    {
        var cssOne = context?.FieldCssClass(accessorOne);
        var cssTwo = context?.FieldCssClass(accessorTwo);
        return GetValidityCss(
            IsValid(cssOne) && IsValid(cssTwo),
            IsInvalid(cssOne) || IsInvalid(cssTwo),
            IsModified(cssOne) && IsModified(cssTwo));
    }

    /// <summary>
    /// Maps the given validation states into corresponding CSS classes.
    /// <list type="bullet">
    /// <item><c>"modified valid"</c>: <c>"fa-check-circle has-text-success"</c></item>
    /// <item><c>"modified invalid"</c>: <c>"fa-times-circle has-text-danger"</c></item>
    /// <item><c>""</c>: <c>"fa-question-circle"</c></item>
    /// </list>
    /// </summary>
    public static string GetValidityCss(
        bool isValid, bool isInvalid, bool isModified) =>
        (isValid, isInvalid) switch
        {
            (true, false) when isModified => "fa-check-circle has-text-success",
            (false, true) when isModified => "fa-times-circle has-text-danger",

            _ => "fa-question-circle"
        };
    
    private static bool IsValid(string? css) =>
        IsContainingClass(css, "valid") && !IsInvalid(css);

    private static bool IsInvalid(string? css) =>
        IsContainingClass(css, "invalid");

    private static bool IsModified(string? css) =>
        IsContainingClass(css, "modified");

    private static bool IsContainingClass(string? css, string name) =>
        css?.Contains(name, StringComparison.OrdinalIgnoreCase) ?? false;
}
