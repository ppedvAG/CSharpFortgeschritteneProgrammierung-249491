using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace TPL_AsyncAwait.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string RequestUri = "http://www.gutenberg.org/files/54700/54700-0.txt";
        private CancellationTokenSource _cancellationTokenSource = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Clear(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Output.Text = string.Empty;
        }

        private void UpdateStatus(object? text)
        {
            Output.Text = text + Environment.NewLine + Output.Text;
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(25);
                UpdateStatus(i);
            }
        }

        private void StartTaskRun(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(25);

                    // UI Updates duerfen nicht von Side Tasks ausgefuehrt werden
                    // Dispatcher ermoeglicht uns auf dem Thread der Komponente beliebigen Code auszufuehren
                    Dispatcher.Invoke(() => UpdateStatus(i));
                }
            });
        }

        private async void StartAsyncAwait(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.IsEnabled = false;

                try
                {
                    for (int i = 0; i < 100; i++)
                    {
                        await Task.Delay(25, _cancellationTokenSource.Token);
                        UpdateStatus(i);
                    }
                }
                catch(TaskCanceledException)
                {
                    var success = _cancellationTokenSource.TryReset();
                    UpdateStatus($"Task canceled by user {Environment.NewLine}Reset: {success}");

                    if (!success)
                    {
                        _cancellationTokenSource.Dispose();
                        _cancellationTokenSource = new CancellationTokenSource();
                    }
                }
                finally
                {
                    btn.IsEnabled = true;
                }
            }
        }

        private void CancelTask(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private async void StartRequest(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                using HttpClient client = new();

                var request = client.GetAsync(RequestUri);
                Output.Text += ("Request started" + Environment.NewLine);

                btn.IsEnabled = false;

                var response = await request;

                // Alternative wenn wir das async Keyword und damit await nicht benutzen koennen
                // Macht Aufruf synchron und UI Thread blockiert
                //var response = request.ConfigureAwait(false).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    Output.Text += "Read response" + Environment.NewLine;
                    var content = await response.Content.ReadAsStringAsync();
                    Output.Text += content;
                } 
                else
                {
                    UpdateStatus(response.ReasonPhrase);
                }

                btn.IsEnabled = true;
            }
        }

        #region Legacy Code

        /// <summary>
        /// Simuliert eine Legacy Methode welche sehr langsam ist und wir nicht anfassen duerfen
        /// weil keine Tests und koennte beim Refactoring kaputt gehen.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public decimal CalcValuesVerySlow(int input)
        {
            Thread.Sleep(5000);

            return input * 1 / 42m; // m markiert diese Zahl als decimal, f fuer float usw.
        }

        // Schritt 1: Async Variante machen
        public Task<decimal> CalcValuesVerySlowAsync(int input) 
            => Task.Factory.StartNew((arg) => CalcValuesVerySlow((int)arg), input, _cancellationTokenSource.Token);

        private async void StartLegacyCode(object sender, RoutedEventArgs e)
        {
            try
            {
                var value = 37;
                Output.Text += "Anfrage gestartet fuer " + value;

                var result = await CalcValuesVerySlowAsync(value);
                Output.Text += Environment.NewLine + "Ergebnis: " + result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hilfe, Fehler!");
            }
        } 
        #endregion
    }
}