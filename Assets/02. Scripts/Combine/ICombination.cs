using System.Collections.Generic;

public interface ICombination
{
    bool CanCombine(List<string> inputUnit);
    string Combine(List<string> inputUnit);

    // 주석 테스트 22
}