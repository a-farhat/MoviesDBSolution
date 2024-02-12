
function ShowQuickDetails(id) {
    var url = "/Home/ShowQuickDetails";
    var data = {id: id}
    event.preventDefault();
    SendAjaxPost(url, data, function (response) {
        // Append the response to the DOM
        $('body').append(response);

        // Show the modal after the response is appended
        $('#movieDetailsModal').modal('show');

        // Remove the modal from the DOM when it's hidden
        $('#movieDetailsModal').on('hidden.bs.modal', function () {
            $(this).remove();
        });
    });
    return false;
};


