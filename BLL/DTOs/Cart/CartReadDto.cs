using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CartReadDto
    {
       public int CartId  { get; set; }
       public string UserId { get; set; }
        public List<CartItemReadDto> cartItems { get; set; }
    }
}
