$(document).ready(function () {
    // Xử lý click vào các nút điều hướng
    $('.nav-categories .nav-link').on('click', function (e) {
        // Loại bỏ lớp 'active' từ tất cả các nút
        $('.nav-categories .nav-link').removeClass('active');
        
        // Thêm lớp 'active' vào nút được click
        $(this).addClass('active');
    });

    // Xử lý khi scroll đến các phần
    $(window).on('scroll', function() {
        var nowShowingTop = $('#now-showing').offset().top - 100;
        var comingSoonTop = $('#coming-soon').offset().top - 100;
        var scrollPosition = $(window).scrollTop();

        // Cập nhật lớp 'active' dựa trên vị trí scroll
        if (scrollPosition >= comingSoonTop) {
            $('.nav-categories .nav-link').removeClass('active');
            $('.nav-categories .nav-link[href="#coming-soon"]').addClass('active');
        } else if (scrollPosition >= nowShowingTop) {
            $('.nav-categories .nav-link').removeClass('active');
            $('.nav-categories .nav-link[href="#now-showing"]').addClass('active');
        }
    });

    // Xử lý khi click vào link để cuộn mượt đến phần tương ứng
    $('.nav-categories .nav-link').on('click', function (e) {
        e.preventDefault();
        
        var target = $(this).attr('href');
        $('html, body').animate({
            scrollTop: $(target).offset().top - 80
        }, 100);
    });
});