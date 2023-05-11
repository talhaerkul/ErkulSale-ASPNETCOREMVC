using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erkulSale.business.Abstract;
using erkulSale.entity;
using erkulSale.webui.Extensions;
using erkulSale.webui.Identity;
using erkulSale.webui.Models;
using erkulSale.webui.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ErkulSale.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private RoleManager<IdentityRole> RoleManager;
        private UserManager<User> UserManager;
        private ICategoryService _categoryService;
        private IProductService _productService;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ICategoryService categoryService, IProductService productService)
        {
            RoleManager = roleManager;
            UserManager = userManager;
            _productService = productService;
            _categoryService = categoryService;
        }
        public IActionResult UserList()
        {
            return View(UserManager.Users);
        }

        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                var selectedRoles = await UserManager.GetRolesAsync(user);
                var roles = RoleManager.Roles.Select(i => i.Name);

                ViewBag.Roles = roles;
                return View(new UserDetailsModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = selectedRoles
                });
            }
            return Redirect("~/admin/userlist");
        }


        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailsModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;

                    var result = await UserManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        var userRoles = await UserManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles ?? new string[] { };
                        await UserManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>());
                        await UserManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>());

                        return Redirect("/admin/userlist");
                    }
                }
                return Redirect("/admin/userlist");
            }

            return View(model);

        }
        public IActionResult RoleList()
        {
            return View(RoleManager.Roles);
        }
        [HttpGet]
        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await RoleManager.CreateAsync(new IdentityRole(model.Name));
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            var members = new List<User>();
            var nonmembers = new List<User>();
            if (role is null)
            {
                role.Name = "";
            }

            foreach (var user in UserManager.Users.ToList())
            {
                var list = await UserManager.IsInRoleAsync(user, role.Name) ? members : nonmembers;
                list.Add(user);
            }
            var model = new RoleDetails() { Role = role, Members = members, NonMembers = nonmembers };

            return View(model);


        }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleDetails model)
        {



            return Redirect("~/admin/roleedit/" + model.Role.Id);
        }
        [HttpPost]
        public async Task<IActionResult> AddToRole(string[] IdsToAdd, string RoleName, string RoleId)
        {
            foreach (var userId in IdsToAdd ?? new string[] { })
            {
                var user = await UserManager.FindByIdAsync(userId);
                if (user is not null)
                {
                    var result = await UserManager.AddToRoleAsync(user, RoleName);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            return Redirect("~/admin/roleedit/" + RoleId);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteFromRole(string[] IdsToDelete, string RoleName, string RoleId)
        {
            foreach (var userId in IdsToDelete ?? new string[] { })
            {
                var user = await UserManager.FindByIdAsync(userId);
                if (user is not null)
                {
                    var result = await UserManager.RemoveFromRoleAsync(user, RoleName);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            return Redirect("~/admin/roleedit/" + RoleId);
        }









        public IActionResult Create()
        {
            ViewBag.Categories = _categoryService.GetAllCategories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductModel productModel, int[] categoryIds, IFormFile file)
        {
            if (true)
            {
                var product = new Product()
                {
                    Name = productModel.Name,
                    Price = productModel.Price,
                    Description = productModel.Description,
                    IsApproved = productModel.IsApproved,
                    IsHome = productModel.IsHome,
                    Url = productModel.Url
                };

                if (file != null)
                {
                    var extention = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{Guid.NewGuid()}{extention}");
                    product.ImgUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                _productService.AddProduct(product, categoryIds);


                TempData.Put("message", new AlertMessage("Bildirme Mesajı", $"{productModel.Name} adlı ürün eklendi.", "success"));


                return RedirectToAction("Update");
            }
            else
            {
                return View(productModel);
            }

        }
        public IActionResult Update(int? id, string q, int page = 1)
        {
            var pageSize = _productService.pageSize();
            var products = _productService.GetAllProducts(page, pageSize);
            if (id != null)
                products = _productService.GetAllProductsByCategoryId((int)id, page, pageSize);
            if (!string.IsNullOrEmpty(q))
                products = _productService.GetApprovedProductsByName(q, page, pageSize);

            var productListViewModel = new ProductListViewModel() { };
            var info = new PageInfo() { CurrentPage = page, ItemsPerPage = pageSize };
            if (id is not null)
            {
                info.TotalItems = _productService.GetCountByCategoryId(id);
                info.CurrentCategoryUrl = _categoryService.GetCategoryById((int)id).Url;
            }
            else
            {
                info.TotalItems = _productService.getCountAllProducts();
                info.CurrentCategoryUrl = "";
            }
            productListViewModel.PageInfo = info;
            productListViewModel.Products = products;


            return View(productListViewModel);
        }

        public IActionResult Edit(int id)
        {
            var product = _productService.GetProductWithCategories(id);
            ProductModel productModel = new ProductModel();
            productModel.Id = product.Id;
            productModel.Name = product.Name;
            productModel.Price = product.Price;
            productModel.Description = product.Description;
            productModel.ImgUrl = product.ImgUrl;
            productModel.IsApproved = product.IsApproved;
            productModel.IsHome = product.IsHome;
            productModel.Url = product.Url;
            productModel.SelectedCategories = product.CategoriesProducts.Select(i => i.Categories).ToList();
            ViewBag.Categories = _categoryService.GetAllCategories();

            return View(productModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductModel productModel, int[] categoryIds, IFormFile file)
        {
            if (true)
            {

                var product = new Product()
                {
                    Id = productModel.Id,
                    Name = productModel.Name,
                    Price = productModel.Price,
                    ImgUrl = productModel.ImgUrl,
                    Description = productModel.Description,
                    IsApproved = productModel.IsApproved,
                    IsHome = productModel.IsHome,
                    Url = productModel.Url
                };

                if (file != null)
                {
                    var extention = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{Guid.NewGuid()}{extention}");
                    product.ImgUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                _productService.UpdateProductMethod(productModel.Id, product, categoryIds);

                TempData.Put("message", new AlertMessage("Bildirme Mesajı", $"{productModel.Name} isimli ürün güncellendi.", "primary"));


                return RedirectToAction("update");
            }
            else
            {
                return View(productModel);
            }

        }
        public IActionResult DeleteList(int? id, string q, int page = 1)
        {
            var pageSize = _productService.pageSize();
            var products = _productService.GetAllProducts(page, pageSize);
            if (id != null)
                products = _productService.GetAllProductsByCategoryId((int)id, page, pageSize);
            if (!string.IsNullOrEmpty(q))
                products = _productService.GetApprovedProductsByName(q, page, pageSize);
            var productListViewModel = new ProductListViewModel() { };
            var info = new PageInfo() { CurrentPage = page, ItemsPerPage = pageSize };
            if (id is not null)
            {
                info.TotalItems = _productService.GetCountByCategoryId(id);
                info.CurrentCategoryUrl = _categoryService.GetCategoryById((int)id).Url;
            }
            else
            {
                info.TotalItems = _productService.getCountAllProducts();
                info.CurrentCategoryUrl = "";
            }
            productListViewModel.PageInfo = info;
            productListViewModel.Products = products;


            return View(productListViewModel);
        }
        public IActionResult Delete(int id)
        {
            var product = _productService.GetProductById(id);
            return View(product);
        }
        public IActionResult DeleteApprove(int? id)
        {
            if (id != null)
            {
                var deletedProductName = _productService.GetProductById((int)id).Name;

                var categories = _categoryService.GetCategoriesByProductId((int)id);
                foreach (var item in categories)
                {
                    _categoryService.DeleteFromCategory((int)id, item.Id);
                }
                _productService.DeleteProduct((int)id);


                TempData.Put("message", new AlertMessage("Bildirme Mesajı", $"{deletedProductName} adlı ürün silindi.", "danger"));


                return RedirectToAction("deletelist");
            }
            else
            {
                return RedirectToAction("update");
            }

        }
        [HttpGet]
        public IActionResult CreateC()
        {

            return View();
        }
        [HttpPost]
        public IActionResult CreateC(CategoryModel model)
        {
            var category = new Category() { Name = model.Name, Url = model.Url };
            _categoryService.AddCategory(category);


            TempData.Put("message", new AlertMessage("Bildirme Mesajı", $"{category.Name} isimli kategori eklendi.", "success"));



            return RedirectToAction("CategoryList");
        }
        public IActionResult CategoryList()
        {
            var categories = _categoryService.GetAllCategories();
            return View(categories);
        }
        [HttpGet]
        public IActionResult UpdateC(int id)
        {
            var category = _categoryService.GetCategoryWithProducts(id);
            if (category == null)
            {
                return NotFound();
            }
            var model = new CategoryModel()
            {
                CategoryId = category.Id,
                Name = category.Name,
                Url = category.Url,
                Products = category.CategoriesProducts.Select(p => p.Products).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateC(CategoryModel model)
        {
            var category = _categoryService.GetCategoryById(model.CategoryId);
            if (category == null)
            {
                return NotFound();
            }
            category.Name = model.Name;
            category.Url = model.Url;

            _categoryService.UpdateCategoryMethod(category.Id, category.Name, category.Url);

            TempData.Put("message", new AlertMessage("Bildirme Mesajı", $"{category.Name} isimli kategori güncellendi.", "success"));


            return Redirect("/Admin/UpdateC/" + model.CategoryId);
        }
        public IActionResult DeleteC()
        {

            return View();
        }



        [HttpPost]
        public IActionResult DeleteFromC(int productId, int categoryId)
        {
            _categoryService.DeleteFromCategory(productId, categoryId);

            return Redirect("/admin/updateC/" + categoryId);
        }
        [HttpPost]
        public IActionResult AddToC(int productId, int categoryId)
        {
            _categoryService.AddProductToCategory(productId, categoryId);

            return Redirect("/admin/updateC/" + categoryId);
        }
    }
}