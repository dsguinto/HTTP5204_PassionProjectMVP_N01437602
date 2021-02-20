using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using HTTP5204_PassionProject_N01437602.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace HTTP5204_PassionProject_N01437602.Controllers
{
    public class UserController : Controller
    {

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static UserController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);

            client.BaseAddress = new Uri("https://localhost:44302/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }


        // GET: User/List
        public ActionResult List()
        {
            string url = "userdata/getusers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<UserDto> SelectedUser = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;
                return View(SelectedUser);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        
        // GET: User/Details/5
        public ActionResult Details(int id)
        {

            ShowUser ViewModel = new ShowUser();
            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            if (response.IsSuccessStatusCode)
            {
                //Puts data into user DTO
                UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;
                ViewModel.user = SelectedUser;

                url = "userdata/getproductsforuser/" + id;
                response = client.GetAsync(url).Result;
                Debug.WriteLine(response.StatusCode);
                IEnumerable<ProductDto> SelectedProduct = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;
                ViewModel.userproducts = SelectedProduct;

                return View(ViewModel);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: User/Create
        public ActionResult Create()
        {
            UpdateUser ViewModel = new UpdateUser();

            return View(ViewModel);
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User UserInfo)
        {
            Debug.WriteLine(UserInfo.UserFirstName);
            string url = "userdata/adduser";
            Debug.WriteLine(jss.Serialize(UserInfo));
            HttpContent content = new StringContent(jss.Serialize(UserInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            
            if (response.IsSuccessStatusCode)
            {
                int userid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = userid });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateUser ViewModel = new UpdateUser();

            string url = "userdata/finduser/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;
                ViewModel.user = SelectedUser;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }       


        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, User UserInfo, HttpPostedFileBase UserPic)
        {
            Debug.WriteLine(UserInfo.UserFirstName);
            string url = "userdata/updateuser/" + id;
            Debug.WriteLine(jss.Serialize(UserInfo));
            HttpContent content = new StringContent(jss.Serialize(UserInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);

            if (response.IsSuccessStatusCode)
            {

                url = "userdata/updateuserpic/" + id;
                
                if (UserPic == null)
                {
                    return RedirectToAction("Details", new { id = id });
                }
                Debug.WriteLine("Received user picture " + UserPic.FileName);
                //Error when NULL

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(UserPic.InputStream);
                requestcontent.Add(imagecontent, "UserPic", UserPic.FileName);
                response = client.PostAsync(url, requestcontent).Result;
                

                return RedirectToAction("Details", new { id = id });
                
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        
        // GET: User/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;
                return View(SelectedUser);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "userdata/deleteuser/" + id;

            
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
