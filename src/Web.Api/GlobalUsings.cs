// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

global using System.Net.Mime;
global using Learning.Blazor.Abstractions.RealTime;
global using Learning.Blazor.Api;
global using Learning.Blazor.Api.Extensions;
global using Learning.Blazor.Api.Http;
global using Learning.Blazor.Api.Hubs;
global using Learning.Blazor.Api.Localization;
global using Learning.Blazor.Api.Options;
global using Learning.Blazor.Api.Resources;
global using Learning.Blazor.Api.Services;
global using Learning.Blazor.CosmosData;
global using Learning.Blazor.CosmosData.Extensions;
global using Learning.Blazor.CosmosData.Repository;
global using Learning.Blazor.DistributedCache.Extensions;
global using Learning.Blazor.Extensions;
global using Learning.Blazor.Http.Extensions;
global using Learning.Blazor.LogicAppServices;
global using Learning.Blazor.LogicAppServices.Extensions;
global using Learning.Blazor.LogicAppServices.Options;
global using Learning.Blazor.Models;
global using Learning.Blazor.Models.Requests;
global using Learning.Blazor.TwitterServices;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.ResponseCompression;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.Extensions.Caching.Distributed;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.Localization;
global using Microsoft.Extensions.Options;
global using Microsoft.Identity.Web;
global using Microsoft.Identity.Web.Resource;
global using Microsoft.OpenApi.Models;
