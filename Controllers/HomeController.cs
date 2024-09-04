using Microsoft.AspNetCore.Mvc;
using RecyclingHub.Models.DbModels;
using RecyclingHub.Dependencies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using RecyclingHub.Models.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
namespace RecyclingHub.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly IHubServices<Sale> SaleService;
        readonly IHubServices<Product> ProductService;
        readonly IHubServices<Insection> InsectionService;
        readonly IHubServices<Inventory> InventoryService;
        readonly IHubServices<Aquisition> AquisitionService;
        readonly IHubServices<Disposal> DisposalService;
        readonly UserManager<AppUser> UserManager;
        readonly SignInManager<AppUser> SignInManager;
        readonly INotyfService NotyfService;

        public HomeController(ILogger<HomeController> logger,IHubServices<Sale> saleService
            , IHubServices<Product> productService, IHubServices<Insection> InsectionService
            , IHubServices<Inventory> inventories , IHubServices<Aquisition> aquisition,
            IHubServices<Disposal> disposalService , UserManager<AppUser> userManager
            , SignInManager<AppUser> signInManager,INotyfService notyfService)
        {
            _logger = logger;
            SaleService = saleService;
            ProductService = productService;
            this.InsectionService = InsectionService;
            this.InventoryService = inventories;
            AquisitionService = aquisition;
            DisposalService = disposalService;
            UserManager = userManager;
            SignInManager = signInManager;
            NotyfService = notyfService;
        }

        public IActionResult Index()
        {
            ViewBag.page = "Products";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SaleForm(string Id)
        {
            var query = Id.Split(",");
            var product = await ProductService.GetAsync(int.Parse(query[0]));
            ViewBag.ProductId = int.Parse(query[0]);
            ViewBag.Mass = query[4];
            ViewBag.Amount = query[2];
            ViewBag.Gross = query[1];
            ViewBag.Tare = query[3];    
            ViewBag.productName = product.Name;
            ViewBag.price = product.Price;
            ViewBag.page = "Sales";
            return View();
        }
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> SaleForm(Sale sale)
        {
            var product = (await ProductService.GetAsync(sale.CompanyName)).SingleOrDefault(p => p.Name == sale.ProductName) ?? new();
            if (ModelState.IsValid)
            {
                if(sale.CashAmount == 0.0)
                {
                    //_logger.Log(LogLevel.Information,sale.CashAmount)
                    ModelState.AddModelError(string.Empty,"Cash amount cannot be Zero");
                    ViewBag.Amount = sale.CashAmount;
                    ViewBag.Mass = sale.WeightInKgs;
                    
                    ViewBag.productName = product.Name;
                    ViewBag.price = product.Price;
                    ViewBag.page = "Sales";
                    return View(sale);
                }
                await SaleService.AddAsync(sale);
                NotyfService.Success($"Sale made for {sale.ProductName}");
                return Redirect("/home/sales");

            }
            ViewBag.page = "Sales";
            ViewBag.Amount = sale.CashAmount;
            ViewBag.Mass = sale.WeightInKgs;
            
            ViewBag.productName = product.Name;
            ViewBag.price = product.Price;
            return View(sale);
        }
        public IActionResult Sales()
        {
            ViewBag.page = "Sales";
            return View();
        }

        public IActionResult ProductEditor(int? Id)
        {
            ViewBag.page = "Products";
            if (Id == null)
            {
                
                ViewBag.mode = "Add";
                return View();
            }
            ViewBag.mode = "Edit";
            return View(ProductService.GetAsync(Id.Value).Result);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ProductEditor(Product product , IFormFile? productImage,string mode)
        {
            ViewBag.page = "Products";
            if (ModelState.IsValid)
            {
                product.Price = product.Price.Replace(",", ".");
                if (productImage != null && productImage.Length > 0) 
                {
                    
                    string fileName = productImage.FileName;
                    
                    if (mode != "Edit")
                    {
                        product.Image = fileName;
                        string newFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Products", fileName);

                        using (var stream = new FileStream(newFilePath, FileMode.Create))
                        {
                            productImage.CopyToAsync(stream).Wait();
                        }
                        await ProductService.AddAsync(product);
                        NotyfService.Success("New Product Added");
                    }
                    else
                    {
                        // Delete the existing file if it exists
                        string existingFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Products",product.Image);
                        if (System.IO.File.Exists(existingFilePath))
                        {
                            System.IO.File.Delete(existingFilePath);
                        }

                        string newFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Products", fileName);

                        using (var stream = new FileStream(newFilePath, FileMode.Create))
                        {
                            productImage.CopyToAsync(stream).Wait();
                        }
                        product.Image = fileName;

                        await ProductService.UpdateAsync(product);
                        NotyfService.Success("Product Edited");
                    }
                    
                }
                else
                {
                    if(mode == "Edit")
                    {
                        await ProductService.UpdateAsync(product);
                        NotyfService.Success("Product Edited");
                    }
                    else
                    {
                        await ProductService.AddAsync(product);
                        NotyfService.Success("New Product Added");

                    }
                }
                return Redirect("/");
            }
            ViewBag.mode = mode;

            return View(product);

        }

        public async Task<RedirectResult> DeleteProduct(int Id)
        {
            await ProductService.RemoveAsync(Id);
            NotyfService.Error("Product Removed");
            return (Redirect("/"));
        }

        public async Task<IActionResult> InsectionForm(string companyName)
        {
            double salesTotal = 0;
            var insections = await InsectionService.GetAsync(companyName); 
            var sales = await SaleService.GetAsync(companyName);
            if (insections.Count != 0)
            {
                salesTotal = insections.Sum(s => s.SalesTotal);
                salesTotal = sales.Sum(s => s.CashAmount) - salesTotal;
            }
            else 
            {
                salesTotal = sales.Sum(s => s.CashAmount);
            }
            ViewBag.page = "Insections";
            ViewBag.salesTotal = salesTotal;    
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> InsectionForm(Insection insection)
        {
            ViewBag.page = "Insections";
            if (ModelState.IsValid)
            {
                var insections = await InsectionService.GetAsync(insection.CompanyName);
                double carryOver = 0;
                //determine the carry Over of the last insections
                if(insections.Count != 0)
                {
                    carryOver = insections.Last().CarryOver;
                }
                insection.CarryOver = carryOver + insection.Amount - insection.SalesTotal - insection.ExpensesAmount;
                await InsectionService.AddAsync(insection);
                NotyfService.Success("Insection Successfull");
                return Redirect("/home/insections");
            }
            ViewBag.salesTotal = insection.SalesTotal;
            NotyfService.Error("Insection Failed");
            return View(insection);
        }
        public IActionResult Insections()
        {
            ViewBag.page = "Insections";
            return View();
        }

        
        public IActionResult AquisitionForm()
        {
            ViewBag.page = "Acquisitions";
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AquisitionForm(Aquisition aquisition)
        {
            ViewBag.page = "Acquisitions";
            if (ModelState.IsValid)
            {
                aquisition.Weight = aquisition.Weight.Replace(".",",");
                aquisition.PurchasePrice = aquisition.PurchasePrice.Replace(".", ",");
                await this.AquisitionService.AddAsync(aquisition);
                NotyfService.Success("Acquisition Successfull");
                return Redirect("/home/Aquisitions");
            }
            NotyfService.Error("Acquisition Failed");
            return View(aquisition);
        }
        public IActionResult Aquisitions()
        {
            ViewBag.page = "Acquisitions";
            return View();
        }
        public IActionResult DisposalForm()
        {
            ViewBag.page = "Disposals";
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DisposalForm(Disposal disposal)
        {
            ViewBag.page = "Disposals";
            if (ModelState.IsValid)
            {
                await DisposalService.AddAsync(disposal);
                return Redirect("/home/Disposals");
            }
            return View(disposal);
        }
        public IActionResult Disposals()
        {
            ViewBag.page = "Disposals";
            return View();
        }
        public IActionResult Employees()
        {
            ViewBag.page = "Employees";
            return View();
        }
        public IActionResult EmployeeForm()
        {
            ViewBag.page = "Employees";
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EmployeeForm(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                //check if the passwords are matching
                if(employee.Password != employee.ConfirmPassword)
                {
                    ModelState.AddModelError(string.Empty , "Passwords must be the same");
                    return View(employee);
                }
                AppUser user = new AppUser()
                {
                    UserName = employee.Username,
                    CompanyName = employee.CompanyName
                };
                var result =  await UserManager.CreateAsync(user, employee.Password);
                //check if there are errors
                if (!result.Succeeded)
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty , error.Description);
                    }
                    return View(employee);
                }
                await UserManager.AddToRoleAsync(user, "Employee");
                NotyfService.Success("New User Added");
                return Redirect("/home/employees");
            }
            NotyfService.Error("User could not be added");
            return View(employee);
        }
        public async Task<IActionResult> Editor(string Id)
        {
			ViewBag.page = "Profile";
			var user = await UserManager.FindByIdAsync(Id);
            return View(new ProfileViewModel
            {
                Username = user!.UserName,
                Id = user.Id,
            });
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Editor(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
               var user = await UserManager.FindByIdAsync(model.Id);
                bool passwordChanged = model.Password != null && model.ConfirmPassword != null;
                bool usernameChanged = model.Username != user!.UserName;
                if(!passwordChanged && !usernameChanged)
                {
					return Redirect("/");
                }
                if (passwordChanged)
                {
					if (model.Password != model.ConfirmPassword)
					{
						ModelState.AddModelError(string.Empty, "Passwords must be the same");
						ViewBag.page = "Profile";
						return View(model);
					}
                    await UserManager.RemovePasswordAsync(user);
                    var result =  await UserManager.AddPasswordAsync(user, model.Password);
                    if (!result.Succeeded)
                    {
                        foreach(var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty,error.Description);
                        }
						ViewBag.page = "Profile";
						return View(model);
                    }
				}
                if(usernameChanged)
                {
                    user.UserName = model.Username;
                }
                var res = await UserManager.UpdateAsync(user);
				if (!res.Succeeded)
				{
					foreach (var error in res.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
					ViewBag.page = "Profile";
					return View(model);
				}

                await SignInManager.SignOutAsync();
                NotyfService.Success("Profile Successfully Edited");
                return Redirect("/");

			}
			ViewBag.page = "Profile";
			return View(model);
        }
    }
    
}
