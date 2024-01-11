using Emarket_Website.Models;

namespace Emarket_Website.ViewModels
{
    public class HomeViewModel
    {
        public List<ShopCategory> shopCatagorIES { get; set; }
        public List<Shop> shopS { get; set; }
        public List<CartItem> cartItemS { get; set; }
        public List<Item> itemS { get; set; }
        public List<User> userS { get; set; }
        public ICollection<ItemImage> ItemImages { get; set; }
        public ICollection<ItemPrice> ItemPrices { get; set; }
        public List<ShopEntry> ShopEntries { get; set; }
        public List<City> cities { get; set; }

        public int ShopCatId { get; set; }
        public List<Order> orders { get; set; }
        public List<Ordereditem> Ordereditems { get; set; }
        public List<OrderStatuss> orderStatusses { get; set; }
        public List <SellerApplication> sellerApplications { get; set; }
        public Review Review { get; set; }
        public int RatingValue { get; set; }
        public string Comment { get; set; }
    }
}