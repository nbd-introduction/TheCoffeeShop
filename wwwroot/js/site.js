$(document).ready(function () {
    // Testimonial slider
    var testimonials = $('.testimonial-item');
    var currentIndex = 0;

    $('.control-next').click(function () {
        testimonials.removeClass('active');
        currentIndex = (currentIndex + 1) % testimonials.length;
        testimonials.eq(currentIndex).addClass('active');
    });

    $('.control-prev').click(function () {
        testimonials.removeClass('active');
        currentIndex = (currentIndex - 1 + testimonials.length) % testimonials.length;
        testimonials.eq(currentIndex).addClass('active');
    });

    // Auto rotate testimonials
    setInterval(function () {
        $('.control-next').click();
    }, 5000);
});