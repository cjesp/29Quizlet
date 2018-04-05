using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.QuizletTypes.Search
{
    public class SearchResult
    {
        public int total_results { get; set; }
        public int total_pages { get; set; }
        public int page { get; set; }
        public IEnumerable<ISearchItem> items { get; set; }
        public bool Success { get; set; }

        public static SearchResult NoResults()
        {
            var result = new SearchResult()
            {
                total_results = 0,
                total_pages = 0,
                items = new List<ISearchItem>(),
                page = 0,
                Success = false
            };
            return result;
        }
    }
}
