using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class RecommendationsResponse<T>
    {
        public Object schema;
        public IEnumerable<T> data;
    }
}
