using Microsoft.Bot.Builder.Dialogs;

namespace Dewiride.Azure.Bot.Framework.Cards.Helper
{
    public interface ICardsHelper
    {
        /// <summary>
        /// Sends a welcome card to the user.
        /// </summary>
        /// <param name="dataJson">The data to merge into the welcome card template.</param>
        /// <param name="stepContext">The context for the current step of the conversation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <param name="paths">Optional. The paths to the welcome card template. If not provided, the default path is used which is { ".", "Cards", "WelcomeCard.json" }.</param>
        /// <returns>A task that represents the asynchronous send operation.</returns>
        Task SendWelcomeCardAsync(object? dataJson, WaterfallStepContext stepContext, CancellationToken cancellationToken, string[]? paths = null);
    }
}