using System;
using System.Collections.Generic;

namespace MyPortfolioWebApp.DbContexts.PortfolioDbContext
{
    public partial class ProjectsHtml
    {
        public int Id { get; set; }
        public string Html { get; set; }

        public virtual Projects IdNavigation { get; set; }
    }
}
