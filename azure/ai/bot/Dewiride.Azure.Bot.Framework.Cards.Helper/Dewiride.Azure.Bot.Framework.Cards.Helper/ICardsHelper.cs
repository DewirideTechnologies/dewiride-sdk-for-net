using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

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

        /// <summary>
        /// Creates and sends an activity with suggested actions to the user. When the user
        /// clicks one of the buttons the text value from the "CardAction" will be
        /// displayed in the channel just as if the user entered the text. There are multiple
        /// "ActionTypes" that may be used for different situations.
        /// </summary>
        /// <param name="stepContext">The context for the current step of the conversation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <param name="replyText">The text to include in the reply message.</param>
        /// <param name="cardActions">A list of card actions to include in the suggested actions.</param>
        /// <returns>A task that represents the asynchronous send operation.</returns>
        Task SendSuggestedActionsAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken, string replyText, List<CardAction> cardActions);
    }
}