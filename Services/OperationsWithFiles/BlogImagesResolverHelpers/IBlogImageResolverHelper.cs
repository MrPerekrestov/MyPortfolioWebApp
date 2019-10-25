namespace MyPortfolioWebApp.Services.OperationsWithFiles.BlogImagesResolverHelpers
{
    public interface IBlogImageResolverHelper
    {
        bool BlogPostsDirectoryExists();
        void CreateAllImagesAndTimeStamps(int blogPostId);
        void CreateCurrentBlogPostDirectory(int blogPostId);
        void CreateImagesDirectory(int blogPostId);
        void CreateImagesTimeStamp(int blogPostId);
        void CreatePostsDicrectory();
        bool CurrentBlogPostDirectoryExists(int blogPostId);
        bool ImagesDirectoryExists(int blogPostId);
        bool TimeStampFileExist(int blogPostId);
        bool TimeStampMatches(int blogPostId);
        void UpdateImagesAndTimeStamps(int blogPostId);
        void UpdateTimeStamp(int blogPostId);
    }
}