using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Data;
using WebApplication.Dto;
using WebApplication.Models;
using System.Drawing;
using System.Drawing.Imaging;
using WebApplication.Helper;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApplication.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors()]
    public class WebController : ControllerBase
    {
        private readonly IRepo _repo;

        public WebController(IRepo re)
        {
            _repo = re;
        }

        //GetLogo
        [HttpGet("GetLogo")]
        public async Task<ActionResult> GetLogoAsync()
        {
            string path = Directory.GetCurrentDirectory();
            path = Path.Combine(path, "StaffPhotos");
            path = Path.Combine(path, "logo.png");
            string respHeader = "";
            string fileName = "";
            if (System.IO.File.Exists(path))
            {
                respHeader = "image/png";
                fileName = path;
                return PhysicalFile(fileName, respHeader);
            }
            else
            {
                return NotFound();
            }

        }

        //Get All Staffs
        [HttpGet("GetAllStaff")]
        public async Task<ActionResult<IEnumerable<staffDtoOut>>> getAllStaffsAsync()
        {
            IEnumerable<Staff> staffs = await _repo.GetStaffsAsync();
            IEnumerable<staffDtoOut> staffOut = staffs.Select(e => new staffDtoOut
            {
                Id = e.Id,
                LastName = e.LastName,
                FirstName = e.FirstName,
                Email = e.Email,
                Tel = e.Tel,
                Title = e.Title,
                Url = e.Url,
                Research = e.Research.Trim('\"')
            });
            return Ok(staffOut);
        }



        //Get Staff photo
        [HttpGet("GetStaffPhoto/{id}")]
        public async Task<ActionResult> GetStaffPhotoAsync(int id)
        {

            string path = Directory.GetCurrentDirectory();
            path = Path.Combine(path, "StaffPhotos");
            string path1 = Path.Combine(path, id + ".jpg");
            string path2 = Path.Combine(path, "default.png");
            string respHeader = "";
            string fileName = "";

            if (System.IO.File.Exists(path1))
            {

                respHeader = "image/jpeg";
                fileName = path1;
                return PhysicalFile(fileName, respHeader);
            }
            else
            {

                respHeader = "image/png";
                fileName = path2;
                return PhysicalFile(fileName, respHeader);
            }
        }


        //Get Vcard
        [HttpGet("GetCard/{id}")]
        public async Task<ActionResult> GetCardAsync(int id)
        {
            Staff staff = await _repo.GetStaffAsync(id);
            //Console.WriteLine(staff.Research);
            string path = Directory.GetCurrentDirectory();
            string path1 = Path.Combine(path, "StaffPhotos/" + id + ".jpg");
            string path2 = Path.Combine(path, "StaffPhotos/logo.png");
            string photoString, photoType, photoString2, photoType2;
            ImageFormat imageFormat, imageFormat2;//using System.Drawing.Imaging;
            if (staff != null)
            {
                Image image;
                if (System.IO.File.Exists(path1))
                {
                    image = Image.FromFile(path1);
                    imageFormat = image.RawFormat;
                    image = ImageHelper.Resize(image, new Size(100, 100), out photoType);
                    photoString = ImageHelper.ImageToString(image, imageFormat);

                }
                else
                {
                    photoString = "";
                    photoType = "UNKNOWN";

                }

                Image image2 = Image.FromFile(path2);
                imageFormat2 = image2.RawFormat;
                image2 = ImageHelper.Resize(image2, new Size(100, 100), out photoType2);
                photoString2 = ImageHelper.ImageToString(image2, imageFormat2);

                CardOut cardout = new CardOut();
                cardout.N = staff.LastName + ";" + staff.FirstName + ";;" + staff.Title + ";";
                cardout.FN = staff.Title + " " + staff.FirstName + " " + staff.LastName;
                cardout.UID = staff.Id;
                cardout.ORG = "Southern Hemisphere Institue of Technology";
                cardout.Email = staff.Email;
                cardout.TEL = staff.Tel;
                cardout.URL = staff.Url;
                cardout.Categories = staff.Research;
                cardout.Photo = photoString;
                cardout.PhotoType = photoType;

                cardout.Photo2 = photoString2;
                cardout.PhotoType2 = photoType2;


                Response.Headers.Add("Content-Type", "text/vcard");
                return Ok(cardout);
            }
            else
            {
                Image image1 = Image.FromFile(path2);
                imageFormat = image1.RawFormat;
                image1 = ImageHelper.Resize(image1, new Size(100, 100), out photoType2);
                photoString2 = ImageHelper.ImageToString(image1, imageFormat);
                CardOut cardout = new CardOut();
                cardout.FN = "";
                cardout.N = ";;;;";
                cardout.UID = 0;
                cardout.ORG = "";
                cardout.Email = "";
                cardout.TEL = "";
                cardout.URL = "";
                cardout.Categories = "";
                cardout.Photo = "";
                cardout.PhotoType = "UNKNOWN";
                cardout.Photo2 = photoString2;
                cardout.PhotoType2 = photoType2;
                Response.Headers.Add("Content-Type", "text/vcard");
                return Ok(cardout);
            }
        }

        //get items
        [HttpGet("GetItems")]
        public async Task<ActionResult> getAllProductsAsync()
        {
            IEnumerable<Product> products = await _repo.GetProductsAsync();
            IEnumerable<ProductOut> productsOut = products.Select(e => new ProductOut
            {
                Id = e.Id,
                description = e.Description.Trim('\"'),
                name = e.Name,
                price = e.Price
            });
            return Ok(productsOut);

        }

        //get item by name
        [HttpGet("GetItems/{name}")]
        public async Task<ActionResult> getAllProductAsync(string name)
        {
            IEnumerable<Product> products = await _repo.GetProductsAsync();
            IEnumerable<Product> res = products.Where(e =>
            {

                return e.Name.ToLower().Contains(name.ToLower());
            });
            return Ok(res);

        }

        //get item photo
        [HttpGet("GetItemPhoto/{id}")]
        public async Task<ActionResult> GetItemPhotoAsync(int id)
        {
            string path = Directory.GetCurrentDirectory();
            path = Path.Combine(path, "ItemsImages");
            string path1 = Path.Combine(path, "" + id + ".jpg");
            string path2 = Path.Combine(path, "" + id + ".png");
            string path3 = Path.Combine(path, "default.png");
            string respHeader = "";
            string fileName = "";
            if (System.IO.File.Exists(path1))
            {
                respHeader = "image/jpeg";
                fileName = path1;
                return PhysicalFile(fileName, respHeader);
            }
            else if (System.IO.File.Exists(path2))
            {
                respHeader = "image/png";
                fileName = path2;
                return PhysicalFile(fileName, respHeader);
            }
            else
            {
                respHeader = "image/png";
                fileName = path3;
                return PhysicalFile(fileName, respHeader);
            }
        }

        //write comment
        [HttpPost("WriteComment")]
        public async Task<ActionResult> WriteCommentAsync(CommentInput InputComment)
        {
            DateTime a = DateTime.Now;
            TheComment res = new TheComment { Comment = InputComment.Comment, Time = a.ToString(), IP = Request.HttpContext.Connection.RemoteIpAddress.ToString(), Name = InputComment.Name };

            await _repo.AddCommentAsync(res);
            return Ok();
        }


        //Get comments
        [HttpGet("GetComments")]
        public async Task<ContentResult> CommentsAsync()
        {
            string fonta = "font-family:Times New Roman";
            string fontb = " style = \"" + fonta + "\" ";
            IEnumerable<TheComment> Commentst = await _repo.GetTheCommentsAsync();
            List<TheComment> Comments = Commentst.ToList();
            int num = Comments.Count();
            Console.WriteLine(num);
            if (num < 5)
            {

                StringBuilder builder = new StringBuilder();
                for (int i = num - 1; i > -1; i--)
                {
                    builder.Append("<p" + fontb + ">" + Comments[i].Comment + " — " + Comments[i].Name + "</p>");
                }
                string outString = builder.ToString();
                return new ContentResult
                {
                    ContentType = "text/html;charset=UTF-8",

                    Content = outString
                };
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("<p" + fontb + ">" + Comments[Comments.Count() - 1].Comment + " — " + Comments[Comments.Count() - 1].Name + "</p>");
                builder.Append("<p" + fontb + ">" + Comments[Comments.Count() - 2].Comment + " — " + Comments[Comments.Count() - 2].Name + "</p>");
                builder.Append("<p" + fontb + ">" + Comments[Comments.Count() - 3].Comment + " — " + Comments[Comments.Count() - 3].Name + "</p>");
                builder.Append("<p" + fontb + ">" + Comments[Comments.Count() - 4].Comment + " — " + Comments[Comments.Count() - 4].Name + "</p>");
                builder.Append("<p" + fontb + ">" + Comments[Comments.Count() - 5].Comment + " — " + Comments[Comments.Count() - 5].Name + "</p>");

                string outString = builder.ToString();
                return new ContentResult
                {
                    ContentType = "text/html;charset=UTF-8",

                    Content = outString
                };
            }


        }


        [HttpPost("Register")]
        public async Task<ActionResult> UserRegister(UserIn u)
        {
            User existOrNot = await _repo.GetUserAsync(u.UserName);
            if (existOrNot == null)
            {

                User res = new User
                {
                    UserName = u.UserName,
                    Address = u.Address,
                    Password = u.Password
                };
                await _repo.AddUserAsync(res);
                return Ok("\"User successfully registered.\"");
            }

            return Ok("\"Username not available.\"");
        }


        [Authorize(AuthenticationSchemes = "MyAuthentication")]
        [Authorize(Policy = "UserOnly")]
        [HttpGet("GetVersionA")]

        public async Task<ActionResult> GetVersion()
        {

            return Ok("v1");
        }

        [Authorize(AuthenticationSchemes = "MyAuthentication")]
        [Authorize(Policy = "UserOnly")]
        [HttpPost("PurchaseItem")]
        public async Task<ActionResult> PurchaseItem(multIn i)
        {
            ClaimsIdentity ci = HttpContext.User.Identities.FirstOrDefault();
            Claim c = ci.FindFirst("userName");
            string name = c.Value;
            bool validItem = await _repo.GetProductByIDAsync(i.ProductID);
            if (validItem)
            {
                Order res = new Order
                {
                    UserName = name,
                    ProductID = i.ProductID,
                    Quantity = i.Quantity
                };
                await _repo.AddOrderAsync(res);
                return Created("", res);
            }
            return Ok("No exist item");
        }


        [Authorize(AuthenticationSchemes = "MyAuthentication")]
        [Authorize(Policy = "UserOnly")]
        [HttpGet("PurchaseSingleItem/{id}")]
        public async Task<ActionResult> PurchaseSingleItem(int id)
        {
            ClaimsIdentity ci = HttpContext.User.Identities.FirstOrDefault();
            Claim c = ci.FindFirst("userName");
            string name = c.Value;
            bool validItem = await _repo.GetProductByIDAsync(id);
            if (validItem)
            {
                Order res = new Order
                {
                    UserName = name,
                    ProductID = id,
                    Quantity = 1
                };
                await _repo.AddOrderAsync(res);
                return Created("", res);
            }
            return Ok("No exist item");
        }





    }
}
