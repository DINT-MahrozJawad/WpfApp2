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
        private const int filasX3 = 3;
        private const int filasX4 = 4;
        private const int filasX5 = 5;

        private const int COLUMNAS = 4;
        private int numeroFilas = int.MinValue;
        private const string iconoOculto = "s";
        private string[] arraySymbolos;
        private int tag;
        private int columnasXFilas;
        private int sumaCartas;
        private bool juegoIniciado;
        private List<string> yaCogidoValor;
        private List<string> symbolosAleatoriosFijo;
        private DispatcherTimer timer;
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
        //Inicializo Componentes desde cero.
        public void ComponentesParaIniciar()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += Timer_Tick;

            columnasXFilas = COLUMNAS * numeroFilas;
            juegoIniciado = true;
            pbStatus.Value = 0;
            tag = 0;
            sumaCartas = 0;
            arraySymbolos = new string[] { "q","w","e","r","t","y","u","i","o","p","a","z","x","c","v","b","n","m","d","f,","g","h","j","k","l" };
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            //paro el despatcherTimer y Habilito mi grid principal.
            OcultaIconoYCambiaBackground(textBlockAnterior1);
            OcultaIconoYCambiaBackground(textBlockAnterior2);
            
            timer.Stop();
            gridPrincipal.IsEnabled = true;

            textBlockAnterior1 = null;
            textBlockAnterior2 = null;
        }

        //Sacndo Symbolos Aleatorio y guardando en una lissta.
        private void GuardaArrayAleatorio()
        {
            string[] symbolosAleatorios = new string[columnasXFilas];
            int aleatorio = 0;

            for (int i = 0; i < columnasXFilas; i++)
            {
                string symbolo = DevuelveSymboloAleatorio(/*i*/); //el "i" que le paso, para hacer que las columnas y filas sea aleatorio, cambiando solamente con el valor.
                //Añadiendo 2 en 2 los symbolos en el array
                symbolosAleatorios[i++] = symbolo;
                symbolosAleatorios[i] = symbolo;
            }
            do
            {
                //Copiando (2 en 2 array)symbolosAleatorios en la lista aleatoriamente.
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
            //Crea columnas
            for (int j = 0; j < COLUMNAS; j++)
                gridPrincipal.ColumnDefinitions.Add(new ColumnDefinition());
            //crea filas
            for (int i = 0; i < numeroFilas; i++)
                gridPrincipal.RowDefinitions.Add(new RowDefinition());

            //Cartas en esas columna*Filas
            for (int i = 0; i < numeroFilas; i++)
                for (int j = 0; j < COLUMNAS; j++)
                    VisualizarCartaConIconoYPosicion(i, j, iconoOculto, myBrush);
        }
        //Visualiza cartas para el metodo "CreaCartasParaOcultar()".
        private void VisualizarCartaConIconoYPosicion(int i, int j, string letra, LinearGradientBrush background)
        {
            border = new Border();
            viewbox = new Viewbox();
            textBlock = new TextBlock();
            //Agregando lo atributo que necesito para visualizar
            border.BorderThickness = new Thickness(1);
            border.BorderBrush = Brushes.Black;
            border.Background = background;
            border.Margin = new Thickness(2);
            border.CornerRadius = new CornerRadius(5);
            border.Tag = tag++;
            //Agregando Eventos.
            border.MouseLeftButtonDown += Border_MouseLeftButtonDown;
            //Agrego el icono pasado
            textBlock.FontFamily = new FontFamily("Webdings");
            textBlock.Text = letra;
            //Añado cada hijo dentro de su padre.
            gridPrincipal.Children.Add(border);
            border.Child = viewbox;
            viewbox.Child = textBlock;
            Grid.SetRow(border, i);
            Grid.SetColumn(border, j);
        }
        
        
        private void OcultaIconoYCambiaBackground(TextBlock t)
        {
            //Cogiendo desde padre, llegando hasta el hijo, para poner la imagen que es de ocultar y cambio el fondo
            viewbox = (Viewbox)t.Parent;
            border = (Border)viewbox.Parent;
            border.Background = myBrush;
            t.Text = iconoOculto;
        }

        //Abro la carta cambiando el fondo y el symbolo.
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Si el sender tiene un background distinto que blanco pues me entro en el if.
            if (((Border)sender).Background != myBrush_White)
            {
                viewbox = (Viewbox)((Border)sender).Child;
                textBlock = (TextBlock)viewbox.Child;
                //Cambio su background
                ((Border)sender).Background = myBrush_White;
                ((Border)sender).Child = viewbox;

                viewbox.Child = textBlock;

                if (symbolosAleatoriosFijo.Count != 0)
                {
                    //Cuando creo la carta le asigno su TAG. y aquí saco para scar el elemento de un array aleatorio.
                    int elementoAt = Convert.ToInt32(((Border)sender).Tag);
                    textBlock.Text = symbolosAleatoriosFijo.ElementAt(elementoAt);
                    //Creo una variable tag que si es par coje el primer textBlockAnterior y cuando no es par coge segundo textoBlockAnterior y se puede poner viseversa.
                    if (tag % 2 != 0) //Simplemento compruebo par y impar.
                        textBlockAnterior1 = textBlock;
                    else
                        textBlockAnterior2 = textBlock;
                    tag++;
                }
            }

            //Comparo los symbolos si son iguales o no, si son iguales se quedan abiertos y la barra se avanza lo que se debe avanzar.

            //Si los dos son distintos que nulo, me entro.
            if (textBlockAnterior1 != null && textBlockAnterior2 != null)
            {

                if (textBlockAnterior1.Text != textBlockAnterior2.Text)
                {
                    gridPrincipal.IsEnabled = false;
                    timer.Start();
                }
                else
                {
                    //Cada vez que aparecen dos cartas del mismo texto sumo hasta llegar al final.
                    sumaCartas += 2;
                    //Controlando la barra sacando su porcentaje de las cartas.
                    double d = (sumaCartas * 100 / columnasXFilas);
                    int value = (int)Math.Round(d, 0);
                    pbStatus.Value = value;
                    //Cuando termina la partida, muestro el mensaje.
                    if (sumaCartas == columnasXFilas)
                        MessageBox.Show("¡Enhorabuena! Partida Finalizada", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                    textBlockAnterior1 = null;
                    textBlockAnterior2 = null;
                }
            }

        }
        //RadioButtons > depende de lo que este elegido para filas que se acumulan.
        private void RadioButton_Checked_Baja(object sender, RoutedEventArgs e)
        {
            //Asigno numeroFilas
            numeroFilas = filasX3;
        }
        private void RadioButton_Checked_Media(object sender, RoutedEventArgs e)
        {
            //Asigno numeroFilas
            numeroFilas = filasX4;
        }
        private void RadioButton_Checked_Alta(object sender, RoutedEventArgs e)
        {
            //Asigno numeroFilas
            numeroFilas = filasX5;
        }
        //Se inicializa los componentes desde cero y borra las columnas y filas del grid que se crea para iniciar la partida y llama a los metodos "GuardaArrayAleatorio()" y "CreaCartasParaOcultar()"
        private void Button_Click_Iniciar(object sender, RoutedEventArgs e)
        {
            //Inicio los componentes cada vez que damos a iniciar.
            ComponentesParaIniciar();
            //si número de filas es mayor que 0, porque al inicializar le pongo el valor minimo por defecto.
            if (numeroFilas > 0)
            {
                //Borro las columnas y filas que se ha creado al iniciar el juego, por que si no se acumularán cada vez que iniciamos.
                gridPrincipal.ColumnDefinitions.Clear();
                gridPrincipal.RowDefinitions.Clear();
                yaCogidoValor.Clear();
                //LLamo a los metodos.
                GuardaArrayAleatorio();
                CreaCartasParaOcultar();
            }
        }
        //Devuelve symbolo aleatorio de la lista sin repetir
        private string DevuelveSymboloAleatorio(/*int i*/) 
        {
            string symbolo = "";
            do
            {
                //Sacando symbolo
                int aleatorio = random.Next(0, arraySymbolos.Length);
                symbolo = arraySymbolos[aleatorio];

                //por si necesitamos coger más symbolos, Necesitamos salir del bucle porque ya cogido valor tendrá todos los symbolos ya cogido.
                //if (i < columnasXFilas)
                  //  break;

            } while (yaCogidoValor.Contains(symbolo)); // Sacando symbolo sin repetir.
            //Añado el symbolo en el array.
            yaCogidoValor.Add(symbolo);
            
            return symbolo;
        }
        //Muestra todos los symbols y la partida se acaba.
        private void Button_Click_Mostrar(object sender, RoutedEventArgs e)
        {
            //index para sacar todos los elemento de la lista recorriendo
            int index = 0;
            pbStatus.Value = 100;
            if (juegoIniciado)
                for (int i = 0; i < numeroFilas; i++)
                    for (int j = 0; j < COLUMNAS; j++) //Recorro como  una matriz y pongo el elemento (Sacado aleatorio: fijo) y cambio el background a blanco
                        VisualizarCartaConIconoYPosicion(i, j, symbolosAleatoriosFijo.ElementAt(index++), myBrush_White);
        }
    }
}
