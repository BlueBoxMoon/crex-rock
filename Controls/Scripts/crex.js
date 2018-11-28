Sys.Application.add_load(function () {
    $('.crex-posterlist-preview .list a').on('mouseover', function (e) {
        $(this).closest('.list').find('a').removeClass('active');
        $(this).addClass('active');
        $(this).closest('.crex-posterlist-preview').find('.poster-image img').attr('src', $(this).data('image'));
        $(this).closest('.crex-posterlist-preview').find('.detail-left').text($(this).data('detail-left'));
        $(this).closest('.crex-posterlist-preview').find('.detail-right').text($(this).data('detail-right'));
        $(this).closest('.crex-posterlist-preview').find('.description').text($(this).data('description'));
    });

    $('.crex-menu-preview .menu-buttons a').on('mouseover', function (e) {
        $(this).closest('.menu-buttons').find('a').removeClass('active');
        $(this).addClass('active');
    });
});
