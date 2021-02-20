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
    public class UserDataController : ApiController
    {
        private StoreDbContext db = new StoreDbContext();

        /// <summary>
        /// Gets a list of users in the database along with status code
        /// </summary>
        /// <returns>A list of users</returns>
        /// <example>
        /// GET: api/UserData/GetUsers
        /// </example>
        [ResponseType(typeof(IEnumerable<UserDto>))]
        public IHttpActionResult GetUsers()
        {
            List<User> Users = db.User.ToList();
            List<UserDto> UserDtos = new List<UserDto> { };

            foreach (var User in Users)
            {
                UserDto NewUser = new UserDto
                {
                    UserID = User.UserID,
                    UserFirstName = User.UserFirstName,
                    UserLastName = User.UserLastName,
                    UserName = User.UserName,
                    UserLocation = User.UserLocation,
                    UserBio = User.UserBio,
                    UserHasPic = User.UserHasPic,
                    UserPicExtension = User.UserPicExtension,
                };
                UserDtos.Add(NewUser);
            }

            //Passes data as 200 status code
            return Ok(UserDtos);
        }

        /// <summary>
        /// Gets a list of products in the database that belongs to a user
        /// </summary>
        /// <param name="id">The user id</param>
        /// <returns>List of products that belongs to user</returns>
        /// <example>
        /// GET: api/UserData/GetProductsForUser
        /// </example>
        [ResponseType(typeof(IEnumerable<ProductDto>))]
        public IHttpActionResult GetProductsForUser(int id)
        {
            List<Product> Products = db.Product.Where(p => p.UserID == id)
                .ToList();
            List<ProductDto> ProductDtos = new List<ProductDto> { };

            foreach (var Product in Products)
            {
                ProductDto NewProduct = new ProductDto
                {
                    ProductID = Product.ProductID,
                    ProductName = Product.ProductName,
                    ProductPrice = Product.ProductPrice,
                    UserID = Product.UserID
                };

                ProductDtos.Add(NewProduct);
     
            }
            return Ok(ProductDtos);
        }
        
        /// <summary>
        /// Finds a single user in database with a 200 status code. Returns 404 status code if user is not found
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <returns>Information about the user (id, username, bio, etc.) </returns>
        /// <example>
        /// GET: api/UserData/FindUser/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult FindUser(int id)
        {
            //Finds data
            User User = db.User.Find(id);
            //If data is not found, returns 404 status code
            if (User == null)
            {
                return NotFound();
            }

            UserDto UserDto = new UserDto
            {
                UserID = User.UserID,
                UserFirstName = User.UserFirstName,
                UserLastName = User.UserLastName,
                UserName = User.UserName,
                UserLocation = User.UserLocation,
                UserBio = User.UserBio,
                UserHasPic = User.UserHasPic,
                UserPicExtension = User.UserPicExtension,
            };

            //Passes data as 200 status code
            return Ok(UserDto);
        }


        /// <summary>
        /// Update a user in the database given information about the user
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="user">A User Object. Received as POST data</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/UserData/UpdateUser/5
        /// FORM DATA: User JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
        /// Gets user picture data, uplaods it to the webserver and udpates user HasPic option
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>status code 200 if successful, 400 if not</returns>

        [HttpPost]
        public IHttpActionResult UpdateUserPic(int id)
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
                    var UserPic = HttpContext.Current.Request.Files[0];

                    if (UserPic.ContentLength > 0)
                    {
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(UserPic.FileName).Substring(1);

                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                string fn = id + "." + extension;

                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Users/"), fn);

                                //Saves image
                                UserPic.SaveAs(path);

                                haspic = true;
                                picextension = extension;

                                //Update user haspic and picexntesion fields in database
                                User SelectedUser = db.User.Find(id);
                                SelectedUser.UserHasPic = haspic;
                                SelectedUser.UserPicExtension = extension;
                                db.Entry(SelectedUser).State = EntityState.Modified;

                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("User Image was not saved succesfully.");
                                Debug.WriteLine("Exception:" + ex);
                            }
                        }
                    }

                }
            }
            return Ok();
        }
        
        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user">A User Object. Sent as Post Form data.</param>
        /// <returns>Status Code 200 if successful, 400 in unsuccessful</returns>
        /// <example>
        /// FORM DATA: User JSON Object
        /// </example>
        [ResponseType(typeof(User))]
        [HttpPost]
        public IHttpActionResult AddUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.User.Add(user);
            db.SaveChanges();

            return Ok(user.UserID);
        }

        
        /// <summary>
        /// Deletes user in the database
        /// </summary>
        /// <param name="id">The id of the user to delete</param>
        /// <returns>200 if successful, 400 in unsuccessful.</returns>
        /// <example>
        /// POST: api/UserData/DeleteUser/5
        /// </example> 
        [HttpPost]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            //Delete image from path
            string path = HttpContext.Current.Server.MapPath("~Content/Users/" + id + "." + user.UserPicExtension);
            if (System.IO.File.Exists(path))
            {
                Debug.WriteLine("Files exsits. Preparing to Delete.");
                System.IO.File.Delete(path);
            }

            db.User.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.User.Count(e => e.UserID == id) > 0;
        }
    }
}