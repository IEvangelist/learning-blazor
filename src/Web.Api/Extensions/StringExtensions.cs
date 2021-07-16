// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Security.Cryptography;
using System.Text;

namespace Learning.Blazor.Api.Extensions
{
    internal static class StringExtensions
    {
        internal static string? ToSha1Hash(this string? value)
        {
            if (value is null or {  Length: 0 })
            {
                return value;
            }

            using SHA1 sha1 = SHA1.Create();

            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(value));
            StringBuilder stringBuilder = new(hash.Length * 2);

            foreach (var b in hash)
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
