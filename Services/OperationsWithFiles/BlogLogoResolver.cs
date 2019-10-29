using MyPortfolioWebApp.Services.OperationsWithFiles.BlogLogoResolverHelpers;


namespace MyPortfolioWebApp.Services.OperationsWithFiles
{
    public class BlogLogoResolver : IBlogLogoResolver
    {
        private readonly IBlogLogoResolverHelper _helper;

        public BlogLogoResolver(IBlogLogoResolverHelper helper)
        {
            _helper = helper;
        }

        public (bool success, string message) Resolve()
        {
            var logosInfo = _helper.GetLogosInfo();
            if (logosInfo == null) { return (false, "Logos are empty"); }
            var returnMessage = "Logo files are up to date";
            foreach (var logoInfo in logosInfo)
            {
                var logoFilePath = _helper.GetLogoFilePath(logoInfo.Id, logoInfo.Extension);
                var timeStampFilePath = _helper.GetLogoTimeStampPath(logoInfo.Id);
                var filesDoesNotExist = _helper.LogoFileDoesNotExist(logoFilePath);
                var timeStampDoesNotMatch = _helper.TimeStampDoesNotMatch(timeStampFilePath, logoInfo.TimeChanged);
                if (filesDoesNotExist ||
                    timeStampDoesNotMatch)
                {
                    var logoBytes = _helper.GetLogoBytes(logoInfo.Id);
                    _helper.CreateLogoImageFile(logoFilePath, logoBytes);
                    _helper.CreateLogoTimeStampFile(timeStampFilePath, logoInfo.TimeChanged);
                    returnMessage = "Logo files were updated";
                }
            }
            return (true, returnMessage);
        }

    }
}
