using System;
using System.Collections.Generic;

namespace MyPortfolioWebApp.DbContexts.BlogDbContext
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Images = new HashSet<Image>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int? LogoId { get; set; }
        public DateTime Published { get; set; }
        public string Description { get; set; }
        public DateTime ImagesChanged { get; set; }
        public string Html { get; set; }

        public virtual Logo Logo { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}
