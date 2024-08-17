using System.Globalization;
using UnityEngine;

public class StringMethods
{
    public static string FormatMoney(float amount, bool wideText = false)
    {
        if (amount < 1000)
        {
            return amount.ToString();
        }

        string[] suffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };
        int suffixIndex = 0;

        while (amount >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            amount /= 1000;
            suffixIndex++;
        }

        if (amount >= 100)
            return amount.ToString($"F{(wideText ? 1 : 0)}", CultureInfo.InvariantCulture) + suffixes[suffixIndex];
        else 
            return amount.ToString($"F{(wideText ? 2 : 1)}", CultureInfo.InvariantCulture) + suffixes[suffixIndex];
    }

    public static float ParseFormattedCost(float amount)
    {
        string[] suffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };
        int suffixIndex = 0;

        while (amount >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            amount /= 1000;
            suffixIndex++;
        }

        if (amount >= 100)
            amount = Mathf.Round(amount); 
        else
            amount = Mathf.Round(amount * 10f) / 10f;

        for (int i = 0; i < suffixIndex; i++)
        {
            amount *= 1000;
        }

        return amount;
    }
}

