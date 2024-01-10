using ClinicManagement.Models;
using ClinicManagement.Repositories;
using ClinicManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Controllers
{
    [Authorize]
    public class AppointmentsController :  BaseController
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        public AppointmentsController(IDoctorRepository doctorRepository, IAppointmentRepository appointmentRepository)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
        }
   
        public IActionResult Index(int? page, int? entries, string search)
        {

            var appointments = _appointmentRepository.GetAppointments();
            var (paginatedAppointments, totalAppointments) = ApplyPagination(appointments, page, entries);
            // Create a view model to pass data to the view
            var viewModel = new AppointmentsViewModel
            {
                Appointments = paginatedAppointments.ToList(),                            // List of patients for the current page
                SelectedEntries = entries,                      // Number of entries selected by the user
                CurrentPage = page ?? 1,                      // Current page being displayed
                TotalPages = (int)Math.Ceiling((double)totalAppointments / (entries ?? 10)),  // Total number of pages
                                                                                          //SearchTerm = search
            };
            // Return the view with the populated view model
            return View(viewModel);
        }

        public IActionResult Create(int id)
        {
            var viewModel = new AppointmentFormViewModel
            {
                SelectedPatientId = id,
                Doctors = _doctorRepository.GetAll(),
                Heading = "New Appointment"
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppointmentFormViewModel viewModel)
        {
         
            if (!ModelState.IsValid)
            {
                viewModel.Doctors = _doctorRepository.GetAll();
                return View(viewModel);
               
            }
            var appointment = new Appointment()
            {
                StartDateTime = viewModel.GetStartDateTime(),
                Detail = viewModel.Detail,
                Status = false,
                PatientId = viewModel.SelectedPatientId,
   
                Doctor = _doctorRepository.Get(viewModel.SelectedDoctorId)

            };
            //Check if the slot is available
            if (_appointmentRepository.ValidateAppointment(appointment.StartDateTime, viewModel.SelectedDoctorId))
                return View("InvalidAppointment");
            _appointmentRepository.Add(appointment);
            return RedirectToAction("Index", "Appointments");
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Appointment appointment = _appointmentRepository.GetAppointment(id);
            var doctors = _doctorRepository.GetAvailableDoctors();
            AppointmentFormViewModel viewModel = new AppointmentFormViewModel
            {

                Heading = "New Appointment",
                Id = appointment.Id,
                Status = appointment.Status,
                PatientId = appointment.PatientId,
                Time= appointment.StartDateTime.ToString("HH:mm"),
                Date = appointment.StartDateTime.ToString("dd MMM yyyy"),
                Detail = appointment.Detail,
                Doctors = doctors,
                SelectedDoctorId = appointment.DoctorId,

            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Edit(AppointmentFormViewModel model)
        {
            // Check if the provided data is valid, if not rerender the edit view
            // so the user can correct and resubmit the edit form
            if (ModelState.IsValid)
            {
                // Retrieve the appointment being edited from the database
                Appointment appointment = _appointmentRepository.GetAppointment(model.Id);
                // Update the appointment object with the data in the model object
                appointment.Id = model.Id;
                appointment.Status = model.Status;
                appointment.PatientId = model.  PatientId;
                appointment.Detail = model.Detail;
                appointment.StartDateTime = model.GetStartDateTime();
                appointment.DoctorId = model.SelectedDoctorId;
                // Call update method on the repository service passing it the
                // Appointment object to update the data in the database table
                Appointment updatedAppointment = _appointmentRepository.Update(appointment);

                return RedirectToAction("index");
            }

            return View(model);
        }
    }
}
