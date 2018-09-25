using System;
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
using System.Drawing;


namespace FiveInRowGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Board's scale chan be configured as desired.
        /// </summary>
        int boardScale = 18;

        /// <summary>
        /// Instance of 5 in a row game, with two players.
        /// </summary>
        FiveInRowGameClass game;

        public MainWindow()
        {
            InitializeComponent();

            initializeGame();
        }

        /// <summary>
        /// Reserts the game board on screen, resets the game state.
        /// </summary>
        void initializeGame()
        {
            InitializeBoard();

            game = new FiveInRowGameClass(boardScale);
        }

        /// <summary>
        /// Initializes a fresh copy of game board on screen.
        /// </summary>
        void InitializeBoard()
        {
            GameBoardGrid.RowDefinitions.Clear();
            GameBoardGrid.ColumnDefinitions.Clear();
            GameBoardGrid.Children.Clear();

            //Add needed columns and rows to board's grid
            for (int i = 0; i < boardScale; i++)
            {
                GameBoardGrid.RowDefinitions.Add(new RowDefinition());
                GameBoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            //Add buttons to grid as unit of game's board
            for (int i = 0; i < boardScale; i++)
            {
                for (int j = 0; j < boardScale; j++)
                {
                    //Create new button
                    Button butt = new Button();
                    butt.Name = "butt" + i + "_" + j;


                    //Add newly created button to GameBoardGrid
                    GameBoardGrid.Children.Add(butt);
                    Grid.SetColumn(butt, j);
                    Grid.SetRow(butt, i);

                    butt.Click += new RoutedEventHandler(OnClick);
                }
            }
        }

        /// <summary>
        /// Arranges the funtions which must work when user selects.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnClick(object sender, RoutedEventArgs e)
        {
            //If the selected button has not been selected before and game is not finished yet, select it.
            if (((sender as Button).IsHitTestVisible == true) && (game.End == FiveInRowGameClass.FieldState.None))
            {
                (sender as Button).IsHitTestVisible = false;
                (sender as Button).Background = Brushes.Green;

                //Save user selected butt
                game.SaveUserSelection(Grid.GetRow(sender as Button), Grid.GetColumn(sender as Button));

                checkForWinner();

                //If the user didnt' win, computer plays.
                if (game.End == FiveInRowGameClass.FieldState.None)
                {
                    //Call myTurn method
                    myTurn(Grid.GetRow(sender as Button), Grid.GetColumn(sender as Button));

                    checkForWinner();
                }

            }
        }

        /// <summary>
        /// Keeps needed functions for the computer to play. 
        /// Needs the last move of user for more efficient move. (Don't need to check the whole board.)
        /// </summary>
        /// <param name="userSelectionRow"></param>
        /// <param name="userSelectionCol"></param>
        void myTurn(int userSelectionRow, int userSelectionCol)
        {
            //Generate my turn's selecteion
            //<<used point for speed up>>
            Point mySelectedPoint = game.GenerateMySelection();

            Button mySelectedButton = (GameBoardGrid.Children.Cast<UIElement>()
            .First(e => Grid.GetRow(e) == mySelectedPoint.X && Grid.GetColumn(e) == mySelectedPoint.Y)) as Button;

            (mySelectedButton as Button).IsHitTestVisible = false;
            (mySelectedButton as Button).Background = Brushes.DarkOrange;
        }

        /// <summary>
        /// Check to see if someOne has won and game is finished.
        /// </summary>
        void checkForWinner()
        {
            if (game.End != FiveInRowGameClass.FieldState.None)
            {
                MessageBox.Show(game.End.ToString() + " won the game!", "Game finished");
            }
        }

        private void RstButt_Click(object sender, RoutedEventArgs e)
        {
            InitializeBoard();
            game.Reset(boardScale);
        }
    }
}
