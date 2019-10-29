using MyPortfolioWebApp.Services.ProjectsRepository.ProjectsRepositoryReturnTypes;

namespace MyPortfolioWebApp.Services.OperationsWithFiles
{
    public interface IProjectFilesResolver
    {
        public void Resolve(ProjectInfo projectInfo);
    }
}
