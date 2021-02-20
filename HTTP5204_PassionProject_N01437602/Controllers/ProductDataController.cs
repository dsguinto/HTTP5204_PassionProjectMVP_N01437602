using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HTTP5204_PassionProject_N01437602.Models;
using System.Diagnostics;

namespace HTTP5204_PassionProject_N01437602.Controllers
{
    public class ProductDataController : ApiController
    {
        private StoreDbContext db = new StoreDbContext();

        /// <summary>
        /// Gets a list of products in the database along with status code
        /// </summary>
        /// <returns>Product Listings with user selling product</returns>
        /// <example>
        /// GET: api/ProductData/GetProducts
        /// </example>
        [ResponseType(typeof(IEnumerable<ProductDto>))]
        public IHttpActionResult GetProducts()
        {
            List<Product> Products = db.Product.ToList();
            List<ProductDto> ProductDtos = new List<ProductDto> { };

            foreach (var Product in Products)
            {
                ProductDto NewProduct = new ProductDto
                {
                    ProductID = Product.ProductID,
                    ProductPrice = Product.ProductPrice,
                    ProductName = Product.ProductName,
                    ProductDesc = Product.ProductDesc,
                    ProductCondition = Product.ProductCondition,
                    ProductGender = Product.ProductGender,
                    ProductColour = Product.ProductColour,
                    ProductType = Product.ProductType,
                    ProductSize = Product.ProductSize,
                    ProductHasPic = Product.ProductHasPic,
                    ProductPicExtension = Product.ProductPicExtension,
                    UserID = Product.UserID
                };
                ProductDtos.Add(NewProduct);
    }

            //Passes data as 200 status code
            return Ok(ProductDtos);
        }

        /// <summary>
        /// Get a single product in database with a 200 status code. Returns 404 status code if product is not found
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <returns>Information about the product (description, condition, colour, etc.) </returns>
        /// <example>
        /// GET: api/ProductData/FindProduct/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ProductDto))]
        public IHttpActionResult FindProduct(int id)
        {
            //Finds data
            Product Product = db.Product.Find(id);
            //If data is not found, returns 404 status code
            if (Product == null)
            {
                return NotFound();
            }

            ProductDto ProductDto = new ProductDto
            {
                ProductID = Product.ProductID,
                ProductPrice = Product.ProductPrice,
                ProductName = Product.ProductName,
                ProductDesc = Product.ProductDesc,
                ProductCondition = Product.ProductCondition,
                ProductGender = Product.ProductGender,
                ProductColour = Product.ProductColour,
                ProductType = Product.ProductType,
                ProductSize = Product.ProductSize,
                ProductHasPic = Product.ProductHasPic,
                ProductPicExtension = Product.ProductPicExtension,
                UserID = Product.UserID
            };

            //Passes data as 200 status code
            return Ok(ProductDto);
        }

        /// <summary>
        /// Finds specific user in database given a product ID, returns error if user does not exist
        /// </summary>
        /// <param name="id">The product id</param>
        /// <returns>Info about the user selling the item</returns>
        /// <example>
        /// GET: api/UserData/FindUserForProduct/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult FindUserforProduct(int id)
        {
            //Finds user that is selling products thats matches the input productid
            {
                User User = db.User
                    .Where(t=> t.Product.Any(p => p.ProductID == id))
                    .FirstOrDefault();

                if (User == null)
                {
                    return NotFound();
                }

                UserDto UserDto = new UserDto
                {
                    UserID = User.UserID,
                    UserName = User.UserName
                };

                return Ok(UserDto);

            }

        }

        /// <summary>
        /// Update a product in the database given information about the product
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <param name="product">A Product Object. Received as POST data</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/ProductData/UpdateProduct/5
        /// FORM DATA: Product JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductID)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Gets product picture data, uplaods it to the webserver and udpates product HasPic option
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>status code 200 if successful, 400 if not</returns>

        [HttpPost]
        public IHttpActionResult UpdateProductPic(int id)
        {
            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received:" + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var ProductPic = HttpContext.Current.Request.Files[0];

                    if (ProductPic.ContentLength > 0)
                    {
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(ProductPic.FileName).Substring(1);

                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                string fn = id + "." + extension;

                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Products/"), fn);

                                //Saves image
                                ProductPic.SaveAs(path);

                                haspic = true;
                                picextension = extension;

                                //Update Product haspic and picexntesion fields in database
                                Product SelectedProduct = db.Product.Find(id);
                                SelectedProduct.ProductHasPic = haspic;
                                SelectedProduct.ProductPicExtension = extension;
                                db.Entry(SelectedProduct).State = EntityState.Modified;

                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Product Image was not saved succesfully.");
                                Debug.WriteLine("Exception:" + ex);
                            }
                        }
                    }

                }
            }
            return Ok();
        }

        /// <summary>
        /// Adds a product to the database
        /// </summary>
        /// <param name="product">A product Object. Sent as Post Form data.</param>
        /// <returns>Status Code 200 if successful, 400 in unsuccessful</returns>
        /// <example>
        /// FORM DATA: Product JSON Object
        /// </example>
        [ResponseType(typeof(Product))]
        [HttpPost]
        public IHttpActionResult AddProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Product.Add(product);
            db.SaveChanges();

            return Ok(product.ProductID);
        }


        /// <summary>
        /// Deletes product in the database
        /// </summary>
        /// <param name="id">The id of the product to delete</param>
        /// <returns>200 if successful, 400 in unsuccessful.</returns>
        /// <example>
        /// POST: api/ProductData/DeleteProduct/5
        /// </example> 
        [HttpPost]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            //Delete image from path
            string path = HttpContext.Current.Server.MapPath("~Content/Products/ " + id + "." + product.ProductPicExtension);
            if (System.IO.File.Exists(path))
            {
                Debug.WriteLine("Files exsits. Preparing to Delete.");
                System.IO.File.Delete(path);
            }

            db.Product.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Product.Count(e => e.ProductID == id) > 0;
        }
    }
}