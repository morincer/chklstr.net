(function ($) {
    function refreshLayout() {
        $('.context_check').each(function (index, el) {
            let isChecked = $(el).is(':checked');
            let targetClass = $(el).val();

            if (isChecked) {
                $('.' + targetClass).removeClass("hidden");
            } else {
                $('.' + targetClass).addClass("hidden");
            }
        })

        if ($('#check_descriptions').is(':checked')) {
            $('.description').removeClass("hidden");
        } else {
            $('.description').addClass("hidden");
        }

        let width = $('#text_width').val();
        if (!width) {
            width = '1000';
        }

        let checklistsContainer = $('.checklists-container');

        checklistsContainer.css('width', width + 'px');

        let text_columnsCount = $('#text_columns_count');

        if ($('#check_multicol').is(':checked')) {
            text_columnsCount.prop('disabled', false);
            let count = text_columnsCount.val();
            checklistsContainer
                .css("column-count", count)
                .css("column-gap", '10px')
        } else {
            text_columnsCount.prop('disabled', true);
            checklistsContainer
                .css('column-count', '')
            ;
        }

        let fontSize = $('#text_font_size').val();
        checklistsContainer.css('font-size', fontSize + 'px' )
    }


    $('.layout input').change(function () {
        refreshLayout();
    });

    refreshLayout();
})(jQuery);