namespace Shop.Api.Utils
{
    public interface IExpressSchedule
    {
        string GetSchedule(string expressName, string expressNumber);
    }
}