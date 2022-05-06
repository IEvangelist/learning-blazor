// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Blazor.Serialization.Extensions;
using Learning.Blazor.Models;

namespace Learning.Blazor.Extensions.Tests;

public class SerializationExtensionsTests
{
    [Fact]
    public void JsonCorrectlyDeserializesFrenchTest()
    {
        var json = @"{
  ""lat"": 48.8632,
  ""lon"": -2.3398,
  ""timezone"": ""Europe/Paris"",
  ""timezone_offset"": 7200,
  ""current"": {
            ""temp"": 289.61,
    ""feels_like"": 289.48,
    ""sunrise"": 1651553144,
    ""sunset"": 1651605998,
    ""pressure"": 1020,
    ""humidity"": 83,
    ""dew_point"": 286.72,
    ""uvi"": 4.88,
    ""clouds"": 13,
    ""visibility"": 10000,
    ""wind_speed"": 5.46,
    ""wind_deg"": 358,
    ""weather"": [
      {
                ""main"": ""Clouds"",
        ""description"": ""peu nuageux"",
        ""icon"": ""02d""
      }
    ],
    ""dt"": 1651587531
  },
  ""hourly"": [
    {
            ""wind_gust"": 6.41,
      ""temp"": 289.61,
      ""feels_like"": 289.48,
      ""pressure"": 1020,
      ""humidity"": 83,
      ""dew_point"": 286.72,
      ""uvi"": 4.88,
      ""clouds"": 13,
      ""visibility"": 10000,
      ""wind_speed"": 5.46,
      ""wind_deg"": 358,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""peu nuageux"",
          ""icon"": ""02d""
        }
      ],
      ""dt"": 1651586400
    },
    {
            ""wind_gust"": 6.21,
      ""temp"": 288.93,
      ""feels_like"": 288.7,
      ""pressure"": 1020,
      ""humidity"": 82,
      ""dew_point"": 285.87,
      ""uvi"": 3.53,
      ""clouds"": 13,
      ""visibility"": 10000,
      ""wind_speed"": 5.25,
      ""wind_deg"": 1,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""peu nuageux"",
          ""icon"": ""02d""
        }
      ],
      ""dt"": 1651590000
    },
    {
            ""wind_gust"": 6.03,
      ""temp"": 288.31,
      ""feels_like"": 288.02,
      ""pressure"": 1020,
      ""humidity"": 82,
      ""dew_point"": 285.26,
      ""uvi"": 2.08,
      ""clouds"": 13,
      ""visibility"": 10000,
      ""wind_speed"": 5.17,
      ""wind_deg"": 2,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""peu nuageux"",
          ""icon"": ""02d""
        }
      ],
      ""dt"": 1651593600
    },
    {
            ""wind_gust"": 5.96,
      ""temp"": 287.68,
      ""feels_like"": 287.3,
      ""pressure"": 1020,
      ""humidity"": 81,
      ""dew_point"": 284.46,
      ""uvi"": 1.01,
      ""clouds"": 12,
      ""visibility"": 10000,
      ""wind_speed"": 5.1,
      ""wind_deg"": 4,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""peu nuageux"",
          ""icon"": ""02d""
        }
      ],
      ""dt"": 1651597200
    },
    {
            ""wind_gust"": 5.47,
      ""temp"": 287.03,
      ""feels_like"": 286.56,
      ""pressure"": 1019,
      ""humidity"": 80,
      ""dew_point"": 283.64,
      ""uvi"": 0.36,
      ""clouds"": 12,
      ""visibility"": 10000,
      ""wind_speed"": 4.74,
      ""wind_deg"": 9,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""peu nuageux"",
          ""icon"": ""02d""
        }
      ],
      ""dt"": 1651600800
    },
    {
            ""wind_gust"": 4.82,
      ""temp"": 286.29,
      ""feels_like"": 285.72,
      ""pressure"": 1020,
      ""humidity"": 79,
      ""dew_point"": 282.74,
      ""uvi"": 0,
      ""clouds"": 5,
      ""visibility"": 10000,
      ""wind_speed"": 4.25,
      ""wind_deg"": 18,
      ""weather"": [
        {
                ""main"": ""Clear"",
          ""description"": ""ciel dégagé"",
          ""icon"": ""01d""
        }
      ],
      ""dt"": 1651604400
    },
    {
            ""wind_gust"": 4.56,
      ""temp"": 286.22,
      ""feels_like"": 285.65,
      ""pressure"": 1021,
      ""humidity"": 79,
      ""dew_point"": 282.66,
      ""uvi"": 0,
      ""clouds"": 4,
      ""visibility"": 10000,
      ""wind_speed"": 3.98,
      ""wind_deg"": 29,
      ""weather"": [
        {
                ""main"": ""Clear"",
          ""description"": ""ciel dégagé"",
          ""icon"": ""01n""
        }
      ],
      ""dt"": 1651608000
    },
    {
            ""wind_gust"": 4.57,
      ""temp"": 285.92,
      ""feels_like"": 285.34,
      ""pressure"": 1021,
      ""humidity"": 80,
      ""dew_point"": 282.61,
      ""uvi"": 0,
      ""clouds"": 5,
      ""visibility"": 10000,
      ""wind_speed"": 3.81,
      ""wind_deg"": 44,
      ""weather"": [
        {
                ""main"": ""Clear"",
          ""description"": ""ciel dégagé"",
          ""icon"": ""01n""
        }
      ],
      ""dt"": 1651611600
    },
    {
            ""wind_gust"": 4.17,
      ""temp"": 285.61,
      ""feels_like"": 285.05,
      ""pressure"": 1021,
      ""humidity"": 82,
      ""dew_point"": 282.55,
      ""uvi"": 0,
      ""clouds"": 6,
      ""visibility"": 10000,
      ""wind_speed"": 3.59,
      ""wind_deg"": 53,
      ""weather"": [
        {
                ""main"": ""Clear"",
          ""description"": ""ciel dégagé"",
          ""icon"": ""01n""
        }
      ],
      ""dt"": 1651615200
    },
    {
            ""wind_gust"": 3.74,
      ""temp"": 285.33,
      ""feels_like"": 284.74,
      ""pressure"": 1021,
      ""humidity"": 82,
      ""dew_point"": 282.35,
      ""uvi"": 0,
      ""clouds"": 8,
      ""visibility"": 10000,
      ""wind_speed"": 3.28,
      ""wind_deg"": 68,
      ""weather"": [
        {
                ""main"": ""Clear"",
          ""description"": ""ciel dégagé"",
          ""icon"": ""01n""
        }
      ],
      ""dt"": 1651618800
    },
    {
            ""wind_gust"": 3.62,
      ""temp"": 285.11,
      ""feels_like"": 284.5,
      ""pressure"": 1021,
      ""humidity"": 82,
      ""dew_point"": 282.21,
      ""uvi"": 0,
      ""clouds"": 8,
      ""visibility"": 10000,
      ""wind_speed"": 3.21,
      ""wind_deg"": 74,
      ""weather"": [
        {
                ""main"": ""Clear"",
          ""description"": ""ciel dégagé"",
          ""icon"": ""01n""
        }
      ],
      ""dt"": 1651622400
    },
    {
            ""wind_gust"": 2.93,
      ""temp"": 285.04,
      ""feels_like"": 284.45,
      ""pressure"": 1021,
      ""humidity"": 83,
      ""dew_point"": 282.19,
      ""uvi"": 0,
      ""clouds"": 13,
      ""visibility"": 10000,
      ""wind_speed"": 2.52,
      ""wind_deg"": 66,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""peu nuageux"",
          ""icon"": ""02n""
        }
      ],
      ""dt"": 1651626000
    },
    {
            ""wind_gust"": 1.88,
      ""temp"": 284.97,
      ""feels_like"": 284.37,
      ""pressure"": 1021,
      ""humidity"": 83,
      ""dew_point"": 282.15,
      ""uvi"": 0,
      ""clouds"": 21,
      ""visibility"": 10000,
      ""wind_speed"": 1.64,
      ""wind_deg"": 62,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""peu nuageux"",
          ""icon"": ""02n""
        }
      ],
      ""dt"": 1651629600
    },
    {
            ""wind_gust"": 1.36,
      ""temp"": 284.93,
      ""feels_like"": 284.3,
      ""pressure"": 1020,
      ""humidity"": 82,
      ""dew_point"": 282,
      ""uvi"": 0,
      ""clouds"": 18,
      ""visibility"": 10000,
      ""wind_speed"": 1.08,
      ""wind_deg"": 62,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""peu nuageux"",
          ""icon"": ""02n""
        }
      ],
      ""dt"": 1651633200
    },
    {
            ""wind_gust"": 1.35,
      ""temp"": 284.94,
      ""feels_like"": 284.32,
      ""pressure"": 1021,
      ""humidity"": 82,
      ""dew_point"": 281.95,
      ""uvi"": 0,
      ""clouds"": 23,
      ""visibility"": 10000,
      ""wind_speed"": 0.9,
      ""wind_deg"": 346,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""peu nuageux"",
          ""icon"": ""02n""
        }
      ],
      ""dt"": 1651636800
    },
    {
            ""wind_gust"": 1.32,
      ""temp"": 284.99,
      ""feels_like"": 284.34,
      ""pressure"": 1020,
      ""humidity"": 81,
      ""dew_point"": 281.87,
      ""uvi"": 0,
      ""clouds"": 36,
      ""visibility"": 10000,
      ""wind_speed"": 0.91,
      ""wind_deg"": 300,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""partiellement nuageux"",
          ""icon"": ""03d""
        }
      ],
      ""dt"": 1651640400
    },
    {
            ""wind_gust"": 1.77,
      ""temp"": 285.08,
      ""feels_like"": 284.44,
      ""pressure"": 1021,
      ""humidity"": 81,
      ""dew_point"": 281.93,
      ""uvi"": 0.23,
      ""clouds"": 37,
      ""visibility"": 10000,
      ""wind_speed"": 1.59,
      ""wind_deg"": 278,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""partiellement nuageux"",
          ""icon"": ""03d""
        }
      ],
      ""dt"": 1651644000
    },
    {
            ""wind_gust"": 2.42,
      ""temp"": 285.17,
      ""feels_like"": 284.57,
      ""pressure"": 1021,
      ""humidity"": 82,
      ""dew_point"": 282.16,
      ""uvi"": 0.76,
      ""clouds"": 15,
      ""visibility"": 10000,
      ""wind_speed"": 2.22,
      ""wind_deg"": 292,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""peu nuageux"",
          ""icon"": ""02d""
        }
      ],
      ""dt"": 1651647600
    },
    {
            ""wind_gust"": 2.97,
      ""temp"": 285.24,
      ""feels_like"": 284.65,
      ""pressure"": 1022,
      ""humidity"": 82,
      ""dew_point"": 282.29,
      ""uvi"": 1.71,
      ""clouds"": 13,
      ""visibility"": 10000,
      ""wind_speed"": 2.58,
      ""wind_deg"": 291,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""peu nuageux"",
          ""icon"": ""02d""
        }
      ],
      ""dt"": 1651651200
    },
    {
            ""wind_gust"": 3.54,
      ""temp"": 285.38,
      ""feels_like"": 284.8,
      ""pressure"": 1022,
      ""humidity"": 82,
      ""dew_point"": 282.33,
      ""uvi"": 3.02,
      ""clouds"": 11,
      ""visibility"": 10000,
      ""wind_speed"": 3.13,
      ""wind_deg"": 292,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""peu nuageux"",
          ""icon"": ""02d""
        }
      ],
      ""dt"": 1651654800
    },
    {
            ""wind_gust"": 4,
      ""temp"": 285.46,
      ""feels_like"": 284.89,
      ""pressure"": 1022,
      ""humidity"": 82,
      ""dew_point"": 282.42,
      ""uvi"": 4.43,
      ""clouds"": 10,
      ""visibility"": 10000,
      ""wind_speed"": 3.36,
      ""wind_deg"": 295,
      ""weather"": [
        {
                ""main"": ""Clear"",
          ""description"": ""ciel dégagé"",
          ""icon"": ""01d""
        }
      ],
      ""dt"": 1651658400
    },
    {
            ""wind_gust"": 4.32,
      ""temp"": 285.59,
      ""feels_like"": 285,
      ""pressure"": 1022,
      ""humidity"": 81,
      ""dew_point"": 282.36,
      ""uvi"": 5.54,
      ""clouds"": 9,
      ""visibility"": 10000,
      ""wind_speed"": 3.5,
      ""wind_deg"": 297,
      ""weather"": [
        {
                ""main"": ""Clear"",
          ""description"": ""ciel dégagé"",
          ""icon"": ""01d""
        }
      ],
      ""dt"": 1651662000
    },
    {
            ""wind_gust"": 4.82,
      ""temp"": 285.67,
      ""feels_like"": 285.09,
      ""pressure"": 1022,
      ""humidity"": 81,
      ""dew_point"": 282.55,
      ""uvi"": 6.04,
      ""clouds"": 9,
      ""visibility"": 10000,
      ""wind_speed"": 3.85,
      ""wind_deg"": 297,
      ""weather"": [
        {
                ""main"": ""Clear"",
          ""description"": ""ciel dégagé"",
          ""icon"": ""01d""
        }
      ],
      ""dt"": 1651665600
    },
    {
            ""wind_gust"": 5.39,
      ""temp"": 285.78,
      ""feels_like"": 285.24,
      ""pressure"": 1022,
      ""humidity"": 82,
      ""dew_point"": 282.76,
      ""uvi"": 5.74,
      ""clouds"": 10,
      ""visibility"": 10000,
      ""wind_speed"": 4.32,
      ""wind_deg"": 303,
      ""weather"": [
        {
                ""main"": ""Clear"",
          ""description"": ""ciel dégagé"",
          ""icon"": ""01d""
        }
      ],
      ""dt"": 1651669200
    },
    {
            ""wind_gust"": 6.04,
      ""temp"": 285.9,
      ""feels_like"": 285.37,
      ""pressure"": 1022,
      ""humidity"": 82,
      ""dew_point"": 282.95,
      ""uvi"": 4.8,
      ""clouds"": 12,
      ""visibility"": 10000,
      ""wind_speed"": 4.8,
      ""wind_deg"": 305,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""peu nuageux"",
          ""icon"": ""02d""
        }
      ],
      ""dt"": 1651672800
    },
    {
            ""wind_gust"": 6.43,
      ""temp"": 285.88,
      ""feels_like"": 285.4,
      ""pressure"": 1021,
      ""humidity"": 84,
      ""dew_point"": 283.17,
      ""uvi"": 3.48,
      ""clouds"": 33,
      ""visibility"": 10000,
      ""wind_speed"": 4.92,
      ""wind_deg"": 304,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""partiellement nuageux"",
          ""icon"": ""03d""
        }
      ],
      ""dt"": 1651676400
    },
    {
            ""wind_gust"": 6.91,
      ""temp"": 285.98,
      ""feels_like"": 285.51,
      ""pressure"": 1021,
      ""humidity"": 84,
      ""dew_point"": 283.29,
      ""uvi"": 1.9,
      ""clouds"": 50,
      ""visibility"": 10000,
      ""wind_speed"": 5.28,
      ""wind_deg"": 298,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""partiellement nuageux"",
          ""icon"": ""03d""
        }
      ],
      ""dt"": 1651680000
    },
    {
            ""wind_gust"": 8.16,
      ""temp"": 285.99,
      ""feels_like"": 285.52,
      ""pressure"": 1021,
      ""humidity"": 84,
      ""dew_point"": 283.38,
      ""uvi"": 0.93,
      ""clouds"": 60,
      ""visibility"": 10000,
      ""wind_speed"": 6.07,
      ""wind_deg"": 300,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""nuageux"",
          ""icon"": ""04d""
        }
      ],
      ""dt"": 1651683600
    },
    {
            ""wind_gust"": 9.13,
      ""temp"": 285.96,
      ""feels_like"": 285.52,
      ""pressure"": 1021,
      ""humidity"": 85,
      ""dew_point"": 283.5,
      ""uvi"": 0.33,
      ""clouds"": 67,
      ""visibility"": 10000,
      ""wind_speed"": 6.64,
      ""wind_deg"": 295,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""nuageux"",
          ""icon"": ""04d""
        }
      ],
      ""dt"": 1651687200
    },
    {
            ""wind_gust"": 9.11,
      ""temp"": 285.85,
      ""feels_like"": 285.42,
      ""pressure"": 1022,
      ""humidity"": 86,
      ""dew_point"": 283.58,
      ""uvi"": 0,
      ""clouds"": 96,
      ""visibility"": 10000,
      ""wind_speed"": 6.35,
      ""wind_deg"": 291,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""couvert"",
          ""icon"": ""04d""
        }
      ],
      ""dt"": 1651690800
    },
    {
            ""wind_gust"": 8.91,
      ""temp"": 285.77,
      ""feels_like"": 285.36,
      ""pressure"": 1022,
      ""humidity"": 87,
      ""dew_point"": 283.68,
      ""uvi"": 0,
      ""clouds"": 96,
      ""visibility"": 10000,
      ""wind_speed"": 6.09,
      ""wind_deg"": 287,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""couvert"",
          ""icon"": ""04n""
        }
      ],
      ""dt"": 1651694400
    },
    {
            ""wind_gust"": 8.84,
      ""temp"": 285.71,
      ""feels_like"": 285.37,
      ""pressure"": 1022,
      ""humidity"": 90,
      ""dew_point"": 284.13,
      ""uvi"": 0,
      ""clouds"": 95,
      ""visibility"": 10000,
      ""wind_speed"": 6.14,
      ""wind_deg"": 284,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""couvert"",
          ""icon"": ""04n""
        }
      ],
      ""dt"": 1651698000
    },
    {
            ""wind_gust"": 8.4,
      ""temp"": 285.64,
      ""feels_like"": 285.35,
      ""pressure"": 1023,
      ""humidity"": 92,
      ""dew_point"": 284.37,
      ""uvi"": 0,
      ""clouds"": 95,
      ""visibility"": 10000,
      ""wind_speed"": 6.14,
      ""wind_deg"": 287,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""couvert"",
          ""icon"": ""04n""
        }
      ],
      ""dt"": 1651701600
    },
    {
            ""wind_gust"": 7.39,
      ""temp"": 285.73,
      ""feels_like"": 285.42,
      ""pressure"": 1023,
      ""humidity"": 91,
      ""dew_point"": 284.31,
      ""uvi"": 0,
      ""clouds"": 85,
      ""visibility"": 10000,
      ""wind_speed"": 5.72,
      ""wind_deg"": 295,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""couvert"",
          ""icon"": ""04n""
        }
      ],
      ""dt"": 1651705200
    },
    {
            ""wind_gust"": 6.51,
      ""temp"": 285.71,
      ""feels_like"": 285.37,
      ""pressure"": 1023,
      ""humidity"": 90,
      ""dew_point"": 284.03,
      ""uvi"": 0,
      ""clouds"": 77,
      ""visibility"": 10000,
      ""wind_speed"": 5.21,
      ""wind_deg"": 304,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""nuageux"",
          ""icon"": ""04n""
        }
      ],
      ""dt"": 1651708800
    },
    {
            ""wind_gust"": 5.91,
      ""temp"": 285.68,
      ""feels_like"": 285.29,
      ""pressure"": 1023,
      ""humidity"": 88,
      ""dew_point"": 283.75,
      ""uvi"": 0,
      ""clouds"": 36,
      ""visibility"": 10000,
      ""wind_speed"": 4.85,
      ""wind_deg"": 308,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""partiellement nuageux"",
          ""icon"": ""03n""
        }
      ],
      ""dt"": 1651712400
    },
    {
            ""wind_gust"": 5.52,
      ""temp"": 285.58,
      ""feels_like"": 285.15,
      ""pressure"": 1023,
      ""humidity"": 87,
      ""dew_point"": 283.57,
      ""uvi"": 0,
      ""clouds"": 65,
      ""visibility"": 10000,
      ""wind_speed"": 4.68,
      ""wind_deg"": 309,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""nuageux"",
          ""icon"": ""04n""
        }
      ],
      ""dt"": 1651716000
    },
    {
            ""wind_gust"": 5.34,
      ""temp"": 285.59,
      ""feels_like"": 285.14,
      ""pressure"": 1023,
      ""humidity"": 86,
      ""dew_point"": 283.36,
      ""uvi"": 0,
      ""clouds"": 70,
      ""visibility"": 10000,
      ""wind_speed"": 4.53,
      ""wind_deg"": 312,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""nuageux"",
          ""icon"": ""04n""
        }
      ],
      ""dt"": 1651719600
    },
    {
            ""wind_gust"": 5.06,
      ""temp"": 285.54,
      ""feels_like"": 285.08,
      ""pressure"": 1023,
      ""humidity"": 86,
      ""dew_point"": 283.22,
      ""uvi"": 0,
      ""clouds"": 77,
      ""visibility"": 10000,
      ""wind_speed"": 4.34,
      ""wind_deg"": 314,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""nuageux"",
          ""icon"": ""04n""
        }
      ],
      ""dt"": 1651723200
    },
    {
            ""wind_gust"": 5.29,
      ""temp"": 285.47,
      ""feels_like"": 284.98,
      ""pressure"": 1024,
      ""humidity"": 85,
      ""dew_point"": 283.13,
      ""uvi"": 0,
      ""clouds"": 73,
      ""visibility"": 10000,
      ""wind_speed"": 4.43,
      ""wind_deg"": 317,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""nuageux"",
          ""icon"": ""04d""
        }
      ],
      ""dt"": 1651726800
    },
    {
            ""wind_gust"": 5.07,
      ""temp"": 285.48,
      ""feels_like"": 284.99,
      ""pressure"": 1024,
      ""humidity"": 85,
      ""dew_point"": 283.04,
      ""uvi"": 0.22,
      ""clouds"": 66,
      ""visibility"": 10000,
      ""wind_speed"": 4.08,
      ""wind_deg"": 325,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""nuageux"",
          ""icon"": ""04d""
        }
      ],
      ""dt"": 1651730400
    },
    {
            ""wind_gust"": 4.69,
      ""temp"": 285.57,
      ""feels_like"": 285.06,
      ""pressure"": 1025,
      ""humidity"": 84,
      ""dew_point"": 282.98,
      ""uvi"": 0.69,
      ""clouds"": 79,
      ""visibility"": 10000,
      ""wind_speed"": 3.8,
      ""wind_deg"": 328,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""nuageux"",
          ""icon"": ""04d""
        }
      ],
      ""dt"": 1651734000
    },
    {
            ""wind_gust"": 3.82,
      ""temp"": 285.62,
      ""feels_like"": 285.12,
      ""pressure"": 1025,
      ""humidity"": 84,
      ""dew_point"": 282.9,
      ""uvi"": 1.56,
      ""clouds"": 90,
      ""visibility"": 10000,
      ""wind_speed"": 3.24,
      ""wind_deg"": 328,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""couvert"",
          ""icon"": ""04d""
        }
      ],
      ""dt"": 1651737600
    },
    {
            ""wind_gust"": 3.2,
      ""temp"": 285.71,
      ""feels_like"": 285.19,
      ""pressure"": 1026,
      ""humidity"": 83,
      ""dew_point"": 282.77,
      ""uvi"": 2.75,
      ""clouds"": 82,
      ""visibility"": 10000,
      ""wind_speed"": 2.89,
      ""wind_deg"": 326,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""nuageux"",
          ""icon"": ""04d""
        }
      ],
      ""dt"": 1651741200
    },
    {
            ""wind_gust"": 3.09,
      ""temp"": 285.82,
      ""feels_like"": 285.26,
      ""pressure"": 1026,
      ""humidity"": 81,
      ""dew_point"": 282.72,
      ""uvi"": 3.8,
      ""clouds"": 68,
      ""visibility"": 10000,
      ""wind_speed"": 2.76,
      ""wind_deg"": 324,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""nuageux"",
          ""icon"": ""04d""
        }
      ],
      ""dt"": 1651744800
    },
    {
            ""wind_gust"": 3.15,
      ""temp"": 285.92,
      ""feels_like"": 285.37,
      ""pressure"": 1026,
      ""humidity"": 81,
      ""dew_point"": 282.68,
      ""uvi"": 4.75,
      ""clouds"": 59,
      ""visibility"": 10000,
      ""wind_speed"": 2.77,
      ""wind_deg"": 312,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""nuageux"",
          ""icon"": ""04d""
        }
      ],
      ""dt"": 1651748400
    },
    {
            ""wind_gust"": 3.65,
      ""temp"": 286.11,
      ""feels_like"": 285.55,
      ""pressure"": 1026,
      ""humidity"": 80,
      ""dew_point"": 282.73,
      ""uvi"": 5.18,
      ""clouds"": 53,
      ""visibility"": 10000,
      ""wind_speed"": 3.12,
      ""wind_deg"": 305,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""nuageux"",
          ""icon"": ""04d""
        }
      ],
      ""dt"": 1651752000
    },
    {
            ""wind_gust"": 4.3,
      ""temp"": 286.29,
      ""feels_like"": 285.75,
      ""pressure"": 1026,
      ""humidity"": 80,
      ""dew_point"": 282.85,
      ""uvi"": 5.35,
      ""clouds"": 26,
      ""visibility"": 10000,
      ""wind_speed"": 3.66,
      ""wind_deg"": 304,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""partiellement nuageux"",
          ""icon"": ""03d""
        }
      ],
      ""dt"": 1651755600
    }
  ],
  ""daily"": [
    {
            ""sunrise"": 1651553144,
      ""sunset"": 1651605998,
      ""moonrise"": 1651557480,
      ""moonset"": 0,
      ""moon_phase"": 0.08,
      ""temp"": {
                ""min"": 285.03,
        ""max"": 289.61,
        ""day"": 288.02,
        ""night"": 285.92,
        ""eve"": 287.03,
        ""morn"": 285.16
      },
      ""feels_like"": {
                ""day"": 287.73,
        ""night"": 285.34,
        ""eve"": 286.56,
        ""morn"": 284.56
      },
      ""pressure"": 1020,
      ""humidity"": 83,
      ""dew_point"": 285.16,
      ""uvi"": 5.83,
      ""clouds"": 38,
      ""visibility"": 0,
      ""wind_speed"": 5.77,
      ""wind_deg"": 29,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""partiellement nuageux"",
          ""icon"": ""03d""
        }
      ],
      ""dt"": 1651579200
    },
    {
            ""sunrise"": 1651639446,
      ""sunset"": 1651692485,
      ""moonrise"": 1651645860,
      ""moonset"": 1651617420,
      ""moon_phase"": 0.11,
      ""temp"": {
                ""min"": 284.93,
        ""max"": 285.99,
        ""day"": 285.67,
        ""night"": 285.71,
        ""eve"": 285.96,
        ""morn"": 285.08
      },
      ""feels_like"": {
                ""day"": 285.09,
        ""night"": 285.37,
        ""eve"": 285.52,
        ""morn"": 284.44
      },
      ""pressure"": 1022,
      ""humidity"": 81,
      ""dew_point"": 282.55,
      ""uvi"": 6.04,
      ""clouds"": 9,
      ""visibility"": 0,
      ""wind_speed"": 6.64,
      ""wind_deg"": 295,
      ""weather"": [
        {
                ""main"": ""Clear"",
          ""description"": ""ciel dégagé"",
          ""icon"": ""01d""
        }
      ],
      ""dt"": 1651665600
    },
    {
            ""sunrise"": 1651725750,
      ""sunset"": 1651778973,
      ""moonrise"": 1651734840,
      ""moonset"": 1651707540,
      ""moon_phase"": 0.14,
      ""temp"": {
                ""min"": 285.47,
        ""max"": 286.6,
        ""day"": 286.11,
        ""night"": 286.23,
        ""eve"": 286.55,
        ""morn"": 285.48
      },
      ""feels_like"": {
                ""day"": 285.55,
        ""night"": 285.79,
        ""eve"": 286.14,
        ""morn"": 284.99
      },
      ""pressure"": 1026,
      ""humidity"": 80,
      ""dew_point"": 282.73,
      ""uvi"": 5.35,
      ""clouds"": 53,
      ""visibility"": 0,
      ""wind_speed"": 6.14,
      ""wind_deg"": 287,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""nuageux"",
          ""icon"": ""04d""
        }
      ],
      ""dt"": 1651752000
    },
    {
            ""sunrise"": 1651812055,
      ""sunset"": 1651865460,
      ""moonrise"": 1651824360,
      ""moonset"": 1651797060,
      ""moon_phase"": 0.17,
      ""temp"": {
                ""min"": 285.44,
        ""max"": 286.37,
        ""day"": 286.03,
        ""night"": 285.93,
        ""eve"": 286.34,
        ""morn"": 285.44
      },
      ""feels_like"": {
                ""day"": 285.72,
        ""night"": 285.67,
        ""eve"": 286.04,
        ""morn"": 285.1
      },
      ""pressure"": 1026,
      ""humidity"": 90,
      ""dew_point"": 284.34,
      ""uvi"": 6.12,
      ""clouds"": 100,
      ""visibility"": 0,
      ""wind_speed"": 5.98,
      ""wind_deg"": 300,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""couvert"",
          ""icon"": ""04d""
        }
      ],
      ""dt"": 1651838400
    },
    {
            ""sunrise"": 1651898361,
      ""sunset"": 1651951947,
      ""moonrise"": 1651914420,
      ""moonset"": 1651886040,
      ""moon_phase"": 0.2,
      ""temp"": {
                ""min"": 285.18,
        ""max"": 286.23,
        ""day"": 285.76,
        ""night"": 285.51,
        ""eve"": 286.23,
        ""morn"": 285.18
      },
      ""feels_like"": {
                ""day"": 285.4,
        ""night"": 285.05,
        ""eve"": 285.76,
        ""morn"": 284.84
      },
      ""pressure"": 1024,
      ""humidity"": 89,
      ""dew_point"": 283.95,
      ""uvi"": 5.94,
      ""clouds"": 57,
      ""visibility"": 0,
      ""wind_speed"": 7.93,
      ""wind_deg"": 6,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""nuageux"",
          ""icon"": ""04d""
        }
      ],
      ""dt"": 1651924800
    },
    {
            ""sunrise"": 1651984670,
      ""sunset"": 1652038433,
      ""moonrise"": 1652004840,
      ""moonset"": 1651974420,
      ""moon_phase"": 0.23,
      ""temp"": {
                ""min"": 284.77,
        ""max"": 286.31,
        ""day"": 285.43,
        ""night"": 285.7,
        ""eve"": 286.31,
        ""morn"": 284.77
      },
      ""feels_like"": {
                ""day"": 284.93,
        ""night"": 285.28,
        ""eve"": 285.87,
        ""morn"": 284.34
      },
      ""pressure"": 1028,
      ""humidity"": 85,
      ""dew_point"": 282.94,
      ""uvi"": 6,
      ""clouds"": 80,
      ""visibility"": 0,
      ""wind_speed"": 8.32,
      ""wind_deg"": 47,
      ""weather"": [
        {
                ""main"": ""Rain"",
          ""description"": ""légère pluie"",
          ""icon"": ""10d""
        }
      ],
      ""dt"": 1652011200
    },
    {
            ""sunrise"": 1652070979,
      ""sunset"": 1652124919,
      ""moonrise"": 1652095440,
      ""moonset"": 1652062380,
      ""moon_phase"": 0.25,
      ""temp"": {
                ""min"": 285.07,
        ""max"": 287.66,
        ""day"": 286.03,
        ""night"": 286.91,
        ""eve"": 287.66,
        ""morn"": 285.14
      },
      ""feels_like"": {
                ""day"": 285.49,
        ""night"": 286.46,
        ""eve"": 287.26,
        ""morn"": 284.67
      },
      ""pressure"": 1028,
      ""humidity"": 81,
      ""dew_point"": 282.88,
      ""uvi"": 6,
      ""clouds"": 94,
      ""visibility"": 0,
      ""wind_speed"": 4.21,
      ""wind_deg"": 59,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""couvert"",
          ""icon"": ""04d""
        }
      ],
      ""dt"": 1652097600
    },
    {
            ""sunrise"": 1652157291,
      ""sunset"": 1652211404,
      ""moonrise"": 1652186160,
      ""moonset"": 1652150100,
      ""moon_phase"": 0.29,
      ""temp"": {
                ""min"": 285.87,
        ""max"": 287.87,
        ""day"": 287.4,
        ""night"": 287.24,
        ""eve"": 287.83,
        ""morn"": 285.87
      },
      ""feels_like"": {
                ""day"": 286.94,
        ""night"": 286.79,
        ""eve"": 287.42,
        ""morn"": 285.42
      },
      ""pressure"": 1023,
      ""humidity"": 79,
      ""dew_point"": 283.73,
      ""uvi"": 6,
      ""clouds"": 61,
      ""visibility"": 0,
      ""wind_speed"": 4.2,
      ""wind_deg"": 194,
      ""weather"": [
        {
                ""main"": ""Clouds"",
          ""description"": ""nuageux"",
          ""icon"": ""04d""
        }
      ],
      ""dt"": 1652184000
    }
  ],
  ""alerts"": [],
  ""units"": 0
}";
        var details = json.FromJson<WeatherDetails>();
        Assert.NotNull(details);
    }

    [Fact]
    public void ToJsonCorrectlySerializesTest()
    {
        var expected = new TestObject
        {
            Name = "System",
            Number = 7,
            Children = new[]
            {
                new TestObject
                {
                    Name = "Product",
                    Number = 8,
                    Children = new[]
                    {
                        new TestObject
                        {
                            Name = "Part",
                            Number = 9
                        },
                        new TestObject
                        {
                            Name = "Accessory",
                            Number = 10
                        }
                    }
                }
            }
        };

        var json = expected.ToJson();
        var actual = json.FromJson<TestObject>();

        Assert.NotStrictEqual(expected, actual);
    }
}

internal record TestObject
{
    public string? Name { get; init; }
    public int Number { get; init; }
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime Date { get; init; } = DateTime.Now;
    public TestObject[] Children { get; init; } = Array.Empty<TestObject>();
}
