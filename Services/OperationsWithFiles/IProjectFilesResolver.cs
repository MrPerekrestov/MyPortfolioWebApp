using MyPortfolioWebApp.Services.ProjectsRepository.ProjectsRepositoryReturnTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolioWebApp.Services.OperationsWithFiles
{
    public interface IProjectFilesResolver
    {
        public void Resolve(ProjectInfo projectInfo);
    }
}
