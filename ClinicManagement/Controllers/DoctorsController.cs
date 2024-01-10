using ClinicManagement.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagement.ViewModels;
using ClinicManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace ClinicManagement.Controllers
{
    [Authorize]
    public class DoctorsController : BaseController
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ISpecializationRepository _specializationRepository;
        public DoctorsController(IDoctorRepository doctorRepository, ISpecializationRepository specializationRepository, IAppointmentRepository appointmentRepository)
        {
            _doctorRepository = doctorRepository;
            _specializationRepository = specializationRepository;
            _appointmentRepository = appointmentRepository;
        }
        public IActionResult Index(int page, int? entries, string search)
        {
            var doctors = _doctorRepository.GetAll();
            //int totalDoctors;
            var (paginatedDoctors, totalDoctors) = ApplyPagination(doctors, page, entries);


            var viewModel = new DoctorsViewModel
            {
                Doctors = paginatedDoctors.ToList(),
                SelectedEntries = entries,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalDoctors / (entries ?? 10)),
                // SearchTerm = search
            };

            return View(viewModel);
        }
        public IActionResult Details(int id)
        {

            DoctorDetailViewModel viewModel = new DoctorDetailViewModel()
            {
                Doctor = _doctorRepository.Get(id),
                UpcomingAppointments = _appointmentRepository.GetTodaysAppointments(id),
                Appointments = _appointmentRepository.GetAppointmentByDoctor(id),
                Heading = "Details of Doctor",

            };
            return View(viewModel);
        }
        [HttpGet]
        public ViewResult Edit(int id)
        {
            var doctor = _doctorRepository.Get(id);
            var specializations = _specializationRepository.GetSpecializations();
            AddDoctorViewModel viewModel = new AddDoctorViewModel
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Specializations= specializations,
                SelectedSpecializationId = doctor.SpecializationId,
                Address = doctor.Address,
                Phone = doctor.Phone,
                IsAvailable = doctor.IsAvailable,
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(AddDoctorViewModel model)
        {
           
            if (ModelState.IsValid)
            {

                var doctor = _doctorRepository.Get(model.Id);
                doctor.Name = model.Name;
                doctor.Address = model.Address;
                doctor.Phone = model.Phone;
                doctor.IsAvailable = model.IsAvailable;
                doctor.SpecializationId = model.SelectedSpecializationId;
                Doctor updatedDoctor = _doctorRepository.Update(doctor);

                return RedirectToAction("index");
            }

            return View(model);
        }
    }
}
