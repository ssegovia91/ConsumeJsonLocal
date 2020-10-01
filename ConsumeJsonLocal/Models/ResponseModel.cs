using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumeJsonLocal.Models
{
    public class ResponseModel<T>
    {
        public string Message { get; set; }
        public T Result { get; set; }
        public int Count
        {
            get
            {
                if (this.Result is IList list)
                {
                    return list.Count;
                }

                return 1;
            }
        }
    }
}
