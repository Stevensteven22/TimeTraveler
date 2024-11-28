using System;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Services;

public class ResultVerifyFiveService : IResultVerifyFiveService
{
    public bool VerifyFive(object result)
    {
        Type resultType = result.GetType();
        var properties = resultType.GetProperties();
        string problem1Answer = "",
            problem3Answer = "";
        foreach (var property in properties)
        {
            if (property.Name == "Problem1Answer")
            {
                problem1Answer = property.GetValue(result).ToString();
            }
            else if (property.Name == "Problem3Answer")
            {
                problem3Answer = property.GetValue(result).ToString();
            }
        }

        if (
            problem1Answer == "C"
            && problem3Answer == "辛焱"
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public (bool, string) BeforeVerifyFive(object result)
    {
        try
        {
            string errorMsg = "";
            Type resultType = result.GetType();
            var properties = resultType.GetProperties();
            string problem1Answer = "",
                problem3Answer = "";
            foreach (var property in properties)
            {
                if (property.Name == "Problem1Answer")
                {
                    problem1Answer = property.GetValue(result).ToString();
                }
                else if (property.Name == "Problem3Answer")
                {
                    problem3Answer = property.GetValue(result).ToString();
                }
            }

            if (string.IsNullOrEmpty(problem3Answer))
            {
                errorMsg += "第一个问题的答案不能为空。\r\n";
            }

            if (string.IsNullOrEmpty(problem1Answer))
            {
                errorMsg += "第二个问题的答案不能为空。\r\n";
            }

            if (
                string.IsNullOrEmpty(problem1Answer)
                || string.IsNullOrEmpty(problem3Answer)
            )
            {
                return (false, errorMsg);
            }
            else
            {
                return (true, errorMsg);
            }
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}
