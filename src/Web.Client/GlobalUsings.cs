﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

global using System.Collections.Concurrent;
global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics;
global using System.Globalization;
global using System.Net.Http.Json;
global using System.Security.Claims;
global using System.Text.Json.Serialization;
global using HaveIBeenPwned.Client.Models;
global using Learning.Blazor;
global using Learning.Blazor.Abstractions.RealTime;
global using Learning.Blazor.BrowserModels;
global using Learning.Blazor.ComponentModels;
global using Learning.Blazor.Components;
global using Learning.Blazor.DataAnnotations;
global using Learning.Blazor.Extensions;
global using Learning.Blazor.Handlers;
global using Learning.Blazor.Localization;
global using Learning.Blazor.LocalStorage;
global using Learning.Blazor.Models;
global using Learning.Blazor.Options;
global using Learning.Blazor.Serialization;
global using Learning.Blazor.Services;
global using Learning.Blazor.Shared;
global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Components.Authorization;
global using Microsoft.AspNetCore.Components.Forms;
global using Microsoft.AspNetCore.Components.Routing;
global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
global using Microsoft.AspNetCore.SignalR.Client;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.Extensions.Localization;
global using Microsoft.Extensions.Options;
global using Microsoft.JSInterop;
global using Polly;
global using Polly.Retry;
