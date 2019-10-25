using System;
using System.Collections.Generic;

namespace MyPortfolioWebApp.DbContexts.BlogDbContext
{
    public partial class Logo
    {
        public Logo()
        {
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] LogoBytes { get; set; }
        public string Extension { get; set; }
        public DateTime TimeChanged { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
