namespace TimeTraveler.Libary.Services;

public interface IResultVerifyFiveService
{
    public bool VerifyFive(object result);
    
    public (bool,string) BeforeVerifyFive(object result);
}
