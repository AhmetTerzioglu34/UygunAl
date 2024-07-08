using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project.BLL.Managers.Abstracts;
using Project.COMMON.Tools;
using Project.COREMVC.Areas.Admin.Models.PureVms.Category;
using Project.COREMVC.Areas.Admin.Models.PureVms.Product;
using Project.COREMVC.Models.PageVms.AppUsers;
using Project.COREMVC.Models.PageVms.OrderPageVm;
using Project.COREMVC.Models.PageVms.Product;
using Project.COREMVC.Models.PageVms.Shopping;
using Project.COREMVC.Models.PureVms.AppUsers;
using Project.COREMVC.Models.PureVms.Attribute;
using Project.COREMVC.Models.PureVms.OrderDetails;
using Project.COREMVC.Models.PureVms.Orders;
using Project.COREMVC.Models.PureVms.Products;
using Project.COREMVC.Models.PureVms.Shoppings;
using Project.COREMVC.Models.SessionServices;
using Project.COREMVC.Models.ShoppingTools;
using Project.ENTITIES.Models;
using System.Linq;
using System.Text;
using X.PagedList;

namespace Project.COREMVC.Controllers
{
    public class ShoppingController : Controller
    {
        readonly IProductManager _productManager;
        readonly ICategoryManager _categoryManager;
        readonly IOrderManager _orderManager;
        readonly IOrderDetailManager _orderDetailManager;
        readonly UserManager<AppUser> _userManager;
        readonly IHttpClientFactory _httpClientFactory;
        readonly IEntityAttributeProductManager _entityAttributeProductManager;
        readonly IEntityAttributeManager _entityAttributeManager;
        public ShoppingController(IProductManager productManager, ICategoryManager categoryManager, IOrderManager orderManager, IOrderDetailManager orderDetailManager, UserManager<AppUser> userManager, IHttpClientFactory httpClientFactory, IEntityAttributeProductManager entityAttributeProductManager, IEntityAttributeManager entityAttributeManager)
        {
            _productManager = productManager;
            _categoryManager = categoryManager;
            _orderManager = orderManager;
            _orderDetailManager = orderDetailManager;
            _userManager = userManager;
            _httpClientFactory = httpClientFactory;
            _entityAttributeProductManager = entityAttributeProductManager;
            _entityAttributeManager = entityAttributeManager;
        }

        public IActionResult Index(int? page, int? categoryID)
        {

            List<Category> categories = _categoryManager.GetActives();

            List<CategoryForShoppingPureVm> cPureVm = categories.Select(cPureVm => new CategoryForShoppingPureVm
            {
                ID = cPureVm.ID,
                CategoryName = cPureVm.CategoryName

            }).ToList();

            List<Product> products = _productManager.GetActives();

            IPagedList<ProductForShoppingPureVm> pPureVm = products.Select(pPureVm => new ProductForShoppingPureVm
            {
                ID = pPureVm.ID,
                ProductName = pPureVm.ProductName,
                UnitPrice = pPureVm.UnitPrice,
                UnitsInStock = pPureVm.UnitsInStock,
                ImagePath = pPureVm.ImagePath,
                CategoryName = pPureVm.Category.CategoryName,
                CategoryID = pPureVm.Category.ID

            }).ToPagedList();



            ShoppingPageVm sPageVm = new ShoppingPageVm();


            sPageVm.ProductForShoppingPureVms = categoryID == null ? pPureVm.ToPagedList(page ?? 1, 5) : pPureVm.Where(x => x.CategoryID == categoryID).ToPagedList(page ?? 1, 5);

            sPageVm.CategoryForShoppingPureVms = cPureVm;



            if (categoryID != null)
            {
                TempData["catID"] = categoryID;
            }




            return View(sPageVm);
        }

       
        public async Task<IActionResult> AddToCart(int id)
        {
            // Burada kısacası sepet varsa onu kullan yoksa yeniden yarat
            Cart c = GetCartFromSession("scart") == null ? new Cart() : GetCartFromSession("scart");
            CartPageVm cartPageVm = new CartPageVm();
            cartPageVm.Cart = c;


            Product productToBeAdded = await _productManager.FindAsync(id);
            ProductForShoppingPureVm pPureVm = new()
            {
                ID = productToBeAdded.ID,
                ProductName = productToBeAdded.ProductName,
                UnitPrice = productToBeAdded.UnitPrice,
                
                CategoryID = productToBeAdded.Category.ID,
                CategoryName = productToBeAdded.Category.CategoryName,
                ImagePath = productToBeAdded.ImagePath

            };


            CartItem ci = new()
            {
                ID = pPureVm.ID,
                ProductName = pPureVm.ProductName,
                UnitPrice = pPureVm.UnitPrice,
                ImagePath = pPureVm.ImagePath,
                CategoryName = pPureVm.CategoryName,
                CategoryID = pPureVm.CategoryID
            };

            cartPageVm.Cart.AddToCart(ci);


            SetCartForSession(cartPageVm.Cart);

            TempData["message"] = $"{ci.ProductName} isimli ürün sepete eklenmiştir";

            return RedirectToAction("Index");
        }

