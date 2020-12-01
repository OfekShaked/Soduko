using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GameLogic;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SodukoGameOS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        TextBox[,] boxes;
        Board b1;
        bool isStarted;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            boxes = new TextBox[9, 9];
            InitializeGridBoxes();
            b1 = new Board();
            b1.GenerateBoard(Difficulty.Easy);
            SetBoard();
            isStarted = true;

        }
        private void InitializeGridBoxes()
        {
            isStarted = false;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Border b1 = new Border();
                    b1.BorderBrush = new SolidColorBrush(Colors.Gray);
                    b1.BorderThickness = new Thickness(2);
                    TextBox box1 = new TextBox();
                    box1.FontSize = 20;
                    box1.FontWeight = FontWeights.ExtraBlack;
                    box1.BorderThickness = new Thickness(0);
                    box1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    box1.TextAlignment = TextAlignment.Center;
                    box1.VerticalAlignment = VerticalAlignment.Center;
                    box1.MaxLength = 1;
                    box1.Name = $"{i},{j}";
                    box1.BeforeTextChanging += Box1_BeforeTextChanging;
                    box1.TextChanged += Box1_TextChanged;
                    Grid.SetColumn(box1, i);
                    Grid.SetRow(box1, j);
                    Grid.SetColumn(b1, i);
                    Grid.SetRow(b1, j);
                    SodukoGrid.Children.Add(b1);
                    SodukoGrid.Children.Add(box1);
                    boxes[i, j] = box1;
                }
            }
        }

        private void Box1_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box1 = sender as TextBox;
            if (box1.Text == "0")
            {
                box1.Text = "";
            }
            else
            {
                if (box1.Text != ""&&isStarted)
                {
                    string[] position = box1.Name.Split(',');
                    if( b1.EnterNumber(byte.Parse(position[0]), byte.Parse(position[1]), byte.Parse(box1.Text))==false)
                    {
                        box1.Foreground = new SolidColorBrush(Colors.Red);
                    }    
                }
            }
        }

        private void Box1_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void ClearBoard()
        {
            isStarted = false;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    boxes[i, j].Text = "";
                    boxes[i, j].IsReadOnly = false;
                }
            }
        }
        private void SetBoard()
        {
            isStarted = false;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    boxes[i, j].Text = b1.GameBoard[i, j].ToString();
                    if (boxes[i, j].Text == "0")
                    {
                        boxes[i, j].Text = "";
                    }
                    else
                    {
                        boxes[i, j].IsReadOnly = true;
                    }
                }
            }
        }

        private void btnCreateBoard_Click(object sender, RoutedEventArgs e)
        {
            isStarted = false;
            ClearBoard();
            if (rdEasy.IsChecked == true)
                b1.GenerateBoard(Difficulty.Easy);
            else if (rdMedium.IsChecked == true)
                b1.GenerateBoard(Difficulty.Medium);
            else if (rdHard.IsChecked == true)
                b1.GenerateBoard(Difficulty.Hard);
            SetBoard();
            isStarted = true;
        }

        private async void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog md;
            if (b1.CheckIfBoardIsFullAndValid() == false)
            {
                md = new MessageDialog("Incorrect");
            }
            else
            {
                md = new MessageDialog("Correct!"); 
            }
            await md.ShowAsync();

        }
    }
}
