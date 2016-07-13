$(function() {
    $('#search').on('keyup', function() {
        var pattern = $(this).val();
        $('.searchable-container .items').hide();
        $('.searchable-container .items').filter(function() {
            return $(this).text().match(new RegExp(pattern, 'i'));
        }).show();
    });
});


$(document).ready(function(){
    $('[data-toggle="tooltip"]').tooltip(); 
});

/*Смена фильтров для разных категорий*/
function change_filters(id) {
    $('#hid_name').val(id);
    $('#sub').click();
    $("#category_id").val(id);
    $("#filters_id").val("");
}
function add_remove_filter(id) {
    if($("#filters_id").val().indexOf(id) + 1 == 0)
    {
        $("#filters_id").val($("#filters_id").val() + id + ";");
    }
    else
    {
        var ids = $("#filters_id").val().split(';');
        var new_ = "";
        $.each(ids, function (i) {
            if (ids[i] != id.toString() && ids[i] != "")
                new_ += ids[i] + ";";
        })
        $("#filters_id").val(new_);
    }
}


function give_result(){
    $('#give_result_sub').click();
}

/*Плавный скроллинг*/


/*РОМА. ГАВНОКОД!!! Сделай по человечески. Код повторяется!!!*/
$('#but_go').click(function(){
       $('html, body').animate({scrollTop:$('#choose_cat').position().top}, 1500);
});
$('#but_filter').click(function(){
       $('html, body').animate({scrollTop:$('#sum').position().top}, 1500);
});
$('#sum_but').click(function(){
       $('html, body').animate({scrollTop:$('#result').position().top}, 1500);
});



