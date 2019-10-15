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
        private List<int> yaCogidoValor;
        private int numeroFilas = int.MinValue;
        private const string iconoOculto = "❓";
        private string[,] matrizSymbolosCogido;
        private string[] arraySymbolos;
        private Border border;
        private Viewbox viewbox;
        private TextBlock textBlock;
        private LinearGradientBrush myBrush = new LinearGradientBrush();
        private LinearGradientBrush myBrush_White = new LinearGradientBrush();

        public MainWindow()
        {
            InitializeComponent();
            arraySymbolos = new string[] { "🌟", "💎", "🍂", "🎥", "❤️", "📢", "✈️", "🦄", "🌻", "📷", "⚽️", "💸", "🌴", "🕷", "🎈", "🐢", "🌎", "💲", "🎁", "🗡" };
            yaCogidoValor = new List<int>();
            myBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            myBrush.GradientStops.Add(new GradientStop(Colors.Orange, 0.5));
            myBrush.GradientStops.Add(new GradientStop(Colors.Red, 1.0));
            myBrush_White.GradientStops.Add(new GradientStop(Colors.White, 0));

        }

        private void GuardaArrayAleatorio()
        {
            matrizSymbolosCogido = new string[numeroFilas, COLUMNAS];
            int tamañoArrayNum2x2 = (COLUMNAS * numeroFilas) / 2;
            int[] num2x2 = new int[(tamañoArrayNum2x2*2)];
            int index = 0;

            for (int i = 0; i < tamañoArrayNum2x2; i++)
            {
                num2x2[i] = NumeroAleatorioSinRepetir(0, tamañoArrayNum2x2);
            }
            for (int i = tamañoArrayNum2x2; i < tamañoArrayNum2x2*2; i++)
            {
                num2x2[i] = num2x2[index++];
            }
            index = 0;
            for (int i = 0; i < numeroFilas; i++)
            {
                for (int j = 0; j < COLUMNAS; j++)
                {
                    matrizSymbolosCogido[i, j] = arraySymbolos[num2x2[index]];
                }
            }
            
            //for (int i = 0; i < numeroFilas; i++)
            //    for (int j = 0; j < COLUMNAS; j++)
            //    {
            //        VisualizarCartaConIconoYPosicion(i, j, matrizSymbolosCogido[i,j]);
            //    }

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
            viewbox = new Viewbox();
            textBlock = new TextBlock();
            ((Border)sender).Background = myBrush_White;
            ((Border)sender).Child = viewbox;
            viewbox.Child = textBlock;

            textBlock.Text = arraySymbolos[0];

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

        private int NumeroAleatorioSinRepetir(int primerValor, int ultimoValor)
        {
            int aleatorio;
            do
            {
                Random random = new Random();
                aleatorio = random.Next(primerValor, ultimoValor + 1);
            } while (yaCogidoValor.Contains(aleatorio));
            yaCogidoValor.Add(aleatorio);
            return aleatorio;
        }

    }

}
