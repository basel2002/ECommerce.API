using Common;
using DAL;

namespace BLL
{
    public class CartManager:ICartManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private CartReadDto MapToCartDto(Cart cart)
        {
            return new CartReadDto
            {
                UserId = cart.UserId,
                CartId = cart.Id,
                cartItems = cart.CartItems.Select(ci => new CartItemReadDto
                {
                    ProductName = ci.product.Name,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.product.Price
                }).ToList()
            };
        }
        public async Task<GeneralResult<CartReadDto>> AddToCartAsync(string userId, int productID)
        {
            var productExists = await _unitOfWork._productRepository.GetByIdAsync(productID);
            if (productExists == null)
                return GeneralResult<CartReadDto>.NotFound("No Such Product Id");
            var userCart = await _unitOfWork._cartRepository.GetCartWithItemsAsync(userId);
            if (userCart == null)
            {
         
                var cart = new Cart()
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                   
                };
                _unitOfWork._cartRepository.Add(cart);
                cart.CartItems = new List<CartItem>()
                {new CartItem()
                {
                    ProductId = productID,
                    Quantity = 1,
                    AddedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    CartId = cart.Id

                }};

                //_unitOfWork._cartRepository.Add(cart); // ← don't forget to add cart
                await _unitOfWork.SaveChangesAsync();
                return GeneralResult<CartReadDto>.SuccessResult("Cart created and item added successfully.");
            }



            var productToBeAdded = userCart.CartItems.FirstOrDefault(p => p.ProductId == productID);

            if(productToBeAdded == null)
            {
                var newCartItem = new CartItem()
                {
                    CartId = userCart.Id,
                    AddedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,

                    ProductId = productID,
                    Quantity = 1

                };
                userCart.CartItems.Add(newCartItem);
                await _unitOfWork.SaveChangesAsync();

                var cartReadDto1 = MapToCartDto(userCart);
                return GeneralResult<CartReadDto>.SuccessResult(cartReadDto1);
            }

            productToBeAdded.Quantity += 1;
            await _unitOfWork.SaveChangesAsync();
            var cartReadDto = MapToCartDto(userCart);
            return GeneralResult<CartReadDto>.SuccessResult(cartReadDto);

        }




        public async Task<GeneralResult<IEnumerable< CartItemReadDto>>> GetUserCart(string userId)
        {

           
            var userCart = await _unitOfWork._cartRepository.GetUserCart(userId);
            if (userCart == null) { 
            
                return  GeneralResult<IEnumerable<CartItemReadDto>>.NotFound("User Has No Cart Yet");
            }

            var cartItemReadDto = userCart.Select(p => new CartItemReadDto()
            {
                ProductName = p.product.Name,
                Quantity = p.Quantity,
                UnitPrice = p.product.Price
            }
            ).ToList();

            return GeneralResult<IEnumerable<CartItemReadDto>>.SuccessResult(cartItemReadDto);


        }
        

        public async Task<GeneralResult<IEnumerable<CartItemReadDto>>> RemoveFromCart(string userId,int productId)
        {
            var userCart = await _unitOfWork._cartRepository.GetUserCart(userId);
            
            if (userCart == null)
            {

                return GeneralResult<IEnumerable<CartItemReadDto>>.NotFound("User Has No Cart Yet");
            }

            var isProductExist = await _unitOfWork._productRepository.GetByIdAsync(productId);
            if(isProductExist == null)
            {
                return GeneralResult<IEnumerable<CartItemReadDto>>.NotFound("No such Product");
            }

            var result  = userCart.Any(p => p.ProductId == productId);
            if(!result)
            {
                return GeneralResult<IEnumerable<CartItemReadDto>>.NotFound("This User Cart Doesn't Contain This Product");

            }
            var toBeremoved  = userCart.FirstOrDefault(p => p.ProductId == productId);
            

            _unitOfWork._cartRepository.RemoveFromCart(toBeremoved);
            await _unitOfWork.SaveChangesAsync();

            var updatedtCart = await _unitOfWork._cartRepository.GetUserCart(userId);

            var cartItemReadDto = updatedtCart.Select(p => new CartItemReadDto()
            {
                ProductName = p.product.Name,
                Quantity = p.Quantity,
                UnitPrice = p.product.Price
            }).ToList();

            return GeneralResult<IEnumerable<CartItemReadDto>>.SuccessResult(cartItemReadDto);


        }

        public async Task<GeneralResult> UpdateCartItemQuantity(string userId,CartItemUpdateDto cartItemUpdateDto)
        {
            var userCart = await _unitOfWork._cartRepository.GetUserCart(userId);
            if(userCart == null)
            {
                return GeneralResult.NotFound("User Has No Cart");
            }
            var isProductExist = await _unitOfWork._productRepository.GetByIdAsync(cartItemUpdateDto.ProductId);
            if(isProductExist == null)
            {
                return GeneralResult.NotFound("No Such Product");
            }

            if(!userCart.Any(p=>p.ProductId == cartItemUpdateDto.ProductId))
            {
                return GeneralResult.NotFound("You are trying to update an item that doesn't exist in the cart");

            }
            var toBeUpdated = userCart.FirstOrDefault(p => p.ProductId == cartItemUpdateDto.ProductId);
            
            if(cartItemUpdateDto.NewQuantity == 0)
            {
              await  RemoveFromCart(userId, cartItemUpdateDto.ProductId);

            }
            toBeUpdated.Quantity = cartItemUpdateDto.NewQuantity;


             _unitOfWork._cartRepository.UpdateCartItemQuantity(toBeUpdated);
            await _unitOfWork.SaveChangesAsync();
            return GeneralResult.SuccessResult("Item Updated ");
        }

            
    }
}
