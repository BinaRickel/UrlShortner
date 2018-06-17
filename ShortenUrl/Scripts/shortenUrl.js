//$(() => {
//    $(".btn-primary").on('click', function () {
//        console.log("hi");
//        const url = $('#url').val();
//        $.post('/home/shortenUrl', {Realurl: url}, result => {
//            $("#shortened").html(`<a href='${result.shortenedUrl}'>${result.shortenedUrl}</a>`);
//            $("#shortened").slideDown();
//        });
//    });
//});


$(() => {
    $(".btn-primary").on('click', function () {
        const url = $("#url").val();
        $.post('/home/shortenUrl', { RealURL: url}, result => {
            $("#shortened").html(`<a href='${result}'>${result}</a>`);
            $("#shortened").slideDown();
        });
    });
});