using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Hoond.Models
{
    public class RFID
    {
        private int[] arrCompanyBits = new int[] { 40, 37, 34, 30, 27, 24, 20 };
        private int[] arrCompanyDigits = new int[] { 12, 11, 10, 9, 8, 7, 6 };
        private int iPartition = 5;

        public string GenerateEPCfromBarCode(string pstrBarCode, string pstrSerial)
        {
            pstrBarCode = pstrBarCode.PadLeft(13, '0');
            byte[] epc = new byte[12];
            fillArray(ref epc, getBinaryValue(Convert.ToDecimal(48)), 96, 8);
            fillArray(ref epc, getBinaryValue(Convert.ToDecimal(1)), 88, 3);
            fillArray(ref epc, getBinaryValue(Convert.ToDecimal(5)), 85, 3);
            fillArray(ref epc, getBinaryValue(Convert.ToDecimal(pstrBarCode.Substring(0, 7))), 82, arrCompanyBits[iPartition]);
            fillArray(ref epc, getBinaryValue(Convert.ToDecimal(pstrBarCode.Substring(7, 5))), 82 - arrCompanyBits[iPartition], 44 - arrCompanyBits[iPartition]);
            fillArray(ref epc, getBinaryValue(Convert.ToDecimal(pstrSerial)), 38, 38);
            return ByteToString(epc).ToUpper();
        }

        public string GenerateURIfromBarCode(string pstrBarCode, string pstrSerial)
        {
            pstrBarCode = pstrBarCode.PadLeft(13, '0');
            string strResult = "tag:sgtin-96:0";
            string strTemp = fillWithLeadingZeroes(pstrBarCode.Substring(0, 7), 7);

            strResult += "." + strTemp;

            strTemp = fillWithLeadingZeroes(pstrBarCode.Substring(7, 5), 6);

            strResult += "." + strTemp;

            strResult += "." + pstrSerial;


            return strResult;
        }

        private string fillWithLeadingZeroes(string pstrArgument, int piRequiredLenght)
        {
            string strResult = pstrArgument;
            if (pstrArgument.Length < piRequiredLenght)
            {
                for (int x = pstrArgument.Length; x < piRequiredLenght; x++)
                {
                    strResult = "0" + strResult;
                }
            }
            else
                strResult = pstrArgument;

            return strResult;

        }

        private void fillArray(ref byte[] pworkingArray, byte[] pvalueArray, int piStart, int piLength)
        {
            Array.Reverse(pworkingArray);
            string temp = string.Empty;

            System.Collections.BitArray workingBits = new System.Collections.BitArray(pworkingArray);
            System.Collections.BitArray valueBits = new System.Collections.BitArray(pvalueArray);
            int iPos = piStart - 1;

            for (int i = piLength - 1; (i > -1) && (iPos > -1); i--)
            {
                if (i > (valueBits.Length - 1))
                    workingBits[iPos] = false;
                else if (!workingBits[iPos])
                    workingBits[iPos] = valueBits[i];

                iPos--;
                temp += Convert.ToInt16(valueBits[i]).ToString();
            }
            workingBits.CopyTo(pworkingArray, 0);
            Array.Reverse(pworkingArray);
        }

        private byte[] getBinaryValue(decimal pValue)
        {
            byte[] workArray;
            using (System.IO.MemoryStream myStream = new System.IO.MemoryStream())
            {
                using (System.IO.BinaryWriter myWriter = new System.IO.BinaryWriter(myStream))
                {
                    myWriter.Write(pValue);
                    workArray = myStream.ToArray();
                }
            }
            return workArray;
        }

        private string ByteToString(byte[] pValue)
        {
            StringBuilder sbMyString = new StringBuilder(pValue.Length);
            for (int i = 0; i < pValue.Length; i++)
            {
                sbMyString.Append(pValue[i].ToString("x2"));
            }
            return sbMyString.ToString();
        }

        public RFID()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}