namespace MyPortfolioWebApp.Services.OperationsWithFiles
{
    public interface IBlogLogoResolver
    {
        (bool success, string message)  Resolve();
    }
}