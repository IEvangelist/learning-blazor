﻿// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

global using HaveIBeenPwned.Client;
global using HaveIBeenPwned.Client.Models;
global using HaveIBeenPwned.Client.Options;
global using Learning.Blazor.DistributedCache.Extensions;
global using Learning.Blazor.Extensions;
global using Learning.Blazor.Http.Extensions;
global using Learning.Blazor.PwnedApi;
global using Learning.Blazor.PwnedApi.Services;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Caching.Distributed;
global using Microsoft.Identity.Web;
global using Microsoft.Identity.Web.Resource;
