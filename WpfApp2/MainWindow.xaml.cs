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
using System.Windows.Threading;

namespace WpfApp2
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int COLUMNAS =4;
        private int numeroFilas = int.MinValue;
        private const string iconoOculto = "❓";
        private string[] arraySymbolos;
        private int tag;
        private int columnasXFilas;
        private int sumaCartas;
        private bool juegoIniciado;
        private List<string> yaCogidoValor;
        private List<string> symbolosAleatoriosFijo;
        private Random random;
        private Border border;
        private Viewbox viewbox;
        private TextBlock textBlock;
        private TextBlock textBlockAnterior1;
        private TextBlock textBlockAnterior2;
        private LinearGradientBrush myBrush;
        private LinearGradientBrush myBrush_White;

        public MainWindow()
        {
            InitializeComponent();
        }
        //Inicializo Componentes.
        public void ComponentesParaIniciar()
        {
            juegoIniciado = true;
            pbStatus.Value = 0;
            tag = 0;
            sumaCartas = 0;
            arraySymbolos = new string[] { "🌟", "💎", "🍂", "🎥", "❤️", "📢", "✈️", "🌻", "📷", "⚽️", "💸", "🌴", "🎈", "🐢", "🌎", "💲", "🎁" };
            yaCogidoValor = new List<string>();
            symbolosAleatoriosFijo = new List<string>();
            random = new Random();
            myBrush = new LinearGradientBrush();
            myBrush_White = new LinearGradientBrush();
            myBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            myBrush.GradientStops.Add(new GradientStop(Colors.Orange, 0.5));
            myBrush.GradientStops.Add(new GradientStop(Colors.Red, 1.0));
            myBrush_White.GradientStops.Add(new GradientStop(Colors.White, 0));
        }
        //Sacndo Symbolos Aleatorio y guardando en una lissta.
        private void GuardaArrayAleatorio()
        {
            string[] symbolosAleatorios = new string[columnasXFilas];
            int aleatorio = 0;

            for (int i = 0; i < columnasXFilas; i++)
            {
                string symbolo = DevuelveSymboloAleatorio();
                symbolosAleatorios[i] = symbolo;
                i++;
                symbolosAleatorios[i] = symbolo;

            }
            do
            {
                aleatorio = random.Next(0, columnasXFilas);

                if (symbolosAleatorios[aleatorio] != "")
                {
                    symbolosAleatoriosFijo.Add(symbolosAleatorios[aleatorio]);
                    symbolosAleatorios[aleatorio] = "";
                }
                

            } while (symbolosAleatoriosFijo.Count != columnasXFilas) ;
            
        }
        //Creando Cartas Para cuando se inicie se muestre las cartas para adivinar.
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
        //Visualiza cartas para el metodo "CreaCartasParaOcultar()".
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
            border.Tag = tag++;

            border.MouseLeftButtonDown += Border_MouseLeftButtonDown;
            border.MouseLeftButtonUp += Border_MouseLeftButtonUp;
            textBlock.Text = icono;

            gridPrincipal.Children.Add(border);
            border.Child = viewbox;
            viewbox.Child = textBlock;
            Grid.SetRow(border, i);
            Grid.SetColumn(border, j);
        }
        //Comparo los symbolos si son iguales o no, si son iguales se quedan abiertos y la barra se avanza lo que se debe avanzar.
        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (textBlockAnterior1 != null && textBlockAnterior2 != null)
            {
                Thread.Sleep(500);
                if (textBlockAnterior1.Text != textBlockAnterior2.Text)
                {
                    viewbox = (Viewbox)textBlockAnterior1.Parent;
                    border = (Border)viewbox.Parent;
                    border.Background = myBrush;
                    textBlockAnterior1.Text = iconoOculto;

                    viewbox = (Viewbox)textBlockAnterior2.Parent;
                    border = (Border)viewbox.Parent;
                    border.Background = myBrush;
                    textBlockAnterior2.Text = iconoOculto;
                }
                else
                {
                    sumaCartas +=2;
                    float a = sumaCartas * 100 / columnasXFilas;
                    int value = (int)Math.Round(a, 0);
                    pbStatus.Value = value;
                    if (sumaCartas == columnasXFilas)
                    {
                        MessageBox.Show("¡Enhorabuena! Partida Finalizada");
                    }
                }
                textBlockAnterior1 = null;
                textBlockAnterior2 = null;
            }
        }
        //Abro la carta cambiando el fondo y el symbolo.
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((Border)sender).Background != myBrush_White)
            {
                viewbox = new Viewbox();
                textBlock = new TextBlock();
                ((Border)sender).Background = myBrush_White;
                ((Border)sender).Child = viewbox;

                viewbox.Child = textBlock;

                if (symbolosAleatoriosFijo.Count != 0)
                {
                    int elementoAt = Convert.ToInt32(((Border)sender).Tag);
                    textBlock.Text = symbolosAleatoriosFijo.ElementAt(elementoAt);

                    if (tag % 2 != 0)
                        textBlockAnterior1 = textBlock;
                    else
                        textBlockAnterior2 = textBlock;
                    tag++;
                }
            }
        }
        //RadioButtons > depende de lo que este elegido para filas que se acumulan.
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
        //Se inicializa los componentes desde cero y borra las columnas y filas del grid que se crea para iniciar la partida y llama a los metodos "GuardaArrayAleatorio()" y "CreaCartasParaOcultar()"
        private void Button_Click_Iniciar(object sender, RoutedEventArgs e)
        {
            ComponentesParaIniciar();

            if (numeroFilas > 0)
            {
                gridPrincipal.ColumnDefinitions.Clear();
                gridPrincipal.RowDefinitions.Clear();
                yaCogidoValor.Clear();
                columnasXFilas = COLUMNAS * numeroFilas;

                GuardaArrayAleatorio();

                CreaCartasParaOcultar();
                
            }
            
        }
        //Saca symbolo aleatorio de la lista sin repetir
        private string DevuelveSymboloAleatorio()
        {
            string symbolo = "";
            do
            {
                int aleatorio = random.Next(0, 17);
                symbolo = arraySymbolos[aleatorio];

            } while (yaCogidoValor.Contains(symbolo));
            yaCogidoValor.Add(symbolo);
            
            return symbolo;
        }
        //Muestra todos los symbols y la partida se acaba.
        private void Button_Click_Mostrar(object sender, RoutedEventArgs e)
        {
            if (juegoIniciado)
            {
                int index = 0;
                for (int i = 0; i < numeroFilas; i++)
                    for (int j = 0; j < COLUMNAS; j++)
                    {
                        VisualizarCartaConIconoYPosicion(i, j, symbolosAleatoriosFijo.ElementAt(index++), myBrush_White);
                    }
            }
            
        }
    }
    

}
