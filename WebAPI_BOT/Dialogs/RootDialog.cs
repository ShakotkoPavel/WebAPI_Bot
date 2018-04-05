using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace WebAPI_BOT.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            HttpClient httpClient = new HttpClient();

            var activity = await result as Activity;
            //if (!string.IsNullOrEmpty(activity.Text))
            //{
                var message = context.MakeMessage();
                StringBuilder stringBuilder = new StringBuilder();

                var cardContentList = new Dictionary<string, string>()
                {
                    {
                        "Get all categoties",
                        "http://localhost:55117/api/Product/GetAllCategories"
                    }///,
                    //{
                    //    "Get all products by category Id",
                    //    "http://localhost:55117/api/Product/GetAllProductsByCategoryId/1"
                    //}
                };

                List<CardAction> cardActions = new List<CardAction>();

                foreach (var item in cardContentList)
                {
                    List<string> cardUrls = new List<string> { };
                    CardAction btn = new CardAction()
                    {
                        Value = $"{item.Key}",
                        Type = ActionTypes.PostBack,
                        Title = item.Key
                    };
                    cardActions.Add(btn);
                }

                HeroCard heroCard = new HeroCard()
                {
                    Title = "Make a choice!",
                    Buttons = cardActions
                };
                Attachment attachment = heroCard.ToAttachment();
                message.Attachments.Add(attachment);

            //}

            //if (!string.IsNullOrEmpty(activity.Text))
            //{
            //    HttpResponseMessage response = await httpClient.GetAsync("http://localhost:55117/api/Product/GetAllCategories");
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var json = await response.Content.ReadAsStringAsync();
            //        var arr = JsonConvert.DeserializeObject<List<string>>(json);
            //        foreach (var item in arr)
            //        {
            //            stringBuilder.Append(item);
            //        }
            //    }
            //}

            // return our reply to the user
            //await context.PostAsync($"{stringBuilder.ToString()}");

            await context.PostAsync(message);
            context.Wait(MessageReceivedAsync);
        }
    }
}
