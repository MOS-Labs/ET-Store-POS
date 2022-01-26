using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETStore
{
    class CredInputValidation
    {
        public static int CredInputVal(string UserID, string Password)
        {
            try
            {

                bool blUserIDEmpty = String.IsNullOrEmpty(UserID);
                bool blPasswordEmpty = String.IsNullOrEmpty(Password);
                int errorID;
                errorID = 0;

                if (blUserIDEmpty & blPasswordEmpty)
                {
                    errorID = 1;

                }

                else if (blPasswordEmpty)
                {

                    errorID = 2;
                }

                else if (blUserIDEmpty)
                {
                    errorID = 3;

                }

                return errorID;


            }

            catch(Exception)
            {
                throw;
            }


        }
    }
}
