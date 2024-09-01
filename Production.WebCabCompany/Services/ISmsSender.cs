namespace Production.WebCabCompany.Services;

public interface ISmsSender
{
    Task SendSmsAsync(string number, string message);
}