using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebProject.Autentication;
using Microsoft.AspNetCore.Identity;
using log4net;
using System.Reflection;

namespace WebProject.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //necessary for registration
        private readonly UserManager<StoreUser> userManager;

        //necessary for login\logout
        public SignInManager<StoreUser> Sign { get; }

        public UsersController(
            SignInManager<StoreUser> sign,
            UserManager<StoreUser> userManager)
        {
            Sign = sign;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            //redirect to a page where you can log in without credentials, in case you try to log in without being logged in
            if (User.Identity.IsAuthenticated)
            {
                log.Info($"Info in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nYou are already authenticated");
                return RedirectToAction("About", "Home");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel userViewM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //try to log in with the user entered in the view
                    var result = await Sign.PasswordSignInAsync(userViewM.Username, userViewM.Password, userViewM.RememberMe, false);

                    if (result.Succeeded)
                    {
                        if (Request.Query.Keys.Contains("ReturnUrl"))
                        {
                            Redirect(Request.Query["ReturnUrl"].First());
                        }
                        else
                        {
                            log.Info($"Info in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nYou are logging");
                            return Ok();
                        }

                    }
                    else throw new Exception("Failed to login. Username or password wrong"); 
                }

                throw new Exception("Failed to login. Missing fields");
            }
            catch(Exception e)
            {
                log.Error($"Error in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\n"+e.Message);
                return BadRequest(e.Message);
            }

            
        }


        //execute the logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await Sign.SignOutAsync();
            }
            catch(Exception e)
            {
                log.Error($"Error in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\n" + e.Message);
            }
            log.Info($"Info in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nYou are logout");
            return RedirectToAction("Login", "Users");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(UserDto storeUser)
        {
            try
            {
                if (ModelState.IsValid && storeUser.Password == storeUser.Password2)
                {
                    //search the database for a user already registered with the input email
                    StoreUser user = await userManager.FindByEmailAsync(storeUser.Email);

                    if (user != null)
                        throw new Exception("E-Mail already registered");

                    if (user == null)
                    {
                        //inserts data for registration into storeUser
                        user = new StoreUser()
                        {
                            FirstName = storeUser.FirstName,
                            LastName = storeUser.LastName,
                            Email = storeUser.Email,
                            UserName = storeUser.Username
                        };

                        //register the user
                        var result = await userManager.CreateAsync(user, storeUser.Password);
                        if (result != IdentityResult.Success)
                            throw new InvalidOperationException("Could not create new user");

                    }
                    log.Info($"Info in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nYou are register");
                    return Ok();
                }

                if (storeUser.Password != storeUser.Password2)
                    throw new Exception("Failed to register, the two passwords do not match");
                throw new Exception("Failed to register, Probably some fields are empty or wrong");
                

            }
            catch(Exception e)
            {
                log.Error($"Error in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\n" + e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}