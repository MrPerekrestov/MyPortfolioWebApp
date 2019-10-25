using MyPortfolioWebApp.Services.BlogPostsRepository.BlogRepositoryReturnTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolioWebApp.Models.Blog
{
    public class BlogModel
    {
        public List<BlogPostInfo> blogPosts { get; set; }
    }
}
