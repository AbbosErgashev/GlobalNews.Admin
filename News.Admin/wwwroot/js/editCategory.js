//$(document).ready(function () {
//    // Auto-focus the name field on page load
//    $('#Name').focus();

//    // Add confirmation for potentially destructive changes
//    $('form').submit(function () {
//        const originalValue = '@Model.Name';
//        const currentValue = $('#Name').val();

//        if (originalValue.toLowerCase() !== currentValue.toLowerCase()) {
//            return confirm('Are you sure you want to change the category name from "' +
//                originalValue + '" to "' + currentValue + '"?');
//        }
//        return true;
//    });
//});