        // Bu metottada sepeti session'a eklemiş bulnmaktayız
        void SetCartForSession(Cart c)
        {
            HttpContext.Session.SetObject("scart", c);
        }

        // Cart nesnesi session da var mı yok mu onu kontrol etmesi için bu geriye değer döndüren metotu yarattık
        Cart GetCartFromSession(string key)
        {
            return HttpContext.Session.GetObject<Cart>(key);
        }


        public IActionResult CartPage()
        {
            if (GetCartFromSession("scart") == null)
            {
                TempData["message"] = "Sepetiniz su anda bos";
                return RedirectToAction("Index");
            }


            Cart c = GetCartFromSession("scart");

            CartPageVm cartPageVm = new CartPageVm();
            cartPageVm.Cart = c;
            return View(cartPageVm); 
        }


        public IActionResult DeleteFromCart(int id)
        {
            if (GetCartFromSession("scart") != null)
            {
                Cart c = GetCartFromSession("scart");


                CartPageVm cartPageVm = new CartPageVm();
                cartPageVm.Cart = c;


               cartPageVm.Cart.RemoveFromCart(id);
                SetCartForSession(cartPageVm.Cart);
                ControlCart(cartPageVm.Cart);
            }
            return RedirectToAction("CartPage");
        }

        //Cart'ın içerisindeki ürünler sıfırsa Cart'ı direkt sil
        void ControlCart(Cart c)
        {
            if (c.GetCartItems.Count == 0) HttpContext.Session.Remove("scart");
        }

        public IActionResult DecreaseFromCart(int id)
        {

            if (GetCartFromSession("scart") != null)
            {
                Cart c = GetCartFromSession("scart");
                CartPageVm cartPageVm= new CartPageVm();
                cartPageVm.Cart = c;
                cartPageVm.Cart.Decrease(id);
                SetCartForSession(cartPageVm.Cart);
                ControlCart(cartPageVm.Cart);
            }

            return RedirectToAction("CartPage");
        }
        //public async Task<IActionResult> IncreaseFromCart(int id)
        //{
        //    Product product = await _productManager.FindAsync(id);

