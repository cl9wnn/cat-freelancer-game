using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringMethods
{
    public static string FormatMoney(float amount)
    {
        if (amount < 1000)
        {
            return amount.ToString("0.##");
        }

        string[] suffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };
        int suffixIndex = 0;

        while (amount >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            amount /= 1000;
            suffixIndex++;
        }

        return amount.ToString("0.##") + suffixes[suffixIndex];
    }
}
