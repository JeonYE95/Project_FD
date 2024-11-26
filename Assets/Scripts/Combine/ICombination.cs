using System.Collections.Generic;

public interface ICombination
{
    bool CanCombine(List<string> inputUnit);
    string Combine(List<string> inputUnit);
}