using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveInRowGame
{
    class FiveInRowGameClass
    {
        //Indicated the state of each cell
        public enum FieldState { None = 'N', Computer = 'C', Partner = 'P' };

        //Keeps the state of whole board
        FieldState[,] gameBoardState;

        //Keeps the value of each cell, the more value means a beeter selection for computer.
        int[,] gameBoardValues;

        //Keeps the state of ongoing game, if is none, means the game not yet finished.
        public FieldState End;

        /// <summary>
        /// Constructor which initializes the board arrays
        /// </summary>
        /// <param name="boardScale"></param>
        public FiveInRowGameClass(int boardScale)
        {
            Reset(boardScale);
        }

        /// <summary>
        /// Selects most valuable cell for computer's turn.
        /// </summary>
        /// <returns></returns>
        public System.Windows.Point GenerateMySelection()
        {
            System.Windows.Point tmpPoint = getHighestValFromValueArray();
            saveSelection(FieldState.Computer, (int)tmpPoint.X, (int)tmpPoint.Y);

            return tmpPoint;
        }

        /// <summary>
        /// Selects most valuable cell from Valued Array
        /// </summary>
        /// <returns></returns>
        System.Windows.Point getHighestValFromValueArray()
        {
            System.Windows.Point maxValuepoint = new System.Windows.Point();

            int maxVal = 0;

            for (int i = 0; i < 18; i++)
            {
                for (int j = 0; j < 18; j++)

                    if (gameBoardValues[i, j] > maxVal)
                    {
                        maxVal = gameBoardValues[i, j];

                        maxValuepoint.X = i;
                        maxValuepoint.Y = j;
                    }
            }
            return maxValuepoint;
        }

        /// <summary>
        /// Saves the user selected cell.
        /// </summary>
        /// <param name="userSelectionRow"></param>
        /// <param name="userSelectionCol"></param>
        public void SaveUserSelection(int userSelectionRow, int userSelectionCol)
        {
            saveSelection(FieldState.Partner, userSelectionRow, userSelectionCol);
        }

        /// <summary>
        /// Saves the newly selected cell due to player and position, checks the winning state.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="selectedRow"></param>
        /// <param name="selectedCol"></param>
        void saveSelection(FieldState f, int selectedRow, int selectedCol)
        {
            gameBoardState[selectedRow, selectedCol] = f;
            gameBoardValues[selectedRow, selectedCol] = 0;

            refreshgameBoardValues();

            checkWinnerState(selectedRow, selectedCol, f);
        }

        /// <summary>
        /// Initializes the state of board, fills all cells with "Not selected" state
        /// </summary>
        /// <param name="boardScale"></param>
        void initializeGameBoardState(int boardScale)
        {

            gameBoardState = new FieldState[boardScale, boardScale];

            for (int i = 0; i < boardScale; i++)
            {
                for (int j = 0; j < boardScale; j++)
                {
                    gameBoardState[i, j] = FieldState.None;
                }
            }
        }

        /// <summary>
        /// Initializes the array of values, due to possibility of winning and prevention of losing.
        /// </summary>
        /// <param name="boardScale"></param>
        void initializeGameBoardValues(int boardScale)
        {
            gameBoardValues = new int[boardScale, boardScale];

            refreshgameBoardValues();
        }

        /// <summary>
        /// Calls value calculation for all cells. This can be more efficient if gets changed cell
        /// and only calculated effected cells
        /// </summary>
        void refreshgameBoardValues()
        {
            for (int i = 0; i < gameBoardValues.GetLength(0); i++)
            {
                for (int j = 0; j < gameBoardValues.GetLength(1); j++)
                {
                    //Here is a summation of computer's valued amount and partner's valued amount, because
                    //we need to: win + not to lose!
                    if (gameBoardState[i, j] == FieldState.None)
                        gameBoardValues[i, j] = getValue(i, j, FieldState.Computer) + ((getValue(i, j, FieldState.Partner)));
                }
            }

        }


        /// <summary>
        /// Calculates cel's value and returns it, it needs to know for which player it should calculate.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="F"></param>
        /// <returns></returns>
        int getValue(int i, int j, FieldState f)
        {
            string fiveLet = "";
            int valueSum = 0;
            int tmpRow = 0;
            int tmpCol = 0;
            FieldState[,] tmpGameBoardState = (FieldState[,])gameBoardState.Clone();
            tmpGameBoardState[i, j] = f;

            //Vertical evaluation
            for (int s = 0; s < 5; s++)
            {
                fiveLet = "";
                for (int d = 0; d < 5; d++)
                {
                    tmpRow = (i - 4) + s + d;
                    tmpCol = j;

                    if ((tmpRow < gameBoardState.GetLength(0)) && (0 <= tmpRow))
                        if ((tmpCol < gameBoardState.GetLength(1)) && (0 <= tmpCol))
                            fiveLet += ((char)tmpGameBoardState[tmpRow, tmpCol]);
                }
                if (fiveLet.Length == 5)
                    valueSum += stringEvaluator(f, fiveLet);
            }

            //Horizontal evaluation
            for (int s = 0; s < 5; s++)
            {
                fiveLet = "";
                for (int d = 0; d < 5; d++)
                {
                    tmpRow = i;
                    tmpCol = (j - 4) + s + d;

                    if ((tmpRow < gameBoardState.GetLength(0)) && (0 <= tmpRow))
                        if ((tmpCol < gameBoardState.GetLength(1)) && (0 <= tmpCol))
                            fiveLet += ((char)tmpGameBoardState[tmpRow, tmpCol]);
                }
                if (fiveLet.Length == 5)
                    valueSum += stringEvaluator(f, fiveLet);
            }

            //Left slant evaluation
            for (int s = 0; s < 5; s++)
            {
                fiveLet = "";
                for (int d = 0; d < 5; d++)
                {
                    tmpRow = (i + 4) - s - d;
                    tmpCol = (j - 4) + s + d;

                    if ((tmpRow < gameBoardState.GetLength(0)) && (0 <= tmpRow))
                        if ((tmpCol < gameBoardState.GetLength(1)) && (0 <= tmpCol))
                            fiveLet += ((char)tmpGameBoardState[tmpRow, tmpCol]);
                }
                if (fiveLet.Length == 5)
                    valueSum += stringEvaluator(f, fiveLet);
            }

            //Right slant evaluation
            for (int s = 0; s < 5; s++)
            {
                fiveLet = "";
                for (int d = 0; d < 5; d++)
                {
                    tmpRow = (i - 4) + s + d;
                    tmpCol = (j - 4) + s + d;

                    if ((tmpRow < gameBoardState.GetLength(0)) && (0 <= tmpRow))
                        if ((tmpCol < gameBoardState.GetLength(1)) && (0 <= tmpCol))
                            fiveLet += ((char)tmpGameBoardState[tmpRow, tmpCol]);
                }
                if (fiveLet.Length == 5)
                    valueSum += stringEvaluator(f, fiveLet);
            }

            return valueSum;
        }


        /// <summary>
        /// Takes 5 fieldstate and generates: 
        /// "null" if string contains non desired fieldstate,
        /// int "value", if it only contains desired fieldstates and "Not selected" states.
        /// </summary>
        /// <returns></returns>
        int stringEvaluator(FieldState f, string fiveLetterState)
        {
            //if it contains chars other than F(desired field state) and "Not selected", return 0.
            int count = 0;
            foreach (char c in fiveLetterState)
                if ((c == (char)f) || (c == (char)FieldState.None)) count++;

            if (count < 5)
                return 0;

            else
            {
                int val = CellValueCalculator.GetValue(fiveLetterState.Replace((char)f, 'm'));

                return val;
            }
        }

        /// <summary>
        /// Checks winning state for 5 same states in row, column, left slant
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        int checkWinnerState(int i, int j, FieldState f)
        {
            string fiveLet = "";
            int valueSum = 0;
            int tmpRow = 0;
            int tmpCol = 0;
            FieldState[,] tmpGameBoardState = (FieldState[,])gameBoardState.Clone();
            tmpGameBoardState[i, j] = f;

            //Vertical evaluation
            for (int s = 0; s < 5; s++)
            {
                fiveLet = "";
                for (int d = 0; d < 5; d++)
                {
                    tmpRow = (i - 4) + s + d;
                    tmpCol = j;

                    if ((tmpRow < gameBoardState.GetLength(0)) && (0 <= tmpRow))
                        if ((tmpCol < gameBoardState.GetLength(1)) && (0 <= tmpCol))
                            fiveLet += ((char)tmpGameBoardState[tmpRow, tmpCol]);
                }
                checkWinner(fiveLet, f);
            }

            //Horizontal evaluation
            for (int s = 0; s < 5; s++)
            {
                fiveLet = "";
                for (int d = 0; d < 5; d++)
                {
                    tmpRow = i;
                    tmpCol = (j - 4) + s + d;

                    if ((tmpRow < gameBoardState.GetLength(0)) && (0 <= tmpRow))
                        if ((tmpCol < gameBoardState.GetLength(1)) && (0 <= tmpCol))
                            fiveLet += ((char)tmpGameBoardState[tmpRow, tmpCol]);
                }
                checkWinner(fiveLet, f);
            }

            //Left slant evaluation
            for (int s = 0; s < 5; s++)
            {
                fiveLet = "";
                for (int d = 0; d < 5; d++)
                {
                    tmpRow = (i + 4) - s - d;
                    tmpCol = (j - 4) + s + d;

                    if ((tmpRow < gameBoardState.GetLength(0)) && (0 <= tmpRow))
                        if ((tmpCol < gameBoardState.GetLength(1)) && (0 <= tmpCol))
                            fiveLet += ((char)tmpGameBoardState[tmpRow, tmpCol]);
                }
                checkWinner(fiveLet, f);
            }

            //Right slant evaluation
            for (int s = 0; s < 5; s++)
            {
                fiveLet = "";
                for (int d = 0; d < 5; d++)
                {
                    tmpRow = (i - 4) + s + d;
                    tmpCol = (j - 4) + s + d;

                    if ((tmpRow < gameBoardState.GetLength(0)) && (0 <= tmpRow))
                        if ((tmpCol < gameBoardState.GetLength(1)) && (0 <= tmpCol))
                            fiveLet += ((char)tmpGameBoardState[tmpRow, tmpCol]);
                }
                checkWinner(fiveLet, f);
            }

            return valueSum;
        }

        void checkWinner(string fiveLet, FieldState f)
        {
            if ((fiveLet.Length == 5) && (CellValueCalculator.WonVal == stringEvaluator(f, fiveLet)))
                End = f;
        }

        public void Reset(int boardScale)
        {
            //Initialize gamebord state
            initializeGameBoardState(boardScale);

            //Initialize gamebord value
            initializeGameBoardValues(boardScale);

            End = FieldState.None;
        }

    }
}
