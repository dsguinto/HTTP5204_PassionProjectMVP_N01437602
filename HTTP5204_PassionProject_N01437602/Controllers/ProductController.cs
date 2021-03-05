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
    public class ProductController : Controller
    {

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static ProductController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);

            client.BaseAddress = new Uri("https://localhost:44302/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        // GET: Product/List
        public ActionResult List(string search)
        {
            string url = "productdata/getproducts";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ProductDto> SelectedProduct = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;
                return View(search == null ? SelectedProduct :
                    SelectedProduct.Where(x => x.ProductName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList());
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            ShowProduct ViewModel = new ShowProduct();
            string url = "productdata/findproduct/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                //Puts data into product DTO
                ProductDto SelectedProduct = response.Content.ReadAsAsync<ProductDto>().Result;
                ViewModel.product = SelectedProduct;

                //Finds user that owns product, buts data into user DTO
                url = "productdata/finduserforproduct/" + id;
                response = client.GetAsync(url).Result;
                UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;
                ViewModel.user = SelectedUser;

                return View(ViewModel);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            UpdateProduct ViewModel = new UpdateProduct();

            return View(ViewModel);
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product ProductInfo)
        {
            Debug.WriteLine(ProductInfo.ProductName);
            string url = "productdata/addproduct";
            Debug.WriteLine(jss.Serialize(ProductInfo));
            HttpContent content = new StringContent(jss.Serialize(ProductInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                int productid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = productid });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateProduct ViewModel = new UpdateProduct();

            string url = "productdata/findproduct/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                ProductDto SelectedProduct = response.Content.ReadAsAsync<ProductDto>().Result;
                ViewModel.product = SelectedProduct;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Product ProductInfo, HttpPostedFileBase ProductPic)
        {
            Debug.WriteLine(ProductInfo.ProductName);
            string url = "productdata/updateproduct/" + id;
            Debug.WriteLine(jss.Serialize(ProductInfo));
            HttpContent content = new StringContent(jss.Serialize(ProductInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);

            if (response.IsSuccessStatusCode)
            {

                url = "productdata/updateproductpic/" + id;

                if (ProductPic == null)
                {
                    return RedirectToAction("Details", new { id = id });
                }
                Debug.WriteLine("Received product picture " + ProductPic.FileName);
                //Error when NULL

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(ProductPic.InputStream);
                requestcontent.Add(imagecontent, "ProductPic", ProductPic.FileName);
                response = client.PostAsync(url, requestcontent).Result;


                return RedirectToAction("Details", new { id = id });

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Product/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "productdata/findproduct/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                ProductDto SelectedProduct = response.Content.ReadAsAsync<ProductDto>().Result;
                return View(SelectedProduct);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "productdata/deleteproduct/" + id;

            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
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
