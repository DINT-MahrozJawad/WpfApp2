﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private const string iconoOculto = "❓";
        private string[] arraySymbolos = new string[] { "🌟", "💎", "🍂", "🎥", "❤️", "📢", "✈️", "🦄", "🌻", "📷", "⚽️", "💸", "🌴", "🕷", "🎈", "🐢", "🌎", "💲", "🎁", "🗡" };
        public MainWindow()
        {
            InitializeComponent();

            Border border = new Border();
            Viewbox viewbox = new Viewbox();
            TextBlock textBlock = new TextBlock();
            
            gridPrincipal.Children = 


        }
    }
}