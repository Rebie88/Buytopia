
$(document).ready(function () {

    if ($(window).width() <= 768) {
        $('.footer-links-wrapper').addClass("collapseClass");
    } else {
        $('.footer-links-wrapper').removeClass("collapseClass");
        $('.footer-links-wrapper ul').show();
    }
    $(window).on('resize', function (event) {
        if ($(window).width() <= 768) {
            $('.footer-links-wrapper').addClass("collapseClass");
            $('.footer-links-wrapper ul').hide();

        } else {
             $('.footer-links-wrapper').removeClass("collapseClass");
            $('.footer-links-wrapper h3').removeClass("addXToClose");

            $('.footer-links-wrapper ul').show();
        }
    });
    $(document).on("click", ".collapseClass h3", function () {
        $(this).next('ul').slideToggle();
        $(this).toggleClass("addXToClose");
    });

    /*------------------------
   Scroll to top
-------------------------- */
    $(function () {
        $(window).on('scroll', function () {
            if ($(this).scrollTop() > 400) {
                $('#back-to-top').fadeIn();
            } else {
                $('#back-to-top').fadeOut();
            }
        });
    });
    $('#back-to-top').on("click", function () {
        $('html, body').animate({ scrollTop: 0 }, 'slow');
        return false;
    });

});