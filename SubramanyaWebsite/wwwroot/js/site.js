(() => {
    const menuButton = document.querySelector('.menu-toggle');
    const navigation = document.querySelector('.site-nav');

    if (menuButton && navigation) {
        menuButton.addEventListener('click', () => {
            const isOpen = navigation.classList.toggle('open');
            menuButton.setAttribute('aria-expanded', String(isOpen));
            menuButton.setAttribute('aria-label', isOpen ? 'Close navigation' : 'Open navigation');
            menuButton.innerHTML = isOpen ? '<i class="fa-solid fa-xmark"></i>' : '<i class="fa-solid fa-bars"></i>';
        });

        const servicesToggle = navigation.querySelector('.nav-services-toggle');
        const servicesMenu = navigation.querySelector('#navServicesMenu');
        if (servicesToggle && servicesMenu) {
            servicesToggle.addEventListener('click', () => {
                const isOpen = servicesToggle.getAttribute('aria-expanded') !== 'true';
                servicesToggle.setAttribute('aria-expanded', String(isOpen));
                servicesToggle.closest('.nav-dropdown').classList.toggle('open', isOpen);
            });
        }

        navigation.querySelectorAll('a').forEach((link) => link.addEventListener('click', () => {
            navigation.classList.remove('open');
            menuButton.setAttribute('aria-expanded', 'false');
            menuButton.setAttribute('aria-label', 'Open navigation');
            menuButton.innerHTML = '<i class="fa-solid fa-bars"></i>';
            servicesToggle?.setAttribute('aria-expanded', 'false');
            servicesToggle?.closest('.nav-dropdown').classList.remove('open');
        }));
    }

    const requestedService = new URLSearchParams(window.location.search).get('service');
    const serviceOptions = {
        'home-nursing': 'Home Nursing Care',
        'elder-care': 'Old Age Care',
        ambulance: 'Ambulance Service',
        'medical-equipment-rental': 'Surgical Material Rental',
        'hospital-staffing': 'Hospital Staffing',
        'patient-visits': 'Patient Visit'
    };
    const serviceSelect = document.querySelector('#RequiredService');
    if (serviceSelect && requestedService && serviceOptions[requestedService]) {
        serviceSelect.value = serviceOptions[requestedService];
    }

    window.submitCareForm = async (event) => {
        event.preventDefault();
        const form = event.currentTarget;
        const button = form.querySelector('button[type="submit"]');
        const result = form.querySelector('#formResult');
        button.disabled = true;
        result.textContent = 'Sending your request…';

        try {
            const response = await fetch('/Home/BookCare', {
                method: 'POST',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded', 'X-Requested-With': 'XMLHttpRequest' },
                body: new URLSearchParams(new FormData(form))
            });
            const payload = await response.json().catch(() => ({}));
            if (!response.ok || !payload.success) throw new Error(payload.message || 'We could not send your request. Please call us directly.');
            result.textContent = payload.message || 'Thank you. Our care team will be in touch.';
            form.reset();
        } catch (error) {
            result.textContent = error.message || 'We could not send your request. Please call +91 96325 54468.';
        } finally {
            button.disabled = false;
        }
    };
})();
