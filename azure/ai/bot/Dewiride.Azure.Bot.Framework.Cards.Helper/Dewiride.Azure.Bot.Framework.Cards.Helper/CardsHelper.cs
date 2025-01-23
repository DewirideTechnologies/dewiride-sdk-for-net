using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.Text;

namespace Dewiride.Azure.Bot.Framework.Cards.Helper
{
    public class CardsHelper : ICardsHelper
    {
        private readonly string[] welcomeCardPath = [".", "Cards", "WelcomeCard.json"];

        /// <summary>
        /// Creates an adaptive card attachment using a template and data.
        /// </summary>
        /// <param name="path">The path to the adaptive card template file.</param>
        /// <param name="dataJson">The data to merge into the template.</param>
        /// <returns>An attachment that can be sent in a message.</returns>
        public static Attachment CreateAdaptiveCardWithData(string[] path, object dataJson)
        {
            string templateJson = System.IO.File.ReadAllText(Path.Combine(path), Encoding.UTF8);
            var template = new AdaptiveCards.Templating.AdaptiveCardTemplate(templateJson);
            var card = template.Expand(dataJson);

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(card),
            };

            return adaptiveCardAttachment;
        }

        /// <summary>
        /// Creates an adaptive card attachment from a template.
        /// </summary>
        /// <param name="path">The path to the adaptive card template file.</param>
        /// <returns>An attachment that can be sent in a message.</returns>
        public static Attachment CreateAdaptiveCard(string[] path)
        {
            string templateJson = System.IO.File.ReadAllText(Path.Combine(path), Encoding.UTF8);

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(templateJson),
            };

            return adaptiveCardAttachment;
        }

        /// <summary>
        /// Sends a welcome card to the user.
        /// </summary>
        /// <param name="dataJson">The data to merge into the welcome card template.</param>
        /// <param name="stepContext">The context for the current step of the conversation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <param name="paths">Optional. The paths to the welcome card template. If not provided, the default path is used which is { ".", "Cards", "WelcomeCard.json" }.</param>
        /// <returns>A task that represents the asynchronous send operation.</returns>
        public async Task SendWelcomeCardAsync(object? dataJson, WaterfallStepContext stepContext, CancellationToken cancellationToken, string[]? paths = null)
        {
            if (paths == null)
                paths = welcomeCardPath;

            var attachment = CreateAdaptiveCardWithData(paths, dataJson);
            var reply = MessageFactory.Attachment(attachment);
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);
        }

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
        public async Task SendSuggestedActionsAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken, string replyText, List<CardAction> cardActions)
        {
            var reply = MessageFactory.Text(replyText);

            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = cardActions
            };
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);
        }
    }
}
