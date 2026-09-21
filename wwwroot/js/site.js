// Ajoute une classe visible lorsque la section est dans la vue
const sections = document.querySelectorAll('.section');

const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.classList.add('visible');
        }
    });
}, { threshold: 0.1 });

sections.forEach(section => {
    observer.observe(section);
});

// Changement de style de la navbar au scroll
window.addEventListener('scroll', () => {
    const navbar = document.querySelector('.navbar');
    if (window.scrollY > 50) {
        navbar.classList.add('scrolled');
    } else {
        navbar.classList.remove('scrolled');
    }
});


// Ajuste le padding du contenu principal en fonction de la hauteur du footer/
function adjustMainPadding() {
    const footer = document.querySelector('.footer');
    const main = document.querySelector('main');
    if (footer && main) {
        const footerHeight = footer.offsetHeight; // Calcule la hauteur du footer
        main.style.paddingBottom = `${footerHeight}px`; // Applique le padding
    }
}

// Exécute la fonction au chargement de la page et lors du redimensionnement de la fenêtre
window.addEventListener('load', adjustMainPadding);
window.addEventListener('resize', adjustMainPadding);



$(document).ready(function () {
    // Initialisation du slider
    $('.slider').slick({
        autoplay: true, // Défilement automatique
        autoplaySpeed: 1000, // Vitesse de défilement (3 secondes)
        dots: true, // Affiche les points de pagination
        arrows: true, // Affiche les flèches de navigation
        infinite: true, // Défilement infini
        speed: 500, // Vitesse de l'animation
        slidesToShow: 1, // Nombre de slides visibles
        slidesToScroll: 1 // Nombre de slides à faire défiler
    });
});