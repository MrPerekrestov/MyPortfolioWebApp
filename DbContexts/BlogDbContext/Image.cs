using System;
using System.Collections.Generic;

namespace MyPortfolioWebApp.DbContexts.BlogDbContext
{
    public partial class Image
    {
        public int ImageId { get; set; }
        public int BlogPostId { get; set; }
        public byte[] ImageBlob { get; set; }
        public string Extension { get; set; }
        public DateTime TimeChanged { get; set; }

        public virtual Post BlogPost { get; set; }
    }
}
