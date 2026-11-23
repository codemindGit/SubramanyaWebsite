using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SubramanyaWebsite.Models;
using SubramanyaWebsite.Services;

namespace SubramanyaWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailQueue _emailQueue;

        private static readonly IReadOnlyDictionary<string, ServicePage> ServicePages = new Dictionary<string, ServicePage>(StringComparer.OrdinalIgnoreCase)
        {
            ["home-nursing"] = new()
            {
                Slug = "home-nursing", Title = "Home Nursing", Eyebrow = "Skilled care, in familiar surroundings", Icon = "fa-solid fa-user-nurse",
                Summary = "Thoughtful nursing support at home, helping patients and families feel more confident through recovery and ongoing care.",
                Image = "~/images/home-nursing.png", ImageAlt = "A registered nurse offering attentive care in a comfortable home",
                IntroTitle = "Professional support, with home at the heart of it.",
                Intro = "When a loved one needs extra support, familiar surroundings can make a meaningful difference. Our team can help you explore nursing and patient-care options at home, based on the person's needs and your family's preferences.",
                Features = ["Nursing support at home", "Post-hospital recovery assistance", "Daily living and bedside support", "Help discussing care requirements"],
                Benefits = ["Care plans discussed around your family", "Local coordination in Belagavi", "A clear point of contact", "Support available around the clock"]
            },
            ["elder-care"] = new()
            {
                Slug = "elder-care", Title = "Old Age Care", Eyebrow = "Respect, companionship and everyday support", Icon = "fa-solid fa-person-cane",
                Summary = "Personalised support for older adults, helping them feel safe, respected and connected in the place they know best.",
                Image = "~/images/elder-care.png", ImageAlt = "A caregiver spending a warm, attentive moment with an older adult",
                IntroTitle = "Care that honours a lifetime of independence.",
                Intro = "Every older adult has their own routines, preferences and story. We help families explore dependable support for day-to-day living, companionship and changing care needs—always starting with a thoughtful conversation.",
                Features = ["Companionship and attentive presence", "Everyday routine assistance", "Help with mobility and comfort", "Family communication and coordination"],
                Benefits = ["Dignity and preferences come first", "Support tailored to daily routines", "Reassurance for family members", "Local Belagavi care coordination"]
            },
            ["ambulance"] = new()
            {
                Slug = "ambulance", Title = "Ambulance Support", Eyebrow = "Help coordinating a patient transfer", Icon = "fa-solid fa-truck-medical",
                Summary = "Speak with our Belagavi team about ambulance and patient transfer needs, including the details that matter for your situation.",
                Image = "~/images/home-nursing.png", ImageAlt = "Healthcare professionals coordinating patient care",
                IntroTitle = "A calmer way to arrange the next step.",
                Intro = "When a patient needs transport, families often have questions about timing, destination and the support required. Contact our team to discuss current availability and the type of transfer you need.",
                Features = ["Discuss patient transfer requirements", "Share pickup and destination details", "Coordinate with our local team", "Ask about current service availability"],
                Benefits = ["Direct conversation with a local coordinator", "Clear details before proceeding", "Support for families during transitions", "Call to discuss urgent requirements"]
            },
            ["medical-equipment-rental"] = new()
            {
                Slug = "medical-equipment-rental", Title = "Surgical Material & Equipment Rental", Eyebrow = "Practical support for care at home", Icon = "fa-solid fa-bed-pulse",
                Summary = "Ask about surgical materials and medical equipment available on rent to support recovery and care at home.",
                Image = "~/images/home-nursing.png", ImageAlt = "A calm home environment prepared for patient care",
                IntroTitle = "The right equipment can make home care easier.",
                Intro = "Recovery may call for equipment or supplies that are only needed for a limited period. Talk with us about what is required, the expected duration and availability in your area.",
                Features = ["Discuss equipment and material needs", "Ask about rental availability", "Share expected duration of use", "Coordinate delivery details with our team"],
                Benefits = ["A practical alternative to purchasing", "Guidance from a local coordinator", "Options discussed around your needs", "Availability confirmed before arranging"]
            },
            ["hospital-staffing"] = new()
            {
                Slug = "hospital-staffing", Title = "Staffing for Hospitals", Eyebrow = "People who help care teams care", Icon = "fa-solid fa-people-group",
                Summary = "Healthcare staffing support for hospitals and care facilities seeking dependable people for patient-centred care.",
                Image = "~/images/home-nursing.png", ImageAlt = "A healthcare professional ready to support a care team",
                IntroTitle = "A staffing conversation built around your facility.",
                Intro = "Every facility has its own patient mix, staffing requirements and operating rhythms. Get in touch to discuss your requirements and the roles, schedules and experience you are looking for.",
                Features = ["Discuss staffing needs and roles", "Share shift and schedule requirements", "Describe experience and skills needed", "Coordinate next steps with our team"],
                Benefits = ["Requirements discussed directly", "Local coordination in Belagavi", "Clear communication about availability", "Support for hospitals and care facilities"]
            },
            ["patient-visits"] = new()
            {
                Slug = "patient-visits", Title = "Patient Visits", Eyebrow = "A helpful first conversation at home", Icon = "fa-solid fa-house-medical-circle-check",
                Summary = "Arrange a patient visit to discuss the kind of care and practical support that could help your family at home.",
                Image = "~/images/elder-care.png", ImageAlt = "A care professional listening to a patient at home",
                IntroTitle = "Start by understanding what would help.",
                Intro = "A visit can help families talk through day-to-day challenges, recovery needs and possible next steps. Contact us to share your location and ask about visit availability in Belagavi.",
                Features = ["Discuss the patient's current needs", "Share home and location details", "Explore possible support options", "Ask our team about scheduling"],
                Benefits = ["A person-to-person conversation", "Support centred on your situation", "Clear next steps for your family", "Belagavi-area coordination"]
            }
        };

        public HomeController(ILogger<HomeController> logger, IEmailQueue emailQueue)
        {
            _logger = logger;
            _emailQueue = emailQueue;
        }

        public IActionResult Index() => View();

        [HttpGet("services/{slug}")]
        public IActionResult Service(string slug)
        {
            if (!ServicePages.TryGetValue(slug, out var service)) return NotFound();
            return View(service);
        }

        public IActionResult Privacy() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BookCare(CareRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Website))
            {
                return Json(new { success = true, message = "Thank you. Your request has been received." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Please check the form details and try again." });
            }

            if (!_emailQueue.TryEnqueue(request))
            {
                return StatusCode(500, new { success = false, message = "We could not accept your request right now. Please call +91 96325 54468 or +91 97392 113907." });
            }

            _logger.LogInformation("Care request accepted for background email delivery from {RequestEmail}.", request.Email);
            return Json(new { success = true, message = $"Thank you {request.Name}! Your request has been received. Our care coordinator will contact you within 15 minutes." });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
