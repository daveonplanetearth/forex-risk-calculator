using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using SimpleEchoBot;
using System.Text;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        protected int count = 1;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            try
            {
                string[] input = message.Text.Split(' ');
                double pips = double.Parse(input[2]);
                RiskCalculator calc = new RiskCalculator();
                string accountCurrency = input[0].ToUpper();
                string pair = input[1].ToUpper();

                double risk = calc.CalcRisk(accountCurrency, pair, pips, double.Parse(input[3]));

                //string rates = calc.ReadExchangeRatesFromAzure();
                await context.PostAsync($"Risk for {pair} {pips}pips is: {risk.ToString("F2")}{accountCurrency}");

                context.Wait(MessageReceivedAsync);
            }
            catch(Exception exc)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine($"Hi, welcome. This is a Forex currency risk calculator. It's free to use and will help you calculate your total risk for a trade.");
                builder.AppendLine($"For example, if your trading account currency is in GBP, you want to trade EURUSD, 100 pip stop loss and 0.05 lot size enter: GBP EURUSD 100 5000");
                builder.AppendLine($"GBP, EUR, USD and CHF trading account currencies are supported. Rates are updated once per day at 23.00 GMT.");
                builder.AppendLine($"Thanks for using this and have a great trading day!");

                await context.PostAsync(builder.ToString());
                context.Wait(MessageReceivedAsync);

                // await context.PostAsync($"Oops! Try again or type ? for help.");

                // context.Wait(MessageReceivedAsync);
            }
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                this.count = 1;
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
            context.Wait(MessageReceivedAsync);
        }

    }
}