using System;
using System.Diagnostics.CodeAnalysis;

namespace MyPortfolioWebApp.Services.BlogPostsRepository.BlogRepositoryReturnTypes
{
    public class BlogPostInfo:IComparable<BlogPostInfo>
    {
        public int Id { get; set; }
        public string Title { get; set; } 
        public int LogoId { get; set; }
        public string Description { get; set; }
        public DateTime ImagesChanged { get; set; } 
        public DateTime Published { get; set; }
        public override string ToString()
        {
            return $"{Id} {Title} {LogoId} {Description} {ImagesChanged} {Published}";
        }
        public int CompareTo([AllowNull] BlogPostInfo other)
        {
            if (Object.ReferenceEquals(this, other)) return 0;

            var result = 
                   this.Id == other.Id && this.Title.Equals(other.Title,StringComparison.Ordinal)&&
                   this.LogoId == other.LogoId && this.Description.Equals(other.Description, StringComparison.Ordinal) &&
                   this.ImagesChanged.Equals(other.ImagesChanged) && this.Equals(other.Published)?0:this.Id>other.Id?1:-1;

            return result;
        }
    }
}
