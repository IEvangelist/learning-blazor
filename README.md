# Learning Blazor: Build Single-Page Apps with WebAssembly and C#

| Book | Details |
|--|--|
| <a href="https://bit.ly/learning-blazor" target="_blank"><img title="Learning Blazor: Build Single-Page Apps with WebAssembly and C#" alt="Learning Blazor: Build Single-Page Apps with WebAssembly and C#" src="images/learning-blazor.png"></a> | This repository is the application detailed in the ["Learning Blazor: Build Single-Page Apps with WebAssembly and C#"][learning-blazor] O'Reilly Media book by David Pine. Take advantage of your C# skills to build UI components and client-side experiences with .NET. With this practical guide, you'll learn how to use Blazor WebAssembly to develop next-generation web experiences. Built on top of ASP.NET Core, Blazor represents the future of .NET single-page applications (SPA) investments. |

## This app is deployed to: https://webassemblyof.net

The app is a Blazor WebAssembly app, deployed to Azure as a Static Web app. It targets .NET 6, and it's packed full of C# 10 features.

| Status | Description |
|--:|:--|
| [![build](https://github.com/IEvangelist/learning-blazor/actions/workflows/build-validation.yml/badge.svg)](https://github.com/IEvangelist/learning-blazor/actions/workflows/build-validation.yml) | Wether the current source code builds successfully, and all tests pass. |
| [![CodeQL](https://github.com/IEvangelist/learning-blazor/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/IEvangelist/learning-blazor/actions/workflows/codeql-analysis.yml) | The current CodeQL security / vulnerability scan result. |
| [![Azure Translation](https://github.com/IEvangelist/learning-blazor/actions/workflows/machine-translation.yml/badge.svg)](https://github.com/IEvangelist/learning-blazor/actions/workflows/machine-translation.yml) | Wether the last machine-translation run was successful. |
| [![Deploy Static Web App](https://github.com/IEvangelist/learning-blazor/actions/workflows/deploy-az-static-webapp.yml/badge.svg)](https://github.com/IEvangelist/learning-blazor/actions/workflows/deploy-az-static-webapp.yml) | The status of the last deployment of the Azure Static Web app. |
| [![Deploy Weather Function](https://github.com/IEvangelist/learning-blazor/actions/workflows/deploy-az-func.yml/badge.svg)](https://github.com/IEvangelist/learning-blazor/actions/workflows/deploy-az-func.yml) | The status of the last deployment of the Azure Functions app. |
| [![Deploy Web API](https://github.com/IEvangelist/learning-blazor/actions/workflows/deploy-az-webapi.yml/badge.svg)](https://github.com/IEvangelist/learning-blazor/actions/workflows/deploy-az-webapi.yml) | The status of the last deployment of the Azure Web API. |
| [![Deploy Pwned Web API](https://github.com/IEvangelist/learning-blazor/actions/workflows/deploy-az-pwnedapi.yml/badge.svg)](https://github.com/IEvangelist/learning-blazor/actions/workflows/deploy-az-pwnedapi.yml) | The status of the last deployment of the Azure Pwned Web API. |

## Home screen (dark theme)

![Learning Blazor: Home screen (dark theme)](images/home-screen-dark.png)

## Home screen (light theme)

![Learning Blazor: Home screen (light theme)](images/home-screen-light.png)

## Featuring

The app is packed with examples of how to do various things with Blazor, including but not limited to:

- Blazor Third-Party Authentication providers:
  - Google
  - Twitter
  - GitHub
  - "Sign up now"-based identity provider registration (with email verification).
- Azure Functions &mdash; .NET
- ASP.NET Core Web API
- ASP.NET Core SignalR
- Bulma (CSS)
- Polly
- Swagger / OpenAPI
- Twitter API
- OpenWeatherMap API
- "Have I Been Pwned"
- Client-Browser Native Speech Synthesis and Speech Recognition
- Reactive Extensions (Rx.NET)
- Azure Cosmos DB Repository-Pattern .NET SDK
- Blazor WebAssembly Localization
- Azure Cognitive Services Translator
- Blazor Component Virtualization
- Humanizer
- Two-way JavaScript Interop (using both `IJSRuntime` and `IJsInProcessRuntime`)
  - As well as Blazorators, for source generation

## NuGet Packages

I'm using several of my open-source projects within this repository.

| Package | Repository | Purpose |
|---------|------------|---------|
| [IEvangelist.Azure.CosmosRepository](https://www.nuget.org/packages/IEvangelist.Azure.CosmosRepository) | [`./azure-cosmos-dotnet-repository`](https://github.com/IEvangelist/azure-cosmos-dotnet-repository) | `IRepository<TItem>` via DI for light-weight access to Azure Cosmos DB. |
| [Blazor.LocalStorage.WebAssembly](https://www.nuget.org/packages/Blazor.LocalStorage.WebAssembly) | [`./blazorators`](https://github.com/IEvangelist/blazorators) | Source-generated `localStorage` API implementation class library from Blazorators: C# Source Generators for Blazor. |
| [Blazor.SpeechRecognition.WebAssembly](https://www.nuget.org/packages/Blazor.SpeechRecognition.WebAssembly) | [`./blazorators`](https://github.com/IEvangelist/blazorators) | Hand-written custom library that wraps browser native `speechRecognition` API implementation. |
| [HaveIBeenPwned.Client](https://www.nuget.org/packages/HaveIBeenPwned.Client) | [`./pwned-client`](https://github.com/IEvangelist/pwned-client) | A .NET HTTP client for the "have i been pwned" API service from Troy Hunt.  |

## GitHub Actions

I'm also using my Resource Translator: https://github.com/IEvangelist/resource-translator, which translates _.resx_ resource files.

> [💡 Ideas](https://gist.github.com/IEvangelist/d43abafb64d207bff25e60769e986bbd) for the application to include.

[learning-blazor]: https://bit.ly/learning-blazor