        //    if (product.UnitsInStock != null)
        //    {
        //        Cart c = GetCartFromSession("scart");
        //        CartPageVm cartPageVm = new CartPageVm();
        //        cartPageVm.Cart = c;
        //        cartPageVm.Cart.AddToCart();
        //        SetCartForSession(cartPageVm.Cart);
        //        ControlCart(cartPageVm.Cart);
        //    }
        //}
        public IActionResult ConfirmOrder()
        {
            return View();
        }
        [HttpPost]
        public async  Task<IActionResult> ConfirmOrder(OrderRequestPageVm orPageVm)
        {

         
            
            Cart c = GetCartFromSession("scart");
            CartPageVm cartPageVm = new CartPageVm();
            cartPageVm.Cart = c;


            orPageVm.OrderPureVm.PriceOfOrder = orPageVm.PaymentRequestPureVm.ShoppingPrice = cartPageVm.Cart.TotalPrice;


             //http://localhost:5118/

            HttpClient client = _httpClientFactory.CreateClient();
            string jsonData = JsonConvert.SerializeObject(orPageVm.PaymentRequestPureVm);
            StringContent content =new StringContent(jsonData, Encoding.UTF8,"application/json");
           HttpResponseMessage responseMessage =  await client.PostAsync("http://localhost:5118/api/Transaction", content);


            if (responseMessage.IsSuccessStatusCode)
            {
                if (User.Identity.IsAuthenticated)
                {
                    AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

                    orPageVm.OrderPureVm.AppUserID = appUser.Id;
                    orPageVm.OrderPureVm.Email = appUser.Email;
                    orPageVm.OrderPureVm.NameDescription = appUser.UserName;

                }

                Order order = new()
                {
                    Email = orPageVm.OrderPureVm.Email,
                    NameDescription = orPageVm.OrderPureVm.NameDescription,
                    AppUserID = orPageVm.OrderPureVm.AppUserID,
                    PriceOfOrder = orPageVm.OrderPureVm.PriceOfOrder,
                    ShippingAddress = orPageVm.OrderPureVm.ShippingAddress,
                    ID = orPageVm.OrderPureVm.ID
                };
                await _orderManager.AddAsync(order);

                
                string urunIsimleri = "";
                decimal ürünToplamFiyat = 0;
                foreach (CartItem item in cartPageVm.Cart.GetCartItems)
                {
                    OrderDetail orderDetail = new()
                    {
                        OrderID = order.ID,
                        ProductID = item.ID,
                        Quantity = item.Amount,
                        UnitPrice = item.UnitPrice
                    };

                    Product product = await _productManager.FindAsync(item.ID);
                    product.UnitsInStock -= item.Amount;
                    await _productManager.UpdateAsync(product);

                    await _orderDetailManager.AddAsync(orderDetail);
                    urunIsimleri += $" {item.ProductName} x{item.Amount} tane,";
                    
                    ürünToplamFiyat += item.SubTotal;
                    

                }

                urunIsimleri = urunIsimleri.TrimEnd(',');
                string body = $"Bizi tercih ettiğiniz için teşkkür ederiz aldığınız ürünler şunlardır : \n {urunIsimleri} \n Toplam fiyatı => {ürünToplamFiyat}";
                
                TempData["message"] = "Siparişiniz bize başarılı bir şekilde ulaşmıştır Teşekkür ederiz";
                MailService.Send(orPageVm.OrderPureVm.Email,body : body);
                HttpContext.Session.Remove("scart");
                return RedirectToAction("Index");
            }


            string result = await responseMessage.Content.ReadAsStringAsync();
            TempData["message"] = result;
            return RedirectToAction("Index");




        }

           
        public async Task<IActionResult> ProductDetail(int id, ProductDetailPageVm model)
        {
            Product p = await _productManager.FindAsync(id);

            List<EntityAttributeProduct> eAProduct = _entityAttributeProductManager.Where(x=> x.ProductID == id).ToList();

            List<EntityAttribute> eAttributes = _entityAttributeManager.GetAll();

            List<EntityAttribute> newEAttributess = eAttributes.Join(eAProduct,
           eAttr => eAttr.ID,  // eAttributes'taki ID
           eProd => eProd.EntityAttributeID,  // eAProducts'taki EntityAttributeID
           (eAttr, eProd) => eAttr)  // Eşleşen eAttributes nesnelerini al
                                     .ToList();



            List<AttributePureVm> pureVms = eAProduct.Join(eAttributes, c => c.EntityAttributeID, a => a.ID, (c, a) => new AttributePureVm
                  {
                   
                    AttributeName = a.AttributeName,
                    ValueName = c.Value
                  }).ToList();




            ProductDetailPureVm pureVm = new()
            {
                ID = p.ID,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                ImagePath = p.ImagePath,
                CategoryName = p.Category.CategoryName
            };

           

           

            ProductDetailPageVm pageVm = new ProductDetailPageVm();
            pageVm.ProductDetailPureVm = pureVm;
            pageVm.AttributePureVms = pureVms;


            return View(pageVm);
        }

       

    }
}
