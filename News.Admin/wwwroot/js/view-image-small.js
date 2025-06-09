$('#imageModal').on('show.bs.modal', function (event) {
    var img = $(event.relatedTarget);
    var url = img.data('img-url');
    $('#modalImage').attr('src', url);
});