using System.Globalization;

namespace MauiAppMeuCombustivel
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Normalize decimal separator to '.' so parsing with InvariantCulture works
                var etanolText = (txt_etanol.Text ?? string.Empty).Replace(',', '.');
                var gasolinaText = (txt_gasolina.Text ?? string.Empty).Replace(',', '.');

                if (!double.TryParse(etanolText, NumberStyles.Any, CultureInfo.InvariantCulture, out double etanol)
                    || !double.TryParse(gasolinaText, NumberStyles.Any, CultureInfo.InvariantCulture, out double gasolina))
                {
                    await DisplayAlertAsync("Entrada inválida", "Por favor insira números válidos para os preços.", "OK");
                    return;
                }

                string msg = etanol <= (gasolina * 0.7)
                    ? "É mais vantajoso abastecer com Etanol."
                    : "É mais vantajoso abastecer com Gasolina.";

                await DisplayAlertAsync("Calculado", msg, "OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro em {nameof(Button_Clicked)}: {ex}");
                await DisplayAlertAsync("Erro", "Ocorreu um erro ao processar sua solicitação. Por favor, tente novamente mais tarde.", "OK");

            }
        }//fecha método Button_Clicked

        private void NumericEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not Entry entry)
                return;

            var newText = e.NewTextValue ?? string.Empty;
            var filtered = FilterNumeric(newText);

            if (filtered != newText)
            {
                // try to preserve cursor position as best as possible
                var oldPos = entry.CursorPosition;
                entry.Text = filtered;
                entry.CursorPosition = Math.Min(filtered.Length, Math.Max(0, oldPos - (newText.Length - filtered.Length)));
            }
        }

        private static string FilterNumeric(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var sb = new System.Text.StringBuilder(input.Length);
            bool foundSeparator = false;

            foreach (var c in input)
            {
                if (char.IsDigit(c))
                {
                    sb.Append(c);
                }
                else if ((c == '.' || c == ',') && !foundSeparator)
                {
                    sb.Append(c);
                    foundSeparator = true;
                }
                // ignore any other characters
            }

            return sb.ToString();
        }
    }// fecha classe MainPage
}// fecha namespace MauiAppMeuCombustivel
