using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int COLUMNAS = 4;
        private List<string> yaCogidoValor;
        private int numeroFilas = int.MinValue;
        private const string iconoOculto = "❓";
        private List<string> symbolosAleatorios;
        private string[] arraySymbolos;
        private Random random;
        private Border border;
        private Viewbox viewbox;
        private TextBlock textBlock;
        private TextBlock textBlockAnterior;
        private LinearGradientBrush myBrush = new LinearGradientBrush();
        private LinearGradientBrush myBrush_White = new LinearGradientBrush();

        public MainWindow()
        {
            InitializeComponent();
            arraySymbolos = new string[] { "🌟", "💎", "🍂", "🎥", "❤️", "📢", "✈️", "🦄", "🌻", "📷", "⚽️", "💸", "🌴", "🕷", "🎈", "🐢", "🌎", "💲", "🎁", "🗡" };
            yaCogidoValor = new List<string>();
            symbolosAleatorios = new List<string>();
            random = new Random();
            myBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            myBrush.GradientStops.Add(new GradientStop(Colors.Orange, 0.5));
            myBrush.GradientStops.Add(new GradientStop(Colors.Red, 1.0));
            myBrush_White.GradientStops.Add(new GradientStop(Colors.White, 0));

        }

        private void GuardaArrayAleatorio()
        {
            for (int i = 0; i < COLUMNAS*numeroFilas/2; i++)
            {
                string symbolo = DevuelveSymboloAleatorio();
                symbolosAleatorios.Add(symbolo);
                symbolosAleatorios.Add(symbolo);
            }
        }

        private void CreaCartasParaOcultar()
        {
            for (int j = 0; j < COLUMNAS; j++)
                gridPrincipal.ColumnDefinitions.Add(new ColumnDefinition());
            
            for (int i = 0; i < numeroFilas; i++)
                gridPrincipal.RowDefinitions.Add(new RowDefinition());


            for (int i = 0; i < numeroFilas; i++)
                for (int j = 0; j < COLUMNAS; j++)
                {
                    VisualizarCartaConIconoYPosicion(i, j, iconoOculto, myBrush);
                }
        }

        private void VisualizarCartaConIconoYPosicion(int i, int j, string icono, LinearGradientBrush background)
        {
            border = new Border();
            viewbox = new Viewbox();
            textBlock = new TextBlock();
            border.BorderThickness = new Thickness(1);
            border.BorderBrush = Brushes.Black;
            border.Background = background;
            border.Margin = new Thickness(2);
            border.CornerRadius = new CornerRadius(5);

            border.MouseLeftButtonDown += Border_MouseLeftButtonDown;
            textBlock.Text = icono;

            gridPrincipal.Children.Add(border);
            border.Child = viewbox;
            viewbox.Child = textBlock;
            Grid.SetRow(border, i);
            Grid.SetColumn(border, j);
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            border = ((Border)sender);
            viewbox = (Viewbox)border.Child;

            if(textBlockAnterior != null)
            {
                if (textBlockAnterior.Text != ((TextBlock)viewbox.Child).Text)
                {
                    viewbox = new Viewbox();
                    textBlock = new TextBlock();
                    ((Border)sender).Background = myBrush;
                    ((Border)sender).Child = viewbox;

                    textBlock.Text = iconoOculto;
                }
            }
            

            textBlockAnterior = (TextBlock)viewbox.Child;




            if (((Border)sender).Background != myBrush_White)
            {
                viewbox = new Viewbox();
                textBlock = new TextBlock();
                ((Border)sender).Background = myBrush_White;
                ((Border)sender).Child = viewbox;

                if (((Border)sender).Tag == null)
                    ((Border)sender).Tag = symbolosAleatorios.Count;

                viewbox.Child = textBlock;

                if (symbolosAleatorios.Count != 0)
                {
                    int aleatorio = random.Next(0, symbolosAleatorios.Count);
                    textBlock.Text = symbolosAleatorios.ElementAt(aleatorio);

                    symbolosAleatorios.RemoveAt(aleatorio);
                }
            }



        }

        private void RadioButton_Checked_Baja(object sender, RoutedEventArgs e)
        {
            numeroFilas = 3;
        }
        private void RadioButton_Checked_Media(object sender, RoutedEventArgs e)
        {
            numeroFilas = 4;
        }
        private void RadioButton_Checked_Alta(object sender, RoutedEventArgs e)
        {
            numeroFilas = 5;
        }

        private void Button_Click_Iniciar(object sender, RoutedEventArgs e)
        {
            if (numeroFilas > 0)
            {
                gridPrincipal.ColumnDefinitions.Clear();
                gridPrincipal.RowDefinitions.Clear();
                yaCogidoValor.Clear();
                symbolosAleatorios.Clear();
                GuardaArrayAleatorio();

                CreaCartasParaOcultar();
            }
            
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                pbStatus.Value++;
                Thread.Sleep(100);
            }
        }

        private string DevuelveSymboloAleatorio()
        {
            string symbolo = "";
            do
            {
                int aleatorio = random.Next(0, 20);
                symbolo = arraySymbolos[aleatorio];

            } while (yaCogidoValor.Contains(symbolo));
            yaCogidoValor.Add(symbolo);
            
            return symbolo;
        }

    }

}
