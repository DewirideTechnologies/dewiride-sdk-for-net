# Dewiride.Azure.Bot.Framework.Cards.Helper

[![NuGet Version](https://img.shields.io/nuget/v/Dewiride.Azure.Bot.Framework.Cards.Helper)](https://www.nuget.org/packages/Dewiride.Azure.Bot.Framework.Cards.Helper)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Dewiride.Azure.Bot.Framework.Cards.Helper)](https://www.nuget.org/packages/Dewiride.Azure.Bot.Framework.Cards.Helper)

## Overview

`Dewiride.Azure.Bot.Framework.Cards.Helper` is a .NET library that provides helper methods for creating and sending Adaptive Cards in Microsoft Bot Framework bots. This package simplifies the process of generating Adaptive Card attachments from JSON templates and sending them to users.

## Installation

You can install the package via NuGet Package Manager:

```bash
dotnet add package Dewiride.Azure.Bot.Framework.Cards.Helper
```

Or via the NuGet Package Manager Console:

```bash
PM> Install-Package Dewiride.Azure.Bot.Framework.Cards.Helper
```

## Usage

### Creating an Adaptive Card from a Template

You can create an adaptive card attachment from a JSON template file using the `CreateAdaptiveCard` method:

```csharp
using Dewiride.Azure.Bot.Framework.Cards.Helper;
using Microsoft.Bot.Schema;

string[] templatePath = { ".", "Cards", "YourCardTemplate.json" };
Attachment cardAttachment = CardsHelper.CreateAdaptiveCard(templatePath);
```

### Creating an Adaptive Card with Data

You can merge data into an adaptive card template using the `CreateAdaptiveCardWithData` method:

```csharp
using Dewiride.Azure.Bot.Framework.Cards.Helper;
using Microsoft.Bot.Schema;

string[] templatePath = { ".", "Cards", "YourCardTemplate.json" };
var data = new { Name = "John Doe", Age = 30 };
Attachment cardAttachment = CardsHelper.CreateAdaptiveCardWithData(templatePath, data);
```

### Sending a Welcome Card

You can send a welcome card to the user using the `SendWelcomeCardAsync` method in a dialog step:

```csharp
using Dewiride.Azure.Bot.Framework.Cards.Helper;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;

public class MyBotDialog : ComponentDialog
{
    private readonly ICardsHelper _cardsHelper;

    public MyBotDialog(ICardsHelper cardsHelper)
    {
        _cardsHelper = cardsHelper;
    }

    private async Task<DialogTurnResult> WelcomeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        var data = new { UserName = "John Doe" };
        await _cardsHelper.SendWelcomeCardAsync(data, stepContext, cancellationToken);
        return await stepContext.NextAsync(null, cancellationToken);
    }
}
```


### Sending Suggested Actions

You can send suggested actions to the user using the `SendSuggestedActionsAsync` method:

```csharp
public MyBotDialog(ICardsHelper cardsHelper)
{
    _cardsHelper = cardsHelper;
}

private async Task<DialogTurnResult> SuggestedActionsStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
{
    var cardActions = new List<CardAction>
    {
        new CardAction { Title = "Option 1", Type = ActionTypes.ImBack, Value = "Option 1" },
        new CardAction { Title = "Option 2", Type = ActionTypes.ImBack, Value = "Option 2" }
    };

    await _cardsHelper.SendSuggestedActionsAsync(stepContext, cancellationToken, "Please choose an option:", cardActions);
    return await stepContext.NextAsync(null, cancellationToken);
}
```


### Dependency Injection

To use the `CardsHelper` with Dependency Injection (DI) in your bot project, register it in the `Startup.cs` or your DI setup:

```csharp
using Dewiride.Azure.Bot.Framework.Cards.Helper;
using Microsoft.Extensions.DependencyInjection;

public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<ICardsHelper, CardsHelper>();
    // Other service registrations
}
```

## Company

- Dewiride Technologies Private Limited

## Repository

- [GitHub Repository](https://github.com/DewirideTechnologies/dewiride-sdk-for-net)