using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolioWebApp.Services.BlogPostsRepository.BlogRepositoryReturnTypes
{
    public class LogoInfo
    {
        public int Id { get; set; }
        public string Extension { get; set; }
        public DateTime TimeChanged { get; set; }
        public override bool Equals(object obj)
        {
            var anotherInfo = obj as LogoInfo;
            if (Object.ReferenceEquals(this, obj)) return true;
            return this.Id == anotherInfo.Id &&
                   this.Extension.Equals(anotherInfo.Extension, StringComparison.Ordinal) &&
                   this.TimeChanged.Equals(anotherInfo.TimeChanged) ? true : false;            
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var result = 0;
                result = (result * 397) ^ Id.GetHashCode();
                result = (result * 397) ^ Extension.GetHashCode();
                result = (result * 397) ^ TimeChanged.Year.GetHashCode();
                result = (result * 397) ^ TimeChanged.Month.GetHashCode();
                result = (result * 397) ^ TimeChanged.Day.GetHashCode();
                result = (result * 397) ^ TimeChanged.Hour.GetHashCode();
                result = (result * 397) ^ TimeChanged.Minute.GetHashCode();
                result = (result * 397) ^ TimeChanged.Second.GetHashCode();
                return result;
            }
        }

    }
}
