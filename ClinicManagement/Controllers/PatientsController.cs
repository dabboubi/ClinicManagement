using ClinicManagement.Core.Helpers;
using ClinicManagement.Models;
using ClinicManagement.Repositories;
using ClinicManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Controllers
{
    [Authorize]
    public class PatientsController : BaseController
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ICityRepository _cityRepository;
        public PatientsController(IAppointmentRepository appointmentRepository, IPatientRepository patientRepository , ICityRepository cityRepository)
        {
            _patientRepository = patientRepository;
            _cityRepository = cityRepository;
            _appointmentRepository = appointmentRepository;
        }
        public IActionResult Index(int? page, int? entries, string search)
        {

            var patients = _patientRepository.GetAll();
            var (paginatedPatients, totalPatients) = ApplyPagination(patients, page, entries);
            // Create a view model to pass data to the view
            var viewModel = new PatientsViewModel
            {
                Patients = paginatedPatients.ToList(),                            // List of patients for the current page
                SelectedEntries = entries,                      // Number of entries selected by the user
                CurrentPage = page ?? 1,                      // Current page being displayed
                TotalPages = (int)Math.Ceiling((double)totalPatients / (entries ?? 10)),  // Total number of pages
                 //SearchTerm = search
            };
            // Return the view with the populated view model
            return View(viewModel);
        }


        // GET: Patients/Add
        public IActionResult Add()
        {
            // Populate the dropdown list with cities
            var viewModel = new AddPatientViewModel
            {
                Heading = "Add Patient", 
                Cities = _cityRepository.GetCities(),
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddPatientViewModel viewModel)
        {
           
            if (!ModelState.IsValid)
            {
                return View(new AddPatientViewModel());// Return a new instance of the view model
            }
            var patient = new Patient
            {
                Name = viewModel.Name,
                Phone = viewModel.Phone,
                Address = viewModel.Address,
                DateTime = DateTime.Now,
                BirthDate = viewModel.BirthDate,
                Height = viewModel.Height,
                Weight = viewModel.Weight,
                CityId = viewModel.SelectedCityId,
                Sex = (Gender)viewModel.SelectedGenderId
            };
            _patientRepository.Add(patient);
            return RedirectToAction("Index", "Patients");
        }
        public IActionResult Details(int id)
        {

            PatientDetailViewModel viewModel = new PatientDetailViewModel()
            {
                Patient = _patientRepository.Get(id),
                Appointments= _appointmentRepository.GetAppointmentWithPatient(id),
                // Attendances= _attendancesRepository
                CountAppointments= _appointmentRepository.CountAppointments(id),
                Heading = "Details of Patient",

            };
            return View( viewModel);
        }
        [HttpGet]
        public ViewResult Edit(int id)
        {
            Patient patient= _patientRepository.Get(id);
            var cities = _cityRepository.GetCities();
            PatientEditViewModel patientEditViewModel = new PatientEditViewModel
            {
                Id = patient.Id,
                Name = patient.Name,
               
                SelectedGenderId = (int)patient.Sex,
                Address = patient.Address,
                Phone = patient.Phone,
                BirthDate = patient.BirthDate,
                Height = patient.Height,
                Weight = patient.Weight,
                Cities = cities,
                SelectedCityId = patient.CityId,

                ExistingPhotoPath = patient.Phone
            };
            return View(patientEditViewModel);
        }
        [HttpPost]
        public IActionResult Edit(PatientEditViewModel model)
        {
            // Check if the provided data is valid, if not rerender the edit view
            // so the user can correct and resubmit the edit form
            if (ModelState.IsValid)
            {
                // Retrieve the patient being edited from the database
                Patient patient = _patientRepository.Get(model.Id);
                // Update the patient object with the data in the model object
                patient.Name = model.Name;
                patient.Sex = (Gender)model.SelectedGenderId;
                patient.Address = model.Address;
                patient.Phone = model.Phone;
                patient.BirthDate = model.BirthDate;
                patient.Height = model.Height;
                patient.Weight = model.Weight;
                patient.CityId = model.SelectedCityId;
                // Call update method on the repository service passing it the
                // patient object to update the data in the database table
                Patient updatedPatient = _patientRepository.Update(patient);

                return RedirectToAction("index");
            }

            return View(model);
        }
        [HttpPost, ActionName("ConfirmDelete")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int id)
        {
            // Retrieve the patient to be deleted
            Patient patientToDelete = _patientRepository.Get(id);

            if (patientToDelete == null)
            {
                // Optionally handle the case where the patient is not found
                return NotFound();
            }

            // Delete the patient from the repository
            _patientRepository.Remove(id);

            // Redirect to the index action after deletion
            return RedirectToAction("Index");
        }
    }
}

