using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static class NWERandom
    {
        //-------------------------------------------------------------------------------------------------------------
        public static void ShuffleList<T>(this IList<T> sList)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = sList.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do
                    provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = sList[k];
                sList[k] = sList[n];
                sList[n] = value;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
