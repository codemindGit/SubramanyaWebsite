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
                Slug = "home-nursing", Title = "Home Nursing Care", Eyebrow = "Nursing support in familiar surroundings", Icon = "fa-solid fa-user-nurse",
                Summary = "Explore nursing and bedside support at home for recovery, ongoing care and changing patient needs in Belagavi.",
                Image = "~/images/home_nursing_belagavi_1786088744936.jpg", ImageAlt = "A nurse discussing a patient's care during a home visit",
                IntroTitle = "Support that starts with understanding the person.",
                Intro = "Home nursing brings patient support into the familiar setting of home. The type and schedule of care depend on the patient's condition, the family's needs and professional availability. Our team can discuss the situation and help you understand possible arrangements before anything is confirmed.",
                Features = ["Post-hospitalisation recovery support", "Post-operative assistance at home", "Bedside support for people with limited mobility", "Patient observation and family updates as agreed"],
                Benefits = ["Care options discussed around the patient", "A local point of contact in Belagavi", "Clear communication before arrangements", "Family preferences included in planning"],
                MetaDescription = "Learn about home nursing and recovery support for patients in Belagavi. Discuss care needs, scheduling and availability with Sri Subramanya Home Healthcare.",
                Sections =
                [
                    new() { Title = "Who may benefit from nursing support at home?", Body = "Families often enquire after a hospital stay, surgery or a change in mobility. Home-based support may also be considered for a person who needs assistance with everyday routines or closer observation. The appropriate care and any clinical tasks must be discussed with the care team and confirmed with the responsible healthcare professional.", Points = ["People returning home after hospitalisation", "Patients recovering after an operation", "Bedridden patients who need regular support", "Families who need help coordinating day-to-day care"] },
                    new() { Title = "What to expect from a nursing enquiry", Body = "We begin by listening to what has changed and what support the family is seeking. We can discuss the patient's current care instructions, preferred schedule and location, then explain what may be arranged and what needs confirmation. A request does not guarantee a particular professional or availability.", Points = ["Share the patient's main care needs and mobility", "Mention discharge or follow-up instructions already provided", "Tell us the preferred start date and hours", "Ask about the role, responsibilities and availability before confirming"] }
                ],
                Steps = ["Contact our care team and describe the situation.", "Share the patient's location, care needs and preferred schedule.", "Discuss suitable support and any questions with the coordinator.", "Confirm the arrangement and timing directly with our team."],
                ServiceNote = "This page describes enquiry and coordination support, not a substitute for medical advice. Specific clinical procedures, professional credentials and availability must be confirmed before care is arranged."
            },
            ["elder-care"] = new()
            {
                Slug = "elder-care", Title = "Old Age Care at Home", Eyebrow = "Everyday support with dignity and warmth", Icon = "fa-solid fa-person-cane",
                Summary = "Thoughtful home support for older adults, from daily routines and mobility to companionship and family coordination.",
                Image = "~/images/elderly_care_belagavi_1786088760571.jpg", ImageAlt = "A caregiver offering companionship to an older woman at home",
                IntroTitle = "Care that honours a lifetime of independence.",
                Intro = "The right support can help an older person feel more comfortable in familiar surroundings while giving family members a clearer way to coordinate care. We start with the person's routines, preferences and support needs—not a one-size-fits-all checklist.",
                Features = ["Assistance with everyday activities and routines", "Personal hygiene and grooming support, as agreed", "Mobility assistance and help moving around the home", "Companionship and communication with family"],
                Benefits = ["Respect for the older adult's preferences", "Support discussed around daily routines", "A local contact for family coordination", "Care arrangements based on availability"],
                MetaDescription = "Explore respectful old age care and daily living support at home in Belagavi, including companionship, mobility assistance and family coordination.",
                Sections =
                [
                    new() { Title = "Practical help for everyday life", Body = "Support can be discussed for daily routines such as getting ready, moving safely around the home and maintaining familiar activities. Personal care requirements and the level of assistance are agreed with the family and the older adult, with dignity and privacy at the centre.", Points = ["Daily living and routine assistance", "Grooming and personal hygiene support", "Mobility and comfort assistance", "Help for older adults who spend time alone"] },
                    new() { Title = "Companionship and family connection", Body = "A dependable, respectful presence can make a difference for a senior who lives alone or whose family cannot be present throughout the day. Families can discuss preferred routines, communication expectations and the type of companionship they have in mind. Support for bedridden seniors can also be discussed based on individual needs.", Points = ["Conversation and attentive companionship", "Support for seniors living alone", "Bedside assistance requirements", "Coordination with family members"] }
                ],
                Steps = ["Tell us about the older adult's daily routine and priorities.", "Share mobility, personal care and companionship needs.", "Discuss preferred hours, location and family communication.", "Confirm a suitable arrangement and availability with our team."],
                ServiceNote = "The exact duties, schedule and availability are agreed in advance. Contact our team to discuss the type of assistance required."
            },
            ["ambulance"] = new()
            {
                Slug = "ambulance", Title = "Ambulance & Patient Transportation", Eyebrow = "Patient transfers, thoughtfully coordinated", Icon = "fa-solid fa-truck-medical",
                Summary = "Talk with our Belagavi team about scheduled patient transportation, pickup details and transfer requirements.",
                Image = "~/images/patient-transport.webp", ImageAlt = "A patient transport ambulance outside a hospital",
                IntroTitle = "A clearer way to coordinate a patient transfer.",
                Intro = "Patient transportation may be needed for a hospital discharge, a scheduled appointment or a transfer between facilities. Contact us to discuss the journey, timing and the patient's mobility and support requirements. We will confirm whether the requested service is available before you make plans.",
                Features = ["Discuss hospital discharge transportation", "Coordinate scheduled hospital appointments", "Share hospital-to-hospital transfer details", "Confirm patient assistance and availability before booking"],
                Benefits = ["Pickup and destination discussed together", "A clear booking conversation", "Timing coordinated directly with the team", "Patient needs considered before confirmation"],
                MetaDescription = "Enquire about scheduled ambulance and patient transportation in Belagavi. Confirm vehicle availability, transfer timing and patient assistance requirements with our team.",
                Sections =
                [
                    new() { Title = "Tell us what the journey requires", Body = "When you call, share the pickup address, destination and preferred time. Let the coordinator know whether the patient can sit, needs help with mobility or uses a wheelchair, and mention any other transport considerations. The team will explain what assistance and vehicle availability can be confirmed for the trip.", Points = ["Pickup location and destination", "Preferred date and time", "Patient mobility and transfer needs", "A contact person available on the day"] },
                    new() { Title = "Scheduled patient transport—not emergency response", Body = "This enquiry is for arranging patient transportation and planned transfers. If someone may be experiencing a medical emergency, call the local emergency number (112 in India) or seek immediate help from the nearest emergency service. Do not wait for a scheduled transport enquiry to receive emergency care.", Points = ["Hospital discharge journeys", "Scheduled appointments", "Inter-facility transfer enquiries", "Availability and onboard assistance confirmed case by case"] }
                ],
                Steps = ["Contact our healthcare team to ask about a transfer.", "Share the pickup address, destination and preferred timing.", "Explain the patient's mobility and transportation requirements.", "Confirm vehicle availability, assistance and timing with the coordinator.", "Coordinate the patient transfer with the family and receiving location."],
                ServiceNote = "Do not assume emergency response, BLS/ALS or ICU capability, onboard clinicians or specific medical equipment. Vehicle type and patient assistance must be confirmed directly. For emergencies in India, call 112."
            },
            ["medical-equipment-rental"] = new()
            {
                Slug = "medical-equipment-rental", Title = "Medical Equipment Rental", Eyebrow = "Equipment enquiries for care at home", Icon = "fa-solid fa-bed-pulse",
                Summary = "Ask about medical equipment and surgical instrument rental for home care. Items shown are categories to enquire about—not a confirmed inventory.",
                Image = "~/images/equipment-rental.webp", ImageAlt = "Hospital bed, wheelchair, walker and oxygen concentrator arranged for a rental catalogue",
                IntroTitle = "Find out what may be available for your care needs.",
                Intro = "Some equipment is needed only during recovery or a period of home care. Tell us what item you are looking for, where it will be used and how long you may need it. Our team can check current availability and explain any applicable rental terms before you decide.",
                Features = ["Patient-care and mobility equipment enquiries", "Respiratory and monitoring device enquiries", "Surgical instrument set enquiries", "Availability and rental duration discussed before confirmation"],
                Benefits = ["Ask about a specific item or category", "Availability checked before arrangements", "Rental terms discussed directly", "Local coordination in Belagavi"],
                MetaDescription = "Enquire about medical equipment and surgical instrument rentals in Belagavi. Browse categories to discuss; availability, specifications and rental terms require confirmation.",
                Sections =
                [
                    new() { Title = "How equipment rental enquiries work", Body = "Start with the item or support need recommended by your healthcare professional. Share the expected rental period, location and when you need it. Our team can confirm whether a suitable item is available and explain delivery, collection, deposit, cleaning and maintenance arrangements—if offered—before a rental is agreed.", Points = ["Name the equipment or describe the intended use", "Share expected duration and delivery area", "Confirm specifications and safe-use instructions", "Ask about any delivery, collection or deposit terms"] },
                    new() { Title = "A note about the categories below", Body = "These are examples of equipment categories families commonly ask about. They are not a representation of confirmed stock. Every product, specification, rental term, delivery option and safety arrangement must be verified with our team before publishing or booking as available.", Points = ["No item is reserved until availability is confirmed", "Confirm device specifications and condition before use", "Follow instructions from your clinician or equipment provider", "Discuss cleaning, maintenance and return requirements"] }
                ],
                Steps = ["Contact us with the equipment name or care requirement.", "Share your location and estimated rental duration.", "Ask us to confirm availability, specifications and terms.", "Agree delivery or collection details, if offered, before proceeding."],
                ServiceNote = "Catalogue images are illustrative category imagery, not product listings. Stock, brands, specifications, pricing, deposits, delivery, cleaning and maintenance arrangements are not confirmed on this page; please enquire.",
                EquipmentCategories =
                [
                    new() { Title = "Patient care equipment", Description = "For positioning, comfort and bedside care at home.", Image = "~/images/equipment-rental.webp", ImageAlt = "Adjustable home-care bed and patient equipment", Examples = ["Hospital beds and mattresses", "Manual or electric bed enquiries", "Wheelchairs and patient transfer aids"] },
                    new() { Title = "Mobility support", Description = "Items families may ask about for supported movement around the home.", Image = "~/images/mobility-equipment.webp", ImageAlt = "Walker, walking stick and commode chair arranged for a rental enquiry", Examples = ["Walkers and walking sticks", "Commode chairs", "Other mobility assistance equipment"] },
                    new() { Title = "Respiratory & monitoring", Description = "Ask about respiratory or basic monitoring equipment recommended for the patient.", Image = "~/images/respiratory-equipment.webp", ImageAlt = "Oxygen concentrator, nebulizer and pulse oximeter arranged for an enquiry", Examples = ["Oxygen concentrators", "Nebulisers", "Pulse oximeters and monitoring devices"] },
                    new() { Title = "Surgical instruments", Description = "Discuss the specific instrument or set requested by your care provider.", Image = "~/images/surgical-instruments.webp", ImageAlt = "Surgical instrument set arranged on a sterile clinical drape", Examples = ["Surgical instrument sets", "Procedure-specific instrument enquiries", "Other instruments subject to confirmation"] }
                ]
            },
            ["hospital-staffing"] = new()
            {
                Slug = "hospital-staffing", Title = "Hospital Staff Supply", Eyebrow = "Staffing conversations for care institutions", Icon = "fa-solid fa-people-group",
                Summary = "Discuss role, shift and experience requirements with our team when your hospital or care facility needs staffing support.",
                Image = "~/images/hospital-team.webp", ImageAlt = "A team of healthcare professionals coordinating in a hospital corridor",
                IntroTitle = "A staffing enquiry shaped around your facility.",
                Intro = "Every hospital has distinct patient needs, operating hours and role expectations. We invite healthcare institutions to share the staffing requirement, desired start date and the experience or qualifications they are seeking so the team can discuss possible next steps.",
                Features = ["Nursing staff requirements", "Nursing assistants and patient care assistants", "Ward attendant and support-role enquiries", "Shift, experience and qualification needs discussed"],
                Benefits = ["Direct discussion with the institution", "Role and schedule clarified up front", "Availability communicated before agreement", "Local coordination for Belagavi facilities"],
                MetaDescription = "Hospital staffing support enquiries in Belagavi. Discuss nursing, patient care assistant and ward support roles, shifts and qualifications with Sri Subramanya Home Healthcare.",
                Sections =
                [
                    new() { Title = "Roles and requirements to discuss", Body = "Institutions can enquire about nursing staff, nursing assistants, patient care assistants, ward attendants and other support roles. Share the role description and the qualifications, experience and responsibilities expected. Specific roles and candidate availability are not guaranteed until confirmed by our team.", Points = ["Role and scope of responsibilities", "Required qualifications and experience", "Number of staff and expected start date", "Facility location and points of contact"] },
                    new() { Title = "Shift and staffing coordination", Body = "Tell us about the shift pattern, hours, duration and any handover or facility onboarding requirements. The team can clarify which details can be accommodated, explain the next steps and coordinate communication with your nominated contact.", Points = ["Shift timing and weekly schedule", "Temporary or ongoing requirement", "Experience relevant to the care setting", "Institutional onboarding expectations"] }
                ],
                Steps = ["Contact our team on behalf of your hospital or care facility.", "Share roles, qualifications, experience and shift requirements.", "Discuss location, staffing timeline and coordination needs.", "Confirm availability and agreed terms directly with the team."],
                ServiceNote = "Staffing capacity, qualifications, screening and verification procedures are not claimed on this page. Please confirm roles, candidate credentials and availability with the business."
            },
            ["patient-visits"] = new()
            {
                Slug = "patient-visits", Title = "Doctor & Nurse Home Visits", Eyebrow = "Scheduled visits, coordinated around home", Icon = "fa-solid fa-house-medical-circle-check",
                Summary = "Enquire about a scheduled doctor or nurse visit at home, including follow-up and post-discharge needs.",
                Image = "~/images/doctor-home-visit.png", ImageAlt = "A doctor speaking with an older patient during a scheduled home visit",
                IntroTitle = "A scheduled visit is different from ongoing nursing care.",
                Intro = "A home visit is a time-limited appointment coordinated for a particular need. Continuous or shift-based nursing support is a separate service. Tell us whether you are looking for a doctor consultation enquiry or a scheduled nursing visit, and our team can discuss availability and next steps.",
                Features = ["Doctor home-visit appointment enquiries", "General consultation and follow-up requests", "Post-hospitalisation follow-up enquiries", "Scheduled nurse assessment and support visits"],
                Benefits = ["Visit type clarified from the start", "Address and preferred time coordinated", "Availability confirmed before scheduling", "Separate options for visits and ongoing care"],
                MetaDescription = "Enquire about scheduled doctor and nurse home visits in Belagavi, including follow-up and post-discharge appointments. Visit availability is confirmed by the care team.",
                Sections =
                [
                    new() { Title = "Doctor home visits", Body = "Families may enquire about a doctor visiting at home for a general consultation or follow-up, including after hospital discharge. The team will need to confirm whether a doctor and appointment time are available for your area. A visit is not arranged until availability is confirmed.", Points = ["General consultation enquiries", "Follow-up visit requests", "Post-hospitalisation follow-up enquiries", "Appointment timing and area confirmed directly"] },
                    new() { Title = "Nurse home visits", Body = "A scheduled nurse visit is intended for a specific assessment or agreed nursing support at a particular time. It is different from continuous home nursing, which involves a separate care arrangement. Share any discharge instructions or existing care plan when you enquire.", Points = ["Scheduled assessment and nursing support enquiries", "Follow-up visit coordination", "Support after hospital discharge", "Ongoing nursing shifts discussed separately"] },
                    new() { Title = "Information to share when requesting a visit", Body = "A few details help the team understand your request and check availability. Please do not send highly sensitive medical documents through an unsecured channel; the coordinator can advise what information is needed.", Points = ["Patient's general need and type of professional requested", "Belagavi address and a nearby landmark", "Preferred date and time", "Contact person, callback number and relevant care instructions"] }
                ],
                Steps = ["Contact the team and explain the patient's need.", "Share the home address and preferred visit time.", "Clarify whether you are requesting a doctor or a nurse visit.", "Confirm the professional's availability and appointment details.", "Coordinate the scheduled home visit with the family."],
                ServiceNote = "Doctor and nurse availability, services provided during a visit and clinical scope must be confirmed when scheduling. This service is not an emergency response; for emergencies in India call 112."
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
            ViewData["Description"] = service.MetaDescription;
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
