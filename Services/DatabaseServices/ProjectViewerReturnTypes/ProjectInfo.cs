using System;
using System.Collections.Generic;
using System.Text;

namespace MyPortfolioWebApp.DatabaseManager.DatabaseService.ProjectViewerReturnTypes
{
    public class ProjectInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Published { get; set; }
        public string Description { get; set; }
        public DateTime ImagesChanged { get; set; }
    }
}
