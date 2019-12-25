using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using WebCuaHangSach.Models;

namespace WebCuaHangSach.Controllers
{
    public class LoginController : ApiController
    {
        private BookContext db = new BookContext();

        [HttpPost]
        public IHttpActionResult Post([FromBody]LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var accounts = db.Accounts.Include(x => x.Role).Where(x => x.UserName == model.UserName && x.Password == model.Password);
                if (accounts.Count() > 0)
                {
                    var acc = accounts.First();
                    var token = TokenManager.GenerateToken(model.UserName);
                    var res = new { token, username = acc.UserName, name = $"{acc.FirstName} {acc.LastName}", role = acc.Role.Name };
                    return Ok(res);
                }
            }
            return BadRequest();
        }
    }
}