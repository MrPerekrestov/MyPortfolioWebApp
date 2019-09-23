using System;
using System.Collections.Generic;

namespace MyPortfolioWebApp.DbContexts.PortfolioDbContext
{
    public partial class Projects
    {
        public Projects()
        {
            ProjectsImages = new HashSet<ProjectsImages>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Creationdate { get; set; }
        public DateTime Publishingdate { get; set; }
        public string Description { get; set; }
        public DateTime? Imageschanged { get; set; }

        public virtual ProjectsHtml ProjectsHtml { get; set; }
        public virtual ICollection<ProjectsImages> ProjectsImages { get; set; }
    }
}
