using System;
using System.Collections.Generic;

namespace MyPortfolioWebApp.DbContexts.PortfolioDbContext
{
    public partial class ProjectsImages
    {
        public int Projectid { get; set; }
        public int Imageid { get; set; }
        public byte[] Image { get; set; }
        public string Extension { get; set; }
        public DateTimeOffset? Timestamp { get; set; }

        public virtual Projects Project { get; set; }
    }
}
