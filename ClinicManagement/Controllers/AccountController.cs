using ClinicManagement.Models;
using ClinicManagement.Repositories;
using ClinicManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Controllers
{
    [Authorize] // Ensure that only authenticated users can access this controller
    public class AccountController : BaseController
    {
        // Dependency injection for required services
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ISpecializationRepository _specializationRepository;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IDoctorRepository doctorRepository,
            ISpecializationRepository specializationRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _doctorRepository = doctorRepository;
            _specializationRepository = specializationRepository;
        }

        // Helper method to create a role if it doesn't exist and assign it to a user
        private async Task<IdentityResult> CreateRoleAndAssignToUser(ApplicationUser user, string roleName)
        {
            //var role = new IdentityRole(roleName);

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            return await _userManager.AddToRoleAsync(user, roleName);
        }

        // Helper method to sign in a user and redirect to the home page
        private async Task<IActionResult> SignInAndRedirect(ApplicationUser user)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous] // Allow access to the Register page for non-authenticated users
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Copy data from RegisterViewModel to IdentityUser
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                // Store user data in AspNetUsers database table
                var result = await _userManager.CreateAsync(user, model.Password);

                // If user is successfully created, sign in the user and redirect to the home page
                if (result.Succeeded)
                {
                    await CreateRoleAndAssignToUser(user, "Admin");
                    await SignInAndRedirect(user);
                }
                // If there are any errors, add them to the ModelState object for display
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Sign out the current user and redirect to the login page
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous] // Allow access to the Login page for non-authenticated users
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to sign in the user using provided credentials
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, false);

                // If sign-in is successful, redirect to the home page; otherwise, display an error
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(model);
        }

        // Display the Add Doctor page with a dropdown list of specializations
        public IActionResult AddDoctor()
        {
            var viewModel = new AddDoctorViewModel
            {
                Heading = "Add Doctor",
                Specializations = _specializationRepository.GetSpecializations(),
            };

            return View(viewModel);
        }

        // Process the form submission to add a new doctor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoctor(AddDoctorViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(new AddDoctorViewModel
                { Specializations = _specializationRepository.GetSpecializations() });
            }

            // Copy data from AddDoctorViewModel to IdentityUser
            var user = new ApplicationUser
            {
                UserName = viewModel.RegisterViewModel.Name,
                Email = viewModel.RegisterViewModel.Email,
                IsActive = true
            };

            // Store user data in AspNetUsers database table
            var result = await _userManager.CreateAsync(user, viewModel.RegisterViewModel.Password);

            // If user is successfully created, sign in the user, create 'Doctor' role if necessary,
            // assign 'Doctor' role to the user, and add the doctor to the repository
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Doctor"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Doctor"));
                }

                await _userManager.AddToRoleAsync(user, "Doctor");
                await _signInManager.SignInAsync(user, isPersistent: false);

                // Use the ViewModel directly for creating the Doctor
                var doctor = new Doctor
                {
                    // ApplicationUser = user,
                    Name = viewModel.RegisterViewModel.Name,
                    Phone = viewModel.Phone,
                    Address = viewModel.Address,
                    SpecializationId = viewModel.SelectedSpecializationId,
                    IsAvailable = viewModel.IsAvailable
                };

                _doctorRepository.Add(doctor);
            }

            return RedirectToAction("Index", "Doctors");
        }

        // List users with their roles
        public async Task<ActionResult> Index(int? page, int? entries, string search)
        {
            var usersWithRoles = await _userManager.Users.ToListAsync();
            var (paginatedUsers, totalUsers) = ApplyPagination(usersWithRoles, page, entries);

            // Iterate over a copy of the collection to avoid modification during enumeration
            foreach (var user in usersWithRoles.ToList())
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.Roles = string.Join(", ", roles);  // Combine roles into a comma-separated string
                usersWithRoles.Add(user);
            }

            var viewModel = new UsersViewModel
            {
                Users = paginatedUsers.ToList(),
                SelectedEntries = entries,                      // Number of entries selected by the user
                CurrentPage = page ?? 1,                      // Current page being displayed
                TotalPages = (int)Math.Ceiling((double)totalUsers / (entries ?? 10)),  // Total number of pages
            };

            return View(viewModel);
        }
        // Edit users 
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            // GetRolesAsync returns the list of user Roles
            var userRoles = await _userManager.GetRolesAsync(user);
            // Concatenate the roles into a single string, separated by a delimiter (e.g., comma)
            var rolesString = string.Join(",", userRoles);
            var model = new ApplicationUser
            {
                Id = user.Id,
                Email = user.Email,
                Roles = rolesString,
                IsActive = user.IsActive,
               
            };

            return View(model);
        }
       [HttpPost]
        public async Task<IActionResult> EditUser(ApplicationUser model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.IsActive = model.IsActive;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

    }
}
