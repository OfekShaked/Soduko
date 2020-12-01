using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class Board
    {
        Random rnd;
        public byte[,] GameBoard { get; set; }
        public Board()
        {
            GameBoard = new byte[9, 9];
            rnd = new Random();
        }

        private byte[] GetBox(byte boxNum)
        {
            switch (boxNum)
            {
                case 0:
                    return BoxNums(0, 0);
                case 1:
                    return BoxNums(0, 3);
                case 2:
                    return BoxNums(0, 6);
                case 3:
                    return BoxNums(3, 0);
                case 4:
                    return BoxNums(3, 3);
                case 5:
                    return BoxNums(3, 6);
                case 6:
                    return BoxNums(6, 0);
                case 7:
                    return BoxNums(6, 3);
                case 8:
                    return BoxNums(6, 6);
                default: return null;
            }
        }
        /// <summary>
        /// Enter a number to the board, if the number is correct keep it if its not correct remove it.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns>True if number is correct, false if number is not correct</returns>
        public bool EnterNumber(byte row, byte column, byte value)
        {
            if (row > 8 || row < 0 || column > 8 || column < 0 || value > 9 || value < 1)
                throw new SodukoException();
            GameBoard[row, column] = value;
            if(CheckBoardValidity()==false)
            {
                GameBoard[row, column] = 0;
                return false;
            }
            return true;
        }
        private byte[] GetRow(byte rowNum)
        {
            byte[] row = new byte[9];
            for (byte i = 0; i < 9; i++)
            {
                row[i] = GameBoard[rowNum, i];
            }
            return row;
        }
        private byte[] GetColumn(byte columnNum)
        {
            byte[] column = new byte[9];
            for (byte i = 0; i < 9; i++)
            {
                column[i] = GameBoard[i, columnNum];
            }
            return column;
        }
        /// <summary>
        /// Returns all numbers in a box from the row and column given
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private byte[] BoxNums(byte row, byte column)
        {
            byte[] nums = new byte[9];
            byte count = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    nums[count] = GameBoard[row + i, column + j];
                    count++;
                }
            }
            return nums;
        }
        /// <summary>
        /// Checks if the 9 digit array is distinct without number repititions except for 0
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        private bool CheckArrayValidity(byte[] nums)
        {
            if (nums.Any(k => k != 0 && nums.Where(j => j != 0 && k == j).Count() > 1)) return false;
            return true;
        }
        /// <summary>
        /// Check if the board is valid, checking each row column and box.
        /// </summary>
        /// <returns></returns>
        private bool CheckBoardValidity()
        {
            for (byte i = 0; i < 9; i++)
            {
                if (CheckArrayValidity(GetBox(i)) == false) return false;
            }
            for (byte i = 0; i < 9; i++)
            {
                if (CheckArrayValidity(GetRow(i)) == false) return false;
            }
            for (byte i = 0; i < 9; i++)
            {
                if (CheckArrayValidity(GetColumn(i)) == false) return false;
            }
            return true;
        }
        /// <summary>
        /// Check if the board is full, and if it is check if its valid.
        /// </summary>
        /// <returns></returns>
        public bool CheckIfBoardIsFullAndValid()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (GameBoard[i, j] == 0) return false;
                }
            }
            return CheckBoardValidity();
        }
        /// <summary>
        /// Generate a new board by the difficulty level.
        /// </summary>
        /// <param name="dif"></param>
        public void GenerateBoard(Difficulty dif)
        {
            Start:
            ResetBoard();
            List<byte> nums;
            int BigReps=0;
            int randomNum;
            for (int i = 0; i < 9; i++)
            {
                nums = GetNineNumsList();
                int countReps = 0;
                for (int j = 0; j < 9;)
                {
                    
                    randomNum = rnd.Next(nums.Count);
                    GameBoard[i, j] = nums[randomNum];
                    if (i > 0)
                    {
                        if (CheckBoardValidity() == false)
                        {
                            GameBoard[i, j] = 0;
                            countReps++;
                            if(countReps>70) //if there are too many repetitions try to create the row again.
                            { 
                                ResetRow(i);
                                BigReps++;
                                nums = GetNineNumsList();
                                countReps = 0;
                                j = 0;
                                if(BigReps>50) //if there are to many repititions in general reset board and start again, this is a safety feature.
                                {
                                    ResetBoard();
                                    goto Start;
                                }
                            }
                        }
                        else
                        {
                            nums.RemoveAt(randomNum);
                            j++;
                        }
                    }
                    else
                    {
                        nums.RemoveAt(randomNum);
                        j++;
                    }
                }
            }
            HideByDifficulty(dif);
        }
        /// <summary>
        /// Hide random numbers in the board depends on the difficulty
        /// </summary>
        /// <param name="dif"></param>
        private void HideByDifficulty(Difficulty dif)
        {
            for (int i = 0; i < 81-(int)dif;)
            {
                int randomRow = rnd.Next(9);
                int randomColumn = rnd.Next(9);
                if(GameBoard[randomRow,randomColumn]!=0)
                {
                    GameBoard[randomRow, randomColumn] = 0;
                    i++;
                }
            }
        }
        private List<byte> GetNineNumsList()
        {
            List<byte> nums = new List<byte>();
            for (byte i = 1; i <= 9; i++)
            {
                nums.Add(i);
            }
            return nums;
        }
        private void ResetRow(int rowNum)
        {
            for (int i = 0; i < 9; i++)
            {
                GameBoard[rowNum, i] = 0;
            }
        }
        private void ResetBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    GameBoard[i, j] = 0;
                }
            }
        }
    }
}
