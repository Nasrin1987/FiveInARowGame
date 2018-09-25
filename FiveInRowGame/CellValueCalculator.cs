using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveInRowGame
{
    static class CellValueCalculator
    {
        /// <summary>
        /// Contains the cell value for winning state
        /// </summary>
        public static int WonVal = 100000;
       
        struct StringValue
        {
            public StringValue(string letters, int val)
            {
                fiveLetters = letters;
                value = val;
            }

            public string fiveLetters;
            public int value;
        }


        /// <summary>
        /// Keeps value of possible strings of 5 letters
        /// </summary>
        static StringValue[] valueArray = new StringValue[32] {
            new StringValue( "NNNNN",0 ),
            new StringValue( "mNNNN",10 ),
            new StringValue( "NmNNN",15 ),
            new StringValue( "NNmNN",20 ),
            new StringValue( "NNNmN",15 ),
            new StringValue( "NNNNm",10 ),
            new StringValue( "mmNNN",30 ),
            new StringValue( "mNmNN",40 ),
            new StringValue( "mNNmN",40 ),
            new StringValue( "mNNNm",30 ),
            new StringValue( "NmmNN",50 ),
            new StringValue( "NmNmN",70 ),
            new StringValue( "NmNNm",50 ),
            new StringValue( "NNmmN",45 ),
            new StringValue( "NNmNm",30 ),
            new StringValue( "NNNmm",30 ),
            new StringValue( "mmmNN",100 ),
            new StringValue( "mmNmN",120 ),
            new StringValue( "mmNNm",100 ),
            new StringValue( "mNmmN",120 ),
            new StringValue( "mNNmm",120 ),
            new StringValue( "mNmNm",110 ),
            new StringValue( "NmmmN",500 ),
            new StringValue( "NmNmm",120 ),
            new StringValue( "NNmmm",100 ),
            new StringValue( "mNmNm",130 ),
            new StringValue( "Nmmmm",2000 ),
            new StringValue( "mNmmm",1500 ),
            new StringValue( "mmNmm",1000 ),
            new StringValue( "mmmNm",1500 ),
            new StringValue( "mmmmN",2000 ),
            new StringValue( "mmmmm",WonVal ),
        };

        /// <summary>
        /// Retrieves value from value array for the given string.
        /// </summary>
        /// <param name="letters"></param>
        /// <returns></returns>
        public static int GetValue(string letters)
        {
            for (int i = 0; i < valueArray.Length; i++)
            {
                if ((valueArray[i]).fiveLetters == letters)
                {                
                    return (valueArray[i]).value;
                }               
            }

            
            //not reachable code
            return 0;
        }
    }
}
