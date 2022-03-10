# Learning Blazor: Build Single-Page Apps with WebAssembly and C#

| Book | Details |
|--|--|
| <a href="https://bit.ly/learning-blazor" target="_blank"><img title="Learning Blazor: Build Single-Page Apps with WebAssembly and C#" alt="Learning Blazor: Build Single-Page Apps with WebAssembly and C#" src="images/learning-blazor.png"></a> | This repository is the application detailed in the ["Learning Blazor: Build Single-Page Apps with WebAssembly and C#"][learning-blazor] O'Reilly Media book by David Pine. Take advantage of your C# skills to build UI components and client-side experiences with .NET. With this practical guide, you'll learn how to use Blazor WebAssembly to develop next-generation web experiences. Built on top of ASP.NET Core, Blazor represents the future of .NET single-page applications (SPA) investments. |

| Status | Description |
|--:|:--|
| [![build](https://github.com/IEvangelist/learning-blazor/actions/workflows/build-validation.yml/badge.svg)](https://github.com/IEvangelist/learning-blazor/actions/workflows/build-validation.yml) | Wether the current source code builds successfully, and all tests pass. |
| [![CodeQL](https://github.com/IEvangelist/learning-blazor/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/IEvangelist/learning-blazor/actions/workflows/codeql-analysis.yml) | The current CodeQL security / vulnerability scan result. |
| [![Azure Translation](https://github.com/IEvangelist/learning-blazor/actions/workflows/machine-translation.yml/badge.svg)](https://github.com/IEvangelist/learning-blazor/actions/workflows/machine-translation.yml) | Wether the last machine-translation run was successful. |
| [![Deploy Static Web App](https://github.com/IEvangelist/learning-blazor/actions/workflows/deploy-az-static-webapp.yml/badge.svg)](https://github.com/IEvangelist/learning-blazor/actions/workflows/deploy-az-static-webapp.yml) | The status of the last deployment of the Azure Static Web app. |
| [![Deploy Weather Function](https://github.com/IEvangelist/learning-blazor/actions/workflows/deploy-az-func.yml/badge.svg)](https://github.com/IEvangelist/learning-blazor/actions/workflows/deploy-az-func.yml) | The status of the last deployment of the Azure Functions app. |
| [![Deploy Web API](https://github.com/IEvangelist/learning-blazor/actions/workflows/deploy-az-webapi.yml/badge.svg)](https://github.com/IEvangelist/learning-blazor/actions/workflows/deploy-az-webapi.yml) | The status of the last deployment of the Azure Web API. |
| [![Deploy Pwned Web API](https://github.com/IEvangelist/learning-blazor/actions/workflows/deploy-az-pwnedapi.yml/badge.svg)](https://github.com/IEvangelist/learning-blazor/actions/workflows/deploy-az-pwnedapi.yml) | The status of the last deployment of the Azure Pwned Web API. |

## Architecture

_**The following is an ASP.NET Core hosting diagram:**_

```mermaid
graph LR;
    A("fa:fa-globe-americas Internet")==HTTP===B("fa:fa-arrows-alt-h Reverse Proxy (IIS, Nginx, Apache)");
    B==HTTP===C("fa:fa-server Kestrel");
    C==HttpContext===D("fa:fa-code Application Code");
    classDef d stroke-dasharray:5,5;
    classDef b stroke:#000,stroke-width:2px;
    classDef blue fill:#33a1ff;
    classDef orange fill:#f37f1c;
    classDef cyan fill:#800080;
    class A b
    class A d
    class B cyan
    class B b
    class C orange
    class C b
    class D blue
    class D b
```

> Powered by Mermaid.js
> &mdash; [🔗 Include diagrams in your Markdown files with Mermaid](https://github.blog/2022-02-14-include-diagrams-markdown-files-mermaid/)

## Home screen (dark theme)

![Learning Blazor: Home screen (dark theme)](images/home-screen-dark.png)

## Home screen (light theme)

![Learning Blazor: Home screen (light theme)](images/home-screen-light.png)

Featuring:

- C# 10
- .NET 6
- Blazor WebAssembly
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
- Two-way JavaScript Interop (using both `IJSRuntime` and `IJsInProcessRuntime`)
  - As well as Blazorators, for source generation

> [💡 Ideas](https://gist.github.com/IEvangelist/d43abafb64d207bff25e60769e986bbd) for the application to include.

[learning-blazor]: https://bit.ly/learning-blazor
